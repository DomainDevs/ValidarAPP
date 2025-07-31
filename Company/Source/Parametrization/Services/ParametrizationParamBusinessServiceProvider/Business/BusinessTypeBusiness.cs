// -----------------------------------------------------------------------
// <copyright file="BusinessTypeBusiness.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>ETriana</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Sistran.Company.Application.ParametrizationParamBusinessService.Model;
    using Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.DAOs;
    using Sistran.Company.Application.ParametrizationParamBusinessService.Enums;

    /// <summary>
    /// BusinessTypeBusiness. Clase Controladora de las diferentes funcionalidades del negocio expuesto
    /// </summary>
    public class BusinessTypeBusiness
    {
        /// <summary>
        /// GetBusinessBusinessType. Obtiene el Listado de BusinessType desde la Negocio
        /// </summary>
        /// <returns>List<CompanyParamBusinessType></returns>
        public List<CompanyParamBusinessType> GetBusinessBusinessType()
        {
             return BusinessTypeDAOs.GetAllBusinessType();
        }

        /// <summary>
        /// GenerateFileToBusinessType. Genera el archivo de reporte en formato excel desde DAOs
        /// </summary>
        /// <param name="BusinessTypeList"></param>
        /// <param name="fileName"></param>
        /// <returns>String</returns>
        public String GenerateFileToBusinessType(List<CompanyParamBusinessType> BusinessTypeList, string fileName)
        {
           FileDAOs file = new FileDAOs();
           return file.GERERATEFILE(BusinessTypeList, fileName, FileProcessType.ParametrizationBusinessTyped);

        }


    }
}
