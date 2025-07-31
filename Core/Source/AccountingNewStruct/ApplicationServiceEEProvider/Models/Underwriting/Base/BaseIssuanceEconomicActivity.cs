using Sistran.Core.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting.Base
{
    [DataContract]
    public class BaseIssuanceEconomicActivity : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Actividad economica
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}