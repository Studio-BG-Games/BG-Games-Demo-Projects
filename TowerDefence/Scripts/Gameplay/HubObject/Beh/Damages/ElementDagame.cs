using UnityEngine;

namespace Gameplay.HubObject.Beh.Damages
{
    [System.Serializable]
    public class ElementDagame
    {
        public TypeDamage TypeDamage => _typeDamage;
        public int Value => isRandom? Random.Range(_valueMin, _valueMax) : _valueMin;
        
        [SerializeField] private TypeDamage _typeDamage;
        [SerializeField] private bool isRandom;
        [Min(0)] [SerializeField] private int _valueMin;
        [Min(0)] [SerializeField] private int _valueMax;

        public ElementDagame(TypeDamage typeDamage, int valueMin, int max)
        {
            _typeDamage = typeDamage;
            if (valueMin < 0)
                valueMin = 0;
            if (max < 1)
                max = 1;
            _valueMax = max;
            _valueMin = valueMin;
        }
        
        public ElementDagame CloneWithNewValue(int value) => new ElementDagame(_typeDamage, _valueMin, _valueMax);

        public void OnValidate()
        {
            if (_valueMax <= _valueMin)
                _valueMax = _valueMin + 1;
        }
    }
}