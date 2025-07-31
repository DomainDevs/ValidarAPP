using Sistran.Company.Application.CommonServices.Models;
using Sistran.Core.Application.CommonService.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.CommonServices
{
    [ServiceContract]
    public interface ICommonService: Sistran.Core.Application.CommonService.ICommonServiceCore
    {
        /// <summary>
        /// Get all of the sub line business items 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SubLineBusiness> GetSubLineBusiness();

        [OperationContract]
        List<CompositionType> GetCompositionTypes();

        [OperationContract]
        List<DetailType> GetDetailTypes();

        [OperationContract]
        List<Nomenclature> GetNomenclatures();

        [OperationContract]
        CompanyParameter FindCoParameter(int parameterId);

        [OperationContract]
        /// <summary>
        /// Get all list Abreviature of Nomenclature 
        /// </summary>
        /// <returns></returns>
        List<Nomenclature> GetNomenclaturesTask(int id, string Nomenclature, string Abreviature, bool getAllData);

        [OperationContract]
        /// <summary>
        /// Get Abreviature of Nomenclature 
        /// </summary>
        /// <returns></returns>
        List<Nomenclature> GetTransformAddress(string Nomenclature);
    }
}
