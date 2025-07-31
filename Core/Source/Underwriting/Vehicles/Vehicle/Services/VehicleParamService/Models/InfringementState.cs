// -----------------------------------------------------------------------
// <copyright file="InfringementState.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.VehicleParamService.Models
{
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using System.Collections.Generic;

    /// <summary>
    /// Modelo de negocio de estado de infracciones
    /// </summary>
    public class InfringementState
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly int infringementStateCode;

        /// <summary>
        /// 
        /// </summary>
        private readonly string infringementStateDescription;
        

        private InfringementState(int infringementStateCode, string infringementStateDescription)
        {
            this.infringementStateCode = infringementStateCode;
            this.infringementStateDescription = infringementStateDescription;
        }

        /// <summary>
        /// 
        /// </summary>
        public int InfringementStateCode
        {
            get
            {
                return this.infringementStateCode;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get
            {
                return this.infringementStateDescription;
            }
        }

        public static ResultValue<InfringementState, ErrorModel> GetInfringementState(int infringementStateCode, string infringementStateDescription)
        {
            return new ResultValue<InfringementState, ErrorModel>(new InfringementState(infringementStateCode, infringementStateDescription));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infringementStateCode"></param>
        /// <param name="infringementStateDescription"></param>
        /// <returns></returns>
        public static Result<InfringementState, ErrorModel> CreateInfringementState(int infringementStateCode, string infringementStateDescription)
        {
            List<string> error = new List<string>();
            if (infringementStateCode <= 0)
            {
                error.Add("El identificador no puede ser un valor negativo");
                return new ResultError<InfringementState, ErrorModel>(ErrorModel.CreateErrorModel(error, ErrorType.BusinessFault, null));
            }
            if (infringementStateDescription.Length < 0 && infringementStateDescription.Length > 60)
            {
                error.Add("La longitud de la descripción no se encuentra en el rango (10-250)");
                return new ResultError<InfringementState, ErrorModel>(ErrorModel.CreateErrorModel(error, ErrorType.BusinessFault, null));
            }
            else
            {
                return new ResultValue<InfringementState, ErrorModel>(new InfringementState(infringementStateCode, infringementStateDescription));
            }
        }
    }
}
