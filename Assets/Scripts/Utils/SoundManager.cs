using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public static class SoundManager
{
    public enum Sound
    {
        Jump,
        Click,
        Dead,
        Coin
    }
    public static void PlaySound(Sound sound)
    {
        GameObject gameObject = new GameObject("Sound", typeof(AudioSource));
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = .01f;
        audioSource.PlayOneShot(GetAudioClip(sound));

    }

    public static AudioClip GetAudioClip(SoundManager.Sound sound)
    {
        foreach (GameAssets.SoundAudioClips audioClips in GameAssets.GetInstance().soundAudioClipsArray)
        {
            if (audioClips.sound == sound)
            {
                return audioClips.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " Not FOUND");
        return null;
    }

    
}
