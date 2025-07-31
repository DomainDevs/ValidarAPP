// -----------------------------------------------------------------------
// <copyright file="ParamExpense.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// BaseParametrizacion para gastos de suscripcion
    /// </summary>
    //[DataContract]
    public class BaseParamExpense: Extension
    {        
        /// <summary>
        /// Identificador del gasto de suscripcion
        /// </summary>
        //[DataMember]
        private readonly int id;

        /// <summary>    
        /// Descripcion del gasto de suscripcion
        /// </summary>
         //[DataMember]
        private readonly string smallDescription;

        /// <sumary>
        /// Descripcion minuscula del gasto de suscripcion
        /// </sumary>
        //[DataMember]
        private readonly string tinyDescripcion;

        /// <summary>
        /// Id  clase de componente
        /// </summary>
        //[DataMember]
        private readonly Enums.ComponentClass componentClass;

        /// <summary>
        /// Id del tipo  de componente
        /// </summary>
        //[DataMember]
        private readonly Enums.ComponnetType componentType;
        /// <summary>
        /// Valor de tasa
        /// </summary>
        //[DataMember]
        private readonly int rate;
        /// <summary>
        /// check box para es obligatorio
        /// </summary>
        //[DataMember]
        private readonly bool isMandatory;

        /// <summary>
        /// check box para inicialmente incluido
        /// </summary>
        //[DataMember]
        private readonly bool isInitially;

       
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
        protected BaseParamExpense(int id, string smallDescription, string tinyDescripcion, Enums.ComponentClass componentClass,
            Enums.ComponnetType componentType, int rate, bool isMandatory, bool isInitially)
        {
            this.id = id;
            this.smallDescription = smallDescription;
            this.tinyDescripcion = tinyDescripcion;
            this.componentClass = componentClass;
            this.componentType = componentType;
            this.rate = rate;
            this.isMandatory = isMandatory;
            this.isInitially = isInitially;

        }

        /// <summary>
        /// Obtiene el identificador
        /// </summary>
        public int Id
        {
            get
            {
                return this.id;
            }
        }
        /// <summary>
        /// Obtiene la descripcion 
        /// </summary>
        public string SmallDescription
        {
            get
            {
                return this.smallDescription;
            }
        }
        /// <summary>
        /// Obtiene la dewscripcion minuscula 
        /// </summary>
        public string TinyDescripcion
        {
            get
            {
                return this.tinyDescripcion;
            }
        }
        /// <summary>
        /// Obtiene la clase del componente
        /// </summary>
        public Enums.ComponentClass ComponentClass
        {
            get
            {
                return this.componentClass;
            }
        }
        /// <summary>
        /// Obtiene la descripcion minuscula 
        /// </summary>
        public Enums.ComponnetType ComponentType
        {
            get
            {
                return this.componentType;
            }
        }

        /// <summary>
        /// Obtiene el identificador
        /// </summary>
        public int Rate
        {
            get
            {
                return this.rate;
            }
           
        }
        /// <summary>
        /// Obtiene la  desicion si es obligatorio
        /// </summary>
        public bool IsMandatory
        {
            get
            {
                return this.isMandatory;
            }
        }
        /// <summary>
        /// Obtiene  la  desicion si inicialmente es incluido
        /// </summary>
        public bool IsInitially
        {
            get
            {
                return this.isInitially;
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
        public static Result<BaseParamExpense, ErrorModel> GetParamComponent(int id, string smallDescription, string tinyDescripcion, Enums.ComponentClass componentClass, Enums.ComponnetType componenType,int rate, bool isMandatory, bool isInitially)
        {
            return new ResultValue<BaseParamExpense, ErrorModel>(new BaseParamExpense( id,  smallDescription,  tinyDescripcion,  componentClass, componenType, rate,  isMandatory,  isInitially));
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
        public static Result<BaseParamExpense, ErrorModel> CreateParamExpense(int id, string smallDescription, string tinyDescripcion,
            Enums.ComponentClass componentClass, Enums.ComponnetType componenType, int rate, bool isMandatory, bool isInitially )
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
                return new ResultError<BaseParamExpense, ErrorModel>(ErrorModel.CreateErrorModel(error, Utilities.Enums.ErrorType.BusinessFault, null));
            }

            return new ResultValue<BaseParamExpense, ErrorModel>(new BaseParamExpense(id, smallDescription, tinyDescripcion, componentClass, componenType, rate, isMandatory, isInitially));
        }
    }
}
