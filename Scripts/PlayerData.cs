﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Use getter and setter syntax
public class PlayerData : MonoBehaviour {
    // ===== Keys - Settings =====
    const string GAME_VOLUME = "Game volume";
    const string MUSIC_VOLUME = "Music volume";
    // ===== Keys - Player Progress =====
    const string CREDITS = "Credits";      // Persistent currency

    // ===== Limits and default values =====
    const float MAX_VOLUME = 1f;
    const float MIN_VOLUME = 0f;
    const float DEFAULT_VOLUME = 0.4f;
    const int DEFAULT_CREDITS = 100;

    public static PlayerData uniqueDataManager;

    // TODO: Default values not working
    void Awake() {
        if (uniqueDataManager != null) {
            if (uniqueDataManager != this) {
                Destroy(gameObject);
            }
        } else {
            uniqueDataManager = this;
            DontDestroyOnLoad(this.gameObject);
        }

        if (!PlayerPrefs.HasKey(GAME_VOLUME)) {
            PlayerPrefs.SetFloat(GAME_VOLUME, DEFAULT_VOLUME);
        }
        if (!PlayerPrefs.HasKey(MUSIC_VOLUME)) {
            PlayerPrefs.SetFloat(MUSIC_VOLUME, DEFAULT_VOLUME);
        }
        if (!PlayerPrefs.HasKey(CREDITS)) {
            PlayerPrefs.SetInt(CREDITS, DEFAULT_CREDITS);
        }
        if (!PlayerPrefs.HasKey("Level1_1")) {
            LockAllLevels();
        }
    }

    public static float GetGameVolume() {
        return PlayerPrefs.GetFloat(GAME_VOLUME);
    }

    public static void SetGameVolume(float newVolume) {
        if (newVolume > MAX_VOLUME) {
            newVolume = MAX_VOLUME;
        } else if (newVolume < MIN_VOLUME) {
            newVolume = MIN_VOLUME;
        }
        PlayerPrefs.SetFloat(GAME_VOLUME, newVolume);
    }

    public static float GetMusicVolume() {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME);
    }

    public static void SetMusicVolume(float newVolume) {
        if (newVolume > MAX_VOLUME) {
            newVolume = MAX_VOLUME;
        } else if (newVolume < MIN_VOLUME) {
            newVolume = MIN_VOLUME;
        }
        PlayerPrefs.SetFloat(MUSIC_VOLUME, newVolume);
    }

    public static int GetCredits() {
        return PlayerPrefs.GetInt(CREDITS);
    }

    public static void SetCredits(int amount) {
        PlayerPrefs.SetInt(CREDITS, amount);
    }

    public static bool LevelIsUnlocked(int zone, int level) {
        string levelName = "Level" + zone.ToString() + "_" + level.ToString();
        if (PlayerPrefs.GetInt(levelName) == 1) {
            return true;
        }
        return false;   
    }

    public static void UnlockLevel(string level) {
        PlayerPrefs.SetInt(level, 1);
    }

    // Lock all levels apart from the first
    private void LockAllLevels() {
        for (int zone = 1; zone <= 5; zone++) {
            for (int level = 1; level <= 10; level++) {
                string levelName = "Level" + zone.ToString() + "_" + level.ToString();
                PlayerPrefs.SetInt(levelName, 0);
                // Debug.Log("Locking: " + levelName);
            }
        }
        UnlockLevel("Level1_1");
    }

    public static int GetShipTier(string shipFamily) {
        if (PlayerPrefs.HasKey(shipFamily)) {
            return PlayerPrefs.GetInt(shipFamily);
        } else {
            return 1;
        }
    }

    public void UpgradeShip(string shipFamily) {
        if (PlayerPrefs.HasKey(shipFamily)) {
            if (PlayerPrefs.GetInt(shipFamily) <= 2) {  // Max upgrade number is 3
                PlayerPrefs.SetInt(shipFamily, PlayerPrefs.GetInt(shipFamily) + 1);
                Debug.Log(shipFamily + " is now tier " + PlayerPrefs.GetInt(shipFamily));
            } else {
                Debug.Log("NO FURTHER UPGRADeS");
            }
        } else {
            PlayerPrefs.SetInt(shipFamily, 2);
            Debug.Log(shipFamily + " is now tier " + PlayerPrefs.GetInt(shipFamily));
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            Debug.Log("Resetting all prefs");
            PlayerPrefs.DeleteAll();
        }
    }
}
