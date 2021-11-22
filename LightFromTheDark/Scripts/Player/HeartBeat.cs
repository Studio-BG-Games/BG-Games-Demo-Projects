using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeat : MonoBehaviour
{
    [Tooltip("При скольки процентах заряда фонарика запускать сердцебиение?")]
    [Range(100, 0)]
    [SerializeField] private float _percentsEnergyWhenActivation;

    [SerializeField] private AudioSource _heartBeat;
    [SerializeField] private AnimationCurve _heartBeatCurve;

    private PlayerController _playerController;
    private Flashlight _flashlight;
    private bool _isHeartBeating;                      //Сердце бьётся?

    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _flashlight = _playerController.Flashlight;
    }

    void Update()
    {
        if (_flashlight.EnergyTimePercents < _percentsEnergyWhenActivation)
        {
            if(_isHeartBeating == false)
            {
                _isHeartBeating = true;
                StartCoroutine(Beats());
            }
        }
        else
        {
            if (_isHeartBeating == true)
            {
                _isHeartBeating = false;
                StopCoroutine(Beats());
            }
        }
    }

    IEnumerator Beats()
    {
        while (_isHeartBeating)
        {
            float localPercents = _flashlight.EnergyTimePercents / _percentsEnergyWhenActivation;
            _heartBeat.volume = 0.5f + (1 - localPercents)*0.5f;
            _heartBeat.pitch = Random.Range(0.9f, 1f);
            _heartBeat.Play();
            float timeBtwnBeat = _heartBeatCurve.Evaluate(1 - localPercents);
            yield return new WaitForSeconds(timeBtwnBeat);
        }
    }
}
