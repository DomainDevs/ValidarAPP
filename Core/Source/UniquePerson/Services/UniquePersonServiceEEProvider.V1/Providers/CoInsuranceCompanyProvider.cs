using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.UniquePersonService.V1.Providers
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

    }
}
