using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    [SerializeField] float minImpactVolume = 0.1f;
    [SerializeField] float maxImpactVolume = 1;
    [SerializeField] float maxImpactVelocity = 5;

    public static SoundManager Instance;

    [System.Serializable]
    public class MaterialAudioClip
    {
        public MaterialType materialType;
        public List<AudioClip> audioClips;
    }

    public List<MaterialAudioClip> materialAudioClips;

    private Dictionary<MaterialType, List<AudioClip>> audioClipDictionary;
    private Queue<AudioSource> audioSourcePool;
    public int poolSize = 10; // Adjust based on your needs
    public GameObject audioSourcePrefab;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize the dictionary
        audioClipDictionary = new Dictionary<MaterialType, List<AudioClip>>();
        foreach (var item in materialAudioClips)
        {
            audioClipDictionary[item.materialType] = item.audioClips;
        }

        // Initialize the pool
        audioSourcePool = new Queue<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewAudioSource();
        }
    }

    private void CreateNewAudioSource()
    {
        GameObject newAudioSourceObject = Instantiate(audioSourcePrefab);
        newAudioSourceObject.transform.parent = transform;
        AudioSource newAudioSource = newAudioSourceObject.GetComponent<AudioSource>();
        newAudioSourceObject.SetActive(false);
        audioSourcePool.Enqueue(newAudioSource);
    }

    public void PlayImpactSound(MaterialType materialType, Vector3 position, float impactMagnitude)
    {
        if (!audioClipDictionary.ContainsKey(materialType))
        {
            Debug.LogWarning("No audio clips found for material type: " + materialType);
            return;
        }

        if (audioSourcePool.Count > 0)
        {
            AudioSource audioSource = audioSourcePool.Dequeue();
            audioSource.transform.position = position;

            // Select a random audio clip from the list
            List<AudioClip> clips = audioClipDictionary[materialType];
            AudioClip clipToPlay = clips[Random.Range(0, clips.Count)];

            audioSource.clip = clipToPlay;
            audioSource.volume = GetVolumeFromImpactMagnitude(impactMagnitude);
            audioSource.gameObject.SetActive(true);
            audioSource.Play();
            StartCoroutine(ReturnAudioSourceToPool(audioSource, clipToPlay.length));
        }
        else
        {
            Debug.LogWarning("No available audio sources in the pool");
        }
    }

    private float GetVolumeFromImpactMagnitude(float impactMagnitude)
    {
        float lerp = Mathf.InverseLerp(0, maxImpactVelocity, impactMagnitude);
        return Mathf.Lerp(minImpactVolume, maxImpactVolume, lerp);
    }

    private IEnumerator ReturnAudioSourceToPool(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.Stop();
        audioSource.gameObject.SetActive(false);
        audioSourcePool.Enqueue(audioSource);
    }
}
