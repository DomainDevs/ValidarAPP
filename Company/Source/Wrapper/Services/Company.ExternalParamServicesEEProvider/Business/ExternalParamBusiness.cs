using Sistran.Core.Application.UniquePersonService.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.ExternalParamServicesEEProvider.Business
{
    public class ExternalParamBusiness
    {
        public bool ValidatePrefixCd(int prefixCd)
        {
            try
            {
                var prefix = DelegateService.commonServiceCore.GetPrefixById(prefixCd);

                if (prefix != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw new BusinessException("Código de Ramo Comercial invalido.");
            }
        }

        public bool ValidateProductCd(int productCd)
        {
            try
            {
                var product = DelegateService.productServiceCore.GetProductById(productCd);

                if (product != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw new BusinessException("Código de Producto invalido.");
            }
        }

        public bool ValidateAssistanceCd(int assitance)
        {
            if (assitance > 0)
            {
                return true;
            }
            return false;
        }

        public List<Agency> GetAgency(int agentCd)
        {
            List<Agency> agencies = DelegateService.uniquePersonService.GetAgenciesByAgentIdDescription(agentCd, null);
            if (agencies.Count > 0)
            {
                return agencies;
            }
            else
            {
                throw new BusinessException("Código de Agente invalido.");
            }
        }

        public bool YearValid(int year)
        {
            if (year > 1900 && year < 10000)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool FasecoldaId(string fasecolda)
        {
            if (fasecolda.Length == 8)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
