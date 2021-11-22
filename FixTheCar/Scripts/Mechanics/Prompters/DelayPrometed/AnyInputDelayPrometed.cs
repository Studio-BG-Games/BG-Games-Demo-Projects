using System;
using System.Collections.Generic;
using Plugins.DIContainer;
using Services.Interfaces;
using UnityEngine;

namespace Mechanics.Prompters.Interfaces
{
    [CreateAssetMenu(menuName = "Prometed/delay type/any input", order = 51)]
    public class AnyInputDelayPrometed : AbsDelayPrometed
    {
        private List<ShellCallback> _shellCallbacks;
        
        public override void Activated(Action callback)
        {
            if(_shellCallbacks==null) _shellCallbacks = new List<ShellCallback>();
            IInput input = DiBox.MainBox.ResolveSingle<IInput>();
            _shellCallbacks.Add(new ShellCallback(callback, input));
            _shellCallbacks[_shellCallbacks.Count - 1].Finished += OnFinishedShell;
        }

        private void OnFinishedShell(ShellCallback obj)
        {
            obj.Finished -= OnFinishedShell;
            _shellCallbacks.Remove(obj);
        }

        private class ShellCallback
        {
            public Action<ShellCallback> Finished;
            
            private Action _callback;
            private IInput _input;

            public ShellCallback(Action callback, IInput input)
            {
                _callback = callback;
                _input = input;
                _input.AnyInput += ActivatedCallback;
            }

            private void ActivatedCallback()
            {
                _input.AnyInput -= ActivatedCallback;
                _callback?.Invoke();
                Finished?.Invoke(this);
            }
        }
    }
}