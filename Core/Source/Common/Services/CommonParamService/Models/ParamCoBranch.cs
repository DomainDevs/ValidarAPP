// -----------------------------------------------------------------------
// <copyright file="ParamCoBranch.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.CommonParamService.Models
{
    using Sistran.Core.Application.CommonService.Models;    

    /// <summary>
    /// modelo de negocio de co sucursales
    /// </summary>
    public class ParamCoBranch
    {
        /// <summary>
        /// Obtiene o establece el tipo de dirección 
        /// </summary>       
        public ParamAddressType AddressType { get; set; }
        
        /// <summary>
        /// Obtiene o establece modelo de sucursales 
        /// </summary>      
        public ParamBranch Branch { get; set; }

        /// <summary>
        /// Obtiene o establece modelo de ciudades 
        /// </summary>        
        public City City { get; set; }

        /// <summary>
        /// Obtiene o establece modelo de país
        /// </summary>       
        public Country Country { get; set; }

        /// <summary>
        /// Obtiene o establece el modelo de tipo de teléfono
        /// </summary>      
        public ParamPhoneType PhoneType { get; set; }

        /// <summary>
        /// Obtiene o establece modelo de estado
        /// </summary>       
        public State State { get; set; }

        /// <summary>
        /// Obtiene o establece el Identificador
        /// </summary>       
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la dirección 
        /// </summary>      
        public string Address { get; set; }

        /// <summary>
        /// Obtiene o establece el teléfono
        /// </summary>      
        public long PhoneNumber { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si está en emision  
        /// </summary>       
        public bool IsIssue { get; set; }
    }
}
