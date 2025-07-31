using Sistran.Company.ExternalParamService.Models;
using SIstran.Company.ExternalParamService.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.ExternalParamService
{
    [ServiceContract]
    [XmlSerializerFormat]
    public interface IExternalParamService
    {
        [OperationContract]
        List<QuoteParamIdentityCardClass> GenerateIdentityCardTypeList();

        [OperationContract]
        List<ParamAddressType> GenerateListAdressType();

        [OperationContract]
        List<ParamCompanyType> GenerateListCompanyType();

        [OperationContract]
        List<ParamCompanyType> GenerateListDetail();

        [OperationContract]
        List<ParamEconomicActivity> GenerateListEconomicActivity();

        [OperationContract]
        List<ParamListProduct> GenerateListProduct(int prefixCd);

        [OperationContract]
        List<ParamListRestrictive> GenerateListRestrictive();

        [OperationContract]
        List<QuoteParamRatingZoneClass> GenerateRatingZoneList();

        [OperationContract]
        QuoteParamVehicleFasecoldaClass GenerateVehicleFasecolda(string fasecoldaCd, int yearVehicle);

        [OperationContract]
        List<QuoteParamVehicleClass> GenerateVehiclesList(int vehicleYearInit, int vehicleYearEnd);

        [OperationContract]
        List<PhoneTypeClass> GenereteListPhoneType();

        [OperationContract]
        AgentModel GetAgent(int agentCode);

        [OperationContract]
        List<ParamAssistanceClass> GetListAssistance(int prefixCd, int productCd);

        [OperationContract]
        List<ParamAssistanceCoverClass> GetListAssistanceCover(int assistanceCd, int prefixCd);

        [OperationContract]
        LastDateClass GetModifyInfo();

        [OperationContract]
        ProductLimitAmtModel GetProductLimitAmt(int productId);

        [OperationContract]
        List<ParamListCountryStateCity> GenerateListCountryStateCity();

    }
}
