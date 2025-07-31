// -----------------------------------------------------------------------
// <copyright file="ParamParameter.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodriguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.CommonParamService.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Contiene las propiedades de parametros
    /// </summary>
    [DataContract]
    public class ParamParameter
    {
        /// <summary>
        /// Obtiene o establece descripcion del parametro
        /// </summary>
        private readonly int parameterId;

        /// <summary>
        /// Obtiene o establece descripcion del parametro
        /// </summary>
        private readonly string description;

        /// <summary>
        /// Obtiene o establece valor del parametro
        /// </summary>
        private readonly int value;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamParameter"/>.
        /// </summary>
        /// <param name="parameterId">Id del parametro</param>
        /// <param name="description">Descricion del parametro</param>
        /// <param name="value">Valor del parametro</param>
        private ParamParameter(int parameterId, string description, int value)
        {
            this.parameterId = parameterId;
            this.description = description;
            this.value = value;
        }

        /// <summary>
        /// Obtiene el id del parametro
        /// </summary>
        public int ParameterId
        {
            get
            {
                return this.parameterId;
            }
        }

        /// <summary>
        /// Obtiene la descripcion del parametro
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        /// <summary>
        /// Obtiene el valor del parametro
        /// </summary>
        public int Value
        {
            get
            {
                return this.value;
            }
        }

        /// <summary>
        /// Objeto que obtiene el parametro
        /// </summary>
        /// <param name="parameterId">Id del parametro</param>
        /// <param name="description">Descripcion del parametro</param>
        /// <param name="value">Valor del parametro</param>
        /// <returns>Modelo ParamParameter</returns>
        public static Result<ParamParameter, ErrorModel> GetParamParameter(int parameterId, string description, int value)
        {
            return new ResultValue<ParamParameter, ErrorModel>(new ParamParameter(parameterId, description, value));
        }

        /// <summary>
        /// Objeto que crea el parametro
        /// </summary>
        /// <param name="parameterId">Id del parametro</param>
        /// <param name="description">Descripcion del parametro</param>
        /// <param name="value">Valor del parametro</param>
        /// <returns>Modelo ParamParameter</returns>
        public static Result<ParamParameter, ErrorModel> CreateParamParameter(int parameterId, string description, int value)
        {
            List<string> error = new List<string>();

            if (parameterId <= 0)
            {
                error.Add("El identificador no puede ser un valor negativo");
                return new ResultError<ParamParameter, ErrorModel>(ErrorModel.CreateErrorModel(error, ErrorType.BusinessFault, null));
            }
            else
            {
                return new ResultValue<ParamParameter, ErrorModel>(new ParamParameter(parameterId, description, value));
            }
        }
    }
}
