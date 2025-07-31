using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class ThirdBusiness
    {

        /// <summary>
        /// Obtener lista de tipos de terceros declinados
        /// </summary>
        public List<ThirdDeclinedType> GetSupplierDeclinedTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var businessCollection = DataFacadeManager.GetObjects(typeof(SupplierDeclinedType));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetProviderDeclinedType");
            return ModelAssembler.CreateThirdDeclinedTypes(businessCollection);
        }
    }
}
