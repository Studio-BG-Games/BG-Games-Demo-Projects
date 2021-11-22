using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Firebase.Firestore;
using UnityEngine;

namespace Scripts.Serializables.Story
{
    [Serializable]
    public struct Story
    {
        public List<General> general;
        public List<Act> acts;
        public string title;
        public string id;
        public long timestamp;
        public List<string> items;
    }
}