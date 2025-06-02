using UnityEngine;

public class Laser : MonoBehaviour
{
    public int damageAmount = 50;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController health = other.GetComponent<PlayerController>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
                Debug.Log("HSV");
            }
        }
    }
}
