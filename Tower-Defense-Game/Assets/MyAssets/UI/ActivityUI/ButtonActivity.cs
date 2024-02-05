using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static GlobalPredefinedModel;

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
        GameActivity.Instance.ga_MouseState.UpdateMouseState(MouseState.Button_UI);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        transform.localScale = cachedScale;
        AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.Standard, "ButtonClick");
        GameActivity.Instance.ga_MouseState.UpdateMouseState(MouseState.None);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        transform.localScale = cachedScale;
        GameActivity.Instance.ga_MouseState.UpdateMouseState(MouseState.None);
    }
}
