using UnityEngine;

public class ObjectSoundController : MonoBehaviour
{
    [SerializeField][Tooltip("Select/Grab Object Sounds")] private AudioClip[] _selectSounds;
    [SerializeField][Tooltip("Deselect/Release Object Sounds")] private AudioClip[] _deselectSounds;
    [SerializeField][Tooltip("Activate Object Sounds")] private AudioClip[] _activateSounds;
    [SerializeField][Tooltip("Deactivate Object Sounds")] private AudioClip[] _deactivateSounds;
    [SerializeField][Tooltip("Moving Object Sounds")] private AudioClip[] _motionSounds;
    [SerializeField][Tooltip("Object Collision Sounds")] private AudioClip[] _impactSounds;

    [SerializeField] private float _minMoveSoundPlaybackSpeed = 0.5f;
    [SerializeField] private float _maxMoveSoundPlaybackSpeed = 2.0f;

    private AudioSource _currentLoopingAudioSource;

    private void Awake()
    {
        if (SoundManager.Instance == null)
        {
            Debug.LogError("SoundManager instance not found in the scene.");
        }
    }

    // Public method to play a random move sound
    public void PlayMoveSound()
    {
        _currentLoopingAudioSource = SoundManager.Instance.PlayRandomLoopingSound(_motionSounds, transform);
    }

    // Public method to stop the move sound
    public void StopSound()
    {
        if (_currentLoopingAudioSource != null)
        {
            SoundManager.Instance.StopSound(_currentLoopingAudioSource);
            _currentLoopingAudioSource = null;
        }
    }

    // Public method to update the move sound playback speed
    public void UpdateMoveSoundSpeed(float interpolationValue)
    {
        if (_currentLoopingAudioSource != null)
        {
            float clampedValue = Mathf.Clamp01(interpolationValue);
            float playbackSpeed = Mathf.Lerp(_minMoveSoundPlaybackSpeed, _maxMoveSoundPlaybackSpeed, clampedValue);
            _currentLoopingAudioSource.pitch = playbackSpeed;
        }
    }

    // Public method to play a random interact sound
    public void PlaySelectSound()
    {
        SoundManager.Instance.PlayRandomSound(_selectSounds, transform);
    }

    // Public method to play a random interact sound
    public void PlayDeselectSound()
    {
        SoundManager.Instance.PlayRandomSound(_deselectSounds, transform);
    }

    // Public method to play a random activate sound
    public void PlayActivateSound()
    {
        SoundManager.Instance.PlayRandomSound(_activateSounds, transform);
    }

    // Public method to play a random deactivate sound
    public void PlayDeactivateSound()
    {
        SoundManager.Instance.PlayRandomSound(_deactivateSounds, transform);
    }
}
