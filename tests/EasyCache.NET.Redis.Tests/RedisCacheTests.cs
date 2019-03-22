using EasyCache.NET.Storage;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace EasyCache.NET.Redis.Tests
{
    public class RedisCacheTests
    {
        private Mock _storage;
        private Caching _caching;

        public RedisCacheTests()
        {
            _storage = new Mock<RedisStorage>();
            _caching = new Caching(_storage.As<ICacheStorage>().Object);
        }

        [Fact]
        public void CanSetCache()
        {
            _caching.SetValue("test", "Some value", TimeSpan.FromDays(1));
            _storage.As<ICacheStorage>().Verify(x => x.SetValue("test", "Some value", TimeSpan.FromDays(1)), Times.Once);
        }

        [Fact]
        public void CanGenerateCacheFromCachelessFunc()
        {
            _caching.GetValue("test", () => "Some value", TimeSpan.FromDays(1));
            _storage.As<ICacheStorage>().Verify(x => x.SetValue("test", "Some value", TimeSpan.FromDays(1)), Times.Once);
        }

        [Fact]
        public void CanGetValueFromCache()
        {
            _caching.GetValue<string>("test");
            _storage.As<ICacheStorage>().Verify(x => x.GetValue<string>("test"), Times.Once);
        }

        [Fact]
        public void CanCacheSeveralDataTypes()
        {
            var date = new DateTime(2019, 1, 21, 22, 15, 0).ToUniversalTime();

            var customObject = new TestObject
            {
                StringField = "Some value",
                IntField = 10,
                LongField = 10L,
                DoubleField = 10.99,
                DecimalField = 10.99m,
                DateField = date,
                BoolField = true,
                EnumField = TestEnum.Item2,
                ObjectField = new AnotherTestObject { StringField = "Some value" },
                ListField = new List<AnotherTestObject>
                {
                    new AnotherTestObject() { StringField = "Some value 1" },
                    new AnotherTestObject() { StringField = "Some value 2" }
                }
            };

            _caching.SetValue("char", 'A', TimeSpan.FromDays(1));
            _caching.SetValue("string", "Some value", TimeSpan.FromDays(1));
            _caching.SetValue("int-pos", 10, TimeSpan.FromDays(1));
            _caching.SetValue("int-neg", -10, TimeSpan.FromDays(1));
            _caching.SetValue("long-pos", 10L, TimeSpan.FromDays(1));
            _caching.SetValue("long-neg", -10L, TimeSpan.FromDays(1));
            _caching.SetValue("double-pos", 10.99, TimeSpan.FromDays(1));
            _caching.SetValue("double-neg", -10.99, TimeSpan.FromDays(1));
            _caching.SetValue("decimal-pos", 10.99m, TimeSpan.FromDays(1));
            _caching.SetValue("decimal-neg", -10.99m, TimeSpan.FromDays(1));
            _caching.SetValue("datetime", date, TimeSpan.FromDays(1));
            _caching.SetValue("bool-true", true, TimeSpan.FromDays(1));
            _caching.SetValue("bool-false", false, TimeSpan.FromDays(1));
            _caching.SetValue("custom-obj", customObject, TimeSpan.FromDays(1));

            var mock = _storage.As<ICacheStorage>();
            
            mock.Verify(x => x.SetValue("char", 'A', TimeSpan.FromDays(1)), Times.Once);
            mock.Verify(x => x.SetValue("string", "Some value", TimeSpan.FromDays(1)), Times.Once);
            mock.Verify(x => x.SetValue("int-pos", 10, TimeSpan.FromDays(1)), Times.Once);
            mock.Verify(x => x.SetValue("int-neg", -10, TimeSpan.FromDays(1)), Times.Once);
            mock.Verify(x => x.SetValue("long-pos", 10L, TimeSpan.FromDays(1)), Times.Once);
            mock.Verify(x => x.SetValue("long-neg", -10L, TimeSpan.FromDays(1)), Times.Once);
            mock.Verify(x => x.SetValue("double-pos", 10.99, TimeSpan.FromDays(1)), Times.Once);
            mock.Verify(x => x.SetValue("double-neg", -10.99, TimeSpan.FromDays(1)), Times.Once);
            mock.Verify(x => x.SetValue("decimal-pos", 10.99m, TimeSpan.FromDays(1)), Times.Once);
            mock.Verify(x => x.SetValue("decimal-neg", -10.99m, TimeSpan.FromDays(1)), Times.Once);
            mock.Verify(x => x.SetValue("datetime", date, TimeSpan.FromDays(1)), Times.Once);
            mock.Verify(x => x.SetValue("bool-true", true, TimeSpan.FromDays(1)), Times.Once);
            mock.Verify(x => x.SetValue("bool-false", false, TimeSpan.FromDays(1)), Times.Once);
            mock.Verify(x => x.SetValue("custom-obj", customObject, TimeSpan.FromDays(1)), Times.Once);
        }

        [Fact]
        public void CanRemoveKey()
        {
            _caching.RemoveKey("test");
            _storage.As<ICacheStorage>().Verify(x => x.RemoveKey("test"), Times.Once);
        }

        [Fact]
        public void CanResetCache()
        {
            _caching.Reset();
            _storage.As<ICacheStorage>().Verify(x => x.Reset(), Times.Once);
        }
    }
    
    public enum TestEnum
    {
        Item1,
        Item2,
        Item3
    }

    public class TestObject
    {
        public string StringField { get; set; }
        public int IntField { get; set; }
        public long LongField { get; set; }
        public double DoubleField { get; set; }
        public decimal DecimalField { get; set; }
        public DateTime DateField { get; set; }
        public bool BoolField { get; set; }
        public TestEnum EnumField { get; set; }
        public AnotherTestObject ObjectField { get; set; }
        public IList<AnotherTestObject> ListField { get; set; }
    }

    public class AnotherTestObject
    {
        public string StringField { get; set; }
    }
}
