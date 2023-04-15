using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioButtonActivity : MonoBehaviour
{
    public GameObject unmuteButton;
    public GameObject muteButton;

    public void Start()
    {
        if (AudioManager.Instance.soundOn) { 
            DisableMuteButton();
        }
        else
            DisableUnmuteButton();
    }

    public void DisableUnmuteButton()
    {
        unmuteButton.SetActive(false);
        muteButton.SetActive(true);
        AudioManager.Instance.EnableSound(false);
    }

    public void DisableMuteButton()
    {
        muteButton.SetActive(false);
        unmuteButton.SetActive(true);
        AudioManager.Instance.EnableSound(true);
    }

}
