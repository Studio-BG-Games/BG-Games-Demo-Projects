using System;
using System.Reflection;
using Gameplay.HubObject.Data;
using Plugins.HabObject;
using UnityEngine;

namespace Extension
{
    public static class HelperID
    {
        public static void GenerateID(IdContainer brick)
        {
            string id = Guid.NewGuid().ToString();

            var firld = brick.GetType().GetField("_habObjecth", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default);
            var valut = firld.GetValue(brick);
            var name = ((HabObject)valut).name;
            var newId = $"{name}+{id}";
            brick.GetType().GetField("_id", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(brick, newId);
        }
    }
}