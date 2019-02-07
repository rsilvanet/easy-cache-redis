using EasyCache.NET.Storage;
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

        public RedisStorage(string hostAndPort, int db)
        {
            _client = ConnectionMultiplexer.Connect(hostAndPort);
            _db = _client.GetDatabase(db);
        }

        public T GetValue<T>(string key)
        {
            var bytes = Encoding.Default.GetBytes(_db.StringGet(key));
            var serializer = new DataContractJsonSerializer(typeof(T));

            return (T)serializer.ReadObject(new MemoryStream(bytes));
        }

        public void SetValue<T>(string key, T value, TimeSpan expiration)
        {
            var stream = new MemoryStream();
            var serializer = new DataContractJsonSerializer(typeof(T));
            serializer.WriteObject(stream, value);

            _db.SetAdd(key, stream.ToArray());
        }

        public bool ContainsValidKey(string key)
        {
            return _db.KeyExists(key);
        }

        public void RemoveKey(string key)
        {
            _db.KeyDelete(key);
        }

        public void Reset()
        {
            _client.GetServer(_client.Configuration).FlushDatabase(_db.Database);
        }

        public int Count()
        {
            return _client.GetServer(_client.Configuration).Keys().Count();
        }
    }
}
