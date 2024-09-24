namespace ZincFramework
{
    namespace Serialization
    {
        public interface IConvertable
        {
            /// <summary>
            /// 这个方法相当于没带头文件的反序列化方法
            /// </summary>
            /// <param name="bytes"></param>
            void Convert(byte[] buffer, ref int nowIndex);
        }

        public interface IAppend
        {
            /// <summary>
            /// 这个方法相当于没带头文件的序列化方法
            /// </summary>
            /// </summary>
            /// <returns></returns>
            void Append(byte[] buffer, ref int nowIndex);
        }
    }
}
