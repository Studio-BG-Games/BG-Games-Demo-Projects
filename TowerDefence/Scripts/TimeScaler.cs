using System;
using UnityEngine;

namespace DefaultNamespace.Ad.Buttons
{
    public class TimeScaler
    {
        public float CurrentSpeed => Time.deltaTime;
        
        public event Action<float> NewSpeed;

        public void Stop() => SetCustomSpeed(0);
        public void Contiun() => SetCustomSpeed(1);

        public void SetCustomSpeed(float speed)
        {
            if (speed < 0)
            {
                Debug.LogError("speed for game set less then zero, check callback of error");
                speed = 1;
            }
            
            Time.timeScale = speed;
            NewSpeed?.Invoke(speed);
        }
    }
}