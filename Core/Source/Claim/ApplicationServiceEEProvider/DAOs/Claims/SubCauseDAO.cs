using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using CLMEN = Sistran.Core.Application.Claims.Entities;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class SubCauseDAO
    {
        public List<SubCause> GetSubCausesByCauseId(int CauseId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CLMEN.SubCause.Properties.CauseCode, typeof(CLMEN.SubCause).Name, CauseId);

            return ModelAssembler.CreateSubCause(DataFacadeManager.GetObjects(typeof(CLMEN.SubCause), filter.GetPredicate()));
        }

        public SubCause CreateSubCause(SubCause subCause)
        {
            CLMEN.SubCause entitySubCause = EntityAssembler.CreateSubCause(subCause);

            return ModelAssembler.CreateSubCause((CLMEN.SubCause)DataFacadeManager.Insert(entitySubCause));
        }

        public SubCause UpdateSubCause(SubCause subCause)
        {
            CLMEN.SubCause entitySubCause = EntityAssembler.CreateSubCause(subCause);
            DataFacadeManager.Update(entitySubCause);

            return ModelAssembler.CreateSubCause(entitySubCause);
        }

        public void DeleteSubCause(int subCauseId)
        {
            PrimaryKey primaryKey = CLMEN.SubCause.CreatePrimaryKey(subCauseId);

            DataFacadeManager.Delete(primaryKey);
        }
    }
}
