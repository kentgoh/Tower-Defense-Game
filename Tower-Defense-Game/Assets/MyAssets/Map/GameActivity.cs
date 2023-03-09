using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameActivity : MonoBehaviour
{
    public float time = -1;
    public GameObject timerUI;
    public GameObject pauseUI;

    // Updated from pointer handle in UIActivity script
    public string selectedTurretName;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        StartCoroutine("addTimeInSeconds");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Pause game
            if(Time.timeScale == 1)
            {
                Debug.Log("Pause Triggered");
                Time.timeScale = 0;
                pauseUI.SetActive(true);
            }
            else
            {
                Debug.Log("Resume Triggered");
                Time.timeScale = 1;
                pauseUI.SetActive(false);
            }

        }
    }

    public IEnumerator addTimeInSeconds()
    {
        while(Time.timeScale == 1) {
            time++;
            timerUI.GetComponentInChildren<TMP_Text>().text = time.ToString();
            yield return new WaitForSeconds(1);
        }
    }
}
