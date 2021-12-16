using Extension;
using Gameplay.Builds.Data;

namespace Gameplay.Builds
{
    public class CubeGhost : GhostView
    {
        public override void Init(SizeOnMap sizeOnMap)
        {
            var scale = sizeOnMap.Size.ToVector3XZ();
            scale.y = transform.localScale.y;
            transform.localScale = scale;
        }
    }
}