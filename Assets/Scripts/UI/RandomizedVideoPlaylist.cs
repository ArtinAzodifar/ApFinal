using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public struct VideoTheme
{
    public VideoClip clip;
    // Changed from a single GameObject to a List
    public List<GameObject> buttonGroups; 
}

[RequireComponent(typeof(VideoPlayer))]
public class RandomizedVideoPlaylist : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer videoPlayer;

    [SerializeField]
    private List<VideoTheme> videoThemes;

    private List<VideoTheme> shuffledPlaylist;
    private int currentVideoIndex = 0;

    void Awake()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }
        
        videoPlayer.isLooping = false;
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void Start()
    {
        if (videoThemes.Count == 0)
        {
            Debug.LogError("No video themes have been added to the playlist.");
            return;
        }

        // --- CHANGE START ---
        // Loop through each theme, then through each button group in that theme
        foreach (var theme in videoThemes)
        {
            foreach (var group in theme.buttonGroups)
            {
                if (group != null)
                {
                    group.SetActive(false);
                }
            }
        }
        // --- CHANGE END ---

        ShuffleAndPlay();
    }

    void ShuffleAndPlay()
    {
        shuffledPlaylist = new List<VideoTheme>(videoThemes);
        
        // Fisher-Yates shuffle algorithm
        for (int i = shuffledPlaylist.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            VideoTheme temp = shuffledPlaylist[i];
            shuffledPlaylist[i] = shuffledPlaylist[randomIndex];
            shuffledPlaylist[randomIndex] = temp;
        }
        
        currentVideoIndex = 0;
        PlayVideoAtIndex(currentVideoIndex);
    }

    void OnVideoFinished(VideoPlayer source)
    {
        currentVideoIndex++;

        if (currentVideoIndex >= shuffledPlaylist.Count)
        {
            ShuffleAndPlay();
        }
        else
        {
            PlayVideoAtIndex(currentVideoIndex);
        }
    }
    
    void PlayVideoAtIndex(int index)
    {
        if (index < 0 || index >= shuffledPlaylist.Count) return;

        VideoTheme currentTheme = shuffledPlaylist[index];
        
        videoPlayer.clip = currentTheme.clip;
        videoPlayer.Play();
        
        // --- CHANGE START ---
        // Pass the entire list of button groups for the current theme
        UpdateButtonGroups(currentTheme.buttonGroups);
        // --- CHANGE END ---
    }

    // --- ENTIRE METHOD UPDATED ---
    void UpdateButtonGroups(List<GameObject> activeGroups)
    {
        // Loop through all themes defined in the inspector
        foreach (var theme in videoThemes)
        {
            // Loop through each button group associated with that theme
            foreach (var group in theme.buttonGroups)
            {
                if (group != null)
                {
                    // Check if the current group is in the list of groups that should be active
                    bool shouldBeActive = activeGroups.Contains(group);
                    group.SetActive(shouldBeActive);
                }
            }
        }
    }
    // --- METHOD UPDATE END ---


    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}