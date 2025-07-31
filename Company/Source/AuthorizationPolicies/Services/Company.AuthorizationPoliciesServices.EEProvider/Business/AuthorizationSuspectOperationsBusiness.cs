using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.AuthorizationPoliciesServices.EEProvider.Assemblers;
using Company.AuthorizationPoliciesServices.Models;
using Sistran.Co.Application.Data;
using Sistran.Company.Application.AuthorizationPoliciesServices.EEProvider.Assemblers;
using Sistran.Company.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using COMMEN = Sistran.Core.Application.Common.Entities;
using EVEN = Sistran.Core.Application.Events.Entities;

namespace Company.AuthorizationPoliciesServices.EEProvider.Business
{
    public class AuthorizationSuspectOperationsBusiness
    {
        public List<CompanySarlaftEventAuthorization> SearchSarlaftSuspectOperations(CompanySarlaftEventAuthorization companySarlaftEventAuthorization)
        {
            companySarlaftEventAuthorization.TypeId = 3;
            NameValue[] parameters = new NameValue[10];

            if (companySarlaftEventAuthorization.EventGroupId != null && companySarlaftEventAuthorization.EventGroupId != 0)
            {
                parameters[0] = new NameValue("GROUP_EVENT_ID", companySarlaftEventAuthorization.EventGroupId);
            }
            else
            {
                parameters[0] = new NameValue("GROUP_EVENT_ID", DBNull.Value, DbType.Int32);
            }
            if (companySarlaftEventAuthorization.TypeId != null && companySarlaftEventAuthorization.TypeId != 0)
            {
                parameters[1] = new NameValue("TYPE_CD", companySarlaftEventAuthorization.TypeId);
            }
            else
            {
                parameters[1] = new NameValue("TYPE_CD", DBNull.Value, DbType.Int32);
            }
            if (companySarlaftEventAuthorization.IsRejected != null)
            {
                parameters[2] = new NameValue("REJECT_IND", companySarlaftEventAuthorization.IsRejected);
            }
            else
            {
                parameters[2] = new NameValue("REJECT_IND", DBNull.Value, DbType.Boolean);
            }
            if (companySarlaftEventAuthorization.IsAuthorized != null)
            {
                parameters[3] = new NameValue("AUTHORIZED_IND", companySarlaftEventAuthorization.IsAuthorized);
            }
            else
            {
                parameters[3] = new NameValue("AUTHORIZED_IND", DBNull.Value, DbType.Boolean);
            }
            if (companySarlaftEventAuthorization.AuthoUserId != null && companySarlaftEventAuthorization.AuthoUserId != 0)
            {
                parameters[4] = new NameValue("AUTHO_USER_ID", companySarlaftEventAuthorization.AuthoUserId);
            }
            else
            {
                parameters[4] = new NameValue("AUTHO_USER_ID", DBNull.Value, DbType.Int32);
            }
            if (companySarlaftEventAuthorization.DocumentNumber != null && companySarlaftEventAuthorization.DocumentNumber != string.Empty)
            {
                parameters[5] = new NameValue("ID_CARD_NO", companySarlaftEventAuthorization.DocumentNumber);
            }
            else
            {
                parameters[5] = new NameValue("ID_CARD_NO", DBNull.Value, DbType.String);
            }
            if (companySarlaftEventAuthorization.FormNumber != null)
            {
                parameters[6] = new NameValue("FORM_NUM", companySarlaftEventAuthorization.FormNumber);
            }
            else
            {
                parameters[6] = new NameValue("FORM_NUM", DBNull.Value, DbType.String);
            }
            if (companySarlaftEventAuthorization.Year != null && companySarlaftEventAuthorization.Year != 0)
            {
                parameters[7] = new NameValue("YEAR", companySarlaftEventAuthorization.Year);
            }
            else
            {
                parameters[7] = new NameValue("YEAR", DBNull.Value, DbType.Int32);
            }
            if (companySarlaftEventAuthorization.IndividualId != null && companySarlaftEventAuthorization.IndividualId != 0)
            {
                parameters[8] = new NameValue("INDIVIDUAL_ID", companySarlaftEventAuthorization.IndividualId);
            }
            else
            {
                parameters[8] = new NameValue("INDIVIDUAL_ID", DBNull.Value, DbType.Int32);
            }
            if (companySarlaftEventAuthorization.UserId != null && companySarlaftEventAuthorization.UserId != 0)
            {
                parameters[9] = new NameValue("USER_ID", companySarlaftEventAuthorization.UserId);
            }
            else
            {
                parameters[9] = new NameValue("USER_ID", DBNull.Value, DbType.Int32);
            }
            DataTable result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("EVE.CO_SUSPECT_OPERATIONS", parameters);
            }
            if (result != null && result.Rows.Count > 0)
            {
                List<CompanySarlaftEventAuthorization> listSarlaftSuspectOperations = new List<CompanySarlaftEventAuthorization>();

                foreach (DataRow dataRow in result.Rows)
                {
                    CompanySarlaftEventAuthorization companySarlaftEventAuthorizationModel = new CompanySarlaftEventAuthorization();
                    companySarlaftEventAuthorizationModel.AuthorizationId = Convert.ToInt32(dataRow[0].ToString());
                    companySarlaftEventAuthorizationModel.EventGroupId = Convert.ToInt32(dataRow[1].ToString());
                    companySarlaftEventAuthorizationModel.EventId = Convert.ToInt32(dataRow[2].ToString());
                    companySarlaftEventAuthorizationModel.UserId = Convert.ToInt32(dataRow[5].ToString());
                    companySarlaftEventAuthorizationModel.AuthoUserId = Convert.ToInt32(dataRow[6].ToString());
                    companySarlaftEventAuthorizationModel.IsAuthorized = Convert.ToBoolean(dataRow[9].ToString());
                    companySarlaftEventAuthorizationModel.IsRejected = Convert.ToBoolean(dataRow[10].ToString());
                    if (!string.IsNullOrEmpty(dataRow[11].ToString()))
                    {
                        companySarlaftEventAuthorizationModel.RejectId = Convert.ToInt32(dataRow[11].ToString());
                    }

                    companySarlaftEventAuthorizationModel.EventDate = Convert.ToDateTime(dataRow[12].ToString());
                    companySarlaftEventAuthorizationModel.Detail = dataRow[14].ToString();
                    companySarlaftEventAuthorizationModel.RequestDetail = dataRow[15].ToString();
                    companySarlaftEventAuthorizationModel.Description = (string.IsNullOrEmpty(dataRow[16].ToString())) ? string.Empty : dataRow[16].ToString();
                    companySarlaftEventAuthorizationModel.Assets = dataRow[17].ToString();
                    if (!string.IsNullOrEmpty(dataRow[18].ToString()))
                    {
                        companySarlaftEventAuthorizationModel.AuthorizeReasonId = Convert.ToInt32(dataRow[18].ToString());
                    }
                    companySarlaftEventAuthorizationModel.Year = Convert.ToInt32(dataRow[19].ToString());
                    companySarlaftEventAuthorizationModel.FormNumber = dataRow[20].ToString();
                    companySarlaftEventAuthorizationModel.User = dataRow[21].ToString();
                    companySarlaftEventAuthorizationModel.DocumentType = dataRow[22].ToString();

                    listSarlaftSuspectOperations.Add(companySarlaftEventAuthorizationModel);
                }
                return GetAssets(listSarlaftSuspectOperations);
            }
            else
            {
                return new List<CompanySarlaftEventAuthorization>();
            }
        }

        public List<CompanySarlaftAuthorizationReason> GetAuthorizationReasons(int eventGroupId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.AuthorizationReason.Properties.GroupEventId, typeof(COMMEN.AuthorizationReason).Name);
            filter.Equal();
            filter.Constant(eventGroupId);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(COMMEN.AuthorizationReason), filter.GetPredicate());

            return ModelAssembler.CreateCompanyAuthorizationReasons(businessCollection);
        }

        public CompanySarlaftEventAuthorization AuthorizeSupectOperation(CompanySarlaftEventAuthorization companySarlaftEventAuthorization)
        {
            DataTable result;
            CompanySarlaftEventAuthorization companySarlaftEventAuthorizations = new CompanySarlaftEventAuthorization();
            NameValue[] parameters = new NameValue[7];

            parameters[0] = new NameValue("@AUTHORIZATION_ID", companySarlaftEventAuthorization.AuthorizationId);
            parameters[1] = new NameValue("@AUTHORIZE_IND", companySarlaftEventAuthorization.IsAuthorized);
            parameters[2] = new NameValue("@REJECT_IND", companySarlaftEventAuthorization.IsRejected);
            if (companySarlaftEventAuthorization.AuthorizeReasonId != null)
            {
                parameters[3] = new NameValue("@AUTHORIZATION_REASON_CD", companySarlaftEventAuthorization.AuthorizeReasonId);
            }
            else
            {
                parameters[3] = new NameValue("@AUTHORIZATION_REASON_CD", DBNull.Value, DbType.Int32);
            }


            if (companySarlaftEventAuthorization.RejectId != null)
            {
                parameters[4] = new NameValue("@REJECT_ID", companySarlaftEventAuthorization.RejectId);
            }
            else
            {
                parameters[4] = new NameValue("@REJECT_ID", DBNull.Value, DbType.Int32);
            }
            parameters[5] = new NameValue("@AUTHORIZATION_DESCRIPTION", companySarlaftEventAuthorization.Description);
            parameters[6] = new NameValue("@ENTITY_DESCRIPTION_VALUE", companySarlaftEventAuthorization.Assets);

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("EVE.CO_UPDATE_AUTHORIZATION_SUSPECT_OPERATIONS", parameters);
            }

            foreach (DataRow item in result.Rows)
            {
                companySarlaftEventAuthorizations.AuthorizationId = Convert.ToInt32(item[0].ToString());
            }

            return companySarlaftEventAuthorizations;
        }

        public List<CompanySarlaftEventAuthorization> GetAssets(List<CompanySarlaftEventAuthorization> companySarlaftEventAuthorizations)
        {
            List<CompanySarlaftEventAuthorization> listCompanySarlaftEventAuthorizations = new List<CompanySarlaftEventAuthorization>();
            foreach (CompanySarlaftEventAuthorization companySarlaftEventAuthorization in companySarlaftEventAuthorizations)
            {
                if (companySarlaftEventAuthorization.Assets != null)
                {
                    string asset = companySarlaftEventAuthorization.Assets;
                    string[] assetArray = new string[1];

                    assetArray = asset.Split('|');

                    foreach (string assetNumber in assetArray)
                    {
                        if (assetNumber.IndexOf("Activos") != -1)
                        {
                            companySarlaftEventAuthorization.Assets = assetNumber.Substring(9);
                        }
                    }
                }

                listCompanySarlaftEventAuthorizations.Add(companySarlaftEventAuthorization);
            }
            return listCompanySarlaftEventAuthorizations;
        }
    }

}
