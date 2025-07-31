using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    /// <summary>
    /// Tipo de Empresa
    /// </summary>
    public class CompanyTypeDAO
    {
        /// <summary>
        /// Obtener Tipos de Empresa
        /// </summary>
        /// <returns>Tipos de Empresa</returns>
        public List<Models.CompanyType> GetCompanyTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.CompanyType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetCompanyTypes");
            return ModelAssembler.CreateCompanyTypes(businessCollection);
        }
    }
}