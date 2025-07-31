using Sistran.Core.Application.Parameters.Entities;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Linq;
using Model = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class RiskCommercialClassDAO
    {
        /// <summary>
        /// Obtiene todos los RiskCommercialClass
        /// </summary>
        /// <returns>Lista de RiskCommercialClass</returns>
        public List<Model.RiskCommercialClass> GetRiskCommercialClass() {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(RiskCommercialClass)));
            return ModelAssembler.CreateRiskCommercialClass(businessCollection).OrderBy(x => x.Description).ToList();
        }
    }
}
