using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.ProductServices.Models.Base
{
    /// <summary>
    /// Comisiones producto
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Extension" />
    [DataContract]
    public class BaseProductAgencyCommiss : Extension
    {
        /// <summary>
        /// Id agente
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Id agencia
        /// </summary>
        [DataMember]
        public int AgencyId { get; set; }

        /// <summary>
        /// Id producto
        /// </summary>
        [DataMember]
        public int ProductId { get; set; }

        /// <summary>
        /// Porcentaje comisión
        /// </summary>
        [DataMember]
        public decimal CommissPercentage { get; set; }

        /// <summary>
        /// Porcentaje comisión Adicional
        /// </summary>
        [DataMember]
        public decimal? AdditionalCommissionPercentage { get; set; }

        /// <summary>
        /// Porcentaje comisión Adicional
        /// </summary>
        [DataMember]
        public decimal? SchCommissPercentage { get; set; }

        /// <summary>
        /// Porcentaje comisión Adicional
        /// </summary>
        [DataMember]
        public decimal? StDisCommissPercentage { get; set; }

        /// <summary>
        /// Porcentaje comisión Adicional
        /// </summary>
        [DataMember]
        public decimal? AdditDisCommissPercentage { get; set; }

        /// <summary>
        /// Nombre Agencia
        /// </summary>
        [DataMember]
        public string AgencyName { get; set; }

    }
}
