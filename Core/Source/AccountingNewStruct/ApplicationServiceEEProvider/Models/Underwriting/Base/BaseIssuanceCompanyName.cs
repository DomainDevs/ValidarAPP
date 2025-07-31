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
    public class BaseIssuanceCompanyName : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int NameNum { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string TradeName { get; set; }

        /// <summary>
        /// IsMain
        /// </summary>
        [DataMember]
        public bool IsMain { get; set; }
    }
}