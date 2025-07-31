using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.UniquePersonService.Providers
{
    public class CoInsuranceCompanyProvider
    {
        /// <summary>
        /// Obtener la lista de compañias Coaseguradoras
        /// </summary>
        /// <returns></returns>
        public List<Models.CoInsuranceCompany> GetCoInsuranceCompanies()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.CoInsuranceCompany)));
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetCoInsuranceCompanies");
            return ModelAssembler.CreateCoInsuranceCompanies(businessCollection);
        }

        /// <summary>
        /// Obtener la lista de compañias Coaseguradoras
        /// </summary>
        /// <returns></returns>
        public Models.CoInsuranceCompany GetCoInsuranceCompanyByCoinsuranceId(int coInsuranceId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.CoInsuranceCompany.Properties.InsuranceCompanyId);
            filter.Equal();
            filter.Constant(coInsuranceId);

            COMMEN.CoInsuranceCompany coInsuranceCompany = (COMMEN.CoInsuranceCompany)DataFacadeManager.Instance.GetDataFacade().List(typeof(COMMEN.CoInsuranceCompany), filter.GetPredicate()).FirstOrDefault();
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetCoInsuranceCompanyByCoinsuranceId");
            return ModelAssembler.CreateCoInsuranceCompany(coInsuranceCompany);

        }
    }
}
