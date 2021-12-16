using Gameplay.Units;

namespace Gameplay.Waves
{
    public class CountEnemy
    {
        public int Count;
        public Unit Template;

        public CountEnemy(int count, Unit template)
        {
            Count = count;
            Template = template;
        }
    }
}