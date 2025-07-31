using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using UP = Sistran.Core.Application.UniquePersonV1.Entities;
using PERMOD = Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    /// <summary>
    /// Tipos de Asociacion
    /// </summary>
    public class CoAssociationTypeDAO
    {
        /// <summary>
        /// Lista de tipos de asociación
        /// </summary>
        /// <returns></returns>
        public List<Models.AssociationType> GetAssociationTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.CoAssociationType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetAssociationTypes");
            return ModelAssembler.CreateAssociationTypes(businessCollection);
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
