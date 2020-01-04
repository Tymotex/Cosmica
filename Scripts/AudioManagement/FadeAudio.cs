using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FadeAudioSource {

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume) {
        if (targetVolume == 0) {
            Debug.Log("===> Fading out");
        } else if (targetVolume == 1) {
            Debug.Log("===> Fading in");
        }
        
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration) {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        Debug.Log("===> Fading completed!");
        yield break;
    }
}