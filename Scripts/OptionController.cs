using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour {
    [SerializeField] Text soundtrackText = null;
    [SerializeField] Slider gameVolumeSlider = null;
    [SerializeField] Slider musicVolumeSlider = null;
    

    void Start() {
        // Adds a listener to the slider gameobjects and invokes a method when the value changes. Stolen from Unity docs: https://docs.unity3d.com/2018.3/Documentation/ScriptReference/UI.Slider-onValueChanged.html
        gameVolumeSlider.onValueChanged.AddListener(delegate { UpdateGameVolume(); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { UpdateMusicVolume(); });
        gameVolumeSlider.value = PlayerData.GetGameVolume();
        musicVolumeSlider.value = PlayerData.GetMusicVolume();
        UpdateTrackName(MusicPlayer.currentSoundtrack);
    }

    public void UpdateTrackName(string soundtrackName) {
        soundtrackText.text = soundtrackName;
    }

    // Invoked when the value of the music volume slider is changed
    public void UpdateGameVolume() {
        float sliderValue = gameVolumeSlider.value;
        PlayerData.SetGameVolume(sliderValue);
    }

    // Invoked when the value of the game volume slider is changed
    public void UpdateMusicVolume() {
        float sliderValue = musicVolumeSlider.value;
        PlayerData.SetMusicVolume(sliderValue);
        MusicPlayer musicPlayer = FindObjectOfType<MusicPlayer>();
        if (musicPlayer != null) {
            musicPlayer.ChangeVolume(sliderValue);
        }
    }    

    public void NextTrack() {

    }

    public void PreviousTrack() {

    }
}
