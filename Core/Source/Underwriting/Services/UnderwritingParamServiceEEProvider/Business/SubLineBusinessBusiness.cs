// -----------------------------------------------------------------------
// <copyright file="SubLineBusinessBusiness.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Business
{
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs;

    /// <summary>
    /// clase para las validaciones de SubRamo Tecnico
    /// </summary>
    public class SubLineBusinessBusiness
    {
        /// <summary>
        /// Valida si existe SubRamos por el mismo nombre
        /// </summary>
        /// <param name="description">parametro descripcion</param>
        /// <returns>Retorna true o false</returns>
        public bool ValidateIfSuBlineBusinessExists(string description)
        {
                SubLineBusinessDAO subLineBusinessDAO = new SubLineBusinessDAO();
                if (subLineBusinessDAO.GetSubLineBusinessByNameAndTitle(description).ToString() == description)
                {
                     return false;
                }
                else
                {
                    return false;
                }
        }
    }
}
