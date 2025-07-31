// -----------------------------------------------------------------------
// <copyright file="EntityAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.PrintingParamServices.EEProvider.Assemblers
{
    using System.Collections.Generic;
    using Sistran.Core.Application.Parameters.Entities;
    using Sistran.Core.Application.PrintingParamServices.Models;
    
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Framework.DAF;    
    

    /// <summary>
    /// Clase enmbladora para mapear entidades a modelos de negocio.
    /// </summary>
    public class EntityAssembler
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ModelAssembler"/>.
        /// </summary>
        protected EntityAssembler()
        {
        }

        /// <summary>
        /// Mapear Modelo ParamCptAlliancePrintFormat a la Entidad CptAlliancePrintFormat
        /// </summary>
        /// <param name="paramCptAlliancePrintFormat">Modelo de negocio.</param>
        /// <returns>Entidad CptAlliancePrintFormat</returns>
        public static CptAlliancePrintFormat CreateCptAlliancePrintFormat(ParamCptAlliancePrintFormat paramCptAlliancePrintFormat)
        {
            return new CptAlliancePrintFormat()
            {
                PrefixCode = paramCptAlliancePrintFormat.PrefixCd,
                EndoTypeCode = paramCptAlliancePrintFormat.EndoTypeCd,
                Format = paramCptAlliancePrintFormat.Format,
                Enable = paramCptAlliancePrintFormat.Enable,
                
            };
        }
    }
}
