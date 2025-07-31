// -----------------------------------------------------------------------
// <copyright file="InsuredObjectBusiness.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Business
{
    using System.Linq;
    using Sistran.Core.Application.UnderwritingParamService.Models;

    /// <summary>
    /// Validacion de plan de pago
    /// </summary>
    public class InsuredObjectBusiness
    {
        /// <summary>
        /// Valida la longitud de la descripcion
        /// </summary>
        /// <param name="description">Descripcion de objetos del seguro </param>
        /// <returns>True en caso de validacion correcta</returns>
        public bool ValidateLengthDescrition(string description)
        {
          if (description == null || description.Length < 50)
            {
                return true;
            }

            return false;
        }
    }
}