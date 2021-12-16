using Newtonsoft.Json;
using UnityEngine;

namespace DefaultNamespace.Ad
{
    public class AdIdRewardedWithNameAndCount : AdIdRewarded
    {
        [JsonIgnore]public string NameAward => _nameAward;
        [JsonIgnore]public int Count => _count;
        
        [JsonProperty][SerializeField] private string _nameAward;
        [JsonProperty][SerializeField] private int _count;

        public AdIdRewardedWithNameAndCount(string name) : base(name)
        {
        }

        public override string ToString()
        {
            return $"nameAward: {_nameAward}, count: {_count}, andorid: {_androindId}, iphone: {_iosID}, name {Name}";
        }
    }
}