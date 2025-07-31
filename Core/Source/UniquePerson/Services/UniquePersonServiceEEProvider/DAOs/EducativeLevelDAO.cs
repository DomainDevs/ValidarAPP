using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    /// <summary>
    /// Nivel Educativo
    /// </summary>
    public class EducativeLevelDAO
    {
        /// <summary>
        /// Obtener la lista de niveles educativos
        /// </summary>
        /// <returns></returns>
        public List<Models.EducativeLevel> GetEducativeLevels()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EducativeLevel)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetEducativeLevels");
            return ModelAssembler.CreateEducativelevels(businessCollection);
        }
    }
}
