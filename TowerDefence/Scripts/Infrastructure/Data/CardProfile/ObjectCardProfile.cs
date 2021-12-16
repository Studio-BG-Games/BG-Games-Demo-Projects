using System.Collections.Generic;
using DefaultNamespace.Infrastructure.Data.ParstOfCardProfiles;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.Infrastructure.Data
{
    public class ObjectCardProfile<T, TData> : ScriptableObject where T : Object where TData : SaveDataProfile
    {
        public T Target => _target;
        [SerializeField] protected T _target;
        [SerializeField] private TData _defaultData;
        [SerializeField] protected List<PartOfCardProfile> _parts;

        public TData CurrentData => _currentData;
        private TData _currentData;
        
        [SerializeField] private bool _setDefault;

        [ContextMenu("SetToDefault")]
        private void SetDefault()
        {
            var ser = new FireBaseProvider();
            ser.Connected += callback;
            ser.Connect();

            void callback()
            {
                ser.Connected -= callback;
                SaveNewData(ser, _defaultData);        
            }
        }
        
        public void UpdateData(SaveDataProvider IProvider)
        {
            //Debug.Log("Update data 1");
            var currentData = IProvider.GetOrNull<ObjectCardProfile<T, TData>, T, TData>(_target);
            //Debug.Log("Update data 2");
            if (currentData == null)
            {
                //Debug.Log("Update data 3");
                //Debug.Log($"load is null - {_target}");
                currentData = SaveNewData(IProvider, _defaultData);
                currentData = _defaultData;
            }
            else
            {
                //Debug.Log("Update data 4");
                //Debug.Log($"load is not null {currentData}");
            }
            //Debug.Log("Update data 5");
            _currentData = currentData;
        }

        public TData SaveNewData(SaveDataProvider IProvider, TData newData)
        {
            IProvider.Save<ObjectCardProfile<T, TData>, T, TData>(_target, newData);
            return _currentData = newData;
        }

        public Y GetFirstOrNull<Y>() where Y : PartOfCardProfile
        {
            foreach (var partOfCardProfile in _parts)
            {
                if (partOfCardProfile.GetType() == typeof(Y))
                    return partOfCardProfile as Y;
            }

            return null;
        }

        public List<Y> GetAll<Y>() where Y : PartOfCardProfile
        {
            List<Y> results = new List<Y>();
            _parts.ForEach(x =>
            {
                if(typeof(Y) == x.GetType())
                    results.Add(x as Y);
            });
            return results;
        }

        public void OnValidate()
        {
            if(_target==null)
                Debug.LogWarning($"{name} не имеет целевого объекта для карточки, FIX MEEEEEEEEEEEEEEEE");
            if (_setDefault)
            {
                SetDefault();
                _setDefault = false;
            }

            CustomOnValidate();
        }
        
        protected virtual void CustomOnValidate(){}
    }
}