using Sistran.Core.Application.PrintingServices.Models.Base;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.PrintingServices.Models
{
    [DataContract]
    public class FilterReport : BaseFilterReport
    {
        /// <summary>
        /// usuario
        /// </summary>
        [DataMember]
        public User User { get; set; }

        /// <summary>
        /// Riesgos
        /// </summary>
        [DataMember]
        public List<Risk> Risks { get; set; }
    }
}