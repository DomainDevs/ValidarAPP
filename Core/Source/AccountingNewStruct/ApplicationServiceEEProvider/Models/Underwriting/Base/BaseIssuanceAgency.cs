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
    public class BaseIssuanceAgency : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        
        /// <summary>
        /// Código
        /// </summary>
        [DataMember]
        public int Code { get; set; }

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

        [DataMember]
        public bool IsPrincipal { get; set; }

        [DataMember]
        public decimal Participation { get; set; }
    }
}