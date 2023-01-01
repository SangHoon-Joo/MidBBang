using System;
using UnityEngine;

namespace ProjectWilson.Infrastructure
{
    [Serializable]
    public abstract class GuidScriptableObject : ScriptableObject
    {
        [HideInInspector]
        [SerializeField]
        private byte[] _Guid;

        public Guid Guid => new Guid(_Guid);

        void OnValidate()
        {
            if (_Guid.Length == 0)
                _Guid = Guid.NewGuid().ToByteArray();
        }
    }
}