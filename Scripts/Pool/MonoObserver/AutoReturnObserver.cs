using UnityEngine;
using ZincFramework.MonoModel;


namespace ZincFramework.Pool.GameObjects
{
    public class AutoReturnObserver : IMonoObserver
    {
        public ReuseableObject _reuseableObject;

        private readonly float _returnOffset;

        private float _returnTimer;

        public AutoReturnObserver(ReuseableObject reuseableObject, float returnOffset)
        {
            _reuseableObject = reuseableObject;
            _returnOffset = returnOffset;
        }

        public void NotifyObserver()
        {
            if (Time.time - _returnTimer > _returnOffset)
            {
                if (_reuseableObject.gameObject.activeSelf)
                {
                    ObjectPoolManager.Instance.ReturnReusableGameObject(_reuseableObject);
                }

                MonoManager.Instance.RemoveFixedUpdateObserver(this);
            }
        }

        public void OnRegist()
        {
            _returnTimer = Time.time;
        }

        public void OnRemove()
        {
            _returnTimer = float.MaxValue;
        }
    }
}