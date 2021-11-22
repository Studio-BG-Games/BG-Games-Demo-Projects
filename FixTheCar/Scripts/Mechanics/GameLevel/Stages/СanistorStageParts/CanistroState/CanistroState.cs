using System;

namespace Mechanics.GameLevel.Stages.СanistorStageParts
{
    public abstract class CanistroState
    {
        private protected  readonly Canistro _canistro;

        public CanistroState(Canistro canistro) => _canistro = canistro;

        public abstract void Action(Action callback);
    }
}