using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DefaultNamespace.Infrastructure.Data;
using Extension;
using Factorys;
using Gameplay.Builds;
using Gameplay.Builds.Beh;
using Gameplay.Units;
using Gameplay.Units.Beh;
using Gameplay.Units.Data;
using Gameplay.Waves;
using Infrastructure.SceneStates;
using Plugins.DIContainer;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.GameSceneScript
{
    public class InterpreterWaveLevel : MonoBehaviour
    {
        [DI] private GameSceneData _gameSceneData;
        [DI] private BuildContainer _buildContainer;
        [DI] private FactoryBuild _factoryBuild;
        [DI] private UnitContainer _unitContainer;
        [DI] private IGold _gold;
        [DI] private SaveCurrentWave _saveCurrentWave;


        public event Action Win;
        public event Action Lose;
        
        private Level _level;

        [DI]
        private void Init() => _level = _gameSceneData.Level;

        public bool LevelIsComplited => _level.Waves.Length == _indexCurrentWave;
        public int IndexCurrentWave => _indexCurrentWave;
        
        private int _indexCurrentWave = 0;
        private List<Portal> _portals;
        private List<Vector3> _allPoints = new List<Vector3>();
        private List<Vector3> _avaiblePoint;
        private Coroutine _actionSpawn;

        int XSize =>_gameSceneData.DataMap.ChunkSettings.ChunkSize * _gameSceneData.DataMap.ChunkSettings.SectorSize *
                    _gameSceneData.DataMap.MapSettings.Size.x;
        int YSize => _gameSceneData.DataMap.ChunkSettings.ChunkSize * _gameSceneData.DataMap.ChunkSettings.SectorSize *
                     _gameSceneData.DataMap.MapSettings.Size.y;
        
        private void Awake()
        {
            for (int x = 0; x < XSize; x++)
            {
                for (int y = 0; y < YSize; y++)
                {
                    _allPoints.Add(new Vector3(x,0,y));
                }
            }
            _indexCurrentWave = _saveCurrentWave.SaveLastNubmerWave;
        }

        public void StartNextWave()
        {
            Wave wave = _level.Waves[_indexCurrentWave];
            _avaiblePoint = GetAvaibelPoint(wave);
            _buildContainer.NeksusDead += OnNeksusDead;
            _actionSpawn =  StartCoroutine(StartWave(wave));
        }

        private void OnEnemyDead()
        {
            UnSubscribe();
            Win?.Invoke();
        }

        private void OnNeksusDead(Build obj) => Lose?.Invoke();

        private void UnSubscribe()
        {
            _buildContainer.NeksusDead -= OnNeksusDead;
            _unitContainer.AllEnemyUnitDead -= OnEnemyDead;
        }

        private List<Vector3> GetAvaibelPoint(Wave wave)
        {
            var neksusPos = _buildContainer.Neksus.transform.position;
            List<Vector3> pointToExute = new List<Vector3>();
            for (int x = (int)neksusPos.x-wave.MinDistanceToNeksus; x < (int)neksusPos.x+wave.MinDistanceToNeksus ; x++)
            {
                for (int y = (int)neksusPos.y-wave.MinDistanceToNeksus; y < (int)neksusPos.y+wave.MinDistanceToNeksus; y++)
                {
                    pointToExute.Add(new Vector3(x,0,y));
                }
            }

            return _allPoints.Except(pointToExute).ToList();
        }

        private IEnumerator StartWave(Wave wave)
        {
            _portals = SpawnPortal(wave.CountPortal, _level.Portal);
            List<Unit> enemyTemplate = new List<Unit>();
            wave.ElementWaves.ForEach(x=>
            {
                var e = x.Get();
                for (int i = 0; i < e.Count; i++)
                {
                    enemyTemplate.Add(e.Template);
                }
            });
            
            List<TypeMonster.Fraction> _fractions = new List<TypeMonster.Fraction>();
            enemyTemplate.ForEach(x=>
            {
                var frac = x.MainDates.GetOrNull<TypeMonster>();
                if(!_fractions.Contains(frac.Frac))
                    _fractions.Add(frac.Frac);
            });
            TypeMonster.Fraction choiseFraction = _fractions[Random.Range(0, _fractions.Count)];
            enemyTemplate = enemyTemplate.Where(x => x.MainDates.GetOrNull<TypeMonster>().Frac == choiseFraction).ToList();
            
            while (enemyTemplate.Count>0)
            {
                for (var i = 0; i < _portals.Count; i++)
                {
                    if(_portals[i].TrySpawn(enemyTemplate[0]))
                        enemyTemplate.RemoveAt(0);
                    if(enemyTemplate.Count==0)
                        break;
                }
                if(enemyTemplate.Count>0)
                    yield return new WaitForSeconds(wave.CoolDownSpawn);
            }

            var boss = wave.Boss.GetBossOrNull();
            if (boss)
                TrySpawnBoss(boss, _portals);
            _gold.Add(wave.Award);
            _indexCurrentWave++;
            _unitContainer.AllEnemyUnitDead += OnEnemyDead;
            _portals.ForEach(x=>Destroy(x.GetComponentInParent<Build>().gameObject));
            _portals = new List<Portal>();
        }

        private void TrySpawnBoss(Unit boss, List<Portal> portals)
        {
            foreach (var portal in portals)
            {
                if(portal.TrySpawn(boss))
                    return;
            }
        }

        private List<Portal> SpawnPortal(int waveCountPortal, Build levelPortal)
        {
            List<Portal> result = new List<Portal>();
            _factoryBuild.CreatedT += OnCreated;
            for (int i = 0; i < waveCountPortal; i++)
            {
                bool resultSpawn = false;
                int indexTry = 0;
                while (!resultSpawn && indexTry < 10)
                {
                    var randomPoint = _avaiblePoint.Random();
                    resultSpawn = _factoryBuild.Spawn(levelPortal, randomPoint, GetRandomPoint());
                    indexTry++;
                }
            }
            _factoryBuild.CreatedT -= OnCreated;
            
            return result;
            
            void OnCreated(Build portal)
            {
                result.Add(portal.ComponentShell.Get<Portal>());
                portal.ComponentShell.Get<Portal>().Init();
            }
        }

        private Vector3 GetRandomPoint()
        {
            float[] angels = new float[4]{0,90,180,270};
            return new Vector3(0,angels[Random.Range(0,4)],0);
        }
    }
}