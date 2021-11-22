using System;
using Infrastructure.Configs;
using Infrastructure.LevelState.States;
using Mechanics.Garage;
using Plugins.DIContainer;
using Plugins.GameStateMachines;
using Plugins.Interfaces;
using Services.Curtains.GarageCurtain;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.LevelState.SceneScripts.Garages
{
    public class GarageChangeStateGame : MonoBehaviour
    {
        [SerializeField] private Button _exitFromGarageButton;
        [SerializeField] private Button _buttonToSketchBook;
        [SerializeField] private GarageCurtain _garageCurtain;
        
        [DI] private SelectorCar _selectorCar;
        [DI] private LevelStateMachine _levelStateMachine;
        [DI] private Curtain _curtain;

        private void Awake()
        {
            _selectorCar.NewCarSelect += OnNewCarSelect;
            _exitFromGarageButton.onClick.AddListener(OnExitGarage);
            _buttonToSketchBook.onClick.AddListener(EnterToScketchBook);
        }

        private void EnterToScketchBook() => 
            _curtain.Fade(()=>_levelStateMachine.Enter<SketchbookScene, ConfigLevel>(null), 0.15f);

        private void OnExitGarage()
        {
            TurnOffAllButton();
            _selectorCar.Off();
            _garageCurtain.Fade(()=>_curtain.Fade(()=>_levelStateMachine.Enter<MainMenu>()));
        }

        private void StartGame()
        {
            TurnOffAllButton();
            _selectorCar.Off();
            _curtain.Fade(()=>_levelStateMachine.Enter<GameScene, ConfigLevel>(_selectorCar.SelectedCar.LevelConfigCar));
        }

        private void TurnOffAllButton()
        {
            _buttonToSketchBook.interactable = false;
            _exitFromGarageButton.interactable = false;
        }

        private void OnDestroy() => _selectorCar.NewCarSelect -= OnNewCarSelect;

        private void OnNewCarSelect(Car obj)
        {
            if(obj)
                StartGame();
        }
    }
}