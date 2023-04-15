using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public bool soundOn = true;

    private AudioSource bgmAudioSource;

    [Serializable]
    public enum AudioSourceType
    {
        Bgm,
        ButtonClick,
        Explosion,
        TurretUISelected,
        TurretUIError,
        TurretBuild,
        LoseGame,
        WinGame
    }

    private void Awake()
    {
        if(!Instance)
            Instance = this;

    }

    public void PlayBGM()
    {
        if (transform.GetComponents<AudioSource>()[(int)AudioSourceType.Bgm])
        {
            bgmAudioSource = transform.GetComponents<AudioSource>()[(int)AudioSourceType.Bgm];
            bgmAudioSource.Play();
        }
    }

    public void StopBGM()
    {
        bgmAudioSource.Stop();
    }

    public void EnableSound(bool enable)
    {
        AudioListener.pause = !enable;
        soundOn = enable;
    }

    public void PlaySound(AudioSourceType audioSourceType)
    {
        if (transform.GetComponents<AudioSource>()[(int)audioSourceType] && soundOn)
        {
            AudioSource audioSource = transform.GetComponents<AudioSource>()[(int)audioSourceType];
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

}
