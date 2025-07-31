using System.Collections.Generic;
using Sistran.Company.Application.OperationQuotaCompanyServices.EEProvider.Assemblers;
using Sistran.Company.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using UPENT = Sistran.Company.Application.UniquePerson.Entities;
using PAENT = Sistran.Company.Application.Parameters.Entities;
using System.Diagnostics;
using Sistran.Core.Framework.Queries;
using Sistran.Company.Application.OperationQuotaServices.EEProvider.View;
using Sistran.Core.Framework.DAF.Engine;
using AQMOD = Sistran.Company.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;
using AQENT = Sistran.Company.Application.AutomaticQuota.Entities;
using System;
using Sistran.Core.Framework.BAF;
using System.Data;
using Sistran.Co.Application.Data;
using Sistran.Core.Framework;
using Sistran.Company.Application.OperationQuotaServices.EEProvider.Utilities;
using Sistran.Core.Application.Utilities.Helper;

namespace Sistran.Company.Application.OperationQuotaCompanyServices.EEProvider.DAOs
{
    public class OperationQuotaDAO
    {
        public List<AQMOD.AutomaticQuotaOperation> GetAutomaticQuotaOperation(int Id)
        {
            List<AQMOD.AutomaticQuotaOperation> automaticQuotaOperations = new List<AQMOD.AutomaticQuotaOperation>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(AQENT.AutomaticQuotaOperation.Properties.Id, typeof(AQENT.AutomaticQuotaOperation).Name, Id);
            BusinessCollection businessObjects = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(AQENT.AutomaticQuotaOperation), filter.GetPredicate()));
            automaticQuotaOperations = ModelAssembler.CreateAutomaticQuotaOperations(businessObjects);
            return automaticQuotaOperations;
        }

        public List<AQMOD.AutomaticQuotaOperation> GetAutomaticQuotaOperationByParentId(int ParentId)
        {
            List<AQMOD.AutomaticQuotaOperation> automaticQuotaOperations = new List<AQMOD.AutomaticQuotaOperation>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(AQENT.AutomaticQuotaOperation.Properties.ParentId, typeof(AQENT.AutomaticQuotaOperation).Name, ParentId);
            BusinessCollection businessObjects = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(AQENT.AutomaticQuotaOperation), filter.GetPredicate()));
            automaticQuotaOperations = ModelAssembler.CreateAutomaticQuotaOperations(businessObjects);
            return automaticQuotaOperations;
        }

        public List<AgentProgram> GetAgentProgram()
        {
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(UPENT.AgentProgram));
            List<AQMOD.AgentProgram> agentPrograms = ModelAssembler.CreateAgentPrograms(businessObjects);
            return agentPrograms;
        }

        public List<AQMOD.UtilityDetails> GetUtility()
        {
            List<UtilityDetails> utilityDetails = new List<UtilityDetails>();
            List<UtilityDetails> utilities = new List<UtilityDetails>();
            List<UtilityDetails> details = new List<UtilityDetails>();
            List<UtilityType> utilityTypes = new List<UtilityType>();
            List<UtilitySummary> utilitySummaries = new List<UtilitySummary>();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            UtilitysDetailsView view = new UtilitysDetailsView();
            ViewBuilder builder = new ViewBuilder("UtilitysDetailsView");
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            if (view.UtilityDetails.Count > 0)
            {
                utilities = ModelAssembler.CreateUtilitiesDetails(view.UtilityDetails);
            }
            if (view.UtilitySummary.Count > 0)
            {
                utilitySummaries = ModelAssembler.CreateUtilitiesDetailSummaries(view.UtilitySummary);
            }
            if (view.UtilityType.Count > 0)
            {
                utilityTypes = ModelAssembler.CreateUtilitiesDetailTypes(view.UtilityType);

            }

            //tabla1
            utilityDetails.Add(utilities[0]);
            utilityDetails.Add(utilities[1]);
            utilityDetails.Add(utilities[2]);
            utilityDetails.Add(new UtilityDetails { Id = 1, Description = "Activo Corriente", Enabled = true, FormUtilitys = 1, UtilitysTypeCd = 0, UtilitysSummaryCd = 1 });
            utilityDetails.Add(utilities[3]);
            utilityDetails.Add(new UtilityDetails { Id = 2, Description = "Activo no corriente", Enabled = true, FormUtilitys = 1, UtilitysTypeCd = 0, UtilitysSummaryCd = 1 });
            utilityDetails.Add(new UtilityDetails { Id = 3, Description = "Total Activo", Enabled = true, FormUtilitys = 1, UtilitysTypeCd = 0, UtilitysSummaryCd = 1 });
            utilityDetails.Add(utilities[4]);
            utilityDetails.Add(utilities[5]);
            utilityDetails.Add(new UtilityDetails { Id = 4, Description = "Pasivo Corriente", Enabled = true, FormUtilitys = 1, UtilitysTypeCd = 0, UtilitysSummaryCd = 2 });
            utilityDetails.Add(utilities[6]);
            utilityDetails.Add(new UtilityDetails { Id = 5, Description = "Pasivo No Corriente", Enabled = true, FormUtilitys = 1, UtilitysTypeCd = 0, UtilitysSummaryCd = 2 });
            utilityDetails.Add(new UtilityDetails { Id = 6, Description = "Total Pasivo", Enabled = true, FormUtilitys = 1, UtilitysTypeCd = 0, UtilitysSummaryCd = 2 });
            utilityDetails.Add(utilities[7]);
            utilityDetails.Add(utilities[8]);
            utilityDetails.Add(utilities[9]);
            utilityDetails.Add(new UtilityDetails { Id = 7, Description = "Total Patrimonio", Enabled = true, FormUtilitys = 1, UtilitysTypeCd = 0, UtilitysSummaryCd = 3 });
            //tabla2
            utilityDetails.Add(utilities[10]);
            utilityDetails.Add(utilities[11]);
            utilityDetails.Add(new UtilityDetails { Id = 8, Description = "Utilidad Bruta", Enabled = true, FormUtilitys = 2, UtilitysTypeCd = 0, UtilitysSummaryCd = 4 });
            utilityDetails.Add(utilities[12]);
            utilityDetails.Add(new UtilityDetails { Id = 9, Description = "Utilidades Operacional", Enabled = true, FormUtilitys = 2, UtilitysTypeCd = 0, UtilitysSummaryCd = 4 });
            utilityDetails.Add(utilities[13]);
            utilityDetails.Add(new UtilityDetails { Id = 10, Description = "Utilidad Antes De Impuestos", Enabled = true, FormUtilitys = 2, UtilitysTypeCd = 0, UtilitysSummaryCd = 4 });
            utilityDetails.Add(utilities[14]);
            utilityDetails.Add(new UtilityDetails { Id = 11, Description = "Utilidad Neta", Enabled = true, FormUtilitys = 2, UtilitysTypeCd = 0, UtilitysSummaryCd = 4 });
            int count = 1;
            foreach (var item in utilityDetails)
            {
                item.UtilityId = item.Id;
                item.Id = count;
                count++;
            }
            return utilityDetails;
        }

        public List<AQMOD.IndicatorConcept> GetIndicatorConcepts()
        {
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(PAENT.IndicatorConcept));
            List<AQMOD.IndicatorConcept> indicatorConcepts = ModelAssembler.CreateindicatorConcepts(businessObjects);
            return indicatorConcepts;
        }
        public List<AQMOD.RiskCenter> GetRiskCenterList()
        {
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(PAENT.RiskCenterList));
            List<AQMOD.RiskCenter> riskCenters = ModelAssembler.CreateRiskCenterList(businessObjects);
            return riskCenters;
        }

        public List<AQMOD.Restrictive> GetRestrictiveList()
        {
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(PAENT.RestrictiveList));
            List<AQMOD.Restrictive> restrictives = ModelAssembler.CreateRestrictiveList(businessObjects);
            return restrictives;
        }

        public List<AQMOD.ReportListSisconc> GetReportListSisconc()
        {
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(PAENT.ReportListSisconc));
            List<AQMOD.ReportListSisconc> reportListSisconcs = ModelAssembler.CreateReportListSisconc(businessObjects);
            return reportListSisconcs;
        }

        public List<AQMOD.PromissoryNoteSignature> GetPromissoryNoteSignature()
        {
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(PAENT.PromissoryNoteSignature));
            List<AQMOD.PromissoryNoteSignature> promissoryNoteSignatures = ModelAssembler.CreatePromissoryNoteSignatures(businessObjects);
            return promissoryNoteSignatures;
        }

        public List<AQMOD.AutomaticQuota> GetAutomaticQuota(int Id)
        {
            List<AQMOD.AutomaticQuota> automaticQuota = new List<AQMOD.AutomaticQuota>();
            List<AQMOD.Prospect> prospects = new List<AQMOD.Prospect>(); ;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            if (Id > 0)
            {
                filter.Property(AQENT.AutomaticQuota.Properties.AutomaticQuotaCode, typeof(AQENT.AutomaticQuota).Name);
                filter.Equal();
                filter.Constant(Id);
            }

            AutomaticQuotaView view = new AutomaticQuotaView();
            ViewBuilder builder = new ViewBuilder("AutomaticQuotaView");

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            if (view.AutomaticQuota.Count > 0)
            {
                automaticQuota.AddRange(ModelAssembler.CreateCompanyAutomaticQuotas(view.AutomaticQuota));
                if (view.Third.Count > 0)
                {
                    List<AQMOD.Third> thirds = new List<AQMOD.Third>(); ;
                    thirds.AddRange(ModelAssembler.CreateThirds(view.Third));
                    automaticQuota[0].Third = thirds[0];
                }
                if (view.Indicator.Count > 0)
                {
                    automaticQuota[0].Indicator.AddRange(ModelAssembler.CreateIndicators(view.Indicator));
                }
                if (view.Utility.Count > 0)
                {
                    automaticQuota[0].Utility.AddRange(ModelAssembler.CreateUtilities(view.Utility));
                }
                if (view.SummaryUtility.Count > 0)
                {
                    automaticQuota[0].Utility.AddRange(ModelAssembler.CreateUtilities(view.SummaryUtility));
                }
                if (view.Company.Count > 0)
                {
                    prospects.AddRange(ModelAssembler.CreateCompanies(view.Company));
                    automaticQuota[0].Prospect = prospects[0];
                }
                else
                {
                    if (view.Person.Count > 0)
                    {
                        prospects.AddRange(ModelAssembler.CreatePersons(view.Person));
                        automaticQuota[0].Prospect = prospects[0];

                    }
                    else
                    {
                        if (view.Prospect.Count > 0)
                        {
                            prospects.AddRange(ModelAssembler.CreateProspects(view.Prospect));
                            automaticQuota[0].Prospect = prospects[0];
                        }
                    }
                }
            }

            return automaticQuota;
        }

        public AQMOD.AutomaticQuotaOperation CreateCompanyAutomaticOperation(AQMOD.AutomaticQuotaOperation companyOperation)
        {
            try
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                var result = ModelAssembler.CreateAutomaticOperation((AQENT.AutomaticQuotaOperation)DataFacadeManager.Insert(EntityAssembler.CreateEntityAutomaticQuotaOperation(companyOperation)));
                stopWatch.Stop();
                return result;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public AQMOD.AutomaticQuotaOperation UpdateCompanyAutomaticOperation(AQMOD.AutomaticQuotaOperation companyOperation)
        {
            try
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                PrimaryKey key = AQENT.AutomaticQuotaOperation.CreatePrimaryKey(companyOperation.Id);
                AQENT.AutomaticQuotaOperation entityOperation = (AQENT.AutomaticQuotaOperation)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                if (entityOperation != null)
                {
                    entityOperation.User = companyOperation.User;
                    entityOperation.ModificationDate = companyOperation.ModificationDate;
                    entityOperation.Operation = companyOperation.Operation;

                }
                stopWatch.Stop();
                DataFacadeManager.Update(entityOperation);
                return companyOperation;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public AQMOD.AutomaticQuota CreateAutomaticQuotaGeneral(AQMOD.AutomaticQuota automatic)
        {
            try
            {
                //return ModelAssembler.CreateModelAutomaticQuota((AQENT.AutomaticQuota)DataFacadeManager.Insert(EntityAssembler.CreateEntityAutomaticQuota(automatic)));
                return null;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Insertar en tablas desde el JSON
        /// </summary>
        public AQMOD.AutomaticQuota CreateAutomaticTemporal(AQMOD.AutomaticQuota automaticQuota)
        {
            try
            {
                OperationQuotaDAO automaticQuotaDAO = new OperationQuotaDAO();
                automaticQuotaDAO.SaveQuotaTables(automaticQuota);
                if (automaticQuota.IndividualId != 0)
                {
                    automaticQuota = automaticQuotaDAO.SaveQuotaTables(automaticQuota);
                }
                return automaticQuota;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Insertar en tablas
        /// </summary>
        public AQMOD.AutomaticQuota SaveQuotaTables(AQMOD.AutomaticQuota automaticQuota)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer = new DynamicPropertiesSerializer();

            GetDatatables dts = new GetDatatables();
            CommonDataTables datatables = dts.GetcommonDataTables(automaticQuota);
            DataTable dataTable;

            NameValue[] parameters = new NameValue[5];

            DataTable dtAutomaticQuo = datatables.dtAutomaticQuota;
            parameters[0] = new NameValue(dtAutomaticQuo.TableName, dtAutomaticQuo);

            DataTable dtAutomaticUtility = datatables.dtUtility;
            parameters[1] = new NameValue(dtAutomaticUtility.TableName, dtAutomaticUtility);

            DataTable dtAutomaticIndicator = datatables.dtIndicator;
            parameters[2] = new NameValue(dtAutomaticIndicator.TableName, dtAutomaticIndicator);

            DataTable dtAutomaticThird = datatables.dtThird;
            parameters[3] = new NameValue(dtAutomaticThird.TableName, dtAutomaticThird);

            DataTable dtSummaryUtility = datatables.dtSummaryUtility;
            parameters[4] = new NameValue(dtSummaryUtility.TableName, dtSummaryUtility);

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("AQ.SAVE_AUTOMATIC_QUOTA_GENERAL", parameters);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                automaticQuota.utilityId = (int)dataTable.Rows[0][1];
                return automaticQuota;
            }
            else
            {
                throw new ValidationException("Error guardado");
            }



        }

        public AQMOD.AutomaticQuota GetAutomaticQuotaDeserealizado(int Id)
        {
            try
            {
                AQMOD.AutomaticQuota automaticQuota = new AQMOD.AutomaticQuota();
                List<AQMOD.AutomaticQuotaOperation> automaticQuotaOperations = new List<AQMOD.AutomaticQuotaOperation>();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(AQENT.AutomaticQuotaOperation.Properties.Id, typeof(AQENT.AutomaticQuotaOperation).Name, Id);
                filter.Or();
                filter.PropertyEquals(AQENT.AutomaticQuotaOperation.Properties.ParentId, typeof(AQENT.AutomaticQuotaOperation).Name, Id);
                BusinessCollection businessObjects = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(AQENT.AutomaticQuotaOperation), filter.GetPredicate()));
                automaticQuotaOperations = ModelAssembler.CreateAutomaticQuotaOperations(businessObjects);
                if (automaticQuotaOperations.Count > 0)
                {
                    foreach (AutomaticQuotaOperation item in automaticQuotaOperations)
                    {
                        if (item.AutomaticOperationType == 1)
                        {
                            automaticQuota = JsonHelper.DeserializeJson<AQMOD.AutomaticQuota>(item.Operation);
                        }
                        else if (item.AutomaticOperationType == 2)
                        {
                            automaticQuota.Third = JsonHelper.DeserializeJson<Third>(item.Operation);

                        }
                        else if (item.AutomaticOperationType == 3)
                        {
                            automaticQuota.Utility = JsonHelper.DeserializeJson<List<Utility>>(item.Operation);
                        }
                    }
                    return automaticQuota;
                }
                else
                {
                    throw new Exception("Temporal no encontrado " + Id);
                }


            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Eliminar JSON
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Eliminado Si/No</returns>
        public bool DeleteAutomaticQuotaOperation(int id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = AQENT.AutomaticQuotaOperation.CreatePrimaryKey(id);
            AQENT.AutomaticQuotaOperation automaticOperationsEntity = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                automaticOperationsEntity = (AQENT.AutomaticQuotaOperation)daf.GetObjectByPrimaryKey(key);
            }


            if (automaticOperationsEntity != null)
            {

                DataFacadeManager.Instance.GetDataFacade().DeleteObject(automaticOperationsEntity);
                DataFacadeManager.Dispose();

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.OperationQuotaServices.DAOs.DeleteAutomaticQuotaOperation");
                return true;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.OperationQuotaServices.DAOs.DeleteAutomaticQuotaOperation");
                return false;
            }
        }

        /// <summary>
        /// Eliminar Hijos de un JSON
        /// </summary>
        /// <param name="parentId">Id Padre</param>
        /// <returns>Eliminados Si/No</returns>
        public bool DeleteAutomaticOperationsByParentId(int parentId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(AQENT.AutomaticQuotaOperation.Properties.ParentId, typeof(AQENT.AutomaticQuotaOperation).Name);
            filter.Equal();
            filter.Constant(parentId);
            BusinessCollection businessCollection = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(AQENT.AutomaticQuotaOperation), filter.GetPredicate()));
            }

            foreach (AQENT.AutomaticQuotaOperation item in businessCollection)
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(item);
            }
            DataFacadeManager.Dispose();
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.OperationQuotaServices.DAOs.DeleteAutomaticOperationsByParentId");
            return true;
        }
    }
}