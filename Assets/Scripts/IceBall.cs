using UnityEngine;

public class IceBall : MonoBehaviour
{
    public AudioClip iceBallBreak;
    void OnCollisionEnter(Collision collision)
    {
        PlayBreakSound();
        Destroy(gameObject);
    }

    private void PlayBreakSound()
    {
        GameObject soundObject = new GameObject("BreakSound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = iceBallBreak;
        audioSource.Play();

        // Auto-destroy the sound object after the clip finishes
        Destroy(soundObject, iceBallBreak.length);
    }
}