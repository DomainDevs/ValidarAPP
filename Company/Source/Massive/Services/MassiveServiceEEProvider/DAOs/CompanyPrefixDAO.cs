using System.Collections.Generic;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Company.Application.MassiveServices.EEProvider.Entities.View;
using UPEN = Sistran.Core.Application.UniquePerson.Entities;
using REQEN = Sistran.Company.Application.Request.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Company.Application.MassiveServices.EEProvider.DAOs
{
    public class CompanyPrefixDAO
    {
        /// <summary>
        /// Obtener Ramos Comerciales Por Agente
        /// </summary>
        /// <param name="agentId">Id Agente</param>
        /// <returns>Ramos Comerciales</returns>
        public List<Prefix> GetPrefixesByAgentId(int agentId)
        {
            CoPrefixRequestView view = new CoPrefixRequestView();
            ViewBuilder builder = new ViewBuilder("CoPrefixRequestView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UPEN.AgentPrefix.Properties.IndividualId, typeof(UPEN.AgentPrefix).Name);
            filter.Equal();
            filter.Constant(agentId);
            filter.And();
            filter.Property(REQEN.CoPrefixRequest.Properties.IsEnabled, typeof(REQEN.CoPrefixRequest).Name);
            filter.Equal();
            filter.Constant(1);
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            return Assemblers.ModelAssembler.CreatePrefixes(view.Prefixes);
        }

        /// <summary>
        /// Obtener los Ramos Comerciales Habilitados para Masivos
        /// </summary>
        /// <returns></returns>
        public List<Prefix> GetPrefixesToMassive()
        {
            CptPrefixView view = new CptPrefixView();
            ViewBuilder builder = new ViewBuilder("CptPrefixView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            //filter.Property(COMMEN.CptPrefix.Properties.IsMassive, typeof(COMMEN.CptPrefix).Name);
            filter.Equal();
            filter.Constant(1);

            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            return Assemblers.ModelAssembler.CreatePrefixes(view.Prefixes);
        }
    }
}