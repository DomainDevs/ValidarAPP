using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{

    /// <summary>
    /// Obtiene Lista de Cargos que puede ocupar la persona
    /// </summary>
    public class OccupationDAO
    {
        /// <summary>
        /// Gets the occupations.
        /// </summary>
        /// <returns></returns>
        public List<Models.Occupation> GetOccupations()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Occupation)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetOccupations");
            return ModelAssembler.CreateOccupations(businessCollection);
        }
    }
}
