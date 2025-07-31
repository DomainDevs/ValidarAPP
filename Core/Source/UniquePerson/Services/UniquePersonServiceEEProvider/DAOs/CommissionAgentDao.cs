using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.UniquePersonService.Entities.views;
using Sistran.Core.Application.UniquePersonService.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Sistran.Core.Application.Product.Entities;
using PRODENT=Sistran.Core.Application.Product.Entities;
using PersonENT = Sistran.Core.Application.UniquePerson.Entities;
using PersonBase = Sistran.Core.Application.UniquePersonService.Models.Base;


namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    public class CommissionAgentDao
    {

        /// <summary>
        /// Creates the CommissionAgent
        /// </summary>
        /// <param name="commissionAgent">The CommissionAgent.</param>
        /// <param name="IndividualId">The individual identifier.</param>
        /// <returns></returns>
        public CommissionAgent CreateAgentCommission(CommissionAgent commissionAgent, int IndividualId)
        {
            PrimaryKey key = AgencyCommissRate.CreatePrimaryKey(commissionAgent.Id);
            AgencyCommissRate agencyCommissionEntity = (AgencyCommissRate)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            if (agencyCommissionEntity == null)
            {
                AgencyCommissRate agencyCommissionEntityAux = EntityAssembler.CreateAgentCommission(commissionAgent, IndividualId);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(agencyCommissionEntityAux);
                return ModelAssembler.CreateAgentCommission(agencyCommissionEntityAux);
            }
            else
            {
                agencyCommissionEntity.AgentAgencyId = commissionAgent.Agency.Id;
                agencyCommissionEntity.PrefixCode = commissionAgent.Prefix.Id;
                agencyCommissionEntity.LineBusinessCode = commissionAgent.LineBusiness.Id;
                agencyCommissionEntity.SubLineBusinessCode = commissionAgent.SubLineBusiness.Id;
                agencyCommissionEntity.StCommissPercentage = commissionAgent.PercentageCommission;
                agencyCommissionEntity.AdditCommissPercentage = commissionAgent.PercentageAdditional;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(agencyCommissionEntity);
                return ModelAssembler.CreateAgentCommission(agencyCommissionEntity);
            }
        }

        // <summary>
        // Obtener lista de Comisiones por Intermediario por el individualId
        // </summary>
        // <param name="agentId">Id agente</param>
        // <returns></returns>
        public List<Models.CommissionAgent> GetAgentCommissionByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(AgencyCommissRate.Properties.IndividualId, typeof(AgencyCommissRate).Name);
            filter.Equal();
            filter.Constant(individualId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(AgencyCommissRate), filter.GetPredicate()));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgentCommissionByIndividualId");
            return ModelAssembler.CreateAgentCommissions(businessCollection);
        }
        /// <summary>
        /// Obtener lista de Comisiones Id agente
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns></returns>
        public List<Models.CommissionAgent> GetAgentCommissionByAgentId(int agentId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            AgentCommissionView view = new AgentCommissionView();
            ViewBuilder builder = new ViewBuilder("AgentCommissionView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(AgencyCommissRate.Properties.IndividualId, typeof(AgencyCommissRate).Name);
            filter.Equal();
            filter.Constant(agentId);
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            List<Models.CommissionAgent> result = new List<Models.CommissionAgent>();
            List<Models.CommissionAgent> agentCommissions = ModelAssembler.CreateAgentCommissions(view.AgentCommissions);
            List<Models.Agency> agencies = ModelAssembler.CreateAgencies(view.AgentAgencys);
            List<BasePrefix> prefix = ModelAssembler.CreatePrefixes(view.Prefixes);
            List<LineBusiness> lineBusiness = ModelAssembler.CreateLineBusinesses(view.LineBusinesss);
            List<SubLineBusiness> subLineBusiness = ModelAssembler.CreateSubLineBusinesses(view.SubLineBusinesss);
            foreach (Models.CommissionAgent item in agentCommissions)
            {
                item.Agency.FullName = agencies.First(x => x.Id == item.Agency.Id).FullName;
                item.Prefix.Description = prefix.First(x => x.Id == item.Prefix.Id).Description;
                item.LineBusiness.Description = lineBusiness.First(x => x.Id == item.LineBusiness.Id).Description;
                item.SubLineBusiness.Description = subLineBusiness.First(x => x.Id == item.SubLineBusiness.Id).Description;

                result.Add(item);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgentCommissionByAgentId");
            return result;
        }
        /// <summary>
        /// Elimina las comisiones por intermediario
        /// </summary>
        /// <param name="commissionAgent">Comision del intermediario</param>
        /// <returns></returns>
        public bool DeleteAgentCommission(CommissionAgent commissionAgent)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PrimaryKey key = AgencyCommissRate.CreatePrimaryKey(commissionAgent.Id);
            AgencyCommissRate agencyCommissionEntity = (AgencyCommissRate)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            if (agencyCommissionEntity != null)
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(agencyCommissionEntity);
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.DeleteAgentCommission");
                return true;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.DeleteAgentCommission");
                return false;
            }

        }
        /// <summary>
        /// Gets the agents commission by individual ids.
        /// </summary>
        /// <param name="IndividualIds">The individual ids.</param>
        /// <returns></returns>
        public async Task<List<Models.CommissionAgent>> GetAgentsCommissionByIndividualIds(List<PersonBase.BaseAgentAgency> IndividualIds, Int16 prefixId = -1)
        {
            try
            {
                if (IndividualIds == null)
                {
                    throw new ArgumentException("Parametro vacio");
                }
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                AgentCommissionView view = new AgentCommissionView();
                ViewBuilder builder = new ViewBuilder("AgentCommissionView");
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(PersonENT.AgentAgency.Properties.IndividualId, typeof(PersonENT.AgentAgency).Name);
                filter.In();
                filter.ListValue();
                IndividualIds.AsParallel().ForAll(
                    x =>
                    {
                        filter.Constant(x.Id);
                    }
                    );
                filter.EndList();
                filter.And();
                filter.Property(PersonENT.AgentAgency.Properties.AgentAgencyId, typeof(PersonENT.AgentAgency).Name);
                filter.In();
                filter.ListValue();
                IndividualIds.AsParallel().ForAll(
                    x =>
                    {
                        filter.Constant(x.AgencyId);

                    }
                    );
                filter.EndList();
                if (prefixId != -1)
                {
                    filter.And();
                    filter.PropertyEquals(PRODENT.AgencyCommissRate.Properties.PrefixCode, typeof(PRODENT.AgencyCommissRate).Name, prefixId);
                }
                builder.Filter = filter.GetPredicate();
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.FillView(builder, view);
                }
                List<Models.CommissionAgent> agentCommissions = await Task.Run(() => ModelAssembler.CreateAgentCommissions(view.AgentCommissions));
                var agentciesEntity = view.AgentAgencys.Cast<PersonENT.AgentAgency>().ToList();
                List<Models.Agency> agencies = await Task.Run(() => ModelAssembler.CreateAgentcies(agentciesEntity));
                List<BasePrefix> prefix = await Task.Run(() => ModelAssembler.CreatePrefixes(view.Prefixes));
                List<LineBusiness> lineBusiness = await Task.Run(() => ModelAssembler.CreateLineBusinesses(view.LineBusinesss));
                List<SubLineBusiness> subLineBusiness = await Task.Run(() => ModelAssembler.CreateSubLineBusinesses(view.SubLineBusinesss));

                if (agentCommissions != null)
                {
                    Parallel.ForEach(agentCommissions, item =>
                   {
                       item.Agency.FullName = agencies?.First(x => x.Agent.IndividualId == item.IndividualId)?.FullName;
                       item.Prefix.Description = prefix?.First(x => x.Id == item.Prefix.Id)?.Description;
                       item.LineBusiness.Description = lineBusiness?.First(x => x.Id == item.LineBusiness.Id)?.Description;
                       item.SubLineBusiness.Description = subLineBusiness.First(x => x.Id == item.SubLineBusiness.Id)?.Description;
                   });
                    stopWatch.Stop();
                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAgentCommissionByAgentId");
                    return agentCommissions;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {

                throw;
            }

        }
    }

}


