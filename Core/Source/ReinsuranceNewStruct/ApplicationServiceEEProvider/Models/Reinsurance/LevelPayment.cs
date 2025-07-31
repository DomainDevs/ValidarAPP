#region Using
using System;
using Sistran.Core.Application.CommonService.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;


#endregion

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// Modelo para Pagos de Nivel de contrato
    /// </summary>
    [DataContract]
    public class LevelPayment
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Level
        /// </summary>
        [DataMember]
        public Level Level { get; set; }

        /// <summary>
        /// Número 
        /// </summary>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// Fecha
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// Cantidad, Moneda
        /// </summary>
        [DataMember]
        public Amount Amount { get; set; }

        
    }
}