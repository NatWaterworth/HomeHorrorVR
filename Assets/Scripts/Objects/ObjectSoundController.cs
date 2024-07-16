using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ObjectSoundController : MonoBehaviour
{
    [SerializeField] [Tooltip("Select/Grab Object Sounds")] private AudioClip[] _selectSounds;
    [SerializeField] [Tooltip("Deselect/Release Object Sounds")] private AudioClip[] _deselectSounds;
    [SerializeField] [Tooltip("Activate Object Sounds")] private AudioClip[] _activateSounds;
    [SerializeField] [Tooltip("Deactivate Object Sounds")] private AudioClip[] _deactivateSounds;
    [SerializeField] [Tooltip("Moving Object Sounds")] private AudioClip[] _motionSounds;
    [SerializeField] [Tooltip("Object Collision Sounds")] private AudioClip[] _impactSounds;

    [SerializeField] private float _minMoveSoundPlaybackSpeed = 0.5f;
    [SerializeField] private float _maxMoveSoundPlaybackSpeed = 2.0f;

    private AudioSource _audioSource;

    private void Awake()
    {
        // Initialize the audioSource with the AudioSource component attached to the same GameObject
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource component not found on this GameObject.");
        }
    }

    // Public method to play a random move sound
    public void PlayMoveSound()
    {
        PlayRandomLoopingSound(_motionSounds);
    }

    // Public method to stop the move sound
    public void StopSound()
    {
        _audioSource.Stop();
    }

    // Public method to update the move sound playback speed
    public void UpdateMoveSoundSpeed(float interpolationValue)
    {
        float clampedValue = Mathf.Clamp01(interpolationValue);
        float playbackSpeed = Mathf.Lerp(_minMoveSoundPlaybackSpeed, _maxMoveSoundPlaybackSpeed, clampedValue);
        _audioSource.pitch = playbackSpeed;
    }

    // Public method to play a random interact sound
    public void PlaySelectSound()
    {
        PlayRandomSound(_selectSounds);
    }

    // Public method to play a random interact sound
    public void PlayDeselectSound()
    {
        PlayRandomSound(_deselectSounds);
    }

    // Public method to play a random activate sound
    public void PlayActivateSound()
    {
        PlayRandomSound(_activateSounds);
    }

    // Public method to play a random deactivate sound
    public void PlayDeactivateSound()
    {
        PlayRandomSound(_deactivateSounds);
    }

    // Private method to play a random sound from an array of AudioClips
    private void PlayRandomSound(AudioClip[] clips)
    {
        if (_audioSource != null && clips != null && clips.Length > 0)
        {
            _audioSource.pitch = 1;
            int randomIndex = Random.Range(0, clips.Length);
            AudioClip clip = clips[randomIndex];
            _audioSource.PlayOneShot(clip);
        }
    }

    private void PlayRandomLoopingSound(AudioClip[] clips)
    {
        if (_audioSource != null && clips != null && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            AudioClip clip = clips[randomIndex];
            _audioSource.clip = clip;
            _audioSource.loop = true;
            _audioSource.Play();
        }
    }
}
