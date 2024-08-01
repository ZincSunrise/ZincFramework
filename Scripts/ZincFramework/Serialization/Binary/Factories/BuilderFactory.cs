using System;
using ZincFramework.Serialization.Binary;


namespace ZincFramework
{
    namespace Serialization
    {
        namespace Factory
        {            
            internal enum E_Builder_Type
            {
                PrimitiveValue = 2,
                Enumerable = 4,
                OtherSystemValue = 8,
                OtherValue = 16,
            }
            
            public abstract class BuilderFactory<T> : IBuilderFactory<T> where T : IBuilder
            {
                public abstract T CreateBuilder(Type type);
                
                public abstract T CreateBuilder(object obj);


                public abstract void AddBuilder(int index, Func<T> func);
                
                internal abstract T CreateBuilder(E_Builder_Type type);
            }
        }
    }
}

