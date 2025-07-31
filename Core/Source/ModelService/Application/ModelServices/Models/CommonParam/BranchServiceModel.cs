// -----------------------------------------------------------------------
// <copyright file="BranchServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.CommonParam
{
    using System.Runtime.Serialization;
    using Param;

    /// <summary>
    /// Unidad de sucursales
    /// </summary>
    [DataContract]
    public class BranchServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece el Identificador
        /// </summary>
        [DataMember]
        public AddressTypeServiceQueryModel AddressType { get; set; }

        /// <summary>
        /// Obtiene o establece el sucursal
        /// </summary>
        [DataMember]
        public BranchServiceQueryModel Branch { get; set; }

        /// <summary>
        /// Obtiene o establece el ciudad
        /// </summary>
        [DataMember]
        public CityServiceRelationModel City { get; set; }

        /// <summary>
        /// Obtiene o establece el país
        /// </summary>
        [DataMember]
        public CountryServiceQueryModel Country { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de teléfono
        /// </summary>
        [DataMember]
        public PhoneTypeServiceQueryModel PhoneType { get; set; }

        /// <summary>
        /// Obtiene o establece el estado
        /// </summary>
        [DataMember]
        public StateServiceQueryModel State { get; set; }

        /// <summary>
        /// Obtiene o establece el Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la dirección
        /// </summary>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// Obtiene o establece número de teléfono
        /// </summary>
        [DataMember]
        public long PhoneNumber { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si hay algún problema
        /// </summary>
        [DataMember]
        public bool IsIssue { get; set; }
    }
}
