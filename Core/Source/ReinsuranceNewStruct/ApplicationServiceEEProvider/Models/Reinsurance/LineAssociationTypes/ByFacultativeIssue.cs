#region Using

using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


#endregion

namespace Sistran.Core.Application.ReinsuranceServices.Models.LineAssociationTypes
{
    /// <summary>
    /// Tipo de asociación de lineas: Facultativo Por Emision
    /// </summary>
    [DataContract]
    public class ByFacultativeIssue : LineAssociationType
    {
       
        /// <summary>
        /// Ramo
        /// </summary>
        [DataMember]
        public List<Prefix> Prefix { get; set; }
    }
}