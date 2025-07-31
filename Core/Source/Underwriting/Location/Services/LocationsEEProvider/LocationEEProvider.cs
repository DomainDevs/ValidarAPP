using Sistran.Core.Application.Locations.EEProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using UNDTO = Sistran.Core.Application.UnderwritingServices.DTOs;

namespace Sistran.Core.Application.Locations.EEProvider
{
    public class LocationsEEProvider : ILocations
    {
        /// <summary>
        /// Obtener lista de subfijos
        /// </summary>
        /// <returns></returns>
        public List<Models.Suffix> GetSuffixes()
        {
            try
            {
                NomenclatureAddressDAO nomenclatureAddressDAO = new NomenclatureAddressDAO();
                return nomenclatureAddressDAO.GetSuffixes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de subfijos
        /// </summary>
        /// <returns>lista de apartamento/Oficina</returns>
        public List<Models.ApartmentOrOffice> GetAparmentOrOffices()
        {
            try
            {
                NomenclatureAddressDAO nomenclatureAddressDAO = new NomenclatureAddressDAO();
                return nomenclatureAddressDAO.GetApartmentsOrOffices();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de subfijos
        /// </summary>
        /// <returns>Lista de tipos de rutas</returns>
        public List<Models.RouteType> GetRouteTypes()
        {
            try
            {
                NomenclatureAddressDAO nomenclatureAddressDAO = new NomenclatureAddressDAO();
                return nomenclatureAddressDAO.GetRouteTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de tipos de construccion
        /// </summary>
        /// <returns>Lista de tipos de construccion</returns>
        public List<Models.ConstructionType> GetConstructionTypes()
        {
            try
            {
                ConstructionTypeDAO constructionTypeDAO = new ConstructionTypeDAO();
                return constructionTypeDAO.GetConstructionTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de tipo de riesgo
        /// </summary>
        /// <returns>Lista de tipos de riesgos</returns>
        public List<RiskType> GetRiskTypes()
        {
            try
            {
                RiskTypeLocationDAO riskTypeLocationDAO = new RiskTypeLocationDAO();
                return riskTypeLocationDAO.GetRiskTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de uso del riesgo
        /// </summary>
        /// <returns> Lista de usos del riesgo</returns>
        public List<Models.RiskUse> GetRiskUses()
        {
            try
            {
                RiskUseEarthquakeDAO riskUseEarthquakeDAO = new RiskUseEarthquakeDAO();
                return riskUseEarthquakeDAO.GetRiskUses();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene polizas asociadas a la direccion
        /// </summary>
        /// <param name="street"></param>
        /// <returns></returns>
        public List<UNDTO.PolicyRiskDTO> GetPolicyRiskDTOsByStreet(string street)
        {
            try
            {
                PropertyDAO propertyDAO = new PropertyDAO();
                return propertyDAO.GetPolicyRiskDTOsByStreet(street);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}

