using System;
using System.Collections;
using System.Collections.Generic;
using Factorys;
using Gameplay.HubObject.Data;
using Gameplay.StateMachine.GameScene;
using Gameplay.Units;
using Gameplay.Units.Beh;
using Pathfinding;
using Plugins.DIContainer;
using UnityEngine;

namespace Gameplay.GameSceneScript
{
    public class ControlAIUnit : MonoBehaviour
    {
        private List<Unit> _units = new List<Unit>();
        
        [DI] private FactoryUnit _factoryUnit;
        [DI] private GameSceneStateMachine _gameSceneStateMachine;

        private static Type FightStateType = typeof(FightState);

        private void Awake()
        {
            _factoryUnit.CreatedT += OnCreatedUnit;
            _gameSceneStateMachine.EnteredTo += OnEnteredToStaet;
        }

        private void OnEnteredToStaet(Type obj)
        {
            _units.RemoveAll(x => x == null);
            foreach (var unit in _units) SetActiveBTByState(obj, unit);
        }

        private void OnCreatedUnit(Unit obj)
        {
            UpdateZoneMove(obj);
            SetActiveBTByState(_gameSceneStateMachine.CurrentState, obj);
            _units.Add(obj);
        }

        private void UpdateZoneMove(Unit unit)
        {
            if(unit.MainDates.GetOrNull<Team>().Type==Team.Typ.Player)
                unit.ComponentShell.Get<ZoneMove>()?.UpdateCenter();
        }

        private void SetActiveBTByState(Type steyType, Unit unit)
        {
            //unit.GetComponent<BehaviorExecutor>().enabled = steyType == FightStateType;
        }
    }
}