using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class RoomController : MonoBehaviour
{
    [Tooltip("Tag of enemies to track")]
    public string enemyTag = "IceEnemy";
    public string bossTag = "IceBoss";

    [Tooltip("Tags to destroy when this room is cleared")]
    public string unlockTag;

    // All enemies currently inside this room
    private HashSet<Enemy> _enemies = new HashSet<Enemy>();
    public AudioClip laserUnlock;

    void OnEnable()
    {
        Enemy.OnEnemyDied += HandleEnemyDied;
    }

    void OnDisable()
    {
        Enemy.OnEnemyDied -= HandleEnemyDied;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(enemyTag) && !other.CompareTag(bossTag)) return;

        Enemy e = other.GetComponent<Enemy>();
        if (e != null)
            _enemies.Add(e);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(enemyTag) && !other.CompareTag(bossTag)) return;

        Enemy e = other.GetComponent<Enemy>();
        if (e != null)
            _enemies.Remove(e);
    }

    private void HandleEnemyDied(Enemy e)
    {
        // If this dead enemy was in our room, remove it
        if (_enemies.Remove(e))
        {
            // If that was the last one, clear the room
            if (_enemies.Count == 0)
                UnlockRoom();
        }
    }

    private void UnlockRoom()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && laserUnlock != null)
        {
            audioSource.PlayOneShot(laserUnlock);
        }
        GameObject[] lasers = GameObject.FindGameObjectsWithTag(unlockTag);
        foreach (GameObject laser in lasers)
        {
            Destroy(laser);
        }
        // Optional: disable this script so it won’t run again
        enabled = false;
    }
}