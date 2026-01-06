using UnityEngine.Events;

namespace AtMycelia.Common
{
    public interface INumericResourceEventHandler
    {
        event UnityAction<INumericResourceHandler> MaxValueIncreased;
        event UnityAction<INumericResourceHandler> MaxValueDecreased;

        event UnityAction<INumericResourceHandler> CurrentValueIncreased;
        event UnityAction<INumericResourceHandler> CurrentValueDecreased;

        event UnityAction<INumericResourceHandler> MinValueIncreased;
        event UnityAction<INumericResourceHandler> MinValueDecreased;
    }
}