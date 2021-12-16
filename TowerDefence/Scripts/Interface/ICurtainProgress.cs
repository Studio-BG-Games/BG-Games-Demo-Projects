using Plugins.Interfaces;

namespace Interface
{
    public interface ICurtainProgress : ICurtain
    {
        public void SetProgress(float normalProgres);
    }
}