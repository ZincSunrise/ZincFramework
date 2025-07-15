// Portions of this code are adapted from the UniTask project (https://github.com/Cysharp/UniTask)
// Copyright (c) Cysharp, Inc.
// Licensed under the MIT License.

using System;
using System.Threading;

namespace ZincFramework.Threading.Tasks
{
    /// <summary>
    /// 用于打印Task中代码的报错
    /// </summary>
    public static class ZincTaskExeptionPublisher
    {
        public static event Action<Exception> UnobservedException;

        public static bool PropagateOperationCanceledException { get; set; } = false;

#if UNITY_2018_3_OR_NEWER
        public static UnityEngine.LogType WriteLogType { get; private set; } = UnityEngine.LogType.Exception;


        public static bool DispatchUnityMainThread { get; private set; } = true;

        // cache delegate.
        private static readonly SendOrPostCallback _handleException = InvokeUnobservedTaskException;

        private static void InvokeUnobservedTaskException(object state)
        {
            UnobservedException.Invoke(state as Exception);
        }
#endif
        internal static void PublishUnobservedException(Exception ex)
        {
            if (ex != null)
            {
                if (!PropagateOperationCanceledException && ex is OperationCanceledException)
                {
                    return;
                }

                if (UnobservedException != null)
                {
#if UNITY_2018_3_OR_NEWER
                    if (!DispatchUnityMainThread || Thread.CurrentThread.ManagedThreadId == ZincTaskLoopHelper.MainThreadId)
                    {
                        // allows inlining call.
                        UnobservedException.Invoke(ex);
                    }
                    else
                    {
                        // Post to MainThread.
                        ZincTaskLoopHelper.UnitySynchronizationContext.Post(_handleException, ex);
                    }
                }

#else
                    Console.WriteLine("UnobservedTaskException: " + ex.ToString());
#endif
                else
                {
#if UNITY_2018_3_OR_NEWER
                    string message = null;
                    if (WriteLogType != UnityEngine.LogType.Exception)
                    {
                        message = "UnobservedTaskException: " + ex.ToString();
                    }
                    switch (WriteLogType)
                    {
                        case UnityEngine.LogType.Error:
                            UnityEngine.Debug.LogError(message);
                            break;
                        case UnityEngine.LogType.Assert:
                            UnityEngine.Debug.LogAssertion(message);
                            break;
                        case UnityEngine.LogType.Warning:
                            UnityEngine.Debug.LogWarning(message);
                            break;
                        case UnityEngine.LogType.Log:
                            UnityEngine.Debug.Log(message);
                            break;
                        case UnityEngine.LogType.Exception:
                            UnityEngine.Debug.LogException(ex);
                            break;
                        default:
                            break;
                    }
#else
                    Console.WriteLine("UnobservedTaskException: " + ex.ToString());
#endif
                }
            }
        }
    }
}
