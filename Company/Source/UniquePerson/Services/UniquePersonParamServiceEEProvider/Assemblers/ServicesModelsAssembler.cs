// -----------------------------------------------------------------------
// <copyright file="ServicesModelsAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
using Sistran.Company.Application.ModelServices.Models;
using Sistran.Company.Application.ModelServices.Models.UniquePerson;
using Sistran.Company.Application.UniquePersonParamService.EEProvider.Resources;
using Sistran.Company.Application.UniquePersonParamService.Models;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Application.Utilities.Error;
using System.Collections.Generic;

namespace Sistran.Company.Application.UniquePersonParamService.EEProvider.Assemblers
{
    /// <summary>
    /// Clase ensambladora para mapear modelos de servicios a modelos de negocio.
    /// </summary>
    public class ServicesModelsAssembler
    {

        /// <summary>
        /// Mapear modelos de negocio ParamLegalRepresentativeSing a modelos de Servicio LegalRepresentativeSingServiceModel.
        /// </summary>
        /// <param name="listLegalRepresentativeSingServiceModels">Modelos de negocio ParamLegalRepresentativeSing.</param>
        /// <returns>Modelos de Servicio LegalRepresentativeSingServiceModel</returns>
        public static Result<List<ParamLegalRepresentativeSing>, ErrorModel> MappListParamLegalRepresentativeSing(List<LegalRepresentativeSingServiceModel> listLegalRepresentativeSingServiceModels)
        {
            List<string> errorListDescription = new List<string>();
            List<ParamLegalRepresentativeSing> listParamLegalRepresentativeSing = new List<ParamLegalRepresentativeSing>();

            foreach (LegalRepresentativeSingServiceModel legalRepresentativeSingServiceModel in listLegalRepresentativeSingServiceModels)
            {
                ParamCompanyType paramCompanyType;
                Result<ParamCompanyType, ErrorModel> resultParamCompanyType = ParamCompanyType.GetParamCompanyType(legalRepresentativeSingServiceModel.CompanyTypeServiceModel.Id, legalRepresentativeSingServiceModel.CompanyTypeServiceModel.Description);
                if (resultParamCompanyType is ResultError<ParamCompanyType, ErrorModel>)
                {
                    errorListDescription.Add(Errors.ErrorMappingServiceModelAndBusinessModelCompany);
                    return new ResultError<List<ParamLegalRepresentativeSing>, ErrorModel>(ErrorModel.CreateErrorModel(errorListDescription, ErrorType.BusinessFault, null));
                }

                paramCompanyType = (resultParamCompanyType as ResultValue<ParamCompanyType, ErrorModel>).Value;

                ParamBranchType paramBranchType;
                Result<ParamBranchType, ErrorModel> resultParamBranchType = ParamBranchType.CreateParamBranchType(legalRepresentativeSingServiceModel.BranchTypeServiceModel.Id, legalRepresentativeSingServiceModel.BranchTypeServiceModel.Description);
                if (resultParamBranchType is ResultError<ParamBranchType, ErrorModel>)
                {
                    errorListDescription.Add(Errors.ErrorMappingServiceModelAndBusinessModelBranch);
                    return new ResultError<List<ParamLegalRepresentativeSing>, ErrorModel>(ErrorModel.CreateErrorModel(errorListDescription, ErrorType.BusinessFault, null));
                }

                paramBranchType = (resultParamBranchType as ResultValue<ParamBranchType, ErrorModel>).Value;

                Result<ParamLegalRepresentativeSing, ErrorModel> itemResult;
                itemResult = ParamLegalRepresentativeSing.CreateParamLegalRepresentativeSing(
                    paramCompanyType,
                    paramBranchType,
                    legalRepresentativeSingServiceModel.CurrentFrom,
                    legalRepresentativeSingServiceModel.LegalRepresentative,
                    legalRepresentativeSingServiceModel.PathSignatureImg,
                    legalRepresentativeSingServiceModel.SignatureImg,
                    legalRepresentativeSingServiceModel.UserId);

                if (itemResult is ResultError<ParamLegalRepresentativeSing, ErrorModel>)
                {
                    ErrorModel errorModelResult = (itemResult as ResultError<ParamLegalRepresentativeSing, ErrorModel>).Message;
                    return new ResultError<List<ParamLegalRepresentativeSing>, ErrorModel>(errorModelResult);
                }
                else
                {
                    ParamLegalRepresentativeSing resultValue = (itemResult as ResultValue<ParamLegalRepresentativeSing, ErrorModel>).Value;
                    listParamLegalRepresentativeSing.Add(resultValue);
                }
            }

            return new ResultValue<List<ParamLegalRepresentativeSing>, ErrorModel>(listParamLegalRepresentativeSing);
        }


        /// <summary>
        /// Mapear modelos de servicio BasicCompanyServiceModel a modelos de Negocio ParamBasicCompany.
        /// </summary>
        /// <param name="basicCompanyServiceModel">Modelos de negocio BasicCompanyServiceModel.</param>
        /// <returns>Modelos de Negocio Result<List<ParamBasicCompany>, ErrorModel></returns>
        public static Result<ParamBasicCompany, ErrorModel> MappListParamBasicCompany(BasicCompanyServiceModel basicCompanyServiceModel)
        {
            List<string> errorListDescription = new List<string>();
            ParamBasicCompany paramBasicCompany = new ParamBasicCompany();
            Result<ParamBasicCompany, ErrorModel> resultParamBasicCompany = ParamBasicCompany.GetParamBasicCompany(basicCompanyServiceModel.IndividualId,
                basicCompanyServiceModel.DocumentType,
                basicCompanyServiceModel.DocumentNumber,
                basicCompanyServiceModel.CompanyDigit,
                basicCompanyServiceModel.CompanyCode,
                basicCompanyServiceModel.TradeName,
                basicCompanyServiceModel.TypePartnership,
                basicCompanyServiceModel.CompanyTypePartnership,
                basicCompanyServiceModel.Country,
                basicCompanyServiceModel.LastUpdate,
                basicCompanyServiceModel.UpdateBy,
                basicCompanyServiceModel.Policy,
                basicCompanyServiceModel.Insured,
                basicCompanyServiceModel.Beneficiary);
            if (resultParamBasicCompany is ResultError<ParamBasicCompany, ErrorModel>)
            {
                errorListDescription.Add(Errors.ErrorMappingServiceModelAndBusinessModelParamBasicCompany);
                return new ResultError<ParamBasicCompany, ErrorModel>(ErrorModel.CreateErrorModel(errorListDescription, ErrorType.BusinessFault, null));
            }
            paramBasicCompany = (resultParamBasicCompany as ResultValue<ParamBasicCompany, ErrorModel>).Value;
            return new ResultValue<ParamBasicCompany, ErrorModel>(paramBasicCompany);
        }


        /// <summary>
        /// Mapear modelos de servicio BasicCompanyServiceModel a modelos de Negocio ParamBasicCompany.
        /// </summary>
        /// <param name="basicCompanyServiceModel">Modelos de negocio BasicCompanyServiceModel.</param>
        /// <returns>Modelos de Negocio Result<List<ParamBasicCompany>, ErrorModel></returns>
        public static Result<ParamBasicPerson, ErrorModel> MappListParamBasicPerson(BasicPersonServiceModel basicPersonServiceModel)
        {
            List<string> errorListDescription = new List<string>();
            ParamBasicPerson paramBasicPerson = new ParamBasicPerson();
            Result<ParamBasicPerson, ErrorModel> resultParamBasicPerson = ParamBasicPerson.GetParameterBasicPerson(
                basicPersonServiceModel.IndividualId,
                basicPersonServiceModel.DocumentType,
                basicPersonServiceModel.DocumentNumber,
                basicPersonServiceModel.PersonCode,
                 basicPersonServiceModel.FirstName,
                 basicPersonServiceModel.LastName,
                 basicPersonServiceModel.Name,
                 basicPersonServiceModel.Gender,
                 basicPersonServiceModel.MaritalStatus,
                 basicPersonServiceModel.Birthdate,
                 basicPersonServiceModel.BirthPlace,
                 basicPersonServiceModel.LastUpdate,
                 basicPersonServiceModel.UpdateBy,
                 basicPersonServiceModel.Policy,
                 basicPersonServiceModel.Insured,
                 basicPersonServiceModel.Beneficiary);
            if (resultParamBasicPerson is ResultError<ParamBasicPerson, ErrorModel>)
            {
                errorListDescription.Add(Errors.ErrorMappingServiceModelAndBusinessModelParamBasicPerson);
                return new ResultError<ParamBasicPerson, ErrorModel>(ErrorModel.CreateErrorModel(errorListDescription, ErrorType.BusinessFault, null));
            }
            paramBasicPerson = (resultParamBasicPerson as ResultValue<ParamBasicPerson, ErrorModel>).Value;
            return new ResultValue<ParamBasicPerson, ErrorModel>(paramBasicPerson);
        }

    }
}
