using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ModelIndividual = Sistran.Core.Application.UniquePersonServiceIndividual.Models;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.UniquePersonService.Providers
{
    public class IndividualTypeProvider
    {
        public List<ModelIndividual.IndividualType> GetIndividualTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PARAMEN.IndividualType)));
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetIndividualTypes");
            return ModelAssembler.CreateIndividualTypes(businessCollection);
        }


        /// <summary>
        /// Obtener la lista de compañias Coaseguradoras
        /// </summary>
        /// <returns></returns>
        public ModelIndividual.IndividualType GetIndividualTypeById(int individualTypeId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PARAMEN.IndividualType.Properties.IndividualTypeCode);
            filter.Equal();
            filter.Constant(individualTypeId);

            PARAMEN.IndividualType coIndividualType = (PARAMEN.IndividualType)DataFacadeManager.Instance.GetDataFacade().List(typeof(PARAMEN.IndividualType), filter.GetPredicate()).FirstOrDefault();
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetIndividualTypeById");
            return ModelAssembler.CreateIndividualType(coIndividualType);

        }
    }
}
