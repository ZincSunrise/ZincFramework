using ZincFramework.MVC.Interfaces;



namespace ZincFramework
{
    namespace MVC
    {
        namespace Observation
        {
            public readonly struct Notification : INotification
            {
                public string Name { get; }

                public object Data { get; }

                public string Type { get; }

                public Notification(string name, object data = null, string type = null)
                {
                    Name = name;
                    Data = data;
                    Type = type;
                }
            }


            public readonly struct Notification<T> : INotification where T : struct
            {
                public string Name { get; }

                public T StructData { get; }

                public string Type { get; }

                public object Data => StructData;

                public Notification(string name, T data, string type = null)
                {
                    Name = name;
                    StructData = data;
                    Type = type;
                }
            }
        }
    }
}
