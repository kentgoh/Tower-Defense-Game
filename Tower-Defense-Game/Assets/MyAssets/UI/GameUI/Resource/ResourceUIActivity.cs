using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUIActivity : MonoBehaviour
{
    private GameObject gameSystem;
    private TMP_Text resourcesTextUI;
    private TMP_Text resourcesPerSecondTextUI;
    private Image resourcesImageUI;
    
    private float timePassed = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameSystem = GameObject.FindGameObjectWithTag("GameSystem");

        if (transform.Find("Count"))
            resourcesTextUI = transform.Find("Count").GetComponent<TMP_Text>();
        if (transform.Find("PerSecond"))
            resourcesPerSecondTextUI = transform.Find("PerSecond").GetComponent<TMP_Text>();
        if (transform.Find("Image"))
            resourcesImageUI = transform.Find("Image").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayResources();
        DisplayResourcesPerSecond();
        DisplayResourcesImage();
    }

    public void DisplayResources()
    {
        resourcesTextUI.text = gameSystem.GetComponent<GameActivity>().ga_Resource.resources.ToString();
    }

    public void DisplayResourcesPerSecond()
    {
        resourcesPerSecondTextUI.text = "+" + gameSystem.GetComponent<GameActivity>().ga_Resource.resourcesPerSecond.ToString() + "/s";
    }

    public void DisplayResourcesImage()
    {
        timePassed += Time.deltaTime;

        float timeRangeClamped = Mathf.InverseLerp(1, 0, timePassed % 1);
        resourcesImageUI.fillAmount = Mathf.Lerp(1, 0, timeRangeClamped);

        if (timePassed >= 1)
            timePassed = 0;

    }
}
