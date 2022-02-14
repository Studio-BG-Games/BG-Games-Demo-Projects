using System.Collections.Generic;
using UnityEngine;

namespace Baby_vs_Aliens.LevelEditor
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class LevelDataEditingTool : MonoBehaviour
    {
        [SerializeField, Min(5)] private int _levelWidth;
        [SerializeField, Min(5)] private int _levelHeight;
        [SerializeField] private Grid _grid;
        [SerializeField] private TileView _tilePrefab;
        [SerializeField] LevelEditorUIView _uiView;
        [SerializeField] private float _cameraDragRate = 0.1f;

        private LevelData _levelData;

        private SaveDataRepository _saveDataRepository;

        private TileView[,] _tiles;

        private bool _isDragging;
        private Vector3 _lastMousePosition;
        private Vector3 _currentMousePosition;

        void Awake()
        {
            _saveDataRepository = new SaveDataRepository();
            _levelData = new LevelData(_levelWidth, _levelHeight, _uiView);

            _uiView.SaveLevelSetButton.onClick.AddListener(OnSaveLevelSetClicked);
            _uiView.LoadLevelSetButton.onClick.AddListener(OnLoadLevelSetClicked);

            _levelData.ClearLayoutRequest += ClearLayout;
            _levelData.SetUpLayoutRequest += SetUpLayout;

            _levelData.Init();
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(1))
            {
                _isDragging = false;
            }

            if ((Screen.height - Input.mousePosition.y) / Screen.height < 0.14f)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                ProcessClick();
            }

            if (Input.GetMouseButtonDown(1))
            {
                _isDragging = true;
                _lastMousePosition = Input.mousePosition;
                _currentMousePosition = Input.mousePosition;
            }

            if (_isDragging)
            {
                _lastMousePosition = _currentMousePosition;
                _currentMousePosition = Input.mousePosition;

                var delta = _currentMousePosition - _lastMousePosition;
                delta.z = delta.y;
                delta.y = 0;

                Camera.main.transform.position -= delta * _cameraDragRate;
            }
        }

        private void OnSaveLevelSetClicked()
        {
            _saveDataRepository.Save(_levelData.CurrentLevelSet);
        }

        private void OnLoadLevelSetClicked()
        {
            var levelSet = new LevelSet();
            _saveDataRepository.LoadWithDialogue(ref levelSet);
            _levelData.ChangeLevelSet(levelSet);
        }

        private void ProcessClick()
        {
            var clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.y = 0;

            if (clickPosition.x < 0 || clickPosition.x > _levelData.LevelWidth ||
                clickPosition.z < 0 || clickPosition.z > _levelData.LevelHeight)
            {
                return;
            }

            var gridIndex = _grid.WorldToCell(clickPosition);
            var x = gridIndex.x;
            var y = gridIndex.z;

            _levelData.ProcessClick(x, y);
            DrawLayout();
        }

        private void SetUpLayout()
        {
            DrawGrid();

            _tiles = new TileView[_levelData.LevelWidth, _levelData.LevelHeight];
            for (int i = 0; i < _levelData.LevelWidth; i++)
                for (int j = 0; j < _levelData.LevelHeight; j++)
                {
                    _tiles[i, j] = Instantiate(_tilePrefab);
                    _tiles[i, j].transform.position = _grid.CellToWorld(new Vector3Int(i, 0, j)) + new Vector3(0.5f, 0f, 0.5f);
                    _tiles[i, j].SetMaterialByType(_levelData.CurrentLevelData[i, j]);
                }
        }

        private void DrawLayout()
        {
            for (int i = 0; i < _levelData.LevelWidth; i++)
                for (int j = 0; j < _levelData.LevelHeight; j++)
                {
                    _tiles[i, j].SetMaterialByType(_levelData.CurrentLevelData[i, j]);
                }
        }

        private void DrawGrid()
        {
            MeshFilter filter = gameObject.GetComponent<MeshFilter>();
            var mesh = new Mesh();
            var verticies = new List<Vector3>();

            int currIndex = 0;

            var indicies = new List<int>();
            for (int i = 0; i < _levelData.LevelWidth; i++)
            {

                verticies.Add(new Vector3(i, 0, 0));
                verticies.Add(new Vector3(i, 0, _levelData.LevelHeight));

                indicies.Add(currIndex++);
                indicies.Add(currIndex++);

            }

            for (int j = 0; j < _levelData.LevelHeight; j++)
            {
                verticies.Add(new Vector3(0, 0, j));
                verticies.Add(new Vector3(_levelData.LevelWidth, 0, j));

                indicies.Add(currIndex++);
                indicies.Add(currIndex++);
            }

            verticies.Add(new Vector3(_levelData.LevelWidth, 0, 0));
            verticies.Add(new Vector3(_levelData.LevelWidth, 0, _levelData.LevelHeight));

            indicies.Add(currIndex++);
            indicies.Add(currIndex++);

            verticies.Add(new Vector3(0, 0, _levelData.LevelHeight));
            verticies.Add(new Vector3(_levelData.LevelWidth, 0, _levelData.LevelHeight));

            indicies.Add(currIndex++);
            indicies.Add(currIndex++);

            mesh.vertices = verticies.ToArray();
            mesh.SetIndices(indicies.ToArray(), MeshTopology.Lines, 0);
            filter.mesh = mesh;

            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
            meshRenderer.material.color = Color.white;
        }

        private void ClearLayout()
        {
            if (_tiles == null)
                return;

            foreach (var tile in _tiles)
                Destroy(tile.gameObject);
        }
    }
}