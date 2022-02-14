using System;
using UnityEngine;

namespace Baby_vs_Aliens.LevelEditor
{
    public class LevelData
    {
        LevelEditorUIView _uiView;

        private ArenaObjectType[,] _levelData;

        private Vector2Int _previousPlayerSpawnIndex;

        private LevelSet _levelSet;
        private int _currentLevelIndex;

        private int _defaultWidth;
        private int _defaultHeight;
        private int _width;
        private int _height;

        public event Action ClearLayoutRequest;
        public event Action SetUpLayoutRequest;

        public int LevelWidth => _width;
        public int LevelHeight => _height;
        public LevelSet CurrentLevelSet => _levelSet;
        public ArenaObjectType[,] CurrentLevelData => _levelData;

        public LevelData(int defalutWidth, int defaultHeight, LevelEditorUIView uiView)
        {
            _defaultHeight = defaultHeight;
            _height = _defaultHeight;
            _defaultWidth = defalutWidth;
            _width = _defaultWidth;

            _uiView = uiView;

            _uiView.LevelSelectionDropdown.onValueChanged.AddListener(OnDropdownValueChange);
            _uiView.AddLevelButton.onClick.AddListener(AddLevel);
            _uiView.DeleteLevelButton.onClick.AddListener(DeleteLevel);
            _uiView.NewLevelSetButton.onClick.AddListener(CreateNewLevelSet);
        }

        public void Init()
        {

            CreateNewLevelSet();
        }

        public void ChangeLevelSet(LevelSet levelSet)
        {
            _levelSet = levelSet;
            _currentLevelIndex = 0;
            _uiView.FillDropdownOptions(_levelSet.LevelCount, _currentLevelIndex);
            LoadLevel(_currentLevelIndex);
        }

        private void CreateNewLevelSet()
        {
            _levelSet = new LevelSet();
            _currentLevelIndex = 0;
            LoadLevel(_currentLevelIndex);
            _uiView.FillDropdownOptions(_levelSet.LevelCount, _currentLevelIndex);
        }

        private void LoadLevel(int index)
        {
            ClearLayoutRequest?.Invoke();

            _levelData = _levelSet.GetLevel(index).LevelData;
            _width = _levelData.GetLength(0);
            _height = _levelData.GetLength(1);

            CheckForWallsAndPlayerSpawn();

            SetUpLayoutRequest?.Invoke();
        }

        private void CheckForWallsAndPlayerSpawn()
        {
            for (int i = 0; i < _width; i++)
                for (int j = 0; j < _height; j++)
                {
                    if (i == 0 || j == 0 || i == _width - 1 || j == _height - 1)
                        _levelData[i, j] = ArenaObjectType.Wall;

                    if (_levelData[i, j] == ArenaObjectType.PlayerSpawn)
                        _previousPlayerSpawnIndex = new Vector2Int(i, j);
                }
        }

        public void ProcessClick(int x, int y)
        {

            if (x <= 0 || y <= 0 || x >= _width - 1 || y >= _height - 1)
                return;

            if (!_uiView.HasToggleSelected)
                return;

            var objectType = _uiView.SelectedObjectType;

            if (objectType == ArenaObjectType.PlayerSpawn)
            {
                if (_levelData[_previousPlayerSpawnIndex.x, _previousPlayerSpawnIndex.y] == ArenaObjectType.PlayerSpawn)
                {
                    _levelData[_previousPlayerSpawnIndex.x, _previousPlayerSpawnIndex.y] = ArenaObjectType.Floor;
                }
                _previousPlayerSpawnIndex = new Vector2Int(x, y);
            }

            if (objectType == ArenaObjectType.ObstacleHor || objectType == ArenaObjectType.ObstacleVer)
            {
                ProcessObstaclePlacement(x, y, objectType);
                return;
            }
            else if (_levelData[x, y] == ArenaObjectType.ObstacleHor || _levelData[x, y] == ArenaObjectType.ObstacleVer)
                ClearNeighbouringObstacle(new Vector2Int(x, y), _levelData[x, y]);

            SetCellData(x, y, objectType);
        }

        private void ProcessObstaclePlacement(int x, int y, ArenaObjectType type)
        {
            if (type != ArenaObjectType.ObstacleHor && type != ArenaObjectType.ObstacleVer)
                return;


            Vector2Int targetCell = new Vector2Int(x, y);
            int offsetX = type == ArenaObjectType.ObstacleHor ? 1 : 0;
            int offsetY = type == ArenaObjectType.ObstacleVer ? 1 : 0;
            Vector2Int neighborCell;

            if (_levelData[targetCell.x, targetCell.y] != type)
            {
                if (_levelData[targetCell.x + offsetX, targetCell.y + offsetY] != ArenaObjectType.ObstacleHor &&
                    _levelData[targetCell.x + offsetX, targetCell.y + offsetY] != ArenaObjectType.ObstacleVer &&
                    _levelData[targetCell.x + offsetX, targetCell.y + offsetY] != ArenaObjectType.Wall)
                    neighborCell = new Vector2Int(targetCell.x + offsetX, targetCell.y + offsetY);
                else if (_levelData[targetCell.x - offsetX, targetCell.y - offsetY] != ArenaObjectType.ObstacleHor &&
                    _levelData[targetCell.x - offsetX, targetCell.y - offsetY] != ArenaObjectType.ObstacleVer &&
                    _levelData[targetCell.x - offsetX, targetCell.y - offsetY] != ArenaObjectType.Wall)
                    neighborCell = new Vector2Int(targetCell.x - offsetX, targetCell.y - offsetY);
                else
                    return;

                var anotherType = type == ArenaObjectType.ObstacleHor ? ArenaObjectType.ObstacleVer : ArenaObjectType.ObstacleHor;
                if (_levelData[targetCell.x, targetCell.y] == anotherType)
                {
                    ClearNeighbouringObstacle(targetCell, anotherType);
                }
            }
            else
            {
                neighborCell = FindNeighboringObstacle(type, targetCell, offsetX, offsetY);
            }

            SetCellData(targetCell.x, targetCell.y, type);
            SetCellData(neighborCell.x, neighborCell.y, type);
        }

        private void ClearNeighbouringObstacle(Vector2Int targetCell, ArenaObjectType type)
        {
            int offsetX = type == ArenaObjectType.ObstacleHor ? 1 : 0;
            int offsetY = type == ArenaObjectType.ObstacleVer ? 1 : 0;

            var neighborCellToClear = FindNeighboringObstacle(type, targetCell, offsetX, offsetY);
            SetCellData(neighborCellToClear.x, neighborCellToClear.y, ArenaObjectType.Floor);
        }

        private Vector2Int FindNeighboringObstacle(ArenaObjectType type, Vector2Int targetCell, int offsetX, int offsetY)
        {
            Vector2Int neighborCell;
            if (_levelData[targetCell.x + offsetX, targetCell.y + offsetY] == type &&
         _levelData[targetCell.x - offsetX, targetCell.y - offsetY] != type)
                neighborCell = new Vector2Int(targetCell.x + offsetX, targetCell.y + offsetY);
            else if (_levelData[targetCell.x - offsetX, targetCell.y - offsetY] == type &&
                _levelData[targetCell.x + offsetX, targetCell.y + offsetY] != type)
                neighborCell = new Vector2Int(targetCell.x - offsetX, targetCell.y - offsetY);
            else
                neighborCell = CountNeighborObstacles(targetCell.x, targetCell.y, type);
            return neighborCell;
        }

        private Vector2Int CountNeighborObstacles(int originX, int originY, ArenaObjectType type)
        {
            int offsetX = type == ArenaObjectType.ObstacleHor ? 1 : 0;
            int offsetY = type == ArenaObjectType.ObstacleVer ? 1 : 0;

            int topRight = 0;
            int bottomLeft = 0;
            int i = 1;
            do
            {
                topRight++;
                i++;
            } while (_levelData[originX + i * offsetX, originY + i * offsetY] == type);

            i = 1;
            do
            {
                bottomLeft++;
                i++;
            } while (_levelData[originX - i * offsetX, originY - i * offsetY] == type);

            topRight %= 2;
            bottomLeft %= 2;

            return topRight > bottomLeft ? new Vector2Int(originX + 1 * offsetX, originY + 1 * offsetY) :
                new Vector2Int(originX - 1 * offsetX, originY - 1 * offsetY);
        }

        private void SetCellData(int x, int y, ArenaObjectType objectType)
        {
            if (_levelData[x, y] != objectType)
                _levelData[x, y] = objectType;
            else
                _levelData[x, y] = ArenaObjectType.Floor;
        }

        private void AddLevel()
        {
            _levelSet.AddNewLevel();
            _currentLevelIndex = _levelSet.LevelCount - 1;
            _uiView.FillDropdownOptions(_levelSet.LevelCount, _currentLevelIndex);
            LoadLevel(_currentLevelIndex);
        }

        private void DeleteLevel()
        {
            if (_levelSet.LevelCount == 1)
                return;

            _levelSet.RemoveLevel(_currentLevelIndex);

            if (_currentLevelIndex >= _levelSet.LevelCount)
                _currentLevelIndex = _levelSet.LevelCount - 1;

            _uiView.FillDropdownOptions(_levelSet.LevelCount, _currentLevelIndex);
            LoadLevel(_currentLevelIndex);
        }

        private void OnDropdownValueChange(int value)
        {
            _currentLevelIndex = value;

            LoadLevel(_currentLevelIndex);
        }
    }
}