#nullable enable

using System;
using System.Collections.Generic;

namespace Core
{
    public sealed class DictionaryObjectPool
    {
        private readonly Dictionary<Type, List<IPoolObject>> _freeObjects = new();

        public void Push(IPoolObject obj)
        {
            var type = obj.GetType();

            if (!_freeObjects.ContainsKey(type))
                _freeObjects.Add(type, new List<IPoolObject>());

            _freeObjects[type].Add(obj);
        }

        public bool TryPop<T>(out T? value) where T : class
        {
            if (!_freeObjects.TryGetValue(typeof(T), out var objects) || (_freeObjects.TryGetValue(typeof(T), out _) && objects.Count == 0))
            {
                value = null;
                return false;
            }

            var selectedObject = objects[0];
            objects.Remove(selectedObject);

            value = (T)selectedObject;
            return true;
        }
    }
}