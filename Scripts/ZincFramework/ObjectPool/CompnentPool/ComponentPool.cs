using System;
using System.Collections.Generic;
using UnityEngine;
using ZincFramework.DataPool;


namespace ZincFramework
{
    namespace CompnentPool
    {
        internal class ComponentPool : ObjectPool<Component>
        {
            private Queue<Component> _usingComponent = new Queue<Component>();
            public GameObject CompnentContainer { get; private set; } = new GameObject();

            public ComponentPool(Func<Component> addFunc, int maxCount, string name) : base(addFunc, maxCount)
            {
                CompnentContainer.name = name + "Container";
            }


            public override Component RentValue()
            {
                Component component;
                if (_cacheValues.Count > 0)
                {
                    component = _cacheValues.Dequeue();
                }
                else if (_usingComponent.Count >= MaxCount)
                {
                    component = _usingComponent.Dequeue();
                    if (component is Behaviour behaviour1)
                    {
                        behaviour1.enabled = false;
                    }
                }
                else
                {
                    component = _createFunction.Invoke();
                }

                if(component is Behaviour behaviour2)
                {
                    behaviour2.enabled = true;
                }

                _usingComponent.Enqueue(component);
                return component;
            }

            public override void ReturnValue(Component component)
            {
                if (component is Behaviour behaviour)
                {
                    behaviour.enabled = false;
                }
                _usingComponent.Dequeue();
                _cacheValues.Enqueue(component);
            }

            public override void Dispose()
            {
                base.Dispose();
                _usingComponent.Clear();
                _usingComponent = null;
                CompnentContainer = null;
            }
        }
    }
}
