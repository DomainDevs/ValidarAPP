using Sistran.Co.Application.Data;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.DAOs;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Enums;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.Consortium;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.EconomicGroup;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using COMMENMOD = Sistran.Core.Application.CommonService.Models;
using Newtonsoft.Json;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.Business
{
    public class OperationQuotaBusiness
    {
        /// <summary>
        ///  Cupo y Cumulo Individual
        /// </summary>
        /// <param name="IndividualId"></param>
        /// <param name="enums"></param>
        /// <returns></returns>
        public OperatingQuotaEvent GetOperationQuotaByIndividualIdByTransactionId(int IndividualId, int enums, int LineBusinessId)
        {
            OperatingQuotaEvent operatingQuotaEvent = new OperatingQuotaEvent();
            operatingQuotaEvent.ApplyEndorsement = new ApplyEndorsement();
            operatingQuotaEvent.IndividualOperatingQuota = new IndividualOperatingQuota();
            NameValue[] parameters = new NameValue[4];

            parameters[0] = new NameValue("@ISSUACE_DATE", DateTime.Now);
            parameters[1] = new NameValue("@INDIVIDUAL_ID", IndividualId);
            parameters[2] = new NameValue("@LINE_BUSINESS_ID", LineBusinessId);
            switch (enums)
            {
                case 1:
                    parameters[3] = new NameValue("@APPLY_REINSURANCE_ENDORSEMENT", (int)EnumEventOperationQuota.ASSIGN_INDIVIDUAL_OPERATION_QUOTA);
                    break;
                case 2:
                    parameters[3] = new NameValue("@APPLY_REINSURANCE_ENDORSEMENT", (int)EnumEventOperationQuota.ASSIGN_INDIVIDUAL_OPERATION_QUOTA);
                    break;
                default:
                    parameters[3] = new NameValue("@APPLY_REINSURANCE_ENDORSEMENT", (int)EnumEventOperationQuota.APPLY_REINSURANCE_ENDORSEMENT);
                    break;
            }


            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataTable("ISS.GET_OPERATION_QUOTA_AND_CUMULO", parameters);
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
                    operatingQuotaEvent.ApplyEndorsement.AmountCoverage = (decimal)row.ItemArray[8];
                }
            }
            else
            {
                operatingQuotaEvent.IndividualOperatingQuota.ValueOpQuotaAMT = 0;
                operatingQuotaEvent.ApplyEndorsement.AmountCoverage = 0;
            }

            return operatingQuotaEvent;
        }

        public static int GetDecimalByProductIdCurrencyId(int productId, short currencyId)
        {
            OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
            return operationQuotaDAO.GetDecimalByProductId(productId, currencyId);
        }

        /// <summary>
        /// Valida si se encuentra el afianzado con cupo
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public TypeSecure GetSecureType(int individualId, int lineBusinessId)
        {
            TypeSecure typeSecure = new TypeSecure();

            //GrupoEconomico
            EconomicGroupDAO economicGroupDAO = new EconomicGroupDAO();
            EconomicGroupEvent economicGroupEvent = economicGroupDAO.GetExistingParticipantGroupEconomicEventByIndividualId(individualId);
            if (economicGroupEvent.IndividualId == individualId && economicGroupEvent.EconomicGroupEventType==(int)EnumEventEconomicGroup.ENABLED_INDIVIDUAL_TO_ECONOMIC_GROUP)
            {
                typeSecure.IsEconomicGroup = true;
            }
            else
            {
                typeSecure.IsEconomicGroup = false;
            }
            //Consorcio

            ConsortiumDAO consortiumDAO = new ConsortiumDAO();
            List<ConsortiumEvent> events = new List<ConsortiumEvent>();
            List<ConsortiumEvent> consortiumEvents = consortiumDAO.GetConsortiumExistsByConsortiumIdMember(individualId);
            if (consortiumEvents.Count > 0)
            {
                typeSecure.IsConsortium = true;
            }
            else
            {
                consortiumEvents = consortiumDAO.GetParticipantConsortiumEventByIndividualId(individualId);

                if (consortiumEvents.Count > 0)
                {
                    int individualId1 = 1;
                    int individualConsortiumId = 1;
                    consortiumEvents = consortiumEvents.OrderBy(x => x.IndividualId).ThenBy(x => x.IssueDate).ToList();
                    foreach (ConsortiumEvent consortiumEvent in consortiumEvents)
                    {
                        if (consortiumEvent.IndividualId != individualId1 && consortiumEvent.IndividualConsortiumID != individualConsortiumId)
                        {
                            if (consortiumEvent.ConsortiumEventEventType != (int)EnumEventConsortium.DISABLED_INDIVIDUAL_TO_CONSORTIUM)
                            {
                                events.Add(consortiumEvent);
                            }
                            individualId = consortiumEvent.IndividualId;
                            individualConsortiumId = consortiumEvent.IndividualConsortiumID;
                        }
                    }
                }
                if (events.Count > 0)
                {
                    typeSecure.IsConsortium = true;
                }

                //Individual
                OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
                List<OperatingQuotaEvent> operatingQuotaEvents = operationQuotaDAO.GetOperationQuotaByIndividualIdByTransactionId(individualId, DateTime.Now, (int)EnumMovement.TOTAL, lineBusinessId);
                if (operatingQuotaEvents.Any(x => x.IdentificationId != 0 && x.OperatingQuotaEventType == (int)EnumEventOperationQuota.ASSIGN_INDIVIDUAL_OPERATION_QUOTA))
                {
                    typeSecure.IsIndividual = true;
                }
                else
                {
                    typeSecure.IsNotIndividual = true;
                }

            }
            return typeSecure;
        }


        /// <summary>
        /// Valida si se encuentra el afianzado con cupo
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public bool GetOperatingQuotaEventByIndividualId(int individualId)
        {
            OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
            OperatingQuotaEvent operatingQuotaEvent = operationQuotaDAO.GetOperatingQuotaEventByIndividualId(individualId);
            if (operatingQuotaEvent.IdentificationId != 0)
            {
                return true;
            }
            return false;
        }
    }
}
