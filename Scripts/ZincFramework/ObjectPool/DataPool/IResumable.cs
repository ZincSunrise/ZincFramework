



namespace ZincFramework
{
    namespace DataPool
    {
        public interface IResumable
        {
            void OnReturn();

            void OnRent();
        }
    }
}
