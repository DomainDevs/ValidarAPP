// -----------------------------------------------------------------------
// <copyright file="Infringement.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.VehicleParamService.Models
{
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using System.Collections.Generic;

    public class Infringement
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly string infringementCode;

        /// <summary>
        /// 
        /// </summary>
        private readonly int? infringementPreviousCode;

        /// <summary>
        /// 
        /// </summary>
        private readonly string description;

        /// <summary>
        /// 
        /// </summary>
        private readonly int? infringementGroupCode;

        /// <summary>
        /// 
        /// </summary>
        private readonly string infringementGroupDescription;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infringementCode"></param>
        /// <param name="infringementPreviousCode"></param>
        /// <param name="description"></param>
        /// <param name="infringementGroupCode"></param>
        /// <param name="infringementGroupDescription"></param>
        private Infringement(string infringementCode, int? infringementPreviousCode, string description, int? infringementGroupCode, string infringementGroupDescription)
        {
            this.infringementCode = infringementCode;
            this.infringementPreviousCode = infringementPreviousCode;
            this.description = description;
            this.infringementGroupCode = infringementGroupCode;
            this.infringementGroupDescription = infringementGroupDescription;
        }

        /// <summary>
        /// 
        /// </summary>
        public string InfringementCode
        {
            get
            {
                return this.infringementCode;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? InfrigementPreviousCode
        {
            get
            {
                return this.infringementPreviousCode;
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
        public int? InfrigemenGroupCode
        {
            get
            {
                return this.infringementGroupCode;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string InfrigemenGroupDescription
        {
            get
            {
                return this.infringementGroupDescription;
            }
        }

        public static ResultValue<Infringement, ErrorModel> GetInfringement(string infringementCode, int? infringementPreviousCode, string description, int? infringementGroupCode, string infringementGroupDescription)
        {
            return new ResultValue<Infringement, ErrorModel>(new Infringement(infringementCode, infringementPreviousCode, description, infringementGroupCode, infringementGroupDescription));
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
        public static Result<Infringement, ErrorModel> CreateInfringement(string infringementCode, int infringementPreviousCode, string description, int infringementGroupCode, string infringementGroupDescription)
        {
            List<string> error = new List<string>();
            if (infringementCode.Length >= 10)
            {
                error.Add("El código no puede tener más de 10 caracteres");
                return new ResultError<Infringement, ErrorModel>(ErrorModel.CreateErrorModel(error, ErrorType.BusinessFault, null));
            }
            if (infringementPreviousCode <= 0 && infringementPreviousCode >= 9999)
            {
                error.Add("El código previo no se encuentra en el rango (0-9999)");
                return new ResultError<Infringement, ErrorModel>(ErrorModel.CreateErrorModel(error, ErrorType.BusinessFault, null));
            }
            if (description.Length > 1000)
            {
                error.Add("La descripción no puede ser mayor de 1000 caracteres.");
                return new ResultError<Infringement, ErrorModel>(ErrorModel.CreateErrorModel(error, ErrorType.BusinessFault, null));
            }
            if (infringementGroupCode <= 0)
            {
                error.Add("El grupo de infracción no existe");
                return new ResultError<Infringement, ErrorModel>(ErrorModel.CreateErrorModel(error, ErrorType.BusinessFault, null));
            }
            else
            {
                return new ResultValue<Infringement, ErrorModel>(new Infringement(infringementCode, infringementPreviousCode, description, infringementGroupCode, infringementGroupDescription));
            }
        }
    }
}
