using System;
using DefaultNamespace;
using Gameplay.StateMachine.GameScene;
using Infrastructure.SceneStates;
using Interface;
using Plugins.DIContainer;
using UnityEngine;

namespace Gameplay
{
    public class CustomCamera : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset = new Vector3(1,0,1);
        [SerializeField] private Vector2 _offsetFromNeksusOnStart = new Vector2(-4,-4);
        [Min(0)][SerializeField] private float _addHeightForCamera = 4;
        [SerializeField] private float _speed;

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        [DI] private IInput _input;
        [DI] private GameSceneData _gameSceneData;
        [DI] private GameSceneStateMachine _gameSceneStateMachine;

        private Vector3 _minPoint;
        private Vector3 _maxPoint;

        private void OnEnable() => _input.MoveCameraAtDiraction += OnMoveEvent;

        private void OnDisable() => _input.MoveCameraAtDiraction -= OnMoveEvent;

        private void Awake()
        {
            enabled = false;
            _minPoint = Vector3.zero + _offset;
            var chunkSet = _gameSceneData.DataMap.ChunkSettings;
            var maxX = chunkSet.ChunkSize * chunkSet.SectorSize * _gameSceneData.DataMap.MapSettings.Size.x;
            var maxz = chunkSet.ChunkSize * chunkSet.SectorSize * _gameSceneData.DataMap.MapSettings.Size.x;
            _maxPoint = new Vector3(maxX, 0, maxz) - _offset;
            var curset = _gameSceneData.LevelSetProfile.Target.GetLevelSet(_gameSceneData.LevelSetProfile.CurrentData.CurrentIndexLevel);
            
            Vector3 startPostion = new Vector3(curset.PostionNeksus.x + _offsetFromNeksusOnStart.x, _gameSceneData.DataMap.MapSettings.HeightModule.Max+_addHeightForCamera, curset.PostionNeksus.y + _offsetFromNeksusOnStart.y);
            transform.position = startPostion;

            _gameSceneStateMachine.EnteredTo += OnStartMachine;
            
            void OnStartMachine(Type obj)
            {
                if(obj==typeof(BuildNeksusState))
                    return;
                enabled = true;
                _gameSceneStateMachine.EnteredTo -= OnStartMachine;
            }
        }

        private void OnMoveEvent(Vector3 dir)
        {
            dir = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * dir;
            ///dir = transform.InverseTransformDirection(dir);
            
            transform.position = Vector3.Lerp(transform.position, transform.position + dir, Time.deltaTime*_speed);
            //transform.Translate(dir, Space.Self);

            transform.position = ClampPosition();
        }

        private Vector3 ClampPosition()
        {
            return new Vector3(
                Mathf.Clamp(transform.position.x, _minPoint.x, _maxPoint.x), 
                transform.position.y, 
                Mathf.Clamp(transform.position.z, _minPoint.z, _maxPoint.z));
        }

        private void OnValidate()
        {
            _offset.y = 0;
            if (_offset.x < 0)
                _offset.x = 0;
            if (_offset.z < 0)
                _offset.z = 0;
        }
    }
}