using System;
using System.Collections;
using UnityEngine;

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
        WinGame,
        BlizzardSE,
        LightningStrikeSE,
        MagneticBoltSE
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
        AudioListener.volume = enable ? 1 : 0;
        soundOn = enable;
    }

    public void PlaySound(AudioSourceType audioSourceType)
    {
        if (transform.GetComponents<AudioSource>()[(int)audioSourceType])
        {
            AudioSource audioSource = transform.GetComponents<AudioSource>()[(int)audioSourceType];
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    public AudioSource PlayLoopSound(AudioSourceType audioSourceType)
    {
        if (transform.GetComponents<AudioSource>()[(int)audioSourceType])
        {
            AudioSource audioSource = transform.GetComponents<AudioSource>()[(int)audioSourceType];
            audioSource.loop = true;
            audioSource.Play();
            return audioSource;
        }

        return new AudioSource();
    }

}
