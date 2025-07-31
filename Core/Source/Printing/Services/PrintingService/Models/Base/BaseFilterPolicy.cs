using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.PrintingServices.Models.Base
{
    [DataContract]
    public class BaseFilterPolicy : Extension
    {
        /// <summary>
        /// Id sucursal
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }

        /// <summary>
        /// Id ramo comercial
        /// </summary>
        [DataMember]
        public int PrefixId { get; set; }

        /// <summary>
        /// Id producto
        /// </summary>
        [DataMember]
        public int? ProductId { get; set; }

        /// <summary>
        /// Id póliza
        /// </summary>
        [DataMember]
        public int PolicyId { get; set; }

        /// <summary>
        /// Número de póliza
        /// </summary>
        [DataMember]
        public decimal PolicyNumber { get; set; }

        /// <summary>
        /// Id endoso
        /// </summary>
        [DataMember]
        public int EndorsementId { get; set; }

        /// <summary>
        /// Id temporal
        /// </summary>
        [DataMember]
        public int TemporalId { get; set; }

        /// <summary>
        /// Riesgos por bloque
        /// </summary>
        [DataMember]
        public int CountRiskBlock { get; set; }

        /// <summary>
        /// Id proceso
        /// </summary>
        [DataMember]
        public int ProcessId { get; set; }

        /// <summary>
        /// Nombre de usuario
        /// </summary>
        [DataMember]
        public string UserName { get; set; }
    }
}
