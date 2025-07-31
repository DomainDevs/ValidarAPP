// -----------------------------------------------------------------------
// <copyright file="ModelAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.PrintingParamServices.EEProvider.Assemblers
{
    using System;
    using System.Collections.Generic;
    using Sistran.Core.Application.Parameters.Entities;    
    using Sistran.Core.Application.PrintingParamServices.Models;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Application.Common.Entities;


   
   
    


    /// <summary>
    /// Clase enmbladora para mapear entidades a modelos de negocio.
    /// </summary>
    public class ModelAssembler
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ModelAssembler"/>.
        /// </summary>
        protected ModelAssembler()
        {
        }

        /// <summary>
        /// Mapeo de la entidad CptAlliancePrintFormat al modelo de negocio ParamCptAlliancePrintFormat.
        /// </summary>
        /// <param name="cptAlliancePrintFormat">Entidad de tipo CptAlliancePrintFormat.</param>
        /// <returns>Modelo de negocio ParamCptAlliancePrintFormat.</returns>
        public static Result<ParamCptAlliancePrintFormat, ErrorModel> CreateCptAlliancePrintFormats(CptAlliancePrintFormat cptAlliancePrintFormat)
        {
            Result<ParamCptAlliancePrintFormat, ErrorModel> result = ParamCptAlliancePrintFormat.GetParamCptAlliancePrintFormat(cptAlliancePrintFormat.AlliancePrintFormatId, cptAlliancePrintFormat.PrefixCode, cptAlliancePrintFormat.EndoTypeCode, cptAlliancePrintFormat.Format, cptAlliancePrintFormat.Enable);
            return result;
        }

        /// <summary>
        /// Mapeo de la entidad EndorsementType al modelo de negocio ParamEndoresementType.
        /// </summary>
        /// <param name="endorsementType">Entidad de tipo EndorsementType.</param>
        /// <returns>Modelo de negocio ParamEndoresementType.</returns>
        public static Result<ParamEndoresementType, ErrorModel> CreateEndorsementType(EndorsementType endorsementType)
        {
            Result<ParamEndoresementType, ErrorModel> result = ParamEndoresementType.GetEndoresementType(endorsementType.EndoTypeCode, endorsementType.Description);
            return result;
        }

        /// <summary>
        /// Mapeo de la entidad Prefix al modelo de negocio ParamPrefix.
        /// </summary>
        /// <param name="prefix">Entidad de tipo Prefix.</param>
        /// <returns>Modelo de negocio ParamPrefix.</returns>
        public static Result<ParamPrefix, ErrorModel> CreatePrefix(Prefix prefix)
        {
            Result<ParamPrefix, ErrorModel> result = ParamPrefix.GetParamPrefix(prefix.PrefixCode, prefix.Description,prefix.SmallDescription);
            return result;
        }

        /// <summary>
        /// Mapeo lista de objeto businessCollection a lista modelo ParamEndoresementType
        /// </summary>
        /// <param name="businessCollection">Colleccion de entidades EndorsementType</param>
        /// <returns>Listado de modelos de negocio ParamEndoresementType.</returns>
        public static Result<List<ParamEndoresementType>, ErrorModel> MappEndoresementTypes(BusinessCollection businessCollection)
        {
            List<ParamEndoresementType> listEndoresementType = new List<ParamEndoresementType>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamEndoresementType, ErrorModel> result;
            foreach (EndorsementType entityEndorsementType in businessCollection)
            {
                result = CreateEndorsementType(entityEndorsementType);
                if (result is ResultError<ParamEndoresementType, ErrorModel>)
                {                    
                    errorModelListDescription.Add(Resources.Errors.AllyPrintFormatEndoTypeMappingEntityError);
                    return new ResultError<List<ParamEndoresementType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamEndoresementType resultValue = (result as ResultValue<ParamEndoresementType, ErrorModel>).Value;
                    listEndoresementType.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamEndoresementType>, ErrorModel>(listEndoresementType);
        }

        /// <summary>
        /// Mapeo lista de objeto businessCollection a lista modelo ParamPrefix.
        /// </summary>
        /// <param name="businessCollection">Colleccion de entidades Prefix</param>
        /// <returns>Listado de modelos de negocio ParamPrefix.</returns>
        public static Result<List<ParamPrefix>, ErrorModel> MappParamPrefixs(BusinessCollection businessCollection)
        {
            List<ParamPrefix> listPrefix = new List<ParamPrefix>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamPrefix, ErrorModel> result;
            foreach (Prefix entityPrefix in businessCollection)
            {
                result = CreatePrefix(entityPrefix);
                if (result is ResultError<ParamPrefix, ErrorModel>)
                {                    
                    errorModelListDescription.Add(Resources.Errors.AllyPrintFormatPrefixMappingEntityError);
                    return new ResultError<List<ParamPrefix>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamPrefix resultValue = (result as ResultValue<ParamPrefix, ErrorModel>).Value;
                    listPrefix.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamPrefix>, ErrorModel>(listPrefix);
        }

        /// <summary>
        /// Mapeo lista de objeto businessCollection a lista modelo ParamCptAlliancePrintFormat
        /// </summary>
        /// <param name="businessCollection">Objeto businessCollection</param>
        /// <returns>Lista de Modelos de negocio ParamCptAlliancePrintFormat</returns>
        public static Result<List<ParamCptAlliancePrintFormat>, ErrorModel> MappCptAlliancePrintFormats(BusinessCollection businessCollection)
        {
            List<ParamCptAlliancePrintFormat> listCptAlliancePrintFormat = new List<ParamCptAlliancePrintFormat>();
            List<string> errorModelListDescription = new List<string>();
            Result<ParamCptAlliancePrintFormat, ErrorModel> result;            
            foreach (CptAlliancePrintFormat entityCptAlliancePrintFormat in businessCollection)
            {
                result = CreateCptAlliancePrintFormats(entityCptAlliancePrintFormat);
                if (result is ResultError<ParamCptAlliancePrintFormat, ErrorModel>)
                {                    
                    errorModelListDescription.Add(Resources.Errors.AllyPrintFormatAlliancePrintFormatMappingEntityError);
                    return new ResultError<List<ParamCptAlliancePrintFormat>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                }
                else
                {
                    ParamCptAlliancePrintFormat resultValue = (result as ResultValue<ParamCptAlliancePrintFormat, ErrorModel>).Value;
                    listCptAlliancePrintFormat.Add(resultValue);
                }                
            }

            return new ResultValue<List<ParamCptAlliancePrintFormat>, ErrorModel>(listCptAlliancePrintFormat);
        }

    }
}
