using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public static class PrefixEndoTypeEnabledDAO
    {
        /// <summary>
        /// Consulta los Tipos de endosos habilitados por ramo
        /// </summary>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <param name="isEnabled">Enabled</param>
        /// <returns> Listado de tipos de endoso </returns>
        public static List<PrefixEndoTypeEnabled> GetPrefixEndoEnabledByPrefixIdIsEnabled(int prefixId, bool isEnabled)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PARAMEN.PrefixEndoTypeEnabled.Properties.PrefixCode, typeof(PARAMEN.PrefixEndoTypeEnabled).Name);
            filter.Equal();
            filter.Constant(prefixId);
            filter.And();
            filter.Property(PARAMEN.PrefixEndoTypeEnabled.Properties.IsEnabled, typeof(PARAMEN.PrefixEndoTypeEnabled).Name);
            filter.Equal();
            filter.Constant(isEnabled);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PARAMEN.PrefixEndoTypeEnabled), filter.GetPredicate()));
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.DAOs.GetPrefixEndoEnabledByPrefixIdIsEnabled");
            return ModelAssembler.CreatePrefixEndoTypesEnabled(businessCollection);
        }
    }
}
