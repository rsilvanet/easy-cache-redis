using EasyCache.NET.Storage;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace EasyCache.NET.Redis
{
    public class RedisStorage : ICacheStorage
    {
        private readonly IConnectionMultiplexer _client;
        private readonly IDatabase _db;

        public RedisStorage() { }

        public RedisStorage(string hostAndPort, int db)
        {
            _client = ConnectionMultiplexer.Connect(hostAndPort);
            _db = _client.GetDatabase(db);
        }

        public virtual T GetValue<T>(string key)
        {
            return JsonConvert.DeserializeObject<T>(_db.StringGet(key));
        }

        public virtual void SetValue<T>(string key, T value, TimeSpan expiration)
        {
            _db.StringSet(key, JsonConvert.SerializeObject(value));
        }

        public virtual bool ContainsValidKey(string key)
        {
            return _db.KeyExists(key);
        }

        public virtual void RemoveKey(string key)
        {
            _db.KeyDelete(key);
        }

        public virtual void Reset()
        {
            _client.GetServer(_client.Configuration).FlushDatabase(_db.Database);
        }

        public virtual int Count()
        {
            return _client.GetServer(_client.Configuration).Keys().Count();
        }
    }
}
