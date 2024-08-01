using System.Collections.Generic;
using System.Reflection;

namespace ZincFramework
{
    namespace Events
    {
        internal class InvokableList
        {
            private readonly List<ZincInvokableBase> _runTimeAction = new List<ZincInvokableBase>();
            private List<ZincInvokableBase> _excutingAction = new List<ZincInvokableBase>();

            public int Count => _runTimeAction.Count;
            private bool _needUpdate = false;

            public InvokableList() { }

            public void AddListener(ZincInvokableBase netWorkEventBase, int layer = -1)
            {
                if(layer == -1)
                {
                    _runTimeAction.Add(netWorkEventBase);
                }
                else
                {
                    if(_runTimeAction.Count == 0)
                    {
                        _runTimeAction.Add(netWorkEventBase);
                    }
                    else
                    {
                        _runTimeAction.Insert(layer, netWorkEventBase);
                    }
                }
                _needUpdate = true;
            }

            public void RemoveListener(object target, MethodInfo methodInfo)
            {
                List<ZincInvokableBase> deleteList = new List<ZincInvokableBase>();
                for (int i = 0; i < _runTimeAction.Count; i++)
                {
                    if (_runTimeAction[i].FindMethod(target, methodInfo))
                    {
                        deleteList.Add(_runTimeAction[i]);
                    }
                }

                _runTimeAction.RemoveAll(deleteList.Contains);
                List<ZincInvokableBase> newList = new List<ZincInvokableBase>(_runTimeAction.Count);
                newList.AddRange(_runTimeAction);

                _excutingAction = newList;
                _needUpdate = false;
            }

            public List<ZincInvokableBase> PrePareInvoke()
            {
                if (_needUpdate)
                {
                    _excutingAction.Clear();
                    _excutingAction.AddRange(_runTimeAction);
                    _needUpdate = false;
                }
                return _excutingAction;
            }

            public void Clear()
            {
                _runTimeAction.Clear();
                _excutingAction.Clear();
                _needUpdate = false;
            }
        }
    }
}

