using OnuListRisk.Enum;
using OnuListRisk.Models;
using Sistran.Co.Application.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnuListRisk.Business
{
    class OnuListRiskBusiness
    {
        public void IssueRecordListRiskReal(string companyListRiskPersonSerialized, string idCard, string userName, DateTime assignmentDate, int processId, int riskListType, int eventType, string riskListDescription)
        {
            string riskListTypeDescription = "";
            string riskListEvent = RiskListConstants.Included;

            if (eventType == (int)RiskListEventEnum.EXCLUDED)
            {
                riskListEvent = RiskListConstants.Excluded;
            }

            switch (riskListType)
            {
                case (int)RiskListTypeEnum.OWN:
                    riskListTypeDescription = RiskListConstants.OWN;
                    break;
                case (int)RiskListTypeEnum.OFAC:
                    riskListTypeDescription = RiskListConstants.OFAC;
                    break;
                case (int)RiskListTypeEnum.ONU:
                    riskListTypeDescription = RiskListConstants.ONU;
                    break;
            }

            NameValue[] parameters = new NameValue[8];

            parameters[0] = new NameValue("@EVENT", riskListEvent);
            parameters[1] = new NameValue("@ID_CARD", idCard);
            parameters[2] = new NameValue("@USER_NAME", userName);
            parameters[3] = new NameValue("@J_MODEL", companyListRiskPersonSerialized);
            parameters[4] = new NameValue("@REGISTRATION_DATE", assignmentDate);
            parameters[5] = new NameValue("@PROCESS_ID", processId);
            parameters[6] = new NameValue("@RISK_LIST_TYPE", riskListTypeDescription);
            parameters[7] = new NameValue("@RISK_LIST_DESCRIPTION", riskListTypeDescription);

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.SAVE_PERSON_LIST_RISK", parameters);
            }
        }
        public void CreateListRiskPersonTemporal(string companyListRiskPersonSerialized, string idCard, string userName, DateTime assignmentDate, int processId, int riskListType, int eventType, string riskListDescription)
        {
            string riskListTypeDescription = "";
            string riskListEvent = "";

            if (eventType == (int)RiskListEventEnum.EXCLUDED)
            {
                riskListEvent = RiskListConstants.Excluded;
            }
            else
            {
                riskListEvent = RiskListConstants.Included;
            }

            switch (riskListType)
            {
                case (int)RiskListTypeEnum.OWN:
                    riskListTypeDescription = RiskListConstants.OWN;
                    break;
                case (int)RiskListTypeEnum.OFAC:
                    riskListTypeDescription = RiskListConstants.OFAC;
                    break;
                case (int)RiskListTypeEnum.ONU:
                    riskListTypeDescription = RiskListConstants.ONU;
                    break;
            }

            NameValue[] parameters = new NameValue[7];

            parameters[0] = new NameValue("@EVENT", riskListEvent);
            parameters[1] = new NameValue("@ID_CARD", idCard);
            parameters[2] = new NameValue("@J_MODEL", companyListRiskPersonSerialized);
            parameters[3] = new NameValue("@REGISTRATION_DATE", assignmentDate);
            parameters[4] = new NameValue("@PROCESS_ID", processId);
            parameters[5] = new NameValue("@RISK_LIST_TYPE", riskListTypeDescription);
            parameters[6] = new NameValue("@RISK_LIST_DESCRIPTION", riskListDescription);

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.SAVE_TEMP_INDIVIDUAL_PERSON_LIST_RISLK", parameters);
            }
        }
        public List<OnuList> GetOnuLists()
        {
            List<OnuList> onuList = new List<OnuList>();

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.GET_ONU_LISTS");
            }
            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow objectResult in result.Rows)
                {
                    OnuList onuObject = new OnuList
                    {
                        Guid = (Guid)objectResult.ItemArray[0],
                        ShaCode = (string)objectResult.ItemArray[1],
                        ProcessId = (int)objectResult.ItemArray[2],
                        RegistrationDate = (DateTime)objectResult.ItemArray[3]

                    };
                    onuList.Add(onuObject);
                }

            }
            return onuList;
        }
        public OnuList CreateOnuList(OnuList onuList)
        {
            NameValue[] parameters = new NameValue[4];

            parameters[0] = new NameValue("@PROCESS_ID", onuList.ProcessId);
            parameters[1] = new NameValue("@REGISTRATION_DATE", onuList.RegistrationDate);
            parameters[2] = new NameValue("@SHA_CODE", onuList.ShaCode == null ? "0" : onuList.ShaCode);
            parameters[3] = new NameValue("@STATUS_ID", onuList.StatusId);

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.CREATE_ONU_LIST", parameters);
            }
            if (result != null && result.Rows.Count > 0)
            {

                onuList.Guid = (Guid)result.Rows[0].ItemArray[0];
                onuList.ShaCode = (string)result.Rows[0].ItemArray[1];
                onuList.ProcessId = (int)result.Rows[0].ItemArray[2];
                onuList.RegistrationDate = (DateTime)result.Rows[0].ItemArray[3];

            }
            return onuList;
        }
        public void UpdateOnuList(OnuList onuList)
        {
            NameValue[] parameters = new NameValue[5];

            parameters[0] = new NameValue("@GUID", onuList.Guid);
            parameters[1] = new NameValue("@SHA_CODE", onuList.ShaCode == null ? "0" : onuList.ShaCode);
            parameters[2] = new NameValue("@PROCESS_ID", onuList.ProcessId);
            parameters[3] = new NameValue("@REGISTRATION_DATE", onuList.RegistrationDate);
            parameters[4] = new NameValue("@STATUS_ID", onuList.StatusId);

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.UPDATE_ONU_LIST", parameters);
            }
        }
        public int CreateAsyncProcess(string processGuid, int totalRows)
        {
            int processId = 0;

            NameValue[] parameters = new NameValue[4];
            parameters[0] = new NameValue("@FILE_NAME", processGuid);
            parameters[1] = new NameValue("@LIST_CODE_ID", (int)RiskListTypeEnum.ONU);
            parameters[2] = new NameValue("@RISK_LIST_TYPE_CD", (int)RiskListTypeEnum.ONU);
            parameters[3] = new NameValue("@TOTAL_ROWS", totalRows);


            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.CREATE_ASYNC_PROCESS", parameters);
            }
            if (result != null && result.Rows.Count > 0)
            {
                processId = (int)result.Rows[0].ItemArray[0];
            }
            return processId;
        }
        public void RegisterOnuListLog(OnuList onuList)
        {
            NameValue[] parameters = new NameValue[5];
            parameters[0] = new NameValue("@GUID", onuList.Guid);
            parameters[1] = new NameValue("@STATUS_DESCRIPTION", (ProcessStatusEnum)onuList.StatusId);
            parameters[2] = new NameValue("@STATUS_CODE", onuList.StatusId);
            parameters[3] = new NameValue("@REGISTRATION_DATE", DateTime.Now);
            parameters[4] = new NameValue("@ERROR_DESCRIPTION", onuList.Error == null ? String.Empty : onuList.Error);
            

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.CREATE_ONU_LIST_PROCESS_LOG", parameters);
            }

        }
        public bool ValidateProcessFinish(int processId)
        {
            return false;
        }
    }
}
