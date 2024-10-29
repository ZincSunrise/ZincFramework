using System;
using System.Reflection;


namespace ZincFramework
{
    namespace Events
    {
        internal class ZincInvokable : ZincInvokableBase
        {
            protected ZincAction _myAction;

            public ZincInvokable()
            {

            }

            public ZincInvokable(ZincAction zincAction)
            {
                _myAction += zincAction;
            }


            public ZincInvokable(object target, MethodInfo methodInfo) : base(target, methodInfo)
            {
                _myAction += (ZincAction)Delegate.CreateDelegate(typeof(ZincAction), target, methodInfo);
            }


            public override bool FindMethod(object Target, MethodInfo methodInfo)
            {
                return _myAction.Target == Target && methodInfo.Equals(_myAction.Method);
            }

            public void Invoke()
            {
                _myAction.Invoke();
            }
        }

        internal class ZincInvokable<T> : ZincInvokableBase
        {
            protected ZincAction<T> _myAction;

            public ZincInvokable()
            {

            }

            public ZincInvokable(ZincAction<T> zincAction)
            {
                _myAction += zincAction;
            }

            public ZincInvokable(object target, MethodInfo methodInfo)
            {
                _myAction += (ZincAction<T>)Delegate.CreateDelegate(typeof(ZincAction<T>) ,target, methodInfo);
            }

            public override bool FindMethod(object Target, MethodInfo methodInfo)
            {
                return _myAction.Target == Target && methodInfo.Equals(_myAction.Method);
            }

            public void Invoke(T arg)
            {
                _myAction.Invoke(arg);
            }
        }


        internal class ZincInvokable<T, K> : ZincInvokableBase
        {
            protected ZincAction<T, K> _myAction;

            public ZincInvokable()
            {

            }

            public ZincInvokable(ZincAction<T, K> zincAction)
            {
                _myAction += zincAction;
            }

            public ZincInvokable(object target, MethodInfo methodInfo)
            {
                _myAction += (ZincAction<T, K>)Delegate.CreateDelegate(typeof(ZincAction<T, K>), target, methodInfo);
            }

            public override bool FindMethod(object Target, MethodInfo methodInfo)
            {
                return _myAction.Target == Target && methodInfo.Equals(_myAction.Method);
            }

            public void Invoke(T arg1, K arg2)
            {
                _myAction.Invoke(arg1, arg2);
            }
        }
    }
}
