namespace ZincFramework.Localization
{
    public abstract class Localizable<T> : ILocalizable<T>
    {
        public abstract void Localize(in T value);
    }
}