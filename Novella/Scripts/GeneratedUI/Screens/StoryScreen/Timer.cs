using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public event Action OnTimerEnd;
    [SerializeField] private Image timerProgressImage;
    private float _time;
    private float _start;

    private void Update()
    {
        var progression = 100 / _start * _time;
        timerProgressImage.fillAmount = progression / 100;
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            OnTimerEnd?.Invoke();
            enabled = false;
        }
    }

    public void SetTimerDuration(float duration)
    {
        _time = duration;
        _start = duration;
    }
    
    public void ActivateTimer()
    {
        enabled = true;
    }
}
