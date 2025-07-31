// -----------------------------------------------------------------------
// <copyright file="ParamBusinessParamBusinessConfiguration.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using System.Collections.Generic;
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Modelo de negocio de negocio y acuerdo de negocio.
    /// </summary>
    public class BaseParamBusinessParamBusinessConfiguration: Extension
    {
        /// <summary>
        /// Modelo del negocio.
        /// </summary>
        private readonly BaseParamBusiness paramBusiness;

        /// <summary>
        /// Acuerdos del negocio asociados.
        /// </summary>
        private readonly List<ParamBusinessConfiguration> paramBusinessConfiguration;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamBusinessParamBusinessConfiguration"/>.
        /// </summary>
        /// <param name="paramBusiness">Modelo de negocio.</param>
        /// <param name="paramBusinessConfiguration">Acuerdos del negocio asociados.</param>
        protected BaseParamBusinessParamBusinessConfiguration(BaseParamBusiness paramBusiness, List<ParamBusinessConfiguration> paramBusinessConfiguration)
        {
            this.paramBusiness = paramBusiness;
            this.paramBusinessConfiguration = paramBusinessConfiguration;
        }

        /// <summary>
        /// Obtiene el modelo de negocio.
        /// </summary>
        public BaseParamBusiness BaseParamBusiness
        {
            get
            {
                return this.paramBusiness;
            }
        }

        /// <summary>
        /// Obtiene los acuerdos del negocio asociados.
        /// </summary>
        public List<ParamBusinessConfiguration> BaseParamBusinessConfiguration
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
        public static Result<BaseParamBusinessParamBusinessConfiguration, ErrorModel> GetParamBusinessParamBusinessConfiguration(BaseParamBusiness paramBusiness, List<ParamBusinessConfiguration> paramBusinessConfiguration)
        {
            return new ResultValue<BaseParamBusinessParamBusinessConfiguration, ErrorModel>(new BaseParamBusinessParamBusinessConfiguration(paramBusiness, paramBusinessConfiguration));            
        }

        public static Result<BaseParamBusinessParamBusinessConfiguration, ErrorModel> CreateParamBusinessParamBusinessConfiguration(BaseParamBusiness paramBusiness, List<ParamBusinessConfiguration> paramBusinessConfiguration)
        {
            return new ResultValue<BaseParamBusinessParamBusinessConfiguration, ErrorModel>(new BaseParamBusinessParamBusinessConfiguration(paramBusiness, paramBusinessConfiguration));
        }
    }
}
