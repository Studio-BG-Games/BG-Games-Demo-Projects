using UnityEngine;

namespace Gameplay.UI.Menu.Canvas
{
    public class PreviewCamera : MonoBehaviour
    {
        public RenderTexture RenderTexture => _renderTexture;
        
        [SerializeField] private Camera _camera;
        [SerializeField] private RenderTexture _renderTexture;
        [SerializeField] private Transform _pointFloorToSpawn;
        [SerializeField] private float _speedRotate;

        private GameObject CurrentModel;
        private Vector3 _defaultRotate;
        
        private void Awake()
        {
            _camera.targetTexture = _renderTexture;
            _defaultRotate = _pointFloorToSpawn.eulerAngles;
        }

        public void SetModel(GameObject animator)
        {
            DeleteModel();
            CurrentModel = Instantiate(animator, _pointFloorToSpawn.position, _pointFloorToSpawn.rotation);
            CurrentModel.transform.SetParent(_pointFloorToSpawn);
        }

        public void DeleteModel()
        {
            if (CurrentModel)
                Destroy(CurrentModel);
            _pointFloorToSpawn.eulerAngles = _defaultRotate;
        }

        public void Rotate(float dirX)
        {
            _pointFloorToSpawn.Rotate(0, dirX*_speedRotate*Time.deltaTime, 0);
        }
    }
}