using UnityEngine.Audio;
using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {
    public static MusicPlayer uniqueAudioManager;
    public SoundClip[] music;
    public static string currentSoundtrack;
    int currentSoundtrackIndex;
    //[SerializeField]
    //float fadeTime = 5f;

    void Awake() {
        if (uniqueAudioManager != null) {
            if (uniqueAudioManager != this) {
                Destroy(gameObject);
            }
        } else {
            uniqueAudioManager = this;
            DontDestroyOnLoad(this.gameObject);
            int randomIndex = Random.Range(0, music.Length);   // First select a random index on start
            StartCoroutine(PlayBGM(randomIndex));  // This should only be called ONCE
        }
    }

    public void ChangeVolume(float newVolume) {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = newVolume;
    }

    public void PlayMusic(string clipName, float initialVolume) {
        // Find the clip to play
        foreach (SoundClip musicClip in music) {
            if (musicClip.clipName == clipName) {
                AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                audioSource.clip = musicClip.clip;
                audioSource.volume = initialVolume;
                audioSource.pitch = musicClip.pitch;
                audioSource.Play();
                currentSoundtrack = musicClip.clipName;
                Debug.Log("===> Playing: " + clipName + " (" + musicClip.clip.length + ")");
                // Update the track name in options menu
                OptionController optionMenu = FindObjectOfType<OptionController>();
                if (optionMenu != null) {
                    optionMenu.UpdateTrackName(currentSoundtrack);
                }
            }
        }
    }

    /*
    private IEnumerator PlayBGM() {
        int randomIndex = Random.Range(0, music.Length);   // First select a random index on start
        PlayMusic(music[randomIndex].clipName, PlayerData.GetMusicVolume());
        currentSoundtrackIndex = randomIndex;
        yield return new WaitForSeconds(music[randomIndex].clip.length);
        int nextTrackIndex = randomIndex + 1;
        while (true) {
            if (nextTrackIndex >= music.Length) {  // Wrap around to the beginning of the soundtrack array
                Debug.Log("WRAPPING AROUND TO FIRST SOUNDTRACK!");
                nextTrackIndex = 0;
            }
            PlayMusic(music[nextTrackIndex].clipName, PlayerData.GetMusicVolume());
            currentSoundtrackIndex = nextTrackIndex;
            yield return new WaitForSeconds(music[nextTrackIndex].clip.length);
            nextTrackIndex++;
        }
    }
    */

    private IEnumerator PlayBGM(int startingIndex) {
        PlayMusic(music[startingIndex].clipName, PlayerData.GetMusicVolume());
        currentSoundtrackIndex = startingIndex;
        yield return new WaitForSeconds(music[startingIndex].clip.length);
        int nextTrackIndex = startingIndex + 1;
        while (true) {
            if (nextTrackIndex >= music.Length) {  // Wrap around to the beginning of the soundtrack array
                Debug.Log("===> !!! Wrapping around to first soundtrack!");
                nextTrackIndex = 0;
            }
            PlayMusic(music[nextTrackIndex].clipName, PlayerData.GetMusicVolume());
            currentSoundtrackIndex = nextTrackIndex;
            yield return new WaitForSeconds(music[nextTrackIndex].clip.length);
            nextTrackIndex++;
        }
    }

    public void ForcePlayNext() {
        StopAllCoroutines();
        currentSoundtrackIndex++;
        if (currentSoundtrackIndex >= music.Length) {
            currentSoundtrackIndex = 0;
        }
        StartCoroutine(PlayBGM(currentSoundtrackIndex));
    }
}


/*private float PlayRandomBGM(float initialVolume) {
    int randomIndex = Random.Range(0, music.Length);
    PlayMusic(music[randomIndex].clipName, initialVolume);  // TODO: A bit inefficient. Why not write code to play it here?
    //Debug.Log("===> Playing music track: " + randomIndex.ToString() + ", " + music[randomIndex].clipName + ", " + music[randomIndex].clip.length + " seconds");
    return music[randomIndex].clip.length;
}*/
/*
StartCoroutine(FadeAudioSource.StartFade(gameObject.GetComponent<AudioSource>(), fadeTime, PlayerData.GetMusicVolume()));  // PlayerData.GetMusicVolume() returns a number between 0-1
yield return new WaitForSeconds(fadeTime);  // Wait for fade in to complete
yield return new WaitForSeconds(trackDuration - 2 * fadeTime);  // Wait until when we should fade out
StartCoroutine(FadeAudioSource.StartFade(gameObject.GetComponent<AudioSource>(), fadeTime, 0));
yield return new WaitForSeconds(fadeTime);  // Wait for fade out to complete
*/
