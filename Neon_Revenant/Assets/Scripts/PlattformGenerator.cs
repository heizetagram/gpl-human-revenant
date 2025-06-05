using UnityEngine;

public class PlattformGenerator : MonoBehaviour
{
    public GameObject platformPrefab;
    public Transform player;
    public float distanceBetween = 5f;
    public float platformMinY = -1f;
    public float platformMaxY = 2f;

    private float _nextPlatformX = 0f;

    void Start()
    {
        _nextPlatformX = player.position.x;
    }

    void Update()
    {
        if (player.position.x + 15f > _nextPlatformX) // nur vor dem Spieler generieren
        {
            float newY = Random.Range(platformMinY, platformMaxY);
            Vector3 spawnPos = new Vector3(_nextPlatformX, newY, 0f);
            Instantiate(platformPrefab, spawnPos, Quaternion.identity);
            _nextPlatformX += distanceBetween;
        }
    }
}

