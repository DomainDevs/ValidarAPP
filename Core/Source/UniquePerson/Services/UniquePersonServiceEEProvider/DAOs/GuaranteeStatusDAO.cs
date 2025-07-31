using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    /// <summary>
    ///  estados de contragarantías
    /// </summary>
    public class GuaranteeStatusDAO
    {
        /// <summary>
        /// Obtiene los estados de contragarantías
        /// </summary>
        /// <returns> Listado de contragarantías </returns>
        public List<Models.GuaranteeStatus> GetGuaranteeStatus()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(GuaranteeStatus)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetGuaranteeStatus");
            return ModelAssembler.CreateGuaranteesStatus(businessCollection);
        }
    }
}