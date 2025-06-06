using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    public Slider sfxSlider;
    public Slider musicSlider;

    public Toggle sfxMuteToggle;
    public Toggle musicMuteToggle;

    public Image sfxMuteIcon;
    public Image musicMuteIcon;

    public Sprite muteSprite;
    public Sprite unmuteSprite;

    void Start()
    {
        sfxSlider.value = AudioController.Instance.sfxVolume * 100;
        musicSlider.value = AudioController.Instance.musicVolume * 100;

        if (sfxMuteToggle != null)
        {
            sfxMuteToggle.isOn = AudioController.Instance.isSFXMuted;
            UpdateMuteIcon(sfxMuteIcon, sfxMuteToggle.isOn);
            sfxSlider.value = sfxMuteToggle.isOn ? 0 : AudioController.Instance.sfxVolume * 100;
        }

        if (musicMuteToggle != null)
        {
            musicMuteToggle.isOn = AudioController.Instance.isMusicMuted;
            UpdateMuteIcon(musicMuteIcon, musicMuteToggle.isOn);
            musicSlider.value = musicMuteToggle.isOn ? 0 : AudioController.Instance.musicVolume * 100;
        }
    }

    public void OnSFXSliderChanged(float val)
    {
        AudioController.Instance.SetSFXVolume(val / 100f);
        if (val > 0 && sfxMuteToggle.isOn)
        {
            sfxMuteToggle.isOn = false;
        }
    }

    public void OnMusicSliderChanged(float val)
    {
        AudioController.Instance.SetMusicVolume(val / 100f);
        if (val > 0 && musicMuteToggle.isOn)
        {
            musicMuteToggle.isOn = false;
        }
    }

    public void OnSFXMuteToggleChanged(bool isMuted)
    {
        AudioController.Instance.ToggleSFXMute(isMuted);
        UpdateMuteIcon(sfxMuteIcon, isMuted);
        sfxSlider.value = isMuted ? 0 : AudioController.Instance.sfxVolume * 100;
    }

    public void OnMusicMuteToggleChanged(bool isMuted)
    {
        AudioController.Instance.ToggleMusicMute(isMuted);
        UpdateMuteIcon(musicMuteIcon, isMuted);
        musicSlider.value = isMuted ? 0 : AudioController.Instance.musicVolume * 100;
    }

    private void UpdateMuteIcon(Image iconImage, bool isMuted)
    {
        if (iconImage != null)
        {
            iconImage.sprite = isMuted ? muteSprite : unmuteSprite;
        }
    }
}
