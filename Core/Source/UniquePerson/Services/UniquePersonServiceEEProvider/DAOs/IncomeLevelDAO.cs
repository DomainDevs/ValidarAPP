using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{

    /// <summary>
    /// Nivel de ingresos de la persona
    /// </summary>
    public class IncomeLevelDAO
    {
        /// <summary>
        /// Obtener lista de Nivel de ingresos de la persona
        /// </summary>
        /// <returns></returns>
        public List<Models.IncomeLevel> GetIncomeLevels()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(IncomeLevel)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetIncomeLevels");
            return ModelAssembler.CreateIncomeLevels(businessCollection);
        }

    }
}
