using Sistran.Core.Application.Parameters.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    /// <summary>
    /// Unidades de Medida
    /// </summary>
    public class MeasurementTypeDAO
    {
        /// <summary>
        /// Obtiene las unidades de medida
        /// </summary>
        /// <returns> Listado de unidad de medida </returns>
        public List<Models.MeasurementType> GetMeasurementType()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(MeasurementType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetMeasurementType");
            return ModelAssembler.CreateMeasurementTypes(businessCollection);
        }
    }
}
