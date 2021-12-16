using System;
using System.Collections;
using System.Collections.Generic;
using Factorys;
using Gameplay.Builds;
using Gameplay.Map;
using Gameplay.Units;
using Plugins.DIContainer;
using Plugins.HabObject;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace Gameplay.GameSceneScript
{
    public class GridBuild : MonoBehaviour
    {
        [SerializeField] private Tile _allowedSprite;
        [SerializeField] private Tile _bannedSprite;
        [SerializeField] private GridBuildTileMap _tileMapTamplate;
        [SerializeField] private float _radius;
        [SerializeField] private Transform _parentForGridSprite;
        [Range(1,40)][SerializeField] private int _countSegmentForAsync=10;

        [DI] private FactoryBuild _factoryBuild;
        [DI] private FactoryUnit _factoryUnit;
        [DI] private WorldShell _worldShell;
        
        private ContainerGrid _containerGrid;
        private Coroutine _updateGridAction;

        private void Awake()
        {
            _containerGrid = new ContainerGrid(_tileMapTamplate, _parentForGridSprite);
        }

        public void VieaArena(Build build)
        {
            if(!build) return;
            List<Brick> bricks = GetBricks();
            foreach (var brick in bricks)
            {
                var pos = brick.transform.position;
                var sprite = _factoryBuild.CanBePlaceAtHere(pos, build) ? _allowedSprite : _bannedSprite;
                _containerGrid.SetSprite(pos,sprite);
            }
        }

        public void VieaArenaAsync(HabObject obj)
        {
            if(obj is Build)
                VieaArenaAsync((Build)obj);
            else if(obj is Unit)
                VieaArenaAsync((Unit)obj);
        }
        
        public void VieaArenaAsync(Build build)
        {
            if(_updateGridAction!=null)
                StopCoroutine(_updateGridAction);
            _updateGridAction = StartCoroutine(VieaArenaCorutine(build));
        }
        
        public void VieaArenaAsync(Unit build)
        {
            if(_updateGridAction!=null)
                StopCoroutine(_updateGridAction);
            _updateGridAction = StartCoroutine(VieaArenaCorutine(build));
        }


        private IEnumerator VieaArenaCorutine(HabObject hab)
        {
            if(!hab) yield break;
            List<Brick> bricks = GetBricks();
            var countSegmentInOneSegment = bricks.Count/_countSegmentForAsync;
            int completeBrick = 0;
            Func<Vector3, bool> CanBePlaceAtHete = GetFunc(hab);
            foreach (var brick in bricks)
            {
                var pos = brick.transform.position;
                
                var sprite = CanBePlaceAtHete.Invoke(pos) ? _allowedSprite : _bannedSprite;
                _containerGrid.SetSprite(pos,sprite);
                completeBrick++;
                if (completeBrick >= countSegmentInOneSegment)
                {
                    completeBrick = 0;
                    yield return null;
                }
            }

            _updateGridAction = null;
        }

        private Func<Vector3,bool> GetFunc(HabObject hab)
        {
            if (hab is Build)
                return v => _factoryBuild.CanBePlaceAtHere(v, hab as Build);
            else
                return v => _factoryUnit.CanBePlaceAtHere(v,hab as Unit);
        }

        [NonSerialized] private List<Brick> _chasBricks;
        
        private List<Brick> GetBricks()
        {
            if (_chasBricks != null)
                return _chasBricks;
            _chasBricks =new List<Brick>();
            for (int x = 0; x < _worldShell.DataMap.ChunkSettings.ChunkSize * _worldShell.DataMap.ChunkSettings.SectorSize * _worldShell.DataMap.MapSettings.Size.x; x++)
            {
                for (int z = 0; z < _worldShell.DataMap.ChunkSettings.ChunkSize * _worldShell.DataMap.ChunkSettings.SectorSize * _worldShell.DataMap.MapSettings.Size.y; z++)
                {
                    var pos = new Vector3(x, 0, z);
                    if(!_worldShell.World.IsOccupied(pos))
                        continue;
                    _chasBricks.Add(_worldShell.World.GetBlock(pos).GetContent().GetUpestBrick());
                }
            }

            return _chasBricks;
        }

        public void HideAll()
        {
            if(_updateGridAction!=null)
                StopCoroutine(_updateGridAction);
            _containerGrid?.ClearAll();
        }

        private class ContainerGrid
        {
            private readonly GridBuildTileMap _template;
            private readonly Transform _parentTileMap;
            private Dictionary<float, GridBuildTileMap> _maps = new Dictionary<float, GridBuildTileMap>();
            
            private static readonly Vector3 OffsetForSprite = new Vector3(0,0.5001f,0);
            
            public ContainerGrid(GridBuildTileMap template, Transform parentTileMap)
            {
                _template = template;
                _parentTileMap = parentTileMap;
            }

            public void SetSprite(Vector3 position, Tile sprite)
            {
                GridBuildTileMap tilemap=null;
                if (!_maps.TryGetValue(position.y, out tilemap))
                {
                    tilemap = Object.Instantiate(_template, new Vector3(0, position.y+OffsetForSprite.y, 0), quaternion.identity, _parentTileMap);
                    tilemap.gameObject.name = "H = " + position.y;
                    _maps.Add(position.y, tilemap);
                }
                tilemap.Tilemap.SetTile(new Vector3Int((int)position.x, (int)position.z, 0), sprite);
            }
            
            public void ClearAll()
            {
                foreach (var gridBuildTileMap in _maps) gridBuildTileMap.Value.Tilemap.ClearAllTiles();
            }
        }
    }
}