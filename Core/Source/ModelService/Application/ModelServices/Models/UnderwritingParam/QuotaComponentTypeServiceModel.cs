// -----------------------------------------------------------------------
// <copyright file="PaymentPlanServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Modelo componentes por cuotas plan de pago
    /// </summary>
    [DataContract]
    public class QuotaComponentTypeServiceModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Value { get; set; }
    }
}
