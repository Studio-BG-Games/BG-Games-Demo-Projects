namespace Gameplay.StateMachine.GameScene
{
    public abstract class GameSceneState
    {
        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update();
    }
}