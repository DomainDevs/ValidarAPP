using Sistran.Company.Application.SarlaftBusinessServices.Models;
using Sistran.Company.Application.SarlaftBusinessServicesProvider.Assemblers;
using Sistran.Company.Application.UniquePerson.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UPCEN = Sistran.Company.Application.UniquePerson.Entities;

namespace Sistran.Company.Application.SarlaftBusinessServicesProvider.Business
{
    public class CoSarlaftBusiness
    {
        public CompanyCoSarlaft CreateCoSarlaft(CompanyCoSarlaft CoSarlaft)
        {
            UPCEN.CoIndividualSarlaft entityCoSarlaft = EntityAssembler.CreateCoSarlaft(CoSarlaft);

            CompanyCoSarlaft CoSarlaftAux= this.GetCoSarlaftByIndividualId(CoSarlaft.IndividualId, CoSarlaft.SarlaftId);

            if (CoSarlaftAux != null && CoSarlaftAux.SarlaftId != null && CoSarlaftAux.SarlaftId > 0)
                DataFacadeManager.Update(entityCoSarlaft);
            else
               DataFacadeManager.Insert(entityCoSarlaft);

            return ModelAssembler.CreateCoSarlaft(entityCoSarlaft);

        }

      

        public CompanyCoSarlaft GetCoSarlaftByIndividualId(int individualId, int sarlaftId)
        {
            CompanyCoSarlaft companyCoSarlaft = new CompanyCoSarlaft();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            
            filter.Property(UPCEN.CoIndividualSarlaft.Properties.IndividualId, typeof(UPCEN.CoIndividualSarlaft).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(UPCEN.CoIndividualSarlaft.Properties.SarlaftId, typeof(UPCEN.CoIndividualSarlaft).Name);
            filter.Equal();
            filter.Constant(sarlaftId);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(CoIndividualSarlaft), filter.GetPredicate());
            companyCoSarlaft  = ModelAssembler.CreateCoSarlaft(businessCollection);

            return companyCoSarlaft;

        }

      

     
    }
}
