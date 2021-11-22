using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

namespace Scripts.Serializables.User
{
    [Serializable]
    public struct UserCurrency
    {
        public int cash;
        public int cocktails;
        public int elixirs;
    }
}