



namespace ZincFramework
{
    namespace DataPools
    {
        public interface IReuseable
        {
            void OnReturn();

            void OnRent();
        }
    }
}
