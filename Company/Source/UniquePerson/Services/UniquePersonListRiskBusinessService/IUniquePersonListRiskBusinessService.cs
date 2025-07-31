using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Company.Application.UniquePersonListRiskBusinessService.Model;

namespace Sistran.Company.Application.UniquePersonListRiskBusinessService
{
    [ServiceContract]
    public interface IUniquePersonListRiskBusinessService
    {
        [OperationContract]
        CompanyListRiskPerson CreateListRiskPersonBusiness(CompanyListRiskPerson listRisk);

        [OperationContract]
        List<CompanyListRiskPerson> GetListRiskPersonList(int documentNumber, string name, string surname, string nickName, int listRiskId);

        [OperationContract]
        List<IdentityCardTypes> GetIdentityCardTypes();

        [OperationContract]
        List<RiskListModel> GetListRisk();

        [OperationContract]
        List<CompanyListRiskPerson> GetListRiskPersonByDocumentNumber(string documentNumber);

        /// <summary>
        /// Metodo que crea la carga de la lista de riesgos
        /// </summary>
        /// <param name="listRiskLoad"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyListRiskLoad GenerateLoadListRisk(CompanyListRiskLoad listRiskLoad);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listRiskLoad"></param>
        /// <returns></returns>
        [OperationContract]
        void CreateListRiskTemporal(string businessCollection);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listRiskLoad"></param>
        /// <returns></returns>
        [OperationContract]
        void CreateListRiskOfacTemporal(string businessCollection);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listRiskLoad"></param>
        /// <returns></returns>
        [OperationContract]
        void IssueRecordListRisk(string businessCollection);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listRiskLoad"></param>
        /// <returns></returns>
        [OperationContract]
        void IssueRecordListRiskOfac(string businessCollection);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listRiskLoad"></param>
        /// <returns></returns>
        [OperationContract]
        void IssueRecordListRiskOnu(string businessCollection);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyListRiskStatus GetStatusByProcessId(int processId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadProcessId"></param>
        /// <returns></returns>
        [OperationContract]
        string GetErrorExcelProcessListRisk(int loadProcessId);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool InitialProcessFile();

        [OperationContract]
        CompanyListRiskLoad RecordListRisk(CompanyListRiskLoad listRiskProccessId);

        [OperationContract]
        CompanyListRiskLoad GetMassiveListRiskByProcessId(int processId);


        [OperationContract]
        CompanyListRiskModel GetAssignedListMantenance(string documentNumber, int? listRisk);

        [OperationContract]
        int GenerateListRiskProcessRequest(bool matchProcess, bool OnuProcess, bool ofacProcess, string searchValue, bool isMasive);

        [OperationContract]
        RiskListMatch SearchProcess(string searchValue);
    }
}
