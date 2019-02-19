using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EasyCache.NET.Tests.Shared
{
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