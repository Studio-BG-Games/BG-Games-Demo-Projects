using System.Collections.Generic;
using Gameplay.HubObject.Beh.Damages;
using Plugins.HabObject;
using UnityEngine;

namespace Gameplay.HubObject.Beh
{
    [BehaviourButton("Combat/Attack/Container")]
    public class AttackContainerBeh : MonoBehaviour
    {
        [Header("Инчае только мин")]
        [Header("Если случаный урон, будет браться диапозон из мин до макс")]
        [SerializeField] private List<ElementDagame> ElementDagames;
        [HideInInspector][SerializeField]private HabObject Parent;

        public Damage GetDamage() => new Damage(Parent, ElementDagames);

        private void OnValidate()
        {
            if (!Parent)
            {
                Parent = GetComponentInParent<HabObject>();
                if(!Parent)
                    Debug.LogError($"{GetType().ToString()} - Должен быть расположен в чилдах у любого HabObject");
            }
            ElementDagames?.ForEach(x=>x.OnValidate());
        }
    }
}