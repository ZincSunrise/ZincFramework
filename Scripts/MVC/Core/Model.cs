using System;
using System.Collections.Concurrent;
using ZincFramework.MVC.Interfaces;
using ZincFramework.MVC.Observation;



namespace ZincFramework
{
    namespace MVC
    {
        namespace Core
        {
            public sealed class Model : Notifier, IModel
            {
                public static Model Instance => _instance.Value;

                private readonly static Lazy<Model> _instance = new Lazy<Model>(() => new Model());

                private readonly ConcurrentDictionary<string, IProcessor> _processorMap = new ConcurrentDictionary<string, IProcessor>();


                public IProcessor GetProcessor(string processorName)
                {
                    if (!_processorMap.TryGetValue(processorName, out var processor)) 
                    {
                        throw new ArgumentException($"不存在名字为{processorName}的所有者");
                    }

                    return processor;
                }

                public bool IsHasProcessor(string processorName)
                {
                    return _processorMap.ContainsKey(processorName);
                }

                public void RegistProcessor(IProcessor processor)
                {
                    if (!_processorMap.TryAdd(processor.ProcessorName, processor))
                    {
                        UnityEngine.Debug.LogWarning($"已经存在名字为{processor.ProcessorName}的所有者");
                    }

                    processor.OnRegister();
                }

                public bool RemoveProcessor(string processorName)
                {
                    if (!_processorMap.TryRemove(processorName, out var processor))
                    {
                        UnityEngine.Debug.LogWarning($"移除{processorName}失败");
                        return false;
                    }

                    processor.OnRemove();
                    return true;
                }

                public bool TryGetProcessor(string processorName, out IProcessor processor)
                {
                    return _processorMap.TryGetValue(processorName, out processor);
                }
            }
        }
    }
}