using Sistran.Company.Application.UniquePersonListRiskApplicationServices.DTO;
using Sistran.Core.Application.UniquePersonListRiskApplicationServices.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonListRiskApplicationServices
{
    [ServiceContract]
    public interface IUniquePersonListRiskApplicationServices
    {
        [OperationContract]
        ListRiskPersonDTO CreateListRiskPersonApplication(ListRiskPersonDTO listRiskPersonDTO);
        [OperationContract]

        List<ListRiskPersonDTO> GetListRiskPersonByAdvanceSearch(int documentNumber, string name, string surname, string nickName, int listRiskId);

        [OperationContract]
        List<DocumentTypeDTO> GetIdentityCardTypes();


        [OperationContract]
        List<ListRiskDTO> GetListRisk();

        [OperationContract]
        List<ListRiskPersonDTO> GetListRiskPersonByDocumentNumber(string documentNumber);

        /// <summary>
        /// Crea la carga de la lista de riesgos
        /// </summary>
        /// <param name="listRiskLoadDTO"></param>
        /// <returns></returns>
        [OperationContract]
        ListRiskLoadDTO GenerateLoadListRisk(ListRiskLoadDTO listRiskLoadDTO);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        [OperationContract]
        ListRiskStatusDTO GetStatusByProcessId(int processId);
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
         bool IntialProcessFile();

        [OperationContract]
        ListRiskLoadDTO RecordListRisk(ListRiskLoadDTO listRiskLoadDTO);

        [OperationContract]
        ListRiskLoadDTO GetMassiveListRiskByProcessId(int processId);

        [OperationContract]
        List<ListRiskPersonDTO> GetAssignedListMantenance(string documentNumber, int? listRiskType);

        [OperationContract]
        int GenerateListRiskProcessRequest(bool matchProcess, bool OnuProcess, bool ofacProcess, string searchValue, bool isMasive);

        [OperationContract]
        List<ListRiskMatchDTO> ValidateListRiskPerson(string documentNumber, string fullName, int? riskListType);

        [OperationContract]
        ListRiskMatchDTO SearchProcess(string searchValue);
    }
}
