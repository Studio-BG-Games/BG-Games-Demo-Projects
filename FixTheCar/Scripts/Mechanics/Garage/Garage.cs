using System;
using System.Collections.Generic;
using Factories;
using Infrastructure.LevelState.SceneScripts.Garages;
using Plugins.DIContainer;
using UnityEngine;
using UnityEngine.UI;

namespace Mechanics.Garage
{
    public class Garage : MonoBehaviour
    {
        public SizeElement SizeGarage => _sizeGarage;
        public Transform PointCamera => _pointCamera;
        
        [SerializeField] private Transform _pointCamera;
        [SerializeField] private SizeElement _sizeGarage;
        [SerializeField] private GaragePlace _leftPlace;
        [SerializeField] private GaragePlace _rightPlace;
        [SerializeField] private Image _templateStar;

        [DI] private SelectorCar _selectorCar;
        
        private List<Car> _carInside = new List<Car>();
        
        public void Init(Car carLeft, Car carRight = null)
        {
            _leftPlace.SetCar(carLeft,_templateStar);
            _carInside.Add(carLeft);
            if (carRight)
            {
                _rightPlace.SetCar(carRight,_templateStar);
                _carInside.Add(carRight);
            }

            _selectorCar.NewCarSelect += OnNewCarSelect;
        }

        private void OnNewCarSelect(Car obj)
        {
            if (!obj || !_carInside.Contains(obj))
            {
                _leftPlace.Unselect();
                _rightPlace.Unselect();
                return;
            }

            if (_leftPlace.Car == obj)
            {
                _leftPlace.Select();
                _rightPlace.Unselect();
            }
            else
            {
                _leftPlace.Unselect();
                _rightPlace.Select();
            }
        }

        private void OnDestroy()
        {
            _selectorCar.NewCarSelect -= OnNewCarSelect;
            _selectorCar.Off();
        }

        [Serializable]
        private class GaragePlace
        {
            [HideInInspector] public Car Car { get; private set; }
            [SerializeField] private Transform PointCar;
            [SerializeField] private Lamp Lamp;
            [SerializeField] private HorizontalLayoutGroup _panelOfStar;

            public void SetCar(Car car, Image startTemplate)
            {
                car.transform.position = PointCar.transform.position;
                car.transform.SetParent(PointCar);
                Car = car;
                for (int i = 0; i < car.LevelConfigCar.Difficulty; i++) 
                    Instantiate(startTemplate, _panelOfStar.transform);
            }

            public void Select()
            {
                Lamp.On();
                Car.Select();
            }

            public void Unselect()
            {
                Lamp.Off();
                Car.Unselect();
            }
        }
    }
}