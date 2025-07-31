using System;
using System.ServiceModel;
using Sistran.Co.Previsora.Application.FullServices.Models;
using System.Collections.Generic;
using System.Data;

namespace Sistran.Co.Previsora.Application.FullServices
{
    [ServiceContract]
    public interface IFullServicesSUP
    {
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        bool GetStatusAplication(StatusAplication statusaplication);
        
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        List<RoleView> GetlistViews(int idRole);        

        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        DtoMaster GetDto(int id_rol, string cod_rol, string entity);              

        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        SUPMessages SetDto(int id_rol, DtoMaster MasterDto);

        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        DataSet GenericQuery(string generic, List<Parameters> ListParameter);

        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        List<ResponseSearch> SearchPerson(ResquestSearch resquestSearch);

        /*[OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        DtoMasterThird GetDtoThird(int id_rol, string cod_rol, string entity);

        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        SUPMessages SetDtoThird(int id_rol, DtoMasterThird MasterDto);*/

        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        double GetExchangeRateDate(DateTime operatingQuotaExchangeRateDate, int currencyCd);

        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        List<OperatingQuotaIndividual> GetIndividualOperatingQuota(int identificationType, string identificationId);


        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        List<TableMessage> ModifyOperatingQuota(List<OPERATING_QUOTA> operatingQuota, List<TableMessage> listTableMessage);

        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        OperatingQuotaResponse RegisterOperativeQuota(WSOperatingQuota operatingQuota);

    }
}
