using UnityEditor;

namespace Interface
{
    public interface IIDContainer
    {
        string ID { get; }

        void Regenerate();
    }
}