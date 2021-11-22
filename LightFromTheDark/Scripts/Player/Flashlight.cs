using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Flashlight : MonoBehaviour
{
    //Текущее время заряда 
    [HideInInspector] public float EnergyTime;

    //Максимальное время заряда
    [SerializeField] private float _maxEnergyTime;
    [SerializeField] private PlayerController _playerContr;
    [SerializeField] private Light2D _light;
    [SerializeField] private AnimationCurve _curveLightIntensity;
    [SerializeField] private AnimationCurve _curveLightOuterRadius;
    private AudioSource _blinkingAudio;

    //Энергии для начала мигания 
    private float _energyStartBlinking;
    //Моргнул?
    private bool _isBlinked;

    public float EnergyTimePercents 
    { 
        get{ return EnergyTime / _maxEnergyTime * 100f; }
    }

    private void Awake()
    {
        _blinkingAudio = GetComponent<AudioSource>();
        EnergyTime = _maxEnergyTime;
        _energyStartBlinking = Random.Range(_maxEnergyTime/2f, _maxEnergyTime/4f);
    }

    private void Update()
    {
        DecreaseInCharge();
        if(_isBlinked == false && EnergyTime < _energyStartBlinking)
        {
            StartCoroutine(Blinking());
        }
    }

    private void DecreaseInCharge()
    {
        if (EnergyTime > 0.0f)
        {
            EnergyTime -= Time.deltaTime;
            _light.intensity = _curveLightIntensity.Evaluate(1 - EnergyTime / _maxEnergyTime);
            _light.pointLightOuterRadius = _curveLightOuterRadius.Evaluate(1 - EnergyTime / _maxEnergyTime);
            if (EnergyTime < 0.0f)
            {
                _playerContr.Death();
            }
        }
    }

    public void AddEnergy(float energy)
    {
        EnergyTime += energy;
        if (EnergyTime > _maxEnergyTime)
            EnergyTime = _maxEnergyTime;
    }

    IEnumerator Blinking()
    {
        _isBlinked = true;
        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            _light.enabled = false;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
            _blinkingAudio.Play();
            _light.enabled = true;
            yield return new WaitForSeconds(Random.Range(0.15f, 0.3f));
        }
    }
}
