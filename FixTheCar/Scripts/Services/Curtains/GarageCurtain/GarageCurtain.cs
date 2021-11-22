using System;
using Plugins.Interfaces;
using UnityEngine;

namespace Services.Curtains.GarageCurtain
{
    public class GarageCurtain : Curtain
    {
        [SerializeField] private float _duration;
        [SerializeField] private GarageDoor _leftDoor;
        [SerializeField] private GarageDoor _rightDoor;
        
        public override event Action Unfaded;
        
        public override void Fade(Action callback)
        {
            _leftDoor.Close(_duration);
            _rightDoor.Close(_duration, () => callback?.Invoke());
        }

        public override void Fade(Action callback, float duration)
        {
            _leftDoor.Close(duration);
            _rightDoor.Close(duration, ()=>callback?.Invoke());
        }

        public override void Unfade()
        {
            _leftDoor.Open(_duration);
            _rightDoor.Open(_duration);
            Unfaded?.Invoke();
        }

        public override void Transit(Action callback) => Fade(()=>{callback?.Invoke(); Unfade();});
    }
}