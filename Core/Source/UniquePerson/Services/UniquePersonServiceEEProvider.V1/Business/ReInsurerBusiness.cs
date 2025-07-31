using Sistran.Core.Application.UniquePersonService.V1.Enums;
using MOUP= Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using System.Diagnostics;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class ReInsurerBusiness
    {
        public MOUP.ReInsurer GetReInsurerByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            MOUP.ReInsurer reInsurerModel = new MOUP.ReInsurer();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (individualId != 0)
            {
                filter.Property(Reinsurer.Properties.IndividualId);
                filter.Equal();
                filter.Constant(individualId);
            }
            var business =(Reinsurer)DataFacadeManager.GetObjects(typeof(Reinsurer), filter.GetPredicate()).FirstOrDefault();
            if(business != null)
            {
                reInsurerModel = ModelAssembler.CreateReinsurer(business);
            }            
            
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetCoInsurerByIndividualId");
            return reInsurerModel;
        }

        public MOUP.ReInsurer CreateReinsurer(MOUP.ReInsurer reinsurer)
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Reinsurer reinsurerEntity = EntityAssembler.CreateReinsurer(reinsurer);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(reinsurerEntity);
            MOUP.ReInsurer reinsurerModel = ModelAssembler.CreateReinsurer(reinsurerEntity);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreateReinsurer");
            return reinsurerModel;
           
        }

        public MOUP.ReInsurer UpdateReInsurer(MOUP.ReInsurer reinsurer)
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

           
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(Reinsurer.Properties.IndividualId);
            filter.Equal();
            filter.Constant(reinsurer.IndividualId);
            var entityReinsurer = (Reinsurer)DataFacadeManager.GetObjects(typeof(Reinsurer), filter.GetPredicate()).FirstOrDefault();

            entityReinsurer.Annotations = reinsurer.Annotations;
            entityReinsurer.DeclinedDate = reinsurer.DeclinedDate;
            entityReinsurer.DeclinedTypeCode = reinsurer.DeclaredTypeCD;
            entityReinsurer.EnteredDate = reinsurer.EnteredDate;
            entityReinsurer.ModifyDate = reinsurer.ModifyDate;
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityReinsurer);
            MOUP.ReInsurer reinsurerModel = ModelAssembler.CreateReinsurer(entityReinsurer);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.UpdateReinsurer");
            return reinsurerModel;
        }

    }
}