using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using UP = Sistran.Core.Application.UniquePersonV1.Entities;
using PERMOD = Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class AssociationTypeBusiness
    {
        public List<Models.AssociationType> GetAssociationTypes()
        {
            return ModelAssembler.CreateAssociationTypes(DataFacadeManager.GetObjects(typeof(UniquePersonV1.Entities.CoAssociationType)));
        }
        /// <summary>
        /// Tipos de asociación por Id
        /// </summary>
        /// <param name="IndividualId"></param>
        /// <returns></returns>
        public PERMOD.CompanyExtended GetAssociationTypeByIndividualId(int IndividualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UP.CoCompany.Properties.IndividualId, typeof(UP.CoCompany).Name);
            filter.Equal();
            filter.Constant(IndividualId);
            BusinessCollection businessCollection = new BusinessCollection();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(UP.CoCompany), filter.GetPredicate()));
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UPService.Providers.GetAssociationTypeByIndividualId");
            //return ModelAssembler.CreateAssociationTypes(businessCollection).FirstOrDefault();
            return ModelAssembler.CreateCoCompanies(businessCollection).FirstOrDefault();
        }
    }
}