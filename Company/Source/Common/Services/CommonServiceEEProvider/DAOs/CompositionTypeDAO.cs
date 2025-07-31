using Sistran.Company.Application.CommonService.Assemblers;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using System.Collections.Generic;
using ParametersEntities = Sistran.Core.Application.Parameters.Entities;
using Sistran.Company.Application.CommonServices.Models;
using System.Linq;

namespace Sistran.Company.Application.CommonServices.EEProvider.DAOs
{
    public class CompositionTypeDAO
    {
        private IDataFacade _dataFacade;
        public CompositionTypeDAO(IDataFacade dataFacade)
        {
            _dataFacade = dataFacade;
        }

        public List<CompositionType> GetCompositionTypes()
        {
            var businessCollection = new BusinessCollection(_dataFacade.SelectObjects(typeof(ParametersEntities.CompositionType)));
            return businessCollection.Select(x => ModelAssembler.CreateCompositionType((ParametersEntities.CompositionType)x)).ToList();
        }
    }
}
