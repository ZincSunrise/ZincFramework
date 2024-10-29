using System.Collections.Generic;

namespace ZincFramework.DataPools
{
    public class LinkedListNodePool<T> : ObjectPool<LinkedListNode<T>>
    {
        public LinkedListNodePool() : base(() => new LinkedListNode<T>(default))
        {

        }

        public LinkedListNode<T> RentValue(T value)
        {
            var node = base.RentValue();
            node.Value = value;
            return node;
        }

        public override void ReturnValue(LinkedListNode<T> value)
        {
            value.Value = default;
            base.ReturnValue(value);
        }
    }
}