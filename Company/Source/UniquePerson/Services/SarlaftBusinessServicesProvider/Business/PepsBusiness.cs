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
    public class PepsBusiness
    {
        public CompanyIndvidualPeps CreatePeps(CompanyIndvidualPeps peps)
        {
            UPCEN.IndividualPeps entityPeps = EntityAssembler.CreatePeps(peps);

            CompanyIndvidualPeps pepsAux = this.GetPepsByIndividualId(peps.IndividualId, peps.SarlaftId);

            if (pepsAux != null && pepsAux.IndividualId != null && pepsAux.IndividualId > 0)
                DataFacadeManager.Update(entityPeps);
            else
                DataFacadeManager.Insert(entityPeps);


            
            return ModelAssembler.CreateIndividualPeps(entityPeps);

        }

        public CompanyIndvidualPeps GetPepsByIndividualId(int IndividualId, int sarlaftId)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UPCEN.IndividualPeps.Properties.IndividualId, typeof(UPCEN.IndividualPeps).Name);
            filter.Equal();
            filter.Constant(IndividualId);
            filter.And();
            filter.Property(UPCEN.IndividualPeps.Properties.SarlaftId, typeof(UPCEN.IndividualPeps).Name);
            filter.Equal();
            filter.Constant(sarlaftId);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(IndividualPeps), filter.GetPredicate());
            CompanyIndvidualPeps IndvidualPeps = ModelAssembler.CreateIndividualPeps(businessCollection);

           
            return IndvidualPeps;

        }

     
    }
}
