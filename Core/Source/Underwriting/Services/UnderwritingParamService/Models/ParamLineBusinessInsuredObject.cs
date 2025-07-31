// -----------------------------------------------------------------------
// <copyright file="ParamLineBusinessInsuredObject.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.Utilities.Error;
    using System.Collections.Generic;
    /// <summary>
    /// Contiene las propiedades de los Objetos del Seguro por Ramo Técnico
    /// </summary>
    public class ParamLineBusinessInsuredObject
    {
        /// <summary>
        /// Ramo Técnico de los Objetos del Seguro por Ramo Técnico.
        /// </summary>
        private readonly ParamLineBusinessModel paramLineBusinessModel;

        /// <summary>
        /// Objetos del Seguro de los Objetos del Seguro por Ramo Técnico.
        /// </summary>
        private readonly List<ParamInsuredObjectModel> paramInsuredObjectModel;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamLineBusinessInsuredObject"/>.
        /// </summary>
        /// <param name="paramLineBusinessModel">Ramo Técnico de los Objetos del Seguro por Ramo Técnico.</param>
        /// <param name="paramInsuredObjectModel">Objetos del Seguro de los Objetos del Seguro por Ramo Técnico.</param>
        private ParamLineBusinessInsuredObject(ParamLineBusinessModel paramLineBusinessModel, List<ParamInsuredObjectModel> paramInsuredObjectModel)
        {
            this.paramLineBusinessModel = paramLineBusinessModel;
            this.paramInsuredObjectModel = paramInsuredObjectModel;
        }

        /// <summary>
        /// Obtiene el Ramo Técnico de los Objetos del Seguro por Ramo Técnico.
        /// </summary>
        public ParamLineBusinessModel ParamLineBusinessModel
        {
            get
            {
                return this.paramLineBusinessModel;
            }
        }

        /// <summary>
        /// Obtiene los Objetos del Seguro de los Objetos del Seguro por Ramo Técnico.
        /// </summary>
        public List<ParamInsuredObjectModel> ParamInsuredObjectModel
        {
            get
            {
                return this.paramInsuredObjectModel;
            }
        }

        /// <summary>
        /// Objeto que obtiene los Objetos del Seguro por Ramo Técnico.
        /// </summary>
        /// <param name="paramLineBusinessModel">Ramo Técnico de los Objetos del Seguro por Ramo Técnico.</param>
        /// <param name="paramInsuredObjectModel">Objetos del Seguro de los Objetos del Seguro por Ramo Técnico.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamLineBusinessInsuredObject, ErrorModel> GetParamLineBusinessInsuredObject(ParamLineBusinessModel paramLineBusinessModel, List<ParamInsuredObjectModel> paramInsuredObjectModel)
        {
            return new ResultValue<ParamLineBusinessInsuredObject, ErrorModel>(new ParamLineBusinessInsuredObject(paramLineBusinessModel, paramInsuredObjectModel));
        }

        /// <summary>
        /// Objeto que crea los Objetos del Seguro por Ramo Técnico.
        /// </summary>
        /// <param name="paramLineBusinessModel">Ramo Técnico de los Objetos del Seguro por Ramo Técnico.</param>
        /// <param name="paramInsuredObjectModel">Objetos del Seguro de los Objetos del Seguro por Ramo Técnico.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamLineBusinessInsuredObject, ErrorModel> CreateParamLineBusinessInsuredObject(ParamLineBusinessModel paramLineBusinessModel, List<ParamInsuredObjectModel> paramInsuredObjectModel)
        {
            return new ResultValue<ParamLineBusinessInsuredObject, ErrorModel>(new ParamLineBusinessInsuredObject(paramLineBusinessModel, paramInsuredObjectModel));
        }
    }
}