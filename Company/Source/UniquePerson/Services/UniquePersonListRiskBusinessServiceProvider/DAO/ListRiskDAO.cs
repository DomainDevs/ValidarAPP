using COMMEN = Sistran.Company.Application.Common.Entities;
using Sistran.Company.Application.UniquePersonListRiskBusinessService.Model;
using Sistran.Company.Application.UniquePersonListRiskBusinessServiceProvider.Assembler;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Linq;
using System;
using Sistran.Company.Application.UniquePersonListRiskBusinessServiceProvider.Business;
using Sistran.Company.Application.UniquePersonListRiskApplicationServices.Enum;
using Sistran.Company.Application.UniquePersonListRiskBusinessServiceProvider.Entities.Views;
using DAFENG = Sistran.Core.Framework.DAF.Engine;
using Newtonsoft.Json;
using Sistran.Co.Application.Data;
using System.Data;
using System.Collections.Generic;
using System.Configuration;

namespace Sistran.Company.Application.UniquePersonListRiskBusinessServiceProvider.DAO
{
    public class ListRiskLoadDAO
    {
        public CompanyListRiskLoad CreateAsyncronousProcess(CompanyListRiskLoad listRiskLoad)
        {
            COMMEN.AsynchronousProcess entityAsynchronousProcess = EntityAssembler.CreateAsynchronousProcess(listRiskLoad);
            if (listRiskLoad.ProcessId == 0)
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityAsynchronousProcess);
            }

            if (entityAsynchronousProcess.ProcessId != 0)
            {
                listRiskLoad.ProcessId = entityAsynchronousProcess.ProcessId;
                CompanyListRiskLoad asynchronousProcess = CreateAsynchronousProcess(listRiskLoad);

                if (asynchronousProcess != null)
                {
                    listRiskLoad.Id = asynchronousProcess.Id;
                    return listRiskLoad;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public CompanyListRiskLoad CreateAsynchronousProcess(CompanyListRiskLoad listRiskLoad)
        {
            COMMEN.CiaAsynchronousProcessListRiskMassiveLoad entityProcessListRiskaMassiveLoad = EntityAssembler.CreateProcessListRiskMassiveLoad(listRiskLoad);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(entityProcessListRiskaMassiveLoad);

            if (entityProcessListRiskaMassiveLoad != null)
            {
                listRiskLoad.Id = entityProcessListRiskaMassiveLoad.Id;
                CreateAsynchronousProcessStatus(listRiskLoad);
                return listRiskLoad;
            }
            else
            {
                return null;
            }
        }

        private void CreateAsynchronousProcessStatus(CompanyListRiskLoad listRiskLoad)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(COMMEN.CiaAsynchronousProcessListRiskStatus.Properties.ProcessId, typeof(COMMEN.CiaAsynchronousProcessListRiskStatus).Name).Equal().Constant(listRiskLoad.ProcessId);
            filter.And();
            filter.Property(COMMEN.CiaAsynchronousProcessListRiskStatus.Properties.ProcessStatusId, typeof(COMMEN.CiaAsynchronousProcessListRiskStatus).Name).Equal().Constant((int)listRiskLoad.ProcessStatus);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.CiaAsynchronousProcessListRiskStatus), filter.GetPredicate()));
            COMMEN.CiaAsynchronousProcessListRiskStatus entityProcessProcessListRiskStatus = businessCollection.Cast<COMMEN.CiaAsynchronousProcessListRiskStatus>().FirstOrDefault();

            COMMEN.CiaAsynchronousProcessListRiskStatus entityCiaAsynchronousProcessStatus = businessCollection.Cast<COMMEN.CiaAsynchronousProcessListRiskStatus>().FirstOrDefault();

            COMMEN.CiaAsynchronousProcessListRiskStatus entityCiaAsynchronousProcessListRiskStatus = EntityAssembler.CreateCiaAsynchronousProcessStatus(listRiskLoad);

            if (businessCollection.Count > 0 && (int)listRiskLoad.ProcessStatus == entityProcessProcessListRiskStatus.ProcessStatusId)
            {
                entityCiaAsynchronousProcessStatus.EndDate = DateTime.Now;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityCiaAsynchronousProcessStatus);
            }
            else if (entityProcessProcessListRiskStatus == null)
            {
                entityCiaAsynchronousProcessListRiskStatus.BeginDate = DateTime.Now;
                entityCiaAsynchronousProcessListRiskStatus.EndDate = null;
                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityCiaAsynchronousProcessListRiskStatus);
            }
        }

        public CompanyListRiskLoad UpdateProcessListRisk(CompanyListRiskLoad listRiskLoad)
        {
            UpdateAsynchronousProcess(listRiskLoad);

            PrimaryKey primaryKey = COMMEN.AsynchronousProcess.CreatePrimaryKey(listRiskLoad.ProcessId);
            COMMEN.AsynchronousProcess entityAsynchronousProcess = (COMMEN.AsynchronousProcess)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

            if (entityAsynchronousProcess != null)
            {
                entityAsynchronousProcess.HasError = listRiskLoad.HasError;
                entityAsynchronousProcess.ErrorDescription = listRiskLoad.Error_Description;
                entityAsynchronousProcess.Status = listRiskLoad.Status;
                entityAsynchronousProcess.EndDate = listRiskLoad.EndDate;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityAsynchronousProcess);

                if (entityAsynchronousProcess != null)
                {
                    return listRiskLoad;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public CompanyListRiskLoad UpdateAsynchronousProcess(CompanyListRiskLoad listRiskLoad)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(COMMEN.CiaAsynchronousProcessListRiskMassiveLoad.Properties.ProcessId, typeof(COMMEN.CiaAsynchronousProcessListRiskMassiveLoad).Name).Equal().Constant(listRiskLoad.ProcessId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.CiaAsynchronousProcessListRiskMassiveLoad), filter.GetPredicate()));

            COMMEN.CiaAsynchronousProcessListRiskMassiveLoad entityProcessListRiskMassiveLoad = businessCollection.Cast<COMMEN.CiaAsynchronousProcessListRiskMassiveLoad>().FirstOrDefault();

            if (entityProcessListRiskMassiveLoad != null)
            {
                entityProcessListRiskMassiveLoad.ProcessId = listRiskLoad.ProcessId;
                entityProcessListRiskMassiveLoad.TotalRows = listRiskLoad.TotalRows;
                entityProcessListRiskMassiveLoad.RiskListTypeCd = listRiskLoad.RiskListType;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityProcessListRiskMassiveLoad);

                //if (listRiskLoad.HasError)
                //{
                //Task.Run(() => CreateAsynchronousProcessStatus(listRiskLoad));
                CreateAsynchronousProcessStatus(listRiskLoad);
                //}

                return listRiskLoad;
            }
            else
            {
                return null;
            }
        }

        public void CreateProcessListRiskLoadRow(CompanyListRiskRow listRiskLoad)
        {
            COMMEN.CiaAsynchronousProcessListriskRow entityCiaAsynchronousProcessListRiskRow = EntityAssembler.CreateProcessListRiskRow(listRiskLoad);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(entityCiaAsynchronousProcessListRiskRow);
        }

        public void CreateListRiskOfacTemporal(string listRiskPerson)
        {
            CompanyListRiskOfac listRisk = JsonConvert.DeserializeObject<CompanyListRiskOfac>(listRiskPerson);
            ListRiskLoadBusiness listRiskLoadBusiness = new ListRiskLoadBusiness();
            listRiskLoadBusiness.CreateListRiskPersonTemporal(listRiskPerson, listRisk.EntNum.ToString(), listRisk.CreatedUser, DateTime.Now, listRisk.ProcessId, listRisk.ListRiskType, listRisk.Event, listRisk.ListRiskTypeDescription, listRisk.ListRiskId);
        }

        public void CreateListRiskTemporal(string listRiskPerson)
        {
            CompanyListRisk listRisk = JsonConvert.DeserializeObject<CompanyListRisk>(listRiskPerson);
            ListRiskLoadBusiness listRiskLoadBusiness = new ListRiskLoadBusiness();
            listRiskLoadBusiness.CreateListRiskPersonTemporal(listRiskPerson, listRisk.DocumentNumber, listRisk.CreatedUser, DateTime.Now, listRisk.ProcessId, listRisk.ListRiskType, listRisk.Event, listRisk.ListRiskTypeDescription, listRisk.ListRiskId);
        }

        public CompanyListRiskLoad GetLoadMassiveListRiskLoad(int loadProcessId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            MassiveListRiskView massiveListRiskView = new MassiveListRiskView();
            DAFENG.ViewBuilder builder = new DAFENG.ViewBuilder("MassiveListRiskView");

            builder.Filter = (filter.Property(COMMEN.CiaAsynchronousProcessListRiskMassiveLoad.Properties.ProcessId, typeof(COMMEN.CiaAsynchronousProcessListRiskMassiveLoad).Name).Equal().Constant(loadProcessId)).GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, massiveListRiskView);

            COMMEN.CiaAsynchronousProcessListRiskMassiveLoad massiveLoadView = massiveListRiskView.CiaAsynchronousProcessListRiskMassiveLoad.Cast<COMMEN.CiaAsynchronousProcessListRiskMassiveLoad>().FirstOrDefault();
            COMMEN.CiaAsynchronousProcessListriskRow collectiveEmissionView = massiveListRiskView.CiaAsynchronousProcessListriskRow.Cast<COMMEN.CiaAsynchronousProcessListriskRow>().FirstOrDefault();

            return ModelAssembler.CreateProcessListRisk(massiveLoadView);
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
            parameters[8] = new NameValue("@STATUS", ProcessStatusEnum.Validando);

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

        public RiskListMatch SearchProcess(string searchValue)
        {
            RiskListMatch riskListMatch = new RiskListMatch();

            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("@ID", searchValue);
            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess("SistranEE"))
            {
                result = pdb.ExecuteSPDataTable("UP.GET_LIST_RISK_PROCESS", parameters);
            }
            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow objectResult in result.Rows)
                {
                    riskListMatch.fileName = objectResult.ItemArray[2] != DBNull.Value ? ConfigurationManager.AppSettings["ExternalFolderFiles"] + "\\" + (string)objectResult.ItemArray[2] : "";
                    riskListMatch.Status = objectResult.ItemArray[9] != DBNull.Value ? (ProcessStatusEnum)objectResult.ItemArray[9] : 0;
                }
            }
            return riskListMatch;
        }
    }
}
