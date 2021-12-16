using UnityEngine;
using UnityEngine.Tilemaps;

namespace Gameplay.GameSceneScript
{
    public class GridBuildTileMap : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        [SerializeField] private Tilemap _tilemap;

        public Grid Grid => _grid;

        public Tilemap Tilemap => _tilemap;
        }
}