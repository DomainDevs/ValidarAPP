using Sistran.Co.Application.Data;
using Sistran.Company.Application.UniquePersonListRiskBusinessService.Model;
using Sistran.Company.Application.UniquePersonListRiskBusinessServiceProvider.Assembler;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using UPEN = Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Framework.Queries;

using Sistran.Core.Framework.DAF.Engine;
using Sistran.Company.Application.UniquePersonListRiskBusinessServiceProvider.Entities.Views;
using Newtonsoft.Json;

namespace Sistran.Company.Application.UniquePersonListRiskBusinessServiceProvider.Business
{
    public class ListRiskBusiness
    {
        public List<IdentityCardTypes> GetIdentityCardTypes()
        {
            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(IdentityCardType));
            return ModelAssembler.CreateIdentityCardTypes(businessCollection);

        }

        public List<RiskListModel> GetListRisk()
        {
            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(RiskList));
            return ModelAssembler.CreateRiskList(businessCollection);

        }

        public List<CompanyListRiskPerson> GetListRiskPersonList(int documentNumber, string name, string surname, string nickName, int listRiskId)
        {
            List<CompanyListRiskPerson> companyListRisks = new List<CompanyListRiskPerson>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (documentNumber > 0)
            {
                filter.Property(UPEN.RiskMaintenance.Properties.IdCardNo, typeof(UPEN.RiskMaintenance).Name);
                filter.Equal();
                filter.Constant(documentNumber.ToString());
                filter.And();
            }
            if (!string.IsNullOrEmpty(name))
            {
                filter.Property(UPEN.RiskMaintenance.Properties.PersonName, typeof(UPEN.RiskMaintenance).Name);
                filter.Equal();
                filter.Constant(name);
                filter.And();
            }

            if (!string.IsNullOrEmpty(surname))
            {
                filter.Property(UPEN.RiskMaintenance.Properties.Surname, typeof(UPEN.RiskMaintenance).Name);
                filter.Equal();
                filter.Constant(surname);
                filter.And();
            }

            if (!string.IsNullOrEmpty(nickName))
            {
                filter.Property(UPEN.RiskMaintenance.Properties.Nickname, typeof(UPEN.RiskMaintenance).Name);
                filter.Equal();
                filter.Constant(nickName);
                filter.And();
            }
            filter.Property(UPEN.RiskList.Properties.RiskListCode, typeof(UPEN.RiskList).Name);
            filter.Equal();
            filter.Constant(listRiskId);

            ListRiskPersonView view = new ListRiskPersonView();
            ViewBuilder builder = new ViewBuilder("ListRiskPersonView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade();
            if (view.RiskAssignedLists.Count > 0)
            {
                List<UPEN.RiskMaintenance> riskMaintenances = view.RiskMaintenances.Cast<UPEN.RiskMaintenance>().ToList();
                List<UPEN.RiskAssignedList> riskAssignedList = view.RiskAssignedLists.Cast<UPEN.RiskAssignedList>().ToList();
                List<UPEN.RiskList> riskLists = view.RiskLists.Cast<UPEN.RiskList>().ToList();

                var companyLiskRiskResult = (from rm in riskMaintenances
                                             join ra in riskAssignedList
                                             on rm.IdCardNo equals ra.IdCardNo
                                             join rl in riskLists
                                             on ra.RiskListCode equals rl.RiskListCode
                                             select new CompanyListRiskPerson
                                             {
                                                 IdCardNo = rm.IdCardNo,
                                                 DocumentType = rm.IdCardTypeCode,
                                                 Name = rm.PersonName,
                                                 LastName = rm.Surname,
                                                 ListRisk = ra.RiskListCode,
                                                 CreateListUserId = ra.CreatedListUserId,
                                                 UpdateListUserId = ra.UpdatedListUserId,
                                                 ExludedPerson = ra.ExcludedPerson,
                                                 AssignmentDate = (DateTime)ra.AssignmentDate,
                                                 LastChangeDate = (ra.LastChangeDate != null) ? (DateTime)ra.LastChangeDate : (DateTime)ra.AssignmentDate,
                                                 IsEnabled = rl.Enabled,
                                                 ListRiskDescription = rl.Description

                                             }).ToList();

                companyLiskRiskResult.ForEach(x => companyListRisks.Add(x));

            }
            return companyListRisks.Take(200).ToList();
        }

        public CompanyListRiskPerson CreateListRiskPerson(CompanyListRiskPerson companyListRiskPerson)
        {


            NameValue[] parameters = new NameValue[13];
            CompanyListRiskPerson resultOperation = new CompanyListRiskPerson();
            parameters[0] = new NameValue("@ID_CARD_NO", companyListRiskPerson.IdCardNo);
            parameters[1] = new NameValue("@ID_CARD_TYPE_CD", companyListRiskPerson.DocumentType);
            parameters[2] = new NameValue("@PERSON_NAME", companyListRiskPerson.Name);
            parameters[3] = new NameValue("@SURNAME", companyListRiskPerson.LastName);            
            parameters[7] = new NameValue("@LIST_RISK_CD", companyListRiskPerson.ListRisk);
            parameters[8] = new NameValue("@CREATE_LIST_USER_ID", companyListRiskPerson.CreateListUserId);
            parameters[9] = new NameValue("@UPDATE_LIST_USER_ID", companyListRiskPerson.UpdateListUserId);
            parameters[10] = new NameValue("@EXCLUDED_PERSON", companyListRiskPerson.ExludedPerson);
            parameters[11] = new NameValue("@ASSIGNMENT_DATE", companyListRiskPerson.AssignmentDate.ToString("yyyy-MM-dd"));
            parameters[12] = new NameValue("@LAST_CHANGE_DATE", (companyListRiskPerson.LastChangeDate != null) ? ((DateTime)companyListRiskPerson.LastChangeDate).ToString("yyyy-MM-dd") : null);

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataTable("UP.SAVE_INDIVIDUAL_PERSON_LIST_RISLK", parameters);
            }
            if (result != null && result.Rows.Count > 0)
            {
                return companyListRiskPerson;
            }
            return resultOperation;
        }

        public List<CompanyListRiskPerson> GetListRiskPersonByDocumentNumber(string documentNumber)
        {
            List<CompanyListRiskPerson> companyPerson = new List<CompanyListRiskPerson>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(UPEN.RiskMaintenance.Properties.IdCardNo, typeof(UPEN.RiskMaintenance).Name);
            filter.Equal();
            filter.Constant(documentNumber);

            ListRiskPersonView view = new ListRiskPersonView();
            ViewBuilder builder = new ViewBuilder("ListRiskPersonView");
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade();
            if (view.RiskAssignedLists.Count > 0)
            {
                List<UPEN.RiskMaintenance> riskMaintenances = view.RiskMaintenances.Cast<UPEN.RiskMaintenance>().ToList();
                List<UPEN.RiskAssignedList> riskAssignedList = view.RiskAssignedLists.Cast<UPEN.RiskAssignedList>().ToList();
                List<UPEN.RiskList> riskLists = view.RiskLists.Cast<UPEN.RiskList>().ToList();
                var companyLiskRiskResult = (from rm in riskMaintenances
                                             join ra in riskAssignedList
                                             on rm.IdCardNo equals ra.IdCardNo
                                             join rl in riskLists
                                             on ra.RiskListCode equals rl.RiskListCode
                                             select new CompanyListRiskPerson
                                             {
                                                 IdCardNo = rm.IdCardNo,
                                                 DocumentType = rm.IdCardTypeCode,
                                                 Name = rm.PersonName,
                                                 LastName = rm.Surname,
                                                 ListRisk = ra.RiskListCode,
                                                 CreateListUserId = ra.CreatedListUserId,
                                                 UpdateListUserId = ra.UpdatedListUserId,
                                                 ExludedPerson = ra.ExcludedPerson,
                                                 AssignmentDate = (DateTime)ra.AssignmentDate,
                                                 LastChangeDate = (ra.LastChangeDate != null) ? (DateTime)ra.LastChangeDate : (DateTime)ra.AssignmentDate,
                                                 IsEnabled = rl.Enabled,
                                                 ListRiskDescription = rl.Description

                                             }).ToList();
                companyLiskRiskResult.ForEach(x => companyPerson.Add(x));

            }
            return companyPerson;

        }

        public string GetListRiskPersonTemporalByDocumentNumber(string documentNumber, int processId)
        {
            CompanyListRiskPerson companyPerson = new CompanyListRiskPerson();
            companyPerson.isTemporal = true;
            NameValue[] parameters = new NameValue[2];
            parameters[0] = new NameValue("@ID_CARD_NO", documentNumber);
            parameters[1] = new NameValue("@PROCESS_ID", processId);

            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataTable("UP.GET_PERSON_LIST_RISLK_TEMPORAL", parameters);
            }
            if (result != null && result.Rows.Count > 0)
            {
                return (string)result.Rows[0].ItemArray[2];
            }
            else
            {
                return null;
            }

        }
    }
}
