using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Gameplay.HubObject.Beh.Damages;
using Gameplay.HubObject.Beh.Effects;
using Gameplay.HubObject.Beh.Resistance;
using Gameplay.HubObject.Data;
using Gameplay.StateMachine.GameScene;
using Plugins.DIContainer;
using Plugins.HabObject;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.HubObject.Beh.Attributes
{
    [BehaviourButton("Stat/Health")]
    public class Health : AbsAttribute
    {
        [SerializeField] private HabObject _habObject;
        [Min(1)] [SerializeField] private int _maxBase;
        [Min(0)] [SerializeField] private int _current;

        private int _realMax;

        public int RealMax => _realMax;

        public int MaxBase => _maxBase;

        public int Current => _current;
        public event Action<float> NewValue;
        public event Action<HabObject> Dead;
        public event Action Damaged;
        public event Action<Damage> DamagedBy;
        public event Action Recovered;

        //Временный код, когда решим как правильно и у кого восстонавливать хп, удалить это и переделать!!!
        #region TempCode

        [DI] private GameSceneStateMachine _gameSceneState;

        protected override void CustomAwake()
        {
            _gameSceneState.EnteredTo += EnterToBuild;
        }

        private void EnterToBuild(Type obj)
        {
            if(!(obj is BuildState))
                return;
            if (_habObject.MainDates.GetOrNull<Team>().Type == Team.Typ.Player)
                _current = _realMax;
        }

        #endregion
        
        public void Damage(Damage damage)
        {
            if(_current<=0)
                return;
            if(EffectContainer)
                damage = (Damage)EffectContainer.MakeEffect(damage);
            damage.GoOverElement(e=>_current-=e.Value);
            DamagedBy?.Invoke(damage);
            Damaged?.Invoke();
            ClamAndEvented();
        }

        private void OnValidate()
        {
            if (!_habObject)
            {
                _habObject = GetComponentInParent<HabObject>();
                if(!_habObject)
                    Debug.LogError($"{GetType().ToString()} - Должен быть расположен в чилдах у любого HabObject");
            }
        }

        public void Recovery(int value)
        {
            _current += value;
            Recovered?.Invoke();
            ClamAndEvented();
        }

        private void ClamAndEvented()
        {
            _current = Mathf.Clamp(_current, 0, _realMax);
            NewValue?.Invoke((float) _current / _realMax);
            if (_current == 0)
                Dead?.Invoke(_habObject);
        }

        public override IModificateData GetCurrent()
        {
            var result = new ModificateData(_current, _maxBase);
            return result;
        }

        protected override void OnUpdateBuff(IModificateData modificateData)
        {
            var data = (ModificateData) modificateData;
            _realMax = data.Max <= 0 ? 1 : data.Max;
            _current = Mathf.Clamp(data.Current <= 0 ? 1 : data.Current, 0, _realMax); 
            ClamAndEvented();
        }

        public class ModificateData : IModificateData
        {
            public ModificateData(int current, int max)
            {
                Current = current;
                Max = max;
            }

            public int Current;
            public int Max;
        }
    }
}