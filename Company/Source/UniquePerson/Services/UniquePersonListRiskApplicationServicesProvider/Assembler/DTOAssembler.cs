using Sistran.Company.Application.UniquePersonListRiskBusinessService.Model;
using Sistran.Company.Application.UniquePersonListRiskApplicationServices.DTO;

namespace Sistran.Company.Application.UniquePersonListRiskApplicationServicesProvider.Assembler
{
    public class DTOAssembler
    {
        public static ListRiskLoadDTO CreateListRiskLoad(CompanyListRiskLoad companyListRiskLoad)
        {
            if (companyListRiskLoad == null)
            {
                return null;
            }

            return new ListRiskLoadDTO
            {
                ProcessId = companyListRiskLoad.ProcessId,
                HasError = companyListRiskLoad.HasError,
                ProcessStatus = companyListRiskLoad.ProcessStatus,
                ListRisk = new ListRiskDTO { 
                    Id = companyListRiskLoad.ListRiskId, 
                    Description = companyListRiskLoad.Description,
                    RiskListType = companyListRiskLoad.RiskListType
                }
            };
        }

        public static ListRiskStatusDTO CreateListRiskStatus(CompanyListRiskStatus listRiskStatus)
        {
            return new ListRiskStatusDTO
            {
                BeginDate = listRiskStatus.BeginDate,
                EndDate = listRiskStatus.EndDate,
                HasError = listRiskStatus.HasError,
                InsertedCount = listRiskStatus.InsertedCount,
                ProcessCount = listRiskStatus.ProcessCount,
                ProcessId = listRiskStatus.ProcessId,
                ProcessStatus = listRiskStatus.ProcessStatus,
                ErrorDescription = listRiskStatus.ErrorDescription
            };

        }
    }
}
