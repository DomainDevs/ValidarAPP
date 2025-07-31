using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

//SISTRAN
using Sistran.Core.Application.CommonService.Models;


namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.CancellationPolicies
{
    /// <summary>
    /// Tipo de Cancelacion de Poliza
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class CancellationPolicyType
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion
        /// </summary>        
        [DataMember]
        public string Description { get; set; }

    }
}
