using Sistran.Company.Application.SarlaftBusinessServices.Models;
using Sistran.Company.Application.SarlaftBusinessServicesProvider.Assemblers;
using UPEN = Sistran.Core.Application.UniquePerson.Entities;
using USENT = Sistran.Core.Application.UniqueUser.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UPCEN = Sistran.Company.Application.UniquePerson.Entities;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.AuthorizationPolicies.Entities;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views;
using UUSM = Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Company.Application.SarlaftBusinessServicesProvider.Entities.Views;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Company.Application.SarlaftBusinessServicesProvider.Resources;

namespace Sistran.Company.Application.SarlaftBusinessServicesProvider.DAO
{

    public class SarlaftDAO
    {
        public CompanyUser GetUserByUserId(int userId, string userName)
        {
            //Busca la sucursal por defecto de un usuario
            var branchCode = 0;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(USENT.UserBranch.Properties.UserId, typeof(USENT.UserBranch).Name);
            filter.Equal();
            filter.Constant(userId);
            filter.And();
            filter.Property(USENT.UserBranch.Properties.DefaultBranch, typeof(USENT.UserBranch).Name);
            filter.Equal();
            filter.Constant(1);

            USENT.UserBranch entityBranch = DataFacadeManager.GetObjects(typeof(USENT.UserBranch), filter.GetPredicate()).Cast<USENT.UserBranch>().First();

            if (entityBranch == null)
            { return null; }

            branchCode = entityBranch.BranchCode;

            //Genera el número de formulario
            var sequential = 0;
            ObjectCriteriaBuilder filter2 = new ObjectCriteriaBuilder();
            filter2.Property(UPCEN.SarlaftBranch.Properties.BranchCode, typeof(UPCEN.SarlaftBranch).Name);
            filter2.Equal();
            filter2.Constant(branchCode);

            sequential = DataFacadeManager.GetObjects(typeof(UPCEN.SarlaftBranch), filter2.GetPredicate()).Cast<UPCEN.SarlaftBranch>().Max(x => (int)x.FormNum);
            sequential = sequential + 1;
            var formNumber = Convert.ToString(branchCode) + Convert.ToString(sequential);

            CompanyUser companyUser = new CompanyUser()
            {
                UserId = userId,
                Name = userName,
                BranchId = entityBranch.BranchCode,
                FormNum = formNumber
            };

            return companyUser;
        }

        /// <summary>
        /// Obtener exoneracion asociada a un individuo
        /// </summary>
        /// <param name="individualId">Id del individuo</param>
        /// <returns></returns>
        public List<CompanySarlaftExoneration> GetSarlaftExonerationByIndividualId(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UPEN.IndividualSarlaftExoneration.Properties.IndividualId, typeof(UPEN.IndividualSarlaftExoneration).Name);
            filter.Equal();
            filter.Constant(individualId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPEN.IndividualSarlaftExoneration), filter.GetPredicate()));
            List<CompanySarlaftExoneration> sarlaftExoneration = ModelAssembler.CreateSarlaftExonerations(businessCollection);

            return sarlaftExoneration;
        }

        /// <summary>
        /// Buscar persona por identificación
        /// </summary>
        /// <param name="person">datos persona</param>
        /// <returns></returns>
        public CompanyPerson GetPersonByDocumentNumberAndSearchType(string documentNum, int searchType)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UPEN.Person.Properties.IdCardNo, typeof(UPEN.Person).Name);
            filter.Equal();
            filter.Constant(documentNum);

            UPEN.Person entityPerson = DataFacadeManager.GetObjects(typeof(UPEN.Person), filter.GetPredicate()).Cast<UPEN.Person>().FirstOrDefault();

            if (entityPerson != null)
            {
                String name = null;
                CompanyPerson insured = ModelAssembler.CreatePerson(entityPerson);
                int individualId = insured.IndividualId;
                string nameSpace = " ";
                name = entityPerson.Name + nameSpace + entityPerson.Surname + nameSpace + entityPerson.MotherLastName;
                insured.AssociationType = this.GetAssociationType(insured?.IndividualId ?? 0);
                //name = GetInsuredByIndividualId(individualId);

                if (name != null)
                {
                    if (insured.EconomicActivity.Id != null)
                    {
                        ObjectCriteriaBuilder filter2 = new ObjectCriteriaBuilder();
                        filter2.Property(COMMEN.EconomicActivity.Properties.EconomicActivityCode, typeof(COMMEN.EconomicActivity).Name);
                        filter2.Equal();
                        filter2.Constant(insured.EconomicActivity.Id);
                        COMMEN.EconomicActivity entityEconomicActivity = DataFacadeManager.GetObjects(typeof(COMMEN.EconomicActivity), filter2.GetPredicate()).Cast<COMMEN.EconomicActivity>().First();
                        insured.EconomicActivity.Description = entityEconomicActivity.Description;
                    }
                    else
                    {
                        insured.EconomicActivity.Description = String.Empty;
                    }
                    insured.PersonType = searchType;
                    insured.Name = name;


                    return (insured);

                }
                else
                {
                    if (insured != null)
                    {
                        return (insured);
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Buscar persona por identificación
        /// </summary>
        /// <param name="person">datos persona</param>
        /// <returns></returns>
        public List<CompanyPerson> GetPersonByDocumentNumberAndSearchTypeList(string documentNum, int searchType)
        {
            List<CompanyPerson> insureds = new List<CompanyPerson>();//ModelAssembler.CreatePerson(entityPerson);
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UPEN.Person.Properties.IdCardNo, typeof(UPEN.Person).Name);
            filter.Equal();
            filter.Constant(documentNum);

            InsuredPersonSarlaftView insuredPersonSarlaftView = new InsuredPersonSarlaftView();
            ViewBuilder viewBuilder = new ViewBuilder("InsuredPersonSarlaftView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, insuredPersonSarlaftView);

            if (insuredPersonSarlaftView.Persons != null && insuredPersonSarlaftView.Persons.Count > 0)
            {
                if (insuredPersonSarlaftView.Insureds != null && insuredPersonSarlaftView.Insureds.Count > 0)
                {
                    UPEN.Insured insured = insuredPersonSarlaftView.Insureds.Cast<UPEN.Insured>().FirstOrDefault();
                    if (insured.DeclinedDate != null && insured.DeclinedDate > DateTime.MinValue)
                    {
                        throw new BusinessException(Errors.InsuredDeclined);
                    }
                }
                List<UPEN.Person> entityPersons = insuredPersonSarlaftView.Persons.Cast<UPEN.Person>().ToList();
                entityPersons.ForEach((x) =>
                {
                    String name = null;
                    CompanyPerson insured = ModelAssembler.CreatePerson(x);
                    int individualId = insured.IndividualId;
                    string nameSpace = " ";
                    name = x.Name + nameSpace + x.Surname + nameSpace + x.MotherLastName;
                    //name = GetInsuredByIndividualId(individualId);
                    insured.AssociationType = this.GetAssociationType(insured?.IndividualId ?? 0);

                    if (name != null)
                    {
                        if (insured.EconomicActivity.Id != null)
                        {
                            ObjectCriteriaBuilder filter2 = new ObjectCriteriaBuilder();
                            filter2.Property(COMMEN.EconomicActivity.Properties.EconomicActivityCode, typeof(COMMEN.EconomicActivity).Name);
                            filter2.Equal();
                            filter2.Constant(insured.EconomicActivity.Id);
                            COMMEN.EconomicActivity entityEconomicActivity = DataFacadeManager.GetObjects(typeof(COMMEN.EconomicActivity), filter2.GetPredicate()).Cast<COMMEN.EconomicActivity>().First();
                            insured.EconomicActivity.Description = entityEconomicActivity.Description;
                        }
                        else
                        {
                            ObjectCriteriaBuilder filter3 = new ObjectCriteriaBuilder();
                            filter3.Property(UPCEN.CoPrvIndividual.Properties.IndividualId, typeof(UPCEN.CoPrvIndividual).Name);
                            filter3.Equal();
                            filter3.Constant(insured.IndividualId);

                            UPCEN.CoPrvIndividual CoPrvIndividual = DataFacadeManager.GetObjects(typeof(UPCEN.CoPrvIndividual), filter3.GetPredicate()).Cast<UPCEN.CoPrvIndividual>().First();
                            insured.EconomicActivity.Id = CoPrvIndividual?.EconomicActivityCdNew ?? 0;

                            if (insured.EconomicActivity.Id != null)
                            {
                                ObjectCriteriaBuilder filter4 = new ObjectCriteriaBuilder();
                                filter4.Property(COMMEN.EconomicActivity.Properties.EconomicActivityCode, typeof(COMMEN.EconomicActivity).Name);
                                filter4.Equal();
                                filter4.Constant(insured.EconomicActivity.Id);
                                COMMEN.EconomicActivity EconomicActivity = DataFacadeManager.GetObjects(typeof(COMMEN.EconomicActivity), filter4.GetPredicate()).Cast<COMMEN.EconomicActivity>().First();
                                insured.FullName = name;
                                insured.EconomicActivity.Description = EconomicActivity.Description;

                            }
                            else
                            {

                                insured.EconomicActivity.Description = String.Empty;
                            }
                        }
                        insured.PersonType = searchType;
                        insured.Name = name;


                        insureds.Add(insured);

                    }
                    else
                    {
                        if (insured != null)
                        {
                            insureds.Add(insured);
                        }
                    }
                });
            }
            return insureds;
        }

        /// <summary>
        /// Buscar persona por identificación
        /// </summary>
        /// <param name="person">datos persona</param>
        /// <returns></returns>
        public CompanyCompany GetCompanyByDocumentNumberAndSearchType(string documentNum, int searchType)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Core.Application.UniquePerson.Entities.Company.Properties.TributaryIdNo, typeof(Core.Application.UniquePerson.Entities.Company).Name);
            filter.Equal();
            filter.Constant(documentNum);

            var entityCompany = DataFacadeManager.GetObjects(typeof(Core.Application.UniquePerson.Entities.Company), filter.GetPredicate()).Cast<Core.Application.UniquePerson.Entities.Company>().FirstOrDefault();


            if (entityCompany != null)
            {
                String name = null;
                CompanyCompany insured = ModelAssembler.CreateCompany(entityCompany);
                int individualId = insured.IndividualId;
                name = insured.FullName;
                //name = GetInsuredByIndividualId(individualId);
                insured.AssociationType = this.GetAssociationType(insured?.IndividualId ?? 0);
                if (name != null)
                {
                    if (insured.EconomicActivity.Id != null)
                    {
                        ObjectCriteriaBuilder filter2 = new ObjectCriteriaBuilder();
                        filter2.Property(COMMEN.EconomicActivity.Properties.EconomicActivityCode, typeof(COMMEN.EconomicActivity).Name);
                        filter2.Equal();
                        filter2.Constant(insured.EconomicActivity.Id);
                        COMMEN.EconomicActivity entityEconomicActivity = DataFacadeManager.GetObjects(typeof(COMMEN.EconomicActivity), filter2.GetPredicate()).Cast<COMMEN.EconomicActivity>().First();
                        insured.PersonType = searchType;
                        insured.FullName = name;
                        insured.EconomicActivity.Description = entityEconomicActivity.Description;
                        return (insured);
                    }
                    else
                    {
                        insured.EconomicActivity.Description = String.Empty;
                        insured.PersonType = searchType;
                        insured.FullName = name;
                        return (insured);
                    }
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public int GetAssociationType(int IndividualId)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(Core.Application.UniquePerson.Entities.CoCompany.Properties.IndividualId, typeof(Core.Application.UniquePerson.Entities.CoCompany).Name);
                filter.Equal();
                filter.Constant(IndividualId);

                var entityCoCompany = DataFacadeManager.GetObjects(typeof(Core.Application.UniquePerson.Entities.CoCompany), filter.GetPredicate()).Cast<Core.Application.UniquePerson.Entities.CoCompany>().FirstOrDefault();

                return entityCoCompany.AssociationTypeCode;
            }
            catch (Exception)
            {
                return 1;
            }

        }

        /// <summary>
        /// Buscar persona por identificación
        /// </summary>
        /// <param name="person">datos persona</param>
        /// <returns></returns>
        public List<CompanyCompany> GetCompanyByDocumentNumberAndSearchTypeList(string documentNum, int searchType)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Core.Application.UniquePerson.Entities.Company.Properties.TributaryIdNo, typeof(Core.Application.UniquePerson.Entities.Company).Name);
            filter.Equal();
            filter.Constant(documentNum);

            InsuredCompanySarlaftView insuredCompanySarlaftView = new InsuredCompanySarlaftView();
            ViewBuilder viewBuilder = new ViewBuilder("InsuredCompanySarlaftView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, insuredCompanySarlaftView);
            
            List<CompanyCompany> entitiesList = new List<CompanyCompany>();
            if (insuredCompanySarlaftView.Companies != null && insuredCompanySarlaftView.Companies.Count > 0)
            {
                if (insuredCompanySarlaftView.Insureds != null && insuredCompanySarlaftView.Insureds.Count > 0)
                {
                    UPEN.Insured insured = insuredCompanySarlaftView.Insureds.Cast<UPEN.Insured>().FirstOrDefault();
                    if (insured.DeclinedDate != null && insured.DeclinedDate > DateTime.MinValue)
                    {
                        throw new BusinessException(Errors.InsuredDeclined);
                    }
                }

                var entityCompany = insuredCompanySarlaftView.Companies.Cast<Core.Application.UniquePerson.Entities.Company>().ToList();
                foreach (var item in entityCompany)
                {
                    String name = null;
                    CompanyCompany insured = ModelAssembler.CreateCompany(item);
                    int individualId = insured.IndividualId;
                    name = insured.FullName;
                    insured.AssociationType = this.GetAssociationType(insured?.IndividualId ?? 0);
                    //name = GetInsuredByIndividualId(individualId);

                    if (name != null)
                    {
                        if (insured.EconomicActivity.Id != null)
                        {
                            ObjectCriteriaBuilder filter2 = new ObjectCriteriaBuilder();
                            filter2.Property(COMMEN.EconomicActivity.Properties.EconomicActivityCode, typeof(COMMEN.EconomicActivity).Name);
                            filter2.Equal();
                            filter2.Constant(insured.EconomicActivity.Id);
                            COMMEN.EconomicActivity entityEconomicActivity = DataFacadeManager.GetObjects(typeof(COMMEN.EconomicActivity), filter2.GetPredicate()).Cast<COMMEN.EconomicActivity>().First();
                            insured.PersonType = searchType;
                            insured.FullName = name;
                            insured.EconomicActivity.Description = entityEconomicActivity.Description;
                            entitiesList.Add(insured);
                        }
                        else
                        {
                            ObjectCriteriaBuilder filter3 = new ObjectCriteriaBuilder();
                            filter3.Property(UPCEN.CoPrvIndividual.Properties.IndividualId, typeof(UPCEN.CoPrvIndividual).Name);
                            filter3.Equal();
                            filter3.Constant(insured.IndividualId);
                            if (DataFacadeManager.GetObjects(typeof(UPCEN.CoPrvIndividual), filter3.GetPredicate()).Count > 0)
                            {
                                UPCEN.CoPrvIndividual CoPrvIndividual = DataFacadeManager.GetObjects(typeof(UPCEN.CoPrvIndividual), filter3.GetPredicate()).Cast<UPCEN.CoPrvIndividual>()?.First();
                                insured.EconomicActivity.Id = CoPrvIndividual?.EconomicActivityCdNew ?? 0;
                            }
                            if (insured.EconomicActivity.Id != null)
                            {
                                ObjectCriteriaBuilder filter4 = new ObjectCriteriaBuilder();
                                filter4.Property(COMMEN.EconomicActivity.Properties.EconomicActivityCode, typeof(COMMEN.EconomicActivity).Name);
                                filter4.Equal();
                                filter4.Constant(insured.EconomicActivity.Id);
                                COMMEN.EconomicActivity EconomicActivity = DataFacadeManager.GetObjects(typeof(COMMEN.EconomicActivity), filter4.GetPredicate()).Cast<COMMEN.EconomicActivity>().First();
                                insured.PersonType = searchType;
                                insured.FullName = name;
                                insured.EconomicActivity.Description = EconomicActivity.Description;
                                entitiesList.Add(insured);
                            }
                            else
                            {

                                insured.EconomicActivity.Description = String.Empty;
                                insured.PersonType = searchType;
                                insured.FullName = name;
                                entitiesList.Add(insured);
                            }
                        }
                    }
                }
            }
            return entitiesList;
        }

        public string GetInsuredByIndividualId(int individualId)
        {
            String CheckPayableTo = null;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UPEN.Insured.Properties.IndividualId, typeof(UPEN.Insured).Name);
            filter.Equal();
            filter.Constant(individualId);

            UPEN.Insured entityInsured = DataFacadeManager.GetObjects(typeof(UPEN.Insured), filter.GetPredicate()).Cast<UPEN.Insured>().First();

            if (entityInsured != null)
            {
                CheckPayableTo = entityInsured.CheckPayableTo;
            }

            return CheckPayableTo;

        }

        public List<CompanyIndividualSarlaft> GetSarlaft(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UPCEN.IndividualSarlaft.Properties.IndividualId, typeof(UPCEN.IndividualSarlaft).Name);
            filter.Equal();
            filter.Constant(individualId);

            var entitySarlaft = DataFacadeManager.GetObjects(typeof(UPCEN.IndividualSarlaft), filter.GetPredicate()).Cast<UPCEN.IndividualSarlaft>().OrderByDescending(x => x.RegistrationDate).ToList();
            List<CompanyIndividualSarlaft> listSarlafts = new List<CompanyIndividualSarlaft>();

            if (entitySarlaft.Count > 0)
            {
                listSarlafts = ModelAssembler.CreateSarlafts(entitySarlaft);

                foreach (CompanyIndividualSarlaft companyIndividualSarlaft in listSarlafts)
                {
                    PrimaryKey branchPrimaryKey = COMMEN.Branch.CreatePrimaryKey(Convert.ToInt32(companyIndividualSarlaft.BranchId));
                    companyIndividualSarlaft.BranchName = ((COMMEN.Branch)DataFacadeManager.GetObject(branchPrimaryKey)).Description;
                    PrimaryKey economicActivityPrimaryKey = COMMEN.EconomicActivity.CreatePrimaryKey(Convert.ToInt32(companyIndividualSarlaft.EconomicActivity.Id));
                    companyIndividualSarlaft.EconomicActivity.Description = ((COMMEN.EconomicActivity)DataFacadeManager.GetObject(economicActivityPrimaryKey)).Description;
                    PrimaryKey secondEconomicActivityPrimaryKey = COMMEN.EconomicActivity.CreatePrimaryKey(Convert.ToInt32(companyIndividualSarlaft.SecondEconomicActivity.Id));
                    companyIndividualSarlaft.SecondEconomicActivity.Description = ((COMMEN.EconomicActivity)DataFacadeManager.GetObject(secondEconomicActivityPrimaryKey)).Description;
                }
            }
            else
            {
                return listSarlafts;
            }
            return listSarlafts;
        }

        public CompanyIndividualSarlaft GetLastSarlaftId(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UPCEN.IndividualSarlaft.Properties.IndividualId, typeof(UPCEN.IndividualSarlaft).Name);
            filter.Equal();
            filter.Constant(individualId);

            var entitySarlaft = DataFacadeManager.GetObjects(typeof(UPCEN.IndividualSarlaft), filter.GetPredicate()).Cast<UPCEN.IndividualSarlaft>().OrderByDescending(x => x.RegistrationDate).FirstOrDefault();

            int paramSarlaftYear = 2169;

            if (entitySarlaft != null)
            {
                CompanyIndividualSarlaft individualSarlaft = ModelAssembler.CreateSarlaft(entitySarlaft);
                PrimaryKey branchPrimaryKey = COMMEN.Branch.CreatePrimaryKey(Convert.ToInt32(individualSarlaft.BranchId));
                individualSarlaft.BranchName = ((COMMEN.Branch)DataFacadeManager.GetObject(branchPrimaryKey)).Description;
                PrimaryKey economicActivityPrimaryKey = COMMEN.EconomicActivity.CreatePrimaryKey(Convert.ToInt32(individualSarlaft.EconomicActivity.Id));
                individualSarlaft.EconomicActivity.Description = ((COMMEN.EconomicActivity)DataFacadeManager.GetObject(economicActivityPrimaryKey)).Description;
                PrimaryKey secondEconomicActivityPrimaryKey = COMMEN.EconomicActivity.CreatePrimaryKey(Convert.ToInt32(individualSarlaft.SecondEconomicActivity.Id));
                individualSarlaft.SecondEconomicActivity.Description = ((COMMEN.EconomicActivity)DataFacadeManager.GetObject(secondEconomicActivityPrimaryKey)).Description;

                //se obtiene un valor parametrico para determinar el vencimiento del SARLAFT
                Parameter paramSarlaft = DelegateService.commonServiceCore.GetParameterByParameterId(paramSarlaftYear);
                if (paramSarlaft != null && paramSarlaft.NumberParameter.HasValue)
                {
                    individualSarlaft.YearParameter = paramSarlaft.NumberParameter.Value;
                }
                return individualSarlaft;
            }
            else
            {
                return null;
            }
        }

        public CompanyCustomerKnowledge CreateSarlaft(CompanyIndividualSarlaft individualSarlaft, CompanyFinancialSarlaft financialSarlaft)
        {
            using (Context.Current)
            {
                using (Transaction transaction = new Transaction())
                {
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(UPCEN.IndividualSarlaft.Properties.IndividualId, typeof(UPCEN.IndividualSarlaft).Name);
                    filter.Equal();
                    filter.Constant(individualSarlaft.IndividualId);
                    filter.And();
                    filter.Property(UPCEN.IndividualSarlaft.Properties.RegistrationDate, typeof(UPCEN.IndividualSarlaft).Name);
                    filter.Like();
                    filter.Constant("%"+individualSarlaft.RegistrationDate.GetValueOrDefault().Year+"%");

                    if (DataFacadeManager.GetObjects(typeof(UPCEN.IndividualSarlaft), filter.GetPredicate()).Count > 0)
                    {
                        return null;
                    }

                    //Genera el Sarlaft ID
                    var sarlaftId = 0;
                    ObjectCriteriaBuilder filter1 = new ObjectCriteriaBuilder();
                    filter1.Property(UPCEN.SarlaftNumber.Properties.BranchCode, typeof(UPCEN.SarlaftNumber).Name);
                    filter1.Equal();
                    filter1.Constant(individualSarlaft.BranchId);

                    sarlaftId = DataFacadeManager.GetObjects(typeof(UPCEN.SarlaftNumber), filter1.GetPredicate()).Cast<UPCEN.SarlaftNumber>().Max(x => x.SarlaftId);
                    sarlaftId = sarlaftId + 1;

                    if (sarlaftId == 0)
                    { return null; }

                    //Crea registro en Individual_Sarlaft
                    individualSarlaft.Id = sarlaftId;

                    UPCEN.IndividualSarlaft entityIndividualSarlaft = EntityAssembler.CreateIndividualSarlaft(individualSarlaft);

                    Sistran.Core.Application.UniquePersonService.V1.Models.EconomicActivity economicActivity = DelegateService.uniquePersonServiceCore.GetEconomicActivitiesById(int.Parse(individualSarlaft.EconomicActivity.Id.ToString()));

                    if (economicActivity == null)
                    {
                        entityIndividualSarlaft.EconomicActivityCode = 0;

                    }
                    Sistran.Core.Application.UniquePersonService.V1.Models.EconomicActivity SecondEconomicActivit = null;
                    if (individualSarlaft.SecondEconomicActivity.Id != null)
                    {
                        SecondEconomicActivit = DelegateService.uniquePersonServiceCore.GetEconomicActivitiesById(int.Parse(individualSarlaft.SecondEconomicActivity.Id.ToString()));
                    }

                    if (SecondEconomicActivit == null)
                    {
                        entityIndividualSarlaft.SecondEconomicActivityCode = 1; // sin definir en base
                    }


                    DataFacadeManager.Insert(entityIndividualSarlaft);

                    //Modifica el número de Formulario en Sarlaft Branch
                    var formNum = 0;
                    ObjectCriteriaBuilder filter2 = new ObjectCriteriaBuilder();
                    filter2.Property(UPCEN.SarlaftBranch.Properties.BranchCode, typeof(UPCEN.SarlaftBranch).Name);
                    filter2.Equal();
                    filter2.Constant(individualSarlaft.BranchId);

                    formNum = DataFacadeManager.GetObjects(typeof(UPCEN.SarlaftBranch), filter2.GetPredicate()).Cast<UPCEN.SarlaftBranch>().Max(x => (int)x.FormNum);
                    formNum = formNum + 1;

                    var primaryKeySarlaftBranch = UPCEN.SarlaftBranch.CreatePrimaryKey((int)individualSarlaft.BranchId);
                    var sarlaftBranch = (UPCEN.SarlaftBranch)DataFacadeManager.GetObject(primaryKeySarlaftBranch);
                    sarlaftBranch.FormNum = formNum;
                    DataFacadeManager.Update(sarlaftBranch);

                    //Modifica el último Id por sucursal en Sarlaft Number
                    var primaryKeySarlaftNumber = UPCEN.SarlaftNumber.CreatePrimaryKey((int)individualSarlaft.BranchId);
                    var sarlaftNumber = (UPCEN.SarlaftNumber)DataFacadeManager.GetObject(primaryKeySarlaftNumber);
                    sarlaftNumber.SarlaftId = sarlaftId;
                    DataFacadeManager.Update(sarlaftNumber);

                    //Crea registro en Financial_Sarlaft
                    financialSarlaft.SarlaftId = sarlaftId;
                    UPCEN.FinancialSarlaft entityFinancialSarlaft = EntityAssembler.CreateFinancialSarlaft(financialSarlaft);

                    DataFacadeManager.Insert(entityFinancialSarlaft);

                    //Crea o actualiza el registro en Co_Prv_Individual o Individual

                    ObjectCriteriaBuilder filter4 = new ObjectCriteriaBuilder();
                    filter4.Property(UPEN.Individual.Properties.IndividualId, typeof(UPEN.Individual).Name);
                    filter4.Equal();
                    filter4.Constant(individualSarlaft.IndividualId);

                    UPEN.Individual entityIndividual = DataFacadeManager.GetObjects(typeof(UPEN.Individual), filter4.GetPredicate()).Cast<UPEN.Individual>().FirstOrDefault();

                    if (entityIndividual.EconomicActivityCode != null)
                    {
                        entityIndividual.EconomicActivityCode = (economicActivity == null) ? 0 : individualSarlaft.EconomicActivity.Id;
                        DataFacadeManager.Update(entityIndividual);
                    }

                    CompanyIndividualSarlaft companyIndividualSarlaft = ModelAssembler.CreateSarlaft(entityIndividualSarlaft);
                    companyIndividualSarlaft.DocumentType = individualSarlaft.DocumentType;
                    companyIndividualSarlaft.DocumentNumber = individualSarlaft.DocumentNumber;
                    companyIndividualSarlaft.PersonType = individualSarlaft.PersonType;

                    CompanyFinancialSarlaft companyFinancialSarlaft = ModelAssembler.CreateFinancialSarlaft(entityFinancialSarlaft);

                    transaction.Complete();

                    return new CompanyCustomerKnowledge
                    {
                        Sarlaft = companyIndividualSarlaft,
                        FinancialSarlaft = companyFinancialSarlaft
                    };
                }
            }
        }

        public CompanyCustomerKnowledge UpdateSarlaft(CompanyIndividualSarlaft individualSarlaft, CompanyFinancialSarlaft financialSarlaft)
        {
            using (Context.Current)
            {
                using (Transaction transaction = new Transaction())
                {
                    var primaryKeyIndividual = UPCEN.IndividualSarlaft.CreatePrimaryKey(individualSarlaft.Id);
                    var entityindividualSarlaft = (UPCEN.IndividualSarlaft)DataFacadeManager.GetObject(primaryKeyIndividual);

                    Sistran.Core.Application.UniquePersonService.V1.Models.EconomicActivity economicActivity = DelegateService.uniquePersonServiceCore.GetEconomicActivitiesById(int.Parse(individualSarlaft.EconomicActivity.Id.ToString()));

                    DataFacadeManager.Update(entityindividualSarlaft);
                    entityindividualSarlaft.RegistrationDate = individualSarlaft.RegistrationDate;
                    entityindividualSarlaft.AuthorizedBy = individualSarlaft.AuthorizedBy;
                    entityindividualSarlaft.FillingDate = individualSarlaft.FillingDate;
                    entityindividualSarlaft.VerifyingEmployee = individualSarlaft.VerifyingEmployee;
                    entityindividualSarlaft.CheckDate = individualSarlaft.CheckDate;
                    entityindividualSarlaft.InterviewDate = individualSarlaft.InterviewDate;
                    entityindividualSarlaft.InterviewerName = individualSarlaft.InterviewerName;
                    entityindividualSarlaft.EconomicActivityCode = (economicActivity == null) ? 0 : Convert.ToInt32(individualSarlaft.EconomicActivity.Id);
                    entityindividualSarlaft.SecondEconomicActivityCode = Convert.ToInt32(individualSarlaft.SecondEconomicActivity.Id);
                    entityindividualSarlaft.InternationalOperations = individualSarlaft.InternationalOperations;
                    entityindividualSarlaft.InterviewPlace = individualSarlaft.InterviewPlace;
                    entityindividualSarlaft.InterviewResultCode = individualSarlaft.InterviewResultId;
                    entityindividualSarlaft.PendingEvent = individualSarlaft.PendingEvent;

                    DataFacadeManager.Update(entityindividualSarlaft);

                    var primaryKeyFinancial = UPCEN.FinancialSarlaft.CreatePrimaryKey(individualSarlaft.Id);
                    var entityFinancialSarlaft = (UPCEN.FinancialSarlaft)DataFacadeManager.GetObject(primaryKeyFinancial);

                    entityFinancialSarlaft.IncomeAmount = financialSarlaft.IncomeAmount;
                    entityFinancialSarlaft.ExpenseAmount = financialSarlaft.ExpenseAmount;
                    entityFinancialSarlaft.ExtraIncomeAmount = financialSarlaft.ExtraIncomeAmount;
                    entityFinancialSarlaft.AssetsAmount = financialSarlaft.AssetsAmount;
                    entityFinancialSarlaft.LiabilitiesAmount = financialSarlaft.LiabilitiesAmount;
                    entityFinancialSarlaft.Description = financialSarlaft.Description;

                    DataFacadeManager.Update(entityFinancialSarlaft);

                    //Crea o actualiza el registro en Co_Prv_Individual o Individual

                    //ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    //filter.Property(UPCEN.CoPrvIndividual.Properties.IndividualId, typeof(UPCEN.CoPrvIndividual).Name);
                    //filter.Equal();
                    //filter.Constant(individualSarlaft.IndividualId);

                    //UPCEN.CoPrvIndividual entityCoPrvIndividual = DataFacadeManager.GetObjects(typeof(UPCEN.CoPrvIndividual), filter.GetPredicate()).Cast<UPCEN.CoPrvIndividual>().FirstOrDefault();

                    //if (entityCoPrvIndividual != null)
                    //{
                    //    if (entityCoPrvIndividual.EconomicActivityCdNew != null)
                    //    {
                    //        entityCoPrvIndividual = EntityAssembler.CreateCoPrvIndividual(individualSarlaft.IndividualId, individualSarlaft.EconomicActivity.Id, individualSarlaft.SecondEconomicActivity.Id);
                    //        DataFacadeManager.Update(entityCoPrvIndividual);
                    //    }
                    //}
                    //else
                    //{
                    //    ObjectCriteriaBuilder filter2 = new ObjectCriteriaBuilder();
                    //    filter2.Property(UPEN.Individual.Properties.IndividualId, typeof(UPEN.Individual).Name);
                    //    filter2.Equal();
                    //    filter2.Constant(individualSarlaft.IndividualId);

                    //    UPEN.Individual entityIndividual = DataFacadeManager.GetObjects(typeof(UPEN.Individual), filter2.GetPredicate()).Cast<UPEN.Individual>().FirstOrDefault();

                    //    if (entityIndividual != null)
                    //    {
                    //        if (entityIndividual.EconomicActivityCode != null)
                    //        {
                    //            entityIndividual.EconomicActivityCode = (economicActivity == null) ? 0 : individualSarlaft.EconomicActivity.Id;
                    //            DataFacadeManager.Update(entityIndividual);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        DataFacadeManager.Insert(entityCoPrvIndividual);
                    //    }
                    //}

                    transaction.Complete();

                    return new CompanyCustomerKnowledge
                    {
                        Sarlaft = individualSarlaft,
                        FinancialSarlaft = financialSarlaft
                    };
                }
            }

        }

        public CompanyCustomerKnowledge GetSarlaftBySarlaftId(int sarlaftID)
        {
            //Obtiene registro Individual_Sarlaft
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UPCEN.IndividualSarlaft.Properties.SarlaftId, typeof(UPEN.IndividualSarlaft).Name);
            filter.Equal();
            filter.Constant(sarlaftID);

            UPCEN.IndividualSarlaft entityIndividualSarlaft = DataFacadeManager.GetObjects(typeof(UPCEN.IndividualSarlaft), filter.GetPredicate()).Cast<UPCEN.IndividualSarlaft>().First();

            //Obtiene registro Financial_Sarlaft
            ObjectCriteriaBuilder filter2 = new ObjectCriteriaBuilder();
            filter2.Property(UPCEN.FinancialSarlaft.Properties.SarlaftId, typeof(UPEN.Insured).Name);
            filter2.Equal();
            filter2.Constant(sarlaftID);

            UPCEN.FinancialSarlaft entityFinancialSarlaft = DataFacadeManager.GetObjects(typeof(UPCEN.FinancialSarlaft), filter2.GetPredicate()).Cast<UPCEN.FinancialSarlaft>().First();

            //Obtiene nombre de usuario
            ObjectCriteriaBuilder filter3 = new ObjectCriteriaBuilder();
            filter3.Property(USENT.UniqueUsers.Properties.UserId, typeof(USENT.UniqueUsers).Name);
            filter3.Equal();
            filter3.Constant(entityIndividualSarlaft.UserId);

            USENT.UniqueUsers entityUser = DataFacadeManager.GetObjects(typeof(USENT.UniqueUsers), filter3.GetPredicate()).Cast<USENT.UniqueUsers>().First();

            //Obtiene nombre de sucursal
            ObjectCriteriaBuilder filter4 = new ObjectCriteriaBuilder();
            filter4.Property(COMMEN.Branch.Properties.BranchCode, typeof(COMMEN.Branch).Name);
            filter4.Equal();
            filter4.Constant(entityIndividualSarlaft.BranchCode);

            COMMEN.Branch entityBranch = DataFacadeManager.GetObjects(typeof(COMMEN.Branch), filter4.GetPredicate()).Cast<COMMEN.Branch>().First();

            UPCEN.CoPrvIndividual entityCoPrvIndividual = new UPCEN.CoPrvIndividual(entityIndividualSarlaft.IndividualId);

            if (entityIndividualSarlaft.EconomicActivityCode == 0)
            {
                ObjectCriteriaBuilder filterActNew = new ObjectCriteriaBuilder();
                filterActNew.Property(UPCEN.CoPrvIndividual.Properties.IndividualId, typeof(UPCEN.CoPrvIndividual).Name);
                filterActNew.Equal();
                filterActNew.Constant(entityIndividualSarlaft.IndividualId);

                entityCoPrvIndividual = DataFacadeManager.GetObjects(typeof(UPCEN.CoPrvIndividual), filterActNew.GetPredicate()).Cast<UPCEN.CoPrvIndividual>().FirstOrDefault();
                entityIndividualSarlaft.EconomicActivityCode = entityCoPrvIndividual?.EconomicActivityCdNew ?? 0;

            }

            if (entityIndividualSarlaft.SecondEconomicActivityCode == 0)
            {
                ObjectCriteriaBuilder filterActNews = new ObjectCriteriaBuilder();
                filterActNews.Property(UPCEN.CoPrvIndividual.Properties.IndividualId, typeof(UPCEN.CoPrvIndividual).Name);
                filterActNews.Equal();
                filterActNews.Constant(entityIndividualSarlaft.IndividualId);

                entityCoPrvIndividual = DataFacadeManager.GetObjects(typeof(UPCEN.CoPrvIndividual), filterActNews.GetPredicate()).Cast<UPCEN.CoPrvIndividual>().FirstOrDefault();
                entityIndividualSarlaft.SecondEconomicActivityCode = entityCoPrvIndividual?.SecondEconomicActivityCdNew ?? 0;
            }


            ObjectCriteriaBuilder filter5 = new ObjectCriteriaBuilder();
            filter5.Property(COMMEN.EconomicActivity.Properties.EconomicActivityCode, typeof(COMMEN.EconomicActivity).Name);
            filter5.Equal();
            filter5.Constant((entityIndividualSarlaft.EconomicActivityCode == 0) ? entityCoPrvIndividual?.EconomicActivityCdNew ?? 0 : entityIndividualSarlaft.EconomicActivityCode);

            COMMEN.EconomicActivity entityEconomicActivity = DataFacadeManager.GetObjects(typeof(COMMEN.EconomicActivity), filter5.GetPredicate()).Cast<COMMEN.EconomicActivity>().FirstOrDefault();

            //Obtiene nombre de la segunda actividad economica
            ObjectCriteriaBuilder filter6 = new ObjectCriteriaBuilder();
            filter6.Property(COMMEN.EconomicActivity.Properties.EconomicActivityCode, typeof(COMMEN.EconomicActivity).Name);
            filter6.Equal();
            filter6.Constant(entityIndividualSarlaft.SecondEconomicActivityCode);

            COMMEN.EconomicActivity entityEconomicActivity2 = DataFacadeManager.GetObjects(typeof(COMMEN.EconomicActivity), filter6.GetPredicate()).Cast<COMMEN.EconomicActivity>().FirstOrDefault();

            CompanyIndividualSarlaft companyIndividualSarlaft = ModelAssembler.CreateSarlaft(entityIndividualSarlaft);
            CompanyFinancialSarlaft companyFinancialSarlaft = ModelAssembler.CreateFinancialSarlaft(entityFinancialSarlaft);

            companyIndividualSarlaft.UserName = entityUser.AccountName;
            companyIndividualSarlaft.BranchName = entityBranch.Description;
            companyIndividualSarlaft.EconomicActivity.Id = entityEconomicActivity?.EconomicActivityCode ?? 0;
            companyIndividualSarlaft.EconomicActivity.Description = entityEconomicActivity?.Description ?? "";
            companyIndividualSarlaft.SecondEconomicActivity.Id = entityEconomicActivity2?.EconomicActivityCode ?? 0;
            companyIndividualSarlaft.SecondEconomicActivity.Description = entityEconomicActivity2?.Description ?? "";

            return new CompanyCustomerKnowledge
            {
                Sarlaft = companyIndividualSarlaft,
                FinancialSarlaft = companyFinancialSarlaft
            };

        }

        public List<CompanyEconomicActivity> GetEconomicActivities(string description)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Int64 id = 0;
            Int64.TryParse(description, out id);

            if (id > 0)
            {
                filter.Property(COMMEN.EconomicActivity.Properties.EconomicActivityCode, typeof(COMMEN.EconomicActivity).Name);
                filter.Equal();
                filter.Constant(id);
            }
            else
            {
                filter.Property(COMMEN.EconomicActivity.Properties.Description, typeof(COMMEN.EconomicActivity).Name);
                filter.Like();
                filter.Constant(description + "%");
            }

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(COMMEN.EconomicActivity), filter.GetPredicate());
            if (businessCollection.Count() == 0)
            {
                filter = new ObjectCriteriaBuilder();

                if (id > 0)
                {
                    filter.Property(COMMEN.EconomicActivity.Properties.EconomicActivityCode, typeof(COMMEN.EconomicActivity).Name);
                    filter.Equal();
                    filter.Constant(id);
                }
                else
                {
                    filter.Property(COMMEN.EconomicActivity.Properties.Description, typeof(COMMEN.EconomicActivity).Name);
                    filter.Like();
                    filter.Constant(description + "%");
                }
                businessCollection = DataFacadeManager.GetObjects(typeof(COMMEN.EconomicActivity), filter.GetPredicate());
                return ModelAssembler.CreateCompanyCoEconomicActivities(businessCollection);
            }
            return ModelAssembler.CreateCompanyEconomicActivities(businessCollection);
        }

        public bool ValidationAccessAndHierarchyByUser(int UserId)
        {

            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                filter.Property(USENT.CoHierarchyAccesses.Properties.UserId, typeof(USENT.CoHierarchyAccesses).Name);
                filter.Equal();
                filter.Constant(UserId);
                filter.And();
                filter.Property(USENT.CoHierarchyAccesses.Properties.ModuleCode, typeof(USENT.CoHierarchyAccesses).Name);
                filter.Equal();
                filter.Constant(20);
                filter.And();
                filter.Property(USENT.CoHierarchyAccesses.Properties.SubmoduleCode, typeof(USENT.CoHierarchyAccesses).Name);
                filter.Equal();
                filter.Constant(32);
                filter.And();
                filter.Property(USENT.CoHierarchyAccesses.Properties.HierarchyCode, typeof(USENT.CoHierarchyAccesses).Name);
                filter.Equal();
                filter.Constant(0);

                BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(USENT.CoHierarchyAccesses), filter.GetPredicate());

                if (businessCollection.Count == 0)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public CompanyTmpSarlaftOperation GetCompanyTmpSarlaftOperation(int operationId)
        {
            try
            {
                PrimaryKey primaryKey = TmpSarlaftOperation.CreatePrimaryKey(operationId);
                var entitySarlaftCollection = (TmpSarlaftOperation)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
                return ModelAssembler.CreateCompanyTmpSarlaftOperation(entitySarlaftCollection);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyTmpSarlaftOperation CreateCompanySarlaftOperation(CompanyTmpSarlaftOperation companyTmpSarlaftOperation)
        {
            try
            {
                TmpSarlaftOperation entityTmpSarlaftOperation = EntityAssembler.CreateTmpSarlaftOperation(companyTmpSarlaftOperation);
                DataFacadeManager.Insert(entityTmpSarlaftOperation);
                companyTmpSarlaftOperation.OperationId = entityTmpSarlaftOperation.OperationId;
                return companyTmpSarlaftOperation;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Consultar politicas generadas para sarlaft
        /// </summary>
        /// <param name="person">datos persona</param>
        /// <returns></returns>
        public List<CompanyTmpSarlaftOperation> GetSarlaftOperationTmp(int IndividualId)
        {
            List<CompanyTmpSarlaftOperation> listSarlaftOperation = new List<CompanyTmpSarlaftOperation>();

            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(TmpSarlaftOperation.Properties.OperationId, "tso")));
            select.AddSelectValue(new SelectValue(new Column(TmpSarlaftOperation.Properties.FunctionId, "tso")));
            select.AddSelectValue(new SelectValue(new Column(TmpSarlaftOperation.Properties.IndividualId, "tso")));

            select.AddSelectValue(new SelectValue(new Column(AuthorizationAnswer.Properties.AuthorizationRequestId, "aa")));
            select.AddSelectValue(new SelectValue(new Column(AuthorizationAnswer.Properties.Enabled, "aa")));

            select.AddSelectValue(new SelectValue(new Column(AuthorizationRequest.Properties.StatusId, "ar")));

            Join join = new Join(new ClassNameTable(typeof(TmpSarlaftOperation), "tso"),
                new ClassNameTable(typeof(AuthorizationRequest), "ar"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(AuthorizationRequest.Properties.FunctionId, "ar").Equal()
                    .Property(TmpPersonOperation.Properties.FunctionId, "tso")

                    .And().Property(TmpSarlaftOperation.Properties.OperationId, "tso").Equal()
                    .Property(AuthorizationRequest.Properties.Key, "ar").GetPredicate()
            };

            join = new Join(join, new ClassNameTable(typeof(AuthorizationAnswer), "aa"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                   .Property(AuthorizationAnswer.Properties.AuthorizationRequestId, "aa").Equal()
                   .Property(AuthorizationRequest.Properties.AuthorizationRequestId, "ar").GetPredicate()

            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(TmpSarlaftOperation.Properties.IndividualId, "tso").Equal().Constant(IndividualId);
            where.And().Property(AuthorizationAnswer.Properties.Enabled, "aa").Equal().Constant(1);
            where.And().Property(AuthorizationRequest.Properties.StatusId, "ar").In();
            where.ListValue();
            where.Constant(1);
            where.Constant(3);
            where.EndList();

            select.Table = join;
            select.Where = where.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    listSarlaftOperation.Add(new CompanyTmpSarlaftOperation
                    {
                        IndividualId = (int)reader["IndividualId"],
                        FunctionId = (int)reader["FunctionId"],
                        StatusId = (int)reader["StatusId"],
                        RequestId = (int)reader["AuthorizationRequestId"]

                    });
                }
            }

            return listSarlaftOperation;

        }


        public List<UUSM.User> GetUserByAccountName(string accountName)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();


            filter.Property(USENT.UniqueUsers.Properties.AccountName, typeof(USENT.UniqueUsers).Name);
            filter.Like();
            filter.Constant("%" + accountName.Trim() + "%");


            UniqueUserView view = new UniqueUserView();
            Core.Framework.DAF.Engine.ViewBuilder builder = new Core.Framework.DAF.Engine.ViewBuilder("UniqueUserView");
            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            List<UUSM.User> users = new List<UUSM.User>();

            if (view.UniqueUsers.Count > 0)
            {
                users = ModelAssembler.CreateUniqueUsers(view.UniqueUsers, view.Persons);
                return users;
            }
            else
            {
                return null;
            }

        }

        public List<UUSM.User> GetUserByAccountNameSarlaft(string accountName)
        {
            List<UUSM.User> users = new List<UUSM.User>();
            int agentCode = 0;

            /*Consulta los intermediarios*/
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(UPEN.AgentAgency.Properties.IndividualId, "aa")));
            select.AddSelectValue(new SelectValue(new Column(UPEN.AgentAgency.Properties.Description, "aa")));

            Join join = new Join(new ClassNameTable(typeof(UPEN.AgentAgency), "aa"), new ClassNameTable(typeof(UPEN.Agent), "a"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(UPEN.AgentAgency.Properties.IndividualId, "aa")
                   .Equal().Property(UPEN.Agent.Properties.IndividualId, "a").GetPredicate()
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            if (int.TryParse(accountName, out agentCode))
            {
                where.Property(UPEN.AgentAgency.Properties.AgentCode, "aa").Equal().Constant(agentCode);
                where.And();
                where.Property(UPEN.AgentAgency.Properties.DeclinedDate, "aa").IsNull();
            }
            else
            {
                where.OpenParenthesis();
                where.Property(UPEN.AgentAgency.Properties.Description, "aa").Like().Constant($"%{accountName.Trim()}%");
                where.Or();
                where.Property(UPEN.Agent.Properties.CheckPayableTo, "a").Like().Constant($"%{accountName.Trim()}%");
                where.CloseParenthesis();
                where.And();
                where.Property(UPEN.AgentAgency.Properties.DeclinedDate, "aa").IsNull();
            }

            select.Table = join;
            select.Where = where.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    users.Add(new UUSM.User
                    {
                        PersonId = int.Parse(reader["IndividualId"].ToString()),
                        AccountName = reader["Description"].ToString()
                    });
                }
            }

            /*Consulta los empleados*/
            select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.IndividualId, "p")));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.Name, "p")));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.Surname, "p")));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.MotherLastName, "p")));

            join = new Join(new ClassNameTable(typeof(UPEN.Person), "p"), new ClassNameTable(typeof(UPEN.Employee), "e"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder().Property(UPEN.Person.Properties.IndividualId, "p")
                   .Equal().Property(UPEN.Employee.Properties.IndividualId, "e").GetPredicate()
            };

            where = new ObjectCriteriaBuilder();
            where.OpenParenthesis();
            where.Property(UPEN.Person.Properties.IdCardNo, "p").Like().Constant($"%{accountName.Trim()}%");
            where.Or();
            where.Property(UPEN.Person.Properties.Name, "p").Like().Constant($"%{accountName.Trim()}%");
            where.Or();
            where.Property(UPEN.Person.Properties.Surname, "p").Like().Constant($"%{accountName.Trim()}%");
            where.Or();
            where.Property(UPEN.Person.Properties.MotherLastName, "p").Like().Constant($"%{accountName.Trim()}%");
            where.CloseParenthesis();
            where.And();
            where.Property(UPEN.Employee.Properties.EgressDate, "e").IsNull();

            select.Table = join;
            select.Where = where.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    string name = reader["Name"] == null ? "" : reader["Name"].ToString();
                    string surname = reader["Surname"] == null ? "" : reader["Surname"].ToString();
                    string motherLastName = reader["MotherLastName"] == null ? "" : reader["MotherLastName"].ToString();

                    users.Add(new UUSM.User
                    {
                        PersonId = int.Parse(reader["IndividualId"].ToString()),
                        AccountName = $"{name} {surname} {motherLastName}".Trim()
                    });
                }
            }

            /*Consulta los usuarios*/
            select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(USENT.UniqueUsers.Properties.PersonId, "uu")));
            select.AddSelectValue(new SelectValue(new Column(USENT.UniqueUsers.Properties.AccountName, "uu")));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.Name, "p")));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.Surname, "p")));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.MotherLastName, "p")));

            join = new Join(new ClassNameTable(typeof(USENT.UniqueUsers), "uu"), new ClassNameTable(typeof(UPEN.Person), "p"), JoinType.Left)
            {
                Criteria = new ObjectCriteriaBuilder().Property(USENT.UniqueUsers.Properties.PersonId, "uu")
                   .Equal().Property(UPEN.Person.Properties.IndividualId, "p").GetPredicate()
            };

            where = new ObjectCriteriaBuilder();
            where.OpenParenthesis();
            where.Property(USENT.UniqueUsers.Properties.AccountName, "uu").Like().Constant($"%{accountName.Trim()}%");
            where.Or();
            where.Property(UPEN.Person.Properties.Name, "p").Like().Constant($"%{accountName.Trim()}%");
            where.Or();
            where.Property(UPEN.Person.Properties.Surname, "p").Like().Constant($"%{accountName.Trim()}%");
            where.Or();
            where.Property(UPEN.Person.Properties.MotherLastName, "p").Like().Constant($"%{accountName.Trim()}%");
            where.CloseParenthesis();
            where.And();
            where.Property(USENT.UniqueUsers.Properties.DisabledDate, "uu").IsNull();

            select.Table = join;
            select.Where = where.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    string name = reader["Name"] == null?"": reader["Name"].ToString();
                    string surname = reader["Surname"] == null? "" : reader["Surname"].ToString();
                    string motherLastName = reader["MotherLastName"] == null ? "" : reader["MotherLastName"].ToString();

                    users.Add(new UUSM.User
                    {
                        PersonId = int.Parse(reader["PersonId"].ToString()),
                        AccountName = reader["AccountName"].ToString(),
                        Name = $"{name} {surname} {motherLastName}".Trim() 
                    });
                }
            }

            return users;
        }

    }
}
