using Sistran.Core.Application.Location.PropertyServices.EEProvider.DAOs;
using Sistran.Core.Application.Location.PropertyServices.Models;
using Sistran.Core.Application.Locations.EEProvider;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;

namespace Sistran.Core.Application.Location.PropertyServices.EEProvider
{
    /// <summary>
    /// Proveedor de metodos
    /// </summary>
    public class PropertyServiceEEProvider : LocationsEEProvider, IPropertyServiceCore
    {
        public PropertyRisk GetRiskPropertyByRiskId(int riskId)
        {
            try
            {
                PropertyDAO propertyDAO = new PropertyDAO();
                return propertyDAO.GetRiskPropertyByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<PropertyRisk> GetRiskPropertiesByAddress(string adderess)
        {
            try
            {
                PropertyDAO propertyDAO = new PropertyDAO();
                return propertyDAO.GetRiskPropertiesByAddress(adderess);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<PropertyRisk> GetRiskPropertiesByEndorsementId(int endorsementId)
        {
            try
            {
                PropertyDAO propertyDAO = new PropertyDAO();
                return propertyDAO.GetRiskPropertiesByEndorsementId(endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<PropertyRisk> GetRiskPropertiesByInsuredId(int insuredId)
        {
            try
            {
                PropertyDAO propertyDAO = new PropertyDAO();
                return propertyDAO.GetRiskPropertiesByInsuredId(insuredId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

    }
}
