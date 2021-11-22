using System;
using System.Collections.Generic;
using DG.Tweening;
using Factories;
using Mechanics.Garage;
using Plugins.DIContainer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Infrastructure.LevelState.SceneScripts.Garages
{
    public class MoveCameraInGarage : MonoBehaviour
    {
        [SerializeField] private InitGarage _initGarage;
        [SerializeField] private Button _nextGarage;
        [SerializeField] private Button _prevGarage;
        [SerializeField] private float _durationChangeGarage;

        private List<Garage> _garages;
        private int _currentIndexGarage = 0;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _initGarage.Inited += OnInited;
            SetActibeButton(false);
            _nextGarage.onClick.AddListener(()=>ChangeGarageAt(1));
            _prevGarage.onClick.AddListener(()=>ChangeGarageAt(-1));
        }

        private void OnInited(List<Garage> obj)
        {
            _garages = obj;
            _currentIndexGarage = 0;
            SetActibeButton(true);
        }

        private void ChangeGarageAt(int i)
        {
            if (_currentIndexGarage + i < 0 || _currentIndexGarage + i >= _garages.Count)
                return;
            SetActibeButton(false);
            _currentIndexGarage += i;
            _camera.transform.DOMove(_garages[_currentIndexGarage].PointCamera.position, _durationChangeGarage).OnStart(()=>SetActibeButton(true));
        }

        private void SetActibeButton(bool toActive)
        {
            _nextGarage.interactable = _prevGarage.interactable = toActive;
            ChangeActiveButtonByIndex();
        }

        private void ChangeActiveButtonByIndex()
        {
            if (_currentIndexGarage == 0)
                _prevGarage.interactable = false;
            if(_garages!=null)
                if (_currentIndexGarage == _garages.Count - 1)
                    _nextGarage.interactable = false;
        }
    }
}