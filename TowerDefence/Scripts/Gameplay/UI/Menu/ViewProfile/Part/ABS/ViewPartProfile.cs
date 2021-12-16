using System;
using System.Security.Cryptography;
using DefaultNamespace.Infrastructure.Data;
using Factorys;
using Gameplay.GameSceneScript;
using Plugins.HabObject;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace Gameplay.UI.Menu.Canvas
{
    public abstract class ViewPartProfile : MonoBehaviour
    {
        public abstract void View<T, TData>(ObjectCardProfile<T, TData> profileToView) where T : Object where TData : SaveDataProfile;
    }
}