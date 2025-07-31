// -----------------------------------------------------------------------
// <copyright file="ParamLineBusinessPeril.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.Utilities.Error;
    using System.Collections.Generic;
    /// <summary>
    /// Contiene las propiedades de los Amparos por Ramo Técnico
    /// </summary>
    public class ParamLineBusinessPeril
    {
        /// <summary>
        /// Ramo Técnico de los Amparos por Ramo Técnico.
        /// </summary>
        private readonly ParamLineBusinessModel paramLineBusinessModel;

        /// <summary>
        /// Amparos de los Amparos por Ramo Técnico.
        /// </summary>
        private readonly List<ParamPerilModel> paramPerilModel;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamLineBusinessPeril"/>.
        /// </summary>
        /// <param name="paramLineBusinessModel">Ramo Técnico de los Amparos por Ramo Técnico.</param>
        /// <param name="paramPerilModel">Amparos de los Amparos por Ramo Técnico.</param>
        private ParamLineBusinessPeril(ParamLineBusinessModel paramLineBusinessModel, List<ParamPerilModel> paramPerilModel)
        {
            this.paramLineBusinessModel = paramLineBusinessModel;
            this.paramPerilModel = paramPerilModel;
        }

        /// <summary>
        /// Obtiene el Ramo Técnico de los Amparos por Ramo Técnico.
        /// </summary>
        public ParamLineBusinessModel ParamLineBusinessModel
        {
            get
            {
                return this.paramLineBusinessModel;
            }
        }

        /// <summary>
        /// Obtiene la Amparos de los Amparos por Ramo Técnico.
        /// </summary>
        public List<ParamPerilModel> ParamPerilModel
        {
            get
            {
                return this.paramPerilModel;
            }
        }

        /// <summary>
        /// Objeto que obtiene los Amparos por Ramo Técnico.
        /// </summary>
        /// <param name="paramLineBusinessModel">Ramo Técnico de los Amparos por Ramo Técnico.</param>
        /// <param name="paramPerilModel">Amparos de los Amparos por Ramo Técnico.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamLineBusinessPeril, ErrorModel> GetParamLineBusinessPeril(ParamLineBusinessModel paramLineBusinessModel, List<ParamPerilModel> paramPerilModel)
        {
            return new ResultValue<ParamLineBusinessPeril, ErrorModel>(new ParamLineBusinessPeril(paramLineBusinessModel, paramPerilModel));
        }

        /// <summary>
        /// Objeto que crea los Amparos por Ramo Técnico.
        /// </summary>
        /// <param name="paramLineBusinessModel">Ramo Técnico de los Amparos por Ramo Técnico.</param>
        /// <param name="paramPerilModel">Amparos de los Amparos por Ramo Técnico.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamLineBusinessPeril, ErrorModel> CreateParamLineBusinessPeril(ParamLineBusinessModel paramLineBusinessModel, List<ParamPerilModel> paramPerilModel)
        {
            return new ResultValue<ParamLineBusinessPeril, ErrorModel>(new ParamLineBusinessPeril(paramLineBusinessModel, paramPerilModel));
        }
    }
}