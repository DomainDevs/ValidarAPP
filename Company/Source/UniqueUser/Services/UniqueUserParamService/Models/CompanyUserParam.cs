using Sistran.Company.Application.UniqueUserServices.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniqueUserParamService.Models
{
    [DataContract]
    public class CompanyUserParam : CompanyUser
    {

        /// <summary>
        /// Listado de puntos de venta de aliados.
        /// </summary>
        [DataMember]
        public List<CptUniqueUserSalePointAlliance> CptUniqueUserSalePointAlliance { get; set; }
    }
}
