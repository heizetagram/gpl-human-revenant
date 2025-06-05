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
    
    private Vector3 _originalCameraPosition;
    private Vector3 _targetPosition;
    private bool _triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_triggered || !other.CompareTag("Player")) return;
        _triggered = true;

        _originalCameraPosition = Camera.main.transform.position;

        Time.timeScale = 0;

        StartCoroutine(MoveCameraSequence());
    }

    private IEnumerator MoveCameraSequence()
    {
        _targetPosition = new Vector3(bossCameraPosition.position.x, bossCameraPosition.position.y,
            _originalCameraPosition.z);
        yield return StartCoroutine(MoveCameraSmooth(_targetPosition));

        GameObject smoke = Instantiate(smokeEffectPrefab, bossSpawnPoint.position, Quaternion.identity);
        ParticleSystem ps = smoke.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Simulate(0f, true, true);
            ps.Play(true);
        }


        yield return new WaitForSecondsRealtime(1f);

        GameObject boss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);

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

        Destroy(smoke, 2f);

        yield return new WaitForSecondsRealtime(1f);

        _targetPosition = new Vector3(player.position.x, player.position.y, _originalCameraPosition.z);
        yield return StartCoroutine(MoveCameraSmooth(_targetPosition));

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

        Camera.main.transform.position = new Vector3(target.x, target.y, Camera.main.transform.position.z);
    }
}
