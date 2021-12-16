using System;
using System.Drawing;
using Extension;
using Factorys;
using Gameplay.Builds.Beh;
using Gameplay.Builds.Data;
using Gameplay.GameSceneScript;
using Gameplay.Map;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Unity.Framework;
using Gameplay.Units;
using Plugins.DIContainer;
using Plugins.HabObject;
using Plugins.HabObject.GeneralProperty;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Builds
{
    [RequireComponent(typeof(FixedRotateble))]
    public class Ghost : MonoBehaviour
    {
        [SerializeField] private CubeGhost _cubeGhost;
        [SerializeField] private Color _colorOnCanPlace;
        [SerializeField] private Color _colorOnCanNotPlace;

        private HabObject _hab;
        private FixedRotateble _fixedRotateble;
        private GhostView _currentView;

        private FixedRotateble FixedRotateble => _fixedRotateble!=null?_fixedRotateble : _fixedRotateble = GetComponent<FixedRotateble>();
        
        [DI] private WorldShell _worldShell;
        [DI] private FactoryBuild _factoryBuild;
        [DI] private FactoryUnit _factoryUnit;
        [DI] private GridBuild _grid;
        private float _lenghtBeforeCamera = 12;

        private void OnDisable()
        {
            _grid.HideAll();
        }

        public void Init(HabObject hab)
        {
            _hab = hab;
            FixedRotateble.SetZero();
            _currentView = CreateView(_hab);
            var size = hab.MainDates.GetOrNull<SizeOnMap>();
            SetPositionView(size);
            _currentView.Init(size);
            GridView(_hab);
        }

        private GhostView CreateView(HabObject hab)
        {
            var container = hab.MainDates.GetOrNull<GhostViewContainer>();
            if (container==null) return CerateDefaultView(_currentView!=null);
            if (container.ViewGhost == null) return CerateDefaultView(_currentView!=null);

            if(_currentView)
                Destroy(_currentView.gameObject);
            _currentView = Instantiate(container.ViewGhost, transform);
            return _currentView;
        }
        
        private GhostView CerateDefaultView(bool createDefault)
        {
            if (_currentView == null || createDefault)
            {
                if(_currentView)
                    Destroy(_currentView.gameObject);
                _currentView = Instantiate(_cubeGhost, transform);
            }
            return _currentView;
        }

        private void SetPositionView(SizeOnMap size)
        {
            if (size == null) return;
            _currentView.transform.position = transform.position + size.Offset.ToVector3XZ()-SizeOnMap.SingModifacateXZ(transform.eulerAngles.y)/2;
        }

        private void GridView(HabObject hab)
        {
            if (hab is Build)
                _grid.VieaArenaAsync(hab as Build);
            else
                _grid.VieaArenaAsync(hab as Unit);
        }

        public void TurnTo(bool toActive)
        {
            if (!toActive)
                _hab = null;
            gameObject.SetActive(toActive);
        }

        public void StandBeforeCamera()
        {
            Camera camera = Camera.main;
            var forwardCamera = camera.transform.forward;
            forwardCamera.y = 0;
            forwardCamera *= _lenghtBeforeCamera;
            var resultPosCamera = camera.transform.position + forwardCamera;
            MoveAtPosition(resultPosCamera);
        }

        public void Rotate(bool toright)
        {
            if(toright) FixedRotateble.RotateRight();
            else FixedRotateble.RotateLeft();
            MoveAtPosition(transform.position);
        }

        public void MoveAtPosition(Vector3 getPositionForGhost)
        {
            Vector3 newPos;
            getPositionForGhost.y = 0;
            getPositionForGhost.x = Mathf.Floor(getPositionForGhost.x);
            getPositionForGhost.z = Mathf.Floor(getPositionForGhost.z);
            if (_worldShell.World.IsOccupied(getPositionForGhost))
                newPos = GetPositionFromBlockWrold(getPositionForGhost);
            else
                newPos = GetPositionFromBlockWrold(Vector3.zero);
            ChangePosition(newPos);
        }

        public void MoveToDiraction(Vector2Int diraction)
        {
            var modXZ = SizeOnMap.GetModifacateXZ(transform.eulerAngles.y);
            var posBlock = transform.position + diraction.ToVector3XZ();
            posBlock.y = 0;
            ChangePosition(GetPositionFromBlockWrold(posBlock));
        }

        private Vector3 GetPositionFromBlockWrold(Vector3 getPositionForGhost)
        {
            getPositionForGhost.y = 0;
            var block = _worldShell.World.GetBlock(getPositionForGhost);
            Brick upestBrick = block.GetContent().GetUpestBrick();
            return upestBrick.transform.position + (Vector3.up * 0.5f);
        }

        private void ChangePosition(Vector3 newPos)
        {
            transform.position = newPos;
            _currentView?.SetColor(CanBePlace() ? _colorOnCanPlace : _colorOnCanNotPlace);
        }

        private bool CanBePlace()
        {
            if (!_hab)
                return false;
            if (_hab is Build)
                return _factoryBuild.CanBePlaceAtHere(_hab as Build, transform.position, transform.eulerAngles);
            else
                return _factoryUnit.CanBePlaceAtHere(_hab as Unit, transform.position, transform.eulerAngles);
        }

        public void SetBuild()
        {
            if(!_hab)
                return;
            if (_hab is Build)
                _factoryBuild.Spawn((Build)_hab, transform.position, transform.eulerAngles);
            else
                _factoryUnit.Spawn((Unit)_hab, transform.position, transform.eulerAngles);
            GridView(_hab);
        }
    }
}