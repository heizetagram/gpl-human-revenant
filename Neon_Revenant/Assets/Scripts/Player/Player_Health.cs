using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 100;
    private int _currentHealth;

    public Image healthFillImage; // Reference to filled (foreground) image

    void Start()
    {
        _currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - amount, 0, maxHealth);
        if (_currentHealth <= 0)
        {
            GetComponent<Animator>().SetTrigger("Death");
            StartCoroutine(RespawnAfterDelay(1f));
            
        }
        GetComponent<Animator>().SetTrigger("Hurt");
        UpdateHealthBar();
    }
    
    public void RestoreFullHealth()
    {
        _currentHealth = maxHealth;
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
        float fillAmount = (float)_currentHealth / maxHealth;
        healthFillImage.fillAmount = fillAmount;
    }
    void Update()
    {

    }
}
