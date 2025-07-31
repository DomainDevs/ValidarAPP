using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.Locations.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using UNDTO = Sistran.Core.Application.UnderwritingServices.DTOs;

namespace Sistran.Core.Application.Locations
{
    [ServiceContract]
    public interface ILocations
    {
        /// <summary>
        ///  tipo de subfijos
        /// </summary>
        [OperationContract]
        List<Suffix> GetSuffixes();

        /// <summary>
        ///  tipo de apartamentos
        /// </summary>
         [OperationContract]
        List<ApartmentOrOffice> GetAparmentOrOffices();

        /// <summary>
        ///  tipo de calles
        /// </summary>
        [OperationContract]
        List<RouteType> GetRouteTypes();

        /// <summary>
        ///  tipo de construccion
        /// </summary>
        [OperationContract]
        List<ConstructionType> GetConstructionTypes();


        /// <summary>
        ///  tipo de riesgos
        /// </summary>
         [OperationContract]
        List<RiskType> GetRiskTypes();

        /// <summary>
        ///  uso del riesgo
        /// </summary>
        [OperationContract]
        List<RiskUse> GetRiskUses();

        /// <summary>
        /// polizas asociadas a la direccion
        /// </summary>
        /// <param name="street"></param>
        /// <returns></returns>
        [OperationContract]
        List<UNDTO.PolicyRiskDTO> GetPolicyRiskDTOsByStreet(string street);

    }

}
