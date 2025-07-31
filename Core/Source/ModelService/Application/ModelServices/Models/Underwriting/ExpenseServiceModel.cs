// -----------------------------------------------------------------------
// <copyright file="VehicleTypeServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Julian Ospina</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Runtime.Serialization;

    /// <summary>
    /// Componente
    /// </summary>
    [DataContract]
    public class ExpenseServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece Identificador del tipo de vehiculo
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la Descripcion del tipo de vehiculo
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion corta
        /// </summary>
        [DataMember]
        public string TinyDescripcion { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion corta
        /// </summary>
        [DataMember]
        public Enums.ComponentClass ComponentClass { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion corta
        /// </summary>
        [DataMember]
        public Enums.ComponnetType ComponentType { get; set; }

        /// <summary>
        /// Obtiene o establece si es obligatorio
        /// </summary>
        [DataMember]
        public bool IsMandatory { get; set; }

        /// <summary>
        /// Obtiene o establece si debe ser incluido al inicializar
        /// </summary>
        [DataMember]
        public bool IsInitially { get; set; }

        /// <summary>
        /// Obtiene o establece porcentaje de tasa
        /// </summary>
        [DataMember]
        public int Rate { get; set; }

        /// <summary>
        /// Obtiene tipo de clase de componente
        /// </summary>
        [DataMember]
        public RuleSetServiceQueryModel RuleSetServiceQueryModel { get; set; }


        /// <summary>
        /// Obtiene tipo de tasa 
        /// </summary>
        [DataMember]
        public RateTypeServiceQueryModel RateTypeServiceQueryModel { get; set; }


    }
}
