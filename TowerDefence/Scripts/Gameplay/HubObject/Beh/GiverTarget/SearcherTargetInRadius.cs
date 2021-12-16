using System;
using System.Collections.Generic;
using Gameplay;
using Gameplay.Builds;
using Gameplay.Builds.Data.Marks;
using Gameplay.HubObject.Beh.Attack;
using Gameplay.HubObject.Beh.Attributes;
using Gameplay.HubObject.Beh.GiverTarget.PartSearchInTarget;
using Gameplay.HubObject.Data;
using Gameplay.Units;
using Plugins.HabObject;
using Plugins.PhysicShell;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace BehTreeBrick.Action
{
    [BehaviourButton("Combat/Attack/GiveTarget/From radius")]
    public class SearcherTargetInRadius : GiveTarget
    {
        public event Action<HabObject> NewObject;
        public event Action<HabObject> ObjectDelete;
        
        [SerializeField] private TriggerShell _triggerShell;
        [SerializeField] private TypeSerach _typeSerach;
        [SerializeField] private HabObject _habObject;
        [RequireInterface(typeof(IPartSearchRadius))][SerializeField] private Object[] _parts;

        private IPartSearchRadius[] _partsIntefaces;
        public bool HasAnyTargetInZone => _habobjects == null ? false : _habobjects.Count > 0;
        
        private List<HabObject> _habobjects=new List<HabObject>();

        private void Awake()
        {
            _partsIntefaces = new IPartSearchRadius[_parts.Length];
            for (int i = 0; i < _parts.Length; i++)
            {
                _partsIntefaces[i] = (IPartSearchRadius) _parts[i];
            }
            if (_habObject != null)
                Init(_habObject);
            _triggerShell.Enter += OnEnter;
            _triggerShell.Exit += OnExit;
        }

        public void Init(HabObject habObject)
        {
            for (int i = 0; i < _partsIntefaces.Length; i++)
                _partsIntefaces[i] = _partsIntefaces[i].Clone(habObject);
        }

        [ContextMenu("Debug Taget")]
        private void DebugAllTargetInRadius()
        {
            _habobjects.ForEach(x=>Debug.Log(x.name, x));
        }
        
        public override List<HabObject> All()
        {
            _habobjects.RemoveAll(x => x == null);
            if(_habobjects==null)
                _habobjects=new List<HabObject>();
            return _habobjects;
        }

        public override HabObject GetOne()
        {
            return GetTargetOrNull();
        }
        
        private void OnEnter(Collider obj)
        {
            if(!obj.TryGetComponent(out HabObject hab))
                return;
            if(!hab.GetComponentInChildren<SelectorHabObject>())
                return;
            if(hab.MainDates.GetOrNull<PropMark>())
                return;
            if(_partsIntefaces!=null)
                foreach (var part in _partsIntefaces)
                {
                    if(part.IsCorrect(hab)==ResultCheck.ToDelete)
                        return;
                }
            if (hab.ComponentShell.TryGet<Health>(out var H)) 
                H.Dead += DeadObject;
            else
                return;
            if (H.Current<=0)
            {
                H.Dead -= DeadObject;
                return;
            }
            _habobjects.Add(hab);
            NewObject?.Invoke(hab);
        }

        private void DeadObject(HabObject obj)
        {
            obj.ComponentShell.Get<Health>().Dead -= DeadObject;
            _habobjects.Remove(obj);
        }

        private void OnExit(Collider obj)
        {
            if (obj.gameObject.TryGetComponent<HabObject>(out var hab))
            {
                _habobjects.Remove(hab);
                ObjectDelete?.Invoke(hab);
            }
        }

        private HabObject GetTargetOrNull()
        {
            switch (_typeSerach)
            {
                case TypeSerach.Nearst:
                    return GetNearstTarget();
                case TypeSerach.Longest:
                    return GetLongestTarget();
                case TypeSerach.Random:
                    if (_habobjects!=null && _habobjects.Count>0)
                        return _habobjects[Random.Range(0, _habobjects.Count)];
                    else
                        return null;
                default: throw new Exception("some thind go wrond");
            }
        }

        private HabObject GetLongestTarget()
        {
            float lastDistance = 0;
            HabObject lastObject = null;
            _habobjects?.ForEach(x =>
            {
                if (x)
                {
                    var dis = Vector3.Distance(_habObject.transform.position, x.transform.position);
                    if (dis > lastDistance)
                    {
                        lastDistance = dis;
                        lastObject = x;
                    }
                }
            });
            return lastObject;
        }
        
        private HabObject GetNearstTarget()
        {
            float lastDistance = 50000000;
            HabObject lastObject = null;
            _habobjects?.ForEach(x =>
            {
                if (x)
                {
                    var dis = Vector3.Distance(_habObject.transform.position, x.transform.position);
                    if (dis < lastDistance)
                    {
                        lastDistance = dis;
                        lastObject = x;
                    }
                }
            });
            return lastObject;
        }
        

        private enum TypeSerach
        {
            Nearst, Longest, Random
        }
        
        
        public enum ResultCheck
        {
            Correct, ToDelete
        }
        
    }
}