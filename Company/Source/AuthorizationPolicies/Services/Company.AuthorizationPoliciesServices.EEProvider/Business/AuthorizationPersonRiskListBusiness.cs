using Company.AuthorizationPoliciesServices.EEProvider.Entities.Views;
using Sistran.Company.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using EVEN = Sistran.Core.Application.Events.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Framework.DAF;
using Sistran.Company.Application.AuthorizationPoliciesServices.EEProvider.Assemblers;
using Company.AuthorizationPoliciesServices.Models;
using Sistran.Co.Application.Data;
using System.Data;

namespace Company.AuthorizationPoliciesServices.EEProvider.Business
{
    public class AuthorizationPersonRiskListBusiness
    {
        public CompanyAuthorizationRiskList AuthorizeRiskListOperation(CompanyAuthorizationRiskList companyAuthorizationRiskList)
        {
            DataTable result;
            CompanyAuthorizationRiskList companySarlaftRiskList = new CompanyAuthorizationRiskList();
            NameValue[] parameters = new NameValue[6];

            parameters[0] = new NameValue("@AUTHORIZATION_ID", companyAuthorizationRiskList.AuthorizationId);
            parameters[1] = new NameValue("@AUTHORIZE_IND", companyAuthorizationRiskList.IsAuthorized);
            parameters[2] = new NameValue("@REJECT_IND", companyAuthorizationRiskList.IsRejected);
            if (companyAuthorizationRiskList.AuthorizeReasonId != null)
            {
                parameters[3] = new NameValue("@AUTHORIZATION_REASON_CD", companyAuthorizationRiskList.AuthorizeReasonId);
            }
            else
            {
                parameters[3] = new NameValue("@AUTHORIZATION_REASON_CD", DBNull.Value, DbType.Int32);
            }


            if (companyAuthorizationRiskList.RejectId != null)
            {
                parameters[4] = new NameValue("@REJECT_ID", companyAuthorizationRiskList.RejectId);
            }
            else
            {
                parameters[4] = new NameValue("@REJECT_ID", DBNull.Value, DbType.Int32);
            }
            parameters[5] = new NameValue("@AUTHORIZATION_DESCRIPTION", companyAuthorizationRiskList.Description);

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("EVE.CO_UPDATE_AUTHORIZATION_RISK_LISTS", parameters);
            }

            foreach (DataRow item in result.Rows)
            {
                companySarlaftRiskList.AuthorizationId = Convert.ToInt32(item[0].ToString());
            }

            return companySarlaftRiskList;
        }
        public List<CompanyEventGroup> GetSarlaftEventGroupByEventGroupId(int eventGroupId)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EVEN.CoEventGroup.Properties.GroupEventId, typeof(EVEN.CoEventGroup).Name);
            filter.Equal();
            filter.Constant(eventGroupId);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(EVEN.CoEventGroup), filter.GetPredicate());

            return ModelAssembler.CreateCompanyEventGroups(businessCollection);
        }

        public List<CompanyAuthorizationRiskList> SearchSarlaftRiskList(CompanyAuthorizationRiskList companyAuthorizationRiskList, int userId)
        {
            DataTable result;
            NameValue[] parameter = new NameValue[5];

            parameter[0] = new NameValue("@EVENT_GROUP_ID", companyAuthorizationRiskList.EventGroupId);
            parameter[1] = new NameValue("@DOCUMENT_NUMBER", companyAuthorizationRiskList.DocumentNumber);
            parameter[2] = new NameValue("@DOCUMENT_TYPE", companyAuthorizationRiskList.DocumentType);
            parameter[4] = new NameValue("@USER_ID", userId);

            if (companyAuthorizationRiskList.EventDate != null)
            {
                parameter[3] = new NameValue("@EVENT_DATE", companyAuthorizationRiskList.EventDate);
            }
            else
            {
                parameter[3] = new NameValue("@EVENT_DATE", DBNull.Value, DbType.DateTime);
            }

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("EVE.CO_SARLAFT_RISK_LIST", parameter);
            }

            List<CompanyAuthorizationRiskList> listcompanyAuthorizationRiskLists = new List<CompanyAuthorizationRiskList>();
            foreach (DataRow item in result.Rows)
            {
                CompanyAuthorizationRiskList companyAuthorizationRiskLists = new CompanyAuthorizationRiskList();
                companyAuthorizationRiskLists.AuthorizationId = Convert.ToInt32(item[0].ToString());
                companyAuthorizationRiskLists.EventGroupId = Convert.ToInt32(item[1].ToString());
                companyAuthorizationRiskLists.DocumentNumber = item[3].ToString();
                companyAuthorizationRiskLists.DocumentType = item[4].ToString();
                companyAuthorizationRiskLists.IsAuthorized = Convert.ToBoolean(item[5].ToString());
                companyAuthorizationRiskLists.IsRejected = Convert.ToBoolean(item[6].ToString());
                ;
                if (!string.IsNullOrEmpty(item[7].ToString()))
                {
                    companyAuthorizationRiskLists.RejectId = Convert.ToInt32(item[7].ToString());

                }
                companyAuthorizationRiskLists.EventDate = Convert.ToDateTime(item[8]);
                companyAuthorizationRiskLists.Detail = item[9].ToString();
                companyAuthorizationRiskLists.RequestDetail = item[10].ToString();
                companyAuthorizationRiskLists.Description = item[11].ToString();
                if (!string.IsNullOrEmpty(item[12].ToString()))
                {
                    companyAuthorizationRiskLists.AuthorizeReasonId = Convert.ToInt32(item[12].ToString());

                }


                listcompanyAuthorizationRiskLists.Add(companyAuthorizationRiskLists);
            }

            return GetDocumentType(listcompanyAuthorizationRiskLists);
        }

        public List<CompanyAuthorizationRiskList> GetDocumentType(List<CompanyAuthorizationRiskList> companyAuthorizationRiskLists)
        {
            List<CompanyAuthorizationRiskList> listCompanyAuthorizationRiskLists = new List<CompanyAuthorizationRiskList>();

            foreach (CompanyAuthorizationRiskList companySarlaftEventAuthorization in companyAuthorizationRiskLists)
            {
                if (companySarlaftEventAuthorization.DocumentType != null)
                {
                    string TypeDocument = companySarlaftEventAuthorization.DocumentType;
                    string[] array = new string[1];

                    array = TypeDocument.Split('|');

                    foreach (string resultDocumentType in array)
                    {
                        switch (resultDocumentType)
                        {
                            case "1":
                                companySarlaftEventAuthorization.DocumentType = "CEDULA DE CIUDADANIA";
                                break;
                            case "2":
                                companySarlaftEventAuthorization.DocumentType = "CEDULA DE EXTRANJERIA";
                                break;
                            case "3":
                                companySarlaftEventAuthorization.DocumentType = "NIT";
                                break;
                            case "4":
                                companySarlaftEventAuthorization.DocumentType = "No. UNICO IDENTIFICACION TRIBU";
                                break;
                            case "5":
                                companySarlaftEventAuthorization.DocumentType = "REGISTRO CIVIL";
                                break;
                            case "6":
                                companySarlaftEventAuthorization.DocumentType = "TARJETA DE IDENTIDAD";
                                break;
                        }
                        break;
                    }
                }

                listCompanyAuthorizationRiskLists.Add(companySarlaftEventAuthorization);
            }

            return listCompanyAuthorizationRiskLists;
        }
    }
}
