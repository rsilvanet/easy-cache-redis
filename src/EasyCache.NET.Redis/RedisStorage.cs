using System;
using EasyCache.NET.Storage;

namespace EasyCache.NET.Redis
{
    public class RedisStorage : ICacheStorage
    {
        public T GetValue<T>(string key)
        {
            throw new NotImplementedException();
        }

        public void SetValue<T>(string key, T value, TimeSpan expiration)
        {
            throw new NotImplementedException();
        }

        public bool ContainsValidKey(string key)
        {
            throw new NotImplementedException();
        }

        public void RemoveKey(string key)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }
    }
}
