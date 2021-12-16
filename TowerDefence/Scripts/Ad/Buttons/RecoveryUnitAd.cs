using System;
using System.Collections.Generic;
using DefaultNamespace.Infrastructure.Data;
using Factorys;
using Gameplay.Builds;
using Gameplay.GameSceneScript;
using Gameplay.HubObject.Data;
using Gameplay.StateMachine.GameScene;
using Gameplay.Units.Beh;
using GoogleMobileAds.Api;
using Infrastructure.ConfigData;
using Infrastructure.SceneStates;
using Plugins.DIContainer;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace.Ad.Buttons
{
    public class RecoveryUnitAd : AdButton
    {
        [DI] private GameSceneData _sceneData;
        [DI] private GameSceneStateMachine _gameSceneStateMachine;
        [DI] private TimeScaler _timeScaler;
        [DI] private AvaibleHabToCreate _avaibleHabToCreate;
        [DI] private FactoryUnit _factoryUnit;
        [DI] private IActionFireBase _actionFireBase;
        [DI] private ConfigGame _configGame;
        
        private List<PairIdAndPos> _idPos = new List<PairIdAndPos>();
        
        private void Awake()
        {
            _gameSceneStateMachine.EnteredTo += OnEnterSomeState;
            _idPos = new List<PairIdAndPos>();
        }

        private void OnEnterSomeState(Type obj)
        {
            if (obj == typeof(FightState))
            {
                OffButton();
                _idPos.Clear();
                foreach (var unit in _avaibleHabToCreate.PlayerUnitOnField)
                {
                    var id = unit.MainDates.GetOrNull<IdContainer>().ID;
                    var zone = unit.ComponentShell.Get<ZoneMove>();
                    _idPos.Add(new PairIdAndPos(id, zone.Center, zone.Rotate));
                }

                _avaibleHabToCreate.AllPlayerUnitDestroy += AllPlayerUnitDestroy;
            }
            else
            {
                _idPos.Clear();
                OffButton();
                _avaibleHabToCreate.AllPlayerUnitDestroy -= AllPlayerUnitDestroy;
            }
            
            void AllPlayerUnitDestroy()
            {
                _avaibleHabToCreate.AllPlayerUnitDestroy -= AllPlayerUnitDestroy;
                OnButton();
                MakeReobserver();
            }
        }
        

        private void OnDestroy() => _gameSceneStateMachine.EnteredTo -= OnEnterSomeState;

        protected override void OnLoadAbs() => OffButton();

        protected override void OnFailLoad(object sender, AdFailedToLoadEventArgs e) => OffButton();

        protected override string GetID() => _actionFireBase.GetRewardedID("RecoveryUnitAd").GetId();

        protected override void OnEarnedReward(object sender, Reward e)
        {
            var chance = _sceneData.Level.ChangeToRecoveryUnit;
            var prevCheckCost = _factoryUnit.ToCheckCost;
            _factoryUnit.ToCheckCost = false;
            foreach (var pair in _idPos)
            {
                if(!Try())
                    continue;
                _factoryUnit.Spawn(_configGame.ConfigContainerUnit.GetTByIdOrNull(pair.ID), pair.Position, pair.Angel);
            }
            _factoryUnit.ToCheckCost = prevCheckCost;
            bool Try() => Random.Range(0, 1) < chance;
        }

        protected override void OnFaildShow(object sender, AdErrorEventArgs e) => OffButton();

        protected override void OnCloseAd(object sender, EventArgs e) => OffButton();

        protected override void OnOpend(object sender, EventArgs e) => _timeScaler.Stop();

        private class PairIdAndPos
        {
            public string ID { get; }
            public Vector3 Position { get; }

            public PairIdAndPos(string id, Vector3 pos, Vector3 angel)
            {
                ID = id;
                Position = pos;
                Angel = angel;
            }

            public Vector3 Angel { get; }
        }
    }
}