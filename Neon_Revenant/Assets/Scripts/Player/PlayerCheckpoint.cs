using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    private Vector3 respawnPosition;

    void Start()
    {
        respawnPosition = transform.position;
    }

    public void SetCheckpoint(Vector3 checkpointPos)
    {
        respawnPosition = checkpointPos;
    }

    public void Respawn()
    {
        transform.position = respawnPosition;
        
    }
    
    void Update()
    {
        if (transform.position.y < -15f)
        {
            GetComponent<PlayerHealth>().RestoreFullHealth();
            Respawn();
        }
    }
}

