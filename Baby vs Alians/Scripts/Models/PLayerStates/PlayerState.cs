namespace Baby_vs_Aliens
{
    public abstract class PlayerState : IUpdateableRegular
    {
        #region Fields

        protected PlayerController _controller;

        #endregion


        #region IUpdateableRegular

        public abstract void UpdateRegular();

        #endregion


        #region Methods
        public abstract void Reset();

        #endregion
    }
}