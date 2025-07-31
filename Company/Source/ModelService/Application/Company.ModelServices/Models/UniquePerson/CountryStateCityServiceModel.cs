// -----------------------------------------------------------------------
// <copyright file="CountryStateCityServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ModelServices.Models
{
    using System.Runtime.Serialization;    
    using Sistran.Core.Application.ModelServices.Models.Param;

    /// <summary>
    /// Contiene las propiedades del país, ciudad, estado
    /// </summary>
    [DataContract]
    public class CountryStateCityServiceModel
    {
        /// <summary>
        /// Obtiene o establece el Id de la ciudad.
        /// </summary>
        [DataMember]
        public int CityCd { get; set; }

        /// <summary>
        /// Obtiene o establece el Description de la ciudad.
        /// </summary>
        [DataMember]
        public string CityDescription { get; set; }

        /// <summary>
        /// Obtiene o establece el Id del país.
        /// </summary>
        [DataMember]
        public int CountryCd { get; set; }

        /// <summary>
        /// Obtiene o establece el Description del país.
        /// </summary>
        [DataMember]
        public string CountryDescription { get; set; }
        /// <summary>
        /// Obtiene o establece el Id del estado.
        /// </summary>
        [DataMember]
        public int StateCd { get; set; }

        /// <summary>
        /// Obtiene o establece el Description del estado.
        /// </summary>
        [DataMember]
        public string StateDescription { get; set; }

        /// <summary>
        /// Obtiene o establece del ParametricServiceModel.
        /// </summary>
        [DataMember]
        public ParametricServiceModel ParametricServiceModel { get; set; }
    }
}