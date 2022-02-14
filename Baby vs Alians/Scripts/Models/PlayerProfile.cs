namespace Baby_vs_Aliens
{
    public class PlayerProfile
    {
        public SubscriptionProperty<GameState> CurrentState { get; }
        private int _currentLevel;
        public CharacterCustomizationInfo CustomizationInfo;

        public int CurrentLevel
        {
            get => _currentLevel;
            set => _currentLevel = value >= 0 ? value : 0;
        }

        public PlayerProfile()
        {
            CurrentState = new SubscriptionProperty<GameState>();
        }
    }
}