using DefaultNamespace.Infrastructure.Data;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.UI.Menu.Canvas
{
    public interface IOpenViewProfilePanel
    {
        void Open<Card, T, TData>(ViewProfile<Card, T, TData> profile, Card card) where Card : ObjectCardProfile<T, TData> where T : Object where TData : SaveDataProfile;
        void Close();
    }
}