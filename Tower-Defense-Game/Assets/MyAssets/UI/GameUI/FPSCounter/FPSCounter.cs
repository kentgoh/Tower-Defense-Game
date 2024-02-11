using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Utility
{
    [RequireComponent(typeof (TMP_Text))]
    public class FPSCounter : MonoBehaviour
    {
        const float fpsMeasurePeriod = 0.5f;
        const string display = "FPS: {0}";
        private TMP_Text FPSText;

        private float fpsCount = 0;


        private void Start()
        {
            FPSText = GetComponent<TMP_Text>();

            StartCoroutine(FPS());
        }


        private void Update()
        {

            // Update the FPS value on Text component
            FPSText.text = string.Format(display, (int) fpsCount);

        }

        public IEnumerator FPS()
        {
            while (true)
            {
                fpsCount = 1f / Time.unscaledDeltaTime;
                yield return new WaitForSeconds(fpsMeasurePeriod);
            }
        }
    }
}
