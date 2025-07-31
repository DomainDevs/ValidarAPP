using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    public class SocialLayerDAO
    {
        /// <summary>
        /// Obtener la lista de Estatos
        /// </summary>
        /// <returns></returns>
        public List<Models.SocialLayer> GetSocialLayers()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(SocialLayer)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetSocialLayers");
            return ModelAssembler.CreateSocialLayers(businessCollection);
        }
    }
}
