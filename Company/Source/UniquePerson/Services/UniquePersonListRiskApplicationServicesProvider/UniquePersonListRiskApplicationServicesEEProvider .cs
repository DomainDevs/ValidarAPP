using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sistran.Company.Application.UniquePersonListRiskApplicationServices;
using Sistran.Company.Application.UniquePersonListRiskApplicationServices.DTO;
using Sistran.Company.Application.UniquePersonListRiskApplicationServices.Enum;
using Sistran.Company.Application.UniquePersonListRiskApplicationServicesProvider.Assembler;
using Sistran.Core.Application.UniquePersonListRiskApplicationServices.DTO;

namespace Sistran.Company.Application.UniquePersonListRiskApplicationServicesProvider
{
    public class UniquePersonListRiskApplicationServicesEEProvider : IUniquePersonListRiskApplicationServices
    {
        public ListRiskPersonDTO CreateListRiskPersonApplication(ListRiskPersonDTO listRiskPersonDTO)
        {
            if (listRiskPersonDTO.ListRisk.RiskListType == (int)RiskListTypeEnum.OWN)
            {
                string businessCollection = JsonConvert.SerializeObject(ModelAssembler.CreateCompanyListRisk(listRiskPersonDTO));
                Delegate.uniquePersonListRiskBusinessService.CreateListRiskTemporal(businessCollection);
                Delegate.uniquePersonListRiskBusinessService.IssueRecordListRisk(businessCollection);
            }
            else if (listRiskPersonDTO.ListRisk.RiskListType == (int)RiskListTypeEnum.OFAC)
            {
                string businessCollection = JsonConvert.SerializeObject(ModelAssembler.CreateCompanyListRiskOfac(listRiskPersonDTO));
                Delegate.uniquePersonListRiskBusinessService.CreateListRiskOfacTemporal(businessCollection);
                Delegate.uniquePersonListRiskBusinessService.IssueRecordListRiskOfac(businessCollection);
            }
            else if (listRiskPersonDTO.ListRisk.RiskListType == (int)RiskListTypeEnum.ONU)
            {
                string businessCollection = JsonConvert.SerializeObject(ModelAssembler.CreateCompanyListRiskOnu(listRiskPersonDTO));
                //Delegate.uniquePersonListRiskBusinessService.CreateListRiskOfacTemporal(businessCollection);
                Delegate.uniquePersonListRiskBusinessService.IssueRecordListRiskOnu(businessCollection);
            }
            return listRiskPersonDTO;
        }

        public List<DocumentTypeDTO> GetIdentityCardTypes()
        {
            List<DocumentTypeDTO> documentTypes = new List<DocumentTypeDTO>();
            Delegate.uniquePersonListRiskBusinessService.GetIdentityCardTypes().ForEach(x => documentTypes.Add(new DocumentTypeDTO { Id = x.Id, Description = x.Description }));
            return documentTypes.OrderBy(x => x.Description).ThenBy(x => x.Description).ToList();
        }

        public List<ListRiskDTO> GetListRisk()
        {
            List<ListRiskDTO> documentTypes = new List<ListRiskDTO>();
            Delegate.uniquePersonListRiskBusinessService.GetListRisk().ForEach(x => documentTypes.Add(new ListRiskDTO { Id = x.Id, Description = x.Description, RiskListType = x.RiskListType }));
            return documentTypes.OrderBy(x => x.Description).ThenBy(x => x.Description).ToList();
        }

        public List<ListRiskPersonDTO> GetListRiskPersonByAdvanceSearch(int documentNumber, string name, string surname, string nickName, int listRiskId)
        {
            return ModelAssembler.CreateListRiskPersonDTOList(Delegate.uniquePersonListRiskBusinessService.GetListRiskPersonList(documentNumber, name, surname, nickName, listRiskId));
        }

        public List<ListRiskPersonDTO> GetListRiskPersonByDocumentNumber(string documentNumber)
        {
            return ModelAssembler.CreateListRiskPersonDTOList(Delegate.uniquePersonListRiskBusinessService.GetListRiskPersonByDocumentNumber(documentNumber));
        }

        public ListRiskLoadDTO GenerateLoadListRisk(ListRiskLoadDTO listRiskLoadDTO)
        {
            return DTOAssembler.CreateListRiskLoad(Delegate.uniquePersonListRiskBusinessService.GenerateLoadListRisk(ModelAssembler.CreateListRiskLoad(listRiskLoadDTO)));
        }

        public ListRiskStatusDTO GetStatusByProcessId(int processId)
        {
            return DTOAssembler.CreateListRiskStatus(Delegate.uniquePersonListRiskBusinessService.GetStatusByProcessId(processId));
        }

        public string GetErrorExcelProcessListRisk(int loadProcessId)
        {
            return Delegate.uniquePersonListRiskBusinessService.GetErrorExcelProcessListRisk(loadProcessId);

        }
        public bool IntialProcessFile()
        {
            return Delegate.uniquePersonListRiskBusinessService.InitialProcessFile();
        }
        public ListRiskLoadDTO RecordListRisk(ListRiskLoadDTO listRiskLoadDTO)
        {
            return DTOAssembler.CreateListRiskLoad(Delegate.uniquePersonListRiskBusinessService.RecordListRisk(ModelAssembler.CreateListRiskLoad(listRiskLoadDTO)));
        }

        public ListRiskLoadDTO GetMassiveListRiskByProcessId(int processId)
        {
            return DTOAssembler.CreateListRiskLoad(Delegate.uniquePersonListRiskBusinessService.GetMassiveListRiskByProcessId(processId));
        }
        public List<ListRiskPersonDTO> GetAssignedListMantenance(string documentNumber, int? listRiskType)
        {
            return ModelAssembler.CreateListRiskPersonDTOList(Delegate.uniquePersonListRiskBusinessService.GetAssignedListMantenance(documentNumber, listRiskType));
        }

        public int GenerateListRiskProcessRequest(bool matchProcess, bool OnuProcess, bool ofacProcess, string searchValue, bool isMasive)
        {
            return Delegate.uniquePersonListRiskBusinessService.GenerateListRiskProcessRequest(matchProcess, OnuProcess, ofacProcess, searchValue, isMasive);
        }

        public List<ListRiskMatchDTO> ValidateListRiskPerson(string documentNumber, string fullName, int? riskListType)
        {
            return Delegate.coreUniquePersonListRiskApplicationServices.ValidateListRiskPerson(documentNumber, fullName, riskListType);
        }

        public ListRiskMatchDTO SearchProcess(string searchValue)
        {
            return ModelAssembler.CreateListRiskMatchDTO(Delegate.uniquePersonListRiskBusinessService.SearchProcess(searchValue));
        }

    }
}
