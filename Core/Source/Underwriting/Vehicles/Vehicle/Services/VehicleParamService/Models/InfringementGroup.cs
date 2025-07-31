// -----------------------------------------------------------------------
// <copyright file="InfringementGroup.cs" company="SISTRAN">
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
    /// Modelo de negocio de grupos de infracciones
    /// </summary>
    public class InfringementGroup
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly int infringementGroupCode;

        /// <summary>
        /// 
        /// </summary>
        private readonly string description;

        /// <summary>
        /// 
        /// </summary>
        private readonly int infringementOneYear;

        /// <summary>
        /// 
        /// </summary>
        private readonly int infringementThreeYear;

        /// <summary>
        /// 
        /// </summary>
        private readonly bool isActive;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infringementGroupCode"></param>
        /// <param name="description"></param>
        /// <param name="infringementOneYear"></param>
        /// <param name="infringementThreeYear"></param>
        /// <param name="isActive"></param>
        private InfringementGroup(int infringementGroupCode, string description, int infringementOneYear, int infringementThreeYear, bool isActive)
        {
            this.infringementGroupCode = infringementGroupCode;
            this.description = description;
            this.infringementOneYear = infringementOneYear;
            this.infringementThreeYear = infringementThreeYear;
            this.isActive = isActive;
        }

        /// <summary>
        /// 
        /// </summary>
        public int InfringementGroupCode
        {
            get
            {
                return this.infringementGroupCode;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int InfringementOneYear
        {
            get
            {
                return this.infringementOneYear;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int InfringementThreeYear
        {
            get
            {
                return this.infringementThreeYear;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsActive
        {
            get
            {
                return this.isActive;
            }
        }

        public static ResultValue<InfringementGroup, ErrorModel> GetInfringementGroup(int infringementGroupCode, string description, int infringementOneYear, int infringementThreeYear, bool isActive)
        {
            return new ResultValue<InfringementGroup, ErrorModel>(new InfringementGroup(infringementGroupCode, description, infringementOneYear, infringementThreeYear, isActive));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infringementGroupCode"></param>
        /// <param name="description"></param>
        /// <param name="infringementOneYear"></param>
        /// <param name="infringementThreeYear"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public static Result<InfringementGroup, ErrorModel> CreateInfringementGroup(int infringementGroupCode, string description, int infringementOneYear, int infringementThreeYear, bool isActive)
        {
            List<string> error = new List<string>();
            if (infringementGroupCode <= 0)
            {
                error.Add("El identificador no puede ser un valor negativo");
                return new ResultError<InfringementGroup, ErrorModel>(ErrorModel.CreateErrorModel(error, ErrorType.BusinessFault, null));
            }
            if (description.Length < 0 && description.Length > 60)
            {
                error.Add("La longitud de la descripción no se encuentra en el rango (10-250)");
                return new ResultError<InfringementGroup, ErrorModel>(ErrorModel.CreateErrorModel(error, ErrorType.BusinessFault, null));
            }
            if (infringementOneYear <= 0 && infringementOneYear > 9999)
            {
                error.Add("El periodo 1 no se encuentra en el rango (1-9999)");
                return new ResultError<InfringementGroup, ErrorModel>(ErrorModel.CreateErrorModel(error, ErrorType.BusinessFault, null));
            }
            if (infringementThreeYear <= 0 && infringementThreeYear > 9999)
            {
                error.Add("El periodo 2 no se encuentra en el rango (1-9999)");
                return new ResultError<InfringementGroup, ErrorModel>(ErrorModel.CreateErrorModel(error, ErrorType.BusinessFault, null));
            }
            else
            {
                return new ResultValue<InfringementGroup, ErrorModel>(new InfringementGroup(infringementGroupCode, description, infringementOneYear, infringementThreeYear, isActive));
            }
        }
    }
}