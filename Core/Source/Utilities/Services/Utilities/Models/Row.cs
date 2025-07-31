using Sistran.Core.Application.UtilitiesServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Services.UtilitiesServices.Models
{
    [DataContract]
    public class Row : BaseRow
    {
        /// <summary>
        /// Campos
        /// </summary>
        [DataMember]
        public List<Field> Fields { get; set; }
    }
}