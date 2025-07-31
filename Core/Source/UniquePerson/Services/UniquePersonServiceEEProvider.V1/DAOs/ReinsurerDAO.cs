using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    public class ReinsurerDAO
    {
        public ReInsurer GetReInsurerByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (individualId != 0)
            {
                filter.Property(UPEN.Reinsurer.Properties.IndividualId);
                filter.Equal();
                filter.Constant(individualId);
            }
            BusinessCollection<UPEN.Reinsurer> businessCollection = new BusinessCollection<UPEN.Reinsurer>();
            businessCollection = DataFacadeManager.Instance.GetDataFacade().List<UPEN.Reinsurer>(filter.GetPredicate());
            ReInsurer reInsurerModel = null;
            if (businessCollection != null && businessCollection.Count > 0)
            {
                List<ReInsurer> lstCoInsurerCompanyModel = ModelAssembler.CreateReinsurer(businessCollection);
                if (lstCoInsurerCompanyModel != null)
                {
                    reInsurerModel = lstCoInsurerCompanyModel[0];
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.UniquePersonServices.DAOs.GetCoInsurerByIndividualId");

            return reInsurerModel;
        }

        public ReInsurer CreateReinsurer(ReInsurer reinsurer)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (reinsurer.IndividualId != 0)
            {
                filter.Property(UPEN.Reinsurer.Properties.IndividualId);
                filter.Equal();
                filter.Constant(reinsurer.IndividualId);
            }
            BusinessCollection<UPEN.Reinsurer> businessCollection = new BusinessCollection<UPEN.Reinsurer>();
            businessCollection = DataFacadeManager.Instance.GetDataFacade().List<UPEN.Reinsurer>(filter.GetPredicate());

            UPEN.Reinsurer reinsurerEntity = new UPEN.Reinsurer(reinsurer.IndividualId);

            if (businessCollection.Count == 0)
            {
                reinsurerEntity = EntityAssembler.CreateReinsurer(reinsurer);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(reinsurerEntity);
            }
            else
            {
                reinsurerEntity = businessCollection[0];

                reinsurerEntity.Annotations = reinsurer.Annotations;
                reinsurerEntity.DeclinedDate = reinsurer.DeclinedDate;
                reinsurerEntity.DeclinedTypeCode = reinsurer.DeclaredTypeCD;
                reinsurerEntity.ModifyDate = reinsurer.ModifyDate;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(reinsurerEntity);
            }

            reinsurer = ModelAssembler.CreateReinsurer(reinsurerEntity);
            return reinsurer;
        }

        public ReInsurer UpdateReInsurer(ReInsurer reinsurer)
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();


            Reinsurer reinsurerEntity = EntityAssembler.CreateReinsurer(reinsurer);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(reinsurerEntity);
            ReInsurer reinsurerModel = ModelAssembler.CreateReinsurer(reinsurerEntity);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.UpdateReinsurer");
            return reinsurerModel;
        }
    }
}
