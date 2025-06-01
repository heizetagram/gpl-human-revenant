using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class HealthBar : MonoBehaviour
{
    private Image _healthBarSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _healthBarSprite = GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        Debug.Log("in der methode!!! :" + currentHealth/ maxHealth);
        _healthBarSprite.fillAmount = currentHealth / maxHealth;
    }
}
