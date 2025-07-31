using Sistran.Core.Application.CommonServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using COMMML = Sistran.Core.Application.CommonService.Models;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    public class DefaultValueDAO
    {
        /// <summary>
        /// Busca valores por defecto de un usuario, modulo y submodulo
        /// </summary>
        /// <param name="defaultValue">defaultValue.</param>
        /// <returns>Lista de DefaultValue</returns>       
        public List<COMMML.DefaultValue> GetDefaultValueByDefaultValue(COMMML.DefaultValue defaultValue)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PARAMEN.DefaultValue.Properties.ModuleId);
            filter.Equal();
            filter.Constant(defaultValue.ModuleId);
            filter.And();
            filter.Property(PARAMEN.DefaultValue.Properties.SubmoduleId);
            filter.Equal();
            filter.Constant(defaultValue.SubModuleId);
            if (defaultValue.UserId != 0)
            {
                filter.And();
                filter.Property(PARAMEN.DefaultValue.Properties.UserId);
                filter.Equal();
                filter.Constant(defaultValue.UserId);
            }
            if (defaultValue.ProfileId != 0)
            {
                filter.And();
                filter.Property(PARAMEN.DefaultValue.Properties.ProfileId);
                filter.Equal();
                filter.Constant(defaultValue.ProfileId);
            }
            if (defaultValue.ViewName != null)
            {
                filter.And();
                filter.Property(PARAMEN.DefaultValue.Properties.ViewName);
                filter.Equal();
                filter.Constant(defaultValue.ViewName);
            }

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PARAMEN.DefaultValue), filter.GetPredicate()));

            List<COMMML.DefaultValue> listValues = ModelAssembler.CreateDefaultValues(businessCollection);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.DAOs.GetDefaultValueByDefaultValue");
            return listValues;
        }
    }
}
