using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    public class PromissoryNoteTypeDAO
    {

        /// <summary>
        /// Obtiene los tipos de pagaré
        /// </summary>
        /// <returns> Listado de tipos de pagaré </returns>
        public List<Models.PromissoryNoteType> GetPromissoryNoteType()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PromissoryNoteType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetPromissoryNoteType");
            return ModelAssembler.CreatePromissoryNoteTypes(businessCollection);
        }
    }
}
