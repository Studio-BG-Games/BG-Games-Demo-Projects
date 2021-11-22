using System;
using DefaultNamespace;
using Factories;
using Infrastructure.Configs;
using Plugins.DIContainer;
using Services.Interfaces;
using UnityEngine;

namespace Mechanics.SketchBook
{
    public class LogicPrometerOnSketchBook : MonoBehaviour
    {
        [SerializeField] private Book _book;
        [SerializeField] private FactoryPrompter _factoryPrompter;
        
        [DI] private DataFinishedLevel _dataFinishedLevel;
        [DI] private ConfigLocalization _configLocalization;
        [DI] private IInput _input;

        private const string IdActionOfPrometerInSketchBook = "PrometerHasTalked";

        private bool IsHasTalkedInSketchBook => PlayerPrefs.GetInt(IdActionOfPrometerInSketchBook, 0) == 1;

        private void Awake()
        {
            if (_dataFinishedLevel.GetAllFinishedLevel().Count == 1 && !IsHasTalkedInSketchBook)
                Talk();
        }

        private void Talk()
        {
            _book.SetInteractable(false);
            _factoryPrompter.ChangeAt(FactoryPrompter.Type.Hello).Unhide(
                ()=>_factoryPrompter.Current.Say(_configLocalization.SketchBookHello,
                    ()=>_input.AnyInput+=OnAnyInput));
        }

        private void OnAnyInput()
        {
            _input.AnyInput -= OnAnyInput;
            PlayerPrefs.SetInt(IdActionOfPrometerInSketchBook, 1);
            _factoryPrompter.Current.Hide(()=>_book.SetInteractable(true));
        }
        
        public static void ZeroMe() => PlayerPrefs.SetInt(IdActionOfPrometerInSketchBook,0);
    }
}