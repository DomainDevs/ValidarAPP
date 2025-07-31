// -----------------------------------------------------------------------
// <copyright file="ParamBusinessParamBusinessConfiguration.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.Utilities.Error;
    using System.Collections.Generic;
    /// <summary>
    /// Modelo de negocio de negocio y acuerdo de negocio.
    /// </summary>
    public class ParamBusinessParamBusinessConfiguration
    {
        /// <summary>
        /// Modelo del negocio.
        /// </summary>
        private readonly ParamBusiness paramBusiness;

        /// <summary>
        /// Acuerdos del negocio asociados.
        /// </summary>
        private readonly List<ParamBusinessConfiguration> paramBusinessConfiguration;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamBusinessParamBusinessConfiguration"/>.
        /// </summary>
        /// <param name="paramBusiness">Modelo de negocio.</param>
        /// <param name="paramBusinessConfiguration">Acuerdos del negocio asociados.</param>
        private ParamBusinessParamBusinessConfiguration(ParamBusiness paramBusiness, List<ParamBusinessConfiguration> paramBusinessConfiguration)
        {
            this.paramBusiness = paramBusiness;
            this.paramBusinessConfiguration = paramBusinessConfiguration;
        }

        /// <summary>
        /// Obtiene el modelo de negocio.
        /// </summary>
        public ParamBusiness ParamBusiness
        {
            get
            {
                return this.paramBusiness;
            }
        }

        /// <summary>
        /// Obtiene los acuerdos del negocio asociados.
        /// </summary>
        public List<ParamBusinessConfiguration> ParamBusinessConfiguration
        {
            get
            {
                return this.paramBusinessConfiguration;
            }
        }

        /// <summary>
        /// Objeto que crea u obtiene el negocio y acuerdos de negocio asociados.
        /// </summary>
        /// <param name="paramBusiness">Modelo de negocio.</param>
        /// <param name="paramBusinessConfiguration">Acuerdos del negocio asociados.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamBusinessParamBusinessConfiguration, ErrorModel> GetParamBusinessParamBusinessConfiguration(ParamBusiness paramBusiness, List<ParamBusinessConfiguration> paramBusinessConfiguration)
        {
            return new ResultValue<ParamBusinessParamBusinessConfiguration, ErrorModel>(new ParamBusinessParamBusinessConfiguration(paramBusiness, paramBusinessConfiguration));            
        }

        public static Result<ParamBusinessParamBusinessConfiguration, ErrorModel> CreateParamBusinessParamBusinessConfiguration(ParamBusiness paramBusiness, List<ParamBusinessConfiguration> paramBusinessConfiguration)
        {
            return new ResultValue<ParamBusinessParamBusinessConfiguration, ErrorModel>(new ParamBusinessParamBusinessConfiguration(paramBusiness, paramBusinessConfiguration));
        }
    }
}
