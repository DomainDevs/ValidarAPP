// -----------------------------------------------------------------------
// <copyright file="HierarchyAssociationServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\cvergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UniqueUser
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the <see cref="HierarchyAssociationServiceModel" />
    /// </summary>
    [DataContract]
    public class HierarchyAssociationServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece la Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece modelo HierarchyServiceQueryModel
        /// </summary>
        [DataMember]
        public HierarchyServiceQueryModel HierarchyServiceQueryModel { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si IsEnabled
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si IsExclusionary
        /// </summary>
        [DataMember]
        public bool IsExclusionary { get; set; }

        /// <summary>
        /// Obtiene o establece modelo ModuleServiceQueryModel
        /// </summary>
        [DataMember]
        public ModuleServiceQueryModel ModuleServiceQueryModel { get; set; }

        /// <summary>
        /// Obtiene o establece modelo SubModuleServicesQueryModel
        /// </summary>
        [DataMember]
        public SubModuleServicesQueryModel SubModuleServicesQueryModel { get; set; }
    }
}
