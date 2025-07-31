using AutoMapper;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using EnumUnderwriting = Sistran.Core.Application.UnderwritingServices.Enums;
using HelperCommon = Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.UniquePersonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Company.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.Vehicles.VehicleCollectiveServices.EEProvider.DAOs
{
    /// <summary>
    /// Proceso Tarifacion Colectivas Autos Asincrono
    /// </summary>
    public class TariffedMassiveDAO
    {

        ///// <summary>
        ///// Prepara los cargues especificados que cumplan con las condicion previas para la tarifacion por parte del servicio
        ///// </summary>
        ///// <param name="TempId">Identificador del temporal</param>
        ///// <param name="massiveLoadIds">Lista con los MassiveLoadId</param>
        ///// <returns> Modelo Error, con ObjectId 0 si todo ha transcurrido correctamente</returns>
        public Error ExecuteTariffedMassiveLoadByTempIdMassiveLoadIds(int tempId, List<int> massiveLoadIds, int userId, bool isCollective)
        {
            try
            {
                Types.FieldSetType fieldSet = new Types.FieldSetType();
                Error error = new Error();

                if (massiveLoadIds != null && massiveLoadIds.Count > 0)
                {
                    int temporaryId = tempId;
                    //Se consultan todos los registros del cargue 
                    int massiveLoadIdStart = 0;
                    List<int> massiveTariffeds = new List<int>();

                    foreach (int massiveLoadId in massiveLoadIds)
                    {
                        MassiveLoad massiveLoad = DelegateService.collectiveService.GetMassiveLoadByMassiveLoadId(massiveLoadId, true);

                        massiveLoadIdStart = massiveLoadId;

                        Types.LoadMassiveStateType status = (Types.LoadMassiveStateType)massiveLoad.State;
                        fieldSet = (Types.FieldSetType)massiveLoad.FieldSetId;

                        switch (status)
                        {
                            case Types.LoadMassiveStateType.Tariffing:
                            case Types.LoadMassiveStateType.Initial:
                                error.ObjectId = massiveLoadId;
                                error.ErrorMessage = "El cargue se encuentra tarifando";
                                break;
                            case Types.LoadMassiveStateType.ChargeCompleted:
                            case Types.LoadMassiveStateType.CompletedWithErrors:

                                if (fieldSet == Types.FieldSetType.RisksExclusion)
                                {
                                    DelegateService.collectiveService.ExecuteExcludeCollective(temporaryId, massiveLoadIdStart);
                                }
                                else
                                {
                                    massiveTariffeds.Add(massiveLoadId);
                                }
                                break;
                            case Types.LoadMassiveStateType.TariffedWithOutEvents:
                                error.ObjectId = massiveLoadId;
                                error.ErrorMessage = "El cargue se encuentra tarifado";
                                break;
                            case Types.LoadMassiveStateType.TariffedWithEvents:
                                error.ObjectId = massiveLoadId;
                                error.ErrorMessage = "El cargue se encuentra tarifado con eventos";
                                break;
                            case Types.LoadMassiveStateType.Issued:
                                error.ObjectId = massiveLoadId;
                                error.ErrorMessage = "El cargue se encuentra Emitido";
                                break;
                            case Types.LoadMassiveStateType.Issuing:
                                error.ObjectId = massiveLoadId;
                                error.ErrorMessage = "El cargue se encuentra en proceso de emicion";
                                break;
                            case Types.LoadMassiveStateType.MovingTemporary:
                                error.ObjectId = massiveLoadId;
                                error.ErrorMessage = "El cargue se encuentra trasladando a temporales";
                                break;
                            default:
                                error.ObjectId = massiveLoadId;
                                error.ErrorMessage = "Estado de cargue especificado no existe";
                                break;
                        }
                        if (error.ObjectId != 0)
                        {
                            break;
                        }
                    }

                    if (fieldSet == Types.FieldSetType.RisksExclusion)
                    {
                        Policy policy = new Policy();
                        //Se consultan los datos del cargue
                        massiveLoadIds = new List<int>();
                        GetMassiveLoadInfo(policy, massiveLoadIds, temporaryId, false);
                        DelegateService.collectiveService.UpdateTemporal(policy);
                    }
                    Task Task = Task.Run(() => ExecuteTariffedMassiveLoad(massiveTariffeds, userId, temporaryId));
                }
                else
                {
                    error.ErrorMessage = "No se ha especificado un cargue";
                }
                return error;
            }
            catch (Exception ex)
            {
                for (int i = 0; i < massiveLoadIds.Count; i++)
                {
                    DelegateService.collectiveService.UpdateMassiveLoadByMassiveState(massiveLoadIds[i], Types.LoadMassiveStateType.TariffedWithErrors);
                }
                throw new Exception("MassiveProvider-ExecuteTariffedMassiveLoadByTempIdMassiveLoadIds", ex);
            }
        }

        ///// <summary>
        ///// Ejecución de hilos para proceso de tarifación
        ///// </summary>
        ///// <param name="massiveLoadIds">Id de cargues a tarifar</param>
        ///// <param name="temporalId">Id temporal</param>
        ///// <returns> Modelo Error, con ObjectId 0 si todo ha transcurrido correctamente</returns>
        public void ExecuteTariffedThreads(List<int> massiveLoadIds, int temporalId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                //Se consulta la parametrización para los hilos
                List<int> packageProcesses = DataFacadeManager.GetPackageProcesses(massiveLoadIds.Count, "MaxThreadCollective");
                CompanyPolicy vehiclePolicyStart = new CompanyPolicy();
                if (packageProcesses.Count > 0)
                {
                    int cont = 0;
                    //Se tarifa el primer Riesgo
                    List<MassiveCollectiveDetail> massiveCollectiveDetails = new List<MassiveCollectiveDetail>();
                    List<MassiveCollectiveDetail> massiveCollectiveDetailsSnError = new List<MassiveCollectiveDetail>();
                    massiveCollectiveDetails = DelegateService.collectiveService.GetMassiveCollectivedetailsByTempIdMassiveLoadId(temporalId, massiveLoadIds[0]);
                    //Se recuperan los registros que no tienen error para tarifarlos
                    massiveCollectiveDetailsSnError = massiveCollectiveDetails.Where(x => x.SnError != true).ToList();
                    if (massiveCollectiveDetailsSnError != null)
                    {
                        //vehiclePolicyStart = TarrifedRiskCar(temporalId, (int)massiveCollectiveDetailsSnError[0].IdRisk);
                    }
                    //Se inicia la Carga de Hilos
                    for (int b = 0; b < packageProcesses.Count; b++)
                    {
                        //Administrador de Hilos
                        List<Task> lstTask = new List<Task>();
                        for (int i = 0; i < packageProcesses[b]; i++)
                        {
                            int pos = cont;
                            //lstTask.Add(Task.Run(() => ExecuteTariffedCollective(massiveLoadIds[pos], temporalId, vehiclePolicyStart)));
                            cont++;
                        }
                        try
                        {
                            Task.WaitAll(lstTask.ToArray());
                        }
                        catch (AggregateException ae)
                        {
                            System.Diagnostics.EventLog.WriteEntry("Application", ae.Flatten().Message);
                            DelegateService.collectiveService.UpdateMassiveLoadByMassiveState(temporalId, Types.LoadMassiveStateType.TariffedWithErrors);
                        }
                    }

                    //Ejecutan evento                   
                    DelegateService.collectiveService.ExecuteEventByMassiveLoadTemporalId(massiveLoadIds, temporalId);
                    Policy policy = new Policy();
                    massiveLoadIds = new List<int>();
                    GetMassiveLoadInfo(policy, massiveLoadIds, temporalId, false);
                    DelegateService.collectiveService.UpdateTemporal(policy);

                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", "Temporal:" + temporalId.ToString() + ":" + massiveLoadIds.ToString() + "-" + ex.StackTrace.ToString(), EventLogEntryType.Error);
                for (int i = 0; i < massiveLoadIds.Count; i++)
                {
                    DelegateService.collectiveService.UpdateMassiveLoadByMassiveState(massiveLoadIds[i], Types.LoadMassiveStateType.TariffedWithErrors);
                }
            }
            finally
            {
                DataFacadeManager.Dispose();
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.MassiveUnderwritingServices.EEProvider.DAOs.ExecuteTariffedThreads" + temporalId.ToString());
            }

        }

        ///// <summary>
        ///// Ejecución de hilos para proceso de tarifación
        ///// </summary>
        ///// <param name="massiveLoadIds">Id de cargues a tarifar</param>
        ///// <param name="userId">Usuario</param>
        ///// <param name="temporalId">Id temporal</param>
        public void ExecuteTariffedMassiveLoad(List<int> massiveLoadIds, int userId, int temporaryId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                DelegateService.collectiveService.ExecuteMassiveCollectiveTempByMassiveLoad(massiveLoadIds, userId);
                ExecuteTariffedThreads(massiveLoadIds, temporaryId);
            }
            catch (Exception ex)
            {
                for (int i = 0; i < massiveLoadIds.Count; i++)
                {
                    DelegateService.collectiveService.UpdateMassiveLoadByMassiveState(massiveLoadIds[i], Types.LoadMassiveStateType.TariffedWithErrors);
                }
                EventLog.WriteEntry("Application", "Temporal:" + temporaryId.ToString() + "-" + ex.StackTrace.ToString(), EventLogEntryType.Error);
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.Vehicles.VehicleCollectiveServices.EEProvider.DAOs.ExecuteTariffedMassiveLoad:" + temporaryId.ToString());
        }

        ///// <summary>
        ///// Obtiene información de la póliza y tarifa componentes
        ///// </summary>
        ///// <param name="policy">Poliza</param>
        ///// <param name="massiveLoadId">Id de cargue</param>
        ///// <param name="temporaryId">Id temporal</param>
        public void GetMassiveLoadInfo(Policy policy, List<int> listMassiveLoad, int temporaryId, bool withOutEvent)
        {
            VehicleMassiveDAO vehicleMassiveDAO = new VehicleMassiveDAO();
            CompanyPolicy vehiclePolicyLast = new CompanyPolicy();
            vehiclePolicyLast = vehicleMassiveDAO.GetCompanyVehiclePolicyByTemporalIdMassiveId(temporaryId, listMassiveLoad, withOutEvent);
            vehiclePolicyLast.PayerComponents = new List<PayerComponent>();
            //vehiclePolicyLast = CalculatePolicyAmounts(vehiclePolicyLast);
            Mapper.CreateMap(vehiclePolicyLast.GetType(), policy.GetType());
            Mapper.Map(vehiclePolicyLast, policy);
        }

        ///// <summary>
        ///// Actualizar componentes y sumatoria de la póliza
        ///// </summary>
        ///// <param name="vehiclePolicy">vehiclePolicy</param>
        //private CompanyVehiclePolicy CalculatePolicyAmounts(CompanyVehiclePolicy vehiclePolicy)
        //{
        //    List<Risk> risks = new List<Risk>();

        //    foreach (CompanyVehicle item in vehiclePolicy.CompanyVehicles)
        //    {
        //        risks.Add(item);
        //    }

        //    vehiclePolicy.PayerComponents = DelegateService.underwritingService.CalculatePayerComponents(vehiclePolicy, risks);

        //    vehiclePolicy = CalculateSummary(vehiclePolicy);
        //    if (vehiclePolicy.Summary != null)
        //    {
        //        vehiclePolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(vehiclePolicy);
        //        vehiclePolicy = CalculateCommissions(vehiclePolicy);
        //    }
        //    else
        //    {
        //        vehiclePolicy.PaymentPlan.Quotas = new List<Quota>();
        //        foreach (Agency agency in vehiclePolicy.Agencies)
        //        {
        //            foreach (Commission item in agency.Commissions)
        //            {
        //                item.CalculateBase = 0;
        //                item.Amount = 0;
        //            }
        //        }
        //    }

        //    return vehiclePolicy;
        //}

        /////// <summary>
        /////// Actualizar componentes y sumatoria de la póliza
        /////// </summary>
        /////// <param name="vehiclePolicy">CompanyVehiclePolicy</param>
        //public CompanyVehiclePolicy CalculateSummary(CompanyVehiclePolicy vehiclePolicy)
        //{
        //    vehiclePolicy.Summary = new Summary();
        //    foreach (PayerComponent payer in vehiclePolicy.PayerComponents)
        //    {
        //        switch (payer.Component.ComponentType)
        //        {
        //            case EnumUnderwriting.ComponentType.Premium:
        //                vehiclePolicy.Summary.AmountInsured += payer.BaseAmount;
        //                vehiclePolicy.Summary.Premium += payer.Amount;
        //                break;
        //            case EnumUnderwriting.ComponentType.Expenses:
        //                vehiclePolicy.Summary.Expenses += payer.Amount;
        //                break;
        //            case EnumUnderwriting.ComponentType.Taxes:
        //                vehiclePolicy.Summary.Taxes += payer.Amount;
        //                break;

        //            default:
        //                break;
        //        }
        //    }

        //    if (vehiclePolicy.BusinessType == EnumUnderwriting.BusinessType.Accepted)
        //    {
        //        vehiclePolicy.Summary.Taxes = 0;
        //    }
        //    if (vehiclePolicy.Summary.Premium != 0)
        //    {
        //        vehiclePolicy.Summary.FullPremium = vehiclePolicy.Summary.Premium + vehiclePolicy.Summary.Expenses + vehiclePolicy.Summary.Taxes;
        //    }
        //    else
        //    {
        //        vehiclePolicy.Summary = null;
        //    }

        //    return vehiclePolicy;
        //}

        /////// <summary>
        /////// Calcular Comisiones
        /////// </summary>
        /////// <param name="vehiclePolicy">CompanyVehiclePolicy</param>
        //private CompanyVehiclePolicy CalculateCommissions(CompanyVehiclePolicy vehiclePolicy)
        //{
        //    foreach (Agency item in vehiclePolicy.Agencies)
        //    {
        //        if (item.Commissions == null || item.Commissions.Count == 0)
        //        {
        //            item.Participation = 100;
        //            item.Commissions = new List<Commission>();
        //            Commission commission = new Commission();

        //            commission.Percentage = DelegateService.underwritingService.GetCommissByAgentIdAgencyIdProductId(vehiclePolicy.Agencies[0].Agent.IndividualId, vehiclePolicy.Agencies[0].Id, vehiclePolicy.Product.Id).CommissPercentage;
        //            commission.PercentageAdditional = 0;
        //            commission.CalculateBase = vehiclePolicy.Summary.Premium;
        //            commission.SubLineBusiness = vehiclePolicy.CompanyVehicles[0].Coverages[0].SubLineBusiness;
        //            commission.Amount = (commission.CalculateBase * (commission.Percentage + commission.PercentageAdditional)) / 100;
        //            item.Commissions.Add(commission);
        //        }
        //        else
        //        {
        //            if (item.Commissions[0].Percentage == 0)
        //            {
        //                item.Commissions[0].Percentage = DelegateService.underwritingService.GetCommissByAgentIdAgencyIdProductId(vehiclePolicy.Agencies[0].Agent.IndividualId, vehiclePolicy.Agencies[0].Id, vehiclePolicy.Product.Id).CommissPercentage;
        //            }
        //            item.Commissions[0].SubLineBusiness = vehiclePolicy.CompanyVehicles[0].Coverages[0].SubLineBusiness;
        //            item.Commissions[0].CalculateBase = ((vehiclePolicy.Summary.Premium * item.Participation) / 100);
        //            item.Commissions[0].Amount = (item.Commissions[0].CalculateBase * (item.Commissions[0].Percentage + item.Commissions[0].PercentageAdditional)) / 100;
        //        }
        //    }

        //    return vehiclePolicy;
        //}

        ///// <summary>
        ///// Ejecutar Tarifacion Riesgos
        ///// </summary>
        ///// <param name="massiveLoadId">Id del Cargue</param>
        ///// <param name="temporaryId">Temporal</param>
        ///// <param name="vehiclePolicy">CompanyVehiclePolicy</param>
        //public void ExecuteTariffedCollective(int massiveLoadId, int temporaryId, CompanyVehiclePolicy vehiclePolicyStart)
        //{
        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();

        //    if (Thread.CurrentThread.Name == null)
        //    {
        //        Thread.CurrentThread.Name = "ExecuteTariffedCollective :" + massiveLoadId.ToString() + ":" + temporaryId.ToString();
        //    }
        //    try
        //    {
        //        List<MassiveCollectiveDetail> massiveCollectiveDetails = new List<MassiveCollectiveDetail>();
        //        List<MassiveCollectiveDetail> massiveCollectiveDetailsSnError = new List<MassiveCollectiveDetail>();
        //        //Se consultan los datos del cargue
        //        MassiveLoad massiveLoad = DelegateService.collectiveService.GetMassiveLoadByMassiveLoadId(massiveLoadId, true);
        //        //Se actualiza el estado del cargue Inicial de tarifación
        //        DelegateService.collectiveService.UpdateMassiveLoadByMassiveIdStateId(massiveLoadId, Types.LoadMassiveStateType.Tariffing);
        //        //Se Obtienen los registros del cargue
        //        massiveCollectiveDetails = DelegateService.collectiveService.GetMassiveCollectivedetailsByTempIdMassiveLoadId(temporaryId, massiveLoadId);
        //        //Se recuperan los registros que no tienen error para tarifarlos
        //        massiveCollectiveDetailsSnError = massiveCollectiveDetails.Where(x => x.SnError != true).ToList();
        //        //Se realiza el calculo de bloques para tarifar 
        //        List<int> packageProcesses = DataFacadeManager.GetPackageProcesses(massiveCollectiveDetailsSnError.Count, "MaxThreadCollective");

        //        if (packageProcesses.Count > 0)
        //        {
        //            if (massiveCollectiveDetailsSnError.Count > 0)
        //            {
        //                int cont = 0;
        //                //Se inicia la Carga de Hilos
        //                for (int b = 0; b < packageProcesses.Count; b++)
        //                {
        //                    //Administrador de Hilos
        //                    List<Task> lstTask = new List<Task>();
        //                    for (int i = 0; i < packageProcesses[b]; i++)
        //                    {
        //                        int pos = cont;
        //                        if (vehiclePolicyStart != null && vehiclePolicyStart.CompanyVehicles != null)
        //                        {
        //                            CompanyVehiclePolicy vehiclePolicyThread = new CompanyVehiclePolicy();
        //                            Mapper.CreateMap(vehiclePolicyStart.GetType(), vehiclePolicyThread.GetType());
        //                            Mapper.Map(vehiclePolicyStart, vehiclePolicyThread);
        //                            vehiclePolicyThread.CompanyVehicles = vehiclePolicyThread.CompanyVehicles.Where(x => x.RiskId == massiveCollectiveDetailsSnError[pos].IdRisk).ToList();

        //                            lstTask.Add(Task.Run(() => ExecuteTariffedTempThreadsCars(vehiclePolicyThread, massiveLoadId, massiveCollectiveDetailsSnError[pos])));
        //                        }
        //                        cont++;
        //                    }
        //                    try
        //                    {
        //                        Task.WaitAll(lstTask.ToArray());
        //                    }
        //                    catch (AggregateException ae)
        //                    {
        //                        System.Diagnostics.EventLog.WriteEntry("Application", "Temporal" + ":" + temporaryId.ToString() + " IdCargue:" + massiveLoadId.ToString() + ae.Flatten().Message);
        //                        DelegateService.collectiveService.UpdateMassiveLoadByMassiveState(massiveLoadId, Types.LoadMassiveStateType.TariffedWithErrors);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        try
        //        {
        //            //Se actualiza el estado del cargue para el error           
        //            DelegateService.collectiveService.UpdateMassiveLoadByMassiveState(massiveLoadId, Types.LoadMassiveStateType.TariffedWithErrors);
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //        finally
        //        {
        //            EventLog.WriteEntry("Application", "Temporal" + ":" + temporaryId.ToString() + " IdCargue:" + massiveLoadId.ToString() + ex.GetBaseException().Message);
        //        }
        //    }
        //    finally
        //    {
        //        DataFacadeManager.Dispose();

        //        stopWatch.Stop();
        //        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.Vehicles.VehicleCollectiveServices.EEProvider.DAOs.ExecuteTariffedCollective:" + temporaryId.ToString() + "-Cargue" + massiveLoadId.ToString());
        //    }
        //}

        /////// <summary>
        /////// Ejecutar Tarifacion Vehiculo
        /////// </summary>
        /////// <param name="vehiclePolicy">CompanyVehiclePolicy</param>
        /////// <param name="massiveLoadId">Id del Cargue</param>
        /////// <param name="massiveCollectiveDetail">massiveCollectiveDetail</param>
        //public void ExecuteTariffedTempThreadsCars(CompanyVehiclePolicy vehiclePolicy, int massiveLoadId, MassiveCollectiveDetail massiveCollectiveDetail)
        //{
        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();

        //    if (Thread.CurrentThread.Name == null)
        //    {
        //        Thread.CurrentThread.Name = "ExecuteTariffedCollective :" + massiveLoadId.ToString() + ":" + massiveCollectiveDetail.LicensePlate.ToString();
        //    }
        //    try
        //    {
        //        bool validTariffedRisk = false;
        //        if (vehiclePolicy != null)
        //        {
        //            if (vehiclePolicy.CompanyVehicles != null)
        //            {
        //                vehiclePolicy.IsPersisted = true;
        //                vehiclePolicy.CompanyVehicles.ForEach(x => x.IsPersisted = true);

        //                if (vehiclePolicy.CompanyVehicles.Count > 0)
        //                {
        //                    vehiclePolicy = DelegateService.vehicleService.Quotate(vehiclePolicy, true, true);
        //                }
        //                else
        //                {
        //                    validTariffedRisk = true;
        //                }
        //            }
        //            foreach (CompanyVehicle item in vehiclePolicy.CompanyVehicles)
        //            {
        //                if (item.Premium == 0)
        //                {
        //                    validTariffedRisk = true;
        //                }
        //            }
        //            if (!validTariffedRisk)
        //            {
        //                foreach (CompanyVehicle risk in vehiclePolicy.CompanyVehicles)
        //                {
        //                    DelegateService.vehicleService.CreateVehicleTemporal(vehiclePolicy.Id, risk);
        //                }
        //                DelegateService.collectiveService.UpdateTariffedByMassiveLoadIdRiskId(massiveLoadId, (int)massiveCollectiveDetail.IdRisk);
        //            }
        //            else
        //            {
        //                //UPDATE CONTROL MASSIVE LOAD ERROR SIN ID JSON
        //                EventLog.WriteEntry("Application", "Temporal" + ":" + vehiclePolicy.Id.ToString() + " IdCargue:" + massiveLoadId.ToString() + " Placa :" + massiveCollectiveDetail.LicensePlate + "--" + "Vehiculo Sin prima");
        //                DelegateService.collectiveService.UpdateControlTarrifError(massiveLoadId, (int)massiveCollectiveDetail.IdExcelRisk, vehiclePolicy.CompanyVehicles[0].RiskId, "Vehiculo Sin prima");
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        try
        //        {
        //            //UPDATE CONTROL MASSIVE LOAD ERROR POR POLIZA
        //            EventLog.WriteEntry("Application", "Temporal" + ":" + vehiclePolicy.Id.ToString() + " IdCargue:" + massiveLoadId.ToString() + " Placa :" + massiveCollectiveDetail.LicensePlate + "--" + ex.GetBaseException().Message, EventLogEntryType.Error);
        //            DelegateService.collectiveService.UpdateControlTarrifError(massiveLoadId, (int)massiveCollectiveDetail.IdExcelRisk, 0, "Error Calculando Prima");
        //        }
        //        catch (Exception ex1)
        //        {
        //            new Exception(massiveLoadId.ToString() + ":" + massiveCollectiveDetail.LicensePlate.ToString(), ex1);
        //        }
        //        new Exception("ExecuteTariffedTempThreadsCars :" + massiveLoadId.ToString() + ":" + massiveCollectiveDetail.LicensePlate.ToString(), ex);
        //    }
        //    finally
        //    {
        //        DataFacadeManager.Dispose();

        //        stopWatch.Stop();
        //        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.Vehicles.VehicleCollectiveServices.EEProvider.DAOs.ExecuteTariffedCollective: " + massiveLoadId.ToString() + "-Placa" + massiveCollectiveDetail.LicensePlate.ToString());
        //    }
        //}

        /// <summary>
        /// Tarrifeds the risk car.
        /// </summary>
        /// <param name="tempId">The temporary identifier.</param>
        ///// <param name="riskId">The risk identifier.</param>
        //private CompanyVehiclePolicy TarrifedRiskCar(int tempId, int riskId)
        //{
        //    CompanyVehiclePolicy vehiclePolicyStart = new CompanyVehiclePolicy();

        //    vehiclePolicyStart = DelegateService.vehicleService.GetVehiclePolicyByTemporalId(tempId);

        //    if (vehiclePolicyStart != null)
        //    {
        //        vehiclePolicyStart.IsPersisted = true;
        //        vehiclePolicyStart.CompanyVehicles.ForEach(x => x.IsPersisted = true);

        //        if (vehiclePolicyStart.Product.CoveredRisk == null)
        //        {
        //            vehiclePolicyStart.Product.CoveredRisk = DelegateService.underwritingService.GetProductCoverRiskTypeByProductId(vehiclePolicyStart.Product.Id);
        //        }

        //        CompanyVehiclePolicy vehiclePolicyTmp = new CompanyVehiclePolicy();

        //        Mapper.CreateMap(vehiclePolicyStart.GetType(), vehiclePolicyTmp.GetType());
        //        Mapper.Map(vehiclePolicyStart, vehiclePolicyTmp);

        //        List<CompanyVehicle> risksTmp = new List<CompanyVehicle>();
        //        CompanyVehicle vehicle = vehiclePolicyTmp.CompanyVehicles.Where(x => x.RiskId == riskId).First();

        //        risksTmp = new List<CompanyVehicle>();
        //        risksTmp.Add(vehicle);
        //        vehiclePolicyTmp.CompanyVehicles = risksTmp;
        //        if (vehiclePolicyTmp.CompanyVehicles != null)
        //        {
        //            DelegateService.vehicleService.Quotate(vehiclePolicyTmp, true, true);
        //        }
        //        CalculatePolicyAmounts(vehiclePolicyStart);
        //    }
        //    return vehiclePolicyStart;
        //}
    }
}
