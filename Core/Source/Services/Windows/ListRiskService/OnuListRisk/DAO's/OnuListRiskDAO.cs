using Newtonsoft.Json;
using OnuListRisk.Enum;
using OnuListRisk.Models;
using Sistran.Co.Application.Data;
using Sistran.Core.Framework.Queues;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Utilities.Excel;
using Utilities.Mail;

namespace OnuListRisk.Business
{
    class OnuListRiskDAO
    {
        public string SendToRefreshOnMemoryListRisks()
        {
            try
            {
                string exchangeName = "ListRiskCache.fanout";
                string json = JsonConvert.SerializeObject(new { UserName = "ONU_SERVICE_SYSTEM" });

                string queueName = $"{Environment.MachineName} - ListRiskCache";
                Dictionary<string, object> args = new Dictionary<string, object>
                {
                    { ArgumentsNames.DeadLetterExchange, exchangeName },
                    { ArgumentsNames.MaxLength, 500 }
                };

                CreateQueueParameters createQueueParameters = new CreateQueueParameters
                {
                    QueueName = queueName,
                    ExchangeName = exchangeName,
                    ExchangeType = "fanout",
                    RoutingKey = string.Empty,
                    PersistentMessages = true,
                    Arguments = args
                };

                createQueueParameters.Serialization = ConfigurationManager.AppSettings["Serialization"];
                IQueue queue = new BaseQueueFactory().CreateQueue(createQueueParameters);
                queue.PutOnQueue(json);
                return "Se envio de manera exitosa el Refresh de la cache (Listas de Riesgo) hacia SISE3G";
            }
            catch (Exception ex)
            {
                return "No se pudo enviar el Refresh de la cache (Listas de Riesgo) hacia SISE3G";
            }
        }

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

            NameValue[] parameters = new NameValue[9];

            parameters[0] = new NameValue("@EVENT", riskListEvent);
            parameters[1] = new NameValue("@ID_CARD", idCard);
            parameters[2] = new NameValue("@USER_NAME", userName);
            parameters[3] = new NameValue("@J_MODEL", companyListRiskPersonSerialized);
            parameters[4] = new NameValue("@REGISTRATION_DATE", assignmentDate);
            parameters[5] = new NameValue("@PROCESS_ID", processId);
            parameters[6] = new NameValue("@RISK_LIST_TYPE", riskListTypeDescription);
            parameters[7] = new NameValue("@RISK_LIST_DESCRIPTION", riskListTypeDescription);
            parameters[8] = new NameValue("@LIST_RISK_ID", 3);

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

            NameValue[] parameters = new NameValue[8];

            parameters[0] = new NameValue("@EVENT", riskListEvent);
            parameters[1] = new NameValue("@ID_CARD", idCard);
            parameters[2] = new NameValue("@J_MODEL", companyListRiskPersonSerialized);
            parameters[3] = new NameValue("@REGISTRATION_DATE", assignmentDate);
            parameters[4] = new NameValue("@PROCESS_ID", processId);
            parameters[5] = new NameValue("@RISK_LIST_TYPE", riskListTypeDescription);
            parameters[6] = new NameValue("@RISK_LIST_DESCRIPTION", riskListDescription);
            parameters[7] = new NameValue("@LIST_RISK_ID", 3);

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
        public void UpdateProcessRequest(OnuList onuList)
        {
            NameValue[] parameters = new NameValue[4];

            parameters[0] = new NameValue("@REQUEST_ID", onuList.RequestId);
            parameters[1] = new NameValue("@PROCESS_ID", onuList.ProcessId <= 0 ? -1 : onuList.ProcessId);
            parameters[2] = new NameValue("@DESCRIPTION", (OnuProcessStatusEnum)onuList.StatusId);
            parameters[3] = new NameValue("@STATUS", onuList.StatusId);

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.UPDATE_LIST_RISK_PROCESS", parameters);
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
            parameters[1] = new NameValue("@STATUS_DESCRIPTION", (OnuProcessStatusEnum)onuList.StatusId);
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
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("@PROCESS_ID", processId);

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.VALIDATE_ONU_PROCESS_FINISH", parameters);
            }
            if (result != null && result.Rows.Count > 0)
            {
                return Convert.ToBoolean(result.Rows[0].ItemArray[0]);
            }
            else
            {
                return false;
            }
        }
        public string CreateListReport(OnuList onuList, string path)
        {
            ExcelService excelService = new ExcelService();
            List<OnuPerson> onuPeople = new List<OnuPerson>();


            FileProcessValue fileProcessValue = new FileProcessValue
            {
                Key1 = (int)FileProcessType.MassiveReport,
                Key4 = 9,
                Key5 = 5
            };

            onuPeople = this.GetListByProcessId(onuList.ProcessId);

            File file = this.GetFileIdByFileProcessValue(fileProcessValue);
            file.Name = "Reporte Onu - " + onuList.ProcessId;

            List<Field> statusFields = file.Templates.Where(x => x.Description == "Resumen de Cargue").FirstOrDefault().Rows.FirstOrDefault().Fields;
            statusFields.Find(u => u.PropertyName == "ProcessNumber").Value = onuList.ProcessId.ToString();
            statusFields.Find(u => u.PropertyName == "StartProcessDate").Value = onuList.RegistrationDate.ToString();
            statusFields.Find(u => u.PropertyName == "EndProcessDate").Value = DateTime.Now.ToString();
            statusFields.Find(u => u.PropertyName == "Estado").Value = Convert.ToString((OnuProcessStatusEnum)onuList.StatusId);
            statusFields.Find(u => u.PropertyName == "TotalRecords").Value = onuPeople.Count.ToString();
            statusFields.Find(u => u.PropertyName == "TotalErrorRecords").Value = ((onuList.PersonCount - onuPeople.Count) < 0 ? 0 : (onuList.PersonCount - onuPeople.Count)).ToString();

            List<Row> personRows = new List<Row>();

            string serializedFields = JsonConvert.SerializeObject(file.Templates.Where(x => x.Description == "Detalle de Cargue").FirstOrDefault().Rows.FirstOrDefault().Fields);

            foreach (OnuPerson person in onuPeople)
            {
                List<Field> personFields = JsonConvert.DeserializeObject<List<Field>>(serializedFields);
                Row personRow = new Row();

                personFields.Find(u => u.PropertyName == "Estado").Value = Convert.ToString(OnuProcessStatusEnum.Procesado);
                personFields.Find(u => u.PropertyName == "RegisterNumber").Value = person.DataId.ToString();
                personFields.Find(u => u.PropertyName == "PersonModel").Value = JsonConvert.SerializeObject(person);
                personFields.Find(u => u.PropertyName == "RegistrationDate").Value = person.SiseReistrationDate.ToString();
                personFields.Find(u => u.PropertyName == "ErrDescription").Value = string.Empty;

                personRow.Fields = new List<Field>(personFields);
                personRows.Add(personRow);
            }
            file.Templates.Where(x => x.Description == "Detalle de Cargue").FirstOrDefault().Rows.AddRange(personRows);

            excelService.CreateListRiskReport(file, path);
            return file.Name;
        }
        public File GetFileIdByFileProcessValue(FileProcessValue fileProcessValue)
        {
            int fileId = 0;
            NameValue[] parameters = new NameValue[5];

            parameters[0] = new NameValue("@KEY_1", fileProcessValue.Key1);
            parameters[1] = new NameValue("@KEY_2", fileProcessValue.Key2);
            parameters[2] = new NameValue("@KEY_3", fileProcessValue.Key3);
            parameters[3] = new NameValue("@KEY_4", fileProcessValue.Key4);
            parameters[4] = new NameValue("@KEY_5", fileProcessValue.Key5);

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.GET_FILE_ID_BY_FILE_PROCESS_VALUE", parameters);
            }
            if (result != null && result.Rows.Count > 0)
            {
                fileId = (int)result.Rows[0].ItemArray[0];
            }
            return this.GetFileByFileId(fileId);
        }
        public List<OnuPerson> GetListByProcessId(int processId)
        {
            List<OnuPerson> onuPeople = new List<OnuPerson>();

            NameValue[] parameters = new NameValue[1];

            parameters[0] = new NameValue("@PROCESS_ID", processId);

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.GET_LIST_BY_PROCESS_ID", parameters);
            }
            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow objectResult in result.Rows)
                {
                    OnuPerson onuPerson = new OnuPerson();
                    onuPerson = JsonConvert.DeserializeObject<OnuPerson>(objectResult.ItemArray[5].ToString());
                    onuPerson.SiseReistrationDate = (DateTime)objectResult.ItemArray[6];
                    onuPeople.Add(onuPerson);
                }

            }


            return onuPeople;
        }
        public File GetFileByFileId(int fileId)
        {
            File file = new File();

            NameValue[] parameters = new NameValue[1];

            parameters[0] = new NameValue("@FILE_ID", fileId);

            DataSet result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess("SistranEE"))
            {
                result = dynamicDataAccess.ExecuteSPDataSet("UP.GET_FILE_BY_FILE_ID", parameters);
            }

            if (result != null)
            {
                foreach (DataTable dataTable in result.Tables)
                {

                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (dataTable.Columns.Contains("FILE_ID"))
                        {
                            file.Id = Convert.ToInt32(row["FILE_ID"].ToString() != string.Empty ? row["FILE_ID"] : 0);
                            file.Description = row["FILE_DESCRIPTION"].ToString();
                            file.SmallDescription = row["FILE_SMALL_DESCRIPTION"].ToString();
                            file.Observations = row["FILE_OBSERVATIONS"].ToString();
                            file.FileType = (FileType)Convert.ToInt32(row["FILE_TYPE_ID"].ToString() != string.Empty ? row["FILE_TYPE_ID"] : 0);
                            file.IsEnabled = Convert.ToBoolean(row["IS_ENABLED"].ToString());
                            file.Templates = new List<Template>();
                        }
                        else if (dataTable.Columns.Contains("TEMPLATE_ID"))
                        {
                            file.Templates.Add(new Template()
                            {
                                Id = Convert.ToInt32(row["ID"].ToString() != string.Empty ? row["ID"] : 0),
                                TemplateId = Convert.ToInt32(row["TEMPLATE_ID"].ToString() != string.Empty ? row["TEMPLATE_ID"] : 0),
                                IsMandatory = Convert.ToBoolean(row["IS_MANDATORY"].ToString()),
                                IsEnabled = Convert.ToBoolean(row["IS_ENABLED"].ToString()),
                                Order = Convert.ToInt32(row["ORDERING"].ToString() != string.Empty ? row["ORDERING"] : 0),
                                IsPrincipal = Convert.ToBoolean(row["IS_PRINCIPAL"].ToString()),
                                Description = row["DESCRIPTION"].ToString(),
                                Rows = new List<Row>()
                            });
                        }
                        else if (dataTable.Columns.Contains("FILE_TEMPLATE_ID"))
                        {
                            if (file.Templates.Where(x => x.Id == (int)row["FILE_TEMPLATE_ID"]).FirstOrDefault().Rows.Count == 0)
                            {
                                file.Templates.Where(x => x.Id == (int)row["FILE_TEMPLATE_ID"]).FirstOrDefault().Rows.Add(new Row()
                                {
                                    Fields = new List<Field>()
                                });
                            }

                            Field field = new Field()
                            {
                                Id = Convert.ToInt32(row["FIELD_ID"].ToString() != string.Empty ? row["FIELD_ID"] : 0),
                                Order = Convert.ToInt32(row["ORDERING"].ToString() != string.Empty ? row["ORDERING"] : 0),
                                ColumnSpan = Convert.ToInt32(row["COLUMN_SPAN"].ToString() != string.Empty ? row["COLUMN_SPAN"] : 0),
                                RowPosition = Convert.ToInt32(row["ROW_POSITION"].ToString() != string.Empty ? row["ROW_POSITION"] : 0),
                                IsMandatory = Convert.ToBoolean(row["IS_MANDATORY"].ToString()),
                                IsEnabled = Convert.ToBoolean(row["IS_ENABLED"].ToString()),
                                Description = row["DESCRIPTION"].ToString(),
                                PropertyName = row["PROPERTY_NAME"].ToString(),
                                PropertyLength = row["PROPERTY_LENGTH"].ToString(),
                                FieldType = (FieldType)Convert.ToInt32(row["FIELD_TYPE_ID"].ToString() != string.Empty ? row["FIELD_TYPE_ID"] : 0)
                            };

                            file.Templates.Where(x => x.Id == (int)row["FILE_TEMPLATE_ID"]).FirstOrDefault().Rows.FirstOrDefault().Fields.Add(field);
                        }
                    }

                };

            }

            return file;

        }
        public void SendMailNotification(bool ews, string smtpServer, int smtpPort, string filePath, string email, string strUserName, string strSenderName, string strPassword,
            string strEmailFrom, string messageBody)
        {
            MailService mailService = new MailService();
            if (ews)
            {
                mailService.SendMailEws(smtpServer, smtpPort, email, strUserName, strPassword, strSenderName, messageBody, filePath, string.Empty);
            }
            else
            {
                mailService.SendMail(filePath, email, smtpServer, smtpPort, strUserName, strSenderName, strPassword, strEmailFrom, messageBody);
            }

        }
        public List<int> GetProcessRequest()
        {
            List<int> processRequest = new List<int>();

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.GET_LIST_RISK_PENDING_PROCESS");
            }
            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow objectResult in result.Rows)
                {
                    if ((bool)objectResult.ItemArray[6] && !(bool)objectResult.ItemArray[4])
                    {
                        processRequest.Add((int)objectResult.ItemArray[0]);
                    }
                }
            }

            return processRequest;
        }
        public int GenerateListRiskProcessRequest(bool matchProcess, bool onuProcess, bool ofacProcess, string searchValue, bool isMasive)
        {
            int id = 0;
            NameValue[] parameters = new NameValue[9];
            parameters[0] = new NameValue("@PROCESS_ID", 0);
            parameters[1] = new NameValue("@MOVEMENT_DATE", DateTime.Now);
            parameters[2] = new NameValue("@DESCRIPTION", "");
            parameters[3] = new NameValue("@RISK_LIST_MATCH", matchProcess);
            parameters[4] = new NameValue("@OFAC_LIST", ofacProcess);
            parameters[5] = new NameValue("@ONU_LIST", onuProcess);
            parameters[6] = new NameValue("@SEARCH_VALUE", searchValue);
            parameters[7] = new NameValue("@IS_MASIVE", isMasive);
            parameters[8] = new NameValue("@STATUS", 1);

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.SAVE_LIST_RISK_PROCESS", parameters);
            }

            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow objectResult in result.Rows)
                {
                    id = (int)objectResult.ItemArray[0];
                }
            }

            return id;
        }
        public OnuPeopleProcessModel GetSystemPeople()
        {
            DataTable result;
            OnuPeopleProcessModel onuMatchingProcessModel = new OnuPeopleProcessModel();
            onuMatchingProcessModel.Company = new List<CompanyModel>();
            onuMatchingProcessModel.People = new List<PersonModel>();

            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.GET_COMPANY_PERSON");
            }

            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow item in result.Rows)
                {
                    if (Convert.ToInt16(item.ItemArray[1]) == (int)IndividualType.Company)
                    {
                        CompanyModel companyCompany = new CompanyModel
                        {
                            IndividualId = item.ItemArray[0] != DBNull.Value ? (int)item.ItemArray[0] : 0,
                            DocumentTypeId = item.ItemArray[2] != DBNull.Value ? Convert.ToInt16(item.ItemArray[2]) : 0,
                            DocumentType = item.ItemArray[3] != DBNull.Value && (string)item.ItemArray[3] != "" ? (string)item.ItemArray[3] : "N/A",
                            DocumentNumber = item.ItemArray[4] != DBNull.Value && (string)item.ItemArray[4] != "" ? (string)item.ItemArray[4] : "N/A",
                            TradeName = item.ItemArray[5] != DBNull.Value && (string)item.ItemArray[5] != "" ? (string)item.ItemArray[5] : "N/A"
                        };

                        onuMatchingProcessModel.Company.Add(companyCompany);
                    }
                    else if (Convert.ToInt16(item.ItemArray[1]) == (int)IndividualType.Person)
                    {
                        PersonModel companyPerson = new PersonModel
                        {
                            IndividualId = item.ItemArray[0] != DBNull.Value ? (int)item.ItemArray[0] : 0,
                            DocumentTypeId = item.ItemArray[6] != DBNull.Value ? Convert.ToInt16(item.ItemArray[6]) : 0,
                            DocumentType = item.ItemArray[7] != DBNull.Value && (string)item.ItemArray[7] != "" ? (string)item.ItemArray[7] : "N/A",
                            DocumentNumber = item.ItemArray[8] != DBNull.Value && (string)item.ItemArray[8] != "" ? (string)item.ItemArray[8] : "N/A",
                            SurName = item.ItemArray[9] != DBNull.Value && (string)item.ItemArray[9] != "" ? (string)item.ItemArray[9] : "N/A",
                            SecondSurName = item.ItemArray[10] != DBNull.Value && (string)item.ItemArray[10] != "" ? (string)item.ItemArray[10] : "N/A",
                            Name = item.ItemArray[11] != DBNull.Value && (string)item.ItemArray[11] != "" ? (string)item.ItemArray[11] : "N/A"
                        };


                        onuMatchingProcessModel.People.Add(companyPerson);
                    }
                }
            }

            return onuMatchingProcessModel;
        }


    }

}
