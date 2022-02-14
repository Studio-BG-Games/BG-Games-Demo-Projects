using System;

namespace Baby_vs_Aliens
{
    public interface IEnemy : IUpdateableRegular, IDisposable
    {
        bool IsDone { get; }
    }
}