// -----------------------------------------------------------------------
// <copyright file="VehicleTypeServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Julian Ospina</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Tipo de vehiculo
    /// </summary>
    [DataContract]
    public class VehicleTypeServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o establece Identificador del tipo de vehiculo
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la Descripcion del tipo de vehiculo
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion corta
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si se encuentra activado
        /// </summary>
        [DataMember]
        public bool IsEnable { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si es de tipo camioneta
        /// </summary>
        [DataMember]
        public bool IsTruck { get; set; }

        //[DataMember]
        //public bool IsElectronicPolicy { get; set; }

        /// <summary>
        /// Obtiene o establece el listado de las carrocerias asociadas al tipo de vehiculo
        /// </summary>
        [DataMember]
        public List<VehicleBodyServiceQueryModel> VehicleBodyServiceQueryModel { get; set; }

        /// <summary>
        /// Texto del tipo de vehiculo
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string value = string.Empty;
            if (!string.IsNullOrEmpty(SmallDescription))
                value = $"{SmallDescription} ";
            if (Id > 0)
                value += $"({Id}) ";
            if (!string.IsNullOrEmpty(value))
                value += ": ";
            return value;
        }
    }
}
