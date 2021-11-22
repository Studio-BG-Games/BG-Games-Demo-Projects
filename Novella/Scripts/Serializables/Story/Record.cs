using System;
using System.Collections.Generic;
namespace Scripts.Serializables.Story
{
    [Serializable]
    public struct Record
    {
        public int row;
        public string[] qline;
        public string type;
        public string text;
        public string english;//replacement for text
        public string[] sound;
        public string background;
        public string[] backgroundOptions;
        public string npc;
        public string[] npcOptions;
        public int timer;
        public string emotion;
        public string effect;        
        public Condition condition;        
        public string item;
        public Option[] options;
    }
}