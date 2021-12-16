using System;
using Interface;
using Plugins.HabObject;

namespace DefaultNamespace.Infrastructure.Data
{
    public class AbsCardProfile<T, TData> : ObjectCardProfile<T, TData> where T : HabObject where TData : SaveDataProfile
    {
        
    }
}