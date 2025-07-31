using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Extensions
{
    /// <summary>
    /// Lista de Propiedades extendidas
    /// </summary>
    [DataContract]
    [Serializable]
    public class Extension
    {
        /// <summary>
        /// The extended properties
        /// </summary>
        private List<ExtendedProperty> extendedProperties = new List<ExtendedProperty>();

        /// <summary>
        /// Gets or sets the extended properties.
        /// </summary>
        /// <value>
        /// The extended properties.
        /// </value>
        [DataMember]
        public List<ExtendedProperty> ExtendedProperties
        {
            get
            {
                return extendedProperties;
            }
            set
            {
                extendedProperties = value;
            }
        }


        /// <summary>
        /// Gets the extended property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public T GetExtendedProperty<T>(string name)
        {
            if (extendedProperties != null && extendedProperties.Exists(x => x.Name == name) && extendedProperties.First(x => x.Name == name).Value != null)
            {
                Type originalType = typeof(T);
                var underlyingType = Nullable.GetUnderlyingType(originalType);
                return (T)Convert.ChangeType(extendedProperties.First(x => x.Name == name).Value, underlyingType ?? originalType);
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// Sets the extended property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void SetExtendedProperty(string name, object value)
        {
            if (value != null)
            {
                extendedProperties.Add(new ExtendedProperty
                {
                    Name = name,
                    Value = value
                });
            }
        }
    }
}
