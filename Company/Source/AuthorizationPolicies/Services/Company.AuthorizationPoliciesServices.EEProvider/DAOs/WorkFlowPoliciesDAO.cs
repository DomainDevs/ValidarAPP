using Sistran.Co.Application.Data;
using Sistran.Company.AuthorizationPoliciesServices.Models;
using Sistran.Core.Framework.DAF.Engine;
using System;
using System.Collections.Generic;
using System.Data;

namespace Sistran.Company.Application.AuthorizationPoliciesServices.EEProvider.DAOs
{
    public class WorkFlowPoliciesDAO
    {
        public List<CompanyEventAuthorization> GetEventAuthorizationsByUserId(int? userId, int eventGroupId, DateTime startDate, DateTime finishDate, int groupId)
        {
            List<CompanyEventAuthorization> companyEventAuthorizations = new List<CompanyEventAuthorization>();
            IDynamicPropertiesSerializer dynamicPropertiesSerializer = new DynamicPropertiesSerializer();
            NameValue[] parameters = new NameValue[5];

            if (userId > 0)
            {
                parameters[0] = new NameValue("@USER_ID", userId);
            }
            else
            {
                parameters[0] = new NameValue("@USER_ID", -1);
            }

            parameters[1] = new NameValue("@EVENT_DATE_FROM", startDate);
            parameters[2] = new NameValue("@EVENT_DATE_TO", finishDate);

            if (eventGroupId > 0)
            {
                parameters[3] = new NameValue("@GROUP_EVENT_ID", eventGroupId);
            }
            else
            {
                parameters[3] = new NameValue("@GROUP_EVENT_ID", DBNull.Value, DbType.Int32);
            }

            parameters[4] = new NameValue("@GROUP_CD", groupId);

            DataTable result = null;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataTable("EVE.CO_GET_PENDING_EVENT_WORKFLOW", parameters);
            }

            foreach (DataRow row in result.Rows)
            {
                companyEventAuthorizations.Add(
                new CompanyEventAuthorization
                {
                    AuthorizationId = Convert.ToInt32(row[0]),
                    GroupEvendId = Convert.ToInt32(row[1]),
                    EventId = Convert.ToInt32(row[2]),
                    AccessId = Convert.ToInt32(row[3]),
                    HierarchyCode = Convert.ToInt32(row[4]),
                    User = new CompanyWorkFlowUser
                    {
                        Id = Convert.ToInt32(row[5])
                    },
                    AuthoUserId = Convert.ToInt32(row[6]),
                    Operation1Id = Convert.ToString(row[7]),
                    Endorsement = new CompanyWorkFlowEndorsement
                    {
                        Id = Convert.ToInt32(row[8]),
                        DocumentNumber = Convert.ToInt32(row[23])
                    },
                    AuthorizedInd = Convert.ToBoolean(row[9]),
                    RejectInd = Convert.ToBoolean(row[10]),
                    RejectId = row[11] == DBNull.Value ? 0 : Convert.ToInt32(row[11]),
                    EventDate = Convert.ToDateTime(row[12]),
                    AuthorizationDate = row[13] == DBNull.Value ? Convert.ToDateTime(null) : Convert.ToDateTime(row[13]),
                    Description = row[14] == DBNull.Value ? null : Convert.ToString(row[14]),
                    AuthorizationDescription = row[15] == DBNull.Value ? null : Convert.ToString(row[15]),
                    EntityDescriptionValues = row[16] == DBNull.Value ? null : Convert.ToString(row[16]),
                    AuthorizationReasonCode = row[17] == DBNull.Value ? null : Convert.ToString(row[17]),
                    Policy = new CompanyWorkFlowPolicy
                    {
                        Id = Convert.ToInt32(row[18]),
                        DocumentNumber = Convert.ToInt32(row[22]),
                        Branch = new CompanyWorkFlowBranch
                        {
                            Id = Convert.ToInt32(row[20])
                        },
                        Prefix = new CompanyWorkFlowPrefix
                        {
                            Id = Convert.ToInt32(row[21])
                        }
                    }
                });
            }

            return companyEventAuthorizations;
        }
        #region new Event_Autho_WorkFlo
        /// <summary>
        /// Persiste la informacion de EVE.CO_EVENT_AUTHORIZATION, de cada movimiento 
        /// </summary>
        /// <param name="EventAuthorization"></param>
        /// <returns></returns>
        public CompanyEventAuthorization CreateBaseEventAuthorization(CompanyEventAuthorization EventAuthorization)
        {
            try
            {
                NameValue[] parametersp = new NameValue[7];
                DataTable dt;

                parametersp[0] = new NameValue("@WORK_FLOW_ID", Convert.ToInt32(EventAuthorization.Endorsement.Id));
                parametersp[1] = new NameValue("@USER_ID", EventAuthorization.AuthoUserId);
                parametersp[2] = new NameValue("@MODULE_CD", EventAuthorization.EventWorkFlowModule);
                parametersp[3] = new NameValue("@SUBMODULE_CD", EventAuthorization.EventWorkFlowSubModule);
                parametersp[4] = new NameValue("@GROUP_EVENT_ID", EventAuthorization.GroupEvendId);
                parametersp[5] = new NameValue("@EVENT_ID", EventAuthorization.EventId);
                parametersp[6] = new NameValue("@AUTHORIZATION_DESCRIPTION", EventAuthorization.Description);

                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    dt = pdb.ExecuteSPDataTable("EVE.CO_VALIDATE_EVENT_AUTHORIZATION", parametersp);
                }

                if (dt?.Rows.Count > 0 && dt.Rows[0] != null && dt.Rows[0][0] != null)
                {
                    if (Convert.ToInt32(dt.Rows[0][0]) == 0 || Convert.ToInt32(dt.Rows[0][0]) == -1)
                    {
                        EventAuthorization.DescriptionError = dt.Rows[0][1].ToString();
                        EventAuthorization.AuthorizedInd = false;
                        return EventAuthorization;
                    }
                    else
                    {
                        EventAuthorization.AuthorizedInd = true;
                        EventAuthorization.AuthorizationId = Convert.ToInt32(dt.Rows[0][0]);
                        return EventAuthorization;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Core.Framework.BAF.BusinessException(ex.Message);
            }
            return EventAuthorization;
        }
        #endregion
    }
}
