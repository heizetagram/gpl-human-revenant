using UnityEngine;
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
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        float fillAmount = (float)currentHealth / maxHealth;
        healthFillImage.fillAmount = fillAmount;
    }
    void Update()
    {
    if (Input.GetKeyDown(KeyCode.H))
        TakeDamage(10);
    }
}
