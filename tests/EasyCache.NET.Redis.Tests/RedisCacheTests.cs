using EasyCache.NET.Storage;
using EasyCache.NET.Tests.Shared;
using Moq;
using System.IO;
using System.Linq;

namespace EasyCache.NET.Redis.Tests
{
    public class RedisCacheTests : BaseCacheTests
    {
        public RedisCacheTests()
        {
            var storage = new Mock<RedisStorage>("localhost:6379", 0)
            {
                CallBase = true
            };

            _storage = storage;
            _caching = new Caching(storage.Object);
        }
    }
}
