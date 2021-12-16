using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Game
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class FlexibleGridLayout : MonoBehaviour
    {
        float defaultRatio16_9 = 1.777133f;
        GridLayoutGroup layout;
        [SerializeField] private float _defaultPadding;
        [SerializeField] private Vector2 _defaultSpacing;
        [SerializeField] private Vector2 _defaultCellSize;
        
        // Start is called before the first frame update
        void Start()
        {
            layout = GetComponent<GridLayoutGroup>();
            MakeCeilSize(layout);
        }

        [ContextMenu("Update ceil size")]
        void UpdateSize() => MakeCeilSize(GetComponent<GridLayoutGroup>());

        private void MakeCeilSize(GridLayoutGroup grid)
        {
            grid.cellSize = _defaultCellSize*(Camera.main.aspect / defaultRatio16_9);
            grid.padding.left = (int) (_defaultPadding / (Camera.main.aspect / defaultRatio16_9));
            grid.padding.right = (int) (_defaultPadding / (Camera.main.aspect / defaultRatio16_9));
            grid.padding.top = (int) (_defaultPadding / (Camera.main.aspect / defaultRatio16_9));
            grid.padding.bottom = (int) (_defaultPadding / (Camera.main.aspect / defaultRatio16_9));
            grid.spacing = _defaultSpacing*(Camera.main.aspect / defaultRatio16_9);
        }
    }
}