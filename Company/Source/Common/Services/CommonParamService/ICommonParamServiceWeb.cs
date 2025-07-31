using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Company.Application.ModelServices.Models;
using paramserviceinterface = Sistran.Core.Application.CommonParamService;

namespace Sistran.Company.Application.CommonParamService
{
    [ServiceContract]
    public interface ICommonParamServiceWeb : paramserviceinterface.ICommonParamServiceWebCore
    {
        /// <summary>
        /// Genera archivo excel ramo comercial
        /// </summary>
        /// <param name="Prefix"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [OperationContract]
        string GenerateFileToPrefix(List<CompanyPrefix> Prefix, string fileName);

        /// <summary>
        /// Obtener todos los ramos comerciales
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessException">Error in GetPrefixAll</exception>
        [OperationContract]
        List<CompanyPrefix> CompanyGetAllPrefix();

        /// <summary>
        /// Guarda el ramo comercial
        /// </summary>
        /// <param name="entityPrefix">Modelo donde se cargan los ramos comerciales que se van a guardar</param>
        /// <returns></returns>
        [OperationContract]
        List<string> SavePrefix(List<CompanyPrefix> prefix);
    }
}
