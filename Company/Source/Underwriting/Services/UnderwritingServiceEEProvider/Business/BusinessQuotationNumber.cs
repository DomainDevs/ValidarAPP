using Sistran.Company.Application.UnderwritingServices.EEProvider.DAO;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Resources;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;


namespace Sistran.Company.Application.UnderwritingBusinessServiceProvider.Business
{
    public class BusinessQuotationNumber
    {

        /// <summary>
        /// Reserva consecutivos temporales de cotizacion
        /// </summary>
        /// <param name="countQuotation"></param>
        /// <returns></returns>
        public List<int> GetReserveListTemp(int countQuotation)
        {
            ValidateCountQuotation(countQuotation);
            List<int> quotationNumber = new List<int>();

            try
            {
                QuotationNumberDAO quotationNumberDAO = new QuotationNumberDAO();
                quotationNumber = quotationNumberDAO.GetReserveListTemp(countQuotation);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
            return quotationNumber;
        }

        /// <summary>
        /// Valida que los argumentos de entrada representen un valor
        /// </summary>
        /// <param name="countQuotation"></param>
        /// <param name="branchCode"></param>
        /// <param name="prefixCode"></param>
        public void ValidateArguments(int countQuotation, int branchCode, int prefixCode)
        {
            ValidateCountQuotation(countQuotation);
            if (branchCode < 0)
            {
                throw new BusinessException(Errors.ErrorCurrentQuotationBranchCode);
            }

            if (prefixCode < 0)
            {
                throw new BusinessException(Errors.ErrorCurrentQuotationPrefixCode);
            }
        }

        /// <summary>
        /// Valida que se ingrese un numero de cotizaciones
        /// </summary>
        /// <param name="countQuotation"></param>
        private static void ValidateCountQuotation(int countQuotation)
        {
            if (countQuotation < 0)
            {
                throw new BusinessException(Errors.ErrorCurrentQuotationCount);
            }
        }
    }
}

