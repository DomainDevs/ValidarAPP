using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class InsuredGuaranteeDocumentationBusiness
    {
        internal Models.InsuredGuaranteeDocumentation CreateInsuredGuaranteeDocumentation(Models.InsuredGuaranteeDocumentation guarantee)
        {
            InsuredGuaranteeDocumentation entityInsuredGuaranteeDocumentation = EntityAssembler.CreateInsuredGuaranteeDocumentation(guarantee);
            DataFacadeManager.Insert(entityInsuredGuaranteeDocumentation);
            return ModelAssembler.CreateInsuredGuaranteeDocumentation(entityInsuredGuaranteeDocumentation);
        }

        internal Models.InsuredGuaranteeDocumentation UpdateInsuredGuaranteeDocumentation(Models.InsuredGuaranteeDocumentation guarantee)
        {
            InsuredGuaranteeDocumentation entityInsuredGuaranteeDocumentation = EntityAssembler.CreateInsuredGuaranteeDocumentation(guarantee);
            DataFacadeManager.Update(entityInsuredGuaranteeDocumentation);
            return ModelAssembler.CreateInsuredGuaranteeDocumentation(entityInsuredGuaranteeDocumentation);
        }

        internal void DeleteInsuredGuaranteeDocumentation(int individualId, int insuredguaranteeId, int guaranteeId, int documentId)
        {
            PrimaryKey primaryKey = InsuredGuaranteeDocumentation.CreatePrimaryKey(individualId, insuredguaranteeId, guaranteeId, documentId);
            DataFacadeManager.Delete(primaryKey);
        }

        internal Models.InsuredGuaranteeDocumentation GetInsuredGuaranteeDocumentationByInsuredGuaranteeDocumentationId(int individualId, int insuredguaranteeId, int guaranteeId, int documentId)
        {
            PrimaryKey primaryKey = InsuredGuaranteeDocumentation.CreatePrimaryKey(individualId, insuredguaranteeId, guaranteeId, documentId);
            InsuredGuaranteeDocumentation entityInsuredGuaranteeDocumentation = (InsuredGuaranteeDocumentation)DataFacadeManager.GetObject(primaryKey);
            return ModelAssembler.CreateInsuredGuaranteeDocumentation(entityInsuredGuaranteeDocumentation);
        }

        internal List<Models.InsuredGuaranteeDocumentation> GetInsuredGuaranteeDocumentation()
        {
            return ModelAssembler.CreateInsuredGuaranteeDocumentations(DataFacadeManager.GetObjects(typeof(InsuredGuaranteeDocumentation)));
        }

        public List<Models.InsuredGuaranteeDocumentation> GetInsuredGuaranteeDocumentation(int individualId, int guaranteeId)
        {
         
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePersonV1.Entities.InsuredGuaranteeDocumentation.Properties.IndividualId, typeof(UniquePersonV1.Entities.InsuredGuaranteeDocumentation).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(UniquePersonV1.Entities.InsuredGuaranteeDocumentation.Properties.GuaranteeId, typeof(UniquePersonV1.Entities.InsuredGuaranteeDocumentation).Name);
            filter.Equal();
            filter.Constant(guaranteeId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.InsuredGuaranteeDocumentation), filter.GetPredicate()));

            return ModelAssembler.CreateInsuredGuaranteeDocumentations(businessCollection);
        }


    }
}
