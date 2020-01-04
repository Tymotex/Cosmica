using UnityEngine.Audio;
using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {
    public static MusicPlayer uniqueAudioManager;
    public SoundClip[] music;
    [SerializeField]
    float fadeTime = 5f;

    void Awake() {
        if (uniqueAudioManager != null) {
            if (uniqueAudioManager != this) {
                Destroy(gameObject);
            }
        } else {
            uniqueAudioManager = this;
            DontDestroyOnLoad(this.gameObject);
            StartCoroutine(PlayBGM());  // This should only be called ONCE
        }
    }

    public void PlayMusic(string clipName, float initialVolume) {
        // Find the clip to play
        foreach(SoundClip musicClip in music) {
            if (musicClip.clipName == clipName) {
                AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                audioSource.clip = musicClip.clip;
                audioSource.volume = initialVolume;
                audioSource.pitch = musicClip.pitch;
                audioSource.Play();
            }
        }
    }
    
    private float PlayRandomBGM(float initialVolume) {
        int randomIndex = Random.Range(0, music.Length);
        PlayMusic(music[randomIndex].clipName, initialVolume);  // TODO: A bit inefficient. Why not write code to play it here?
        Debug.Log("===> Playing music track: " + randomIndex.ToString() + ", " + music[randomIndex].clipName + ", " + music[randomIndex].clip.length + " seconds");
        return music[randomIndex].clip.length;
    }
    
    private IEnumerator PlayBGM() {
        while (true) {
            float trackDuration = PlayRandomBGM(0);
            StartCoroutine(FadeAudioSource.StartFade(gameObject.GetComponent<AudioSource>(), fadeTime, 1));
            yield return new WaitForSeconds(fadeTime);  // Wait for fade in to complete
            yield return new WaitForSeconds(trackDuration - 2 * fadeTime);  // Wait until when we should fade out
            StartCoroutine(FadeAudioSource.StartFade(gameObject.GetComponent<AudioSource>(), fadeTime, 0));
            yield return new WaitForSeconds(fadeTime);  // Wait for fade out to complete
        }
    }
} 
