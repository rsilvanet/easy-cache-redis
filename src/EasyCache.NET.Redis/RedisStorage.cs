using EasyCache.NET.Storage;
using ServiceStack.Redis;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace EasyCache.NET.Redis
{
    public class RedisStorage : ICacheStorage
    {
        private readonly IRedisClientsManager _client;

        public T GetValue<T>(string key)
        {
            using (var redis = _client.GetClient())
            {
                var bytes = Encoding.Default.GetBytes(redis.GetValue(key));
                var serializer = new DataContractJsonSerializer(typeof(T));

                return (T)serializer.ReadObject(new MemoryStream(bytes));
            }
        }

        public void SetValue<T>(string key, T value, TimeSpan expiration)
        {
            var stream = new MemoryStream();
            var serializer = new DataContractJsonSerializer(typeof(T));
            serializer.WriteObject(stream, value);

            using (var redis = _client.GetClient())
            {
                redis.SetValue(key, Encoding.Default.GetString(stream.ToArray()));
            }
        }

        public bool ContainsValidKey(string key)
        {
            using (var redis = _client.GetClient())
            {
                return redis.ContainsKey(key);
            }
        }

        public void RemoveKey(string key)
        {
            using (var redis = _client.GetClient())
            {
                redis.Remove(key);
            }
        }

        public void Reset()
        {
            using (var redis = _client.GetClient())
            {
                redis.FlushAll();
            }
        }

        public int Count()
        {
            throw new NotImplementedException();
        }
    }
}
