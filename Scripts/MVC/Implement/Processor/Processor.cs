using ZincFramework.MVC.Interfaces;
using ZincFramework.MVC.Observation;



namespace ZincFramework
{
    namespace MVC
    {
        public class Processor : Notifier, IProcessor
        {
            public string ProcessorName { get; protected set; }

            public virtual object MyData { get; protected set; }

            public Processor(string processorName, object myData = null)
            {
                ProcessorName = processorName;
                MyData = myData;
            }

            public virtual void OnRegister()
            {

            }

            public virtual void OnRemove()
            {

            }
        }
    }
}
