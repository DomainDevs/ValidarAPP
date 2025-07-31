using Sistran.Core.Application.UniquePersonService.V1.Enums;
using MOUP = Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using System.Diagnostics;
using System.Linq;
using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Application.UniquePersonService.V1.Entities.views;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Application.Common.Entities;
using Sistran.Co.Application.Data;
using CMM = Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Product.Entities;
using System;
using ENTV1 = Sistran.Core.Application.UniquePersonV1.Entities;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class AgentBusiness
    {
        #region CreateAgent

        #region agent

        public MOUP.Agent GetAgentByIndividualId(int individualId)
        {
            MOUP.Agent agentModel = null;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PrimaryKey key = Agent.CreatePrimaryKey(individualId);
            var business = (Agent)DataFacadeManager.GetObject(key);
            if (business == null)
                return agentModel;
            agentModel = ModelAssembler.CreateAgent(business);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetAgentByIndividualId");
            return agentModel;
        }
        public MOUP.Agent CreateAgent(MOUP.Agent agent)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Agent agentEntityAux = EntityAssembler.CreateAgent(agent);
            DataFacadeManager.Insert(agentEntityAux);
            MOUP.Agent agentModel = ModelAssembler.CreateAgent(agentEntityAux);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreateAgent");
            return agentModel;
        }
        public MOUP.Agent UpdateAgent(MOUP.Agent agent)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = Agent.CreatePrimaryKey(agent.IndividualId);

            var agentEntityAux = (Agent)DataFacadeManager.GetObject(key);

            if (agent.AgentDeclinedType != null)
            {
                agentEntityAux.AgentDeclinedTypeCode = String.IsNullOrEmpty(agent.AgentDeclinedType.Id.ToString()) ? null : (int?)agent.AgentDeclinedType.Id;
            }
            else
            {
                agentEntityAux.AgentDeclinedTypeCode = null;
            }
            if (agent.AgentType != null)
            {
                agentEntityAux.AgentTypeCode = agent.AgentType.Id;
            }
            if (agent.GroupAgent != null)
            {
                agentEntityAux.AgentGroupCode = String.IsNullOrEmpty(agent.GroupAgent.Id.ToString()) ? null : (int?)agent.GroupAgent.Id;
            }
            if (agent.SalesChannel != null)
            {
                agentEntityAux.SalesChannelCode = String.IsNullOrEmpty(agent.SalesChannel.Id.ToString()) ? null : (int?)agent.SalesChannel.Id;
            }
            if (agent.EmployeePerson != null)
            {
                agentEntityAux.AccExecutiveIndId = String.IsNullOrEmpty(agent.EmployeePerson.Id.ToString()) ? null : (int?)agent.EmployeePerson.Id;
            }
            else
            {
                agentEntityAux.AccExecutiveIndId = null;
            }

            agentEntityAux.CheckPayableTo = agent.FullName;
            agentEntityAux.EnteredDate = agent.DateCurrent;
            agentEntityAux.DeclinedDate = agent.DateDeclined;
            agentEntityAux.Annotations = agent.Annotations;
            agentEntityAux.ModifyDate = agent.DateModification;
            agentEntityAux.Locker = agent.Locker;
            agentEntityAux.CommissionDiscountAgreement = Convert.ToInt32(agent.CommissionDiscountAgreement);

            DataFacadeManager.Update(agentEntityAux);
            MOUP.Agent agentModel = ModelAssembler.CreateAgent(agentEntityAux);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.UpdateAgent");
            return agentModel;
        }
        #endregion

        #region CreateAgencyByInvidualId
        public List<MOUP.Agency> GetAgencyByInvidualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(AgentAgency.Properties.IndividualId, typeof(AgentAgency).Name);
            filter.Equal();
            filter.Constant(individualId);

            var businessCollection = DataFacadeManager.GetObjects(typeof(AgentAgency), filter.GetPredicate());

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetAgencyByInvidualId");
            return ModelAssembler.CreateAgencies(businessCollection);
        }
        public List<MOUP.Agency> GetActiveAgenciesByInvidualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(AgentAgency.Properties.IndividualId, typeof(AgentAgency).Name);
            filter.Equal();
            filter.Constant(individualId);
            //filter.And();
            //filter.Property(AgentAgency.Properties.AgentDeclinedTypeCode, typeof(AgentAgency).Name);
            //filter.IsNull();
            var  businessCollection =  DataFacadeManager.GetObjects(typeof(AgentAgency), filter.GetPredicate());
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetAgencyByInvidualId");
            return ModelAssembler.CreateAgencies(businessCollection);
        }
        public MOUP.Agency CreateCompanyAgency(MOUP.Agency agencies, int IndividualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            AgentAgency agencyEntity = null;
            agencyEntity = EntityAssembler.CreateAgency(agencies, IndividualId);
            DataFacadeManager.Insert(agencyEntity);
            MOUP.Agency result = ModelAssembler.CreateAgency(agencyEntity);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreateCompanyAgency");
            return result;
        }
        public MOUP.Agency UpdateCompanyAgency(MOUP.Agency agencies, int IndividualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = AgentAgency.CreatePrimaryKey(IndividualId, agencies.Id);
            var agencyEntityAux = (AgentAgency)DataFacadeManager.GetObject(key);
            int? AgentDeclinedTypeId = null;
            if (agencies.AgentDeclinedType != null)
            {
                AgentDeclinedTypeId = agencies.AgentDeclinedType.Id;
            }
            agencyEntityAux.AgentTypeCode = agencies.AgentType.Id;
            agencyEntityAux.AgentDeclinedTypeCode = AgentDeclinedTypeId;
            agencyEntityAux.DeclinedDate = agencies.DateDeclined;
            agencyEntityAux.BranchCode = agencies.Branch.Id;
            agencyEntityAux.Description = agencies.FullName;
            agencyEntityAux.AgentCode = agencies.Code;
            agencyEntityAux.Annotations = agencies.Annotations;
            DataFacadeManager.Update(agencyEntityAux);
            MOUP.Agency result = ModelAssembler.CreateAgency(agencyEntityAux);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.UpdateCompanyAgency");
            return result;
        }
        #endregion CreateAgencyByInvidualId

        #region AgentPrefix
        /// <summary>
        /// Obtener lista de ramos comerciales por agente
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns></returns>
        public List<BasePrefix> GetPrefixesByAgentId(int agentId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            AgentPrefixViewV1 view = new AgentPrefixViewV1();
            ViewBuilder builder = new ViewBuilder("AgentPrefixViewV1");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(AgentPrefix.Properties.IndividualId, typeof(AgentPrefix).Name);
            filter.Equal();
            filter.Constant(agentId);
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetPrefixesByAgentId");
            return ModelAssembler.CreatePrefixes(view.Prefixes);
        }

        /// <summary>
        /// Creates the agent prefix.
        /// </summary>
        /// <param name="agentPrefix">The agent prefix.</param>
        /// <param name="IndivualId">The indivual identifier.</param>
        /// <returns></returns>
        public BasePrefix CreateAgentPrefix(BasePrefix agentPrefix, int IndivualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            AgentPrefix agentPrefixEntityAux = EntityAssembler.CreateAgentPrefix(agentPrefix, IndivualId);
            DataFacadeManager.Insert(agentPrefixEntityAux);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreateAgentPrefix");
            return ModelAssembler.CreatePrefix(agentPrefixEntityAux);
        }

        /// <summary>
        /// Creates the agent prefix.
        /// </summary>
        /// <param name="agentPrefix">The agent prefix.</param>
        /// <param name="IndivualId">The indivual identifier.</param>
        /// <returns></returns>
        public BasePrefix UpdateteAgentPrefix(BasePrefix agentPrefix, int IndivualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = AgentPrefix.CreatePrimaryKey(agentPrefix.Id, IndivualId);
            AgentPrefix agentPrefixEntity = (AgentPrefix)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            AgentPrefix agentPrefixEntityAux = EntityAssembler.CreateAgentPrefix(agentPrefix, IndivualId);
            DataFacadeManager.Update(agentPrefixEntityAux);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.UpdateteAgentPrefix");
            return ModelAssembler.CreatePrefix(agentPrefixEntityAux);
        }

        /// <summary>
        /// Deletes the agent prefix by indivual identifier.
        /// </summary>
        /// <param name="IndivualId">The indivual identifier.</param>
        public void deleteAgentPrefixByIndivualId(int IndivualId)
        {
            var list = GetPrefixesByAgentId(IndivualId);

            foreach (CMM.Prefix item in list)
            {
                PrimaryKey key = AgentPrefix.CreatePrimaryKey(item.Id, IndivualId);
                var agentPrefixEntity = (AgentPrefix)DataFacadeManager.GetObject(key);
                if (agentPrefixEntity != null)
                {
                    DataFacadeManager.Delete(key);
                }
            }
        }

        public BasePrefix deleteAgentPrefix(BasePrefix prefix, int IndivualId)
        {
            NameValue[] parameters = new NameValue[2];
            parameters[0] = new NameValue("PREFIXCODE", prefix.Id);
            parameters[1] = new NameValue("INDIVIDUAL_ID", IndivualId);
            object result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPScalar("UP.DELETE_AGENT_PREFIX", parameters);
            }

            if (result!=null && int.Parse(result.ToString()) !=0)
            {
                PrimaryKey key = AgentPrefix.CreatePrimaryKey(prefix.Id, IndivualId);
                var agentPrefixEntity = (AgentPrefix)DataFacadeManager.GetObject(key);
                if (agentPrefixEntity != null)
                {
                    DataFacadeManager.Delete(key);
                }
                return null;
            }
            else
            {
                return prefix;
            }

        }
        #endregion

        #region ComisionesAgent
        // <summary>
        // Obtener lista de Comisiones por Intermediario por el individualId
        // </summary>
        // <param name="agentId">Id agente</param>
        // <returns></returns>
        public List<Models.Commission> GetAgentCommissionByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ENTV1.AgencyCommissRate.Properties.IndividualId, typeof(ENTV1.AgencyCommissRate).Name);
            filter.Equal();
            filter.Constant(individualId);

            var  businessCollection =  DataFacadeManager.GetObjects(typeof(ENTV1.AgencyCommissRate), filter.GetPredicate());

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetAgentCommissionByIndividualId");
            return ModelAssembler.CreateAgentCommissions(businessCollection);
        }
        /// <summary>
        /// Creates the CommissionAgent
        /// </summary>
        /// <param name="commissionAgent">The CommissionAgent.</param>
        /// <param name="IndividualId">The individual identifier.</param>
        /// <returns></returns>
        public MOUP.Commission CreateAgentCommission(MOUP.Commission commissionAgent, int IndividualId, int AgencyId)
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ENTV1.AgencyCommissRate agencyCommissionEntityAux = EntityAssembler.CreateAgentCommission(commissionAgent, IndividualId);
            DataFacadeManager.Insert(agencyCommissionEntityAux);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreateAgentCommission");
            return ModelAssembler.CreateAgentCommission(agencyCommissionEntityAux);
        }
        /// <summary>
        /// Creates the CommissionAgent
        /// </summary>
        /// <param name="commissionAgent">The CommissionAgent.</param>
        /// <param name="IndividualId">The individual identifier.</param>
        /// <returns></returns>
        public MOUP.Commission UpdateAgentCommission(MOUP.Commission commissionAgent, int IndividualId, int AgencyId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = ENTV1.AgencyCommissRate.CreatePrimaryKey(commissionAgent.Id);
            ENTV1.AgencyCommissRate agencyCommissionEntity = (ENTV1.AgencyCommissRate)DataFacadeManager.GetObject(key);
            agencyCommissionEntity.AgentAgencyId = AgencyId;
            agencyCommissionEntity.PrefixCode = commissionAgent.Prefix.Id;
            agencyCommissionEntity.LineBusinessCode = commissionAgent.LineBusiness.Id;
            agencyCommissionEntity.SubLineBusinessCode = commissionAgent.SubLineBusiness.Id;
            agencyCommissionEntity.StCommissPercentage = commissionAgent.PercentageCommission;
            agencyCommissionEntity.AdditCommissPercentage = commissionAgent.PercentageAdditional;
            DataFacadeManager.Update(agencyCommissionEntity);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.UpdateAgentCommission");
            return ModelAssembler.CreateAgentCommission(agencyCommissionEntity);

        }


        /// <summary>
        /// Obtener lista de Comisiones Id agente
        /// </summary>
        /// <param name="agencyId">Id agente</param>
        /// <returns></returns>
        public List<Models.Commission> GetAgentCommissionByAgencyId(int agencyId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            AgentCommissionViewV1 view = new AgentCommissionViewV1();
            ViewBuilder builder = new ViewBuilder("AgentCommissionViewV1");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ENTV1.AgencyCommissRate.Properties.AgentAgencyId, typeof(ENTV1.AgencyCommissRate).Name);
            filter.Equal();
            filter.Constant(agencyId);
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            List<Models.Commission> result = new List<Models.Commission>();

            var businessCollection = DataFacadeManager.GetObjects(typeof(ENTV1.AgencyCommissRate), filter.GetPredicate());

            List<Models.Commission> agentCommissions = ModelAssembler.CreateAgentCommissions(businessCollection);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetAgentCommissionByAgentId");
            return agentCommissions;
        }
        /// <summary>
        /// Elimina las comisiones por intermediario
        /// </summary>
        /// <param name="commissionAgent">Comision del intermediario</param>
        /// <returns></returns>
        public bool DeleteAgentCommission(MOUP.Commission commissionAgent)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PrimaryKey key = ENTV1.AgencyCommissRate.CreatePrimaryKey(commissionAgent.Id);
            var agencyCommissionEntity = (ENTV1.AgencyCommissRate)DataFacadeManager.GetObject(key);
            if (agencyCommissionEntity != null)
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(agencyCommissionEntity);
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.DeleteAgentCommission");
                return true;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.DeleteAgentCommission");
                return false;
            }

        }
        #endregion
        #endregion CreateAgent

        #region InsuredAgent
        public Models.InsuredAgent CreateInsuredAgent(Models.Agency agency, int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(InsuredAgent.Properties.InsuredIndId, typeof(InsuredAgent).Name);
            filter.Equal();
            filter.Constant(individualId);

            InsuredAgent entityInsuredAgent = null;

            entityInsuredAgent = EntityAssembler.CreateInsuredAgent(individualId, agency);
            entityInsuredAgent.IsMain = true;
            DataFacadeManager.Insert(entityInsuredAgent);
            return ModelAssembler.CreateInsuredAgent(entityInsuredAgent);
        }

        public Models.InsuredAgent UpdateInsuredAgent(Models.Agency insuredAgency, int individualId)
        {
            try
            {
                //aca se actualiza todas las agencias para que ninguna quede como principal
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(InsuredAgent.Properties.InsuredIndId, typeof(InsuredAgent).Name);
                filter.Equal();
                filter.Constant(individualId);
                var InsuredAgents = DataFacadeManager.GetObjects(typeof(InsuredAgent), filter.GetPredicate());
                foreach (InsuredAgent InsuredAgent in InsuredAgents)
                {
                    InsuredAgent.IsMain = false;
                    DataFacadeManager.Update(InsuredAgent);
                }

                //pregunto si ya existe una agencia para ese asegurado con ese id y ese agente
                PrimaryKey key = InsuredAgent.CreatePrimaryKey(individualId, insuredAgency.Agent.IndividualId, insuredAgency.Id);
                var insuredAgencyEntity = (InsuredAgent)DataFacadeManager.GetObject(key);
                if (insuredAgencyEntity != null)
                {
                    insuredAgencyEntity.IsMain = true;
                    DataFacadeManager.Update(insuredAgencyEntity);
                }
                else
                {
                    var insInsuredAgencyEntity = EntityAssembler.CreateInsuredAgent(individualId, insuredAgency);
                    insInsuredAgencyEntity.IsMain = true;
                    insuredAgencyEntity = (InsuredAgent) DataFacadeManager.Insert(insInsuredAgencyEntity);
                }

                return ModelAssembler.CreateInsuredAgent(insuredAgencyEntity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}