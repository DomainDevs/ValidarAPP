// -----------------------------------------------------------------------
// <copyright file="ParamBusinessConfiguration.cs" company="SISTRAN">
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
    /// Modelo de acuerdo de negocio.
    /// </summary>
    public class ParamBusinessConfiguration: BaseParamBusinessConfiguration
    {
        /// <summary>
        /// Solicitud agrupadora del acuerdo de negocio.
        /// </summary>
        private readonly ParamRequestEndorsement request;

        /// <summary>
        /// Producto del acuerdo de negocio.
        /// </summary>
        private readonly ParamProduct product;

        /// <summary>
        /// Grupo de cobertura del acuerdo de negocio.
        /// </summary>
        private readonly ParamGroupCoverage groupCoverage;

        /// <summary>
        /// Tipo de asistencia del acuerdo de negocio.
        /// </summary>
        private readonly ParamAssistanceType assistance;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamBusinessConfiguration"/>.
        /// </summary>
        /// <param name="businessConfigurationId">Id del acuerdo de negocio.</param>
        /// <param name="businessId">Id del negocio.</param>
        /// <param name="request">Solicitud agrupadora del acuerdo de negocio.</param>
        /// <param name="product">Producto del acuerdo de negocio.</param>
        /// <param name="groupCoverage">Grupo de cobertura del acuerdo de negocio.</param>
        /// <param name="assistance">Tipo de asistencia del acuerdo de negocio.</param>
        /// <param name="productIdResponse">Id producto respuesta.</param>
        private ParamBusinessConfiguration(int businessConfigurationId, int businessId, ParamRequestEndorsement request, ParamProduct product, ParamGroupCoverage groupCoverage, ParamAssistanceType assistance, string productIdResponse):
            base(businessConfigurationId, businessId, productIdResponse)
        {
            this.request = request;
            this.product = product;
            this.groupCoverage = groupCoverage;
            this.assistance = assistance;
        }


        /// <summary>
        /// Obtiene la solicitud agrupadora del acuerdo de negocio.
        /// </summary>
        public ParamRequestEndorsement Request
        {
            get
            {
                return this.request;
            }
        }

        /// <summary>
        /// Obtiene el producto del acuerdo de negocio.
        /// </summary>
        public ParamProduct Product
        {
            get
            {
                return this.product;
            }
        }

        /// <summary>
        /// Obtiene el grupo de cobertura del acuerdo de negocio.
        /// </summary>
        public ParamGroupCoverage GroupCoverage
        {
            get
            {
                return this.groupCoverage;
            }
        }

        /// <summary>
        /// Obtiene el tipo de asistencia del acuerdo de negocio.
        /// </summary>
        public ParamAssistanceType Assistance
        {
            get
            {
                return this.assistance;
            }
        }

       
        /// <summary>
        /// Objeto que crea u obtiene el acuerdo de negocio.
        /// </summary>
        /// <param name="businessConfigurationId">Id del acuerdo de negocio.</param>
        /// <param name="businessId">Id del negocio.</param>
        /// <param name="request">Solicitud agrupadora del acuerdo de negocio.</param>
        /// <param name="product">Producto del acuerdo de negocio.</param>
        /// <param name="groupCoverage">Grupo de cobertura del acuerdo de negocio.</param>
        /// <param name="assistance">Tipo de asistencia del acuerdo de negocio.</param>
        /// <param name="productIdResponse">Id producto respuesta.</param>
        /// <returns>Retorna el modelo de acuerdo de negocio o un error.</returns>
        public static Result<ParamBusinessConfiguration, ErrorModel> GetParamBusinessConfiguration(int businessConfigurationId, int businessId, ParamRequestEndorsement request, ParamProduct product, ParamGroupCoverage groupCoverage, ParamAssistanceType assistance, string productIdResponse)
        {
            return new ResultValue<ParamBusinessConfiguration, ErrorModel>(new ParamBusinessConfiguration(businessConfigurationId, businessId, request, product, groupCoverage, assistance, productIdResponse));
        }

        /// <summary>
        /// Objeto que crea u obtiene el acuerdo de negocio.
        /// </summary>
        /// <param name="businessConfigurationId">Id del acuerdo de negocio.</param>
        /// <param name="businessId">Id del negocio.</param>
        /// <param name="request">Solicitud agrupadora del acuerdo de negocio.</param>
        /// <param name="product">Producto del acuerdo de negocio.</param>
        /// <param name="groupCoverage">Grupo de cobertura del acuerdo de negocio.</param>
        /// <param name="assistance">Tipo de asistencia del acuerdo de negocio.</param>
        /// <param name="productIdResponse">Id producto respuesta.</param>
        /// <returns>Retorna el modelo de acuerdo de negocio o un error.</returns>
        public static Result<ParamBusinessConfiguration, ErrorModel> CreateParamBusinessConfiguration(int businessConfigurationId, int businessId, ParamRequestEndorsement request, ParamProduct product, ParamGroupCoverage groupCoverage, ParamAssistanceType assistance, string productIdResponse)
        {
            List<string> listErrors = new List<string>();
            if (product==null)
            {
                listErrors.Add("El campo producto es obligatorio");
            }
            if (groupCoverage == null)
            {
                listErrors.Add("El campo grupo de cobertura es obligatorio");
            }
            if (assistance == null)
            {
                listErrors.Add("El campo asistencia es obligatorio");
            }
            if (listErrors.Count > 0)
            {
                return new ResultError<ParamBusinessConfiguration, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.BusinessFault, null));
            }
            else
            {
                return new ResultValue<ParamBusinessConfiguration, ErrorModel>(new ParamBusinessConfiguration(businessConfigurationId, businessId, request, product, groupCoverage, assistance, productIdResponse));
            }
        }
    }
}

