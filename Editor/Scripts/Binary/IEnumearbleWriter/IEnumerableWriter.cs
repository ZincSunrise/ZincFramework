using System.Collections;


namespace ZincFramework.Binary.Serialization.ProfileWriters
{
    public abstract class IEnumerableWriter<TEnumerable, TElement> : ReferenceWriter<TEnumerable> where TEnumerable : IEnumerable
    {
        public BasicWriter<TElement> ElementWriter 
        {
            get => _basicWriter ??= WriterFactory.Instance.GetWriter(typeof(TElement).Name) as BasicWriter<TElement>;
            set => _basicWriter = value;
        }

        private BasicWriter<TElement> _basicWriter;
    }
}
