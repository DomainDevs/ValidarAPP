#region Using
using System;
using Sistran.Core.Application.CommonService.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;


#endregion

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// Modelo para Restablecer de Nivel de contrato
    /// </summary>
    [DataContract]
    public class LevelRestore
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
        /// Porcentaje Restablecimiento
        /// </summary>
        [DataMember]
        public decimal RestorePercentage { get; set; }

        /// <summary>
        /// Porcentaje de Aviso
        /// </summary>
        [DataMember]
        public decimal NoticePercentage { get; set; }

        
    }
}