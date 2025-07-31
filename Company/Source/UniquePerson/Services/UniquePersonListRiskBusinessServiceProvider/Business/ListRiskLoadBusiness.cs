using Newtonsoft.Json;
using Sistran.Co.Application.Data;
using Sistran.Company.Application.UniquePersonListRiskApplicationServices.Enum;
using Sistran.Company.Application.UniquePersonListRiskBusinessService.Model;
using Sistran.Company.Application.UniquePersonListRiskBusinessServiceProvider.Assembler;
using Sistran.Company.Application.UniquePersonListRiskBusinessServiceProvider.DAO;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using COMMEN = Sistran.Company.Application.Common.Entities;

namespace Sistran.Company.Application.UniquePersonListRiskBusinessServiceProvider.Business
{
    public class ListRiskLoadBusiness
    {
        public CompanyListRiskLoad UpdateProcessListRisk(CompanyListRiskLoad listRiskLoad)
        {
            ListRiskLoadDAO listRiskLoadDAO = new ListRiskLoadDAO();
            return listRiskLoadDAO.UpdateProcessListRisk(listRiskLoad);
        }

        public CompanyListRiskStatus GetStatusByProcessId(int processId)
        {

            NameValue[] parameters = new NameValue[1];
            CompanyListRiskStatus comapnyListRiskStatus = new CompanyListRiskStatus();
            parameters[0] = new NameValue("@PROCESS_ID", processId);
            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataTable("UP.GET_STATUS_LIST_RISK_PROCESS", parameters);
            }
            if (result != null && result.Rows.Count > 0)
            {
                return new CompanyListRiskStatus
                {
                    ProcessId = (int)result.Rows[0].ItemArray[0],
                    ProcessCount = (int)result.Rows[0].ItemArray[1],
                    InsertedCount = (int)result.Rows[0].ItemArray[2],
                    ProcessStatus = (int)result.Rows[0].ItemArray[3],
                    HasError = (int)result.Rows[0].ItemArray[4],
                    BeginDate = (DateTime)result.Rows[0].ItemArray[5],
                    EndDate = (result.Rows[0].ItemArray[6] == null) ? (DateTime)result.Rows[0].ItemArray[6] : new DateTime(),
                    ErrorDescription = result.Rows[0].ItemArray[8] == DBNull.Value ? string.Empty : (string)result.Rows[0].ItemArray[8]
                };
            }


            return null;
        }

        public CompanyListRiskLoad CreateAsyncronousProcess(CompanyListRiskLoad listRiskLoad)
        {
            ListRiskLoadDAO listRiskLoadDAO = new ListRiskLoadDAO();
            return listRiskLoadDAO.CreateAsyncronousProcess(listRiskLoad);
        }

        public void CreateProcessListRiskLoadRow(CompanyListRiskRow listRiskLoad)
        {
            ListRiskLoadDAO listRiskLoadDAO = new ListRiskLoadDAO();
            listRiskLoadDAO.CreateProcessListRiskLoadRow(listRiskLoad);
        }

        public void CreateListRiskTemporal(string businessCollection)
        {
            ListRiskLoadDAO listRiskLoadDAO = new ListRiskLoadDAO();
            listRiskLoadDAO.CreateListRiskTemporal(businessCollection);
        }

        public void CreateListRiskOfacTemporal(string businessCollection)
        {

            ListRiskLoadDAO listRiskLoadDAO = new ListRiskLoadDAO();
            listRiskLoadDAO.CreateListRiskOfacTemporal(businessCollection);
        }

        public void IssueRecordListRisk(string businessCollection)
        {
            CompanyListRisk listRisk = JsonConvert.DeserializeObject<CompanyListRisk>(businessCollection);
            IssueRecordListRiskReal(businessCollection, listRisk.DocumentNumber, listRisk.CreatedUser, DateTime.Now, listRisk.ProcessId, listRisk.ListRiskType, listRisk.Event, listRisk.ListRiskTypeDescription, listRisk.ListRiskId);
        }

        public void IssueRecordListRiskOfac(string businessCollection)
        {
            CompanyListRiskOfac listRisk = JsonConvert.DeserializeObject<CompanyListRiskOfac>(businessCollection);
            IssueRecordListRiskReal(businessCollection, listRisk.EntNum.ToString(), listRisk.CreatedUser, DateTime.Now, listRisk.ProcessId, listRisk.ListRiskType, listRisk.Event, listRisk.ListRiskTypeDescription, listRisk.ListRiskId);
        }

        public void IssueRecordListRiskOnu(string businessCollection)
        {
            CompanyListRiskOnu listRisk = JsonConvert.DeserializeObject<CompanyListRiskOnu>(businessCollection);
            IssueRecordListRiskReal(businessCollection, listRisk.DataId.ToString(), listRisk.CreatedUser, DateTime.Now, listRisk.ProcessId, listRisk.ListRiskType, listRisk.Event, listRisk.ListRiskTypeDescription, listRisk.ListRiskId);
        }


        public void IssueRecordListRiskReal(string companyListRiskPersonSerialized, string idCard, string userName, DateTime assignmentDate, int processId, int riskListType, int eventType, string riskListDescription, int riskListId)
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

            NameValue[] parameters = new NameValue[9];
            CompanyListRiskPerson resultOperation = new CompanyListRiskPerson();
            parameters[0] = new NameValue("@EVENT", riskListEvent);
            parameters[1] = new NameValue("@ID_CARD", idCard);
            parameters[2] = new NameValue("@USER_NAME", userName);
            parameters[3] = new NameValue("@J_MODEL", companyListRiskPersonSerialized);
            parameters[4] = new NameValue("@REGISTRATION_DATE", assignmentDate);
            parameters[5] = new NameValue("@PROCESS_ID", processId);
            parameters[6] = new NameValue("@RISK_LIST_TYPE", riskListTypeDescription);
            parameters[7] = new NameValue("@RISK_LIST_DESCRIPTION", riskListDescription);
            parameters[8] = new NameValue("@LIST_RISK_ID", riskListId);

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataTable("UP.SAVE_PERSON_LIST_RISK", parameters);
            }

            if (result != null && result.Rows.Count > 0)
            {
                var status = new CompanyListRiskStatus
                {
                    ProcessId = (int)result.Rows[0].ItemArray[0],
                    ProcessCount = (int)result.Rows[0].ItemArray[1],
                    InsertedCount = (int)result.Rows[0].ItemArray[2],
                    ProcessStatus = (int)result.Rows[0].ItemArray[3],
                    HasError = (int)result.Rows[0].ItemArray[4],
                    BeginDate = (DateTime)result.Rows[0].ItemArray[5],
                    EndDate = (result.Rows[0].ItemArray[6] == null) ? (DateTime)result.Rows[0].ItemArray[6] : new DateTime(),
                    ErrorDescription = result.Rows[0].ItemArray[8] == DBNull.Value ? string.Empty : (string)result.Rows[0].ItemArray[8]
                };

                var pendingRows = status.ProcessCount - status.InsertedCount - status.HasError;

                if (pendingRows == 0)
                {
                    Delegate.uniquePersonListRiskBusinessService.SendToRefreshOnMemoryListRisks(userName);
                }
            }
        }
        public void CreateListRiskPersonTemporal(string companyListRiskPersonSerialized, string idCard, string userName, DateTime assignmentDate, int processId, int riskListType, int eventType, string riskListDescription, int riskListId)
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

            NameValue[] parameters = new NameValue[8];

            parameters[0] = new NameValue("@EVENT", riskListEvent);
            parameters[1] = new NameValue("@ID_CARD", idCard);
            parameters[2] = new NameValue("@J_MODEL", companyListRiskPersonSerialized);
            parameters[3] = new NameValue("@REGISTRATION_DATE", assignmentDate);
            parameters[4] = new NameValue("@PROCESS_ID", processId);
            parameters[5] = new NameValue("@RISK_LIST_TYPE", riskListTypeDescription);
            parameters[6] = new NameValue("@RISK_LIST_DESCRIPTION", riskListDescription);
            parameters[7] = new NameValue("@LIST_RISK_ID", riskListId);

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataTable("UP.SAVE_TEMP_INDIVIDUAL_PERSON_LIST_RISLK", parameters);
            }
        }

        public string GetErrorExcelProcessListRisk(int loadProcessId)
        {
            CompanyListRiskLoad listRisk = new CompanyListRiskLoad();
            listRisk = GetMassiveListRiskByProcessId(loadProcessId, true);


            FileProcessValue fileProcessValue = new FileProcessValue();

            fileProcessValue.Key1 = (int)FileProcessType.ListRisk;
            fileProcessValue.Key4 = 2;
            fileProcessValue.Key5 = (int)SubCoveredRiskType.Transport;


            File file = Delegate.utilitiesService.GetFileByFileProcessValue(fileProcessValue);

            int headersCount = file.Templates.First(x => x.IsPrincipal).Rows.Count;

            file.Templates[0].Rows.Last().Fields.Add(new Field
            {
                Order = file.Templates[0].Rows.Last().Fields.Max(y => y.Order) + 1,
                ColumnSpan = 1,
                RowPosition = file.Templates[0].Rows.Last().Fields.First().RowPosition,
                FieldType = FieldType.String,
                Description = Resources.Errors.LabelError
            });
            foreach (CompanyListRiskRow listRiskRow in listRisk.Rows)
            {
                File fileSerialized = JsonConvert.DeserializeObject<File>(listRiskRow.SerializedRow);

                foreach (Template template in fileSerialized.Templates)
                {
                    if (template.IsPrincipal)
                    {
                        template.Rows.Last().Fields.Add(new Field
                        {
                            Order = template.Rows.Last().Fields.Max(y => y.Order) + 1,
                            ColumnSpan = 1,
                            RowPosition = template.Rows.Last().Fields.First().RowPosition,
                            FieldType = FieldType.String,
                            Value = template.Rows.Last().ErrorDescription
                        });

                        if (file.Templates.First(x => x.Order == 1).Rows.Count == headersCount || template.Order != 1)
                        {
                            file.Templates.First(x => x.PropertyName == template.PropertyName).Rows.AddRange(template.Rows);
                        }
                    }
                }
            }
            file.Name = "Errores_ListRisk_" + loadProcessId;
            return Delegate.utilitiesService.GenerateFile(file);

        }

        public CompanyListRiskLoad GetMassiveListRiskByProcessId(int processId, bool? withErrors)
        {
            CompanyListRiskLoad companyListRisk = new CompanyListRiskLoad();
            ListRiskLoadDAO listRiskLoadDAO = new ListRiskLoadDAO();

            companyListRisk = listRiskLoadDAO.GetLoadMassiveListRiskLoad(processId);

            if (companyListRisk != null)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(COMMEN.CiaAsynchronousProcessListriskRow.Properties.ProcessId, typeof(COMMEN.CiaAsynchronousProcessListriskRow).Name);
                filter.Equal();
                filter.Constant(processId);

                filter.And();
                filter.Property(COMMEN.CiaAsynchronousProcessListriskRow.Properties.HasError, typeof(COMMEN.CiaAsynchronousProcessListriskRow).Name);
                filter.Equal();
                filter.Constant(withErrors.Value);

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.CiaAsynchronousProcessListriskRow), filter.GetPredicate()));
                List<CompanyListRiskRow> collectiveLoadProcesses = ModelAssembler.CreateMassiveListRiskRow(businessCollection);
                companyListRisk.Rows = collectiveLoadProcesses;
            }

            return companyListRisk;
        }

        public bool IntialProcessFile()
        {
            CompanyListRiskStatus comapnyListRiskStatus = new CompanyListRiskStatus();

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataTable("UP.MOVE_LIST_RISK_TMP_TO_REAL");
            }

            if (result != null && result.Rows.Count > 0)
            {
                return true;
            }


            return false;
        }

        public CompanyListRiskModel GetAssignedListMantenance(string documentNumber, int? listRiskType)
        {
            CompanyListRiskModel resultOperation = new CompanyListRiskModel
            {
                CompanyRiskListOwn = new List<CompanyListRisk>(),
                CompanyRiskListOfac = new List<CompanyListRiskOfac>(),
                CompanyListRiskOnu = new List<CompanyListRiskOnu>()
            };

            DataTable result;
            NameValue[] parameters = new NameValue[2];

            parameters[0] = new NameValue("@RISK_LIST_TYPE", listRiskType ?? -1);
            parameters[1] = new NameValue("@ID_CARD_NO", documentNumber ?? "");

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataTable("UP.GET_RISK_LIST_PERSON", parameters);
            }
            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow item in result.Rows)
                {
                    if ((string)item.ItemArray[2] == RiskListConstants.OWN)
                    {
                        CompanyListRisk listRisk = JsonConvert.DeserializeObject<CompanyListRisk>((string)item.ItemArray[1]);
                        listRisk.Id = int.Parse(item.ItemArray[0].ToString());
                        listRisk.ListRiskDescription = (string)item.ItemArray[5];
                        listRisk.ListRiskTypeDescription = RiskListConstants.OWN;
                        listRisk.ProcessId = int.Parse(item.ItemArray[4].ToString());
                        listRisk.ListRiskId = int.Parse(item.ItemArray[7].ToString());
                        listRisk.IssueDate = DateTime.Parse(item.ItemArray[6].ToString());
                        listRisk.AssignmentDate = listRisk.AssignmentDate;

                        if ((string)item.ItemArray[3] == RiskListConstants.Included)
                        {
                            listRisk.Event = (int)RiskListEventEnum.INCLUDED;
                        }
                        else
                        {
                            listRisk.Event = (int)RiskListEventEnum.EXCLUDED;
                        }
                        resultOperation.CompanyRiskListOwn.Add(listRisk);
                    }
                    else if ((string)item.ItemArray[2] == RiskListConstants.OFAC)
                    {
                        CompanyListRiskOfac listRisk = JsonConvert.DeserializeObject<CompanyListRiskOfac>((string)item.ItemArray[1]);
                        listRisk.Id = int.Parse(item.ItemArray[0].ToString());
                        listRisk.ListRiskDescription = (string)item.ItemArray[5];
                        listRisk.ListRiskTypeDescription = RiskListConstants.OFAC;
                        listRisk.ProcessId = int.Parse(item.ItemArray[4].ToString());
                        listRisk.ListRiskId = int.Parse(item.ItemArray[7].ToString());
                        listRisk.AssignmentDate = DateTime.Parse(item.ItemArray[6].ToString());

                        if ((string)item.ItemArray[3] == RiskListConstants.Included)
                        {
                            listRisk.Event = (int)RiskListEventEnum.INCLUDED;
                        }
                        else
                        {
                            listRisk.Event = (int)RiskListEventEnum.EXCLUDED;
                        }
                        resultOperation.CompanyRiskListOfac.Add(listRisk);
                    }
                    else if ((string)item.ItemArray[2] == RiskListConstants.ONU)
                    {
                        CompanyListRiskOnu listRisk = JsonConvert.DeserializeObject<CompanyListRiskOnu>((string)item.ItemArray[1]);
                        listRisk.Id = int.Parse(item.ItemArray[0].ToString());
                        listRisk.ListRiskDescription = (string)item.ItemArray[5];
                        listRisk.ListRiskTypeDescription = RiskListConstants.ONU;
                        listRisk.ProcessId = int.Parse(item.ItemArray[4].ToString());
                        listRisk.ListRiskId = int.Parse(item.ItemArray[7].ToString());
                        listRisk.AssignmentDate = DateTime.Parse(item.ItemArray[6].ToString());

                        if ((string)item.ItemArray[3] == RiskListConstants.Included)
                        {
                            listRisk.Event = (int)RiskListEventEnum.INCLUDED;
                        }
                        else
                        {
                            listRisk.Event = (int)RiskListEventEnum.EXCLUDED;
                        }
                        resultOperation.CompanyListRiskOnu.Add(listRisk);
                    }
                }
            }
            return resultOperation;
        }
    }
}
