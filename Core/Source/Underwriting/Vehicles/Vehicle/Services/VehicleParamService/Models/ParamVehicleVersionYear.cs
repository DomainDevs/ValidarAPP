// -----------------------------------------------------------------------
// <copyright file="ParamVehicleVersionYear.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------


namespace Sistran.Core.Application.VehicleParamService.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// Valor por año del vehículo
    public class ParamVehicleVersionYear
    {
        /// <summary>
        /// Marca del vehiculo
        /// </summary>
        public ParamVehicleMake ParamVehicleMake { get; set; }

        /// <summary>
        /// Modelo del vehiculo
        /// </summary>
        public ParamVehicleModel ParamVehicleModel { get; set; }

        /// <summary>
        /// Version del vehiculo
        /// </summary>
        public ParamVehicleVersion ParamVehicleVersion { get; set; }

        /// <summary>
        /// Moneda usada para el precio del vehiculo
        /// </summary>
        public ParamCurrency ParamCurrency { get; set; }

        /// <summary>
        /// Año del vehiculo
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Precio del vehiculo
        /// </summary>
        public decimal Price { get; set; }
    }
}
