using Sistran.Core.Application.UniquePersonService.V1.Enums;
using MOUP= Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using System.Diagnostics;
using System.Linq;
using Sistran.Core.Application.UniquePersonService.V1.DAOs;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class InsuredBusiness
    {
        /// <summary>
        /// Crea Asegurado
        /// </summary>
        /// <param name="insured"></param>
        /// <returns></returns>
        public Models.Insured CreateInsured(Models.Insured insured)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePersonV1.Entities.Insured.Properties.IndividualId, typeof(UniquePersonV1.Entities.Insured).Name);
            filter.Equal();
            filter.Constant(insured.IndividualId);
            UniquePersonV1.Entities.Insured insuredEntity = null;

            insuredEntity = EntityAssembler.CreateInsured(insured);

            DataFacadeManager.Insert(insuredEntity);
            var resultInsured = ModelAssembler.CreateInsured(insuredEntity);
            
            if (insured.Agency != null)
            {
                AgentBusiness insuredAgentBusiness = new AgentBusiness();
                insuredAgentBusiness.CreateInsuredAgent(insured.Agency, insured.IndividualId);
                resultInsured.Agency = insured.Agency;
            }

            if (insured.Concept != null)
            {
                insured.Concept.InsuredCode = insuredEntity.InsuredCode;
                InsuredBusiness insuredConceptBusiness = new InsuredBusiness();
                resultInsured.Concept = insuredConceptBusiness.CreateInsuredConcept(insured.Concept);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.CreateInsured");
            
            return resultInsured;
        }
        public MOUP.Insured UpdateInsured(MOUP.Insured insured)
        {
            PrimaryKey key = Insured.CreatePrimaryKey(insured.IndividualId, insured.InsuredCode);
            var insuredEntity = (Insured)DataFacadeManager.GetObject(key);

            insuredEntity = EntityAssembler.CreateInsured(insured);
            insuredEntity.IndividualId = insured.IndividualId;
            insuredEntity.InsuredCode = insured.InsuredCode;
            insuredEntity.CheckPayableTo = insured.FullName;
            insuredEntity.BranchCode = insured.Branch.Id;
            insuredEntity.InsDeclinedTypeCode = insured.DeclinedType?.Id;
            insuredEntity.Annotations = insured.Annotations;
            insuredEntity.InsSegmentCode = insured.Segment.Id;
            insuredEntity.InsProfileCode = insured.Profile.Id;
            insuredEntity.IsMailAddress = (insured.IsMailAddress == true) ? 1 : 0;
            insuredEntity.IsSms = (insured.IsSMS == true) ? 1 : 0;
            insuredEntity.ReferredBy = insured.ReferedBy;
            insuredEntity.EnteredDate = insured.EnteredDate;
            insuredEntity.DeclinedDate = insured.DeclinedDate;
            insuredEntity.ModifyDate = insured.ModifyDate;
            DataFacadeManager.Update(insuredEntity);

            if (insured.Agency != null)
            { 
                AgentBusiness insuredAgentBusiness = new AgentBusiness();
                insuredAgentBusiness.UpdateInsuredAgent(insured.Agency, insured.IndividualId);
            }

            if (insured.Concept != null)
            {
                insured.Concept.InsuredCode = insuredEntity.InsuredCode;
                InsuredBusiness insuredConceptBusiness = new InsuredBusiness();
                insuredConceptBusiness.UpdateInsuredConcept(insured.Concept);
            }

            return ModelAssembler.CreateInsured(insuredEntity);
        }

        

        public MOUP.Insured UpdateInsuredElectronicBilling(MOUP.Insured insured)
        {
            PrimaryKey key = Insured.CreatePrimaryKey(insured.IndividualId, insured.InsuredCode);
            var entityInsured = (Insured)DataFacadeManager.GetObject(key);

            entityInsured = EntityAssembler.CreateInsured(insured);
            entityInsured.IndividualId = insured.IndividualId;
            entityInsured.InsuredCode = insured.InsuredCode;
            entityInsured.CheckPayableTo = insured.FullName;
            entityInsured.BranchCode = insured.Branch.Id;
            entityInsured.InsDeclinedTypeCode = insured.DeclinedType?.Id;
            entityInsured.Annotations = insured.Annotations;
            entityInsured.InsSegmentCode = insured.Segment.Id;
            entityInsured.InsProfileCode = insured.Profile.Id;
            entityInsured.IsMailAddress = (insured.IsMailAddress == true) ? 1 : 0;
            entityInsured.IsSms = (insured.IsSMS == true) ? 1 : 0;
            entityInsured.ReferredBy = insured.ReferedBy;
            entityInsured.EnteredDate = insured.EnteredDate;
            entityInsured.DeclinedDate = insured.DeclinedDate;
            entityInsured.ModifyDate = insured.ModifyDate;
            DataFacadeManager.Update(entityInsured);

            
            return ModelAssembler.CreateInsured(entityInsured);
        }
        /// <summary>
        /// Trae Asegurado por IndividualId
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public Models.Insured GetInsuredByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePersonV1.Entities.Insured.Properties.IndividualId, typeof(UniquePersonV1.Entities.Insured).Name);
            filter.Equal();
            filter.Constant(individualId);
            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(UniquePersonV1.Entities.Insured), filter.GetPredicate());
            Models.Insured insured = ModelAssembler.CreateInsureds(businessCollection).FirstOrDefault();
            if (insured != null)
            {
                ObjectCriteriaBuilder filterConceptInsured = new ObjectCriteriaBuilder();
                filterConceptInsured.Property(UniquePersonV1.Entities.InsuredConcept.Properties.InsuredCode, typeof(UniquePersonV1.Entities.InsuredConcept).Name);
                filterConceptInsured.Equal();
                filterConceptInsured.Constant(insured.InsuredCode);
                UniquePersonV1.Entities.InsuredConcept insuredConcept = (UniquePersonV1.Entities.InsuredConcept)DataFacadeManager.GetObjects(typeof(UniquePersonV1.Entities.InsuredConcept), filterConceptInsured.GetPredicate()).FirstOrDefault();
                if (insuredConcept != null)
                {
                    insured.Concept = ModelAssembler.CreateInsuredConcept(insuredConcept);
                }

                ObjectCriteriaBuilder filterAseAge = new ObjectCriteriaBuilder();
                filterAseAge.Property(UniquePersonV1.Entities.InsuredAgent.Properties.InsuredIndId, typeof(UniquePersonV1.Entities.InsuredAgent).Name);
                filterAseAge.Equal();
                filterAseAge.Constant(individualId);
                filterAseAge.And();
                filterAseAge.Property(UniquePersonV1.Entities.InsuredAgent.Properties.IsMain, typeof(UniquePersonV1.Entities.InsuredAgent).Name);
                filterAseAge.Equal();
                filterAseAge.Constant(true);
                UniquePersonV1.Entities.InsuredAgent insuredAgent = (UniquePersonV1.Entities.InsuredAgent)DataFacadeManager.GetObjects(typeof(UniquePersonV1.Entities.InsuredAgent), filterAseAge.GetPredicate()).FirstOrDefault();

                ObjectCriteriaBuilder filterAgent = new ObjectCriteriaBuilder();
                UniquePersonV1.Entities.Agent agent = null;
                if (insuredAgent != null && insuredAgent.AgentIndId > 0)
                {
                    filterAgent.Property(UniquePersonV1.Entities.Agent.Properties.IndividualId, typeof(UniquePersonV1.Entities.Agent).Name);
                    filterAgent.Equal();
                    filterAgent.Constant(insuredAgent.AgentIndId);
                    agent = (UniquePersonV1.Entities.Agent)DataFacadeManager.GetObjects(typeof(UniquePersonV1.Entities.Agent), filterAgent.GetPredicate()).First();
                }
                if (agent != null)
                {
                    insured.Agency = new MOUP.Agency();
                    insured.Agency.Id = insuredAgent.AgentAgencyId;
                    insured.Agency.Agent = new MOUP.Agent
                    {
                        IndividualId = agent.IndividualId
                    };
                    insured.Agency.FullName = agent.CheckPayableTo;
                }

                PrimaryKey key = Individual.CreatePrimaryKey(individualId);
                Sistran.Core.Application.UniquePersonV1.Entities.Individual individualEntity = (Sistran.Core.Application.UniquePersonV1.Entities.Individual)DataFacadeManager.GetObject(key);

                insured.IndividualType = (IndividualType)individualEntity.IndividualTypeCode;

                ObjectCriteriaBuilder filterIdentificationInsured = new ObjectCriteriaBuilder();
                if (insured.IndividualType == IndividualType.Person)
                {
                    filterIdentificationInsured.Property(UniquePersonV1.Entities.Person.Properties.IndividualId, typeof(UniquePersonV1.Entities.Person).Name);
                    filterIdentificationInsured.Equal();
                    filterIdentificationInsured.Constant(individualId);
                    UniquePersonV1.Entities.Person person = (UniquePersonV1.Entities.Person)DataFacadeManager.GetObjects(typeof(UniquePersonV1.Entities.Person), filterIdentificationInsured.GetPredicate()).First();
                    if (person != null)
                    {
                        insured.FullName = person.Name + " " + person.Surname + " " + person.MotherLastName;
                        insured.IdentificationDocument = person.IdCardNo;
                    }
                }
                else
                {
                    filterIdentificationInsured.Property(UniquePersonV1.Entities.Person.Properties.IndividualId, typeof(UniquePersonV1.Entities.Person).Name);
                    filterIdentificationInsured.Equal();
                    filterIdentificationInsured.Constant(individualId);
                    UniquePersonV1.Entities.Company company = (UniquePersonV1.Entities.Company)DataFacadeManager.GetObjects(typeof(UniquePersonV1.Entities.Company), filterIdentificationInsured.GetPredicate()).First();
                    if (company != null)
                    {
                        insured.FullName = company.TradeName;
                        insured.IdentificationDocument = company.TributaryIdNo;
                    }
                }
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetInsuredByIndividualId");
            return insured;
        }
        #region InsuredConcept
        public Models.InsuredConcept CreateInsuredConcept(Models.InsuredConcept insuredConcept)
        {
            //Filtro
            ObjectCriteriaBuilder filterInsuredConcept = new ObjectCriteriaBuilder();
            filterInsuredConcept.Property(UniquePersonV1.Entities.InsuredConcept.Properties.InsuredCode, typeof(UniquePersonV1.Entities.InsuredConcept).Name);
            filterInsuredConcept.Equal();
            filterInsuredConcept.Constant(insuredConcept.InsuredCode);

            UniquePersonV1.Entities.InsuredConcept insuredConceptEntity = null;

            insuredConceptEntity = EntityAssembler.CreateInsuredConcept(insuredConcept);
            DataFacadeManager.Insert(insuredConceptEntity);
            return ModelAssembler.CreateInsuredConcept(insuredConceptEntity);
        }

        public Models.InsuredConcept UpdateInsuredConcept(Models.InsuredConcept insuredConcept)
        {
            PrimaryKey key = InsuredConcept.CreatePrimaryKey(insuredConcept.InsuredCode);
            InsuredConcept insuredConceptEntity;

            insuredConceptEntity = (InsuredConcept)DataFacadeManager.GetObject(key);

            if (insuredConceptEntity == null)
            {
                CreateInsuredConcept(insuredConcept);
            }

            insuredConceptEntity = EntityAssembler.CreateInsuredConcept(insuredConcept);

            DataFacadeManager.Update(insuredConceptEntity);

            return ModelAssembler.CreateInsuredConcept(insuredConceptEntity);
        }


        #endregion


        
        #region ElectronicBilling
         public Models.Insured GetInsuredElectronicBillingByIndividualId(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePersonV1.Entities.Insured.Properties.IndividualId, typeof(UniquePersonV1.Entities.Insured).Name);
            filter.Equal();
            filter.Constant(individualId);
            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(UniquePersonV1.Entities.Insured), filter.GetPredicate());
            Models.Insured insured = ModelAssembler.CreateInsureds(businessCollection).FirstOrDefault();
            return insured;
        }
            #endregion ElectronicBilling









            // Metodos que existen en el DAO
            //        public Models.Insured GetInsuredByIndividualCode(int individualCode)
            //        {
            //            Stopwatch stopWatch = new Stopwatch();
            //            stopWatch.Start();

            //            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            //            filter.Property(UniquePersonV1.Entities.Insured.Properties.InsuredCode, typeof(UniquePersonV1.Entities.Insured).Name);
            //            filter.Equal();
            //            filter.Constant(individualCode);
            //            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.Insured), filter.GetPredicate()));
            //            Models.Insured insured = ModelAssembler.CreateInsureds(businessCollection).FirstOrDefault();
            //            stopWatch.Stop();
            //            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetInsuredByIndividualId");
            //            return insured;
            //        }
            //        /// Obtener lista de asegurados
            //        /// </summary>
            //        /// <param name="description">Identificacion o nombre de asegurado</param>
            //        /// <returns></returns>
            //        public List<Models.Insured> GetInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
            //        {
            //            Stopwatch stopWatch = new Stopwatch();
            //            stopWatch.Start();
            //            List<Models.Insured> insureds = new List<Models.Insured>();
            //            int maxRows = 50;

            //            if (customerType.HasValue)
            //            {
            //                if (customerType.Value == CustomerType.Individual)
            //                {
            //                    insureds.AddRange(GetPersonsByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));
            //                    insureds.AddRange(GetCompaniesByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));
            //                }
            //                else
            //                {
            //                    insureds.AddRange(GetProspectsByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));
            //                }
            //            }
            //            else
            //            {
            //                insureds.AddRange(GetPersonsByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));
            //                insureds.AddRange(GetCompaniesByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));
            //                insureds.AddRange(GetProspectsByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));
            //            }

            //            if (insureds.Count == 1 && insureds[0].CustomerType == CustomerType.Individual)
            //            {
            //                CompanyNameDAO companyNameDAO = new CompanyNameDAO();
            //                insureds[0].CompanyName = companyNameDAO.GetNotificationAddressesByIndividualId(insureds[0].IndividualId, insureds[0].CustomerType).FirstOrDefault();

            //                PaymentMethodDAO paymentMethodDAO = new PaymentMethodDAO();
            //                insureds[0].PaymentMethod = paymentMethodDAO.GetPaymentMethodByIndividualId(insureds[0].IndividualId);
            //            }

            //            stopWatch.Stop();
            //            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetInsuredsByDescriptionInsuredSearchTypeCustomerType");
            //            return insureds;
            //        }

            //        public List<Models.Insured> GetCompaniesByDescriptionInsuredSearchTypeMaxRows(string description, InsuredSearchType insuredSearchType, int maxRows)
            //        {
            //            List<Insured> insureds = new List<Insured>();
            //            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            //            Int64 identificationNumber = 0;
            //            Int64.TryParse(description, out identificationNumber);

            //            if (identificationNumber > 0)
            //            {
            //                switch (insuredSearchType)
            //                {
            //                    case InsuredSearchType.IndividualId:
            //                        filter.Property(UniquePersonV1.Entities.Company.Properties.IndividualId, "c");
            //                        filter.Equal();
            //                        filter.Constant(identificationNumber);
            //                        break;
            //                    case InsuredSearchType.DocumentNumber:
            //                        filter.Property(UniquePersonV1.Entities.Company.Properties.TributaryIdNo, "c");
            //                        filter.Equal();
            //                        filter.Constant(identificationNumber.ToString());
            //                        break;
            //                }
            //            }
            //            else
            //            {
            //                filter.Property(UniquePersonV1.Entities.Company.Properties.TradeName, "c");
            //                filter.Like();
            //                filter.Constant(description.Trim() + "%");
            //            }

            //            SelectQuery selectQuery = new SelectQuery();
            //            selectQuery.MaxRows = maxRows;

            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Company.Properties.IndividualId, "c")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Company.Properties.TradeName, "c")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Company.Properties.EconomicActivityCode, "c")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Company.Properties.TributaryIdNo, "c")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Company.Properties.TributaryIdTypeCode, "c")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.CheckPayableTo, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.InsuredCode, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.EnteredDate, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.DeclinedDate, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.InsDeclinedTypeCode, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.Annotations, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.InsProfileCode, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.BranchCode, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.ModifyDate, "i")));

            //            Join join = new Join(new ClassNameTable(typeof(UniquePersonV1.Entities.Company), "c"), new ClassNameTable(typeof(UniquePersonV1.Entities.Insured), "i"), JoinType.Left);
            //            join.Criteria = (new ObjectCriteriaBuilder().Property(UniquePersonV1.Entities.Company.Properties.IndividualId, "c").Equal().Property(UniquePersonV1.Entities.Insured.Properties.IndividualId, "i").GetPredicate());

            //            selectQuery.Table = join;
            //            selectQuery.Where = filter.GetPredicate();

            //            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            //            {
            //                while (reader.Read())
            //                {
            //                    Models.Insured insured = new Models.Insured
            //                    {
            //                        CustomerType = CustomerType.Individual,
            //                        IndividualType = IndividualType.Company,
            //                        IndividualId = Convert.ToInt32(reader["IndividualId"]),
            //                        Name = reader["TradeName"].ToString(),
            //                        IdentificationDocument = new IdentificationDocument
            //                        {
            //                            Number = reader["TributaryIdNo"].ToString(),
            //                            DocumentType = new DocumentType
            //                            {
            //                                Id = Convert.ToInt32(reader["TributaryIdTypeCode"])
            //                            }
            //                        },
            //                        EconomicActivity = new EconomicActivity
            //                        {
            //                            Id = Convert.ToInt32(reader["EconomicActivityCode"])
            //                        }
            //                    };

            //                    if (reader["InsuredCode"] != null)
            //                    {
            //                        insured.InsuredCode = Convert.ToInt32(reader["InsuredCode"]);
            //                        //insured.Name = reader["CheckPayableTo"] == null ? "" : reader["CheckPayableTo"].ToString();
            //                        insured.EnteredDate = Convert.ToDateTime(reader["EnteredDate"]);
            //                        insured.DeclinedDate = (DateTime?)reader["DeclinedDate"];
            //                        insured.InsDeclinesType = Convert.ToInt32(reader["InsDeclinedTypeCode"]);
            //                        insured.Annotations = reader["Annotations"] == null ? null : reader["Annotations"].ToString();
            //                        insured.Profile = Convert.ToInt32(reader["InsProfileCode"]);
            //                        insured.BranchCode = Convert.ToInt32(reader["BranchCode"]);
            //                        insured.ModifyDate = (DateTime?)reader["ModifyDate"];
            //                    }

            //                    insureds.Add(insured);
            //                }
            //            }

            //            return insureds;
            //        }

            //        public List<Models.Insured> GetPersonsByDescriptionInsuredSearchTypeMaxRows(string description, InsuredSearchType insuredSearchType, int maxRows)
            //        {
            //            List<Insured> insureds = new List<Insured>();
            //            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            //            Int64 identificationNumber = 0;
            //            Int64.TryParse(description, out identificationNumber);

            //            if (identificationNumber > 0)
            //            {
            //                switch (insuredSearchType)
            //                {
            //                    case InsuredSearchType.IndividualId:
            //                        filter.Property(UniquePersonV1.Entities.Person.Properties.IndividualId, "p");
            //                        filter.Equal();
            //                        filter.Constant(identificationNumber);
            //                        break;
            //                    case InsuredSearchType.DocumentNumber:
            //                        filter.Property(UniquePersonV1.Entities.Person.Properties.IdCardNo, "p");
            //                        filter.Equal();
            //                        filter.Constant(identificationNumber.ToString());
            //                        break;
            //                }
            //            }
            //            else
            //            {
            //                string[] fullName = description.Trim().Split(' ');
            //                fullName = fullName.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            //                filter.Property(UniquePersonV1.Entities.Person.Properties.Surname, "p");
            //                filter.Like();
            //                filter.Constant(fullName[0] + "%");

            //                if (fullName.Length > 1)
            //                {
            //                    filter.And();
            //                    filter.OpenParenthesis();
            //                    filter.Property(UniquePersonV1.Entities.Person.Properties.MotherLastName, "p");
            //                    filter.Like();
            //                    filter.Constant(fullName[1] + "%");

            //                    filter.Or();
            //                    filter.Property(UniquePersonV1.Entities.Person.Properties.Name, "p");
            //                    filter.Like();
            //                    filter.Constant(fullName[1] + "%");
            //                    filter.CloseParenthesis();

            //                    if (fullName.Length > 2)
            //                    {
            //                        string name = "";
            //                        int cont = 0;
            //                        string space = "";
            //                        for (int i = 2; i < fullName.Length; i++)
            //                        {
            //                            if (cont > 0)
            //                            {
            //                                space = " ";
            //                            }
            //                            name += space + fullName[i];
            //                            cont++;
            //                        }

            //                        filter.And();
            //                        filter.Property(UniquePersonV1.Entities.Person.Properties.Name, "p");
            //                        filter.Like();
            //                        filter.Constant(name + "%");
            //                    }
            //                }
            //            }

            //            SelectQuery selectQuery = new SelectQuery();
            //            selectQuery.MaxRows = maxRows;

            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.IndividualId, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.Surname, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.MotherLastName, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.Name, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.EconomicActivityCode, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.IdCardNo, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.IdCardTypeCode, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.BirthDate, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.Gender, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.CheckPayableTo, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.InsuredCode, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.EnteredDate, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.DeclinedDate, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.InsDeclinedTypeCode, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.Annotations, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.InsProfileCode, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.BranchCode, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.ModifyDate, "i")));

            //            Join join = new Join(new ClassNameTable(typeof(UniquePersonV1.Entities.Person), "p"), new ClassNameTable(typeof(UniquePersonV1.Entities.Insured), "i"), JoinType.Left);
            //            join.Criteria = (new ObjectCriteriaBuilder().Property(UniquePersonV1.Entities.Person.Properties.IndividualId, "p").Equal().Property(UniquePersonV1.Entities.Insured.Properties.IndividualId, "i").GetPredicate());

            //            selectQuery.Table = join;
            //            selectQuery.Where = filter.GetPredicate();

            //            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            //            {
            //                while (reader.Read())
            //                {
            //                    Models.Insured insured = new Models.Insured
            //                    {
            //                        CustomerType = CustomerType.Individual,
            //                        IndividualType = IndividualType.Person,
            //                        IndividualId = Convert.ToInt32(reader["IndividualId"]),
            //                        SurName = reader["Surname"].ToString(),
            //                        SecondSurName = reader["MotherLastName"] == null ? "" : reader["MotherLastName"].ToString(),
            //                        Name = reader["Name"].ToString(),
            //                        IdentificationDocument = new IdentificationDocument
            //                        {
            //                            Number = reader["IdCardNo"].ToString(),
            //                            DocumentType = new DocumentType
            //                            {
            //                                Id = Convert.ToInt32(reader["IdCardTypeCode"])
            //                            }
            //                        },
            //                        EconomicActivity = new EconomicActivity
            //                        {
            //                            Id = Convert.ToInt32(reader["EconomicActivityCode"])
            //                        },
            //                        BirthDate = (DateTime?)reader["BirthDate"],
            //                        Gender = reader["Gender"].ToString()
            //                    };

            //                    if (reader["InsuredCode"] != null)
            //                    {
            //                        insured.InsuredCode = Convert.ToInt32(reader["InsuredCode"]);
            //                        //insured.Name = reader["CheckPayableTo"] == null ? "" : reader["CheckPayableTo"].ToString();
            //                        insured.EnteredDate = Convert.ToDateTime(reader["EnteredDate"]);
            //                        insured.DeclinedDate = (DateTime?)reader["DeclinedDate"];
            //                        insured.InsDeclinesType = Convert.ToInt32(reader["InsDeclinedTypeCode"]);
            //                        insured.Annotations = reader["Annotations"] == null ? null : reader["Annotations"].ToString();
            //                        insured.Profile = Convert.ToInt32(reader["InsProfileCode"]);
            //                        insured.BranchCode = Convert.ToInt32(reader["BranchCode"]);
            //                        insured.ModifyDate = (DateTime?)reader["ModifyDate"];
            //                    }

            //                    insureds.Add(insured);
            //                }
            //            }

            //            return insureds;
            //        }

            //        public List<Models.Insured> GetProspectsByDescriptionInsuredSearchTypeMaxRows(string description, InsuredSearchType insuredSearchType, int maxRows)
            //        {
            //            List<Insured> insureds = new List<Insured>();
            //            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            //            Int64 identificationNumber = 0;
            //            Int64.TryParse(description, out identificationNumber);

            //            if (identificationNumber > 0)
            //            {
            //                switch (insuredSearchType)
            //                {
            //                    case InsuredSearchType.IndividualId:
            //                        filter.Property(UniquePersonV1.Entities.Prospect.Properties.ProspectId, "p");
            //                        filter.Equal();
            //                        filter.Constant(identificationNumber);
            //                        break;
            //                    case InsuredSearchType.DocumentNumber:
            //                        filter.Property(UniquePersonV1.Entities.Prospect.Properties.IdCardNo, "p");
            //                        filter.Equal();
            //                        filter.Constant(identificationNumber.ToString());
            //                        filter.Or();
            //                        filter.Property(UniquePersonV1.Entities.Prospect.Properties.TributaryIdNo, "p");
            //                        filter.Equal();
            //                        filter.Constant(identificationNumber.ToString());
            //                        break;
            //                }
            //            }
            //            else
            //            {
            //                filter.Property(UniquePersonV1.Entities.Prospect.Properties.TradeName, "p");
            //                filter.Equal();
            //                filter.Constant(description.Trim() + "%");
            //                filter.Or();

            //                string[] fullName = description.Trim().Split(' ');
            //                fullName = fullName.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            //                filter.Property(UniquePersonV1.Entities.Prospect.Properties.Surname, "p");
            //                filter.Like();
            //                filter.Constant(fullName[0] + "%");

            //                if (fullName.Length > 1)
            //                {
            //                    filter.And();
            //                    filter.Property(UniquePersonV1.Entities.Prospect.Properties.MotherLastName, "p");
            //                    filter.Like();
            //                    filter.Constant(fullName[1] + "%");

            //                    if (fullName.Length > 2)
            //                    {
            //                        string name = "";

            //                        for (int i = 2; i < fullName.Length; i++)
            //                        {
            //                            name += fullName[i] + " ";
            //                        }

            //                        filter.And();
            //                        filter.Property(UniquePersonV1.Entities.Prospect.Properties.Name, "p");
            //                        filter.Like();
            //                        filter.Constant(name + "%");
            //                    }
            //                }
            //            }

            //            SelectQuery selectQuery = new SelectQuery();
            //            selectQuery.MaxRows = maxRows;

            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Prospect.Properties.ProspectId, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Prospect.Properties.IndividualTypeCode, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Prospect.Properties.TradeName, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Prospect.Properties.TributaryIdNo, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Prospect.Properties.TributaryIdTypeCode, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Prospect.Properties.Surname, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Prospect.Properties.MotherLastName, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Prospect.Properties.Name, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Prospect.Properties.IdCardNo, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Prospect.Properties.IdCardTypeCode, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Prospect.Properties.BirthDate, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Prospect.Properties.Gender, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Prospect.Properties.Street, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Prospect.Properties.PhoneNumber, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Prospect.Properties.EmailAddress, "p")));

            //            selectQuery.Table = new ClassNameTable(typeof(UniquePersonV1.Entities.Prospect), "p");
            //            selectQuery.Where = filter.GetPredicate();

            //            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            //            {
            //                while (reader.Read())
            //                {
            //                    Models.Insured insured = new Models.Insured
            //                    {
            //                        CustomerType = CustomerType.Prospect,
            //                        IndividualType = (IndividualType)Convert.ToInt32(reader["IndividualTypeCode"]),
            //                        IndividualId = Convert.ToInt32(reader["ProspectId"]),
            //                    };

            //                    insured.CompanyName = new CompanyName
            //                    {
            //                        Address = new Address
            //                        {
            //                            Description = reader["Street"] == null ? "" : reader["Street"].ToString()
            //                        },
            //                        Phone = new Phone
            //                        {
            //                            Description = reader["PhoneNumber"] == null ? "" : reader["PhoneNumber"].ToString()
            //                        },
            //                        Email = new Email
            //                        {
            //                            Description = reader["EmailAddress"] == null ? "" : reader["EmailAddress"].ToString()
            //                        }
            //                    };

            //                    if (insured.IndividualType == IndividualType.Person)
            //                    {
            //                        insured.SurName = reader["Surname"].ToString();
            //                        insured.SecondSurName = reader["MotherLastName"] == null ? "" : reader["MotherLastName"].ToString();
            //                        insured.Name = reader["Name"].ToString();

            //                        insured.IdentificationDocument = new IdentificationDocument
            //                        {
            //                            Number = reader["IdCardNo"] == null ? "" : reader["IdCardNo"].ToString(),
            //                            DocumentType = new DocumentType
            //                            {
            //                                Id = Convert.ToInt32(reader["IdCardTypeCode"])
            //                            }
            //                        };
            //                        insured.BirthDate = (DateTime?)reader["BirthDate"];
            //                        insured.Gender = reader["Gender"] == null ? "" : reader["Gender"].ToString();
            //                    }
            //                    else
            //                    {
            //                        insured.Name = reader["TradeName"].ToString();
            //                        insured.IdentificationDocument = new IdentificationDocument
            //                        {
            //                            Number = reader["TributaryIdNo"].ToString(),
            //                            DocumentType = new DocumentType
            //                            {
            //                                Id = Convert.ToInt32(reader["TributaryIdTypeCode"])
            //                            }
            //                        };
            //                    }

            //                    insureds.Add(insured);
            //                }
            //            }

            //            return insureds;
            //        }
            //        public Models.Insured GetInsuredByInsuredCode(int insuredCode)
            //        {
            //            Insured insured = new Insured();
            //            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            //            filter.Property(UniquePersonV1.Entities.Insured.Properties.InsuredCode, "i");
            //            filter.Equal();
            //            filter.Constant(insuredCode);

            //            SelectQuery selectQuery = new SelectQuery();

            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.InsuredCode, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.DeclinedDate, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Individual.Properties.IndividualId, "in")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Individual.Properties.EconomicActivityCode, "in")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Individual.Properties.IndividualTypeCode, "in")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Company.Properties.TradeName, "c")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Company.Properties.TributaryIdNo, "c")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Company.Properties.TributaryIdTypeCode, "c")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.Surname, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.MotherLastName, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.Name, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.IdCardNo, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.IdCardTypeCode, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.BirthDate, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.Gender, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.IndividualPaymentMethod.Properties.PaymentMethodCode, "pma")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.IndividualPaymentMethod.Properties.PaymentId, "pma")));

            //            Join join = new Join(new ClassNameTable(typeof(UniquePersonV1.Entities.Insured), "i")
            //                , new ClassNameTable(typeof(UniquePersonV1.Entities.Individual), "in"), JoinType.Inner);
            //            join.Criteria = (new ObjectCriteriaBuilder()
            //                .Property(UniquePersonV1.Entities.Insured.Properties.IndividualId, "i")
            //                .Equal().Property(UniquePersonV1.Entities.Company.Properties.IndividualId, "in")
            //                .GetPredicate());

            //            join = new Join(join, new ClassNameTable(typeof(UniquePersonV1.Entities.Company), "c"), JoinType.Left);
            //            join.Criteria = (new ObjectCriteriaBuilder()
            //                .Property(UniquePersonV1.Entities.Insured.Properties.IndividualId, "i")
            //                .Equal().Property(UniquePersonV1.Entities.Company.Properties.IndividualId, "c")
            //                .GetPredicate());

            //            join = new Join(join, new ClassNameTable(typeof(UniquePersonV1.Entities.Person), "p"), JoinType.Left);
            //            join.Criteria = (new ObjectCriteriaBuilder()
            //                .Property(UniquePersonV1.Entities.Insured.Properties.IndividualId, "i")
            //                .Equal().Property(UniquePersonV1.Entities.Person.Properties.IndividualId, "p")
            //                .GetPredicate());

            //            join = new Join(join, new ClassNameTable(typeof(UniquePersonV1.Entities.IndividualPaymentMethod), "pma"), JoinType.Left);
            //            join.Criteria = (new ObjectCriteriaBuilder()
            //                .Property(UniquePersonV1.Entities.Insured.Properties.IndividualId, "i")
            //                .Equal().Property(UniquePersonV1.Entities.IndividualPaymentMethod.Properties.IndividualId, "pma")
            //                .GetPredicate());

            //            selectQuery.Table = join;
            //            selectQuery.Where = filter.GetPredicate();

            //            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            //            {
            //                while (reader.Read())
            //                {

            //                    if (Convert.ToInt32(reader["IndividualTypeCode"].ToString()) == Convert.ToInt32(IndividualType.Company))
            //                    {
            //                        insured = new Models.Insured
            //                        {
            //                            CustomerType = CustomerType.Individual,
            //                            IndividualType = IndividualType.Company,
            //                            IndividualId = Convert.ToInt32(reader["IndividualId"]),
            //                            Name = reader["TradeName"].ToString(),
            //                            IdentificationDocument = new IdentificationDocument
            //                            {
            //                                Number = reader["TributaryIdNo"].ToString(),
            //                                DocumentType = new DocumentType
            //                                {
            //                                    Id = Convert.ToInt32(reader["TributaryIdTypeCode"])
            //                                }
            //                            },
            //                            EconomicActivity = new EconomicActivity
            //                            {
            //                                Id = Convert.ToInt32(reader["EconomicActivityCode"])
            //                            },

            //                            InsuredCode = Convert.ToInt32(reader["InsuredCode"]),
            //                            DeclinedDate = (DateTime?)reader["DeclinedDate"],
            //                            PaymentMethod = new IndividualPaymentMethod
            //                            {
            //                                Id = reader["PaymentMethodCode"] == null ? 0 : Convert.ToInt32(reader["PaymentMethodCode"]),
            //                                PaymentId = reader["PaymentId"] == null ? 0 : Convert.ToInt32(reader["PaymentId"])
            //                            }
            //                        };
            //                    }
            //                    else
            //                    {
            //                        insured = new Models.Insured
            //                        {
            //                            CustomerType = CustomerType.Individual,
            //                            IndividualType = IndividualType.Person,
            //                            IndividualId = Convert.ToInt32(reader["IndividualId"]),
            //                            SurName = reader["Surname"].ToString(),
            //                            SecondSurName = reader["MotherLastName"] == null ? "" : reader["MotherLastName"].ToString(),
            //                            Name = reader["Name"].ToString(),
            //                            IdentificationDocument = new IdentificationDocument
            //                            {
            //                                Number = reader["IdCardNo"].ToString(),
            //                                DocumentType = new DocumentType
            //                                {
            //                                    Id = Convert.ToInt32(reader["IdCardTypeCode"])
            //                                }
            //                            },
            //                            EconomicActivity = new EconomicActivity
            //                            {
            //                                Id = Convert.ToInt32(reader["EconomicActivityCode"])
            //                            },
            //                            BirthDate = (DateTime?)reader["BirthDate"],
            //                            Gender = reader["Gender"].ToString(),

            //                            InsuredCode = Convert.ToInt32(reader["InsuredCode"]),
            //                            DeclinedDate = (DateTime?)reader["DeclinedDate"],
            //                            PaymentMethod = new IndividualPaymentMethod
            //                            {
            //                                Id = reader["PaymentMethodCode"] == null ? 0 : Convert.ToInt32(reader["PaymentMethodCode"]),
            //                                PaymentId = reader["PaymentId"] == null ? 0 : Convert.ToInt32(reader["PaymentId"])
            //                            }
            //                        };
            //                        insured.Name = insured.Name + " " + insured.SecondSurName + " " + insured.SurName;
            //                    }
            //                }
            //            }

            //            if (insured.IndividualId > 0)
            //            {
            //                CompanyNameDAO companyNameDAO = new CompanyNameDAO();
            //                insured.CompanyName = companyNameDAO.GetNotificationAddressesByIndividualId(insured.IndividualId, insured.CustomerType).FirstOrDefault();

            //                return insured;
            //            }
            //            else
            //            {
            //                return null;
            //            }
            //        }

            //        public IdentificationDocument GetIdentificationDocumentByInsuredId(int insuredId)
            //        {
            //            Stopwatch stopWatch = new Stopwatch();
            //            stopWatch.Start();

            //            InsuredView view = new InsuredView();
            //            ViewBuilder builder = new ViewBuilder("InsuredView");
            //            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            //            filter.Property(UniquePersonV1.Entities.Insured.Properties.InsuredCode, typeof(UniquePersonV1.Entities.Insured).Name);
            //            filter.Equal();
            //            filter.Constant(insuredId);
            //            builder.Filter = filter.GetPredicate();
            //            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            //            int? docType = null;
            //            string docNum = string.Empty;

            //            if (view.Persons != null)
            //            {
            //                UniquePersonV1.Entities.Person person = (UniquePersonV1.Entities.Person)view.Persons.First();
            //                docType = person.IdCardTypeCode;
            //                docNum = person.IdCardNo;
            //            }
            //            else if (view.Companies != null)
            //            {
            //                UniquePersonV1.Entities.Company company = (UniquePersonV1.Entities.Company)view.Companies.First();
            //                docType = company.TributaryIdTypeCode;
            //                docNum = company.TributaryIdNo;
            //            }

            //            if (!docType.HasValue)
            //            {
            //                return new IdentificationDocument();
            //            }

            //            IdentificationDocument identificationDocument = new IdentificationDocument
            //            {
            //                DocumentType = new DocumentType { Id = docType.Value },
            //                Number = docNum
            //            };

            //            stopWatch.Stop();
            //            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetIdentificationDocumentByInsuredId");
            //            return identificationDocument;

            //        }

            //        public List<Models.InsuredSegment> GetInsuredSegment()
            //        {
            //            List<Models.InsuredSegment> baseInsuredSegments = new List<Models.InsuredSegment>();
            //            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.InsuredSegment)));

            //            foreach (UniquePersonV1.Entities.InsuredSegment item in businessCollection)
            //            {
            //                Models.InsuredSegment insuredSegment = new Models.InsuredSegment();
            //                insuredSegment.Id = item.InsSegmentCode;
            //                insuredSegment.Description = item.Description;
            //                baseInsuredSegments.Add(insuredSegment);
            //            }

            //            return baseInsuredSegments;
            //        }

            //        public List<Models.InsuredProfile> GetInsuredProfile()
            //        {
            //            List<Models.InsuredProfile> baseInsuredProfile = new List<Models.InsuredProfile>();

            //            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.InsuredProfile)));

            //            foreach (UniquePersonV1.Entities.InsuredProfile item in businessCollection)
            //            {
            //                Models.InsuredProfile insuredProfile = new Models.InsuredProfile();
            //                insuredProfile.IndividualId = item.InsProfileCode;
            //                insuredProfile.Description = item.Description;
            //                baseInsuredProfile.Add(insuredProfile);
            //            }

            //            return baseInsuredProfile;
            //        }

            //        public List<Models.Base.BaseInsuredMain> GetInsuredsByName(string stringFilter)
            //        {
            //            List<Models.Base.BaseInsuredMain> baseInsuredMain = new List<Models.Base.BaseInsuredMain>();
            //            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            //            int validator = 0;
            //            if (!int.TryParse(stringFilter, out validator))
            //            {
            //                filter.Property(UniquePersonV1.Entities.Person.Properties.Name, typeof(UniquePersonV1.Entities.Person).Name);
            //                filter.Equal();
            //                filter.Constant(stringFilter);
            //            }
            //            else
            //            {
            //                filter.Property(UniquePersonV1.Entities.Person.Properties.IdCardNo, typeof(UniquePersonV1.Entities.Person).Name);
            //                filter.Equal();
            //                filter.Constant(stringFilter);
            //            }


            //            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.Person), filter.GetPredicate()));

            //            foreach (UniquePersonV1.Entities.Person item in businessCollection)
            //            {
            //                Models.Base.BaseInsuredMain insuredMain = new Models.Base.BaseInsuredMain();
            //                insuredMain.IndividualId = item.IndividualId;
            //                insuredMain.FullName = item.Name;
            //                baseInsuredMain.Add(insuredMain);
            //            }

            //            return baseInsuredMain;
            //        }

            //        #region Emision
            //        /// <summary>
            //        /// Gets the type of the i holders by description insured search type customer.
            //        /// </summary>
            //        /// <param name="description">The description.</param>
            //        /// <param name="insuredSearchType">Type of the insured search.</param>
            //        /// <param name="customerType">Type of the customer.</param>
            //        /// <returns></returns>
            //        public List<Models.Insured> GetHoldersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
            //        {
            //            Stopwatch stopWatch = new Stopwatch();
            //            stopWatch.Start();
            //            List<Models.Insured> insureds = new List<Models.Insured>();
            //            int maxRows = 50;

            //            if (customerType.HasValue)
            //            {
            //                if (customerType.Value == CustomerType.Individual)
            //                {
            //                    var taskPerson = Task.Run(() =>
            //                    {
            //                        var insuredResult = GetHolderPersonsByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows);
            //                        DataFacadeManager.Dispose();
            //                        return insuredResult;
            //                    }
            //                    );
            //                    var taskcompany = Task.Run(() =>
            //                    {

            //                        var holderResult = GetHolderCompaniesByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows);
            //                        DataFacadeManager.Dispose();
            //                        return holderResult;
            //                    }
            //                    );
            //                    taskPerson.Wait();
            //                    if (taskPerson.Result != null && taskPerson.Result.Count > 0)
            //                    {
            //                        insureds.AddRange(taskPerson.Result);
            //                    }
            //                    taskcompany.Wait();
            //                    if (taskcompany.Result != null && taskcompany.Result.Count > 0)
            //                    {
            //                        insureds.AddRange(taskcompany.Result);
            //                    }
            //                }
            //                else
            //                {
            //                    insureds.AddRange(GetProspectsByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));
            //                }
            //            }
            //            else
            //            {
            //                var taskPerson = Task.Run(() =>
            //                {
            //                    var result = GetHolderPersonsByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows);
            //                    DataFacadeManager.Dispose();
            //                    return result;
            //                }
            //                );
            //                var taskcompany = Task.Run(() =>
            //                {
            //                    var result = GetHolderCompaniesByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows);
            //                    DataFacadeManager.Dispose();
            //                    return result;
            //                });
            //                var taskprospect = Task.Run(() =>
            //                {
            //                    var result = GetProspectsByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows);
            //                    DataFacadeManager.Dispose();
            //                    return result;
            //                });
            //                taskPerson.Wait();
            //                taskcompany.Wait();
            //                taskprospect.Wait();
            //                if (taskPerson.Result != null)
            //                {
            //                    insureds.AddRange(taskPerson.Result);
            //                }
            //                if (taskcompany.Result != null)
            //                {
            //                    insureds.AddRange(taskcompany.Result);
            //                }
            //                if (taskprospect.Result != null)
            //                {
            //                    insureds.AddRange(taskprospect.Result);
            //                }
            //            }

            //            if (insureds != null && insureds.Count == 1 && insureds[0].CustomerType == CustomerType.Individual)
            //            {
            //                CompanyNameDAO companyNameDAO = new CompanyNameDAO();
            //                insureds[0].CompanyName = companyNameDAO.GetHolderNotificationAddressesByIndividualId(insureds[0].IndividualId, insureds[0].CustomerType).FirstOrDefault();

            //                PaymentMethodDAO paymentMethodDAO = new PaymentMethodDAO();
            //                insureds[0].PaymentMethod = paymentMethodDAO.GetPaymentMethodByIndividualId(insureds[0].IndividualId);
            //            }
            //            stopWatch.Stop();
            //            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetInsuredsByDescriptionInsuredSearchTypeCustomerType");
            //            return insureds;
            //        }

            //        /// <summary>
            //        /// Gets the holder persons by description insured search type maximum rows.
            //        /// </summary>
            //        /// <param name="description">The description.</param>
            //        /// <param name="insuredSearchType">Type of the insured search.</param>
            //        /// <param name="maxRows">The maximum rows.</param>
            //        /// <returns></returns>
            //        public List<Models.Insured> GetHolderPersonsByDescriptionInsuredSearchTypeMaxRows(string description, InsuredSearchType insuredSearchType, int maxRows)
            //        {

            //            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            //            Int64 identificationNumber = 0;
            //            Int64.TryParse(description, out identificationNumber);

            //            if (identificationNumber > 0)
            //            {
            //                switch (insuredSearchType)
            //                {
            //                    case InsuredSearchType.IndividualId:
            //                        filter.Property(UniquePersonV1.Entities.Person.Properties.IndividualId, "p");
            //                        filter.Equal();
            //                        filter.Constant(identificationNumber);
            //                        break;
            //                    case InsuredSearchType.DocumentNumber:
            //                        filter.Property(UniquePersonV1.Entities.Person.Properties.IdCardNo, "p");
            //                        filter.Equal();
            //                        filter.Constant(identificationNumber.ToString());
            //                        break;
            //                }
            //            }
            //            else
            //            {
            //                string[] fullName = description.Trim().Split(' ');
            //                fullName = fullName.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            //                filter.Property(UniquePersonV1.Entities.Person.Properties.Surname, "p");
            //                filter.Like();
            //                filter.Constant(fullName[0] + "%");

            //                if (fullName.Length > 1)
            //                {
            //                    filter.And();
            //                    filter.OpenParenthesis();
            //                    filter.Property(UniquePersonV1.Entities.Person.Properties.MotherLastName, "p");
            //                    filter.Like();
            //                    filter.Constant(fullName[1] + "%");

            //                    filter.Or();
            //                    filter.Property(UniquePersonV1.Entities.Person.Properties.Name, "p");
            //                    filter.Like();
            //                    filter.Constant(fullName[1] + "%");
            //                    filter.CloseParenthesis();

            //                    if (fullName.Length > 2)
            //                    {
            //                        string name = "";
            //                        int cont = 0;
            //                        string space = "";
            //                        for (int i = 2; i < fullName.Length; i++)
            //                        {
            //                            if (cont > 0)
            //                            {
            //                                space = " ";
            //                            }
            //                            name += space + fullName[i];
            //                            cont++;
            //                        }

            //                        filter.And();
            //                        filter.Property(UniquePersonV1.Entities.Person.Properties.Name, "p");
            //                        filter.Like();
            //                        filter.Constant(name + "%");
            //                    }
            //                }
            //            }

            //            SelectQuery selectQuery = new SelectQuery();
            //            selectQuery.MaxRows = maxRows;

            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.IndividualId, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.Surname, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.MotherLastName, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.Name, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.IdCardNo, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.IdCardTypeCode, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.BirthDate, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Person.Properties.Gender, "p")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.InsuredCode, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.DeclinedDate, "i")));
            //            Join join = new Join(new ClassNameTable(typeof(UniquePersonV1.Entities.Person), "p"), new ClassNameTable(typeof(UniquePersonV1.Entities.Insured), "i"), JoinType.Left);
            //            join.Criteria = (new ObjectCriteriaBuilder().Property(UniquePersonV1.Entities.Person.Properties.IndividualId, "p").Equal().Property(UniquePersonV1.Entities.Insured.Properties.IndividualId, "i").GetPredicate());

            //            selectQuery.Table = join;
            //            selectQuery.Where = filter.GetPredicate();
            //            DataTable table = new DataTable();
            //            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            //            {
            //                table.Load(daf.Select(selectQuery));
            //            }
            //            if (table != null && table.Rows.Count > 0)
            //            {
            //                ConcurrentBag<Insured> insureds = new ConcurrentBag<Insured>();
            //                Parallel.For(0, table.Rows.Count, row =>
            //                {
            //                    var insured = new Models.Insured
            //                    {
            //                        CustomerType = CustomerType.Individual,
            //                        IndividualType = IndividualType.Person,
            //                        IndividualId = Convert.ToInt32(table.Rows[row]["IndividualId"]),
            //                        SurName = table.Rows[row]["Surname"].ToString(),
            //                        SecondSurName = table.Rows[row]["MotherLastName"] == null ? "" : table.Rows[row]["MotherLastName"].ToString(),
            //                        Name = table.Rows[row]["Name"].ToString(),
            //                        IdentificationDocument = new IdentificationDocument
            //                        {
            //                            Number = table.Rows[row]["IdCardNo"].ToString(),
            //                            DocumentType = new DocumentType
            //                            {
            //                                Id = Convert.ToInt32(table.Rows[row]["IdCardTypeCode"])
            //                            }
            //                        },
            //                        BirthDate = (DateTime?)table.Rows[row]["BirthDate"],
            //                        Gender = table.Rows[row]["Gender"].ToString()
            //                    };
            //                    if (!DBNull.Value.Equals(table.Rows[row]["InsuredCode"]))
            //                    {
            //                        insured.InsuredCode = Convert.ToInt32(table.Rows[row]["InsuredCode"]);
            //                        if (!DBNull.Value.Equals(table.Rows[row]["DeclinedDate"]))
            //                        {
            //                            insured.DeclinedDate = (DateTime?)table.Rows[row]["DeclinedDate"];
            //                        }

            //                    }
            //                    insureds.Add(insured);
            //                });
            //                return insureds.ToList();
            //            }
            //            else
            //            {
            //                return null;
            //            }
            //        }

            //        /// <summary>
            //        /// Gets the holder companies by description insured search type maximum rows.
            //        /// </summary>
            //        /// <param name="description">The description.</param>
            //        /// <param name="insuredSearchType">Type of the insured search.</param>
            //        /// <param name="maxRows">The maximum rows.</param>
            //        /// <returns></returns>
            //        public List<Models.Insured> GetHolderCompaniesByDescriptionInsuredSearchTypeMaxRows(string description, InsuredSearchType insuredSearchType, int maxRows)
            //        {
            //            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            //            Int64 identificationNumber = 0;
            //            Int64.TryParse(description, out identificationNumber);

            //            if (identificationNumber > 0)
            //            {
            //                switch (insuredSearchType)
            //                {
            //                    case InsuredSearchType.IndividualId:
            //                        filter.Property(UniquePersonV1.Entities.Company.Properties.IndividualId, "c");
            //                        filter.Equal();
            //                        filter.Constant(identificationNumber);
            //                        break;
            //                    case InsuredSearchType.DocumentNumber:
            //                        filter.Property(UniquePersonV1.Entities.Company.Properties.TributaryIdNo, "c");
            //                        filter.Equal();
            //                        filter.Constant(identificationNumber.ToString());
            //                        break;
            //                }
            //            }
            //            else
            //            {
            //                filter.Property(UniquePersonV1.Entities.Company.Properties.TradeName, "c");
            //                filter.Like();
            //                filter.Constant(description.Trim() + "%");
            //            }

            //            SelectQuery selectQuery = new SelectQuery();
            //            selectQuery.MaxRows = maxRows;

            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Company.Properties.IndividualId, "c")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Company.Properties.TradeName, "c")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Company.Properties.TributaryIdNo, "c")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Company.Properties.TributaryIdTypeCode, "c")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.CheckPayableTo, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.InsuredCode, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.EnteredDate, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.DeclinedDate, "i")));
            //            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePersonV1.Entities.Insured.Properties.InsDeclinedTypeCode, "i")));

            //            Join join = new Join(new ClassNameTable(typeof(UniquePersonV1.Entities.Company), "c"), new ClassNameTable(typeof(UniquePersonV1.Entities.Insured), "i"), JoinType.Left);
            //            join.Criteria = (new ObjectCriteriaBuilder().Property(UniquePersonV1.Entities.Company.Properties.IndividualId, "c").Equal().Property(UniquePersonV1.Entities.Insured.Properties.IndividualId, "i").GetPredicate());

            //            selectQuery.Table = join;
            //            selectQuery.Where = filter.GetPredicate();

            //            DataTable table = new DataTable();
            //            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            //            {
            //                table.Load(daf.Select(selectQuery));
            //            }
            //            if (table != null && table.Rows.Count > 0)
            //            {
            //                ConcurrentBag<Insured> insureds = new ConcurrentBag<Insured>();
            //                Parallel.For(0, table.Rows.Count, row =>
            //                {
            //                    var insured = new Models.Insured
            //                    {
            //                        CustomerType = CustomerType.Individual,
            //                        IndividualType = IndividualType.Company,
            //                        IndividualId = Convert.ToInt32(table.Rows[row]["IndividualId"]),
            //                        Name = table.Rows[row]["TradeName"].ToString(),
            //                        IdentificationDocument = new IdentificationDocument
            //                        {
            //                            Number = table.Rows[row]["TributaryIdNo"].ToString(),
            //                            DocumentType = new DocumentType
            //                            {
            //                                Id = Convert.ToInt32(table.Rows[row]["TributaryIdTypeCode"])
            //                            }
            //                        }
            //                    };
            //                    if (!DBNull.Value.Equals(table.Rows[row]["InsuredCode"]))
            //                    {
            //                        insured.InsuredCode = Convert.ToInt32(table.Rows[row]["InsuredCode"]);
            //                        if (!DBNull.Value.Equals(table.Rows[row]["DeclinedDate"]))
            //                        {
            //                            insured.DeclinedDate = (DateTime?)table.Rows[row]["DeclinedDate"];
            //                        }

            //                    }
            //                    insureds.Add(insured);
            //                });
            //                return insureds.ToList();
            //            }
            //            else
            //            {
            //                return null;
            //            }
            //        }


            public List<Models.InsuredSegment> GetInsuredSegment()
        {
            List<Models.InsuredSegment> insuredSegments = new List<Models.InsuredSegment>();
            var business = DataFacadeManager.GetObjects(typeof(UniquePersonV1.Entities.InsuredSegment));

            foreach (UniquePersonV1.Entities.InsuredSegment item in business)
            {
                Models.InsuredSegment insuredSegment = new Models.InsuredSegment();
                insuredSegment.Id = item.InsSegmentCode;
                insuredSegment.Description = item.Description;
                insuredSegments.Add(insuredSegment);
            }

            return insuredSegments;
        }

        public List<Models.InsuredProfile> GetInsuredProfile()
        {
            List<Models.InsuredProfile> insuredProfiles = new List<Models.InsuredProfile>();

            var business = DataFacadeManager.GetObjects(typeof(UniquePersonV1.Entities.InsuredProfile));

            foreach (UniquePersonV1.Entities.InsuredProfile item in business)
            {
                Models.InsuredProfile insuredProfile = new Models.InsuredProfile();
                insuredProfile.Id = item.InsProfileCode;
                insuredProfile.Description = item.Description;
                insuredProfiles.Add(insuredProfile);
            }

            return insuredProfiles;
        }

    }
}
