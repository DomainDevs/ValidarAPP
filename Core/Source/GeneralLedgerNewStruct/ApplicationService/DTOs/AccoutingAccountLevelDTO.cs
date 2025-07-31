using System.Runtime.Serialization;

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    [DataContract]
    public class AccoutingAccountLevelDTO
    {
        /// <summary>
        ///     código de nivel
        /// </summary>
        [DataMember]
        public int LevelCode { get; set; }

        /// <summary>
        ///     Descripción del nivel
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Longitud de la cuenta contable
        /// </summary>
        [DataMember]
        public int Length { get; set; }
    }
}