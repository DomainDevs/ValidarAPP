using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Entities.views;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PRODENT = Sistran.Core.Application.Product.Entities;
using PersonENT = Sistran.Core.Application.UniquePersonV1.Entities;
using PersonBase = Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using Sistran.Core.Application.Product.Entities;
using ENTV1 = Sistran.Core.Application.UniquePersonV1.Entities;
using TP = Sistran.Core.Application.Utilities.Utility;


namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    public class CommissionAgentDao
    {

        /// <summary>
        /// Creates the CommissionAgent
        /// </summary>
        /// <param name="commissionAgent">The CommissionAgent.</param>
        /// <param name="IndividualId">The individual identifier.</param>
        /// <returns></returns>
        public Commission CreateAgentCommission(Commission commissionAgent, int IndividualId, int AgencyId)
        {
            PrimaryKey key = ENTV1.AgencyCommissRate.CreatePrimaryKey(commissionAgent.Id);
            ENTV1.AgencyCommissRate agencyCommissionEntity = (ENTV1.AgencyCommissRate)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            if (agencyCommissionEntity == null)
            {
                ENTV1.AgencyCommissRate agencyCommissionEntityAux = EntityAssembler.CreateAgentCommission(commissionAgent, IndividualId);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(agencyCommissionEntityAux);
                return ModelAssembler.CreateAgentCommission(agencyCommissionEntityAux);
            }
            else
            {
                agencyCommissionEntity.AgentAgencyId = AgencyId;
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
        public List<Models.Commission> GetAgentCommissionByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ENTV1.AgencyCommissRate.Properties.IndividualId, typeof(ENTV1.AgencyCommissRate).Name);
            filter.Equal();
            filter.Constant(individualId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ENTV1.AgencyCommissRate), filter.GetPredicate()));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetAgentCommissionByIndividualId");
            return ModelAssembler.CreateAgentCommissions(businessCollection);
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
        public bool DeleteAgentCommission(Commission commissionAgent)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PrimaryKey key = ENTV1.AgencyCommissRate.CreatePrimaryKey(commissionAgent.Id);
            ENTV1.AgencyCommissRate agencyCommissionEntity = (ENTV1.AgencyCommissRate)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
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
        /// <summary>
        /// Gets the agents commission by individual ids.
        /// </summary>
        /// <param name="IndividualIds">The individual ids.</param>
        /// <returns></returns>
        public async Task<List<Models.Commission>> GetAgentsCommissionByIndividualIds(List<PersonBase.BaseAgentAgency> IndividualIds, Int16 prefixId = -1)
        {
            try
            {
                if (IndividualIds == null)
                {
                    throw new ArgumentException("Parametro vacio");
                }
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                AgentCommissionViewV1 view = new AgentCommissionViewV1();
                ViewBuilder builder = new ViewBuilder("AgentCommissionViewV1");
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
                    filter.PropertyEquals(ENTV1.AgencyCommissRate.Properties.PrefixCode, typeof(ENTV1.AgencyCommissRate).Name, prefixId);
                }
                builder.Filter = filter.GetPredicate();
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.FillView(builder, view);
                }
                List<Models.Commission> agentCommissions = await TP.Task.Run(() => ModelAssembler.CreateAgentCommissions(view.AgentCommissions));
                var agentciesEntity = view.AgentAgencys.Cast<PersonENT.AgentAgency>().ToList();
                List<Models.Agency> agencies = await TP.Task.Run(() => ModelAssembler.CreateAgentcies(agentciesEntity));
                List<BasePrefix> prefix = await TP.Task.Run(() => ModelAssembler.CreatePrefixes(view.Prefixes));
                List<LineBusiness> lineBusiness = await TP.Task.Run(() => ModelAssembler.CreateLineBusinesses(view.LineBusinesss));
                List<SubLineBusiness> subLineBusiness = await TP.Task.Run(() => ModelAssembler.CreateSubLineBusinesses(view.SubLineBusinesss));

                if (agentCommissions != null)
                {
                    TP.Parallel.ForEach(agentCommissions, item =>
                   {
                       item.Prefix.Description = prefix?.First(x => x.Id == item.Prefix.Id)?.Description;
                       item.LineBusiness.Description = lineBusiness?.First(x => x.Id == item.LineBusiness.Id)?.Description;
                       item.SubLineBusiness.Description = subLineBusiness.First(x => x.Id == item.SubLineBusiness.Id)?.Description;
                   });
                    stopWatch.Stop();
                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetAgentCommissionByAgentId");
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

        /// <summary>
        /// Obtener lista de Comisiones Id agente
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns></returns>
        public List<Models.CommissionAgent> GetAgentCommissionByAgentId(int agentId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            AgentCommissionViewV1 view = new AgentCommissionViewV1();
            ViewBuilder builder = new ViewBuilder("AgentCommissionViewV1");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PRODENT.AgencyCommissRate.Properties.IndividualId, typeof(PRODENT.AgencyCommissRate).Name);
            filter.Equal();
            filter.Constant(agentId);
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            List<Models.CommissionAgent> result = new List<Models.CommissionAgent>();
            List<Models.CommissionAgent> agentCommissions = ModelAssembler.CreateAgentCommissionsAgents(view.AgentCommissions);
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
    }

}


