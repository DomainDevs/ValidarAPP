// -----------------------------------------------------------------------
// <copyright file="ParamInfringementLog.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodriguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.CommonParamService.Models
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Contiene las propiedades del log dias Infracción
    /// </summary>
    [DataContract]
    public class ParamInfringementLog
    {
        /// <summary>
        /// Obtiene o establece Dias de Infracción
        /// </summary>
        private readonly long daysValidateInfringement;

        /// <summary>
        /// Obtiene o establece Fecha de registro
        /// </summary>
        private readonly DateTime registrationDate;

        /// <summary>
        /// Obtiene o establece id del usuario
        /// </summary>
        private readonly int userId;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamInfringementLog"/>.
        /// </summary>
        /// <param name="daysValidateInfringement">Dias de infraccion</param>
        /// <param name="registrationDate">Fecha registro log Discontinuidad</param>
        /// <param name="userId">Id usuario</param>
        private ParamInfringementLog(long daysValidateInfringement, DateTime registrationDate, int userId)
        {
            this.daysValidateInfringement = daysValidateInfringement;
            this.registrationDate = registrationDate;
            this.userId = userId;
        }

        /// <summary>
        /// Obtiene Dias de Infracción
        /// </summary>
        public long DaysValidateInfringement
        {
            get
            {
                return this.daysValidateInfringement;
            }
        }

        /// <summary>
        /// Obtiene Fecha de registro
        /// </summary>
        public DateTime RegistrationDate
        {
            get
            {
                return this.registrationDate;
            }
        }

        /// <summary>
        /// Obtiene id del usuario
        /// </summary>
        public int UserId
        {
            get
            {
                return this.userId;
            }
        }

        /// <summary>
        /// Objeto que obtiene el log dias Infracción 
        /// </summary>
        /// <param name="daysValidateInfringement">Dias de log dias Infracción </param>
        /// <param name="registrationDate">Fecha de registo</param>
        /// <param name="userId">Id Usuario</param>
        /// <returns>Modelo ParamInfringementLog</returns>
        public static Result<ParamInfringementLog, ErrorModel> GetParamInfringementLog(long daysValidateInfringement, DateTime registrationDate, int userId)
        {
            return new ResultValue<ParamInfringementLog, ErrorModel>(new ParamInfringementLog(daysValidateInfringement, registrationDate, userId));
        }

        /// <summary>
        /// Objeto que crea log dias Infracción
        /// </summary>
        /// <param name="daysValidateInfringement">Dias de log dias Infracción </param>
        /// <param name="registrationDate">Fecha de registo</param>
        /// <param name="userId">Id Usuario</param>
        /// <returns>Modelo ParamInfringementLog</returns>
        public static Result<ParamInfringementLog, ErrorModel> CreateParamInfringementLog(long daysValidateInfringement, DateTime registrationDate, int userId)
        {
            System.Collections.Generic.List<string> error = new List<string>();

            return new ResultValue<ParamInfringementLog, ErrorModel>(new ParamInfringementLog(daysValidateInfringement, registrationDate, userId));
        }
    }
}
