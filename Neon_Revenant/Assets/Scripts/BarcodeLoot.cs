using UnityEngine;

public class BarcodeLoot : MonoBehaviour
{
    public int value = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Barcode eingesammelt! +" + value);

            // Barcode dem Manager hinzufügen
            if (BarcodeManager.Instance != null)
                BarcodeManager.Instance.AddBarcode(value);

            Destroy(gameObject);
        }
    }
}