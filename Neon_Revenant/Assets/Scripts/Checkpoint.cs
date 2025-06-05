using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Sprite inactiveSprite; 
    public Sprite activeSprite;   
    private SpriteRenderer spriteRenderer;
    private bool _isActivated = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = inactiveSprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isActivated) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("HSV");
            PlayerCheckpoint player = other.GetComponent<PlayerCheckpoint>();
            if (player != null)
            {
                player.SetCheckpoint(transform.position);
                ActivateCheckpoint();
            }
        }
    }

    void ActivateCheckpoint()
    {
        _isActivated = true;
        spriteRenderer.sprite = activeSprite;
    }
}