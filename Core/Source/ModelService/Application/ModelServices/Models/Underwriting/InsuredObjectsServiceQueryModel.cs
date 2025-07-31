// -----------------------------------------------------------------------
// <copyright file="InsuredObjectsServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    /// <summary>
    /// MOD-S Objeto del seguro
    /// </summary>
    [DataContract]
    public class InsuredObjectsServiceQueryModel : ErrorServiceModel
    {
        [DataMember]
        public List<InsuredObjectServiceQueryModel> InsuredObjectServiceQueryModels { get; set; }
    }
}
