using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Diagnostics;
using MUp = Sistran.Core.Application.UniquePersonService.Models;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    public class OriginDAO
    {
        /// <summary>
        /// Obtener lista de tipos de origen
        /// </summary>
        public List<MUp.OriginType> GetOriginTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePerson.Entities.OriginType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetOriginTypes");
            return ModelAssembler.CreateOriginTypes(businessCollection);
        }
    }
}
