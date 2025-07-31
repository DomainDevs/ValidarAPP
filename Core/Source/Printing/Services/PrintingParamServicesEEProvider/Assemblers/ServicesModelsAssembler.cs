// -----------------------------------------------------------------------
// <copyright file="ServicesModelsAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.PrintingParamServices.EEProvider.Assemblers
{
    using System.Collections.Generic;
    using Sistran.Core.Application.ModelServices;
    using Sistran.Core.Application.ModelServices.Enums;    
    using Sistran.Core.Application.ModelServices.Models.Printing;
    using Sistran.Core.Application.PrintingParamServices.Models;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Clase ensambladora para mapear modelos de servicio a modelos de Negocio.
    /// </summary>
    public class ServicesModelsAssembler
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ServicesModelsAssembler"/>.
        /// </summary>
        protected ServicesModelsAssembler()
        {
        }

        /// <summary>
        /// Mapear modelos de negocio ParamCptAlliancePrintFormat a modelos de Servicio CptAlliancePrintFormatsServiceModel.
        /// </summary>
        /// <param name="paramCptAlliancePrintFormats">Modelos de negocio ParamCptAlliancePrintFormat.</param>
        /// <returns>Modelos de Servicio CptAlliancePrintFormatsServiceModel</returns>
        public static Result<List<ParamCptAlliancePrintFormat>, ErrorModel> MappListParamCptAlliancePrintFormats(List<CptAlliancePrintFormatServiceModel> listCptAlliancePrintFormatServiceModels)
        {
            List<string> errorListDescription = new List<string>();
            List<ParamCptAlliancePrintFormat> listParamCptAlliancePrintFormat = new List<ParamCptAlliancePrintFormat>();            
            foreach (CptAlliancePrintFormatServiceModel cptAlliancePrintFormatServiceModel in listCptAlliancePrintFormatServiceModels)
            {
                Result<ParamCptAlliancePrintFormat, ErrorModel> itemResult;
                itemResult = ParamCptAlliancePrintFormat.CreateParamCptAlliancePrintFormat(cptAlliancePrintFormatServiceModel.Id, cptAlliancePrintFormatServiceModel.PrefixServiceQueryModel.PrefixCode, cptAlliancePrintFormatServiceModel.EndorsementTypeServiceQueryModel.Id, cptAlliancePrintFormatServiceModel.Format, cptAlliancePrintFormatServiceModel.Enable);

                if (itemResult is ResultError<ParamCptAlliancePrintFormat, ErrorModel>)
                {
                    ErrorModel errorModelResult = (itemResult as ResultError<ParamCptAlliancePrintFormat, ErrorModel>).Message;
                    return new ResultError<List<ParamCptAlliancePrintFormat>, ErrorModel>(errorModelResult);
                }
                else
                {
                    ParamCptAlliancePrintFormat resultValue = (itemResult as ResultValue<ParamCptAlliancePrintFormat, ErrorModel>).Value;
                    listParamCptAlliancePrintFormat.Add(resultValue);
                }
            }
            
            return new ResultValue<List<ParamCptAlliancePrintFormat>, ErrorModel>(listParamCptAlliancePrintFormat);
        }
    }
}
