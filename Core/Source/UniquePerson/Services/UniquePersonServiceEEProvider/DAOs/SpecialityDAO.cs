using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    public class SpecialityDAO
    {
        /// <summary>
        /// Obtener lista de especialidades
        /// </summary>
        /// <returns></returns>
        public List<Models.Speciality> GetSpecialties()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Speciality)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetSpecialties");
            return ModelAssembler.CreateSpecialties(businessCollection);
        }
        
        
    }
}
