using System;
using System.Collections.Generic;
using Gameplay.HubObject.Beh.Attributes;
using Gameplay.HubObject.Data;
using Gameplay.Units;
using Plugins.DIContainer;
using Plugins.HabObject;
using UnityEngine;

namespace Factorys
{
    public class UnitContainer : MonoBehaviour
    {
        public event Action AllPlayerUnitDead;
        public event Action AllEnemyUnitDead;
        
        [DI] private FactoryUnit _factoryUnit;

        private List<Unit> _players = new List<Unit>();
        private List<Unit> _enemys = new List<Unit>();
        
        private void Awake() => _factoryUnit.CreatedT += OnUnitCreated;

        public void GoOverUnit(TypeUnit unit, Func<Unit, bool> callback)
        {
            var list = unit == TypeUnit.Player ? _players : _enemys;
            foreach (var unit1 in list)
            {
                if(callback.Invoke(unit1))
                    break;
            }
        }

        private void OnUnitCreated(Unit obj)
        {
            var team = obj.MainDates.GetOrNull<Team>();
            if(team.Type == Team.Typ.NonePlayer)
                _enemys.Add(obj);
            else
                _players.Add(obj);

            var h = obj.ComponentShell.Get<Health>();
            h.Dead += OnDeadUnit;
        }

        private void OnDeadUnit(HabObject obj)
        {
            _enemys.Remove((Unit)obj);
            _players.Remove((Unit)obj);
            if(!HasAnyPlayer)
                AllPlayerUnitDead?.Invoke();
            if(!HasAneEnemy)
                AllEnemyUnitDead?.Invoke();
        }

        public bool HasAnyPlayer
        {
            get
            {
                _players.RemoveAll(x => x == null);
                return _players.Count > 0;
            }
        }

        public bool HasAneEnemy {
            get
            {
                _enemys.RemoveAll(x => x == null);
                return _enemys.Count > 0;
            }
        }

        public enum TypeUnit
        {
            Player, Enemy
        }
    }
}