using System;
using System.Collections.Generic;


namespace Scripts.Serializables.Story
{
    [Serializable]
    public struct Condition
    {
        public string target;
        public string operand;
        public int value;
    }
}
