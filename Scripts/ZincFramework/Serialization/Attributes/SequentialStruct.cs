using System;


namespace ZincFramework
{
    namespace Serialization
    {
        /// <summary>
        /// ���һ���ṹ��ֻ���л������ͻ��ߺ��в����������͵Ľṹ�壬�Ϳ��Դ��ϴ��������������
        /// </summary>
        [AttributeUsage(AttributeTargets.Struct)]
        public class SequentialStruct : BinaryAttribute
        {

        }
    }
}