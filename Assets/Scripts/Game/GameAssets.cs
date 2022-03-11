using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;
    public static GameAssets GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    public Sprite pipeHeadSprite;
    public Transform pfPipeHead;
    public Transform pfPipeBody;
    public Transform pfGround;
    public Transform pfCloud1;
    public Transform pfCloud2;
    public Transform pfCloud3;
    public SoundAudioClips[] soundAudioClipsArray;
    [Serializable]
    public class SoundAudioClips
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;

    }

    public Transform GetCloudPrefab(int number)
    {
        switch (number)
        {
            case 1:
                return pfCloud1;
            case 2:
                return pfCloud2;
            case 3:
                return pfCloud3;
            default:
                return null;
        }
    }
}
