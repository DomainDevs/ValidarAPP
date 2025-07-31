using Newtonsoft.Json;
using Sistran.Co.Application.Data;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.DAOs;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Enums;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.Consortium;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using COMMENMOD = Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.Business
{
    public class ConsortiumBusiness
    {
        /// <summary>
        /// Consulta el cupo operativo de cada participante de un consorcio
        /// </summary>
        /// <param name="consortiumEvent"></param>
        /// <param name="enums"></param>
        /// <param name="linesBusiness"></param>
        /// <returns></returns>
        public OperatingQuotaEvent GetOperationQuotaIndividualParticeByConsortium(ConsortiumEvent consortiumEvent, int enums, int lineBusinessId)
        {
            OperatingQuotaEvent operatingQuotaEvent = new OperatingQuotaEvent();
            OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
            if (consortiumEvent.IndividualId != 0)
            {
                operatingQuotaEvent.ApplyEndorsement = new ApplyEndorsement();
                operatingQuotaEvent.IndividualOperatingQuota = new IndividualOperatingQuota();
                NameValue[] parameters = new NameValue[5];

                parameters[0] = new NameValue("@ISSUACE_DATE", DateTime.Now);
                parameters[1] = new NameValue("@INDIVIDUAL_ID", consortiumEvent.IndividualId);
                parameters[2] = new NameValue("@LINE_BUSINESS_ID", lineBusinessId);
                parameters[3] = new NameValue("@PARTICIPANT_CONSORTIUM", consortiumEvent.Consortiumpartners.ParticipationRate);

                switch (enums)
                {
                    case 1:
                        parameters[4] = new NameValue("@APPLY_REINSURANCE_ENDORSEMENT", (int)EnumEventOperationQuota.ASSIGN_INDIVIDUAL_OPERATION_QUOTA);
                        break;
                    case 2:
                        parameters[4] = new NameValue("@APPLY_REINSURANCE_ENDORSEMENT", (int)EnumEventOperationQuota.ASSIGN_INDIVIDUAL_OPERATION_QUOTA);
                        break;
                    default:
                        parameters[4] = new NameValue("@APPLY_REINSURANCE_ENDORSEMENT", (int)EnumEventOperationQuota.APPLY_REINSURANCE_ENDORSEMENT);
                        break;
                }


                DataTable result;
                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    result = pdb.ExecuteSPDataTable("ISS.GET_OPERATION_QUOTA_AND_CUMULO_CONSORTIUM", parameters);
                }
                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        operatingQuotaEvent.Cov_Init_Date = row.ItemArray[0].ToString() == String.Empty ? Convert.ToDateTime(JsonConvert.DeserializeObject(row.ItemArray[0].ToString())) : (DateTime)row.ItemArray[0];
                        operatingQuotaEvent.Cov_End_Date = row.ItemArray[1].ToString() == String.Empty ? Convert.ToDateTime(JsonConvert.DeserializeObject(row.ItemArray[1].ToString())) : (DateTime)row.ItemArray[1];
                        operatingQuotaEvent.LineBusinessID = (int)row.ItemArray[2];
                        operatingQuotaEvent.OperatingQuotaEventType = (int)row.ItemArray[3];
                        operatingQuotaEvent.IssueDate = row.ItemArray[4].ToString() == String.Empty ? Convert.ToDateTime(JsonConvert.DeserializeObject(row.ItemArray[4].ToString())) : (DateTime)row.ItemArray[4];
                        operatingQuotaEvent.IdentificationId = (int)row.ItemArray[5];
                        operatingQuotaEvent.IndividualOperatingQuota.IndividualID = (int)row.ItemArray[5];
                        operatingQuotaEvent.IndividualOperatingQuota.ValueOpQuotaAMT = (decimal)row.ItemArray[6];
                        operatingQuotaEvent.IndividualOperatingQuota.EndDateOpQuota = row.ItemArray[7].ToString() == String.Empty ? Convert.ToDateTime(JsonConvert.DeserializeObject(row.ItemArray[7].ToString())) : (DateTime)row.ItemArray[7];
                        operatingQuotaEvent.ApplyEndorsement.AmountCoverage = 0/*(decimal)row.ItemArray[8]*/;
                    }
                }
                else
                {
                    operatingQuotaEvent.IndividualOperatingQuota.ValueOpQuotaAMT = 0;
                    operatingQuotaEvent.ApplyEndorsement.AmountCoverage = 0;
                }
            }
            else
            {
                OperationQuotaBusiness operationQuotaBusiness = new OperationQuotaBusiness();
                operatingQuotaEvent = operationQuotaBusiness.GetOperationQuotaByIndividualIdByTransactionId(consortiumEvent.IndividualConsortiumID, enums, lineBusinessId);
            }

            return operatingQuotaEvent;
        }

        /// <summary>
        /// Consulta el cupo operativo de cada participante de un consorcio IsConsortium
        /// </summary>
        /// <param name="consortiumEvent"></param>
        /// <param name="enums"></param>
        /// <param name="linesBusiness"></param>
        /// <returns></returns>
        public OperatingQuotaEvent GetOperationQuotaIndividualParticeByConsortiumByIsConsortium(ConsortiumEvent consortiumEvent, int enums, int lineBusinessId)
        {
            List<OperatingQuotaEvent> operatingQuotaEvents = new List<OperatingQuotaEvent>();
            OperatingQuotaEvent operatingQuotaEvent = new OperatingQuotaEvent();
            operatingQuotaEvent.ApplyEndorsement = new ApplyEndorsement();
            operatingQuotaEvent.IndividualOperatingQuota = new IndividualOperatingQuota();
            OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
            if (consortiumEvent.IndividualId != 0)
            {
                operatingQuotaEvents = operationQuotaDAO.GetOperationQuotaByIndividualIdTransactionIdByIsConsortium(consortiumEvent.IndividualId, DateTime.Now, enums, lineBusinessId);
                foreach (OperatingQuotaEvent item in operatingQuotaEvents)
                {
                    if (item.ApplyEndorsement?.CurrencyType == (int)EnumExchangeRateCurrency.CURRENCY_PESOS)
                    {
                        COMMENMOD.ExchangeRate exchangeRate = operationQuotaDAO.GetExchangeRateByCurrencyId(item.ApplyEndorsement.CurrencyType);
                        item.ApplyEndorsement.AmountCoverage = item.ApplyEndorsement.AmountCoverage * exchangeRate.SellAmount;
                    }

                    if (item.OperatingQuotaEventType == (int)EnumEventOperationQuota.ASSIGN_INDIVIDUAL_OPERATION_QUOTA)
                    {
                        //se consulta el cupo 
                        operatingQuotaEvent.Cov_Init_Date = item.Cov_Init_Date;
                        operatingQuotaEvent.Cov_End_Date = item.Cov_End_Date;
                        operatingQuotaEvent.LineBusinessID = item.LineBusinessID;
                        operatingQuotaEvent.OperatingQuotaEventType = item.OperatingQuotaEventType;
                        operatingQuotaEvent.IssueDate = item.IssueDate;
                        operatingQuotaEvent.IdentificationId = item.IdentificationId;
                        operatingQuotaEvent.IndividualOperatingQuota.ValueOpQuotaAMT = item.IndividualOperatingQuota.ValueOpQuotaAMT * item.IndividualOperatingQuota.ParticipationPercentage / 100;
                        operatingQuotaEvent.IndividualOperatingQuota.ParticipationPercentage = item.IndividualOperatingQuota.ParticipationPercentage;
                        operatingQuotaEvent.IndividualOperatingQuota.InitDateOpQuota = item.IndividualOperatingQuota.InitDateOpQuota;
                        operatingQuotaEvent.IndividualOperatingQuota.EndDateOpQuota = item.IndividualOperatingQuota.EndDateOpQuota;
                        operatingQuotaEvent.IndividualOperatingQuota.IndividualID = item.IndividualOperatingQuota.IndividualID;
                    }
                    else
                    {
                        //suma el cumulo
                        if (item.IdentificationId != 0)
                        {
                            operatingQuotaEvent.ApplyEndorsement.AmountCoverage = operatingQuotaEvent.ApplyEndorsement.AmountCoverage + item.ApplyEndorsement.AmountCoverage;
                        }
                        else
                        {
                            operatingQuotaEvent.ApplyEndorsement.AmountCoverage = operatingQuotaEvent.ApplyEndorsement.AmountCoverage + item.ApplyEndorsement.AmountCoverage;
                        }
                    }
                }
            }
            return operatingQuotaEvent;
        }

        /// <summary>
        /// Consulta el Consorcio Cupos y Cumulos por ConsorcioId Y LineBusinessId
        /// </summary>
        /// <param name="consortiumId"></param>
        /// <param name="lineBusinessId"></param>
        /// <returns></returns>
        public OperatingQuotaEvent GetCumuloCupoConsortiumEventByConsortiumIdByLineBusinessId(int consortiumId, int lineBusinessId, bool? endorsement = false, int Id = 0)
        {
            OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
            ConsortiumDAO consortiumDAO = new ConsortiumDAO();
            OperatingQuotaEvent operatingQuotaEvent = new OperatingQuotaEvent();
            List<OperatingQuotaEvent> operatingQuotaEvents = new List<OperatingQuotaEvent>();

            int individualId = 1;
            List<ConsortiumEvent> consortiumEvents = new List<ConsortiumEvent>();
            /*Validacion endoso para consorcios: true recupera participantes de la poliza -false: estado actual*/
            if (endorsement == true)
            {

                List<RiskConsortium> riskConsortiums = new List<RiskConsortium>();
                riskConsortiums = consortiumDAO.GetRiskConsortiumbyPolicy(Id);
                foreach (RiskConsortium riskConsortium in riskConsortiums)
                {
                    ConsortiumEvent consortiumEvent = new ConsortiumEvent();
                    consortiumEvent.Consortiumpartners = new Consortiumpartners
                    {
                        ConsortiumId = riskConsortium.ConsortiumId,
                        IndividualPartnerId = riskConsortium.IndividualId,
                        ParticipationRate = riskConsortium.PjePart,
                        Enabled = true
                    };

                    consortiumEvent.IndividualConsortiumID = consortiumId;
                    consortiumEvent.IndividualId = riskConsortium.IndividualId;
                    consortiumEvents.Add(consortiumEvent);
                }
                //List<ConsortiumEvent> events = new List<ConsortiumEvent>();
                //DeclineInsured decline = operationQuotaDAO.GetDeclineDate(consortiumId);
                //if (decline != null)
                //{
                //    operatingQuotaEvent.declineInsured = decline;
                //}
                if (consortiumEvents.Count > 0)
                {
                    //CONSORCIO
                    consortiumEvents = consortiumEvents.OrderBy(x => x.IndividualId).ThenBy(x => x.IssueDate).ToList();

                    //foreach (ConsortiumEvent consortiumEvent in consortiumEvents)
                    //{
                    //    if (consortiumEvent.IndividualId != individualId && consortiumEvent.IndividualConsortiumID == consortiumId)
                    //    {
                    //        List<ConsortiumEvent> consortiumActive = consortiumDAO.GetParticipantConsortiumEventByIndividualIdByConsortiumId(consortiumEvent.IndividualId, consortiumId);

                    //        if (consortiumActive.Last().ConsortiumEventEventType != (int)EnumEventConsortium.DISABLED_INDIVIDUAL_TO_CONSORTIUM)
                    //        {
                    //            events.AddRange(consortiumActive);
                    //            individualId = consortiumEvent.IndividualId;
                    //        }
                    //    }
                    //}

                    foreach (ConsortiumEvent consortiumEvent in consortiumEvents)
                    {
                        if (consortiumEvent.IndividualId != 0 && consortiumEvent.Consortiumpartners.Enabled)
                        {
                            operatingQuotaEvent = GetOperationQuotaIndividualParticeByConsortium(consortiumEvent, (int)EnumMovement.TOTAL, lineBusinessId);
                            operatingQuotaEvent.consortiumEvent = consortiumEvent;
                            operatingQuotaEvent.consortiumEvent = new ConsortiumEvent();
                            operatingQuotaEvent.consortiumEvent.OperationQuotaConsortium = Convert.ToDecimal(operatingQuotaEvent.IndividualOperatingQuota.ValueOpQuotaAMT - operatingQuotaEvent.ApplyEndorsement.AmountCoverage) * (consortiumEvent.Consortiumpartners.ParticipationRate / 100);
                            operatingQuotaEvents.Add(operatingQuotaEvent);
                        }
                        else if (consortiumEvent.IndividualId == 0)
                        {
                            operatingQuotaEvent = GetOperationQuotaIndividualParticeByConsortium(consortiumEvent, (int)EnumMovement.TOTAL, lineBusinessId);
                            operatingQuotaEvent.consortiumEvent = consortiumEvent;
                            operatingQuotaEvents.Add(operatingQuotaEvent);
                        }
                    }

                    operatingQuotaEvent = operatingQuotaEvents.First();
                    operatingQuotaEvent.consortiumEvent.OperationQuotaConsortium = operatingQuotaEvents.Sum(x => x.consortiumEvent.OperationQuotaConsortium);
                    if (operatingQuotaEvents.First().IdentificationId == 0)
                    {
                        operatingQuotaEvent.consortiumEvent.OperationQuotaConsortium = operatingQuotaEvent.consortiumEvent.OperationQuotaConsortium - operatingQuotaEvent.ApplyEndorsement.AmountCoverage;
                        operatingQuotaEvents.RemoveAll(x => x.IdentificationId == 0);

                    }
                    operatingQuotaEvent.IndividualOperatingQuota.ValueOpQuotaAMT = Convert.ToDecimal(operatingQuotaEvents.Sum(x => x.IndividualOperatingQuota?.ValueOpQuotaAMT)) - Convert.ToDecimal(operatingQuotaEvents.Sum(x => x.ApplyEndorsement?.AmountCoverage)); ;

                }
                else
                {
                    ///PARTICIPANTE DEL CONSORCIO
                    OperatingQuotaEvent operatingQuotaEvent1 = new OperatingQuotaEvent();
                    consortiumEvents = consortiumDAO.GetParticipantConsortiumEventByIndividualId(consortiumId);

                    if (consortiumEvents.Count > 0)
                    {
                        List<ConsortiumEvent> consortiumEvents1 = consortiumDAO.GetConsortiumExistsByConsortiumId(consortiumEvents.First().IndividualConsortiumID);
                        consortiumEvents1.RemoveAll(x => x.IndividualId != (int)EnumInicialEvent.INICIAL_EVENT);
                        consortiumEvents1.AddRange(consortiumEvents);

                        if (consortiumEvents1.Count > 0)
                        {
                            foreach (ConsortiumEvent consortiumEvent in consortiumEvents1)
                            {
                                if (consortiumEvent.IndividualId != 0 && consortiumEvent.IndividualId == consortiumId)
                                {
                                    OperationQuotaBusiness operationQuotaBusiness = new OperationQuotaBusiness();
                                    operatingQuotaEvent = operationQuotaBusiness.GetOperationQuotaByIndividualIdByTransactionId(consortiumEvent.IndividualId, (int)EnumMovement.TOTAL, lineBusinessId);
                                    operatingQuotaEvent.consortiumEvent = consortiumEvents1.First();
                                    operatingQuotaEvent.consortiumEvent.OperationQuotaConsortium = Convert.ToDecimal(operatingQuotaEvent.IndividualOperatingQuota.ValueOpQuotaAMT * consortiumEvent.Consortiumpartners.ParticipationRate / 100);
                                    operatingQuotaEvent.consortiumEvent.Consortiumpartners = new Consortiumpartners();
                                    operatingQuotaEvent.consortiumEvent.Consortiumpartners.ParticipationRate = consortiumEvent.Consortiumpartners.ParticipationRate;
                                }
                            }
                        }
                    }
                }
                //if (decline != null)
                //{
                //    operatingQuotaEvent.declineInsured = decline;
                //}
            }
            else
            {
                consortiumEvents = consortiumDAO.GetConsortiumExistsByConsortiumIdMember(consortiumId);
                List<ConsortiumEvent> events = new List<ConsortiumEvent>();
                DeclineInsured decline = operationQuotaDAO.GetDeclineDate(consortiumId);
                if (decline != null)
                {
                    operatingQuotaEvent.declineInsured = decline;
                }
                if (consortiumEvents.Count > 0)
                {
                    //CONSORCIO
                    consortiumEvents = consortiumEvents.OrderBy(x => x.IndividualId).ThenBy(x => x.IssueDate).ToList();

                    foreach (ConsortiumEvent consortiumEvent in consortiumEvents)
                    {
                        if (consortiumEvent.IndividualId != individualId && consortiumEvent.IndividualConsortiumID == consortiumId)
                        {
                            List<ConsortiumEvent> consortiumActive = consortiumDAO.GetParticipantConsortiumEventByIndividualIdByConsortiumId(consortiumEvent.IndividualId, consortiumId);

                            if (consortiumActive.Last().ConsortiumEventEventType != (int)EnumEventConsortium.DISABLED_INDIVIDUAL_TO_CONSORTIUM)
                            {
                                events.AddRange(consortiumActive);
                                individualId = consortiumEvent.IndividualId;
                            }
                        }
                    }

                    foreach (ConsortiumEvent consortiumEvent in events)
                    {
                        if (consortiumEvent.IndividualId != 0 && consortiumEvent.Consortiumpartners.Enabled)
                        {
                            operatingQuotaEvent = GetOperationQuotaIndividualParticeByConsortium(consortiumEvent, (int)EnumMovement.TOTAL, lineBusinessId);
                            operatingQuotaEvent.consortiumEvent = consortiumEvent;
                            operatingQuotaEvent.consortiumEvent = new ConsortiumEvent();
                            operatingQuotaEvent.consortiumEvent.OperationQuotaConsortium = Convert.ToDecimal(operatingQuotaEvent.IndividualOperatingQuota.ValueOpQuotaAMT - operatingQuotaEvent.ApplyEndorsement.AmountCoverage) * (consortiumEvent.Consortiumpartners.ParticipationRate / 100);
                            operatingQuotaEvents.Add(operatingQuotaEvent);
                        }
                        else if (consortiumEvent.IndividualId == 0)
                        {
                            operatingQuotaEvent = GetOperationQuotaIndividualParticeByConsortium(consortiumEvent, (int)EnumMovement.TOTAL, lineBusinessId);
                            operatingQuotaEvent.consortiumEvent = consortiumEvent;
                            operatingQuotaEvents.Add(operatingQuotaEvent);
                        }
                    }

                    operatingQuotaEvent = operatingQuotaEvents.First();
                    operatingQuotaEvent.consortiumEvent.OperationQuotaConsortium = operatingQuotaEvents.Sum(x => x.consortiumEvent.OperationQuotaConsortium);
                    if (operatingQuotaEvents.First().IdentificationId == 0)
                    {
                        operatingQuotaEvent.consortiumEvent.OperationQuotaConsortium = operatingQuotaEvent.consortiumEvent.OperationQuotaConsortium - operatingQuotaEvent.ApplyEndorsement.AmountCoverage;
                        operatingQuotaEvents.RemoveAll(x => x.IdentificationId == 0);

                    }
                    operatingQuotaEvent.IndividualOperatingQuota.ValueOpQuotaAMT = Convert.ToDecimal(operatingQuotaEvents.Sum(x => x.IndividualOperatingQuota?.ValueOpQuotaAMT)) - Convert.ToDecimal(operatingQuotaEvents.Sum(x => x.ApplyEndorsement?.AmountCoverage)); ;

                }
                else
                {
                    ///PARTICIPANTE DEL CONSORCIO
                    OperatingQuotaEvent operatingQuotaEvent1 = new OperatingQuotaEvent();
                    consortiumEvents = consortiumDAO.GetParticipantConsortiumEventByIndividualId(consortiumId);

                    if (consortiumEvents.Count > 0)
                    {
                        List<ConsortiumEvent> consortiumEvents1 = consortiumDAO.GetConsortiumExistsByConsortiumId(consortiumEvents.First().IndividualConsortiumID);
                        consortiumEvents1.RemoveAll(x => x.IndividualId != (int)EnumInicialEvent.INICIAL_EVENT);
                        consortiumEvents1.AddRange(consortiumEvents);

                        if (consortiumEvents1.Count > 0)
                        {
                            foreach (ConsortiumEvent consortiumEvent in consortiumEvents1)
                            {
                                if (consortiumEvent.IndividualId != 0 && consortiumEvent.IndividualId == consortiumId)
                                {
                                    OperationQuotaBusiness operationQuotaBusiness = new OperationQuotaBusiness();
                                    operatingQuotaEvent = operationQuotaBusiness.GetOperationQuotaByIndividualIdByTransactionId(consortiumEvent.IndividualId, (int)EnumMovement.TOTAL, lineBusinessId);
                                    operatingQuotaEvent.consortiumEvent = consortiumEvents1.First();
                                    operatingQuotaEvent.consortiumEvent.OperationQuotaConsortium = Convert.ToDecimal(operatingQuotaEvent.IndividualOperatingQuota.ValueOpQuotaAMT * consortiumEvent.Consortiumpartners.ParticipationRate / 100);
                                    operatingQuotaEvent.consortiumEvent.Consortiumpartners = new Consortiumpartners();
                                    operatingQuotaEvent.consortiumEvent.Consortiumpartners.ParticipationRate = consortiumEvent.Consortiumpartners.ParticipationRate;
                                }
                            }
                        }
                    }
                }
                if (decline != null)
                {
                    operatingQuotaEvent.declineInsured = decline;
                }
            }


            return operatingQuotaEvent;
        }

        /// <summary>
        /// Consulta el Consorciado Cupos y Cumulos por ConsorcioId Y LineBusinessId
        /// </summary>
        /// <param name="consortiumId"></param>
        /// <param name="lineBusinessId"></param>
        /// <returns></returns>
        public List<OperatingQuotaEvent> GetCumulusQuotaConsortiumByIndividualIdByLineBusinessId(int individualId, int lineBusinessId)
        {
            ConsortiumDAO consortiumDAO = new ConsortiumDAO();
            List<OperatingQuotaEvent> operatingQuotaEvents = new List<OperatingQuotaEvent>();
            List<ConsortiumEvent> consortiumEvents = consortiumDAO.GetParticipantConsortiumEventByIndividualId(individualId);
            List<ConsortiumEvent> events = new List<ConsortiumEvent>();
            consortiumEvents = consortiumEvents.OrderBy(x => x.IndividualConsortiumID).ThenBy(x => x.IssueDate).ToList();
            int consortiumId = 1;

            foreach (ConsortiumEvent consortiumEvent in consortiumEvents)
            {
                if (consortiumEvent.IndividualConsortiumID != consortiumId && consortiumEvent.IndividualId == individualId)
                {
                    events.Add(consortiumEvent);
                    consortiumId = consortiumEvent.IndividualConsortiumID;
                }
            }


            if (events.Count > 0)
            {
                foreach (ConsortiumEvent item in events)
                {
                    //trae el estatus final del participante
                    List<ConsortiumEvent> consortiumActive = consortiumDAO.GetParticipantConsortiumEventByIndividualIdByConsortiumId(item.IndividualId, item.IndividualConsortiumID);
                    consortiumActive = consortiumActive.OrderBy(x => x.IndividualId).ThenBy(x => x.IssueDate).ToList();
                    if (consortiumActive.Last().ConsortiumEventEventType != (int)EnumEventConsortium.DISABLED_INDIVIDUAL_TO_CONSORTIUM)
                    {
                        OperatingQuotaEvent operatingQuotaEvent = new OperatingQuotaEvent();
                        operatingQuotaEvent.IndividualOperatingQuota = new IndividualOperatingQuota();
                        operatingQuotaEvent.ApplyEndorsement = new ApplyEndorsement();
                        List<ConsortiumEvent> consortiumEvents1 = consortiumDAO.GetConsortiumExistsByConsortiumId(item.IndividualConsortiumID);
                        if (consortiumEvents1.Count > 0)
                        {
                            int individual = 1;
                            List<ConsortiumEvent> events1 = new List<ConsortiumEvent>();
                            consortiumEvents1 = consortiumEvents1.OrderBy(x => x.IndividualId).ThenBy(x => x.IssueDate).ToList();

                            foreach (ConsortiumEvent consortiumEvent in consortiumEvents1)
                            {
                                if (consortiumEvent.IndividualId != individual)
                                {
                                    events1.Add(consortiumEvent);
                                    individual = consortiumEvent.IndividualId;
                                }
                            }

                            foreach (ConsortiumEvent consortiumEvent in events1)
                            {
                                if (consortiumEvent.IndividualId != 0)
                                {
                                    OperatingQuotaEvent operatingQuotaEventConsortium = GetOperationQuotaIndividualParticeByConsortium(consortiumEvent, (int)EnumMovement.TOTAL, lineBusinessId);
                                    operatingQuotaEvent.IndividualOperatingQuota.ValueOpQuotaAMT += operatingQuotaEventConsortium.IndividualOperatingQuota.ValueOpQuotaAMT - operatingQuotaEventConsortium.ApplyEndorsement.AmountCoverage;
                                }
                                else
                                {
                                    operatingQuotaEvent = GetOperationQuotaIndividualParticeByConsortium(consortiumEvent, (int)EnumMovement.TOTAL, lineBusinessId);
                                    operatingQuotaEvent.consortiumEvent = consortiumEvent;
                                }
                            }
                            operatingQuotaEvents.Add(operatingQuotaEvent);

                        }
                    }
                }
            }
            return operatingQuotaEvents;
        }

        public List<OperatingQuotaEvent> GetValidityParticipantCupoInConsortium(int consortiumId, long AmountInsured, int lineBusinessId)
        {
            List<OperatingQuotaEvent> operatingQuotaEvents = new List<OperatingQuotaEvent>();
            List<ConsortiumEvent> consortiumEvents = new List<ConsortiumEvent>();
            OperatingQuotaEvent operatingQuotaEvent = new OperatingQuotaEvent();
            ConsortiumDAO consortiumDAO = new ConsortiumDAO();
            OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
            consortiumEvents = consortiumDAO.GetConsortiumExistsByConsortiumIdMember(consortiumId);
            int individualId = 1;
            List<ConsortiumEvent> events = new List<ConsortiumEvent>();

            if (consortiumEvents.Count > 0)
            {
                consortiumEvents = consortiumEvents.OrderBy(x => x.IndividualId).ThenBy(x => x.IssueDate).ToList();

                foreach (ConsortiumEvent consortiumEvent in consortiumEvents)
                {
                    if (consortiumEvent.IndividualId != individualId && consortiumEvent.IndividualConsortiumID == consortiumId)
                    {
                        List<ConsortiumEvent> consortiumActive = consortiumDAO.GetParticipantConsortiumEventByIndividualIdByConsortiumId(consortiumEvent.IndividualId, consortiumId);

                        if (consortiumActive.Last().ConsortiumEventEventType != (int)EnumEventConsortium.DISABLED_INDIVIDUAL_TO_CONSORTIUM)
                        {
                            events.AddRange(consortiumActive);
                            individualId = consortiumEvent.IndividualId;
                        }
                    }
                }

                foreach (ConsortiumEvent consortiumEvent in events)
                {
                    if (consortiumEvent.IndividualId != 0)
                    {
                        OperationQuotaBusiness operationQuotaBusiness = new OperationQuotaBusiness();
                        operatingQuotaEvent = new OperatingQuotaEvent();
                        operatingQuotaEvent = operationQuotaBusiness.GetOperationQuotaByIndividualIdByTransactionId(consortiumEvent.IndividualId, (int)EnumMovement.TOTAL, lineBusinessId);
                        operatingQuotaEvent.consortiumEvent = consortiumEvent;
                        if (AmountInsured != 0)
                        {
                            decimal TotalAvaliableparticipation;
                            decimal AmountInsuredParticiparion;
                            TotalAvaliableparticipation = Convert.ToDecimal(operatingQuotaEvent.IndividualOperatingQuota.ValueOpQuotaAMT - operatingQuotaEvent.ApplyEndorsement.AmountCoverage);
                            AmountInsuredParticiparion = AmountInsured * consortiumEvent.Consortiumpartners.ParticipationRate / 100;
                            if (TotalAvaliableparticipation <= AmountInsuredParticiparion)
                            {
                                operatingQuotaEvent.consortiumEvent.IsConsortium = true;
                            }
                        }

                        operatingQuotaEvents.Add(operatingQuotaEvent);
                    }
                }
            }
            return operatingQuotaEvents;
        }
    }
}
