using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReportingServices.Models
{
    [DataContract]
    public class Parameter
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// El parametro es considerado formula de calculo del reporte
        /// </summary>        
        [DataMember]
        public bool IsFormula { get; set; }

        /// <summary>
        /// Description: Nombre del parámetro
        /// </summary>        
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Value: Valor del parámetro
        /// </summary>        
        [DataMember]
        public object Value { get; set; }

        /// <summary>
        /// DbType: Tipo de dato
        /// </summary>        
        [DataMember]
        public Type DbType { get; set; }

    }
}
