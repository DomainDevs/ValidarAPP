using Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Business
{
    class CoCoverageValueBusiness
    {
        /// <summary>
        /// GetApplicationBusinessCoverageValueByPrefixId: metodo que consulta listado de valores por cobertura tabla QUO.CO_COVERAGE_VALUE  partir del prefixId de busqueda simple
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        public List<ParamCoCoverageValue> GetApplicationBusinessCoverageValueByPrefixId(int prefixId)
        {
            try
            {
                CoCoverageValueDAO coCoverageDao = new CoCoverageValueDAO();
                return coCoverageDao.GetCoCoverageValueByPrefixId(prefixId);
            }          
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// CreateBusinessCoCoverage: metodod que inserta un registro tabla QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <param name="paramCoCoverageValue"></param>
        /// <returns></returns>
        public ParamCoCoverageValue CreateBusinessCoCoverage(ParamCoCoverageValue paramCoCoverageValue)
        {
            try
            {
                CoCoverageValueDAO coCoverageDao = new CoCoverageValueDAO();
                return coCoverageDao.CreateCoCoverageValue(paramCoCoverageValue);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetBusinessCoverageValueAdv: metodo que consulta listado de valores por cobertura tabla QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <param name="paramCoCoverageValue"></param>
        /// <returns></returns>
        public List<ParamCoCoverageValue> GetBusinessCoverageValueAdv(ParamCoCoverageValue paramCoCoverageValue)
        {
            try
            {
                CoCoverageValueDAO coCoverageDao = new CoCoverageValueDAO();
                return coCoverageDao.GetAdvCoCoverageValue(paramCoCoverageValue);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GenerateFileBusinessToCoverageValue: metodo que genera el archivo excel del listadode peso por cobertura tabla QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileBusinessToCoverageValue(string fileName)
        {
            CoCoverageValueDAO coCoverageDao = new CoCoverageValueDAO();
            return coCoverageDao.GenerateExcelCoCoverageValue(fileName);
        }

        /// <summary>
        /// GetAllCoCoverageValue: metodo que consulta el listado de los pesos por cobertura tabla QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <returns></returns>
        public List<ParamCoCoverageValue> GetAllCoCoverageValue()
        {
            try
            {
                CoCoverageValueDAO coCoverageDao = new CoCoverageValueDAO();
                return coCoverageDao.GetAllCoCoverageValue();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateBusinessCoCoverageValue: metodo que actualiza el campo  value_pje de la tabla QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <param name="paramCoCoverageValue"></param>
        /// <returns></returns>
        public ParamCoCoverageValue UpdateBusinessCoCoverageValue(ParamCoCoverageValue paramCoCoverageValue)
        {
            try
            {
                CoCoverageValueDAO coCoverageDao = new CoCoverageValueDAO();
                return coCoverageDao.UpdateCoCoverageValue(paramCoCoverageValue);
            }
            catch(Exception e)
            {
                throw new BusinessException(e.Message);
            }
            
        }

        /// <summary>
        /// DeleteBusinessCoCoverageValue: metodo que elimina un registro de la tabla QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <param name="paramCoCoverageValue"></param>
        /// <returns></returns>
        public string DeleteBusinessCoCoverageValue(ParamCoCoverageValue paramCoCoverageValue)
        {
            try { 
                CoCoverageValueDAO coCoverageDao = new CoCoverageValueDAO();
                bool result= coCoverageDao.DeleteCoCoverageValue(paramCoCoverageValue);
                return (result) ? "Ok" : "Error";
            }
            catch (Exception e)
            {
                throw new BusinessException(e.Message);
            }
        }

        public List<BaseParamCoverage> GetCoverageByPrefixId(int prefixId)
        {
            CoCoverageValueDAO coCoverageDao = new CoCoverageValueDAO();
            var result = coCoverageDao.GetCoverageByPrefixId(prefixId);
            return result;
          
        }
    }
}
