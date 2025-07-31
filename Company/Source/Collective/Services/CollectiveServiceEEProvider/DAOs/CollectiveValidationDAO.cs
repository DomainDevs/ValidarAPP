using Sistran.Co.Application.Data;
using Sistran.Company.Application.CollectiveServices.EEProvider.Resources;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
//using UPENUM = Sistran.Core.Application.UniquePersonService.Enums;

namespace Sistran.Company.Application.CollectiveServices.EEProvider.DAOs
{
    public class CollectiveValidationDAO
    {
        public List<ValidationIdentificator> GetGeneralValidationsByEmissionTemplate(File file, Row row, int userId)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BranchId).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.Branch;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId).Description;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                validations.Add(validation);
            }

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.SalePoint).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.SalePoint;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.SalePoint).Description;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                validation.ParameterValue2 = userId;
                validation.ParameterValue3 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.SalePoint));
                validations.Add(validation);
            }

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.HolderInsuredCode).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderInsuredCode).Description;
                validation.ValidationId = (int)ValidationProperty.Holder;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderInsuredCode));
                validations.Add(validation);
            }
            else
            {
                IndividualType individualType = new IndividualType();
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.HolderIndividualType).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderIndividualType).Description;
                    validation.ValidationId = (int)ValidationProperty.IndividualType;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderIndividualType));
                    validations.Add(validation);

                    individualType = (IndividualType)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderIndividualType));
                    switch (individualType)
                    {
                        case IndividualType.Company:

                            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.HolderCompanyDocumentType).Value))
                            {
                                validation = new ValidationIdentificator();
                                validation.Id = file.Id;
                                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyDocumentType).Description;
                                validation.ValidationId = (int)ValidationProperty.TributaryType;
                                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyDocumentType));
                                validations.Add(validation);
                            }

                            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.HolderIndividualType).Value))
                            {
                                validation = new ValidationIdentificator();
                                validation.Id = file.Id;
                                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderIndividualType).Description;
                                validation.ValidationId = (int)ValidationProperty.CompanyType;
                                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderIndividualType));
                                validations.Add(validation);
                            }
                            break;

                        case IndividualType.Person:

                            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.HolderPersonDocumentType).Value))
                            {
                                validation = new ValidationIdentificator();
                                validation.Id = file.Id;
                                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonDocumentType).Description;
                                validation.ValidationId = (int)ValidationProperty.DocumentType;
                                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonDocumentType));
                                validations.Add(validation);
                            }
                            break;
                    }
                }
                if (!CanFindHolderByDocNumberAndDocType(row, individualType))
                {
                    row.HasError = true;
                    row.ErrorDescription += string.Format(Errors.ErrorHolderNotExist + KeySettings.ReportErrorSeparatorMessage());
                }
            }
            return validations;
        }

        public List<ValidationRegularExpression> GetRegularExpressionValidationsByEmisionTemplate(int fileId, Row row)
        {
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();
            //-- Validacion email 
            if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.HolderEmailDescription).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.HolderEmailDescription).Value))
            {
                ValidationRegularExpression validation = new ValidationRegularExpression();
                validation.Id = fileId;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEmailDescription).Description;
                validation.ValidationId = (int)RegularExpression.email;
                validation.ParameterValue = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEmailDescription));
                validationRegularExpressions.Add(validation);
            }

            if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.PolicyCurrentFrom).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.PolicyCurrentFrom).Value))
            {
                ValidationRegularExpression validation = new ValidationRegularExpression();
                validation.Id = fileId;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom).Description;
                validation.ValidationId = (int)RegularExpression.date;
                validation.ParameterValue = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));
                validationRegularExpressions.Add(validation);
            }

            if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.PolicyCurrentTo).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.PolicyCurrentTo).Value))
            {
                ValidationRegularExpression validation = new ValidationRegularExpression();
                validation.Id = fileId;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentTo).Description;
                validation.ValidationId = (int)RegularExpression.date;
                validation.ParameterValue = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentTo));
                validationRegularExpressions.Add(validation);
            }

            //-- Validacion genero
            if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.HolderPersonGender).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.HolderPersonGender).Value))
            {
                ValidationRegularExpression validation = new ValidationRegularExpression();
                validation.Id = fileId;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonGender).Description;
                validation.ValidationId = (int)RegularExpression.gender;
                validation.ParameterValue = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonGender));
                validationRegularExpressions.Add(validation);
            }
            return validationRegularExpressions;
        }

        public List<ValidationRegularExpression> GetRegularExpressionValidationsByRiskTemplate(int fileId, Row row)
        {
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();

            //-- Validacion email 
            if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.InsuredEmailDescription).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredEmailDescription).Value))
            {
                ValidationRegularExpression validation = new ValidationRegularExpression();
                validation.Id = fileId;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailDescription).Description;
                validation.ValidationId = (int)RegularExpression.email;
                validation.ParameterValue = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailDescription));
                validationRegularExpressions.Add(validation);
            }
            if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.BeneficiaryEmailDescription).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryEmailDescription).Value))
            {
                ValidationRegularExpression validation = new ValidationRegularExpression();
                validation.Id = fileId;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailDescription).Description;
                validation.ValidationId = (int)RegularExpression.email;
                validation.ParameterValue = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailDescription));
                validationRegularExpressions.Add(validation);
            }

            //-- Validacion genero
            if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.InsuredPersonGender).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredPersonGender).Value))
            {
                ValidationRegularExpression validation = new ValidationRegularExpression();
                validation.Id = fileId;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonGender).Description;
                validation.ValidationId = (int)RegularExpression.gender;
                validation.ParameterValue = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonGender));
                validationRegularExpressions.Add(validation);
            }
            if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.BeneficiaryPersonGender).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryPersonGender).Value))
            {
                ValidationRegularExpression validation = new ValidationRegularExpression();
                validation.Id = fileId;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonGender).Description;
                validation.ValidationId = (int)RegularExpression.gender;
                validation.ParameterValue = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonGender));
                validationRegularExpressions.Add(validation);
            }

            if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.RiskRate).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskRate).Value))
            {
                ValidationRegularExpression validation = new ValidationRegularExpression();
                validation.Id = fileId;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRate).Description;
                validation.ValidationId = (int)RegularExpression.RiskRate;
                validation.ParameterValue = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRate));
                validationRegularExpressions.Add(validation);
            }

            return validationRegularExpressions;
        }

        public List<ValidationRegularExpression> GetRegularExpressionValidationsByAdditionalBeneficiaries(int fileId, Template template)
        {
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();
            int countAdditional = 1;

            foreach (Row row in template.Rows)
            {
                if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.InsuredEmailDescription).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryEmailDescription).Value))
                {
                    ValidationRegularExpression validation = new ValidationRegularExpression();
                    validation.Id = fileId;
                    validation.AdditionalRow = countAdditional;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailDescription).Description;
                    validation.ValidationId = (int)RegularExpression.email;
                    validation.ParameterValue = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailDescription));
                    validationRegularExpressions.Add(validation);
                }
                if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.BeneficiaryPersonGender).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryPersonGender).Value))
                {
                    ValidationRegularExpression validation = new ValidationRegularExpression();
                    validation.Id = fileId;
                    validation.AdditionalRow = countAdditional;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonGender).Description;
                    validation.ValidationId = (int)RegularExpression.gender;
                    validation.ParameterValue = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonGender));
                    validationRegularExpressions.Add(validation);
                }
                countAdditional++;
            }

            return validationRegularExpressions;
        }

        public List<ValidationIdentificator> GetGeneralValidationsByRiskTemplate(int fileId, Row row)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();

            #region Asegurado
            GetGeneralValidationsByRiskTemplateInsured(fileId, row, validations);
            #endregion

            #region Beneficiario
            GetGeneralValidationsByRiskTemplateBeneficiary(fileId, row, validations);
            #endregion

            return validations;
        }

        public List<ValidationIdentificator> GetGeneralValidationsByRiskTemplateInsured(int fileId, Row row, List<ValidationIdentificator> validations)
        {
            if (row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.InsuredEqualHolder).Value != "True")
            {
                ValidationIdentificator validation = new ValidationIdentificator();
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredCode).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = fileId;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCode).Description;
                    validation.ValidationId = (int)ValidationProperty.Insured;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCode));
                    validations.Add(validation);
                }
                else
                {
                    IndividualType individualType = new IndividualType();
                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredIndividualType).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = fileId;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredIndividualType).Description;
                        validation.ValidationId = (int)ValidationProperty.IndividualType;
                        validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredIndividualType));
                        validations.Add(validation);

                        individualType = (IndividualType)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredIndividualType));

                        switch (individualType)
                        {
                            case IndividualType.Company:

                                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredCompanyDocumentType).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = fileId;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyDocumentType).Description;
                                    validation.ValidationId = (int)ValidationProperty.TributaryType;
                                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyDocumentType));
                                    validations.Add(validation);
                                }

                                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredCompanyType).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = fileId;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyType).Description;
                                    validation.ValidationId = (int)ValidationProperty.CompanyType;
                                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyType));
                                    validations.Add(validation);
                                }
                                break;

                            case IndividualType.Person:

                                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredPersonDocumentType).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = fileId;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonDocumentType).Description;
                                    validation.ValidationId = (int)ValidationProperty.DocumentType;
                                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonDocumentType));
                                    validations.Add(validation);
                                }

                                //if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredPersonMaritalStatus).Value))
                                //{
                                //    validation = new ValidationIdentificator();
                                //    validation.Id = fileId;
                                //    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonMaritalStatus).Description;
                                //    validation.ValidationId = (int)ValidationProperty.MaritalStatus;
                                //    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonMaritalStatus));
                                //    validations.Add(validation);
                                //}
                                break;
                        }
                    }
                    else
                    {
                        row.HasError = true;
                        row.ErrorDescription += " " + string.Format(Errors.ValidateTypeIndividualRequeried + KeySettings.ReportErrorSeparatorMessage());
                    }
                    if (!CanFindInsuredByDocNumberAndDocType(row, individualType))
                    {
                        row.HasError = true;
                        row.ErrorDescription += " " + string.Format(Errors.InsuredData + KeySettings.ReportErrorSeparatorMessage());
                    }
                }
            }
            return validations;
        }

        public List<ValidationIdentificator> GetGeneralValidationsByRiskTemplateBeneficiary(int fileId, Row row, List<ValidationIdentificator> validations)
        {
            if (row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryEqualInsured).Value != "True")
            {
                ValidationIdentificator validation = new ValidationIdentificator();
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryInsuredCode).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = fileId;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryInsuredCode).Description;
                    validation.ValidationId = (int)ValidationProperty.Beneficiary;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryInsuredCode));
                    validation.ParameterValue2 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryBeneficiaryType));
                    validations.Add(validation);
                }
                else
                {
                    IndividualType individualType = new IndividualType();
                    validation = new ValidationIdentificator();
                    validation.Id = fileId;
                    validation.ValidationId = (int)ValidationProperty.BeneficiaryType;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryBeneficiaryType).Description;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryBeneficiaryType));
                    validations.Add(validation);

                    if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.BeneficiaryIndividualType).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryIndividualType).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = fileId;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryIndividualType).Description;
                        validation.ValidationId = (int)ValidationProperty.IndividualType;
                        validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryIndividualType));
                        validations.Add(validation);

                        individualType = (IndividualType)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryIndividualType));
                        switch (individualType)
                        {
                            case IndividualType.Company:

                                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentType).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = fileId;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentType).Description;
                                    validation.ValidationId = (int)ValidationProperty.TributaryType;
                                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentType));
                                    validations.Add(validation);
                                }

                                if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.BeneficiaryCompanyType).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryCompanyType).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = fileId;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyType).Description;
                                    validation.ValidationId = (int)ValidationProperty.CompanyType;
                                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyType));
                                    validations.Add(validation);
                                }
                                break;

                            case IndividualType.Person:

                                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryPersonDocumentType).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = fileId;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonDocumentType).Description;
                                    validation.ValidationId = (int)ValidationProperty.DocumentType;
                                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonDocumentType));
                                    validations.Add(validation);
                                }
                                break;
                        }
                    }
                    if (!CanFindBeneficiaryByDocNumberAndDocType(row, individualType))
                    {
                        row.HasError = true;
                        row.ErrorDescription += " " + string.Format(Errors.ErrorBeneficiaryNotExistTemplateEmission + KeySettings.ReportErrorSeparatorMessage());
                    }
                }
            }
            return validations;
        }

        public List<ValidationPhoneType> GetPhoneTypesValidationsByEmissionTemplate(File file, Row row)
        {
            List<ValidationPhoneType> validations = new List<ValidationPhoneType>();

            if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.HolderPhoneDescription).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.HolderPhoneDescription).Value))
            {
                ValidationPhoneType validation = new ValidationPhoneType();
                validation.Id = file.Id;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneDescription).Description;
                validation.PhoneType = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneType));
                validation.Number = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneDescription));
                validations.Add(validation);
            }

            //validar si la propiedad es necesaria en previsora
            //if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.HolderCellNumber).Value))
            //{
            //    ValidationPhoneType validation = new ValidationPhoneType();
            //    validation.Id = file.Id;
            //    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.HolderCellNumber).Description;
            //    validation.PhoneType = DelegateService.commonService.GetExtendedParameterByParameterId((int)CompanyParameterType.CellPhone).NumberParameter.GetValueOrDefault();
            //    validation.Number = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.HolderCellNumber));
            //    validations.Add(validation);
            //}

            return validations;
        }

        public List<ValidationPhoneType> GetPhoneTypesValidationsByRiskDetailTemplate(int fileId, Template template)
        {
            List<ValidationPhoneType> validations = new List<ValidationPhoneType>();

            foreach (Row row in template.Rows)
            {
                if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.InsuredPhoneDescription).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredPhoneDescription).Value))
                {
                    ValidationPhoneType validation = new ValidationPhoneType();
                    validation.Id = fileId;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPhoneDescription).Description;
                    validation.PhoneType = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPhoneType));
                    validation.Number = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPhoneDescription));
                    validations.Add(validation);
                }
                //Validar si la propiedad es necesaria en previsora
                //if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.InsuredCellNumber).Value))
                //{
                //    ValidationPhoneType validation = new ValidationPhoneType();
                //    validation.Id = fileId;
                //    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.InsuredCellNumber).Description;
                //    validation.PhoneType = DelegateService.commonService.GetExtendedParameterByParameterId((int)CompanyParameterType.CellPhone).NumberParameter.GetValueOrDefault();
                //    validation.Number = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.InsuredCellNumber));
                //    validations.Add(validation);
                //}

                if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.BeneficiaryPhoneDescription).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryPhoneDescription).Value))
                {
                    ValidationPhoneType validation = new ValidationPhoneType();
                    validation.Id = fileId;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPhoneDescription).Description;
                    validation.PhoneType = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPhoneType));
                    validation.Number = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPhoneDescription));
                    validations.Add(validation);
                }
                //Validar si la propiedad es necesaria en previsora
                //if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.BeneficiaryCellNumber).Value))
                //{
                //    ValidationPhoneType validation = new ValidationPhoneType();
                //    validation.Id = fileId;
                //    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.BeneficiaryCellNumber).Description;
                //    validation.PhoneType = DelegateService.commonService.GetExtendedParameterByParameterId((int)CompanyParameterType.CellPhone).NumberParameter.GetValueOrDefault();
                //    validation.Number = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.BeneficiaryCellNumber));
                //    validations.Add(validation);
                //}
            }
            return validations;
        }

        public List<ValidationIdentificator> GetValidationsByAdditionalBeneficiariesTemplate(File file, Template template)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();
            int countAdditional = 1;

            foreach (Row row in template.Rows)
            {
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryInsuredCode).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.Beneficiary;
                    validation.AdditionalRow = countAdditional;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryInsuredCode).Description;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryInsuredCode));
                    validation.ParameterValue2 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryBeneficiaryType));
                    validations.Add(validation);
                    countAdditional++;
                }
                else
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.BeneficiaryType;
                    validation.AdditionalRow = countAdditional;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryBeneficiaryType).Description;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryBeneficiaryType));
                    validations.Add(validation);
                    IndividualType individualType = new IndividualType();

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryIndividualType).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.IndividualType;
                        validation.AdditionalRow = countAdditional;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryIndividualType).Description;
                        validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryIndividualType));
                        validations.Add(validation);

                        individualType = (IndividualType)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryIndividualType));
                        switch (individualType)
                        {
                            case IndividualType.Company:

                                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentType).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = file.Id;
                                    validation.ValidationId = (int)ValidationProperty.TributaryType;
                                    validation.AdditionalRow = countAdditional;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentType).Description;
                                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentType));
                                    validations.Add(validation);
                                }

                                if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.BeneficiaryCompanyType).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryCompanyType).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = file.Id;
                                    validation.ValidationId = (int)ValidationProperty.CompanyType;
                                    validation.AdditionalRow = countAdditional;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyType).Description;
                                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyType));
                                    validations.Add(validation);
                                }
                                break;

                            case IndividualType.Person:

                                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryPersonDocumentType).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = file.Id;
                                    validation.ValidationId = (int)ValidationProperty.DocumentType;
                                    validation.AdditionalRow = countAdditional;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonDocumentType).Description;
                                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonDocumentType));
                                    validations.Add(validation);
                                }
                                break;
                        }
                    }
                    if (!CanFindBeneficiaryByDocNumberAndDocType(row, individualType))
                    {
                        row.HasError = true;
                        row.ErrorDescription += " " + string.Format(Errors.ErrorBeneficiaryNotExistTemplateBeneficiary + KeySettings.ReportErrorSeparatorMessage());
                    }
                    countAdditional++;
                }
            }
            return validations;
        }

        public List<ValidationPhoneType> GetPhoneTypeValidationsByAdditionalBeneficiaries(int fileId, Template template)
        {
            List<ValidationPhoneType> validationsPhoneType = new List<ValidationPhoneType>();
            int countAdditional = 1;

            foreach (Row row in template.Rows)
            {
                if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.BeneficiaryPhoneDescription).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryPhoneDescription).Value))
                {
                    ValidationPhoneType validation = new ValidationPhoneType();
                    validation.Id = fileId;
                    validation.AdditionalRow = countAdditional;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPhoneDescription).Description;
                    validation.PhoneType = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPhoneType));
                    validation.Number = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPhoneDescription));
                    validationsPhoneType.Add(validation);
                }
                //Validar si la propiedad es necesaria en previsora
                //if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.BeneficiaryCellNumber).Value))
                //{
                //    ValidationPhoneType validation = new ValidationPhoneType();
                //    validation.Id = fileId;
                //    validation.AdditionalRow = countAdditional;
                //    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.BeneficiaryCellNumber).Description;
                //    validation.PhoneType = DelegateService.commonService.GetExtendedParameterByParameterId((int)CompanyParameterType.CellPhone).NumberParameter.GetValueOrDefault();
                //    validation.Number = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.BeneficiaryCellNumber));
                //    validationsPhoneType.Add(validation);
                //}

                countAdditional++;
            }
            return validationsPhoneType;
        }

        private bool CanFindHolderByDocNumberAndDocType(Row row, IndividualType individualType)
        {
            string companyDocType = row.Fields.First(y => y.PropertyName == FieldPropertyName.HolderCompanyDocumentType).Value;
            string companyDocNumber = row.Fields.Find(y => y.PropertyName == FieldPropertyName.HolderCompanyDocumentNumber).Value;
            string personDocType = row.Fields.Find(y => y.PropertyName == FieldPropertyName.HolderPersonDocumentType).Value;
            string personDocNumber = row.Fields.Find(y => y.PropertyName == FieldPropertyName.HolderPersonDocumentNumber).Value;

            if (individualType == IndividualType.Person)
            {
                if (personDocType == string.Empty || personDocNumber == string.Empty)
                {
                    row.HasError = true;
                    row.ErrorDescription += " " + string.Format(Errors.ErrorTypeAndDocumentNumberAreRequired, row.Fields.First(y => y.PropertyName == FieldPropertyName.HolderPersonDocumentType).Description, row.Fields.Find(y => y.PropertyName == FieldPropertyName.HolderPersonDocumentNumber).Description) + KeySettings.ReportErrorSeparatorMessage();
                    return true;
                }
            }
            else if (individualType == IndividualType.Company)
            {
                if (companyDocType == string.Empty || companyDocNumber == string.Empty)
                {
                    row.HasError = true;
                    row.ErrorDescription += " " + string.Format(Errors.ErrorTypeAndDocumentNumberAreRequired, row.Fields.First(y => y.PropertyName == FieldPropertyName.HolderCompanyDocumentType).Description, row.Fields.Find(y => y.PropertyName == FieldPropertyName.HolderCompanyDocumentNumber).Description) + KeySettings.ReportErrorSeparatorMessage();
                    return true;
                }
            }
            return CanFindIndividualByDocNumberAndDocType(companyDocType, companyDocNumber, personDocType, personDocNumber);

        }

        private bool CanFindInsuredByDocNumberAndDocType(Row row, IndividualType individualType)
        {
            string companyDocType = row.Fields.First(y => y.PropertyName == FieldPropertyName.InsuredCompanyDocumentType).Value;
            string companyDocNumber = row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredCompanyDocumentNumber).Value;
            string personDocType = row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredPersonDocumentType).Value;
            string personDocNumber = row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredPersonDocumentNumber).Value;

            if (individualType == IndividualType.Person)
            {
                if (personDocType == string.Empty || personDocNumber == string.Empty)
                {
                    row.HasError = true;
                    row.ErrorDescription += " " + string.Format(Errors.ErrorTypeAndDocumentNumberAreRequired, row.Fields.First(y => y.PropertyName == FieldPropertyName.InsuredPersonDocumentType).Description, row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredPersonDocumentNumber).Description) + KeySettings.ReportErrorSeparatorMessage();
                    return true;
                }
            }
            else if (individualType == IndividualType.Company)
            {
                if (companyDocType == string.Empty || companyDocNumber == string.Empty)
                {
                    row.HasError = true;
                    row.ErrorDescription += " " + string.Format(Errors.ErrorTypeAndDocumentNumberAreRequired, row.Fields.First(y => y.PropertyName == FieldPropertyName.InsuredCompanyDocumentType).Description, row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredCompanyDocumentNumber).Description) + KeySettings.ReportErrorSeparatorMessage();
                    return true;
                }
            }
            return CanFindIndividualByDocNumberAndDocType(companyDocType, companyDocNumber, personDocType, personDocNumber);

        }

        private bool CanFindBeneficiaryByDocNumberAndDocType(Row row, IndividualType individualType)
        {
            string companyDocType = row.Fields.First(y => y.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentType).Value;
            string companyDocNumber = row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentNumber).Value;
            string personDocType = row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryPersonDocumentType).Value;
            string personDocNumber = row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryPersonDocumentNumber).Value;

            if (individualType == IndividualType.Person)
            {
                if (personDocType == string.Empty || personDocNumber == string.Empty)
                {
                    row.HasError = true;
                    row.ErrorDescription += " " + string.Format(Errors.ErrorTypeAndDocumentNumberAreRequired, row.Fields.First(y => y.PropertyName == FieldPropertyName.BeneficiaryPersonDocumentType).Description, row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryPersonDocumentNumber).Description) + KeySettings.ReportErrorSeparatorMessage();
                    return true;
                }
            }
            else if (individualType == IndividualType.Company)
            {
                if (companyDocType == string.Empty || companyDocNumber == string.Empty)
                {
                    row.HasError = true;
                    row.ErrorDescription += " " + string.Format(Errors.ErrorTypeAndDocumentNumberAreRequired, row.Fields.First(y => y.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentType).Description, row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentNumber).Description) + KeySettings.ReportErrorSeparatorMessage();
                    return true;
                }
            }
            return CanFindIndividualByDocNumberAndDocType(companyDocType, companyDocNumber, personDocType, personDocNumber);

        }

        private bool CanFindIndividualByDocNumberAndDocType(string companyDocType, string companyDocNumber, string personDocType, string personDocNumber)
        {

            NameValue[] parameters = new NameValue[3];

            if (!string.IsNullOrEmpty(companyDocType) && !string.IsNullOrEmpty(companyDocNumber))
            {
                parameters[0] = new NameValue("@INDIVIDUAL_TYPE_ID", (int)IndividualType.Company);
                parameters[1] = new NameValue("@DOCUMENT_TYPE_ID", Convert.ToInt32(companyDocType));
                parameters[2] = new NameValue("@DOCUMENT_NUMBER", companyDocNumber);

            }
            else if (!string.IsNullOrEmpty(personDocType) && !string.IsNullOrEmpty(personDocNumber))
            {
                parameters[0] = new NameValue("@INDIVIDUAL_TYPE_ID", (int)IndividualType.Person);
                parameters[1] = new NameValue("@DOCUMENT_TYPE_ID", Convert.ToInt32(personDocType));
                parameters[2] = new NameValue("@DOCUMENT_NUMBER", personDocNumber);
            }
            else
            {
                return false;
            }

            object result = null;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPScalar("MSV.CAN_FIND_INDIVIDUAL_BY_DOCUMENT", parameters);
            }

            return System.Convert.ToBoolean(result);

        }
    }
}
