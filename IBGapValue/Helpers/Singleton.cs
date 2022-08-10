using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace IBGapValue.Helpers
{
    public static class Singleton<T> where T : new()
    {
        public static ConcurrentDictionary<Type, T> instances = new ConcurrentDictionary<Type, T>();
        public static T Instance => instances.GetOrAdd(typeof(T), (t) => new T());
    }
}