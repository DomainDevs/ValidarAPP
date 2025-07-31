using Sistran.Company.Application.UniquePersonListRiskBusinessService.Model;
using COMMEN = Sistran.Company.Application.Common.Entities;

namespace Sistran.Company.Application.UniquePersonListRiskBusinessServiceProvider.Assembler
{
    public class EntityAssembler
    {
        internal static COMMEN.AsynchronousProcess CreateAsynchronousProcess(CompanyListRiskLoad listRiskLoad)
        {
            COMMEN.AsynchronousProcess asynchronousProcess = new COMMEN.AsynchronousProcess
            {
                Description = listRiskLoad.Description,
                BeginDate = listRiskLoad.BeginDate,
                EndDate = listRiskLoad.EndDate,
                UserId = listRiskLoad.User.UserId,
                Status = listRiskLoad.Status,
                HasError = listRiskLoad.HasError,
                ErrorDescription = listRiskLoad.Error_Description
            };

            if (listRiskLoad.ProcessId > 0)
            {
                asynchronousProcess.ProcessId = listRiskLoad.ProcessId;
            }

            return asynchronousProcess;
        }

        internal static COMMEN.CiaAsynchronousProcessListRiskMassiveLoad CreateProcessListRiskMassiveLoad(CompanyListRiskLoad listRiskLoad)
        {
            COMMEN.CiaAsynchronousProcessListRiskMassiveLoad entityListRiskMassiveLoad = new COMMEN.CiaAsynchronousProcessListRiskMassiveLoad
            {
                TotalRows = listRiskLoad.TotalRows,
                FileName = listRiskLoad.File.Name,
                ListCodeId = listRiskLoad.ListRiskId ,
                RiskListTypeCd = listRiskLoad.RiskListType
            };

            if (listRiskLoad.ProcessId > 0)
            {
                entityListRiskMassiveLoad.ProcessId = listRiskLoad.ProcessId;
            }

            return entityListRiskMassiveLoad;
        }

        internal static COMMEN.CiaAsynchronousProcessListRiskStatus CreateCiaAsynchronousProcessStatus(CompanyListRiskLoad listRiskLoad)
        {
            return new COMMEN.CiaAsynchronousProcessListRiskStatus
            {
                ProcessId = listRiskLoad.ProcessId,
                ProcessStatusId = (int)listRiskLoad.ProcessStatus,
             
                BeginDate = listRiskLoad.BeginDate,
                EndDate = listRiskLoad.EndDate
            };
        }

        internal static COMMEN.CiaAsynchronousProcessListriskRow CreateProcessListRiskRow(CompanyListRiskRow listRiskRow)
        {
            return new COMMEN.CiaAsynchronousProcessListriskRow
            {
                ListriskMassiveLoadId = listRiskRow.ProcessMassiveLoadId,
                ProcessId = listRiskRow.ProcessId,
                RowNumber = listRiskRow.RowNumber,
                HasError = listRiskRow.HasError,
                IdCardNo = listRiskRow.IdCardNo,
                ErrorDescription = listRiskRow.Error_Description,
                SerializedRow = listRiskRow.SerializedRow
            };
        }
    }
}
