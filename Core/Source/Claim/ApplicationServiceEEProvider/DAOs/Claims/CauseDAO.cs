using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using CLMEN = Sistran.Core.Application.Claims.Entities;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class CauseDAO
    {
        public List<Cause> GetCausesByPrefixId(int prefixId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CLMEN.Cause.Properties.PrefixCode, typeof(CLMEN.Cause).Name, prefixId);

            return ModelAssembler.CreateCauses(DataFacadeManager.GetObjects(typeof(CLMEN.Cause), filter.GetPredicate()));
        }
    }
}
