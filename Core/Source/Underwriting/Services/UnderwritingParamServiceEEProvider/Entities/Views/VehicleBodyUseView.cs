// -----------------------------------------------------------------------
// <copyright file="VehicleBodyUseView.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres Gonzalez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views
{
    using System;
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Views;

    /// <summary>
    /// Vista de Carrocería de vehículo y Usos
    /// </summary>
    [Serializable]
    public class VehicleBodyUseView : BusinessView
    {
        /// <summary>
        /// Obtiene Carrocería de vehículo
        /// </summary>
        public BusinessCollection VehicleBodies
        {
            get
            {
                return this["VehicleBody"];
            }
        }

        /// <summary>
        /// Obtiene los usos
        /// </summary>
        public BusinessCollection VehicleUses
        {
            get
            {
                return this["VehicleUse"];
            }
        }

        /// <summary>
        /// Método para asociar Carrocería de vehículo y Usos
        /// </summary>
        /// <param name="vehicleBody">Entidad Carrocería de vehículo</param>
        /// <returns>Lista de Carrocería de vehículo</returns>
        public BusinessCollection GetRelatedUses(VehicleBody vehicleBody)
        {
            BusinessCollection returnBusinessCollection = new BusinessCollection();

            foreach (BusinessObject item in this.GetObjectsByRelation("BodyVehicle", vehicleBody, false))
            {
                var businessObject = this.GetObjectByRelation("UseBodyVehicle", item, false);
                if (businessObject != null)
                {
                    returnBusinessCollection.Add(businessObject);
                }
            }

            return returnBusinessCollection;
        }
    }
}
