using System.Runtime.Serialization;

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
     [DataContract]
    public class BranchDTO
    {
        /// <summary>
        /// Identificador del ramo
        /// </summary>
        [DataMember]
        public int Id { get; set; }


        /// <summary>
        /// Nombre del ramo
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
