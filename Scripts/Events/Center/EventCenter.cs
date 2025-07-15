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
            private readonly static Dictionary<EventType, ZincEventBase> _eventDictionary = new Dictionary<EventType, ZincEventBase>();

            private readonly static Dictionary<string, ZincEventBase> _strEventDictionary = new Dictionary<string, ZincEventBase>();

            /// <summary>
            /// 使用事件中心模块的对象必须在自己销毁时取消已经的注册事件
            /// </summary>
            public static void Boardcast(EventType eventName)
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
            public static void Boardcast<T>(EventType eventName, T t)
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
            public static void Boardcast<T, K>(EventType eventName, T arg1, K arg2)
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
            public static void Subscribe(EventType eventName, ZincAction action)
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
            public static void Subscribe<T>(EventType eventName, ZincAction<T> action)
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
            public static void Subscribe<T, K>(EventType eventName, ZincAction<T, K> action)
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
            public static void Unsubscribe(EventType eventName, ZincAction action)
            {
                if (_eventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    (zincEventBase as ZincEvent).RemoveListener(action);
                }
            }

            /// <summary>
            /// 使用事件中心模块的对象必须在自己销毁时取消已经的注册事件
            /// </summary>
            public static void Unsubscribe<T>(EventType eventName, ZincAction<T> action)
            {
                if (_eventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    (zincEventBase as ZincEvent<T>).RemoveListener(action);
                }
            }


            /// <summary>
            /// 使用事件中心模块的对象必须在自己销毁时取消已经的注册事件
            /// </summary>
            public static void Unsubscribe<T, K>(EventType eventName, ZincAction<T, K> action)
            {
                if (_eventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    (zincEventBase as ZincEvent<T, K>).RemoveListener(action);
                }
            }


            /// <summary>
            /// 使用事件中心模块的对象必须在自己销毁时取消已经的注册事件
            /// </summary>
            public static void RemoveSubscription(EventType eventName)
            {
                if (_eventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    zincEventBase.RemoveAllListeners();
                    _eventDictionary.Remove(eventName);
                }
            }

            public static void Boardcast(string eventName)
            {
                if (_strEventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    if (zincEventBase is ZincEvent zincEvent)
                    {
                        zincEvent.Invoke();
                    }
                }
            }

            public static void Boardcast<T>(string eventName, T t)
            {
                if (_strEventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    if (zincEventBase is ZincEvent<T> zincEvent)
                    {
                        zincEvent.Invoke(t);
                    }
                }
            }

            public static void Boardcast<T, K>(string eventName, T arg1, K arg2)
            {
                if (_strEventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    if (zincEventBase is ZincEvent<T, K> zincEvent)
                    {
                        zincEvent.Invoke(arg1, arg2);
                    }
                }
            }

            // ===== Subscribe =====

            public static void Subscribe(string eventName, ZincAction action)
            {
                if (!_strEventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    zincEventBase = new ZincEvent();
                    _strEventDictionary.Add(eventName, zincEventBase);
                }

                (zincEventBase as ZincEvent).AddListener(action);
            }

            public static void Subscribe<T>(string eventName, ZincAction<T> action)
            {
                if (!_strEventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    zincEventBase = new ZincEvent<T>();
                    _strEventDictionary.Add(eventName, zincEventBase);
                }

                (zincEventBase as ZincEvent<T>).AddListener(action);
            }

            public static void Subscribe<T, K>(string eventName, ZincAction<T, K> action)
            {
                if (!_strEventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    zincEventBase = new ZincEvent<T, K>();
                    _strEventDictionary.Add(eventName, zincEventBase);
                }

                (zincEventBase as ZincEvent<T, K>).AddListener(action);
            }

            // ===== Unsubscribe =====

            public static void Unsubscribe(string eventName, ZincAction action)
            {
                if (_strEventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    (zincEventBase as ZincEvent).RemoveListener(action);
                }
            }

            public static void Unsubscribe<T>(string eventName, ZincAction<T> action)
            {
                if (_strEventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    (zincEventBase as ZincEvent<T>).RemoveListener(action);
                }
            }

            public static void Unsubscribe<T, K>(string eventName, ZincAction<T, K> action)
            {
                if (_strEventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    (zincEventBase as ZincEvent<T, K>).RemoveListener(action);
                }
            }

            // ===== RemoveSubscription =====

            public static void RemoveSubscription(string eventName)
            {
                if (_strEventDictionary.TryGetValue(eventName, out ZincEventBase zincEventBase))
                {
                    zincEventBase.RemoveAllListeners();
                    _strEventDictionary.Remove(eventName);
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


                foreach (var @event in _strEventDictionary.Values)
                {
                    @event.RemoveAllListeners();
                }

                _eventDictionary.Clear();
                _strEventDictionary.Clear();
            }
        }
    }
}

