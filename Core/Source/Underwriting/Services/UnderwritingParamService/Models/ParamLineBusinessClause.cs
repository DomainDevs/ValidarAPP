// -----------------------------------------------------------------------
// <copyright file="ParamLineBusinessClause.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.Utilities.Error;
    using System.Collections.Generic;
    /// <summary>
    /// Contiene las propiedades de las Cláusulas por Ramo Técnico
    /// </summary>
    public class ParamLineBusinessClause
    {
        /// <summary>
        /// Ramo Técnico de las Cláusulas por Ramo Técnico.
        /// </summary>
        private readonly ParamLineBusinessModel paramLineBusinessModel;

        /// <summary>
        /// Cláusulas de las Cláusulas por Ramo Técnico.
        /// </summary>
        private readonly List<ParamClauseModel> paramClauseModel;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamLineBusinessClause"/>.
        /// </summary>
        /// <param name="paramLineBusinessModel">Ramo Técnico de las Cláusulas por Ramo Técnico.</param>
        /// <param name="paramClauseModel">Cláusulas de las Cláusulas por Ramo Técnico.</param>
        private ParamLineBusinessClause(ParamLineBusinessModel paramLineBusinessModel, List<ParamClauseModel> paramClauseModel)
        {
            this.paramLineBusinessModel = paramLineBusinessModel;
            this.paramClauseModel = paramClauseModel;
        }

        /// <summary>
        /// Obtiene el Ramo Técnico de las Cláusulas por Ramo Técnico.
        /// </summary>
        public ParamLineBusinessModel ParamLineBusinessModel
        {
            get
            {
                return this.paramLineBusinessModel;
            }
        }

        /// <summary>
        /// Obtiene la Cláusulas de las Cláusulas por Ramo Técnico.
        /// </summary>
        public List<ParamClauseModel> ParamClauseModel
        {
            get
            {
                return this.paramClauseModel;
            }
        }

        /// <summary>
        /// Objeto que obtiene las Cláusulas por Ramo Técnico.
        /// </summary>
        /// <param name="paramLineBusinessModel">Ramo Técnico de las Cláusulas por Ramo Técnico.</param>
        /// <param name="paramClauseModel">Cláusulas de las Cláusulas por Ramo Técnico.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamLineBusinessClause, ErrorModel> GetParamLineBusinessClause(ParamLineBusinessModel paramLineBusinessModel, List<ParamClauseModel> paramClauseModel)
        {
            return new ResultValue<ParamLineBusinessClause, ErrorModel>(new ParamLineBusinessClause(paramLineBusinessModel, paramClauseModel));
        }

        /// <summary>
        /// Objeto que crea las Cláusulas por Ramo Técnico.
        /// </summary>
        /// <param name="paramLineBusinessModel">Ramo Técnico de las Cláusulas por Ramo Técnico.</param>
        /// <param name="paramClauseModel">Cláusulas de las Cláusulas por Ramo Técnico.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamLineBusinessClause, ErrorModel> CreateParamLineBusinessClause(ParamLineBusinessModel paramLineBusinessModel, List<ParamClauseModel> paramClauseModel)
        {
            return new ResultValue<ParamLineBusinessClause, ErrorModel>(new ParamLineBusinessClause(paramLineBusinessModel, paramClauseModel));
        }
    }
}