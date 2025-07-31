// -----------------------------------------------------------------------
// <copyright file="ParamVehicleType.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Julian Ospina</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using System.Collections.Generic;
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// BaseParametrizacion del tipo de vehiculo
    /// </summary>
    public class BaseParamVehicleType : Extension
    {
        /// <summary>
        /// Identificador del tipo de vehiculo
        /// </summary>
        private readonly int id;

        /// <summary>
        /// Descripcion del tipo de vehiculo
        /// </summary>
        private readonly string description;

        /// <summary>
        /// Descripcion corta del tipo de vehiculo
        /// </summary>
        private readonly string smallDescription;

        /// <summary>
        /// Indica si esta activado
        /// </summary>
        private readonly bool isEnable;

        /// <summary>
        /// Indica si es un camion
        /// </summary>
        private readonly bool isTruck;
        
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamVehicleType" />
        /// </summary>
        /// <param name="id">Identificador de tipo de vehiculo</param>
        /// <param name="description">Descripcion de tipo de vehiculo</param>
        /// <param name="smallDescription">Descripcion corta</param>
        /// <param name="isEnable">Indica si esta activado</param>
        /// <param name="isTruck">Indica si es una camioneta</param>
        protected BaseParamVehicleType(int id, string description, string smallDescription, bool isEnable, bool isTruck)
        {
            this.id = id;
            this.description = description;
            this.smallDescription = smallDescription;
            this.isEnable = isEnable;
            this.isTruck = isTruck;
        }

        /// <summary>
        /// Obtiene el identificador
        /// </summary>
        public int Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// Obtiene la descripcion
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        /// <summary>
        /// Obtiene la descripcion corta
        /// </summary>
        public string SmallDescription
        {
            get
            {
                return this.smallDescription;
            }
        }

        /// <summary>
        /// Obtiene un valor que indica si se encuentra activado
        /// </summary>
        public bool IsEnable
        {
            get
            {
                return this.isEnable;
            }
        }

        /// <summary>
        /// Obtiene un valor que indica si es una camioneta
        /// </summary>
        public bool IsTruck
        {
            get
            {
                return this.isTruck;
            }
        }
    }
}
