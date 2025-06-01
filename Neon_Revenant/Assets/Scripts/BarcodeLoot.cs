using UnityEngine;

public class BarcodeLoot : MonoBehaviour
{
    public int value = 1;

    void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.CompareTag("Player"))
        {
            Debug.Log("Barcode eingesammelt! +" + value);
            Destroy(gameObject);
        }
    }
}

