using UnityEngine;


public class MusicPlayer : MonoBehaviour
{
    public AudioSource musicSource;

    void Start()
    {
        AudioController.Instance?.RegisterMusic(musicSource);
    }
}