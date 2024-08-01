using System;
using System.Collections;
using ZincFramework.Serialization.Binary;
using ZincFramework.Serialization.Cache;



namespace ZincFramework
{
    namespace Serialization
    {
        namespace Factory
        {            
            public interface IBuilderFactory
            {                
                public static readonly Type EnumerableType = typeof(IEnumerable);
                
                IBuilder CreateBuilder(Type type);
                
                IBuilder CreateBuilder(object obj);
            }
            
            
            
            public interface IBuilderFactory<out T> where T : IBuilder
            {
                T CreateBuilder(Type type);
                
                T CreateBuilder(object obj);
            }
        }
    }
}