// -----------------------------------------------------------------------
// <copyright file="ParamCompanyType.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.UniquePersonServices.Models
{
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contiene las propiedades de la compañia
    /// </summary>
    [DataContract]
    public class ParamCompanyType
    {
        /// <summary>
        /// ID de Tipo de compañia.
        /// </summary>
        [DataMember]
        private readonly decimal id;

        /// <summary>
        /// Descripción de Tipo de compañia.
        /// </summary>
        [DataMember]
        private readonly string description;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamCompanyType"/>.
        /// </summary>
        /// <param name="id">Id de Tipo de compañia.</param>
        /// <param name="description">Descripción de Tipo de compañia.</param>
        private ParamCompanyType(decimal id, string description)
        {
            this.id = id;
            this.description = description;
        }

        /// <summary>
        /// Obtiene el Id de Tipo de compañia.
        /// </summary>
        [DataMember]
        public decimal Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// Obtiene la Descripción de Tipo de compañia.
        /// </summary>
        [DataMember]
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        /// <summary>
        /// Objeto que obtiene la Tipo de compañia.
        /// </summary>
        /// <param name="id">Id de Tipo de compañia.</param>
        /// <param name="description">Descripción de Tipo de compañia.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamCompanyType, ErrorModel> GetParamCompanyType(decimal id, string description)
        {
            return new ResultValue<ParamCompanyType, ErrorModel>(new ParamCompanyType(id, description));
        }

        /// <summary>
        /// Objeto que crea la Tipo de compañia.
        /// </summary>
        /// <param name="id">Id de Tipo de compañia.</param>
        /// <param name="description">Descripción de Tipo de compañia.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamCompanyType, ErrorModel> CreateParamCompanyType(decimal id, string description)
        {
            //// escribir todas las validaciones requeridas para la creación del modelo. Ejemplo
            List<string> error = new List<string>();

            if (id <= 0)
            {
                error.Add("El identificador no puede ser un valor negativo");
                return new ResultError<ParamCompanyType, ErrorModel>(ErrorModel.CreateErrorModel(error, ErrorType.BusinessFault, null));
            }
            else
            {
                return new ResultValue<ParamCompanyType, ErrorModel>(new ParamCompanyType(id, description));
            }
        }
    }
}
