#region Using

using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    /// <summary>
    ///     Tipo de Movimiento
    /// </summary>
    [DataContract]
    public class ReconciliationMovementTypeDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// SmallDescription: Descripcion reducida
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Description: Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///  AccountingNature
        /// </summary>
        [DataMember]
        public int AccountingNature { get; set; }
    }
}