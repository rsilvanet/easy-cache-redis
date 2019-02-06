using EasyCache.NET.Storage;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyCache.NET.Redis
{
    public class RedisStorage : ICacheStorage
    {
        private readonly IRedisClientsManager _client;

        public T GetValue<T>(string key)
        {
            using (var redis = _client.GetClient())
            {
                return redis.GetValues<T>(new List<string> { key }).FirstOrDefault();
            }
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
