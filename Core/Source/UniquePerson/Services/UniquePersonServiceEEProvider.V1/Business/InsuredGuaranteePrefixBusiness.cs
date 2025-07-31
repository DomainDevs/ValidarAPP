using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.UniquePersonV1.Entities;
using System.Collections.Generic;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class InsuredGuaranteePrefixBusiness
    {
        internal Models.InsuredGuaranteePrefix CreateInsuredGuaranteePrefix(Models.InsuredGuaranteePrefix InsuredGuaranteePrefix)
        {
            InsuredGuaranteePrefix entityInsuredGuaranteePrefix = EntityAssembler.CreateInsuredGuaranteePrefix(InsuredGuaranteePrefix);
            DataFacadeManager.Insert(entityInsuredGuaranteePrefix);
            return ModelAssembler.CreateInsuredGuaranteePrefix(entityInsuredGuaranteePrefix);
        }

        internal Models.InsuredGuaranteePrefix UpdateInsuredGuaranteePrefix(Models.InsuredGuaranteePrefix InsuredGuaranteePrefix)
        {
            InsuredGuaranteePrefix entityInsuredGuaranteePrefix = EntityAssembler.CreateInsuredGuaranteePrefix(InsuredGuaranteePrefix);
            DataFacadeManager.Update(entityInsuredGuaranteePrefix);
            return ModelAssembler.CreateInsuredGuaranteePrefix(entityInsuredGuaranteePrefix);
        }

        internal void DeleteInsuredGuaranteePrefix(int individualId, int guaranteeId, int InsuredGuaranteePrefixId)
        {
            PrimaryKey primaryKey = InsuredGuaranteePrefix.CreatePrimaryKey(individualId, guaranteeId, InsuredGuaranteePrefixId);
            DataFacadeManager.Delete(primaryKey);
        }

        internal Models.InsuredGuaranteePrefix GetInsuredGuaranteePrefixById(int individualId, int guaranteeId, int InsuredGuaranteePrefixId)
        {
            PrimaryKey primaryKey = InsuredGuaranteePrefix.CreatePrimaryKey(individualId, guaranteeId, InsuredGuaranteePrefixId);
            InsuredGuaranteePrefix entityInsuredGuaranteePrefix = (InsuredGuaranteePrefix)DataFacadeManager.GetObject(primaryKey);
            return ModelAssembler.CreateInsuredGuaranteePrefix(entityInsuredGuaranteePrefix);
        }

        internal List<Models.InsuredGuaranteePrefix> GetInsuredGuaranteePrefixs(int guaranteeId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(InsuredGuaranteePrefix.Properties.GuaranteeId, typeof(InsuredGuaranteePrefix).Name, guaranteeId);
            return ModelAssembler.CreateInsuredGuaranteePrefixes(DataFacadeManager.GetObjects(typeof(InsuredGuaranteePrefix), filter.GetPredicate()));
        }

        internal List<Models.InsuredGuaranteePrefix> CreateInsuredGuaranteePrefixByIndividualIdByGuaranteeId(int individualId, int guaranteeId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(InsuredGuaranteePrefix.Properties.IndividualId, typeof(InsuredGuaranteePrefix).Name, individualId);
            filter.And();
            filter.PropertyEquals(InsuredGuaranteePrefix.Properties.GuaranteeId, typeof(InsuredGuaranteePrefix).Name, guaranteeId);

            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(InsuredGuaranteePrefix), filter.GetPredicate());
            List<Models.InsuredGuaranteePrefix> InsuredGuaranteePrefixs = ModelAssembler.CreateInsuredGuaranteePrefixes(businessObjects);

            return InsuredGuaranteePrefixs;
        }

    }
}
