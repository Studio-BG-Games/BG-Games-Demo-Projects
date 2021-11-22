using System;
using UnityEngine;

namespace Mechanics.Interfaces
{
    public abstract class AbsTransitPlayer : MonoBehaviour
    {
        public abstract void Transit(Player player, Vector3 pointCamera,  Action callback);

        public void InstantaneousTransit(Player player, Vector3 pointCamera, Vector3 pointPlayer, Action callback)
        {
            player.transform.position = pointPlayer;
            Camera.main.transform.position = pointCamera;
            callback?.Invoke();
        }  
    }
}