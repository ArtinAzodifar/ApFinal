using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class AudioThemeUI
{
    public string themeName; // e.g., "Green", "Brown", "Gray"
    public Slider sfxSlider;
    public Slider musicSlider;
    public Toggle sfxMuteToggle;
    public Toggle musicMuteToggle;
    public Image sfxMuteIcon;
    public Image musicMuteIcon;
    public Sprite muteSprite;
    public Sprite unmuteSprite;
}
public class AudioSettingsUI : MonoBehaviour
{
    public List<AudioThemeUI> themes = new List<AudioThemeUI>();

    private void Start()
    {
        // Wait until the AudioController is ready before doing anything.
        if (AudioController.Instance == null)
        {
            Debug.LogError("AudioController.Instance not found! AudioSettingsUI cannot initialize.", this.gameObject);
            return;
        }

        // Add listeners for every theme's UI elements.
        foreach (var theme in themes)
        {
            // Skip any empty slots in the list to prevent errors.
            if (theme == null) continue;

            if (theme.musicSlider != null)
                theme.musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
            if (theme.sfxSlider != null)
                theme.sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
            if (theme.sfxMuteToggle != null)
                theme.sfxMuteToggle.onValueChanged.AddListener(OnSfxMuteToggleChanged);
            if (theme.musicMuteToggle != null)
                theme.musicMuteToggle.onValueChanged.AddListener(OnMusicMuteToggleChanged);
        }

        // Set the initial state of ALL UI elements.
        UpdateAllUI();
    }

    // --- GENERIC EVENT HANDLERS ---

    public void OnSfxSliderChanged(float val)
    {
        if (AudioController.Instance == null) return;
        AudioController.Instance.SetSFXVolume(val / 100f);

        if (val > 0 && AudioController.Instance.isSFXMuted)
        {
            AudioController.Instance.ToggleSFXMute(false);
        }
        UpdateAllUI();
    }

    public void OnMusicSliderChanged(float val)
    {
        if (AudioController.Instance == null) return;
        AudioController.Instance.SetMusicVolume(val / 100f);
        if (val > 0 && AudioController.Instance.isMusicMuted)
        {
            AudioController.Instance.ToggleMusicMute(false);
        }
        UpdateAllUI();
    }

    public void OnSfxMuteToggleChanged(bool isMuted)
    {
        if (AudioController.Instance == null) return;
        AudioController.Instance.ToggleSFXMute(isMuted);
        UpdateAllUI();
    }

    public void OnMusicMuteToggleChanged(bool isMuted)
    {
        if (AudioController.Instance == null) return;
        AudioController.Instance.ToggleMusicMute(isMuted);
        UpdateAllUI();
    }

    // --- UI SYNCHRONIZATION ---

    private void UpdateAllUI()
    {
        // Check that the AudioController exists before trying to use it.
        if (AudioController.Instance == null) return;
        
        float sfxVol = AudioController.Instance.sfxVolume;
        float musicVol = AudioController.Instance.musicVolume;
        bool isSfxMuted = AudioController.Instance.isSFXMuted;
        bool isMusicMuted = AudioController.Instance.isMusicMuted;

        foreach (var theme in themes)
        {
            // Skip any empty slots in the list.
            if (theme == null)
            {
                Debug.LogWarning("Found a null theme element in the AudioSettingsUI list. Please check the Inspector.", this.gameObject);
                continue;
            }
            
            // Update SFX UI
            if (theme.sfxSlider != null)
                theme.sfxSlider.SetValueWithoutNotify(isSfxMuted ? 0 : sfxVol * 100);
            if (theme.sfxMuteToggle != null)
                theme.sfxMuteToggle.SetIsOnWithoutNotify(isSfxMuted);
            if (theme.sfxMuteIcon != null && theme.muteSprite != null && theme.unmuteSprite != null)
                theme.sfxMuteIcon.sprite = isSfxMuted ? theme.muteSprite : theme.unmuteSprite;

            // Update Music UI
            if (theme.musicSlider != null)
                theme.musicSlider.SetValueWithoutNotify(isMusicMuted ? 0 : musicVol * 100);
            if (theme.musicMuteToggle != null)
                theme.musicMuteToggle.SetIsOnWithoutNotify(isMusicMuted);
            if (theme.musicMuteIcon != null && theme.muteSprite != null && theme.unmuteSprite != null)
                theme.musicMuteIcon.sprite = isMusicMuted ? theme.muteSprite : theme.unmuteSprite;
        }
    }
}