using System;
using DefaultNamespace.Ad.Buttons;
using Gameplay.HubObject.Beh.Effects.Realise;
using Gameplay.StateMachine.GameScene;
using Plugins.DIContainer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.GameSceneScript
{
    public class TimeScaler_MonoInGame : MonoBehaviour
    {
        [DI] private TimeScaler _timeScaler;
        [DI] private GameSceneStateMachine _gameStateMachine;

        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private string _postfix;

        private State _currentStaet;
        
        private void Awake()
        {
            _timeScaler.NewSpeed += ViewSpeed;
            _gameStateMachine.EnteredTo += OnEnterNewStateGame;
        }

        private void OnEnable() => _button.onClick.AddListener(ChangeSpeed);

        private void OnDisable() => _button.onClick.RemoveAllListeners();

        private void OnEnterNewStateGame(Type obj)
        {
            _currentStaet = new First();
            _timeScaler.SetCustomSpeed(_currentStaet.Speed());
        }

        private void ChangeSpeed()
        {
            Debug.Log("Change speed");
            _currentStaet = _currentStaet.NextState();
            _timeScaler.SetCustomSpeed(_currentStaet.Speed());
        }

        private void ViewSpeed(float obj) => _label.text = $"{obj}{_postfix}";
        
       private abstract class State
       {
           public abstract float Speed();
           public abstract State NextState();
       }
       
       private class First : State
       {
           public override float Speed() => 1;
           public override State NextState() => new Second();
       }
       
       private class Second : State
       {
           public override float Speed() => 1.5f;
           public override State NextState() => new Third();
       }
       
       private class Third : State
       {
           public override float Speed() => 2;
           public override State NextState() => new First();
       }
    }
}