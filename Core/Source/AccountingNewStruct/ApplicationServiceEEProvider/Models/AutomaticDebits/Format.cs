using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.AutomaticDebits
{
    /// <summary>
    /// FileFormat: Formato de Archivo
    /// </summary>
    [DataContract]
    public class Format
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Description: Descripción 
        /// </summary>        
        [DataMember]
        public string Description { get; set; }
  
        /// <summary>
        /// BankNetwork: Red
        /// </summary>        
        [DataMember]
        public BankNetwork BankNetwork { get; set; }
       
        /// <summary>
        /// FormatType: Tipo de Formato
        /// </summary>        
        [DataMember]
        public Enums.FormatTypes FormatType { get; set; }


        /// <summary>
        /// FormatUsingType: Tipo de Uso del Formato
        /// </summary>        
        [DataMember]
        public Enums.FormatUsingTypes FormatUsingType { get; set; }

        /// <summary>
        /// Date: Fecha de Vigencia
        /// </summary>        
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// Separator: Separador
        /// </summary>        
        [DataMember]
        public string Separator { get; set; }
                
        /// <summary>
        /// Fields: Campos del Formato
        /// </summary>        
        [DataMember]
        public List<Field> Fields { get; set; }    
        
    }
}
