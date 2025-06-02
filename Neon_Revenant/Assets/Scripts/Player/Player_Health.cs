using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Image healthFillImage; // Reference to filled (foreground) image

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        if (currentHealth <= 0)
        {
            GetComponent<Animator>().SetTrigger("Death");
            StartCoroutine(RespawnAfterDelay(3f));
            
        }
        
        UpdateHealthBar();
    }
    
    public void RestoreFullHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }
    
    IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        PlayerCheckpoint checkpoint = GetComponent<PlayerCheckpoint>();
        if (checkpoint != null)
        {
            checkpoint.Respawn();
            RestoreFullHealth();
        }

        GetComponent<Animator>().ResetTrigger("Death");
        GetComponent<Animator>().Play("Player_Idle");

    }

    void UpdateHealthBar()
    {
        float fillAmount = (float)currentHealth / maxHealth;
        healthFillImage.fillAmount = fillAmount;
    }
    void Update()
    {

    }
}
