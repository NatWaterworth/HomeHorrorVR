using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ObjectSoundController : MonoBehaviour
{
    [SerializeField] private AudioClip[] interactSounds;
    [SerializeField] private AudioClip[] activateSounds;
    [SerializeField] private AudioClip[] deactivateSounds;
    [SerializeField] private AudioClip[] grabSounds;
    [SerializeField] private AudioClip[] releaseSounds;

    private AudioSource audioSource;

    private void Awake()
    {
        // Initialize the audioSource with the AudioSource component attached to the same GameObject
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on this GameObject.");
        }
    }

    // Public method to play a random interact sound
    public void PlayInteractSound()
    {
        PlayRandomSound(interactSounds);
    }

    // Public method to play a random activate sound
    public void PlayActivateSound()
    {
        PlayRandomSound(activateSounds);
    }

    // Public method to play a random deactivate sound
    public void PlayDeactivateSound()
    {
        PlayRandomSound(deactivateSounds);
    }

    // Public method to play a random grab sound
    public void PlayGrabSound()
    {
        PlayRandomSound(grabSounds);
    }

    // Public method to play a random release sound
    public void PlayReleaseSound()
    {
        PlayRandomSound(releaseSounds);
    }

    // Private method to play a random sound from an array of AudioClips
    private void PlayRandomSound(AudioClip[] clips)
    {
        if (audioSource != null && clips != null && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            AudioClip clip = clips[randomIndex];
            audioSource.PlayOneShot(clip);
        }
    }
}
