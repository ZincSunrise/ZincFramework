using System;


namespace ZincFramework
{
    namespace Serialization
    {
        /// <summary>
        /// 如果一个结构体只含有基础类型或者含有不含基础类型的结构体，就可以打上此特性以提高性能
        /// </summary>
        [AttributeUsage(AttributeTargets.Struct)]
        public class SequentialStruct : BinaryAttribute
        {

        }
    }
}