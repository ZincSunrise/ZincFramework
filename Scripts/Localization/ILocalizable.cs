namespace ZincFramework.Localization
{
    public interface ILocalizable
    {
        void Localize();
    }

    public interface ILocalizable<T>
    {
        void Localize(in T value);
    }
}