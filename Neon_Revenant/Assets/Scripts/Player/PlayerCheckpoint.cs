using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    private Vector3 _respawnPosition;

    void Start()
    {
        _respawnPosition = transform.position;
    }

    public void SetCheckpoint(Vector3 checkpointPos)
    {
        _respawnPosition = checkpointPos;
    }

    public void Respawn()
    {
        transform.position = _respawnPosition;
        
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

