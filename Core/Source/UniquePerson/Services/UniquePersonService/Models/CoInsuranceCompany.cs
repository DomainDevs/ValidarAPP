using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models
{
    [DataContract]
    public class CoInsuranceCompany : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public decimal Id { get; set; }

        /// <summary>
        /// Compañia coaseguradora
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Porcentaje de participación
        /// </summary>
        [DataMember]
        public decimal ParticipationPercentage { get; set; }

        /// <summary>
        /// Porcentaje de gastos
        /// </summary>
        [DataMember]
        public decimal ExpensesPercentage { get; set; }

        /// <summary>
        /// Porcentaje de participacion propia
        /// </summary>
        [DataMember]
        public decimal ParticipationPercentageOwn { get; set; }

        /// <summary>
        /// Numero de poliza
        /// </summary>
        [DataMember]
        public string PolicyNumber { get; set; }

        /// <summary>
        /// Numero de endoso
        /// </summary>
        [DataMember]
        public string EndorsementNumber { get; set; }

        /// <summary>
        /// Numero de coasegurado
        /// </summary>
        [DataMember]
        public string TributaryIdCardNo { get; set; }
    }
}
