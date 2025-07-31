using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.Audit
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class PrimaryKeyModel
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [DataMember]
        public object Value { get; set; }

        /// <summary>
        /// Gets the primary key model.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns></returns>
        public static List<PrimaryKeyModel> GetPrimaryKeyModel(IDictionary<string, object> dictionary)
        {
            var data = new List<PrimaryKeyModel>();
            if (dictionary != null)
            {
                foreach (KeyValuePair<string, object> val in dictionary)
                {
                    data.Add(new PrimaryKeyModel { Key = val.Key, Value = val.Value });
                }
                return data;
            }
            return null;
        }
    }
}
