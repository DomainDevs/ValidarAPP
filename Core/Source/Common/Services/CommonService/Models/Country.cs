using Sistran.Core.Application.CommonService.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.CommonService.Models
{
    [DataContract]
    public class Country : BaseCountry
    {
        /// <summary>
        /// Listado de estados
        /// </summary>
        [DataMember]
        public List<State> States { get; set; }
    }
}
