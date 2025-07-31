using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    /// <summary>
    /// Tipos de Asociacion
    /// </summary>
    public class CoAssociationTypeDAO
    {
        /// <summary>
        /// Lista de tipos de asociación
        /// </summary>
        /// <returns></returns>
        public List<Models.AssociationType> GetAssociationTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePerson.Entities.CoAssociationType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAssociationTypes");
            return ModelAssembler.CreateAssociationTypes(businessCollection);
        }
    }
}
