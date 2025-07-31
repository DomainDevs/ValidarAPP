using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    public class PersonTypeDAO
    {
        /// <summary>
        /// Obtener los tipos de persona
        /// </summary>
        /// <returns></returns>
        public List<Models.PersonType> GetPersonTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.PersonType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetPersonTypes");
            return ModelAssembler.CreatePersonTypes(businessCollection);
        }
    }
}
