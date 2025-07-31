using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    /// <summary>
    /// Fecha Baja Asegurado
    /// </summary>
    public class InsuredDeclinedTypeDAO
    {
        /// <summary>
        /// Obtener Fecha Baja Asegurado
        /// </summary>
        /// <returns></returns>
        public List<Models.InsuredDeclinedType> GetInsuredDeclinedTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePerson.Entities.InsuredDeclinedType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetInsuredDeclinedTypes");
            return ModelAssembler.CreateInsuredDeclinedTypes(businessCollection);
        }

    }
}
