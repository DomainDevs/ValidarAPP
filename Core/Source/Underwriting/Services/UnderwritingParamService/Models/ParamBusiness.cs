// -----------------------------------------------------------------------
// <copyright file="ParamBusiness.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using System.Collections.Generic;
    /// <summary>
    /// Modelo de negocio de los tipos de riesgo cubierto.
    /// </summary>
    public class ParamBusiness: BaseParamBusiness
    {
        
        /// <summary>
        /// Negocio habilitado.
        /// </summary>
        private readonly ParamPrefix prefix;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamBusiness"/>.
        /// </summary>
        /// <param name="businessId">Identificador del negocio.</param>
        /// <param name="description">Descripción del negocio.</param>
        /// <param name="isEnabled">Negocio habilitado.</param>
        /// <param name="prefix">Ramo comercial asociado.</param>
        private ParamBusiness(int businessId, string description, bool isEnabled, ParamPrefix prefix):
            base(businessId, description, isEnabled)
        {
            this.prefix = prefix;
        }

        
        /// <summary>
        /// Obtiene el ramo comercial asociado.
        /// </summary>
        public ParamPrefix Prefix
        {
            get
            {
                return this.prefix;
            }
        }

        /// <summary>
        /// Objeto que crea u obtiene el negocio.
        /// </summary>
        /// <param name="businessId">Identificador del negocio.</param>
        /// <param name="description">Nombre del negocio.</param>
        /// <param name="isEnabled">Negocio habilitado.</param>
        /// <param name="prefix">Ramo comercial asociado.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamBusiness, ErrorModel> GetParamBusiness(int businessId, string description, bool isEnabled, ParamPrefix prefix)
        {
            return new ResultValue<ParamBusiness, ErrorModel>(new ParamBusiness(businessId, description, isEnabled, prefix));            
        }

        /// <summary>
        /// Objeto que crea u obtiene el negocio.
        /// </summary>
        /// <param name="businessId">Identificador del negocio.</param>
        /// <param name="description">Nombre del negocio.</param>
        /// <param name="isEnabled">Negocio habilitado.</param>
        /// <param name="prefix">Ramo comercial asociado.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamBusiness, ErrorModel> CreateParamBusiness(int businessId, string description, bool isEnabled, ParamPrefix prefix)
        {
            List<string> listErrors = new List<string>();
            if (string.IsNullOrEmpty(description))
            {
                listErrors.Add("El campo descripción es obligatorio");
            }
            if (listErrors.Count>0)
            {
                return new ResultError<ParamBusiness, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.BusinessFault, null));
            }
            else
            {
                return new ResultValue<ParamBusiness, ErrorModel>(new ParamBusiness(businessId, description, isEnabled, prefix));
            }
        }
    }
}
