using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.Helper;
using UTILITY = Sistran.Core.Application.Utilities.Utility;
using Sistran.Core.Application.Utilities.DataFacade;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Enums;
using COMMEN = Sistran.Core.Application.Common.Entities;
using COMMENMOD = Sistran.Core.Application.CommonService.Models;
using PRODEN = Sistran.Core.Application.Product.Entities;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.BAF;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.DAOs
{
    public class OperationQuotaDAO
    {
        /// <summary>
        /// Retorna el Id del Ultimo Registro de la tabla
        /// </summary>
        /// <returns></returns>
        /// 
        static object obj = new object();
        public int GetOperationQuotaEventId()
        {
            int NumberParameter = 0;
            List<Task> agentTask = new List<Task>();
            agentTask.Add(Task.Run(() =>
            {
                lock (obj)
                {
                    Parameter parameter = DelegateService.commonServiceCore.GetParameterByDescription("OPERATING_QUOTA_EVENT");
                    parameter.NumberParameter += 1;
                    DelegateService.commonServiceCore.UpdateParameter(parameter);
                    NumberParameter = (int)parameter.NumberParameter;

                }
            }));
            Task.WaitAll(agentTask.ToArray());
            return NumberParameter;
        }

        /// <summary>
        /// Insertar Evento Asignacion Cupo Operativo.
        /// </summary>
        /// <param name="operatingQuotaEvent"></param>
        /// <returns></returns>
        public OperatingQuotaEvent InsertOperatingQuotaEvent(OperatingQuotaEvent operatingQuotaEvent)
        {
            operatingQuotaEvent.Payload = JsonHelper.SerializeObjectToJson(operatingQuotaEvent.IndividualOperatingQuota);
            UPEN.OperatingQuotaEvent entityOperatingQuotaEvent = EntityAssembler.CreateOperatingQuotaEvent(operatingQuotaEvent);
            DataFacadeManager.Insert(entityOperatingQuotaEvent);
            operatingQuotaEvent = ModelAssembler.CreateOperatingQuotaEvent(entityOperatingQuotaEvent);
            return operatingQuotaEvent;
        }
        /// <summary>
        /// Insertar Evento a Aplicar Endoso
        /// </summary>
        /// <param name="operatingQuotaEvent"></param>
        /// <returns></returns>
        public OperatingQuotaEvent InsertOperatingQuotaEventEndorsement(OperatingQuotaEvent operatingQuotaEvent)
        {
            operatingQuotaEvent.Payload = JsonHelper.SerializeObjectToJson(operatingQuotaEvent.ApplyEndorsement);
            UPEN.OperatingQuotaEvent entityOperatingQuotaEvent = EntityAssembler.CreateOperatingQuotaEvent(operatingQuotaEvent);
            DataFacadeManager.Insert(entityOperatingQuotaEvent);
            operatingQuotaEvent = ModelAssembler.CreateOperatingQuotaEvent(entityOperatingQuotaEvent);
            return operatingQuotaEvent;
        }

        /// <summary>
        /// Cumulo Individual
        /// </summary>
        /// <param name="IndividualId"></param>
        /// <param name="ApplyEndorsment"></param>
        /// <returns></returns>
        public OperatingQuotaEvent GetCumulosByIndividualIdByEndorsement(int individualId, int applyEndorsment)
        {
            OperatingQuotaEvent operatingQuotaEvent = new OperatingQuotaEvent();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(UPEN.OperatingQuotaEvent.Properties.IdentificationId, typeof(UPEN.OperatingQuotaEvent).Name, individualId);
            filter.And();
            filter.PropertyEquals(UPEN.OperatingQuotaEvent.Properties.OperatingQuotaTypeEvent, typeof(UPEN.OperatingQuotaEvent).Name, applyEndorsment);
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(UPEN.OperatingQuotaEvent), filter.GetPredicate());
            List<OperatingQuotaEvent> operatingQuotaEvents = ModelAssembler.CreateOperatingQuotaEvents(businessObjects);
            if (operatingQuotaEvents.Count > 0)
            {
                operatingQuotaEvent = operatingQuotaEvents.Last();
                operatingQuotaEvent.ApplyEndorsement.AmountCoverage = operatingQuotaEvents.Sum(x => x.ApplyEndorsement.AmountCoverage);
            }
            return operatingQuotaEvent;

        }

        /// <summary>
        /// Consulta todas las transacciones por individualId
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="issuaceDate"></param>
        /// <param name="enums"></param>
        /// <param name="lineBusinessId"></param>
        /// <returns></returns>
        public List<OperatingQuotaEvent> GetOperationQuotaByIndividualIdByTransactionId(int individualId, DateTime? issuaceDate, int enums, int lineBusinessId)
        {
            OperatingQuotaEvent operatingQuotaEvent = new OperatingQuotaEvent();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            List<OperatingQuotaEvent> operatingQuotaEvents = new List<OperatingQuotaEvent>();
            BusinessCollection businessObjects = new BusinessCollection();
            filter.PropertyEquals(UPEN.OperatingQuotaEvent.Properties.IdentificationId, typeof(UPEN.OperatingQuotaEvent).Name, individualId);
            filter.And();
            filter.PropertyEquals(UPEN.OperatingQuotaEvent.Properties.LineBusinessCode, typeof(UPEN.OperatingQuotaEvent).Name, lineBusinessId);
            filter.And();
            filter.Property(UPEN.OperatingQuotaEvent.Properties.OperatingQuotaTypeEvent, typeof(UPEN.OperatingQuotaEvent).Name);
            filter.Distinct();
            filter.Constant((int)EnumEventOperationQuota.APPLY_REINSURANCE_ENDORSEMENT);
            switch (enums)
            {
                case 1:
                    filter.And();
                    filter.PropertyEquals(UPEN.OperatingQuotaEvent.Properties.OperatingQuotaTypeEvent, typeof(UPEN.OperatingQuotaEvent).Name, (int)EnumEventOperationQuota.ASSIGN_INDIVIDUAL_OPERATION_QUOTA);
                    break;
                case 2:
                    filter.And();
                    filter.Property(UPEN.OperatingQuotaEvent.Properties.OperatingQuotaTypeEvent, typeof(UPEN.OperatingQuotaEvent).Name);
                    filter.Distinct();
                    filter.Constant((int)EnumEventOperationQuota.ASSIGN_INDIVIDUAL_OPERATION_QUOTA);
                    break;
                default:
                    break;
            }
            filter.And();
            filter.OpenParenthesis();
            filter.OpenParenthesis();
            filter.Property(UPEN.OperatingQuotaEvent.Properties.PolicyEndDate);
            filter.LessEqual();
            filter.Constant(issuaceDate);
            filter.And();
            filter.Property(UPEN.OperatingQuotaEvent.Properties.PolicyInitDate);
            filter.GreaterEqual();
            filter.Constant(issuaceDate);
            filter.CloseParenthesis();
            filter.Or();
            filter.Property(UPEN.OperatingQuotaEvent.Properties.OperatingQuotaTypeEvent);
            filter.Distinct();
            filter.Constant((int)EnumEventOperationQuota.APPLY_REINSURANCE_ENDORSEMENT);
            filter.CloseParenthesis();

            businessObjects = DataFacadeManager.GetObjects(typeof(UPEN.OperatingQuotaEvent), filter.GetPredicate());
            operatingQuotaEvents = ModelAssembler.CreateOperatingQuotaEvents(businessObjects);
            operatingQuotaEvents.RemoveAll(x => x.Cov_End_Date < issuaceDate && x.Cov_End_Date != null);
            return operatingQuotaEvents;
        }

        public List<OperatingQuotaEvent> GetOperationQuotaByIndividualIdTransactionIdByIsConsortium(int individualId, DateTime? issuaceDate, int enums, int lineBusinessId)
        {
            OperatingQuotaEvent operatingQuotaEvent = new OperatingQuotaEvent();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            List<OperatingQuotaEvent> operatingQuotaEvents = new List<OperatingQuotaEvent>();
            BusinessCollection businessObjects = new BusinessCollection();
            filter.PropertyEquals(UPEN.OperatingQuotaEvent.Properties.IdentificationId, typeof(UPEN.OperatingQuotaEvent).Name, individualId);
            filter.And();
            filter.PropertyEquals(UPEN.OperatingQuotaEvent.Properties.LineBusinessCode, typeof(UPEN.OperatingQuotaEvent).Name, lineBusinessId);
            filter.And();
            filter.Property(UPEN.OperatingQuotaEvent.Properties.OperatingQuotaTypeEvent, typeof(UPEN.OperatingQuotaEvent).Name);
            filter.Distinct();
            filter.Constant((int)EnumEventOperationQuota.APPLY_REINSURANCE_ENDORSEMENT);
            switch (enums)
            {
                case 1:
                    filter.And();
                    filter.PropertyEquals(UPEN.OperatingQuotaEvent.Properties.OperatingQuotaTypeEvent, typeof(UPEN.OperatingQuotaEvent).Name, (int)EnumEventOperationQuota.ASSIGN_INDIVIDUAL_OPERATION_QUOTA);
                    break;
                case 2:
                    filter.And();
                    filter.Property(UPEN.OperatingQuotaEvent.Properties.OperatingQuotaTypeEvent, typeof(UPEN.OperatingQuotaEvent).Name);
                    filter.Distinct();
                    filter.Constant((int)EnumEventOperationQuota.ASSIGN_INDIVIDUAL_OPERATION_QUOTA);
                    break;
                default:
                    break;
            }
            filter.And();
            filter.OpenParenthesis();
            filter.OpenParenthesis();
            filter.Property(UPEN.OperatingQuotaEvent.Properties.PolicyEndDate);
            filter.LessEqual();
            filter.Constant(issuaceDate);
            filter.And();
            filter.Property(UPEN.OperatingQuotaEvent.Properties.PolicyInitDate);
            filter.GreaterEqual();
            filter.Constant(issuaceDate);
            filter.CloseParenthesis();
            filter.Or();
            filter.Property(UPEN.OperatingQuotaEvent.Properties.OperatingQuotaTypeEvent);
            filter.Distinct();
            filter.Constant((int)EnumEventOperationQuota.APPLY_REINSURANCE_ENDORSEMENT);
            filter.CloseParenthesis();

            businessObjects = DataFacadeManager.GetObjects(typeof(UPEN.OperatingQuotaEvent), filter.GetPredicate());
            operatingQuotaEvents = ModelAssembler.CreateOperatingQuotaEvents(businessObjects);
            operatingQuotaEvents.RemoveAll(x => x.ApplyEndorsement?.IsConsortium == true || x.Cov_End_Date < issuaceDate && x.Cov_End_Date != null);
            return operatingQuotaEvents;
        }

        /// <summary>
        /// Consulta el valor de la moneda por currencyId
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns></returns>
        public COMMENMOD.ExchangeRate GetExchangeRateByCurrencyId(int currencyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(COMMEN.ExchangeRate.Properties.CurrencyCode, typeof(COMMEN.ExchangeRate).Name, currencyId);
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(COMMEN.ExchangeRate), filter.GetPredicate());
            List<COMMENMOD.ExchangeRate> exchangeRate = new List<COMMENMOD.ExchangeRate>();
            exchangeRate = ModelAssembler.CreateExchangeRates(businessObjects);
            return exchangeRate.Last();
        }

        public List<OperatingQuotaEvent> InsertOperatingQuotaEventReinsurance(List<OperatingQuotaEvent> operatingQuotaEvents)
        {
            int operatingQuotaEventID = 0;
            List<OperatingQuotaEvent> operatingQuotaEventsReinsurance = new List<OperatingQuotaEvent>();
            List<UPEN.OperatingQuotaEvent> operatingQuotaEventEntities = new List<UPEN.OperatingQuotaEvent>();

            lock (obj)
            {
                operatingQuotaEventsReinsurance = operatingQuotaEvents;
                operatingQuotaEventEntities = operatingQuotaEventsReinsurance.Select(EntityAssembler.CreateUPENOperatingQuotaEventByOperatingQuotaEvent()).ToList();
                Parameter parameter = DelegateService.commonServiceCore.GetParameterByDescription("OPERATING_QUOTA_EVENT");
                operatingQuotaEventID = Convert.ToInt32(parameter.NumberParameter+1);
                parameter.NumberParameter += operatingQuotaEvents.Count + 1;
                DelegateService.commonServiceCore.UpdateParameter(parameter);
            }

            try
            {
                UTILITY.Parallel.ForEach(operatingQuotaEventEntities, x =>
                {
                    UPEN.OperatingQuotaEvent operatingQuotaEvent = null;
                    try
                    {
                        List<Task> agentTask = new List<Task>();
                        agentTask.Add(Task.Run(() =>
                        {
                            lock (obj)
                            {
                                operatingQuotaEvent = x;
                                operatingQuotaEventID += 1;
                                operatingQuotaEvent.OperatingQuotaEventCode = operatingQuotaEventID;
                                x.OperatingQuotaEventCode = operatingQuotaEventID;
                            }
                        }));
                        Task.WaitAll(agentTask.ToArray());
                        if (operatingQuotaEventID > 0)
                        {
                            DataFacadeManager.Insert(operatingQuotaEvent);
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLog.WriteEntry("Application", operatingQuotaEventID.ToString() + " - " + x.Payload + " - " + ex.Message, EventLogEntryType.Error);
                    }

                }, Utilities.Helper.ParallelHelper.DebugParallelFor());

                EventLog.WriteEntry("Application", "Se ha finalizado la inserción de datos para la migración de cúmulos de reaseguros ", EventLogEntryType.Information);

                return operatingQuotaEventEntities.Select(ModelAssembler.CreateOperatingQuotaEventByUPENOperatingQuotaEvent()).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public List<OperatingQuotaEvent> GetCumulusCoveragesByIndividual(FilterOperationQuota filterOperationQuota, List<int> idParticipants)
        {
            List<OperatingQuotaEvent> operatingQuotaEvents = new List<OperatingQuotaEvent>();
            OperatingQuotaEvent operatingQuotaEvent = new OperatingQuotaEvent();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            BusinessCollection businessObjects = new BusinessCollection();

            if (idParticipants.Count > 0)
            {
                filter.Property(UPEN.OperatingQuotaEvent.Properties.IdentificationId);
                filter.In();
                filter.ListValue();
                foreach (int i in idParticipants)
                {
                    filter.Constant(i);
                }
                filter.EndList();
            }
            else
            {
                filter.PropertyEquals(UPEN.OperatingQuotaEvent.Properties.IdentificationId, typeof(UPEN.OperatingQuotaEvent).Name, filterOperationQuota.IndividualId);
            }
            
            filter.And();
            filter.PropertyEquals(UPEN.OperatingQuotaEvent.Properties.OperatingQuotaTypeEvent, typeof(UPEN.OperatingQuotaEvent).Name, (int)EnumEventOperationQuota.APPLY_REINSURANCE_ENDORSEMENT);
            
            if (filterOperationQuota.IsFuture)
            {
                filter.And();
                
                filter.OpenParenthesis();
                filter.Property(UPEN.OperatingQuotaEvent.Properties.IssueDate);
                filter.LessEqual();
                filter.Constant(filterOperationQuota.DateCumulus);

                filter.CloseParenthesis();
            }
            else
            {
                filter.And();
                
                filter.OpenParenthesis();
                filter.Property(UPEN.OperatingQuotaEvent.Properties.CovInitDat);
                filter.LessEqual();
                filter.Constant(filterOperationQuota.DateCumulus);
                
                filter.And();
                
                filter.Property(UPEN.OperatingQuotaEvent.Properties.CovEndDa);
                filter.GreaterEqual();
                filter.Constant(filterOperationQuota.DateCumulus);
                filter.CloseParenthesis();
            }

            if(filterOperationQuota.PrefixCd > 0)
            {
                filter.And();
                filter.PropertyEquals(UPEN.OperatingQuotaEvent.Properties.PrefixCode, typeof(UPEN.OperatingQuotaEvent).Name, filterOperationQuota.PrefixCd);
            }

            if (filterOperationQuota.LineBusiness > 0)
            {
                filter.And();
                filter.PropertyEquals(UPEN.OperatingQuotaEvent.Properties.LineBusinessCode, typeof(UPEN.OperatingQuotaEvent).Name, filterOperationQuota.LineBusiness);

            }
            
            if (filterOperationQuota.SubLineBusiness > 0 )
            {
                filter.And();
                filter.PropertyEquals(UPEN.OperatingQuotaEvent.Properties.SubLineBusinessCode, typeof(UPEN.OperatingQuotaEvent).Name, filterOperationQuota.SubLineBusiness);
            }

            businessObjects = DataFacadeManager.GetObjects(typeof(UPEN.OperatingQuotaEvent), filter.GetPredicate());
            operatingQuotaEvents = ModelAssembler.CreateOperatingQuotaEvents(businessObjects);
            return operatingQuotaEvents;
        }

        /// <summary>
        /// Gets the decimal by product identifier.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="currencyId">The currency identifier.</param>
        /// <returns></returns>
        public int GetDecimalByProductId(int productId, Int16 currencyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.PropertyEquals(PRODEN.ProductCurrency.Properties.ProductId, typeof(PRODEN.ProductCurrency).Name, productId);
            filter.And();
            filter.PropertyEquals(PRODEN.ProductCurrency.Properties.CurrencyCode, typeof(PRODEN.ProductCurrency).Name, currencyId);
            PRODEN.ProductCurrency productCurrency = (PRODEN.ProductCurrency)DataFacadeManager.GetObjects(typeof(PRODEN.ProductCurrency), filter.GetPredicate()).FirstOrDefault();
            return productCurrency.DecimalQuantity;
        }

        /// <summary>
        /// Recupera la Informacion de la cupo por IndividualId  y EventType
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public OperatingQuotaEvent GetOperatingQuotaEventByIndividualId(int individualId)
        {
            OperatingQuotaEvent operatingQuotaEvent = new OperatingQuotaEvent();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(UPEN.OperatingQuotaEvent.Properties.IdentificationId, typeof(UPEN.OperatingQuotaEvent).Name, individualId);
            filter.And();
            filter.Property(UPEN.OperatingQuotaEvent.Properties.OperatingQuotaTypeEvent);
            filter.Distinct();
            filter.Constant((int)EnumEventOperationQuota.ASSIGN_INDIVIDUAL_OPERATION_QUOTA);
            filter.And();
            filter.Property(UPEN.OperatingQuotaEvent.Properties.OperatingQuotaTypeEvent);
            filter.Distinct();
            filter.Constant((int)EnumEventOperationQuota.APPLY_REINSURANCE_ENDORSEMENT);
            //filter.(UPEN.OperatingQuotaEvent.Properties.OperatingQuotaTypeEvent, typeof(UPEN.OperatingQuotaEvent).Name, EnumEventOperationQuota.ASSIGN_INDIVIDUAL_OPERATION_QUOTA);
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(UPEN.OperatingQuotaEvent), filter.GetPredicate());

            if (businessObjects.Count > 0)
            {
                List<OperatingQuotaEvent> operatingQuotaEvents = ModelAssembler.CreateOperatingQuotaEvents(businessObjects);
                return operatingQuotaEvents.Last();
            }
            return null;
        }

        /// <summary>
        /// Valida si el asegurado fue dado de baja
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public DeclineInsured GetDeclineDate(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(UPEN.Insured.Properties.IndividualId, typeof(UPEN.Insured).Name, individualId);
            filter.And();
            filter.Property(UPEN.Insured.Properties.DeclinedDate);
            filter.IsNotNull();

            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(UPEN.Insured), filter.GetPredicate());
            List<DeclineInsured> declineInsureds = new List<DeclineInsured>();
            DeclineInsured declineInsured = new DeclineInsured();

            if (businessObjects.Count > 0)
            {
                declineInsureds = ModelAssembler.CreateDeclineInsureds(businessObjects);
                if (declineInsureds.Count > 0)
                {
                    declineInsured = declineInsureds.First();
                    declineInsured.Decline = true;
                }
            }
            else
            {
                return null;
            }
            return declineInsured;
        }

        public List<ReinsuranceOperatingQuotaEvent> InsertReinsuranceOperatingQuotaEvent(List<ReinsuranceOperatingQuotaEvent> reinsuranceOperatingQuotaEvents)
        {
            List<UPEN.ReinsOperatingQuotaEvent> reinsuranceOperatingQuotaEventEntities = new List<UPEN.ReinsOperatingQuotaEvent>();

            lock (obj)
            {
                reinsuranceOperatingQuotaEventEntities = reinsuranceOperatingQuotaEvents.Select(EntityAssembler.CreateReinsOperatingQuotaEventByUPENReinsOperatingQuotaEvent()).ToList();
            }

            try
            {
                UTILITY.Parallel.ForEach(reinsuranceOperatingQuotaEventEntities, x =>
                {
                    UPEN.ReinsOperatingQuotaEvent reinsOperatingQuotaEvent = null;
                    try
                    {
                        List<Task> agentTask = new List<Task>();
                        agentTask.Add(Task.Run(() =>
                        {
                            lock (obj)
                            {
                                reinsOperatingQuotaEvent = x;
                            }
                        }));
                        Task.WaitAll(agentTask.ToArray());
                        
                        if (reinsOperatingQuotaEvent != null)
                        {
                            DataFacadeManager.Insert(reinsOperatingQuotaEvent);
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }

                }, Utilities.Helper.ParallelHelper.DebugParallelFor());

                return reinsuranceOperatingQuotaEvents;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

    }
}
