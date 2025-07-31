using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using REINSEN = Sistran.Core.Application.Reinsurance.Entities;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Data;
using System;
using Sistran.Core.Framework.Views;
using System.Linq;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums;
using Sistran.Co.Application.Data;
using System.Xml.Linq;
using System.Xml;
using System.Globalization;
using Sistran.Core.Application.CommonService.Models;
using sp = Sistran.Core.Framework.DAF.Engine.StoredProcedure;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.ReinsuranceServices.DTOs;
using Sistran.Core.Application.ReinsuranceServices.Assemblers;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using Sistran.Core.Application.TempCommonServices.Models;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{
    internal class ReinsuranceDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        private int RowsGrid = 200;
        private IDbConnection connection = null; //Defino la Conexión
        private IDbTransaction transaction = null; //Defino la Transacción
        private IDataFacade dataFacade;            //Defino el DataFacade   

        #endregion

        #region Get

        /// <summary>
        /// GetReinsuranceByEndorsement
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns>List<EndorsementReinsurance/></returns>
        public List<EndorsementReinsurance> GetReinsuranceByEndorsement(int endorsementId)
        {
            List<EndorsementReinsurance> endorsementReinsuranceDTOs = new List<EndorsementReinsurance>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.IssueReinsurance.Properties.EndorsementId, endorsementId);

            UIView data = _dataFacadeManager.GetDataFacade().GetView("GetEndorsementReinsurance",
                            criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out RowsGrid);

            foreach (DataRow reinsurance in data)
            {
                endorsementReinsuranceDTOs.Add(new EndorsementReinsurance()
                {
                    EndorsementId = Convert.ToInt32(reinsurance["EndorsementId"]),
                    IssueLayerId = Convert.ToInt32(reinsurance["IssueLayerId"]),
                    ReinsuranceNumber = Convert.ToInt32(reinsurance["ReinsuranceNumber"]),
                    Description = reinsurance["Description"].ToString(),
                    ProcessDate = Convert.ToDateTime(reinsurance["ProcessDate"]),
                    LayerNumber = Convert.ToInt32(reinsurance["LayerNumber"]),
                    LayerPercentage = Convert.ToDecimal(reinsurance["LayerPercentage"]),
                    PremiumPercentage = Convert.ToDecimal(reinsurance["PremiumPercentage"]),
                    IssueDate = Convert.ToDateTime(reinsurance["IssueDate"]),
                    ValidityFrom = Convert.ToDateTime(reinsurance["ValidityFrom"]),
                    ValidityTo = Convert.ToDateTime(reinsurance["ValidityTo"]),
                    IsAutomatic = Convert.ToString(reinsurance["IsAutomatic"]).ToUpper() == "TRUE" ? "SI" : "NO",
                    UserName = reinsurance["AccountName"].ToString(),
                    Amount = Convert.ToDecimal(reinsurance["Amount"]),
                    Premium = Convert.ToDecimal(reinsurance["Premium"]),
                });
            }

            List<EndorsementReinsurance> endorsementReinsuranceDTO = endorsementReinsuranceDTOs
                                    .GroupBy(g => new
                                    {
                                        g.EndorsementId,
                                        g.IssueLayerId,
                                        g.ReinsuranceNumber,
                                        g.Description,
                                        g.ProcessDate,
                                        g.IssueDate,
                                        g.ValidityFrom,
                                        g.ValidityTo,
                                        g.IsAutomatic,
                                        g.UserName,
                                        g.LayerNumber,
                                        g.LayerPercentage,
                                        g.PremiumPercentage
                                    })
                                .Select(cl => new EndorsementReinsurance
                                {
                                    EndorsementId = cl.First().EndorsementId,
                                    IssueLayerId = cl.First().IssueLayerId,
                                    ReinsuranceNumber = cl.First().ReinsuranceNumber,
                                    Description = cl.First().Description,
                                    ProcessDate = cl.First().ProcessDate,
                                    LayerNumber = cl.First().LayerNumber,
                                    LayerPercentage = cl.First().LayerPercentage,
                                    PremiumPercentage = cl.First().PremiumPercentage,
                                    IssueDate = cl.First().IssueDate,
                                    ValidityFrom = cl.First().ValidityFrom,
                                    ValidityTo = cl.First().ValidityTo,
                                    IsAutomatic = cl.First().IsAutomatic,
                                    UserName = cl.First().UserName,
                                    Amount = cl.Sum(c => c.Amount),
                                    Premium = cl.Sum(c => c.Premium)
                                }).ToList();


            return endorsementReinsuranceDTO;
        }
        #endregion

        #region Update
        public void UpdateTempReinsuranceProcess(int tempReinsuranceProcessId, int? recordsProcessed,
                                   int? recordsFailed, DateTime endDate, int status)
        {
            // Crea la Primary key con el codigo de la entidad
            PrimaryKey primaryKey = REINSEN.TempReinsuranceProcess.CreatePrimaryKey(tempReinsuranceProcessId);

            // Realizar las operaciones con los entities utilizando DAF
            REINSEN.TempReinsuranceProcess entityTempReinsuranceProcess = (REINSEN.TempReinsuranceProcess)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            entityTempReinsuranceProcess.EndingDate = endDate;
            entityTempReinsuranceProcess.Status = status;

            if (recordsProcessed != null && recordsFailed != null)
            {
                entityTempReinsuranceProcess.RecordsProcessed = recordsProcessed;
                entityTempReinsuranceProcess.RecordsFailed = recordsFailed;
            }

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().UpdateObject(entityTempReinsuranceProcess);
        }
        #endregion

        /// <summary>
        /// Proceso de reaseguro masivo de pólizas
        /// </summary>
        /// <param name="processId"></param>
        public void IssueMasiveProcess(int processId)
        {
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("PROCESS_ID", processId);

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                dynamicDataAccess.ExecuteSPDataTable("REINS.ISSUE_MASSIVE_PROCESS", parameters);
            }
        }

        /// <summary>
        /// Proceso de reaseguro masivo de siniestros
        /// </summary>
        /// <param name="processId"></param>
        public void ClaimMasiveProcess(int processId)
        {
            NameValue[] parameters = new NameValue[1];

            parameters[0] = new NameValue("PROCESS_ID", processId);

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                dynamicDataAccess.ExecuteSPDataTable("REINS.CLAIMS_MASSIVE_PROCESS", parameters);
            }
        }

        /// <summary>
        /// Proceso de reaseguro masivo de solicitudes de pagos
        /// </summary>
        /// <param name="processId"></param>
        public void PaymentMasiveProcess(int processId)
        {
            NameValue[] parameters = new NameValue[1];

            parameters[0] = new NameValue("PROCESS_ID", processId);

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                dynamicDataAccess.ExecuteSPDataTable("REINS.PAYMENTS_MASSIVE_PROCESS", parameters);
            }
        }

        /// <summary>
        /// LoadReinsuranceLayer
        /// Ejecuta el procedimiento de carga de reaseguros
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <param name="userId">Usuario que ejecuta el proceso</param>
        /// <param name="processType"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="prefixes"></param>
        /// <returns>int</returns>
        public int LoadReinsuranceLayer(int endorsementId, int userId, int processType,
                                                    DateTime dateFrom, DateTime dateTo, List<Prefix> prefixes)
        {
            NameValue[] parameters = new NameValue[7];

            parameters[0] = new NameValue("ENDORSEMENT_ID", endorsementId);

            if (processType == (int)ProcessTypes.Manual)
            {
                parameters[1] = new NameValue("DATE_FROM", "");//cambio a SyBase   
                parameters[2] = new NameValue("DATE_TO", "");//cambio a SyBase   
                parameters[3] = new NameValue("PROCESS_TYPE", (int)ProcessTypes.Manual);//proceso 4
            }
            else if (processType == (int)ProcessTypes.Massive)
            {
                parameters[1] = new NameValue("DATE_FROM", dateFrom.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                parameters[2] = new NameValue("DATE_TO", dateTo.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                parameters[3] = new NameValue("PROCESS_TYPE", (int)ProcessTypes.Massive);//proceso 3
            }
            parameters[4] = new NameValue("USER_ID", userId);

            #region XmlPrefix
            XDocument xDocumentPrefix = new XDocument(new XElement("PrefixMassive", new XElement("fill", new XElement("option", 0))));

            if (processType == (int)ProcessTypes.Massive && prefixes.Count > 0)
            {
                foreach (Prefix prefix in prefixes)
                {
                    xDocumentPrefix.Element("PrefixMassive").Element("fill").AddAfterSelf(new XElement("Prefixes", new XElement("prefixCd", prefix.Id.ToString())));
                }
            }

            XmlDocument xmlDocumentPrefix = new XmlDocument();
            xmlDocumentPrefix.LoadXml(xDocumentPrefix.ToString());

            #endregion

            parameters[5] = new NameValue("PREFIX_XML", xmlDocumentPrefix.DocumentElement.OuterXml); // Lista de Ramo
            parameters[6] = new NameValue("SERVICE", "0"); //esta línea no había (cambio a SyBase   )


            DataTable processes;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                processes = dynamicDataAccess.ExecuteSPDataTable("REINS.ISS_AUTOPROC1_WORKTABLES_LOAD", parameters);
            }

            int success = 0;
            int tempReinsuranceProcessId = 0;

            if (processes != null && processes.Rows.Count > 0)
            {
                foreach (DataRow process in processes.Rows)
                {
                    tempReinsuranceProcessId = int.Parse(process[0].ToString()); // El Id del Proceso
                    success = int.Parse(process[1].ToString());                  // Si fue exitoso o no
                }
            }


            if (success == 0)
            {
                return 0; // Proceso no fue exitoso
            }

            return tempReinsuranceProcessId;
        }

        /// <summary>
        /// SaveIssueReinsurance
        /// Pasa de temporales a reales y devuelve el reaseguro(s) final
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="temporalReinsurance"></param>
        /// <returns>List<Reinsurance></returns>
        public List<Models.Reinsurance.Reinsurance> SaveIssueReinsurance(Policy policy, Models.Reinsurance.Reinsurance temporalReinsurance)
        {

            NameValue[] parameters = new NameValue[5];

            parameters[0] = new NameValue("PROCESS_ID", 0); //policy.Id
            parameters[1] = new NameValue("IS_AUTOMATIC", 0); // Según EF
            parameters[2] = new NameValue("USER_ID", temporalReinsurance.UserId);
            parameters[3] = new NameValue("ENDORSEMENT_ID", policy.EndorsmentId);
            parameters[4] = new NameValue("SERVICE", 1); //DEVUELVE EL SELECT (se aumenta por sybase)


            DataTable processes;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                processes = dynamicDataAccess.ExecuteSPDataTable("REINS.ISS_AUTOPROC3_RECORDING_FINAL_TABLES", parameters);
            }
            List<Models.Reinsurance.Reinsurance> reinsurances = new List<Models.Reinsurance.Reinsurance>();

            foreach (DataRow process in processes.Rows)
            {
                Models.Reinsurance.Reinsurance reinsurance = new Models.Reinsurance.Reinsurance();

                if (Convert.ToInt32(process[1]) == 1) //FUE EXITOSO
                {

                    reinsurance.ReinsuranceId = int.Parse(process[0].ToString()); // El Id del Reaseguro Modificado

                    switch (Convert.ToInt32(process[2])) //En ésta posición debe devolver el Tipo de Movimiento
                    {
                        case (int)Movements.Original:
                            reinsurance.Movements = Utilities.Enums.Movements.Original;
                            break;

                        case (int)Movements.Counterpart:
                            reinsurance.Movements = Utilities.Enums.Movements.Counterpart;
                            break;

                        case (int)Movements.Adjustment:
                            reinsurance.Movements = Utilities.Enums.Movements.Adjustment;
                            break;
                    }
                    reinsurance.Number = Convert.ToInt32(process[3]); //Número secuencial slip por año                            

                }
                else
                {
                    reinsurance.ReinsuranceId = 0;//NO FUE EXITOSO                     
                }

                reinsurances.Add(reinsurance);

            }

            return reinsurances.OrderBy(x => x.ReinsuranceId).ToList();
        }

        /// <summary>
        /// ReinsureEndorsement
        /// Reasegurar Poliza de Emisión por el proceso automático 
        /// </summary>
        /// <param name = "policy" ></ param >
        /// < param name="userId"></param>
        /// <param name = "onLine" ></ param >
        /// < returns > Reinsurance </ returns >
        public Models.Reinsurance.Reinsurance ReinsureEndorsement(int userId, PolicyDTO policyDTO, List<LineDTO> lineDTOs)
        {
            Models.Reinsurance.Reinsurance reinsurance = new Models.Reinsurance.Reinsurance();

            //Valida si existe algún error en las coberturas
            if (ValidateLineCumulus(policyDTO.ToModel()))
            {
                //Con la implementación vía SP no se haría nada con las lineas esto serviría con la implementación basada en servicios
                if (lineDTOs.Any())
                {
                    try
                    {
                        //Calcula y graba el reaseguro esto hace el SP
                        dataFacade = _dataFacadeManager.GetDataFacade();     // Se pide un datafacade al manager de datafacades
                        this.connection = dataFacade.GetCurrentConnection(); // Se pide la conexión al datafacade

                        //Se inicia la transacción
                        this.transaction = connection.BeginTransaction();

                        sp.StoredProcedure storedProcedure = new sp.StoredProcedure(connection, this.transaction);

                        NameValue[] parameters = new NameValue[3];

                        parameters[0] = new NameValue("ENDORSEMENT_ID", policyDTO.Endorsement.Id);
                        parameters[1] = new NameValue("USER_ID", userId);
                        parameters[2] = new NameValue("SERVICE", 1); //DEVUELVE EL SELECT


                        DataTable dataTable;
                        using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                        {
                            dataTable = dynamicDataAccess.ExecuteSPDataTable("REINS.ISSUE_AUTOMATIC_PROCESS", parameters);
                        }
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            foreach (DataRow list in dataTable.Rows)
                            {
                                reinsurance.ReinsuranceId = Convert.ToInt32(list[0]);
                                reinsurance.Number = Convert.ToInt32(list[2]);
                                reinsurance.ProcessDate = Convert.ToDateTime(list[4]);
                                reinsurance.IssueDate = Convert.ToDateTime(list[5]);
                                reinsurance.ValidityFrom = Convert.ToDateTime(list[6]);
                                reinsurance.ValidityTo = Convert.ToDateTime(list[7]);
                                reinsurance.UserId = Convert.ToInt32(list[9]);


                                switch (Convert.ToInt32(list[3]))
                                {
                                    case (int)Movements.Original:
                                        reinsurance.Movements = Utilities.Enums.Movements.Original;
                                        break;

                                    case (int)Movements.Counterpart:
                                        reinsurance.Movements = Utilities.Enums.Movements.Counterpart;
                                        break;

                                    case (int)Movements.Adjustment:
                                        reinsurance.Movements = Utilities.Enums.Movements.Adjustment;
                                        break;
                                }
                            }
                        }
                        else //el SP no devolvió nada
                        {
                            reinsurance.ReinsuranceId = -1;
                        }

                        transaction.Commit();
                    }
                    catch (BusinessException ex)
                    {
                        transaction.Rollback();
                        throw new BusinessException(ex.Message, ex);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new BusinessException(ex.Message, ex);
                    }
                }
            }
            else
            {
                reinsurance.ReinsuranceId = 0;

            }
            return reinsurance;
        }

        /// <summary>
        /// Verificar codigo de error sea diferente igual a 0 ó 78 es verdadero caso contrario falso
        /// </summary>
        /// <param name = "policy" > Poliza reaseguro</param>
        /// <returns>bool</returns>
        public bool ValidateLineCumulus(Policy policy)
        {

            int errorCoverage = 0;
            bool isValid = false;

            foreach (Risk risk in policy.Endorsement.Risks)
            {
                foreach (Coverage coverage in risk.Coverages)
                {
                    if ((coverage.ErrorId == 0 || coverage.ErrorId == 78) && (Convert.ToInt32(coverage.LineId) > 0))
                    {
                        errorCoverage++;
                    }
                }
                if (errorCoverage == risk.Coverages.Count)
                {
                    isValid = true;
                }
                else
                {
                    isValid = false;
                    break;
                }

                errorCoverage = 0;
            }

            return isValid;
        }

        /// <summary>
        /// CalculateLoadReinsurance
        /// Ejecuta el procedimiento de recálculo del reaseguro
        /// y devuelve la Cabecera/Capas/Líneas/Distribución del Reaseguro
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="service"></param>
        /// <returns>ArrayList</returns>
        public DataTable CalculateLoadReinsurance(int endorsementId, int typeOperation)
        {
            NameValue[] parameters = new NameValue[4];

            parameters[0] = new NameValue("PROCESS_ID", 0);
            parameters[1] = new NameValue("DISPLAY", 1);
            parameters[2] = new NameValue("ENDORSEMENT_ID", endorsementId);
            parameters[3] = new NameValue("SERVICE", typeOperation); // 1 Calcula y trae la cabecera //2 Trae capas //3. Trae Líneas //4. Distribución

            DataTable result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("REINS.ISS_AUTOPROC2_REINSURANCE_CALCULATION", parameters);
            }
            return result;
        }

        /// <summary>
        /// GetTempReinsuranceProcess
        /// Trae información de registros procesados/fallidos del proceso masivo
        /// </summary>
        /// <param name="tempReinsuranceProcessId"></param>
        /// <param name="moduleId"></param>
        /// <returns>List<TempReinsuranceProcess/></returns>
        public List<TempReinsuranceProcess> GetTempReinsuranceProcess(int? tempReinsuranceProcessId, int moduleId)
        {
            List<TempReinsuranceProcess> tempReinsuranceProcessDTOs = new List<TempReinsuranceProcess>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            if (moduleId > 0)
            {
                if (tempReinsuranceProcessId > 0)
                {
                    criteriaBuilder.PropertyEquals(REINSEN.TempReinsuranceProcess.Properties.TempReinsuranceProcessId, tempReinsuranceProcessId);
                    criteriaBuilder.And();
                }
                criteriaBuilder.PropertyEquals(REINSEN.TempReinsuranceProcess.Properties.ModuleCode, moduleId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(REINSEN.TempReinsuranceProcess.Properties.ProcessTypeCode, ProcessTypes.Massive);
            }
            else
            {
                if (tempReinsuranceProcessId > 0)
                {
                    criteriaBuilder.PropertyEquals(REINSEN.TempReinsuranceProcess.Properties.TempReinsuranceProcessId, tempReinsuranceProcessId);
                }
                else
                {
                    criteriaBuilder.PropertyEquals(REINSEN.TempReinsuranceProcess.Properties.Status, 2); //EN PROCESO   
                }
            }

            UIView tempReinsuranceProcesses = _dataFacadeManager.GetDataFacade().GetView("GetTempReinsuranceProcess",
                criteriaBuilder.GetPredicate(), null, 0, RowsGrid, null, true, out RowsGrid);

            foreach (DataRow tempReinsuranceProcessEntity in tempReinsuranceProcesses.Rows)
            {
                TempReinsuranceProcess tempReinsuranceProcessDTO = new TempReinsuranceProcess();

                tempReinsuranceProcessDTO.TempReinsuranceProcessId = tempReinsuranceProcessEntity["TempReinsuranceProcessId"] == DBNull.Value ? 0 : Convert.ToInt32(tempReinsuranceProcessEntity["TempReinsuranceProcessId"]);
                tempReinsuranceProcessDTO.Description = tempReinsuranceProcessEntity["Description"].ToString();
                tempReinsuranceProcessDTO.RecordsFailed = tempReinsuranceProcessEntity["RecordsFailed"] == DBNull.Value ? 0 : Convert.ToInt32(tempReinsuranceProcessEntity["RecordsFailed"]);
                tempReinsuranceProcessDTO.RecordsNumber = tempReinsuranceProcessEntity["RecordsNumber"] == DBNull.Value ? 0 : Convert.ToInt32(tempReinsuranceProcessEntity["RecordsNumber"]);
                tempReinsuranceProcessDTO.RecordsProcessed = tempReinsuranceProcessEntity["RecordsProcessed"] == DBNull.Value ? 0 : Convert.ToInt32(tempReinsuranceProcessEntity["RecordsProcessed"]);
                tempReinsuranceProcessDTO.Status = tempReinsuranceProcessEntity["Status"] == DBNull.Value ? 0 : Convert.ToInt32(tempReinsuranceProcessEntity["Status"]);
                tempReinsuranceProcessDTO.StartDate = tempReinsuranceProcessEntity["StartDate"] == DBNull.Value ? Convert.ToDateTime("") : Convert.ToDateTime(tempReinsuranceProcessEntity["StartDate"].ToString());
                tempReinsuranceProcessDTO.ModuleId = tempReinsuranceProcessEntity["ModuleCode"] == DBNull.Value ? 0 : Convert.ToInt32(tempReinsuranceProcessEntity["ModuleCode"]);

                switch (tempReinsuranceProcessEntity["Status"].ToString())
                {
                    case "0":
                        tempReinsuranceProcessDTO.StatusDescription = "ERROR";
                        break;
                    case "1":
                        tempReinsuranceProcessDTO.StatusDescription = "FINALIZADO";
                        break;
                }

                tempReinsuranceProcessDTOs.Add(tempReinsuranceProcessDTO);
            }

            return tempReinsuranceProcessDTOs;
        }

        /// <summary>
        /// GetTempReinsuranceProcessDetails
        /// Trae información detalle de registros procesados/fallidos del proceso masivo
        /// </summary>
        /// <param name="tempReinsuranceProcessId"></param>
        /// <returns>List<TempReinsuranceProcess/></returns>
        public List<TempReinsuranceProcess> GetTempReinsuranceProcessDetails(int tempReinsuranceProcessId)
        {
            List<TempReinsuranceProcess> tempReinsurancesProcess = new List<TempReinsuranceProcess>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(REINSEN.TempDistributionErrorsMassive.Properties.TempReinsuranceProcessCode, typeof(REINSEN.PriorityRetention).Name, tempReinsuranceProcessId);
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(REINSEN.TempDistributionErrorsMassive), filter.GetPredicate());

            foreach (REINSEN.TempDistributionErrorsMassive entityTempDistributionErrorsMassive in businessObjects)
            {
                TempReinsuranceProcess tempReinsuranceProcess = new TempReinsuranceProcess();

                tempReinsuranceProcess.ModuleId = entityTempDistributionErrorsMassive.ModuleCode;
                tempReinsuranceProcess.TempReinsuranceProcessId = entityTempDistributionErrorsMassive.TempReinsuranceProcessCode;
                tempReinsuranceProcess.BranchId = entityTempDistributionErrorsMassive.BranchCode;
                tempReinsuranceProcess.BranchDescription = entityTempDistributionErrorsMassive.BranchDescription;
                tempReinsuranceProcess.PrefixId = entityTempDistributionErrorsMassive.PrefixCode;
                tempReinsuranceProcess.PrefixDescription = entityTempDistributionErrorsMassive.PrefixDescription;
                tempReinsuranceProcess.PolicyNumber = entityTempDistributionErrorsMassive.PolicyNumber.ToString();
                tempReinsuranceProcess.RiskNumber = entityTempDistributionErrorsMassive.RiskNumber;
                tempReinsuranceProcess.CoverageNumber = entityTempDistributionErrorsMassive.CoverageNumber;
                tempReinsuranceProcess.EndorsementNumber = entityTempDistributionErrorsMassive.EndorsementNumber;
                tempReinsuranceProcess.ErrorDescription = entityTempDistributionErrorsMassive.ErrorDescription;

                tempReinsurancesProcess.Add(tempReinsuranceProcess);
            }

            return tempReinsurancesProcess;
        }

        /// <summary>
        /// GetModules
        /// Trae los módulos de proceso
        /// </summary>
        /// <returns>List<Module/></returns>
        public List<Module> GetModules()
        {
            List<Module> moduleDTOs = new List<Module>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.Property(REINSEN.Module.Properties.ModuleId);
            criteriaBuilder.Greater();
            criteriaBuilder.Constant(0);

            UIView modules = _dataFacadeManager.GetDataFacade().GetView("GetModules", criteriaBuilder.GetPredicate(),
                                                                 null, 0, -1, null, true, out RowsGrid);

            foreach (DataRow modulesEntity in modules.Rows)
            {
                Module moduleDTO = new Module();

                moduleDTO.Id = Convert.ToInt32(modulesEntity["ModuleId"]);
                moduleDTO.Description = modulesEntity["Description"].ToString();
                moduleDTOs.Add(moduleDTO);
            }
            return moduleDTOs;
        }

        /// <summary>
        /// GetReinsuranceAccountingParameters
        /// Utilizado para contabilización de Reaseguros: Endoso/Siniestro/Pago
        /// </summary>
        /// <param name="processId"></param>
        /// <returns>List<ReinsuranceAccountingParameter/></returns>
        public List<ReinsuranceAccountingParameter> GetReinsuranceAccountingParameters(int processId)
        {
            List<ReinsuranceAccountingParameter> reinsuranceAccountingParameterDTOs = new List<ReinsuranceAccountingParameter>();
            NameValue[] parameters = new NameValue[1];

            parameters[0] = new NameValue("REINSURANCE_ID", processId);

            DataTable accountingParameters;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                accountingParameters = dynamicDataAccess.ExecuteSPDataTable("REINS.GET_REINSURANCE_ACCOUNTING_PARAMETERS", parameters);
            }

            if (accountingParameters.Rows.Count > 0)
            {
                foreach (DataRow accountingParameter in accountingParameters.Rows)
                {
                    ReinsuranceAccountingParameter reinsuranceAccountingParameterDTO = new ReinsuranceAccountingParameter();

                    reinsuranceAccountingParameterDTO.BranchCd = Convert.ToInt32(accountingParameter[0]);
                    reinsuranceAccountingParameterDTO.CompanyTypeId = Convert.ToInt32(accountingParameter[1]);
                    reinsuranceAccountingParameterDTO.ConceptId = Convert.ToInt32(accountingParameter[2]);
                    reinsuranceAccountingParameterDTO.ContractTypeId = Convert.ToInt32(accountingParameter[3]);
                    reinsuranceAccountingParameterDTO.CurrencyCd = Convert.ToInt32(accountingParameter[4]);
                    reinsuranceAccountingParameterDTO.EndorsementDocumentNumber = Convert.ToString(accountingParameter[5]);
                    reinsuranceAccountingParameterDTO.EndorsementId = Convert.ToInt32(accountingParameter[6]);
                    reinsuranceAccountingParameterDTO.ExchangeRate = Convert.ToDecimal(accountingParameter[7]);
                    reinsuranceAccountingParameterDTO.IncomeAmountValue = accountingParameter[8] == DBNull.Value ? 0 : Convert.ToDecimal(accountingParameter[8]);
                    reinsuranceAccountingParameterDTO.CompanyId = Convert.ToInt32(accountingParameter[9]);
                    reinsuranceAccountingParameterDTO.PolicyDocumentNumber = Convert.ToString(accountingParameter[10]);
                    reinsuranceAccountingParameterDTO.PolicyId = Convert.ToInt32(accountingParameter[11]);
                    reinsuranceAccountingParameterDTO.PrefixCd = Convert.ToInt32(accountingParameter[12]);
                    reinsuranceAccountingParameterDTO.PrefixDescription = Convert.ToString(accountingParameter[13]);
                    reinsuranceAccountingParameterDTO.LocalAmount = accountingParameter[14] == DBNull.Value ? 0 : Convert.ToDecimal(accountingParameter[14]);
                    reinsuranceAccountingParameterDTO.BranchDescription = Convert.ToString(accountingParameter[15]);
                    reinsuranceAccountingParameterDTO.Description = Convert.ToString(accountingParameter[16]);

                    reinsuranceAccountingParameterDTOs.Add(reinsuranceAccountingParameterDTO);
                }
            }

            return reinsuranceAccountingParameterDTOs;
        }

        /// <summary>
        /// Obtiene la distribución por coberturas de reaseguros por id del endoso
        /// </summary>
        /// <param name="endorsmentId"></param>
        /// <returns></returns>
        public List<IssueAllocationRiskCover> GetIssueAllocationRiskCoveragesByEndorsementId(int endorsmentId, List<int> issueReinsuranceIds = null)
        {
            List<IssueAllocationRiskCover> issueAllocationRiskCoverages = new List<IssueAllocationRiskCover>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            BusinessCollection businessObjects = new BusinessCollection();

            filter.PropertyEquals(REINSEN.IssueAllocationRiskCover.Properties.EndorsementId, typeof(REINSEN.IssueAllocationRiskCover).Name, endorsmentId);

            if (issueReinsuranceIds != null)
            {
                for (int i = 0; i < issueReinsuranceIds.Count; i++)
                {
                    if (i == 0)
                    {
                        filter.And();
                    }

                    filter.PropertyEquals(REINSEN.IssueAllocationRiskCover.Properties.IssueReinsuranceId, typeof(REINSEN.IssueAllocationRiskCover).Name, issueReinsuranceIds[i]);

                    if (i < issueReinsuranceIds.Count - 1)
                    {
                        filter.Or();
                    }

                }
            }

            businessObjects = DataFacadeManager.GetObjects(typeof(REINSEN.IssueAllocationRiskCover), filter.GetPredicate());
            issueAllocationRiskCoverages = businessObjects.CreateIssueAllocationRiskCoverages();
            return issueAllocationRiskCoverages;
        }

        public List<IssueAllocationRiskCover> GetIssueAllocationRiskCoverages()
        {
            List<IssueAllocationRiskCover> issueAllocationRiskCoverages = new List<IssueAllocationRiskCover>();
            BusinessCollection businessObjects = new BusinessCollection();
            businessObjects = DataFacadeManager.GetObjects(typeof(REINSEN.IssueAllocationRiskCover));
            issueAllocationRiskCoverages = businessObjects.CreateIssueAllocationRiskCoverages();
            return issueAllocationRiskCoverages;
        }


        public List<Policy> GetTempIssuesByProcessId(int processId)
        {
            List<Policy> policies = new List<Policy>();
            NameValue[] parameters = new NameValue[1];

            parameters[0] = new NameValue("PROCESS_ID", processId);

            DataTable dataTable;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                dataTable = dynamicDataAccess.ExecuteSPDataTable("REINS.GET_POLICIES_TO_REINSURE", parameters);
            }

            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    Policy policy = new Policy();
                    Risk risk = new Risk();
                    policy.Prefix = new Prefix();
                    policy.Endorsement = new Endorsement();
                    policy.Endorsement.Risks = new List<Risk>();
                    policy.Endorsement.Id = Convert.ToInt32(row[0]);
                    policy.Id = Convert.ToInt32(row[1]);
                    policy.Endorsement.IssueDate = Convert.ToDateTime(row[2]);
                    risk.IndividualId = Convert.ToInt32(row[3]);
                    policy.Endorsement.Risks.Add(risk);
                    policy.Prefix.Id = Convert.ToInt32(row[4]);
                    policies.Add(policy);
                }
            }

            return policies;
        }

        public List<Policy> GetIssueReinsuranceByProcessId(int processId)
        {
            List<Policy> policies = new List<Policy>();
            NameValue[] parameters = new NameValue[1];

            parameters[0] = new NameValue("PROCESS_ID", processId);

            DataTable dataTable;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                dataTable = dynamicDataAccess.ExecuteSPDataTable("REINS.GET_REINSURED_POLICIES", parameters);
            }

            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    Policy policy = new Policy();
                    policy.Endorsement = new Endorsement();
                    policy.Endorsement.Id = Convert.ToInt32(row[0]);

                    policies.Add(policy);
                }
            }

            return policies;
        }

        public bool DeleteReinsurance(decimal documentNumber, int endorsementNumber)
        {
            NameValue[] parameters = new NameValue[2];

            parameters[0] = new NameValue("ENDORSEMENT_NUMBER", endorsementNumber);
            parameters[1] = new NameValue("DOCUMENT_NUM", documentNumber);

            DataTable result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("REINS.DELETE_ISSUE_REINSURANCE", parameters);
            }

            if(result.Rows.Count > 0)
            {
                DataRow row;
                row = result.Rows[0];
                if (Convert.ToInt32(row[0]) == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }
}
