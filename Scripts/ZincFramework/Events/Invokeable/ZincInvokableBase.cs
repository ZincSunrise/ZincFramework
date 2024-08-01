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
                    throw new ArgumentNullException("�Ҳ�����Ϣ��");
                }

                if (methodInfo.IsStatic)
                {
                    if (target != null)
                    {
                        throw new ArgumentException("��̬�������ú����Ķ������Ϊ��");
                    }
                }
                else if (target == null)
                {
                    throw new ArgumentNullException("����Ҫ�е��ú����Ķ���");
                }
            }

            protected static void CheckInvaild<T>(object target)
            {
                if ((target != null) && target is not T)
                {
                    throw new ArgumentException("����Ĳ������ʹ���!");
                }
            }
        }
    }
}
