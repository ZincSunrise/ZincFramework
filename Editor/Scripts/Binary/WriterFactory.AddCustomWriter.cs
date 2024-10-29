using ZincFramework.Binary.Serialization.ProfileWriters;

namespace ZincFramework.Binary.Serialization
{
    public partial class WriterFactory
    {
        private bool _isAdded = false;

        public void AddCustomWriter()
        {
            if (_isAdded)
            {
                return;
            }

            _isAdded = true;
            _baseProfileWriters.Add("UnityEngine.Vector2", new Vector2Writer());
            _baseProfileWriters.Add("ZincFramework.TextTree.VisibleState", new VisibleStateWriter());
        }
    }
}