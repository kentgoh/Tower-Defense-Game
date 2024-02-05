using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public bool soundOn = true;
    public bool soundPaused = false;

    private GameObject standardAudioSource;
    private GameObject mapAudioSource;
    private GameObject spellAudioSource;
    private AudioSource bgmAudioSource;

    public List<AudioSource> loopingOneShotAudioSourceList;

    [Serializable]
    public enum AudioSourceType
    {
        Standard,
        Map,
        Spell
    }

    public enum StandardAudioSource
    {
        Bgm,
        ButtonClick,
        UISelected,
        UIError,
        LoseGame,
        WinGame
    }

    public enum MapAudioSource
    {
        Explosion,
        TurretBuild
    }

    public enum SpellAudioSource
    {
        BlizzardSE,
        LightningStrikeSE,
        MagneticBoltSE
    }

    private void Awake()
    {
        if(!Instance)
            Instance = this;

        if (transform.Find("Standard"))
            standardAudioSource = transform.Find("Standard").gameObject;
        if (transform.Find("Map"))
            mapAudioSource = transform.Find("Map").gameObject;
        if (transform.Find("Spell"))
            spellAudioSource = transform.Find("Spell").gameObject;

    }

    private void Update()
    {
        ControlLoopingOneShotAudioList();
    }

    public void PlayBGM()
    {
        if (standardAudioSource.transform.GetComponents<AudioSource>()[(int)StandardAudioSource.Bgm])
        {
            bgmAudioSource = standardAudioSource.transform.GetComponents<AudioSource>()[(int)StandardAudioSource.Bgm];
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

    public void PlaySound(AudioSourceType audioSourceType, string audioSourceName)
    {
        if(audioSourceType == AudioSourceType.Standard)
        {
            if (standardAudioSource.GetComponents<AudioSource>()[(int)Enum.Parse(typeof(StandardAudioSource), audioSourceName)])
            {
                AudioSource temp = standardAudioSource.GetComponents<AudioSource>()[(int)Enum.Parse(typeof(StandardAudioSource), audioSourceName)];
                temp.PlayOneShot(temp.clip);
            }
            else
            {
                string value = String.Format("{0} not found in {1}", audioSourceName, audioSourceType.ToString());
                Debug.Log(value);
            }

        }
        else if (audioSourceType == AudioSourceType.Map)
        {
            if (mapAudioSource.GetComponents<AudioSource>()[(int)Enum.Parse(typeof(MapAudioSource), audioSourceName)])
            {
                AudioSource temp = mapAudioSource.GetComponents<AudioSource>()[(int)Enum.Parse(typeof(MapAudioSource), audioSourceName)];
                temp.PlayOneShot(temp.clip);
            }
            else
            {
                string value = String.Format("{0} not found in {1}", audioSourceName, audioSourceType.ToString());
                Debug.Log(value);
            }
        }
        else if (audioSourceType == AudioSourceType.Spell)
        {
            if (spellAudioSource.GetComponents<AudioSource>()[(int)Enum.Parse(typeof(SpellAudioSource), audioSourceName)])
            {
                AudioSource temp = spellAudioSource.GetComponents<AudioSource>()[(int)Enum.Parse(typeof(SpellAudioSource), audioSourceName)];
                temp.PlayOneShot(temp.clip);
            }
            else
            {
                string value = String.Format("{0} not found in {1}", audioSourceName, audioSourceType.ToString());
                Debug.Log(value);
            }
        }

    }

    public AudioSource PlayLoopSound(AudioSourceType audioSourceType, string audioSourceName)
    {
        if (audioSourceType == AudioSourceType.Standard)
        {
            if (standardAudioSource.GetComponents<AudioSource>()[(int)Enum.Parse(typeof(StandardAudioSource), audioSourceName)])
            {
                AudioSource temp = standardAudioSource.GetComponents<AudioSource>()[(int)Enum.Parse(typeof(StandardAudioSource), audioSourceName)];
                temp.loop = true;
                temp.Play();

                loopingOneShotAudioSourceList.Add(temp);
                return temp;
            }
            else
            {
                string value = String.Format("{0} not found in {1}", audioSourceName, audioSourceType.ToString());
                Debug.Log(value);
                return new AudioSource();
            }
        }
        else if (audioSourceType == AudioSourceType.Map)
        {
            if (mapAudioSource.GetComponents<AudioSource>()[(int)Enum.Parse(typeof(MapAudioSource), audioSourceName)])
            {
                AudioSource temp = mapAudioSource.GetComponents<AudioSource>()[(int)Enum.Parse(typeof(MapAudioSource), audioSourceName)];
                temp.loop = true;
                temp.Play();

                loopingOneShotAudioSourceList.Add(temp);
                return temp;
            }
            else
            {
                string value = String.Format("{0} not found in {1}", audioSourceName, audioSourceType.ToString());
                Debug.Log(value);
                return new AudioSource();
            }
        }
        else if (audioSourceType == AudioSourceType.Spell)
        {
            if (spellAudioSource.GetComponents<AudioSource>()[(int)Enum.Parse(typeof(SpellAudioSource), audioSourceName)])
            {
                AudioSource temp = spellAudioSource.GetComponents<AudioSource>()[(int)Enum.Parse(typeof(SpellAudioSource), audioSourceName)];
                temp.loop = true;
                temp.Play();

                loopingOneShotAudioSourceList.Add(temp);
                return temp;
            }
            else
            {
                string value = String.Format("{0} not found in {1}", audioSourceName, audioSourceType.ToString());
                Debug.Log(value);
            }
        }

        return null;   

    }

    public void PlaySoundFromGameObject(AudioSource audioSource)
    {
        audioSource.PlayOneShot(audioSource.clip);
    }

    public AudioSource PlayLoopSoundFromGameObject(AudioSource audioSource)
    {
        audioSource.loop = true;
        audioSource.Play();

        loopingOneShotAudioSourceList.Add(audioSource);
        return audioSource;

    }

    public void AddToLoopingOneShotAudioList(AudioSource audioSource)
    {
        loopingOneShotAudioSourceList.Add(audioSource);
    }

    public void RemoveFromLoopingOneShotAudioList(AudioSource currentAudioSource)
    {
        foreach (AudioSource audioSource in loopingOneShotAudioSourceList)
        {
            if (audioSource == currentAudioSource)
            {
                loopingOneShotAudioSourceList.Remove(audioSource);
                break;
            }
        }
    }

    public void ControlLoopingOneShotAudioList()
    {
        // Game paused, pause all looping audioSource
        if(Time.timeScale == 0)
        {
            foreach(AudioSource audioSource in loopingOneShotAudioSourceList)
                audioSource.Pause();

            soundPaused = true;
        }
        else if (soundPaused)
        {
            foreach (AudioSource audioSource in loopingOneShotAudioSourceList)
                audioSource.UnPause();

            soundPaused = false;
        }
    }
}
