// -----------------------------------------------------------------------
// <copyright file="ParamDiscontinuityLog.cs" company="SISTRAN">
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
    /// Obtiene o establece Id del log
    /// </summary>
    [DataContract]
    public class ParamDiscontinuityLog
    {
        /// <summary>
        /// Obtiene o establece Dias de Discontinuidad
        /// </summary>
        private readonly long daysDiscontinuity;

        /// <summary>
        /// Obtiene o establece Fecha de registro
        /// </summary>
        private readonly DateTime registrationDate;

        /// <summary>
        /// Obtiene o establece id del usuario
        /// </summary>
        private readonly int userId;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamDiscontinuityLog"/>.
        /// </summary>
        /// <param name="daysDiscontinuity">Dias de Discontinuidad</param>
        /// <param name="registrationDate">Fecha registro log Discontinuidad</param>
        /// <param name="userId">Id usuario </param>
        private ParamDiscontinuityLog(long daysDiscontinuity, DateTime registrationDate, int userId)
        {
            this.daysDiscontinuity = daysDiscontinuity;
            this.registrationDate = registrationDate;
            this.userId = userId;
        }

        /// <summary>
        /// Obtiene Dias de Infracción
        /// </summary>
        public long DaysDiscontinuity
        {
            get
            {
                return this.daysDiscontinuity;
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
        /// Obtiene Id del usuario
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
        /// <param name="daysDiscontinuity">Dias de log dias Infracción </param>
        /// <param name="registrationDate">Fecha de registo</param>
        /// <param name="userId">Id Usuario</param>
        /// <returns>Modelo ParamInfringementLog</returns>
        public static Result<ParamDiscontinuityLog, ErrorModel> GetParamDiscontinuityLog(long daysDiscontinuity, DateTime registrationDate, int userId)
        {
            return new ResultValue<ParamDiscontinuityLog, ErrorModel>(new ParamDiscontinuityLog(daysDiscontinuity, registrationDate, userId));
        }

        /// <summary>
        /// Objeto que crea log dias Infracción
        /// </summary>
        /// <param name="daysDiscontinuity">Dias de log dias Infracción </param>
        /// <param name="registrationDate">Fecha de registo</param>
        /// <param name="userId">Id Usuario</param>
        /// <returns>Modelo ParamInfringementLog</returns>
        public static Result<ParamDiscontinuityLog, ErrorModel> CreateParamDiscontinuityLog(long daysDiscontinuity, DateTime registrationDate, int userId)
        {
            System.Collections.Generic.List<string> error = new List<string>();

            return new ResultValue<ParamDiscontinuityLog, ErrorModel>(new ParamDiscontinuityLog(daysDiscontinuity, registrationDate, userId));
        }
    }
}
