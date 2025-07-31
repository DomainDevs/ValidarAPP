using Sistran.Company.Application.CommonParamService.EEProvider.DAOs;
using Sistran.Company.Application.CommonParamService.EEProvider.Resources;
using Sistran.Company.Application.ModelServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.CommonParamService.EEProvider
{
    public class CommonParamServiceEEProviderWeb : Sistran.Core.Application.CommonParamService.CommonParamServiceEEProviderWebCore,ICommonParamServiceWeb
    {
        /// <summary>
        /// Genera archivo excel ramo comercial
        /// </summary>
        /// <param name="Prefix"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileToPrefix(List<CompanyPrefix> Prefix, string fileName)
        {
            try
            {
                CompanyFileDAO prefix = new CompanyFileDAO();
                return prefix.GenerateFileToPrefix(Prefix, fileName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGenerateFileToPrefix), ex);
            }
        }

        /// <summary>
        /// Obtener todos los ramos comerciales
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessException">Error in GetPrefixAll</exception>
        public List<CompanyPrefix> CompanyGetAllPrefix()
        {
            try
            {
                var companyPrefixDAO = new CompanyPrefixDAO();
                return companyPrefixDAO.GetAllPrefix();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetAllPrefix), ex);
            }
        }

        /// <summary>
        /// Realiza los procesos del CRUD para el ramo comercial
        /// </summary>
        /// <param name="prefixAdd">Lista de ramo comercial para ser agregados</param>
        /// <param name="prefixEdit">Lista de ramo comercial para ser actualizados</param>
        /// <param name="prefixDelete">Lista de ramo comercial para ser eliminados</param>
        /// <returns></returns>
        public List<string> SavePrefix(List<CompanyPrefix> prefix)
        {
            try
            {
                CompanyPrefixDAO PrefixDAO = new CompanyPrefixDAO();
                return PrefixDAO.SavePrefix(prefix);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCRUDProcessesForPrefix), ex);
            }
        }
    }
}
