using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Diagnostics;
using MUp = Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
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
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.OriginType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetOriginTypes");
            return ModelAssembler.CreateOriginTypes(businessCollection);
        }
    }
}
