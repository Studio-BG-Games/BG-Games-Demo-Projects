using Gameplay.Builds.Data;
using Gameplay.HubObject.Data;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Interfaces;
using Gameplay.Units.Data;
using Plugins.HabObject.GeneralProperty;
using UnityEngine;

namespace Gameplay.Units
{
    [RequireComponent(typeof(Description), typeof(Name), typeof(TypeMonster))]
    [RequireComponent(typeof(TargetContainer), typeof(Team), typeof(IdContainer))]
    [RequireComponent(typeof(SizeOnMap))]
    public class DatasUnit : MainDates
    {
        [ContextMenu("CheckItems")]
        private void Log()
        {
            string result = "";
            foreach (var v in _propDict)
            {
                result += v.Key + "\n";
            }
            Debug.Log(result);
        }
    }
}