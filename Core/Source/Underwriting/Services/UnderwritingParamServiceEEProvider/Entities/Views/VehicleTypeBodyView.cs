// -----------------------------------------------------------------------
// <copyright file="VehicleTypeBodyView.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Julian Ospina</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views
{
    using System;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Views;
    using Sistran.Core.Application.Common.Entities;

    /// <summary>
    /// Vista de tipo de vehiculos y carrocerias
    /// </summary>
    [Serializable]
    public class VehicleTypeBodyView : BusinessView
    {
        /// <summary>
        /// Obtiene tipo de vehiculos
        /// </summary>
        public BusinessCollection VehicleTypes
        {
            get
            {
                return this["VehicleType"];
            }
        }

        /// <summary>
        /// Obtiene las carrocerias
        /// </summary>
        public BusinessCollection VehicleBodies
        {
            get
            {
                return this["VehicleBody"];
            }
        }

        public BusinessCollection GetRelatedBodies(VehicleType vehicleType)
        {
            BusinessCollection returnBusinessCollection = new BusinessCollection();

            foreach (BusinessObject item in this.GetObjectsByRelation("TypeVehicle", vehicleType, false))
            {
                returnBusinessCollection.Add(this.GetObjectByRelation("TypeBodyVehicle", item, false));
            }
            return returnBusinessCollection;
        }
    }
}
