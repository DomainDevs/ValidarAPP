// -----------------------------------------------------------------------
// <copyright file="SurchargeBusiness.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Business
{    
    using Sistran.Core.Application.UnderwritingParamService.Models;

    /// <summary>
    /// Validacion de surcharge
    /// </summary>
    public class SurchargeBusiness
    {
        /// <summary>
        /// Valida la longitud de la descripcion 
        /// </summary>
        /// <param name="component">Componnete de recargo MOD-B</param>
        /// <returns>True en caso de validacion correcta</returns>
        public bool ValidateLengthDescription(ParamSurcharge component)
        {
            if (component.Description != null)
            {
                if (component.Description.Length <= 15)
                {
                    return true;
                }
            }

            return false;
        }
    }
}