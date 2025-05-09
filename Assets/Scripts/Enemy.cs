using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Enemy : MonoBehaviour
{
    [Header("References")]
    public Transform player;           
    public string roomID;              // Room this enemy belongs to

    [Header("Movement")]
    public float moveSpeed = 3f;       // Units per second

    [Header("Combat")]
    public int   maxHealth      = 3;   // How many hits they can take
    public int   damagePerHit   = 1;   // Damage from each IceBall
    private int     _currentHealth;
    private TrackPlayerRoom playerTracker;
    public AudioClip iceBallBreak;
    public static event System.Action<Enemy> OnEnemyDied;

    void Awake()
    {
        _currentHealth = maxHealth;
    }

    void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player")?.transform;

        if (player != null)
            playerTracker = player.GetComponent<TrackPlayerRoom>();
        else
            Debug.LogError("Enemy: No player found!");
    }

    void Update()
    {
        // Safety checks
        if (player == null || playerTracker == null) 
            return;

        // Only chase when the player is in the same room
        if (playerTracker.currentRoom == roomID)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // keep movement horizontal
            transform.position += direction * moveSpeed * Time.deltaTime;
            if (direction.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(direction);
        }

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damagePerHit){
        _currentHealth -= damagePerHit;
        Debug.LogError("Hit! Health: " + _currentHealth);
        PlayHitSound();
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Enemy hit by {other.name} (tag={other.tag})");
        if (other.CompareTag("IceBall")){
            TakeDamage(damagePerHit);
            Destroy(other.gameObject);
        }
    }
    private void PlayHitSound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && iceBallBreak != null)
            audioSource.PlayOneShot(iceBallBreak);
    }

    private void Die()
    {
        PlayHitSound();
        OnEnemyDied?.Invoke(this);
        Destroy(gameObject);
    }
}
