using System;
using System.Collections.Generic;

namespace Scripts.Serializables.Story
{
    [Serializable]
    public struct Act
    {
        public int id;
        public string title;
        public List<Record> records;
    }
}