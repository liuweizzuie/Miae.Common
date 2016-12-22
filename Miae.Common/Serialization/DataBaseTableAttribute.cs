using System;

namespace Miae.Data.Serialization
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class DataBaseTableAttribute:Attribute
    {
        public DataBaseTableAttribute(string name) { this.Name = name; }
        public string Name { get; protected set; }
    }
}