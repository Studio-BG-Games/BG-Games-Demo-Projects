using System;
using Firebase.Extensions;
using Firebase.Messaging;
using UnityEngine;

namespace DefaultNamespace
{
    public class NotificationByFireBase : MonoBehaviour
    {
        public void Init() {
            Debug.Log("Subscribe to firebase mes");

            Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
            Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
            
            FirebaseMessaging.SubscribeAsync("News");
            FirebaseMessaging.SubscribeAsync("Other");
        }

        public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token) {
            UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
        }

        public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e) {
            UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
        }
    }
}