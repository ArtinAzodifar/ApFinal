using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

[System.Serializable]
public class ThemeColorTarget
{
    public Graphic uiElement;
    public Color themeColor;
}

[System.Serializable]
public struct VideoTheme
{
    public VideoClip clip;
    public List<ThemeColorTarget> colorTargets;
}

[RequireComponent(typeof(VideoPlayer))]
public class RandomizedVideoPlaylist : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer videoPlayer;

    [SerializeField]
    private List<VideoTheme> videoThemes;

    [Header("Fade Transition Settings")]
    [SerializeField]
    private CanvasGroup fadeCanvasGroup;

    [SerializeField]
    private float fadeDuration = 0.5f;

    private Coroutine transitionCoroutine;
    private List<VideoTheme> shuffledPlaylist;
    private int currentVideoIndex = 0;

    void Awake()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = false;
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void Start()
    {
        if (fadeCanvasGroup == null)
        {
            Debug.LogError("Fade Canvas Group is not assigned in the Inspector!");
            return;
        }

        if (videoThemes.Count == 0)
        {
            Debug.LogError("No video themes have been added to the playlist.");
            return;
        }

        fadeCanvasGroup.alpha = 0f;

        HashSet<Graphic> allThemedElements = new HashSet<Graphic>();
        foreach (var theme in videoThemes)
        {
            foreach (var target in theme.colorTargets)
            {
                if (target.uiElement != null)
                {
                    allThemedElements.Add(target.uiElement);
                }
            }
        }

        foreach (var element in allThemedElements)
        {
            Color initialColor = element.color;
            initialColor.a = 0f;
            element.color = initialColor;
        }

        ShuffleAndPlay();
    }

    void ShuffleAndPlay(VideoClip lastClip = null)
    {
        shuffledPlaylist = new List<VideoTheme>(videoThemes);

        for (int i = shuffledPlaylist.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            VideoTheme temp = shuffledPlaylist[i];
            shuffledPlaylist[i] = shuffledPlaylist[randomIndex];
            shuffledPlaylist[randomIndex] = temp;
        }

        if (lastClip != null && shuffledPlaylist.Count > 1 && shuffledPlaylist[0].clip == lastClip)
        {
            VideoTheme firstTheme = shuffledPlaylist[0];
            int lastIndex = shuffledPlaylist.Count - 1;
            shuffledPlaylist[0] = shuffledPlaylist[lastIndex];
            shuffledPlaylist[lastIndex] = firstTheme;
        }

        currentVideoIndex = 0;
        PlayVideoAtIndex(currentVideoIndex);
    }

    void OnVideoFinished(VideoPlayer source)
    {
        currentVideoIndex++;

        if (currentVideoIndex >= shuffledPlaylist.Count)
        {
            ShuffleAndPlay(source.clip);
        }
        else
        {
            PlayVideoAtIndex(currentVideoIndex);
        }
    }

    void PlayVideoAtIndex(int index)
    {
        if (index < 0 || index >= shuffledPlaylist.Count) return;

        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(TransitionToVideo(index));
    }

    private IEnumerator TransitionToVideo(int index)
    {
        yield return StartCoroutine(Fade(1f));

        VideoTheme currentTheme = shuffledPlaylist[index];

        ApplyTheme(currentTheme);

        videoPlayer.clip = currentTheme.clip;
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        videoPlayer.Play();

        yield return StartCoroutine(Fade(0f));

        transitionCoroutine = null;
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
    }

    void ApplyTheme(VideoTheme theme)
    {
        foreach (var target in theme.colorTargets)
        {
            if (target.uiElement != null)
            {
                target.uiElement.color = target.themeColor;
            }
        }
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}