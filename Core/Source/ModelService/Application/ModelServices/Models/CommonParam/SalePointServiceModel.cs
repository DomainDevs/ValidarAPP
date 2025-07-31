// -----------------------------------------------------------------------
// <copyright file="SalePointServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.CommonParam
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Unidad de Sale Point Service Model
    /// </summary>
    [DataContract]
    public class SalePointServiceModel
    {
        /// <summary>
        /// Obtiene o establece el Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece una descripcion corta
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece el modelo de la sucursal
        /// </summary>
        [DataMember]
        public BranchServiceQueryModel Branch { get; set; }

        /// <summary>
        /// Obtiene o establece el estado del punto de venta
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }
    }
}
