using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

//Sistran
using Sistran.Core.Application.CommonService.Models;


namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect
{
    /// <summary>
    /// CollectControl: Control de Apertura de Ingreso
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class CollectControl
    {

        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// User: Usuario
        /// </summary>        
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Branch: Sucursal del pago
        /// </summary>        
        [DataMember]
        public Branch Branch { get; set; }

        /// <summary>
        /// State: Estado 1 abierto , 0 cerrado
        /// </summary>        
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        /// Date: Fecha contable
        /// </summary>        
        [DataMember]
        public DateTime AccountingDate { get; set; }

        /// <summary>
        /// Date: Fecha de apertura
        /// </summary>        
        [DataMember]
        public DateTime OpenDate { get; set; }

        /// <summary>
        /// Date: Fecha de apertura
        /// </summary>        
        [DataMember]
        public DateTime? CloseDate { get; set; }

        /// <summary>
        /// Collects:Ingresos 
        /// </summary>        
        [DataMember]
        public List<Collect> Collects { get; set; }

        /// <summary>
        /// CollectControlPayments: Control de Pagos
        /// </summary>        
        [DataMember]
        public List<CollectControlPayment> CollectControlPayments { get; set; }

        
    }
}
