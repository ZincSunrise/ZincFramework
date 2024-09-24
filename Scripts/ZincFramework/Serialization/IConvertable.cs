namespace ZincFramework
{
    namespace Serialization
    {
        public interface IConvertable
        {
            /// <summary>
            /// ��������൱��û��ͷ�ļ��ķ����л�����
            /// </summary>
            /// <param name="bytes"></param>
            void Convert(byte[] buffer, ref int nowIndex);
        }

        public interface IAppend
        {
            /// <summary>
            /// ��������൱��û��ͷ�ļ������л�����
            /// </summary>
            /// </summary>
            /// <returns></returns>
            void Append(byte[] buffer, ref int nowIndex);
        }
    }
}
