using DefaultNamespace.Ad;

namespace DefaultNamespace.Infrastructure.Data
{
    public interface IActionFireBase
    {
        public void SendFeedBack(string mes);
        public AdIdRewarded GetRewardedID(string name);
        public AdIdRewardedWithNameAndCount GetExtendedRewardedId(string name);

    }
}