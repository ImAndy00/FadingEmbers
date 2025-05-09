using UnityEngine;

public class Puzzles : MonoBehaviour
{
    private Rigidbody rb;
    private static bool bluePlaced  = false;
    private static bool redPlaced  = false;
    public AudioClip laserUnlock;

    void Awake()
    {
        // cache the Rigidbody
        rb = GetComponent<Rigidbody>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlueSpace"))
        {
            bluePlaced = true;
            if (bluePlaced && redPlaced){
                SolvePuzzle2();
            }

        }
        if (other.CompareTag("RedSpace"))
        {
            redPlaced = true;
            if (bluePlaced && redPlaced){
                SolvePuzzle2();
            }

        }
        AudioSource audioSource = GetComponent<AudioSource>();
        if (other.CompareTag("BigCubeSpace"))
        {
            rb.isKinematic = true;
            if (audioSource != null && laserUnlock != null)
            {
                audioSource.PlayOneShot(laserUnlock);
            }
            GameObject[] lasers = GameObject.FindGameObjectsWithTag("Room3PuzzleLaser");
            foreach (GameObject laser in lasers)
            {
                Destroy(laser);
            }

        }
        if (CompareTag("IceEnemy")) {
            if (other.CompareTag("MonsterSpace"))
            {
                 if (audioSource != null && laserUnlock != null)
                {
                    audioSource.PlayOneShot(laserUnlock);
                }
                GameObject[] lasers = GameObject.FindGameObjectsWithTag("Room9PuzzleLaser");
                foreach (GameObject laser in lasers)
                {
                    Destroy(laser);
                }

            }
        }
    }

    void SolvePuzzle2(){
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && laserUnlock != null)
        {
            audioSource.PlayOneShot(laserUnlock);
        }
        GameObject[] lasers = GameObject.FindGameObjectsWithTag("Room2PuzzleLaser");
        foreach (GameObject laser in lasers)
        {
            Destroy(laser);
        }
    }
}
