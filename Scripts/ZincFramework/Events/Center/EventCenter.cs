using System;
using System.Collections.Generic;

namespace ZincFramework
{
    namespace Events
    {
        /// <summary>
        /// 事件中心模块，通过订阅和广播的形式来进行事件的传递 
        /// </summary>
        public static class EventCenter
        {
            private readonly static Dictionary<E_Event_Type, ZincEventBase> _eventDictionary = new Dictionary<E_Event_Type, ZincEventBase>();

            /// <summary>
            /// 使用事件中心模块的对象必须在自己销毁时取消已经的注册事件
            /// </summary>
            public static void Boardcast(E_Event_Type eventName)
            {
                if (_eventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    if(zincEventBase is ZincEvent zincEvent)
                    {
                        zincEvent.Invoke();
                    }
                }
            }

            /// <summary>
            /// 使用事件中心模块的对象必须在自己销毁时取消已经的注册事件
            /// </summary>
            public static void Boardcast<T>(E_Event_Type eventName, T t)
            {
                if (_eventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    if (zincEventBase is ZincEvent<T> zincEvent)
                    {
                        zincEvent.Invoke(t);
                    }
                }
            }

            /// <summary>
            /// 使用事件中心模块的对象必须在自己销毁时取消已经的注册事件
            /// </summary>
            public static void Boardcast<T, K>(E_Event_Type eventName, T arg1, K arg2)
            {
                if (_eventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    if (zincEventBase is ZincEvent<T, K> zincEvent)
                    {
                        zincEvent.Invoke(arg1, arg2);
                    }
                }
            }


            /// <summary>
            /// 使用事件中心模块的对象必须在自己销毁时取消已经的注册事件
            /// </summary>
            public static void Subscribe(E_Event_Type eventName, ZincAction action)
            {
                if (!_eventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    zincEventBase = new ZincEvent();
                    _eventDictionary.Add(eventName, zincEventBase);
                }

                (zincEventBase as ZincEvent).AddListener(action);
            }


            /// <summary>
            /// 使用事件中心模块的对象必须在自己销毁时取消已经的注册事件
            /// </summary>
            public static void Subscribe<T>(E_Event_Type eventName, ZincAction<T> action)
            {
                if (!_eventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    zincEventBase = new ZincEvent<T>();
                    _eventDictionary.Add(eventName, zincEventBase);
                }

                (zincEventBase as ZincEvent<T>).AddListener(action);
            }


            /// <summary>
            /// 使用事件中心模块的对象必须在自己销毁时取消已经的注册事件
            /// </summary>
            public static void Subscribe<T, K>(E_Event_Type eventName, ZincAction<T, K> action)
            {
                if (!_eventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    zincEventBase = new ZincEvent<T, K>();
                    _eventDictionary.Add(eventName, zincEventBase);
                }

                (zincEventBase as ZincEvent<T, K>).AddListener(action);
            }


            /// <summary>
            /// 使用事件中心模块的对象必须在自己销毁时取消已经的注册事件
            /// </summary>
            public static void Unsubscribe(E_Event_Type eventName, ZincAction action)
            {
                if (_eventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    (zincEventBase as ZincEvent).RemoveListener(action);
                }
            }

            /// <summary>
            /// 使用事件中心模块的对象必须在自己销毁时取消已经的注册事件
            /// </summary>
            public static void Unsubscribe<T>(E_Event_Type eventName, ZincAction<T> action)
            {
                if (_eventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    (zincEventBase as ZincEvent<T>).RemoveListener(action);
                }
            }


            /// <summary>
            /// 使用事件中心模块的对象必须在自己销毁时取消已经的注册事件
            /// </summary>
            public static void Unsubscribe<T, K>(E_Event_Type eventName, ZincAction<T, K> action)
            {
                if (_eventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    (zincEventBase as ZincEvent<T, K>).RemoveListener(action);
                }
            }


            /// <summary>
            /// 使用事件中心模块的对象必须在自己销毁时取消已经的注册事件
            /// </summary>
            public static void RemoveSubscription(E_Event_Type eventName)
            {
                if (_eventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    zincEventBase.RemoveAllListeners();
                    _eventDictionary.Remove(eventName);
                }
            }

            /// <summary>
            /// 使用事件中心模块的对象必须在自己销毁时取消已经的注册事件
            /// </summary>
            public static void ClearAllSubscription()
            {
                foreach(var @event in _eventDictionary.Values)
                {
                    @event.RemoveAllListeners(); 
                }

                _eventDictionary.Clear();
            }
        }
    }
}

