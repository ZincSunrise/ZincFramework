using System;
using System.Collections.Generic;
using ZincFramework.MVC.Interfaces;


namespace ZincFramework.MVC.PooledCommand
{
    public sealed class CommandPool
    {
        private readonly Queue<WeakReference<ICommand>> _unusedWeakRef = new Queue<WeakReference<ICommand>>();

        private Func<ICommand> _commandFactory;

        public CommandPool(Func<ICommand> factory)
        {
            _commandFactory = factory;
        }

        public ICommand RentCommand(out WeakReference<ICommand> weakReference)
        {
            if (_unusedWeakRef.Count > 0)
            {
                weakReference = _unusedWeakRef.Dequeue();
            }
            else
            {
                weakReference = new WeakReference<ICommand>(_commandFactory.Invoke());
            }

            if (!weakReference.TryGetTarget(out var command))
            {
                weakReference.SetTarget(_commandFactory.Invoke());
            }

            return command;
        }

        public void ReturnCommand(WeakReference<ICommand> weakReference) 
        {
            _unusedWeakRef.Enqueue(weakReference);
        }
    }
}