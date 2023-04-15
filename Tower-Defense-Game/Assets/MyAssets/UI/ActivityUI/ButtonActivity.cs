using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonActivity : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Vector3 cachedScale;

    private void Start()
    {
        cachedScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        transform.localScale = cachedScale;
        AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.ButtonClick);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        transform.localScale = cachedScale;
    }
}
