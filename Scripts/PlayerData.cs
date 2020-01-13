using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {
    // Keys:
    const string GAME_VOLUME = "Game volume";
    const string MUSIC_VOLUME = "Music volume";
    // Limits:
    const float MAX_VOLUME = 1f;
    const float MIN_VOLUME = 0f;

    public static void SetGameVolume(float newVolume) {
        if (newVolume > MAX_VOLUME) {
            newVolume = MAX_VOLUME;
        } else if (newVolume < MIN_VOLUME) {
            newVolume = MIN_VOLUME;
        }
        PlayerPrefs.SetFloat(GAME_VOLUME, newVolume);
    }

    public static float GetGameVolume() {
        return PlayerPrefs.GetFloat(GAME_VOLUME);
    }

    public static void SetMusicVolume(float newVolume) {
        if (newVolume > MAX_VOLUME) {
            newVolume = MAX_VOLUME;
        } else if (newVolume < MIN_VOLUME) {
            newVolume = MIN_VOLUME;
        }
        PlayerPrefs.SetFloat(MUSIC_VOLUME, newVolume);
    }

    public static float GetMusicVolume() {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME);
    }
}
