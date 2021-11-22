using Infrastructure.Configs;
using Mechanics.Garage;
using Plugins.DIContainer;
using UnityEngine;

namespace Factories
{
    public class FactoryGarage
    {
        DiBox _diBox = DiBox.MainBox;

        public Garage Create(Garage template, Vector2 position, ConfigLevel left, ConfigLevel right=null)
        {
            Garage result = _diBox.CreatePrefab(template);
            result.transform.position = position;
            
            var leftCar = CreateCar(left);
            Car rightCar = null;
            if (right)
                rightCar = CreateCar(right);

            result.Init(leftCar, rightCar);
            return result;
        }

        private Car CreateCar(ConfigLevel left)
        {
            Car leftCar = _diBox.CreatePrefab(left.CarTemplate);
            leftCar.Init(left);
            return leftCar;
        }
    }
}