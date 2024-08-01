using System;
using System.Reflection;


namespace ZincFramework
{
    namespace Events
    {
        public abstract class ZincInvokableBase
        {
            public abstract bool FindMethod(object target, MethodInfo methodInfo);

            protected ZincInvokableBase() { }

            protected ZincInvokableBase(object target, MethodInfo methodInfo)
            {
                if (methodInfo == null)
                {
                    throw new ArgumentNullException("找不到信息！");
                }

                if (methodInfo.IsStatic)
                {
                    if (target != null)
                    {
                        throw new ArgumentException("静态函数调用函数的对象必须为空");
                    }
                }
                else if (target == null)
                {
                    throw new ArgumentNullException("必须要有调用函数的对象");
                }
            }

            protected static void CheckInvaild<T>(object target)
            {
                if ((target != null) && target is not T)
                {
                    throw new ArgumentException("传入的参数类型错误!");
                }
            }
        }
    }
}
