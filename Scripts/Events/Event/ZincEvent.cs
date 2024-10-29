using System.Collections.Generic;
using System.Reflection;


namespace ZincFramework
{
    namespace Events
    {
        public class ZincEvent<T, K> : ZincEventBase
        {
            public void AddListener(ZincAction<T, K> action)
            {
                if (action != null)
                {
                    _callsList.AddListener(GetDalegate(action));
                }
            }

            public void RemoveListener(ZincAction<T, K> action)
            {
                _callsList.RemoveListener(action.Target, action.Method);
            }

            public void Invoke(T argument1, K argument2)
            {
                List<ZincInvokableBase> netWorkInvokableBases = _callsList.PrePareInvoke();
                for (int i = 0; i < netWorkInvokableBases.Count; i++)
                {
                    if (netWorkInvokableBases[i] is ZincInvokable<T, K> invokeable1)
                    {
                        invokeable1.Invoke(argument1, argument2);
                    }
                    else if (netWorkInvokableBases[i] is ZincInvokable invokeable2)
                    {
                        invokeable2.Invoke();
                    }
                }
            }

            private ZincInvokable<T, K> GetDalegate(ZincAction<T, K> action)
            {
                return new ZincInvokable<T, K>(action);
            }
        }


        public class ZincEvent<T> : ZincEventBase
        {
            public void AddListener(ZincAction<T> action, int layer = -1)
            {
                if (action != null)
                {
                    _callsList.AddListener(GetDalegate(action), layer);
                }
            }

            public void RemoveListener(ZincAction<T> action)
            {
                _callsList.RemoveListener(action.Target, action.Method);
            }

            public void Invoke(T argument)
            {
                List<ZincInvokableBase> netWorkInvokableBases = _callsList.PrePareInvoke();
                for (int i = 0; i < netWorkInvokableBases.Count; i++)
                {
                    if (netWorkInvokableBases[i] is ZincInvokable<T> invokeable1)
                    {
                        invokeable1.Invoke(argument);
                    }
                    else if (netWorkInvokableBases[i] is ZincInvokable invokeable2)
                    {
                        invokeable2.Invoke();
                    }
                }
            }

            private ZincInvokable<T> GetDalegate(ZincAction<T> action)
            {
                return new ZincInvokable<T>(action);
            }
        }


        public class ZincEvent : ZincEventBase
        {
            public void AddListener(ZincAction action)
            {
                if(action != null)
                {
                    _callsList.AddListener(GetDalegate(action));
                }
            }

            public void RemoveListener(ZincAction action)
            {
                _callsList.RemoveListener(action.Target, action.Method);
            }

            public void Invoke()
            {
                List<ZincInvokableBase> netWorkInvokableBases = _callsList.PrePareInvoke();
                for (int i = 0; i < netWorkInvokableBases.Count; i++)
                {
                    if (netWorkInvokableBases[i] is ZincInvokable invokeable2)
                    {
                        invokeable2.Invoke();
                    }
                }
            }

            private ZincInvokable GetDalegate(ZincAction action)
            {
                return new ZincInvokable(action);
            }
        }
    }
}

