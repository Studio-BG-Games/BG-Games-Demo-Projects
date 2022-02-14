using UnityEngine;

namespace Baby_vs_Aliens
{
    public class RandomArenaGenerator
    {
        #region Methods

        public ArenaObjectType[,] CreateRandomArena(Vector2Int size)
        {
            var arena = new ArenaObjectType[size.x, size.y];

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    if (i == 0 || j == 0 || i == size.x - 1 || j == size.y - 1)
                    {
                        arena[i, j] = ArenaObjectType.Wall;
                    }
                    else if (i % 3 == 0 && j % 3 == 0 && i != size.x - 3 && j != size.y -3)
                    {
                        if (Random.value < 0.25f)
                        {
                            if (Random.value < 0.5f)
                            {
                                arena[i, j] = ArenaObjectType.ObstacleHor;
                                arena[i + 1, j] = ArenaObjectType.ObstacleHor;
                            }
                            else
                            {
                                arena[i, j] = ArenaObjectType.ObstacleVer;
                                arena[i, j + 1] = ArenaObjectType.ObstacleVer;
                            }
                        }
                    }
                }
            }

            return arena;
        }

        #endregion
    }
}