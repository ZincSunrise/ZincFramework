namespace ZincFramework.Pools
{
    public interface IReuseable
    {
        void OnReturn();

        void OnRent();
    }
}
