using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] soundClips;          

    public void PlaySound(int index)
    {
        if (audioSource == null || soundClips == null || index >= soundClips.Length) return;

        audioSource.pitch = Random.Range(1f -  0.05f, 1f +  0.05f); // leichte Abwechslung
        audioSource.PlayOneShot(soundClips[index], 1f);
    }
}