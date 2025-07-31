using Sistran.Core.Application.Location.PropertyServices.Models;
using System.Collections.Generic;
using System.ServiceModel;
namespace Sistran.Core.Application.Location.PropertyServices
{
    [ServiceContract]
    public interface IPropertyServiceCore:Sistran.Core.Application.Locations.ILocations
    {
        #region Claims

        [OperationContract]
        List<PropertyRisk> GetRiskPropertiesByInsuredId(int insuredId);

        [OperationContract]
        List<PropertyRisk> GetRiskPropertiesByEndorsementId(int endorsementId);

        [OperationContract]
        PropertyRisk GetRiskPropertyByRiskId(int riskId);

        [OperationContract]
        List<PropertyRisk> GetRiskPropertiesByAddress(string adderess);

        #endregion
    }
}
