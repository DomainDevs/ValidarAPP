using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    public class ComponentTypeDAO
    {
        /// <summary>
        /// Obtener ciudades por pais
        /// </summary>
        /// <param name="country">pais</param>
        /// <returns></returns>
        public List<ComponentType> GetComponentType()
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
          

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PARAMEN.ComponentType)));
            return ModelAssembler.CreateComponentTypes(businessCollection);
        }
    }
}
