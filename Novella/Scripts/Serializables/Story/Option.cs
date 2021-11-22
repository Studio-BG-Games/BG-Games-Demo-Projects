using System;

namespace  Scripts.Serializables.Story
{
    [Serializable]
    public class Option
    {
        public string[] qline;
        public string text;
        public string english;
        public int points;
        public string effect;
        public int price;
        public string item;
        public int timer;
    }
}
