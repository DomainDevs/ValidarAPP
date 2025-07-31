using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class CatastrophicEventDAO
    {
        public List<Catastrophe> GetCatastrophes()
        {
            return ModelAssembler.CreateCatastrophes(DataFacadeManager.GetObjects(typeof(PARAMEN.Catastrophe)));
        }

        public List<Catastrophe> GetCatastrophesByDescription(string query)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PARAMEN.Catastrophe.Properties.Description, typeof(PARAMEN.Catastrophe).Name);
            filter.Like();
            filter.Constant(query + "%");

            return ModelAssembler.CreateCatastrophes(DataFacadeManager.GetObjects(typeof(PARAMEN.Catastrophe), filter.GetPredicate()));
        }
    }
}
