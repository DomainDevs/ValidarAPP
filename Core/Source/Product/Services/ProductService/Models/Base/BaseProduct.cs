using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.ProductServices.Models.Base
{
    /// <summary>
    /// Producto
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Extension" />
    [DataContract]
    public class BaseProduct : Extension
    {
        #region propiedades
        /// <summary>
        /// Atributo para la propiedad Identificador Producto
        /// </summary> 
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Atributo para la propiedad Descripcion
        /// </summary> 
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Atributo para la propiedad Descripcion Corta
        /// </summary> 
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Atributo para la propiedad Script de Reglas
        /// </summary> 
        [DataMember]
        public int? ScriptId { get; set; }

        /// <summary>
        /// Atributo para la propiedad Id de la Regla
        /// </summary> 
        [DataMember]
        public int? RuleSetId { get; set; }


        /// <summary>
        /// Atributo para la propiedad Reglas pre
        /// </summary> 
        [DataMember]
        public int? PreRuleSetId { get; set; }

        /// <summary>
        /// Atributo para la propiedad Fecha Inicio
        /// </summary> 
        [DataMember]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Atributo para la propiedad Fecha final
        /// </summary> 
        [DataMember]
        public DateTime? CurrentTo { get; set; }

        /// <summary>
        ///  Obtiene o setea Si el producto esta en Uso
        /// </summary>
        /// <value>
        ///   <c>true</c> Instancia s i es en uso; si no sta en uso, <c>false</c>.
        /// </value>
        [DataMember]
        public Boolean IsUse { get; set; }

        /// <summary>
        /// Atributo para calcular priuma mínima
        /// </summary> 
        [DataMember]
        public bool CalculateMinPremium { get; set; }
        /// <summary>
        /// Atributo para la propiedad si aplica para cotizador liviano
        /// </summary> 
        [DataMember]
        public bool IsGreen { get; set; }

        /// <summary>
        /// Atributo para la propiedad Polizas Individuales
        /// </summary> 
        [DataMember]
        public bool IsInteractive { get; set; }

        /// <summary>
        /// Atributo para la propiedad Polizas Colectivas
        /// </summary> 
        [DataMember]
        public bool IsCollective { get; set; }

        /// <summary>
        /// Atributo para la propiedad Polizas Masivas
        /// </summary> 
        [DataMember]
        public bool IsMassive { get; set; }

        /// <summary>
        /// Atributo para la propiedad Si aplica tasa Unica
        /// </summary> 
        [DataMember]
        public bool IsFlatRate { get; set; }


        /// <summary>
        /// Obtiene o setea Solicitud Agrupadora
        /// </summary> 
        [DataMember]
        public bool IsRequest { get; set; }

        /// <summary>
        /// Obtiene o setea la version del Producto
        /// </summary>
        /// <value>
        /// Version del producto
        /// </value>
        [DataMember]
        public int Version { get; set; }

        #endregion


        #region comisiones
        /// <summary>
        /// Atributo para la propiedad Porcentaje COmision Adicional del producto
        /// </summary> 
        [DataMember]
        public decimal? AdditionalCommissionPercentage { get; set; }

        /// <summary>
        /// Atributo para la propiedad porcentaje Descuento comision
        /// </summary> 
        [DataMember]
        public decimal? StdDiscountCommPercentage { get; set; }


        /// <summary>
        /// Atributo para la propiedad Comision Normal
        /// </summary> 
        [DataMember]
        public decimal? StandardCommissionPercentage { get; set; }


        /// <summary>
        /// Atributo para la propiedad Comision adicional
        /// </summary> 
        [DataMember]
        public decimal? AdditDisCommissPercentage { get; set; }

        /// <summary>
        /// Atributo para la propiedad Recargo Comision
        /// </summary> 
        [DataMember]
        public decimal? SurchargeCommissionPercentage { get; set; }

        /// <summary>
        /// Atributo para la propiedad DecrementCommisionAdjustFactorPercentage
        /// </summary> 
        [DataMember]
        public decimal? DecrementCommisionAdjustFactorPercentage { get; set; }

        /// <summary>
        /// Atributo para la propiedad IncrementCommisionAdjustFactorPercentage
        /// </summary> 
        [DataMember]
        public decimal? IncrementCommisionAdjustFactorPercentage { get; set; }

        /// <summary>
        /// Estado del producto.
        /// </summary>
        private bool Active { get; set; }

        /// <summary>
        /// Atributo para la propiedad Recargo Comision
        /// </summary> 
        [DataMember]
        public decimal? AdditCommissPercentage { get; set; }
        #endregion
    }
}
