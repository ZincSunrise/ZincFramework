using System;

namespace ZincFramework.Runtime.CompilerServices
{
    internal class ContinuationAction
    {
        public static Action<object> InvokeActionDelegate { get; } = Invoke;

        private static void Invoke(object state)
        {
            if (state is Action action)
            {
                action.Invoke();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
