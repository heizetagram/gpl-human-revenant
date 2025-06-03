using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelGoal : MonoBehaviour
{
    public float wartezeit = 2f;
    public GameObject gewinnUI;
    public int WinAmount = 100;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (gewinnUI != null)
                gewinnUI.SetActive(true);
            
            if (BarcodeManager.Instance != null)
                BarcodeManager.Instance.AddBarcode(WinAmount);

            StartCoroutine(ZuruckZumMenu());
        }
    }

    private System.Collections.IEnumerator ZuruckZumMenu()
    {
        yield return new WaitForSeconds(wartezeit);
        SceneManager.LoadScene("MainMenu");
        Debug.Log("fertig");
    }
}