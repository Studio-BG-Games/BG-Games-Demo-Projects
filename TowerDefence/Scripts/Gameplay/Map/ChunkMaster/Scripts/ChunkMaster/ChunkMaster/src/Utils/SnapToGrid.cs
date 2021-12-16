/* The snap to grid class, allows users to easily
 * snap values to a grid given the grid width.
 * 
 * Author: Corey St-Jacques
 */

using System;

namespace Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Utils
{
    /// <summary>
    /// A simple utility class for snapping points 
    /// to a grid in world or local space.
    /// </summary>
    public static class SnapToGrid
    {
        /// <summary>
        /// Snap a single value to the grid.
        /// </summary>
        /// <param name="num">The current value you would like to snap.</param>
        /// <param name="value">The grid size you would like top snap to.</param>
        /// <returns>double</returns>
        public static double snap(double num, int value = 1)
        {
            double tmp;
            tmp = Math.Floor(num / value) * value;
            return tmp;
        }

        /// <summary>
        /// Snap to a 3 dimensional grid given a Point object.
        /// </summary>
        /// <param name="hit_coor">The current Point object you would like to snap to the grid.</param>
        /// <param name="value">The size of the grid you would like to snap to.</param>
        /// <returns>Point</returns>
        public static Point snap(Point hit_coor, int value = 1)
        {
            return new Point(
                snap(hit_coor.x, value), 
                snap(hit_coor.y, value), 
                snap(hit_coor.z, value)
                );
        }
    }
}
