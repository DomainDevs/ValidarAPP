using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;




namespace Sistran.Core.Application.ReconciliationServices.Models
{
    /// <summary>
    /// Reconciliation: Conciliacion
    /// </summary>
    [DataContract]
    public class Reconciliation
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// ReconciliationType : Tipo de Conciliacion 
        /// </summary>        
        [DataMember]
        public ReconciliationTypes ReconciliationType { get; set; }
        
        /// <summary>
        /// Date: Fecha
        /// </summary>        
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// User: Usuario
        /// </summary>        
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// BankStatements: Extractos Bancarios
        /// </summary>
        [DataMember]
        public List<Statement> BankStatements { get; set; }


        /// <summary>
        /// CompanyStatements: Extractos de la Compania
        /// </summary>
        [DataMember]
        public List<Statement> CompanyStatements { get; set; }
    }
}
