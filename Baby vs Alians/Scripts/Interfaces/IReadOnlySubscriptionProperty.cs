using System;

namespace Baby_vs_Aliens
{
    public interface IReadOnlySubscriptionProperty<T>
    {
        T Value { get; }
        void SubscribeOnChange(Action<T> subscriptionAction);
        void UnSubscribeOnChange(Action<T> unsubscriptionAction);
    }
}
