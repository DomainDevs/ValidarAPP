using Sistran.Company.Application.CommonService.Assemblers;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using System.Collections.Generic;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Company.Application.CommonServices.EEProvider.DAOs
{
    public class SubLineBusinessDAO
    {
        private IDataFacade _dataFacade;
        public SubLineBusinessDAO(IDataFacade dataFacade)
        {
            _dataFacade = dataFacade;
        }
        public List<SubLineBusiness> GetSubLineBusiness()
        {
            var businessCollection = new BusinessCollection(_dataFacade.SelectObjects(typeof(COMMEN.SubLineBusiness)));
            return businessCollection.Select(x => ModelAssembler.CreateSubLineBusiness((COMMEN.SubLineBusiness)x)).ToList();
        }
    }
}
