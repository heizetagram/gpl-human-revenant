using UnityEngine;

public class Bullet : MonoBehaviour {
    public float speed = 20f;
    public int damage = 40;
    public Rigidbody2D rb;
    public float timeToLive = 2f;
    public string shooterTag;
    private int _direction = 1;
    private bool directionSet = false;
    private Vector2 _startPosition;
    public AudioClip sound;
    public AudioClip hitmarkerSound;

    public void SetDirection(int dir)
    {
        _direction = dir;
        directionSet = true;

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(_direction * speed, 0f);
        }

        Destroy(gameObject, timeToLive);
    }

    void Start()
    {
        _startPosition = transform.position;
        if (directionSet)
        {
            rb.linearVelocity = new Vector2(_direction * speed, 0f);
        }
        else
        {
            rb.linearVelocity = new Vector2(speed, 0f);
        }

        if (sound != null)
        {
            GameObject soundObj = new GameObject("BulletSound");
            AudioSource audioSource = soundObj.AddComponent<AudioSource>();
            audioSource.clip = sound;
            audioSource.Play();

            Destroy(soundObj, sound.length);
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag(shooterTag)) return;
        IDamageable damageable = hitInfo.GetComponent<IDamageable>();
        if (damageable != null)
        {
            GameObject soundObj = new GameObject("HitmarkerSound");
            AudioSource audioSource = soundObj.AddComponent<AudioSource>();
            audioSource.clip = hitmarkerSound;
            audioSource.Play();

            damageable.TakeDamage(damage);

            float distanceTraveled = Vector2.Distance(_startPosition, transform.position);

            float maxEffectiveRange = 10f;
            float minDamageMultiplier = 0.3f; // Never go below 30% of base damage

            float damageMultiplier = Mathf.Max(minDamageMultiplier, 1 - (distanceTraveled / maxEffectiveRange));
            int finalDamage = Mathf.RoundToInt(damage * damageMultiplier);

            damageable.TakeDamage(finalDamage);
            Destroy(gameObject);
        }
    }
     
    }

