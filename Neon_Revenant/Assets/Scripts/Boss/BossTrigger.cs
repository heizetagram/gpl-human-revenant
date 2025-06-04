using UnityEngine;
using System.Collections;

public class BossTrigger : MonoBehaviour
{
    public GameObject bossPrefab;
    public GameObject smokeEffectPrefab;
    public Transform bossSpawnPoint;
    public Transform bossCameraPosition;
    public Transform player;
    public float cameraSpeed = 2f;
    public GameObject gewinnUI;
    
    private Vector3 originalCameraPosition;
    private Vector3 targetPosition;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Player")) return;
        triggered = true;

        // Kamera-Startposition speichern
        originalCameraPosition = Camera.main.transform.position;

        // Spiel pausieren
        Time.timeScale = 0;

        // Kamera zum Boss bewegen (per Coroutine mit unscaled time)
        StartCoroutine(MoveCameraSequence());
    }

    private IEnumerator MoveCameraSequence()
    {
        // Phase 1: Kamera fährt zum Boss
        targetPosition = new Vector3(bossCameraPosition.position.x, bossCameraPosition.position.y,
            originalCameraPosition.z);
        yield return StartCoroutine(MoveCameraSmooth(targetPosition));

        GameObject smoke = Instantiate(smokeEffectPrefab, bossSpawnPoint.position, Quaternion.identity);
        ParticleSystem ps = smoke.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Simulate(0f, true, true);  // sofort vorbereiten
            ps.Play(true);                // sicherstellen, dass es mit unscaled Time läuft
        }


        // Kurze Pause damit Rauch sichtbar wird
        yield return new WaitForSecondsRealtime(1f);

        // Boss spawnen
        GameObject boss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);

        // Spieler übergeben
        Boss bossScript = boss.GetComponent<Boss>();
        BossHealth healthScript = bossScript.GetComponent<BossHealth>();
        if (bossScript != null)
        {
            bossScript.player = player;
        }
        
        if (healthScript != null)
        {
            healthScript.gewinnUI = gewinnUI;
        }

        // Rauch nach kurzer Zeit entfernen
        Destroy(smoke, 2f);

        // Kurze Pause bevor Kamera zurückfährt
        yield return new WaitForSecondsRealtime(1f);

        // Phase 2: Kamera zurück zum Spieler
        targetPosition = new Vector3(player.position.x, player.position.y, originalCameraPosition.z);
        yield return StartCoroutine(MoveCameraSmooth(targetPosition));

        // Spiel weiterlaufen lassen
        Time.timeScale = 1;
    }

    private IEnumerator MoveCameraSmooth(Vector3 target)
    {
        while (Vector2.Distance(Camera.main.transform.position, target) > 0.05f)
        {
            Vector3 current = Camera.main.transform.position;
            Vector3 next = Vector3.Lerp(current, target, Time.unscaledDeltaTime * cameraSpeed);
            Camera.main.transform.position = new Vector3(next.x, next.y, current.z);
            yield return null;
        }

        // Endposition direkt setzen
        Camera.main.transform.position = new Vector3(target.x, target.y, Camera.main.transform.position.z);
    }
}
