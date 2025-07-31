// -----------------------------------------------------------------------
// <copyright file="MiniumPremiunViewModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Wilfrido Heredia Carrera</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    using Sistran.Core.Application.ModelServices.Enums;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo del Prima Minima
    /// </summary>
    [DataContract]
    public class MinPremiunRelationViewModel
    {
        /// <summary>
        /// Obtiene o establece el Identificador de la Prima minima
        /// </summary>        
        public int Id { get; set; }

        /// <summary>
        /// Ramo comercial
        /// </summary>
        public PrefixViewModel Prefix { get; set; }

        /// <summary>
        /// Sucursal
        /// </summary>
        public BranchViewModel Branch { get; set; }

        /// <summary>
        /// Tipo de Endoso
        /// </summary>             
        public EndoTypeViewModel EndorsementType { get; set; }
       
        /// <summary>
        /// Moneda
        /// </summary>             
        public CurrencyViewModel Currency { get; set; }


        /// <summary>
        /// Moneda
        /// </summary>             
        public ProductViewModel Product { get; set; }

        /// <summary>
        /// Obtiene o establece Id de la Clave
        /// </summary>
        public ClaveViewModel Clave { get; set; }

        /// <summary>
        /// Valor de la prima minima
        /// </summary>             
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")] 
        public decimal MiniumPremiunValue { get; set; }

        /// <summary>
        /// Valor de la sub minima
        /// </summary>             
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")] 
        public decimal MiniumSubValue { get; set; }

        /// <summary>
        /// Obtiene o establece estado
        /// </summary>
        public StatusTypeService StatusTypeService { get; set; }

    }
}