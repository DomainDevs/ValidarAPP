// -----------------------------------------------------------------------
// <copyright file="DiscountBusiness.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Business
{
    using Models;   

    /// <summary>
    /// Validacion de Descuentos
    /// </summary>
    public class DiscountBusiness
    {
        /// <summary>
        /// Valida la longitud de la descripcion 
        /// </summary>
        /// <param name="component">Componnete de descuento MOD-B</param>
        /// <returns>True en caso de validacion correcta</returns>
        public bool ValidateLengthDescription(ParamDiscount component)
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