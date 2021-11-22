using System;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

namespace Scripts.Serializables.User
{
    [Serializable]
    public struct User
    {
        public string email;
        public string username;

        public bool isNotNewUser;
        // isElixirOn перемещен в StoryProgress - уникальное поле для каждой истории.
        public UserCurrency currencies;
        public List<StoryProgress> progress;
        public List<StoryProgress> partialProgress;
    }
}
