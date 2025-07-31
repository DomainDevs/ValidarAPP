// -----------------------------------------------------------------------
// <copyright file="ParamExpense.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using Sistran.Core.Application.Utilities.Error;
    using System.Collections.Generic;
    /// <summary>
    /// Parametrizacion para gastos de suscripcion
    /// </summary>
    //[DataContract]
    public class ParamExpense: BaseParamExpense
    {        
       
        /// <summary>
        /// ParamRuleSet regla de ejecucion
        /// </summary>
        //[DataMember]
        private readonly ParamRuleSet paramRuleSet;


        /// <summary>
        /// ParamRateType para tipo de tasa
        /// </summary>
        //[DataMember]
        private readonly ParamRateType paramRateType;

        

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamExpense" />
        /// </summary>
        /// <param name="id">Identificador de componente</param>
        /// <param name="smallDescription">Descripcion componente</param>
        /// <param name="tinyDescripcion">Descripcion corta</param>
        /// <param name="componentClass">Indica clase de componenete</param>
        /// <param name="componentType">Indica tipo de componente</param>
        /// <param name="rate">Indica tipo de componente</param>
        /// <param name="isMandatory">Indica si es obligatorio</param>
        /// <param name="isInitially">Indica si se debe incluir al inicializar </param>
        /// <param name="paramRuleSet">Indica regla de ejecucion</param>
        /// <param name="paramRateType">Indica tipo de tasa </param>
        private ParamExpense(int id, string smallDescription, string tinyDescripcion, Enums.ComponentClass componentClass,
            Enums.ComponnetType componentType, int rate, bool isMandatory, bool isInitially, ParamRuleSet paramRuleSet, ParamRateType paramRateType):
            base(id,smallDescription,tinyDescripcion, componentClass, componentType, rate, isMandatory, isInitially)
        {
            this.paramRuleSet = paramRuleSet;
            this.paramRateType = paramRateType;
        }

        ///<summary>
        /// Obtiene la clase del componente
        /// </summary>
        public ParamRuleSet ParamRuleSet
        {
            get
            {
                return this.paramRuleSet;
            }
        }
        /// <summary>
        /// Obtiene el tipo de componente
        /// </summary>
        public ParamRateType ParamRateType
        {
            get
            {
                return this.paramRateType;
            }
        }
        /// <summary>
        /// Construye la clase para lectura
        /// </summary>
        /// <param name="id">Identificador de tipo de vehiculo</param>
        /// <param name="smallDescription">Descripcion de tipo de vehiculo</param>
        /// <param name="tinyDescripcion">Descripcion corta</param>
        /// <param name="componentClass">Indica si esta activado</param>
        /// <param name="componentType">Indica si es una camioneta</param>
        /// <param name="rate">Indica el valor de la tasa</param>
        /// <param name="isMandatory">Indica si es una camioneta</param>
        /// <param name="isInitially">Indica si es una camioneta</param>
        /// <param name="paramRuleSet">Indica si es una camioneta</param>
        /// <param name="paramRateType">Indica si es una camioneta</param>
        /// <returns>Tipo de vehiculo</returns>
        public static Result<ParamExpense, ErrorModel> GetParamComponent(int id, string smallDescription, string tinyDescripcion, Enums.ComponentClass componentClass, Enums.ComponnetType componenType,int rate, bool isMandatory, bool isInitially, ParamRuleSet paramRuleSet, ParamRateType paraRateType)
        {
            return new ResultValue<ParamExpense, ErrorModel>(new ParamExpense( id,  smallDescription,  tinyDescripcion,  componentClass, componenType, rate,  isMandatory,  isInitially,  paramRuleSet, paraRateType));
        }

        /// <summary>
        /// Crea el modelo de tipo de vehiculo y realiza las validaciones de negocio
        /// </summary>
        /// <param name="id">Identificador de tipo de vehiculo</param>
        /// <param name="smallDescription">Descripcion de tipo de vehiculo</param>
        /// <param name="tinyDescripcion">Descripcion corta</param>
        /// <param name="componentClass">Indica si esta activado</param>
        /// <param name="componentType">Indica si es una camioneta</param>
        /// <param name="isMandatory">Indica si es una camioneta</param>
        /// <param name="isInitially">Indica si es una camioneta</param>
        /// <param name="paramRuleSet">Indica si es una camioneta</param>
        /// <param name="paramRateType">Indica si es una camioneta</param>
        /// <returns>Tipo de vehiculo</returns>
        public static Result<ParamExpense, ErrorModel> CreateParamExpense(int id, string smallDescription, string tinyDescripcion,
            Enums.ComponentClass componentClass, Enums.ComponnetType componenType, int rate, bool isMandatory, bool isInitially, 
            ParamRuleSet paramRuleSet, ParamRateType paraRateType)
        {
            List<string> error = new List<string>();
            if (string.IsNullOrEmpty(smallDescription))
            {
                error.Add(Resources.Errors.ValidateDescriptionExpense);
            }

            if (smallDescription.Length > 50)
            {
                error.Add(Resources.Errors.ValidateDescriptionCarateresExpense);
            }

            if (string.IsNullOrEmpty(smallDescription))
            {
                error.Add(Resources.Errors.ValidateSmallDescriptionExpense);
            }

            if (smallDescription.Length > 15)
            {
                error.Add(Resources.Errors.ValidateSmallDescriptionCaracteresExpense);
            }

            if (error.Count > 0)
            {
                return new ResultError<ParamExpense, ErrorModel>(ErrorModel.CreateErrorModel(error, Utilities.Enums.ErrorType.BusinessFault, null));
            }

            return new ResultValue<ParamExpense, ErrorModel>(new ParamExpense(id, smallDescription, tinyDescripcion, componentClass, componenType, rate, isMandatory, isInitially, paramRuleSet, paraRateType));
        }
    }
}
