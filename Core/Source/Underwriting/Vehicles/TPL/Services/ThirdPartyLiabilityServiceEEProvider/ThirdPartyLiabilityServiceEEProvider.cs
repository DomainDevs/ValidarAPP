using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using System.ServiceModel;
using System.Collections.Generic;
using Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService.Models;
using System;
using Sistran.Core.Framework.BAF;

namespace Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ThirdPartyLiabilityServiceEEProvider : Vehicles.EEProvider.VehiclesEEProvider, IThirdPartyLiabilityServiceCore
    {
        /// <summary>
        /// Obtener trayectos disponibles
        /// </summary>
        /// <returns></returns>
        public List<Shuttle> GetShuttlesEnabled()
        {
            try
            {
                return ShuttleDAO.GetShuttlesEnabled();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }  
        }

        /// <summary>
        /// Obtener deducibles por producto
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <returns>Lista de deducibles</returns>
        public List<Deductible> GetDeductiblesByProductId(int productId)
        {
            try
            {
                ProductDeductibleDAO productDeductibleDAO = new ProductDeductibleDAO();
                return productDeductibleDAO.GetDeductiblesByProductId(productId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            } 
        }
    }
}
