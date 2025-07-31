using System.Collections.Generic;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Company.Application.MassiveServices.EEProvider.Entities.View;
using Sistran.Core.Framework.BAF;
using System;
using System.Diagnostics;


namespace Sistran.Company.Application.MassiveServices.EEProvider.DAOs
{
    public class CoPrefixDAO
    {

        /// <summary>
        /// Obtiene los ramos de un intermediario habilitados para solicitud agrupadora
        /// </summary>
        /// <param name="agentId"> Identificador del intermediario </param>
        /// <returns> Listado de ramos de un intermediario habilitados para solicitud agrupadora </returns>
        public List<Prefix> GetCoPrefixesByAgentId(int agentId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                CoPrefixRequestView view = new CoPrefixRequestView();
                ViewBuilder builder = new ViewBuilder("CoPrefixRequestView");

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(Sistran.Core.Application.UniquePersonService.Entities.AgentPrefix.Properties.IndividualId, typeof(Sistran.Core.Application.UniquePersonService.Entities.AgentPrefix).Name);
                filter.Equal();
                filter.Constant(agentId);
                filter.And();
                filter.Property(Entities.CoPrefixRequest.Properties.IsEnabled, typeof(Entities.CoPrefixRequest).Name);
                filter.Equal();
                filter.Constant(1);
                builder.Filter = filter.GetPredicate();

                MassiveServiceEEProvider.dataFacadeManager.GetDataFacade().FillView(builder, view);

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.MassiveServices.EEProvider.DAOs.GetCoPrefixesByAgentId");

                return Sistran.Core.Application.CommonService.Assemblers.ModelAssembler.CreatePrefixes(view.Prefixes);
            }
            catch (Exception exception)
            {
                throw new BusinessException("GetCoPrefixesByAgentId", exception);
            }
        }

    }
}
