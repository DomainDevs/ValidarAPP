using System;
using System.Collections.Generic;
using Sistran.Co.Application.Data;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAO
{
    public class QuotationNumberDAO
    {
        /// </summary>
        /// <param name="countQuotation"></param>
        /// <param name="branchCode"></param>
        /// <param name="prefixCode"></param>
        /// <returns></returns>
        public List<int> GetReserveListQuotes(int countQuotation, int branchCode, int prefixCode)
        {
            NameValue[] parameters = new NameValue[3];

            parameters[0] = new NameValue("CANT", countQuotation);
            parameters[1] = new NameValue("PREFIX_CD", prefixCode);
            parameters[2] = new NameValue("BRANCH_CD", branchCode);

            object result = null;

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPScalar("COMM.EMISSION_TEMP_QUOTATION", parameters);
            }

            return GetNumbersQuoteToResult(countQuotation, result);
        }

        /// <summary>
        /// Reserva consecutivos temporales de cotizacion
        /// </summary>
        /// <param name="countQuotation"></param>
        /// <returns></returns>
        public List<int> GetReserveListTemp(int countQuotation)
        {
            NameValue quantity = new NameValue("CANT", countQuotation);
            object result = null;

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPScalar("COMM.RESERVE_TEMP_IDS", quantity);
            }

            return GetNumbersQuoteToResult(countQuotation, result);
        }

        /// <summary>
        /// Reserva un consecutivo Agrupador de Cotizaciones
        /// </summary>
        /// <returns></returns>
        public int GetConsecutiveGroupQuotes()
        {
            object result = null;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPScalar("COMM.RESERVE_GROUP_QUOTES_ID");
            }

            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Obtiene el numero de cotizaciones solicitadas a partir del resultado de consulta en BD
        /// </summary>
        /// <param name="countQuotation"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static List<int> GetNumbersQuoteToResult(int countQuotation, object result)
        {
            List<int> ConsecutiveQuotationList = new List<int>();
            string results = result.ToString();
            Int32 quotationId;

            if (int.TryParse(results, out quotationId))
            {
                for (int count = 0; count < countQuotation; count++)
                {
                    ConsecutiveQuotationList.Add(quotationId);
                }
            }
            else
            {
                throw new Sistran.Core.Framework.ValidationException(results);
            }

            return ConsecutiveQuotationList;
        }
    }
}
