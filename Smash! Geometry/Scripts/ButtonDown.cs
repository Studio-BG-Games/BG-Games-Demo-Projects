using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonDown :  MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent onPointerDown;
    public UnityEvent onPointerUp;
    [SerializeField] GameController gameController;
// gets invoked every frame while pointer is down
public UnityEvent whilePointerPressed;

private Button _button;

private void Awake()
{
    _button = GetComponent<Button>();
}

private IEnumerator WhilePressed()
{
    // this looks strange but is okey in a Coroutine
    // as long as you yield somewhere
    while (true)
    {
        whilePointerPressed?.Invoke();
        yield return null;
    }
}

public void OnPointerDown(PointerEventData eventData)
{
    // ignore if button not interactable
    if (!_button.interactable) return;

        // just to be sure kill all current routines
        // (although there should be none)
        gameController.ClickZone();
       // StopAllCoroutines();

    onPointerDown?.Invoke();
}

public void OnPointerUp(PointerEventData eventData)
{
        StartCoroutine(nameof(ReloadButton));
        //StopAllCoroutines();
        onPointerUp?.Invoke();

}

public void OnPointerExit(PointerEventData eventData)
{
        
       // StopAllCoroutines();
    onPointerUp?.Invoke();
        
        
}

    IEnumerator ReloadButton()
    {
        _button.interactable = false;
        yield return new WaitForEndOfFrame();
        _button.interactable = true;
        yield break;
    }

    // Afaik needed so Pointer exit works .. doing nothing further
    public void OnPointerEnter(PointerEventData eventData) { }
}
