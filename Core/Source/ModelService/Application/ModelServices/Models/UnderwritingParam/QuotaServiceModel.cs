// -----------------------------------------------------------------------
// <copyright file="QuotaServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Distribucion de cuota
    /// </summary>
    [DataContract]
    public class QuotaServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece el Id de la cupta
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el numero de la cuota
        /// </summary>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// Obtiene o establece el porcentaje de cuota
        /// </summary>
        [DataMember]
        public decimal Percentage { get; set; }

        /// <summary>
        /// Obtiene o establece el tiempo entre cuotas
        /// </summary>
        [DataMember]
        public int GapQuantity { get; set; }

        /// <summary>
        /// tipos de componentes por cuota plan de pago 
        /// </summary>
        [DataMember]
        public List<QuotaComponentTypeServiceModel> QuotaComponentTypeServiceModel { get; set; }


    }
}
