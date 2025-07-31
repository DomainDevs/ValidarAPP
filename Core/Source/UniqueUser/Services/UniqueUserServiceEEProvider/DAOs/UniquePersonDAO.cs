using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Assemblers;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using EtPerson = Sistran.Core.Application.UniquePerson.Entities;
namespace Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs
{
    public class UniquePersonDAO
    {
        /// <summary>
        /// Obtener cantidad Agencias Por Usuario
        /// </summary>
        /// <param name="userId">Id Usuario</param>
        /// <returns>numero agencias</returns>
        public int GetCountAgenciesByUserId(int userId)
        {
            int countAgencies = 0;
            SelectQuery select = new SelectQuery();
            Function function = new Function(FunctionType.Count);
            function.AddParameter(new Column(EtPerson.IndividualRelationApp.Properties.IndividualRelationAppId, typeof(EtPerson.IndividualRelationApp).Name));
            select.AddSelectValue(new SelectValue(function, "IndividualRelationApp"));

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(UniqueUser.Entities.UniqueUsers.Properties.UserId, typeof(UniqueUser.Entities.UniqueUsers).Name);
            filter.Equal();
            filter.Constant(userId);
            select.Where = filter.GetPredicate();

            Join join = new Join(new ClassNameTable(typeof(UniqueUser.Entities.UniqueUsers), typeof(UniqueUser.Entities.UniqueUsers).Name), new ClassNameTable(typeof(EtPerson.IndividualRelationApp), typeof(EtPerson.IndividualRelationApp).Name), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder().Property(UniqueUser.Entities.UniqueUsers.Properties.PersonId, typeof(UniqueUser.Entities.UniqueUsers).Name).Equal().Property(EtPerson.IndividualRelationApp.Properties.ParentIndividualId, typeof(EtPerson.IndividualRelationApp).Name).GetPredicate());
            select.Table = join;

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    if (reader[0] != null)
                    {
                        countAgencies = Convert.ToInt32(reader[0].ToString());
                    }
                }
            }
            return countAgencies;
        }

        public List<UserAgency> GetAgenciesByUserId(int userId)
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<UserAgency> agencies = new List<UserAgency>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.UniqueUsers.Properties.UserId, typeof(UniqueUser.Entities.UniqueUsers).Name);
            filter.Equal();
            filter.Constant(userId);

            AgencyIndividualRelationUUView agencyIndividualRelationView = new AgencyIndividualRelationUUView();
            ViewBuilder builder = new ViewBuilder("AgencyIndividualRelationUUView");

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, agencyIndividualRelationView);

            if (agencyIndividualRelationView.IndividualRelationsApp.Count > 0)
            {
                agencies = ModelAssembler.CreateAgencies(agencyIndividualRelationView.AgentAgencies);

                foreach (UserAgency agency in agencies)
                {
                    agency.Agent = ModelAssembler.CreateAgent(agencyIndividualRelationView.Agents.Cast<EtPerson.Agent>().First(x => x.IndividualId == agency.Agent.IndividualId));
                }
            }
            ///*uso cotizador liviano*/
            //else
            //{
            //    List<CoServiceQuotationParameter> quotationParameter = ListCoQuotationParameterBySourceCode(1);
            //    string agenCode = quotationParameter.Where(x => x.SourceCode == 1 && x.CoQuotationId == 4).First().ParameterValue;
            //    string agentTypeCode = quotationParameter.Where(x => x.SourceCode == 1 && x.CoQuotationId == 4).First().ParameterValue;
            //    var result = ModelAssembler.CreateAgency(GetAgentAgencyByParameters(agenCode, agentTypeCode));
            //    agencies.Add(result);

            //}

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs.GetAgenciesByUserId");
            return agencies;
        }

        public List<CoServiceQuotationParameter> ListCoQuotationParameterBySourceCode(int sourceCode)
        {
            List<CoServiceQuotationParameter> quotationParameter = new List<CoServiceQuotationParameter>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CoServiceQuotationParameter.Properties.SourceCode);
            filter.Equal();
            filter.Constant(sourceCode);
            var result = DataFacadeManager.GetObjects(typeof(CoServiceQuotationParameter), filter.GetPredicate()).Select(x => (CoServiceQuotationParameter)x).ToList();
            result.ForEach(x => quotationParameter.Add(x));
            return quotationParameter;

        }

        public EtPerson.AgentAgency GetAgentAgencyByParameters(string agentCode, string agentTypeCode)
        {
            EtPerson.AgentAgency agent;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(AgentAgency.Properties.AgentCode);
            filter.Equal();
            filter.Constant(int.Parse(agentCode));
            filter.And();
            filter.Property(AgentAgency.Properties.AgentTypeCode);
            filter.Equal();
            filter.Constant(int.Parse(agentTypeCode));
            agent = DataFacadeManager.GetObjects(typeof(EtPerson.AgentAgency), filter.GetPredicate()).Select(x => (EtPerson.AgentAgency)x).First();
            return agent;
        }



        /// <summary>
        /// Obtiene los Datos del Usuario que se encuentran
        /// en las tablas UNIQUE_USERS
        /// </summary>
        /// <param name="account">Nombre de la Cuenta (Account_Name)</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<User> GetUserPersonsByAccount(string account)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<User> userPerson = new List<User>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.UniqueUsers.Properties.AccountName, typeof(UniqueUser.Entities.UniqueUsers).Name);
            filter.Like();
            filter.Constant("%" + account + "%");

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.UniqueUsers), filter.GetPredicate()));

            userPerson = Assemblers.ModelAssembler.CreateUniqueUsers(businessCollection);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs.GetUserPersonsByAccount");
            return userPerson;
        }

        /// <summary>
        /// Gets the person by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public UserPerson GetPersonByUserIdOrPersonId(int userId, int personId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            UserPerson person = null;

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (personId != 0)
            {
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.PersonId, typeof(UniqueUser.Entities.UniqueUsers).Name);
                filter.Equal();
                filter.Constant(personId);
            }
            else
            {
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.UserId, typeof(UniqueUser.Entities.UniqueUsers).Name);
                filter.Equal();
                filter.Constant(userId);
            }

            PersonUUView view = new PersonUUView();
            ViewBuilder builder = new ViewBuilder("PersonUUView");
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            if (view.UniqueUser.Count > 0)
            {
                UserPerson userPerson = ModelAssembler.CreatePersons(view.People).First();
                userPerson.Emails = ModelAssembler.CreateEmails(view.Emails);

                return userPerson;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener Agencias Por Usuario
        /// </summary>
        /// <param name="userId">Id Usuario</param>
        /// <param name="agentId">Id Agente</param>
        /// <param name="description">Código o Nombre</param>
        /// <returns>Lista De Agencias</returns>
        public List<UserAgency> GetAgenciesByUserIdAgentIdDescription(int userId, int agentId, string description)
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<UserAgency> agencies = new List<UserAgency>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.UniqueUsers.Properties.UserId, typeof(UniqueUser.Entities.UniqueUsers).Name);
            filter.Equal();
            filter.Constant(userId);
            filter.And();

            if (agentId > 0)
            {
                filter.Property(EtPerson.Agent.Properties.IndividualId, typeof(EtPerson.Agent).Name);
                filter.Equal();
                filter.Constant(agentId);
                filter.And();
            }

            Int32 agencyCode = 0;
            Int32.TryParse(description, out agencyCode);

            if (agencyCode > 0)
            {
                filter.Property(EtPerson.AgentAgency.Properties.AgentCode, typeof(EtPerson.AgentAgency).Name);
                filter.Equal();
                filter.Constant(agencyCode);
            }
            else
            {
                filter.Property(EtPerson.Agent.Properties.CheckPayableTo, typeof(EtPerson.Agent).Name);
                filter.Like();
                filter.Constant("%" + description + "%");
            }

            AgencyIndividualRelationUUView agencyIndividualRelationView = new AgencyIndividualRelationUUView();
            ViewBuilder builder = new ViewBuilder("AgencyIndividualRelationUUView");

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, agencyIndividualRelationView);

            if (agencyIndividualRelationView.IndividualRelationsApp.Count > 0)
            {
                agencies = ModelAssembler.CreateAgencies(agencyIndividualRelationView.AgentAgencies);

                foreach (UserAgency agency in agencies)
                {
                    agency.Agent = ModelAssembler.CreateAgent(agencyIndividualRelationView.Agents.Cast<EtPerson.Agent>().First(x => x.IndividualId == agency.Agent.IndividualId));
                }
            }
            else
            {
                filter = new ObjectCriteriaBuilder();

                if (agentId > 0)
                {
                    filter.Property(EtPerson.Agent.Properties.IndividualId, typeof(EtPerson.Agent).Name);
                    filter.Equal();
                    filter.Constant(agentId);
                    filter.And();
                }

                if (agencyCode > 0)
                {
                    filter.Property(EtPerson.AgentAgency.Properties.AgentCode, typeof(EtPerson.AgentAgency).Name);
                    filter.Equal();
                    filter.Constant(agencyCode);
                }
                else
                {
                    filter.Property(EtPerson.Agent.Properties.CheckPayableTo, typeof(EtPerson.Agent).Name);
                    filter.Like();
                    filter.Constant("%" + description + "%");
                }

                AgentAgencyUUView agentAgencyView = new AgentAgencyUUView();
                builder = new ViewBuilder("AgentAgencyUUView");

                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, agentAgencyView);

                agencies = ModelAssembler.CreateAgencies(agentAgencyView.AgentAgencies);

                foreach (UserAgency agency in agencies)
                {
                    agency.Agent = ModelAssembler.CreateAgent(agentAgencyView.Agents.Cast<EtPerson.Agent>().First(x => x.IndividualId == agency.Agent.IndividualId));
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgenciesByUserIdAgentIdDescription");
            return agencies;
        }

        /// <summary>
        /// Obtener lista bitacora del asegurado y garantia
        /// </summary>
        /// <param name="individualId">individualId Asegurado</param>
        /// <param name="guaranteeId">id de garantia Asegurado</param>
        /// <returns>Listado de Bitacora de asegurado y garantia</returns>
        //public List<MlPerson.InsuredGuaranteeLog> GetInsuredGuaranteeLogs(int individualId, int guaranteeId)
        //{
        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();
        //    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //    filter.Property(EtPerson.InsuredGuaranteeLog.Properties.IndividualId, typeof(EtPerson.InsuredGuaranteeLog).Name);
        //    filter.Equal();
        //    filter.Constant(individualId);
        //    filter.And();
        //    filter.Property(EtPerson.InsuredGuaranteeLog.Properties.GuaranteeId, typeof(EtPerson.InsuredGuaranteeLog).Name);
        //    filter.Equal();
        //    filter.Constant(guaranteeId);

        //    BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(EtPerson.InsuredGuaranteeLog), filter.GetPredicate());

        //    stopWatch.Stop();
        //    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetInsuredGuaranteeLogs");
        //    return Assemblers.ModelAssembler.CreateInsuredGuaranteeLogs(businessCollection).OrderByDescending(x => x.LogDate).ToList();
        //}



        /// <summary>
        /// Obtener Agencias del usuario
        /// </summary>
        /// <param name="agentId">Id Agente</param>
        /// <param name="description">Código o Nombre</param>
        /// <returns>Agencias</returns>
        public List<UserAgency> GetAgenciesByAgentIdDescriptionUserId(int agentId, string description, int userId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //Obtener individualrelationapp por personId
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(UniqueUser.Entities.UniqueUsers.Properties.UserId);
            filter.Equal();
            filter.Constant(userId);

            UserIndividualRelation view = new UserIndividualRelation();
            ViewBuilder builder = new ViewBuilder("UserIndividualRelation");

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            filter = new ObjectCriteriaBuilder();

            if (agentId > 0)
            {
                filter.Property(EtPerson.Agent.Properties.IndividualId, typeof(EtPerson.Agent).Name);
                filter.Equal();
                filter.Constant(agentId);
                filter.And();
            }

            int agencyCode = 0;
            int.TryParse(description, out agencyCode);

            if (agencyCode > 0)
            {
                filter.Property(EtPerson.AgentAgency.Properties.AgentCode, typeof(EtPerson.AgentAgency).Name);
                filter.Equal();
                filter.Constant(agencyCode);
            }
            else
            {
                filter.Property(EtPerson.AgentAgency.Properties.Description, typeof(EtPerson.AgentAgency).Name);
                filter.Like();
                filter.Constant("%" + description + "%");

                filter.Or();
                filter.Property(EtPerson.Agent.Properties.CheckPayableTo, typeof(EtPerson.Agent).Name);
                filter.Like();
                filter.Constant("%" + description + "%");
            }

            List<UserAgent> agents = new List<UserAgent>();
            List<UserAgency> agencies = new List<UserAgency>();

            if (view.IndividualRelationsApp.Count > 0 || (agentId == 0 && description == ""))
            {
                filter.And();
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.UserId, typeof(UniqueUser.Entities.UniqueUsers).Name);
                filter.Equal();
                filter.Constant(userId);

                AgencyIndividualRelationUUView viewAgency = new AgencyIndividualRelationUUView();
                builder = new ViewBuilder("AgencyIndividualRelationUUView");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, viewAgency);
                agents = ModelAssembler.CreateAgents(viewAgency.Agents);
                agencies = ModelAssembler.CreateAgencies(viewAgency.AgentAgencies);
            }
            else
            {
                AgentAgencyUUView viewAgent = new AgentAgencyUUView();
                builder = new ViewBuilder("AgentAgencyUUView");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, viewAgent);
                agents = ModelAssembler.CreateAgents(viewAgent.Agents);
                agencies = ModelAssembler.CreateAgencies(viewAgent.AgentAgencies);
            }

            foreach (UserAgency item in agencies)
            {
                item.Agent.FullName = item.FullName + " - " + agents.FirstOrDefault(x => x.IndividualId == item.Agent.IndividualId).FullName;
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgenciesByAgentIdDescriptionUserId");
            return agencies;
        }


        /// <summary>
        /// Obtener lista de agencias Id agente
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns></returns>
        public List<UserAgency> GetAgenciesByAgentIdUserId(int agentId, int userId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //Obtener individualrelationapp por personId
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(UniqueUser.Entities.UniqueUsers.Properties.UserId);
            filter.Equal();
            filter.Constant(userId);

            UserIndividualRelation view = new UserIndividualRelation();
            ViewBuilder builder = new ViewBuilder("UserIndividualRelation");
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            filter = new ObjectCriteriaBuilder();
            filter.Property(EtPerson.AgentAgency.Properties.IndividualId, typeof(EtPerson.AgentAgency).Name);
            filter.Equal();
            filter.Constant(agentId);

            List<UserAgency> agencies = new List<UserAgency>();

            if (view.IndividualRelationsApp.Count > 0)
            {
                filter.And();
                filter.Property(UniqueUser.Entities.UniqueUsers.Properties.UserId, typeof(UniqueUser.Entities.UniqueUsers).Name);
                filter.Equal();
                filter.Constant(userId);

                AgencyIndividualRelationUUView viewAgency = new AgencyIndividualRelationUUView();
                builder = new ViewBuilder("AgencyIndividualRelationUUView");

                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, viewAgency);
                agencies = ModelAssembler.CreateAgencies(viewAgency.AgentAgencies);
            }
            else
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EtPerson.AgentAgency), filter.GetPredicate()));
                agencies = ModelAssembler.CreateAgencies(businessCollection);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgenciesByAgentId");
            return agencies;
        }


        public List<UserAgency> GetAgenciesByIndividualIdAgentId(string description)
        {
            List<UserAgency> userAgencies = new List<UserAgency>();
            UserAgency userAgency = new UserAgency();
            ObjectCriteriaBuilder filterCompany = new ObjectCriteriaBuilder();
            ObjectCriteriaBuilder filterPerson = new ObjectCriteriaBuilder();
            ObjectCriteriaBuilder filter1 = new ObjectCriteriaBuilder();

            filterCompany.PropertyEquals(EtPerson.Company.Properties.TributaryIdNo, typeof(EtPerson.Company).Name, description);
            BusinessCollection businessObjectsCompany = DataFacadeManager.GetObjects(typeof(EtPerson.Company), filterCompany.GetPredicate());

            filterPerson.PropertyEquals(EtPerson.Person.Properties.IdCardNo, typeof(EtPerson.Person).Name, description);
            BusinessCollection businessObjectsPerson = DataFacadeManager.GetObjects(typeof(EtPerson.Person), filterPerson.GetPredicate());

            if (businessObjectsCompany.Count > 0)
            {
                List<EtPerson.Company> companies = ModelAssembler.CreateCompanies(businessObjectsCompany);
                filter1.PropertyEquals(EtPerson.AgentAgency.Properties.IndividualId, typeof(EtPerson.AgentAgency).Name, companies.First().IndividualId);
                BusinessCollection businesses = DataFacadeManager.GetObjects(typeof(EtPerson.AgentAgency), filter1.GetPredicate());
                userAgencies = ModelAssembler.CreateAgencies(businesses);
            }
            else if (businessObjectsPerson.Count > 0)
            {
                List<EtPerson.Person> persons = ModelAssembler.CreatePeoples(businessObjectsPerson);
                filter1.PropertyEquals(EtPerson.AgentAgency.Properties.IndividualId, typeof(EtPerson.AgentAgency).Name, persons.First().IndividualId);
                BusinessCollection businesses = DataFacadeManager.GetObjects(typeof(EtPerson.AgentAgency), filter1.GetPredicate());
                userAgencies = ModelAssembler.CreateAgencies(businesses);
            }
            return userAgencies;
        }
    }
}
