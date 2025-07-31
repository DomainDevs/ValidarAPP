using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.CancellationMassiveEndorsement.Models
{
    [DataContract]
    public class CancellationModel
    {
        /// <summary>
        /// Numero documento Polizas
        /// </summary>
        [DataMember]
        public Decimal DocumentNumber { get; set; }


        /// <summary>
        /// Identificador  Sucursal 
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }


        /// <summary>
        /// Identificador Ramo
        /// </summary>
        [DataMember]
        public int PrefixId { get; set; }

        /// <summary>
        /// Identificador Cancelacion
        /// </summary>
        [DataMember]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Identificador Tipo de Cancelacion
        /// </summary>
        [DataMember]
        public int CancelationTypeId { get; set; }

        /// <summary>
        /// Obtener o setear Motivo de Cancelacion
        /// </summary>
        /// <value>
        /// Identificado Motivo de Cancelacion
        /// </value>
        [DataMember]
        public int CancelationReasonId { get; set; }

        /// <summary>
        /// Numero de radicado
        /// </summary>
        [DataMember]
        public int TicketNumber{ get; set; }

        [DataMember]
        public DateTime TicketDate { get; set; }
    }

}
