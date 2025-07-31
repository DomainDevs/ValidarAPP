using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using Sistran.Co.Application.Data;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Application.Utilities.DataFacade;
using COMMEN = Sistran.Core.Application.Common.Entities;
using TMPEN = Sistran.Core.Application.Temporary.Entities;
using Sistran.Company.Application.MassiveServices.EEProvider.Entities.View;
using Sistran.Company.Application.MassiveVehicleParamBusinessService.Model;
using Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.Enum;
using Sistran.Company.Application.MassiveVehicleParamBusinessServiceProvider.Assembler;

namespace Sistran.Company.Application.MassiveVehicleParamBusinessServiceProvider.Dao
{
    public class FaseColdaLoadDAO
    {
        public void CreateFasecoldaPrice(CompanyFasecoldaPrice fasecoldaPrice)
        {
            TMPEN.LoadFasecoldaPrice entityFasecoldaPrice = EntityAssembler.CreateFasecoldaPrice(fasecoldaPrice);
            DataFacadeManager.Insert(entityFasecoldaPrice);
        }

        public void CreateFasecoldaCode(CompanyFasecoldaCode fasecoldaCode)
        {
            TMPEN.LoadFasecoldaCod entityFasecoldaCod = EntityAssembler.CreateFasecoldaCode(fasecoldaCode);
            DataFacadeManager.Insert(entityFasecoldaCod);
        }

        public static void DeleteFasecoldaPrice()
        {
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                dynamicDataAccess.ExecuteNonQuery("DELETE FROM TMP.LOAD_FASECOLDA_PRICE");
            }
        }

        public static void DeleteFasecoldaCode()
        {
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                dynamicDataAccess.ExecuteNonQuery("DELETE FROM TMP.LOAD_FASECOLDA_COD");
            }
        }

        public List<CompanyProcessFasecoldaMassiveLoad> GetProcessMassiveVehiclefasecolda(int loadProcessId)
        {
            List<CompanyProcessFasecoldaMassiveLoad> companyProcessFasecoldaMassiveLoads = new List<CompanyProcessFasecoldaMassiveLoad>();
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("PROCESS_ID", loadProcessId);
            DataTable resultTable = new DataTable();

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                resultTable = dynamicDataAccess.ExecuteSPDataTable("COMM.MASSIVE_LOAD_FASECOLDA", parameters);
            }

            if (resultTable != null && resultTable.Rows.Count > 0)
            {
                foreach (DataRow row in resultTable.Rows)
                {
                    companyProcessFasecoldaMassiveLoads.Add(new CompanyProcessFasecoldaMassiveLoad()
                    {
                        TotalRows = (int)row["TOTAL"],
                        TotalRowsProcesseds = (int)row["PROCESSED"],
                        Pendings = (int)row["PENDING"],
                        WithErrorsLoaded = (int)row["WITH_ERROR"],
                        TypeFile = (FileTypeFasecoldaEnum)row["TYPE_FILE"],
                        StatusId = (int)row["STATUS_ID"],
                        EnableProcessing = (int)row["ENABLE_PROCESSING"]
                    });
                }
            }

            return companyProcessFasecoldaMassiveLoads;
        }

        public CompanyProcessFasecolda GetLoadMassiveVehiclefasecolda(int loadProcessId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            MassiveFasecoldaView massiveFasecoldaView = new MassiveFasecoldaView();
            ViewBuilder builder = new ViewBuilder("MassiveFasecoldaView");

            builder.Filter = (filter.Property(COMMEN.CiaAsynchronousProcessFasecoldaMassiveLoad.Properties.ProcessId, typeof(COMMEN.CiaAsynchronousProcessFasecoldaMassiveLoad).Name).Equal().Constant(loadProcessId)).GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, massiveFasecoldaView);

            COMMEN.CiaAsynchronousProcessFasecoldaMassiveLoad massiveLoadView = massiveFasecoldaView.CiaAsynchronousProcessFasecoldaMassiveLoads.Cast<COMMEN.CiaAsynchronousProcessFasecoldaMassiveLoad>().FirstOrDefault();
            COMMEN.CiaAsynchronousProcessFasecoldaRow collectiveEmissionView = massiveFasecoldaView.CiaAsynchronousProcessFasecoldaRows.Cast<COMMEN.CiaAsynchronousProcessFasecoldaRow>().FirstOrDefault();

            return ModelAssembler.CreateProcessFasecolda(massiveLoadView);
        }

        public CompanyProcessFasecolda CreateProcessFasecolda(CompanyProcessFasecolda processFasecolda)
        {
            COMMEN.AsynchronousProcess entityAsynchronousProcess = EntityAssembler.CreateAsynchronousProcess(processFasecolda);
            if (processFasecolda.ProcessId == 0)
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityAsynchronousProcess);
            }

            if (entityAsynchronousProcess.ProcessId != 0)
            {
                processFasecolda.ProcessId = entityAsynchronousProcess.ProcessId;
                CompanyProcessFasecolda asynchronousProcess = CreateAsynchronousProcess(processFasecolda);

                if (asynchronousProcess != null)
                {
                    processFasecolda.Id = asynchronousProcess.Id;
                    return processFasecolda;
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

        public CompanyProcessFasecolda CreateAsynchronousProcess(CompanyProcessFasecolda processFasecolda)
        {
            COMMEN.CiaAsynchronousProcessFasecoldaMassiveLoad entityProcessFasecoldaMassiveLoad = EntityAssembler.CreateProcessFasecoldaMassiveLoad(processFasecolda);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(entityProcessFasecoldaMassiveLoad);

            if (entityProcessFasecoldaMassiveLoad != null)
            {
                processFasecolda.Id = entityProcessFasecoldaMassiveLoad.Id;
                //Task.Run(() => 
                CreateAsynchronousProcessStatus(processFasecolda);
                //);
                return processFasecolda;
            }
            else
            {
                return null;
            }
        }

        private void CreateAsynchronousProcessStatus(CompanyProcessFasecolda processFasecolda)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(COMMEN.CiaAsynchronousProcessFasecoldaStatus.Properties.ProcessId, typeof(COMMEN.CiaAsynchronousProcessFasecoldaStatus).Name).Equal().Constant(processFasecolda.ProcessId);
            filter.And();
            filter.Property(COMMEN.CiaAsynchronousProcessFasecoldaStatus.Properties.ProcessFasecoldaStatusId, typeof(COMMEN.CiaAsynchronousProcessFasecoldaStatus).Name).Equal().Constant((int)processFasecolda.ProcessStatusType.StatusType);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.CiaAsynchronousProcessFasecoldaStatus), filter.GetPredicate()));
            COMMEN.CiaAsynchronousProcessFasecoldaStatus entityProcessFasecoldaStatus = businessCollection.Cast<COMMEN.CiaAsynchronousProcessFasecoldaStatus>().FirstOrDefault();

            COMMEN.CiaAsynchronousProcessFasecoldaStatus entityCiaAsynchronousProcessFasecoldaStatus = EntityAssembler.CreateCiaAsynchronousProcessFasecoldaStatus(processFasecolda);

            if (businessCollection.Count > 0 && (int)processFasecolda.ProcessStatusType.StatusType == (int)entityProcessFasecoldaStatus.ProcessFasecoldaStatusId)
            {
                entityProcessFasecoldaStatus.EndDate = DateTime.Now;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityProcessFasecoldaStatus);

                //entityCiaAsynchronousProcessFasecoldaStatus.EndDate = DateTime.Now;
                //DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityCiaAsynchronousProcessFasecoldaStatus);
            }
            else if(entityProcessFasecoldaStatus == null)
            {
                entityCiaAsynchronousProcessFasecoldaStatus.BeginDate = DateTime.Now;
                entityCiaAsynchronousProcessFasecoldaStatus.EndDate = null;
                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityCiaAsynchronousProcessFasecoldaStatus);
            }
        }

        public CompanyProcessFasecolda UpdateProcessFasecolda(CompanyProcessFasecolda processFasecolda)
        {
            UpdateAsynchronousProcess(processFasecolda);

            PrimaryKey primaryKey = COMMEN.AsynchronousProcess.CreatePrimaryKey(processFasecolda.ProcessId);
            COMMEN.AsynchronousProcess entityAsynchronousProcess = (COMMEN.AsynchronousProcess)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

            if (entityAsynchronousProcess != null)
            {
                entityAsynchronousProcess.HasError = processFasecolda.HasError;
                entityAsynchronousProcess.ErrorDescription = processFasecolda.Error_Description;
                //EN COMENTARTIO CON PROPOSITOS DELA INTEGRACIÓN
                entityAsynchronousProcess.Status = processFasecolda.Status;
                //
                entityAsynchronousProcess.EndDate = processFasecolda.EndDate;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityAsynchronousProcess);

                if (entityAsynchronousProcess != null)
                {
                    return processFasecolda;
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

        public CompanyProcessFasecolda UpdateAsynchronousProcess(CompanyProcessFasecolda processFasecolda)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(COMMEN.CiaAsynchronousProcessFasecoldaMassiveLoad.Properties.ProcessId, typeof(COMMEN.CiaAsynchronousProcessFasecoldaMassiveLoad).Name).Equal().Constant(processFasecolda.ProcessId);
            filter.And();
            filter.Property(COMMEN.CiaAsynchronousProcessFasecoldaMassiveLoad.Properties.TypeFile, typeof(COMMEN.CiaAsynchronousProcessFasecoldaMassiveLoad).Name).Equal().Constant(processFasecolda.TypeFile);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.CiaAsynchronousProcessFasecoldaMassiveLoad), filter.GetPredicate()));

            COMMEN.CiaAsynchronousProcessFasecoldaMassiveLoad entityProcessFasecoldaMassiveLoad = businessCollection.Cast<COMMEN.CiaAsynchronousProcessFasecoldaMassiveLoad>().FirstOrDefault();

            if (entityProcessFasecoldaMassiveLoad != null)
            {
                entityProcessFasecoldaMassiveLoad.ProcessId = processFasecolda.ProcessId;
                entityProcessFasecoldaMassiveLoad.Pendings = processFasecolda.Pendings;
                entityProcessFasecoldaMassiveLoad.WithErrorsProcesseds = processFasecolda.WithErrorsProcesseds;
                entityProcessFasecoldaMassiveLoad.Loaded = processFasecolda.Loaded;
                entityProcessFasecoldaMassiveLoad.WithErrorsLoaded = processFasecolda.WithErrorsLoaded;
                entityProcessFasecoldaMassiveLoad.TotalRowsProcesseds = processFasecolda.TotalRowsProcesseds;
                entityProcessFasecoldaMassiveLoad.TotalRowsLoading = processFasecolda.TotalRowsLoaded;
                entityProcessFasecoldaMassiveLoad.TotalRows = processFasecolda.TotalRows;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityProcessFasecoldaMassiveLoad);

                //if (processFasecolda.HasError)
                //{
                //Task.Run(() => 
                CreateAsynchronousProcessStatus(processFasecolda);
                //}

                return processFasecolda;
            }
            else
            {
                return null;
            }
        }
    }
}
