using System;
using System.Collections.Generic;
using System.Reflection;

namespace Miae.Data.Serialization
{
    public class DataBaseColumnAttribute:Attribute
    {

        public DataBaseColumnAttribute(string name) { this.Name = name; }

        public DataBaseColumnAttribute(string name, bool primaryKey):this(name)
        {
            this.PrimaryKey = primaryKey;
        }

        public string Name { get; protected set; }

        public bool PrimaryKey { get; protected set; }

        /// <summary>
        /// 获取属性-列映射
        /// </summary>
        /// <param name="entityType">要映射的实体类型</param>
        /// <returns>属性-列映射表。</returns>
        public static IDictionary<string, string> MapPropertyColumnName(Type entityType)
        {
            Dictionary<string, string> mapping = new Dictionary<string, string>();
            PropertyInfo[] pis = entityType.GetProperties();

            foreach (PropertyInfo pi in pis) //从 DataBaseColumnAttribute 中读取 属性-列 映射。
            {
                DataBaseColumnAttribute item = pi.GetCustomAttribute<DataBaseColumnAttribute>();
                if (item != null)
                {
                    mapping.Add(pi.Name,
                        string.IsNullOrEmpty(item.Name) ? pi.Name : item.Name);
                }
            }
            return mapping;
        }
    }
}