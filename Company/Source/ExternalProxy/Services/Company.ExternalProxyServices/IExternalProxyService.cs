using Sistran.Company.Application.ExternalProxyServices.InfraccionesBizTalkService;
using Sistran.Company.Application.ExternalProxyServices.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.ExternalProxyServices
{
    [ServiceContract]
    public interface IExternalProxyService
    {
        /// <summary>
        /// Metodo que realiza el grabado del log de consulta a externos.
        /// </summary>
        /// <param name="externalInfromationLog">datos de grabado</param>
        /// <returns></returns>
        [OperationContract]
        ExternalInformationLogDTO RegisterExternalInformationLog(ExternalInformationLogDTO externalInfromationLogDTO);

        /// <summary>
        /// Metodo que realiza la consulta de datacredito a través de Biztalk.
        /// </summary>
        /// <param name="RequestScoreCredit">Solicitud score crediticio</param>
        /// <returns>Objeto que contiene el resultado de la consulta de crediscore.</returns>
        [OperationContract]
        ResponseScoreCredit ExecuteWebServiceScoreCreditBiztalk(RequestScoreCredit requestScoreCredit);

        [OperationContract]
        ResponseGoodExperienceYear CalculateGoodExperienceYears(RequestGoodExperienceYear requestGoodExperienceYear);

        [OperationContract]
        bool ImplementWSDataCredit();

        [OperationContract]
        bool ImplementWSSimit();

        [OperationContract]
        ResponseInfringement ExecuteWebServiceBizTalkInfringementSimit(int DocumentType, String DocumentNumber, Guid Guid);

        [OperationContract]
        ResponseFasecoldaSISA ExecuteWebServiceSISAQueryBiztalk(RequestFasecoldaSISA requestFasecoldaSISA);

        [OperationContract]
        ResponseCexper ExecuteWebServiceCEXPERQueryBiztalk(RequestCexper requestCexper);

        [OperationContract]
        ResponseFasecoldaSISA ExecuteWebServiceSISA(RequestFasecoldaSISA requestFasecoldaSISA);

        [OperationContract]
        ResponseCexper ExecuteWebServiceCEXPER(RequestCexper requestCexper);

        [OperationContract]
        List<ResponsePerson> GetPerson2gByDocumentNumber(string documentNumber, bool company);

        [OperationContract]
        ResponsePerson GetPerson2gByPersonId(int personId, bool company);

        [OperationContract]
        ResponseCompany GetCompany2gByPersonId(int personId, bool company);

        [OperationContract]
        ResponseProvider GetProvider2g(int personId);

        [OperationContract]
        List<ResponseBankTransfer> GetBankTransfer2g(int personId);

        [OperationContract]
        List<ResponseTax> GetTax2g(int personId);

        [OperationContract]
        ResponsePolicyPayment GetPolicyPayment(RequestPolicyPayment requestPolicyPayment);

        [OperationContract]
        ResponseDocumentPayments GetPaymentsPolicyByDocuments(RequestDocumentPayments requestDocumentPayments);

        [OperationContract]
        ResponseReinsurance GetReinsurancePolicy(RequestReinsurance requestReinsurance);
    }
}
