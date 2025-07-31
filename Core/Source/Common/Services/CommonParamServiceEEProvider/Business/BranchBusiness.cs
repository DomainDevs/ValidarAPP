// -----------------------------------------------------------------------
// <copyright file="BranchBusiness.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Business
{
    using System;
    using System.Linq;
    using CommonParamService.Models;

    /// <summary>
    /// Validacion de sucursales
    /// </summary>
    public class BranchBusiness
    {
        /// <summary>
        /// Valida si la longitud de la descricion de sucursales es menor de lo permitido
        /// </summary>
        /// <param name="description">Descricion de la sucursal</param>
        /// <returns>True en caso de validacion correcta</returns>
        public bool ValidateLengthDescription(string description)
        {
            if (description == null || description.Length <= 50)
            {
                return true;
            }

            return false;
        }
    }
}