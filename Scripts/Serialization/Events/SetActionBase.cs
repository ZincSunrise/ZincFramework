using System;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Events
        {
            public class SetActionBase
            {

            }

            public class SetAction<TObject, TSet> : SetActionBase
            {
                private readonly Action<TObject, TSet> _setAction;

                public SetAction(Action<TObject, TSet> action) 
                {
                    _setAction = action;
                }

                public void Invoke(TObject obj, TSet set)
                {
                    _setAction.Invoke(obj, set);
                }
            }
        }
    }
}
