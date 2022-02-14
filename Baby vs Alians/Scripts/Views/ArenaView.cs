using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

namespace Baby_vs_Aliens
{
    public class ArenaView : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Grid _floorGrid;
        [SerializeField] private Vector2Int _size;
        [SerializeField] ArenaObjectPrefabDictionary _arenaObjectsPrefabs;
        [SerializeField] GameObject _lampPrefab;
        [SerializeField] GameObject _picturePrefab;
        [SerializeField] private float _lampChance = 0.15f;
        [SerializeField] private float _pictureChance = 0.15f;

        private ArenaObjectType[,] _arena;

        private List<GameObject> _arenaObjects;
        private BoxCollider _collider;

        private GameObject _door;

        public Action ExitReachedCallback;

        private Vector2Int _playerSpawnIndex = new Vector2Int(0,0);
        private Dictionary<EnemyType, List<Vector2Int>> _enemySpawnIndices = new Dictionary<EnemyType, List<Vector2Int>> 
        { 
            {EnemyType.Big, new List<Vector2Int>() },
            {EnemyType.Small, new List<Vector2Int>() }
        };

        #endregion


        #region Properties

        public Vector2Int Size => _size;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _arenaObjects = new List<GameObject>();

            if (_floorGrid == null)
                _floorGrid = GetComponentInChildren<Grid>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;

            for (int x = _size.x / 2; x > _size.x / 2 - _size.x; x--)
                for (int y = _size.y / 2; y > _size.y / 2 - _size.y; y--)
                    DrawCell(new Vector2Int(x, y));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                ExitReachedCallback?.Invoke();
        }

        #endregion


        #region Methods

        private void DrawCell(Vector2Int cell)
        {
            var cellPosition = _floorGrid.CellToWorld(new Vector3Int(cell.x, 0, cell.y));
            var cellSizeX = _floorGrid.cellSize.x;
            var cellSizeZ = _floorGrid.cellSize.z;
            var bottomLeft = new Vector3(cellPosition.x - cellSizeX / 2, 0, cellPosition.z - cellSizeZ / 2);
            var bottomRight = bottomLeft + Vector3.right * cellSizeX;
            var topLeft = bottomLeft + Vector3.forward * cellSizeZ;
            var topRight = bottomLeft + Vector3.right * cellSizeX + Vector3.forward * cellSizeZ;
            Gizmos.DrawLine(bottomLeft, bottomRight);
            Gizmos.DrawLine(bottomLeft, topLeft);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
        }

        public Vector3 GetRandomPosition()
        {
            (int x, int y) randomIndex;
            do
            {
                randomIndex.x = Random.Range(2, _size.x - 1);
                randomIndex.y = Random.Range(2, _size.y - 1);

            } while (_arena[randomIndex.x, randomIndex.y] != ArenaObjectType.Floor);

            var randomCell = new Vector3Int(
                    -_size.x / 2 + randomIndex.x,
                    0,
                    -_size.y / 2 + randomIndex.y);

            return _floorGrid.CellToWorld(randomCell);
        }

        public Vector3 GetPlayerSpawnPosition()
        {
            if (_playerSpawnIndex.x > 0 && _playerSpawnIndex.y > 0 &&
                _playerSpawnIndex.x < _arena.GetLength(0) - 1 && _playerSpawnIndex.y < _arena.GetLength(1) - 1)
            {
                var cell = new Vector3Int(
                    -_size.x / 2 + _playerSpawnIndex.x,
                    0,
                    -_size.y / 2 + _playerSpawnIndex.y);

                return _floorGrid.CellToWorld(cell);
            }

            return GetRandomPosition();
        }

        public Vector3 GetEnemySpawnPosition(EnemyType enemyType)
        {
            if (_enemySpawnIndices[enemyType].Count > 0)
            {
                var randomIdx = Random.Range(0, _enemySpawnIndices[enemyType].Count);

                var spawnIndex = _enemySpawnIndices[enemyType][randomIdx];

                var cell = new Vector3Int(
                    -_size.x / 2 + spawnIndex.x,
                    0,
                    -_size.y / 2 + spawnIndex.y);

                _enemySpawnIndices[enemyType].RemoveAt(randomIdx);

                return _floorGrid.CellToWorld(cell);
            }

            return GetRandomPosition();
        }

        public void CreateRandomArena()
        {
            ResetSpawnsInfo();

            _arena = new ArenaObjectType[_size.x, _size.y];

            for (int i = 0; i < _size.x; i++)
            {
                for (int j = 0; j < _size.y; j++)
                {
                    if (i == 0 || j == 0 || i == _size.x - 1 || j == _size.y - 1)
                    {
                        _arena[i, j] = ArenaObjectType.Wall;
                    }
                    else if (i % 2 == 0 && j % 2 == 0)
                    {
                        if (Random.value > 0.9f)
                        {
                            _arena[i, j] = ArenaObjectType.ObstacleHor;

                            int a = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                            int b = a != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                            _arena[i + a, j + b] = ArenaObjectType.ObstacleHor;
                        }
                    }
                }
            }

            AddDoor();

            PlaceObjects();
        }

        public void CreateArenaFromLevelData(ArenaObjectType[,] arena)
        {
            ResetSpawnsInfo();

            _arena = new ArenaObjectType[arena.GetLength(0), arena.GetLength(1)];
            Array.Copy(arena, _arena, arena.Length);
            _size = new Vector2Int(_arena.GetLength(0), _arena.GetLength(1));

            AddDoor();
            AssessSpawnPlacements();
            PlaceObjects();
        }

        private void AddDoor()
        {
            _arena[_size.x / 2, _size.y - 1] = ArenaObjectType.Door;
            var offset = _floorGrid.CellToWorld(new Vector3Int(0, 0, -_size.y / 2 + _size.y - 1));
            _collider.center = offset;
        }

        private void PlaceObjects()
        {
            List<(int x, int y)> cellsToIgnore = new List<(int x, int y)>()
                ;
            for (int i = 0; i < _size.x; i++)
            {
                for (int j = 0; j < _size.y; j++)
                {
                    if (cellsToIgnore.Contains((i, j)))
                        continue;

                    GameObject go;
                    var randomRotation = new Vector3
                        (0,
                        Random.value > .5f ? 180 : 0,
                        0);

                    switch (_arena[i, j]) 
                    {
                        case ArenaObjectType.Wall:
                            go = Object.Instantiate(_arenaObjectsPrefabs[ArenaObjectType.Wall], transform);
                            AddDecorations(i, j, go);
                            break;
                        case ArenaObjectType.ObstacleHor:
                            go = Object.Instantiate(_arenaObjectsPrefabs[ArenaObjectType.ObstacleHor], 
                                _arenaObjectsPrefabs[ArenaObjectType.ObstacleHor].transform.position, Quaternion.Euler(randomRotation), transform);
                            break;
                        case ArenaObjectType.ObstacleVer:
                            go = Object.Instantiate(_arenaObjectsPrefabs[ArenaObjectType.ObstacleVer], 
                                _arenaObjectsPrefabs[ArenaObjectType.ObstacleVer].transform.position, Quaternion.Euler(randomRotation), transform);
                            break;
                        case ArenaObjectType.Door:
                            go = Object.Instantiate(_arenaObjectsPrefabs[ArenaObjectType.Door], transform);
                            _door = go;
                            break;
                        default:
                            continue;
                    }

                    var position = _floorGrid.CellToWorld(new Vector3Int(-_size.x / 2 + i, 0, -_size.y / 2 + j));

                    if (_arena[i, j] == ArenaObjectType.ObstacleHor)
                    {
                        position.x += _floorGrid.cellSize.x / 2;
                        cellsToIgnore.Add((i + 1, j));
                    }

                    if (_arena[i, j] == ArenaObjectType.ObstacleVer)
                    {
                        position.z += _floorGrid.cellSize.z / 2;
                        cellsToIgnore.Add((i, j + 1));
                    }

                    position.y = go.transform.position.y;
                    go.transform.position = position;

                    _arenaObjects.Add(go);


                }
            }
        }

        private void AddDecorations(int i, int j, GameObject wall)
        {
            if (!(i == 0 && j == 0 || i == 0 && j == _size.y - 1 ||
                i == _size.x - 1 && j == 0 || i == _size.x - 1 && j == _size.y - 1))
            {
                var rnd = Random.value;

                if (rnd < _lampChance || rnd > 1 - _pictureChance)
                {

                    Quaternion rotation = Quaternion.identity;

                    if (i == 0)
                    {
                        rotation = Quaternion.Euler(new Vector3(0, -90, 0));
                    }

                    if (i == _size.x - 1)
                    {
                        rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                    }

                    if (j == 0)
                    {
                        rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    }

                    if (rnd < _lampChance)
                    {
                        Object.Instantiate(_lampPrefab, wall.transform.position, rotation, wall.transform);
                    }
                    else
                    {
                        Object.Instantiate(_picturePrefab, wall.transform.position, rotation, wall.transform);
                    }
                }
            }

            if (i == 0 && j != 0 && j != _size.y - 1)
            {
                wall.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 20));
                wall.transform.Translate(new Vector3(0, -0.15f, 0));
            }

            if (i == _size.x - 1 && j != 0 && j != _size.y - 1)
            {
                wall.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -20));
                wall.transform.Translate(new Vector3(0, -0.15f, 0));
            }
        }

        private void AssessSpawnPlacements()
        {
            for (int i = 0; i < _size.x; i++)
            {
                for (int j = 0; j < _size.y; j++)
                {
                    if (_arena[i, j] == ArenaObjectType.PlayerSpawn)
                        _playerSpawnIndex = new Vector2Int(i, j);

                    if (_arena[i, j] == ArenaObjectType.BigEnemySpawn)
                        _enemySpawnIndices[EnemyType.Big].Add(new Vector2Int(i, j));

                    if (_arena[i, j] == ArenaObjectType.SmallEnemySpawn)
                        _enemySpawnIndices[EnemyType.Small].Add(new Vector2Int(i, j));
                }
            }
        }

        private void ResetSpawnsInfo()
        {
            _playerSpawnIndex = new Vector2Int(0, 0);
            _enemySpawnIndices[EnemyType.Big].Clear();
            _enemySpawnIndices[EnemyType.Small].Clear();
        }

        public void ClearArena()
        {
            foreach (var go in _arenaObjects)
                Destroy(go);
            _arenaObjects.Clear();
        }

        public void OpenDoor()
        {
            try
            {
                _door.GetComponentInChildren<Animator>().SetTrigger("Open");
            }
            catch
            {
                _door.SetActive(false);
            }
        }

        #endregion
    }
}