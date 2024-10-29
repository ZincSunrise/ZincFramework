using System;
using UnityEngine;


namespace ZincFramework.DialogueSystem.TextData
{
    [Serializable]
    public struct VisibleState : IEquatable<VisibleState>
    {
        public readonly int VisableId => _visableId;

        public readonly int Differential => _differential;

        public readonly bool IsFocus => _isFocus;

        public readonly Vector2 Position => _positon;


        [SerializeField]
        private int _visableId;

        [SerializeField]
        private int _differential;

        [SerializeField]
        private bool _isFocus;

        [SerializeField]
        private Vector2 _positon;

        public VisibleState(int visibleId, int differential, bool isFocus, Vector2 position)
        {
            _visableId = visibleId;
            _differential = differential;
            _isFocus = isFocus;
            _positon = position;
        }

        public readonly void Deconstruct(out int visibleId, out int differential)
        {
            visibleId = _visableId;
            differential = _differential;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(_visableId);
            hash.Add(_differential);
            hash.Add(_isFocus);
            hash.Add(_positon);
            return hash.ToHashCode();
        }


        public override bool Equals(object obj)
        {
            return obj is VisibleState other && other == this;
        }

        public bool Equals(VisibleState other)
        {
            return this == other;
        }

        public static bool operator ==(VisibleState left, VisibleState right)
        {
            return left._visableId == right._visableId 
                && left._differential == right._differential 
                && left._isFocus == right._isFocus 
                && left._positon == right._positon;
        }

        public static bool operator !=(VisibleState left, VisibleState right)
        {
            return !(left == right);
        }
    }
}