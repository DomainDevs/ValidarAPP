using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.CollectiveServices.Models;
using COLLEN = Sistran.Core.Application.Collective.Entities;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.CollectiveServices.EEProvider.Assemblers;

namespace Sistran.Core.Application.CollectiveServices.EEProvider.DAOs
{
    public class LoadTypeDAO
    {
        /// <summary>
        /// Obtener Tipos De Cargue Por Tipo De Endoso
        /// </summary>
        /// <param name="endosermentTypeId">Id Tipo De Endoso</param>
        /// <returns>Tipos De Cargue</returns>
        public List<LoadType> GetLoadTypesByEndosermentTypeId(int endosermentTypeId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COLLEN.LoadType.Properties.EndorsementTypeId, typeof(COLLEN.LoadType).Name).Equal().Constant(endosermentTypeId);
            
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COLLEN.LoadType), filter.GetPredicate()));

            return ModelAssembler.CreateLoadTypes(businessCollection);
        }        
    }
}