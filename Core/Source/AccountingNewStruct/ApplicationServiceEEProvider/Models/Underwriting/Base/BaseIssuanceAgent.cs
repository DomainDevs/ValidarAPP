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
    public class BaseIssuanceAgent : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }
        
        /// <summary>
        /// Nombre
        /// </summary>
        [DataMember]
        public string FullName { get; set; }

        /// <summary>
        /// Fecha de Baja
        /// </summary>
        [DataMember]
        public DateTime? DateDeclined { get; set; }
    }
}