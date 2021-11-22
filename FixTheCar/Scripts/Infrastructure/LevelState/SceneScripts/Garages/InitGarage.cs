using System;
using System.Collections.Generic;
using Factories;
using Infrastructure.Configs;
using Infrastructure.LevelState.States;
using Mechanics.Garage;
using Plugins.DIContainer;
using Plugins.Interfaces;
using UnityEngine;

namespace Infrastructure.LevelState.SceneScripts.Garages
{
    public class InitGarage : MonoBehaviour
    {
        public event Action<List<Garage>> Inited;
        
        [SerializeField] private FactoryPrompter _factoryPrompter;
        
        [DI] private Curtain _curtain;
        [DI] private ConfigGame _configGame;
        [DI] private FactoryGarage _factoryGarage;
        [DI] private ConfigLocalization _configLocalization;
        
        private List<Garage> _garages;
        private Camera _camera;

        private void Start()
        {
            _garages = SpawnGatages(_configGame.ConfigLevels, _configGame.TemplateGarage);
            SetCamera();
            _factoryPrompter.ChangeAt(FactoryPrompter.Type.Idea);
            _curtain.Unfade();
            StartDialog();
        }

        private void StartDialog()
        {
            var current = _factoryPrompter.Current;
            current.Unhide(
                () => current.Say(_configLocalization.FirstFixCar,
                    () => Inited?.Invoke(_garages)
                )
            );
        }

        private void SetCamera()
        {
            _camera = Camera.main;
            _camera.transform.position = _garages[0].PointCamera.position;
        }

        private List<Garage> SpawnGatages(in List<ConfigLevel> levels, Garage template)
        {
            List<Garage> result = new List<Garage>();
            for (int i = 0; i < levels.Count; i += 2)
            {
                var leftLevel = levels[i];
                ConfigLevel rightLevel = null;
                if (i + 1 < _configGame.ConfigLevels.Count)
                    rightLevel = levels[i+1];

                Vector3 positionSpawn = result.Count==0? Vector3.zero : GetPositionSpawn(result[result.Count-1], template);
                result.Add( _factoryGarage.Create(template, positionSpawn, leftLevel, rightLevel));
            }

            return result;
        }

        private Vector3 GetPositionSpawn(Garage prev, Garage next)
        {
            Vector3 result = prev.transform.position;
            Vector3 offset = new Vector3(prev.SizeGarage.Size.x/2+next.SizeGarage.Size.x/2, 0, 0);
            return result + offset;
        }
    }
}