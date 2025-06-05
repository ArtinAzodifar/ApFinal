using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class NamedAudioClip
    {
        public string name;
        public AudioClip clip;
    }

    public AudioSource audioSource;
    public List<NamedAudioClip> soundClips;

    private Dictionary<string, AudioClip> soundMap;

    void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // Build the lookup dictionary
        soundMap = new Dictionary<string, AudioClip>();
        foreach (var item in soundClips)
        {
            if (!soundMap.ContainsKey(item.name))
                soundMap.Add(item.name, item.clip);
        }
    }

    // This method can be called from Animation Events
    public void PlaySound(string soundName)
    {
        if (soundMap.TryGetValue(soundName, out AudioClip clip))
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Sound not found: " + soundName);
        }
    }
}