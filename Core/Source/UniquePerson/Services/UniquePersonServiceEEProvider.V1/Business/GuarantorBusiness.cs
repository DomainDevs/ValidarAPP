using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Linq;
using Models = Sistran.Core.Application.UniquePersonService.V1.Models;


namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class GuarantorBusiness
    {

        internal Models.Guarantor CreateGuarantor(Models.Guarantor Guarantor)
        {
            Guarantor entityGuarantor = EntityAssembler.CreateGuarantor(Guarantor);
            entityGuarantor.GuarantorId = CreateGuarantorId(Guarantor.IndividualId, Guarantor.GuaranteeId);
            DataFacadeManager.Insert(entityGuarantor);
            return ModelAssembler.CreateGuarantor(entityGuarantor);
        }

        internal Models.Guarantor UpdateGuarantor(Models.Guarantor Guarantor)
        {
            Guarantor entityGuarantor = EntityAssembler.CreateGuarantor(Guarantor);
            DataFacadeManager.Update(entityGuarantor);
            return ModelAssembler.CreateGuarantor(entityGuarantor);
        }

        internal void DeleteGuarantor(Models.Guarantor guarantor)
        {
            PrimaryKey primaryKey = Guarantor.CreatePrimaryKey(guarantor.IndividualId, guarantor.GuaranteeId, guarantor.GuarantorId);
            DataFacadeManager.Delete(primaryKey);
        }

        internal Models.Guarantor GetGuarantorByGuarantorId(int individualId, int guarantorId, int guaranteeId)
        {
            PrimaryKey primaryKey = Guarantor.CreatePrimaryKey(individualId, guaranteeId, guarantorId);
            Guarantor entityGuarantor = (Guarantor)DataFacadeManager.GetObject(primaryKey);
            return ModelAssembler.CreateGuarantor(entityGuarantor);
        }

        internal List<Models.Guarantor> GetGuarantorsByindividualIdByid(int individualId, int guaranteeId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(Guarantor.Properties.IndividualId, typeof(Guarantor).Name, individualId);
            filter.And();
            filter.PropertyEquals(Guarantor.Properties.GuaranteeId, typeof(Guarantor).Name, guaranteeId);
            return ModelAssembler.CreateGuarantors(DataFacadeManager.GetObjects(typeof(Guarantor), filter.GetPredicate()));
        }

        internal int CreateGuarantorId(int individualId, int guaranteeId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(Guarantor.Properties.IndividualId, typeof(Guarantor).Name, individualId);
            filter.And();
            filter.PropertyEquals(Guarantor.Properties.GuaranteeId, typeof(Guarantor).Name, guaranteeId);

            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(Guarantor), filter.GetPredicate());
            List<Models.Guarantor> guarantors = ModelAssembler.CreateGuarantors(businessObjects);

            if (guarantors == null || guarantors.Count == 0)
            {
                return 1;
            }
            else
            {
                return guarantors.Max(x => x.GuarantorId) + 1;
            }
        }

    }
}
