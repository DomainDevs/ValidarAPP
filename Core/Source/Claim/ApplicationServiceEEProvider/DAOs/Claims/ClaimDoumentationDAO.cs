using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class ClaimDoumentationDAO
    {
        public List<ClaimDocumentation> GetDocumentationBySubmodule(int submoduleId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(PARAMEN.Documentation.Properties.SubmoduleCode, typeof(PARAMEN.Documentation).Name);
            filter.Equal();
            filter.Constant(submoduleId);
            return ModelAssembler.CreateDocumentations(DataFacadeManager.GetObjects(typeof(PARAMEN.Documentation), filter.GetPredicate()));
        }

        public void DeleteDocumentation(int DocumentationId)
        {
            PrimaryKey primaryKey = PARAMEN.Documentation.CreatePrimaryKey(DocumentationId);
            DataFacadeManager.Delete(primaryKey);
        }

        public ClaimDocumentation UpdateDocumentation(ClaimDocumentation documentation)
        {
            PARAMEN.Documentation entityDocumentation = EntityAssembler.CreateDocumentation(documentation);
            DataFacadeManager.Update(entityDocumentation);

            return ModelAssembler.CreateDocumentation(entityDocumentation);
        }
        public ClaimDocumentation CreateDocumentation(ClaimDocumentation documentation)
        {
            PARAMEN.Documentation entityDocumentation = EntityAssembler.CreateDocumentation(documentation);
            DataFacadeManager.Insert(entityDocumentation);

            return ModelAssembler.CreateDocumentation(entityDocumentation);
        }
    }
}
