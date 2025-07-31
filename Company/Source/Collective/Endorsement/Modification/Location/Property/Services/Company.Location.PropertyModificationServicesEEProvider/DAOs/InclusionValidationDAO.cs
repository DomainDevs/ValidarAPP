using Sistran.Company.Application.CommonServices.Enums;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using UPENUM = Sistran.Core.Application.UniquePersonService.Enums;

namespace Sistran.Company.Application.Location.PropertyModificationService.EEProvider.DAOs
{
    public class InclusionValidationDAO
    {
        public List<Validation> GetValidationsByFiles(List<File> files, CollectiveEmission collectiveEmission, int productId, int policyType, Row emissionRow, DateTime CurrentFrom, DateTime CurrentTo)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();
            List<ValidationPhoneType> validationsPhoneType = new List<ValidationPhoneType>();
            List<ValidationTemporalPolicy> validationsTemporalPolicies = new List<ValidationTemporalPolicy>();

            if (!emissionRow.HasError)
            {
                var emissionFile = new File { Id = files.Count + 1 };
                validations.AddRange(GetValidationsByEmissionTemplate(emissionFile, emissionRow, validationsTemporalPolicies));
            }

            foreach (File file in files)
            {
                if (!file.Templates.Any(t => t.Rows.Any(r => r.HasError)))
                {
                    int groupCoverageId = 0;

                    Row rowRiskDetail = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.RiskDetail).Rows.FirstOrDefault();
                    groupCoverageId = (int)DelegateService.commonService.GetValueByField<int>(rowRiskDetail.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskGroupCoverage));

                    foreach (Template template in file.Templates)
                    {
                        if (template.PropertyName == TemplatePropertyName.RiskDetail)
                        {
                            validations.AddRange(GetValidationsByRiskDetailTemplate(file, template, productId, collectiveEmission));
                            validationsPhoneType.AddRange(DelegateService.collectiveService.GetPhoneTypesValidationsByRiskDetailTemplate(file.Id, template));

                            foreach (Row row in template.Rows)
                            {
                                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredEmailDescription).Value))
                                {
                                    ValidationRegularExpression validation = new ValidationRegularExpression();
                                    validation.Id = file.Id;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailDescription).Description;
                                    validation.ValidationId = (int)RegularExpression.email;
                                    validation.ParameterValue = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailDescription));
                                    validationRegularExpressions.Add(validation);
                                }
                                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryEmailDescription).Value))
                                {
                                    ValidationRegularExpression validation = new ValidationRegularExpression();
                                    validation.Id = file.Id;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailDescription).Description;
                                    validation.ValidationId = (int)RegularExpression.email;
                                    validation.ParameterValue = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailDescription));
                                    validationRegularExpressions.Add(validation);
                                }
                                if (!string.IsNullOrEmpty(rowRiskDetail.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskLongitude).Value))
                                {
                                    ValidationRegularExpression validation = new ValidationRegularExpression();
                                    validation.Id = file.Id;
                                    validation.FieldDescription = rowRiskDetail.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLongitude).Description;
                                    validation.ValidationId = (int)RegularExpression.longitude;
                                    validation.ParameterValue = (string)DelegateService.commonService.GetValueByField<string>(rowRiskDetail.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLongitude));
                                    validationRegularExpressions.Add(validation);
                                }
                                if (!string.IsNullOrEmpty(rowRiskDetail.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskLatitude).Value))
                                {
                                    ValidationRegularExpression validation = new ValidationRegularExpression();
                                    validation.Id = file.Id;
                                    validation.FieldDescription = rowRiskDetail.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLatitude).Description;
                                    validation.ValidationId = (int)RegularExpression.latitude;
                                    validation.ParameterValue = (string)DelegateService.commonService.GetValueByField<string>(rowRiskDetail.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLatitude));
                                    validationRegularExpressions.Add(validation);
                                }
                            }
                        }

                        if (template.PropertyName == TemplatePropertyName.InsuredObjects)
                        {
                            validations.AddRange(GetValidationsByInsuredObjectsTemplate(file, template, productId, groupCoverageId));
                        }

                        if (template.PropertyName == TemplatePropertyName.ModifyCoverages)
                        {
                            validations.AddRange(GetValidationsByModifyCoveragesTemplate(file, template, productId, groupCoverageId));
                        }

                        if (template.PropertyName == TemplatePropertyName.AdditionalBeneficiaries)
                        {
                            validations.AddRange(DelegateService.collectiveService.GetValidationsByAdditionalBeneficiariesTemplate(file, template));

                            int countAdditional = 1;
                            foreach (Row row in template.Rows)
                            {
                                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryEmailDescription).Value))
                                {
                                    ValidationRegularExpression validation = new ValidationRegularExpression();
                                    validation.Id = file.Id;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailDescription).Description;
                                    validation.ValidationId = (int)RegularExpression.email;
                                    validation.ParameterValue = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailDescription));
                                    validationRegularExpressions.Add(validation);
                                }
                                countAdditional++;
                            }
                        }
                    }
                }
            }
            List<Validation> result = new List<Validation>();
            if (validations.Count > 0 || validationRegularExpressions.Count > 0)
            {
                result = DelegateService.commonService.ExecuteValidations(validations, validationRegularExpressions);
            }
            if (validationsPhoneType.Count > 0)
            {
                result = DelegateService.commonService.GetValidatedPhoneTypes(validationsPhoneType, result);
            }
            if (validationsTemporalPolicies.Count > 0)
            {
                result = DelegateService.commonService.GetValidatedTemporalPolicies(result, validationsTemporalPolicies);
            }
            return result;
        }

        private List<ValidationIdentificator> GetValidationsByEmissionTemplate(File file, Row row, List<ValidationTemporalPolicy> validationsTemporalPolicies)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();

            // Validacion de temporales
            ValidationTemporalPolicy validationTemporalPolicy = new ValidationTemporalPolicy();
            validationTemporalPolicy.Id = file.Id;
            validationTemporalPolicy.PrefixId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyPrefixCode));
            validationTemporalPolicy.BranchId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
            validationTemporalPolicy.DocumentNum = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumber));
            validationsTemporalPolicies.Add(validationTemporalPolicy);

            #region Póliza

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BranchId).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.Branch;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId).Description;
                validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                validations.Add(validation);
            }
            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.PolicyPrefixCode).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.Prefix;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyPrefixCode).Description;
                validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyPrefixCode));
                validations.Add(validation);
            }

            #endregion

            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByRiskDetailTemplate(File file, Template template, int productId, CollectiveEmission collectiveEmission)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();
            UPENUM.IndividualType individualType;

            foreach (Row row in template.Rows)
            {
                #region Asegurado

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredCode).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.Insured;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCode).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCode));
                    validations.Add(validation);
                }
                else
                {
                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredEconomicActivity).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.EconomicActivity;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEconomicActivity).Description;
                        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEconomicActivity));
                        validations.Add(validation);
                    }

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredEmailType).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.AddressType;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailType).Description;
                        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailType));
                        validations.Add(validation);
                    }

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredAddressCountry).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.Country;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCountry).Description;
                        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCountry));
                        validations.Add(validation);
                    }

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredAddressState).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.State;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressState).Description;
                        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCountry));
                        validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressState));
                        validations.Add(validation);
                    }

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredAddressCity).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.City;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCity).Description;
                        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCountry));
                        validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressState));
                        validation.ParameterValue3 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCity));
                        validations.Add(validation);
                    }

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredPhoneType).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.PhoneType;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPhoneType).Description;
                        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPhoneType));
                        validations.Add(validation);
                    }

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredEmailType).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.EmailType;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailType).Description;
                        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailType));
                        validations.Add(validation);
                    }

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredIndividualType).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.IndividualType;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredIndividualType).Description;
                        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredIndividualType));
                        validations.Add(validation);

                        individualType = (UPENUM.IndividualType)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredIndividualType));
                        switch (individualType)
                        {
                            case UPENUM.IndividualType.LegalPerson:

                                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredCompanyDocumentType).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = file.Id;
                                    validation.ValidationId = (int)ValidationProperty.TributaryType;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyDocumentType).Description;
                                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyDocumentType));
                                    validations.Add(validation);
                                }

                                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredCompanyType).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = file.Id;
                                    validation.ValidationId = (int)ValidationProperty.CompanyType;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyType).Description;
                                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyType));
                                    validations.Add(validation);
                                }
                                break;

                            case UPENUM.IndividualType.Person:

                                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredPersonDocumentType).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = file.Id;
                                    validation.ValidationId = (int)ValidationProperty.DocumentType;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonDocumentType).Description;
                                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonDocumentType));
                                    validations.Add(validation);
                                }

                                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredPersonMaritalStatus).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = file.Id;
                                    validation.ValidationId = (int)ValidationProperty.MaritalStatus;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonMaritalStatus).Description;
                                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonMaritalStatus));
                                    validations.Add(validation);
                                }
                                break;
                        }
                    }
                }

                #endregion

                #region Beneficiario

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryInsuredCode).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.Beneficiary;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryInsuredCode).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryInsuredCode));
                    validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryBeneficiaryType));
                    validations.Add(validation);
                }
                else
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.BeneficiaryType;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryBeneficiaryType).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryBeneficiaryType));
                    validations.Add(validation);

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryEconomicActivity).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.EconomicActivity;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEconomicActivity).Description;
                        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEconomicActivity));
                        validations.Add(validation);
                    }

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryEmailType).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.AddressType;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailType).Description;
                        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailType));
                        validations.Add(validation);
                    }

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryAddressCountry).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.Country;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCountry).Description;
                        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCountry));
                        validations.Add(validation);
                    }

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryAddressState).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.State;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressState).Description;
                        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCountry));
                        validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressState));
                        validations.Add(validation);
                    }

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryAddressCity).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.City;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCity).Description;
                        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCountry));
                        validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressState));
                        validation.ParameterValue3 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCity));
                        validations.Add(validation);
                    }

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryPhoneType).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.PhoneType;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPhoneType).Description;
                        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPhoneType));
                        validations.Add(validation);
                    }

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryEmailType).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.EmailType;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailType).Description;
                        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailType));
                        validations.Add(validation);
                    }

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryIndividualType).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.IndividualType;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryIndividualType).Description;
                        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryIndividualType));
                        validations.Add(validation);

                        individualType = (UPENUM.IndividualType)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryIndividualType));
                        switch (individualType)
                        {
                            case UPENUM.IndividualType.LegalPerson:

                                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentType).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = file.Id;
                                    validation.ValidationId = (int)ValidationProperty.DocumentType;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentType).Description;
                                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentType));
                                    validations.Add(validation);
                                }

                                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryCompanyType).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = file.Id;
                                    validation.ValidationId = (int)ValidationProperty.CompanyType;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyType).Description;
                                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyType));
                                    validations.Add(validation);
                                }
                                break;

                            case UPENUM.IndividualType.Person:

                                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryPersonDocumentType).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = file.Id;
                                    validation.ValidationId = (int)ValidationProperty.DocumentType;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonDocumentType).Description;
                                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonDocumentType));
                                    validations.Add(validation);
                                }

                                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryPersonMaritalStatus).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = file.Id;
                                    validation.ValidationId = (int)ValidationProperty.MaritalStatus;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonMaritalStatus).Description;
                                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonMaritalStatus));
                                    validations.Add(validation);
                                }
                                break;
                        }
                    }
                }

                #endregion

                #region Riesgo

                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.GroupCoverage;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskGroupCoverage).Description;
                validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskGroupCoverage));
                validation.ParameterValue2 = productId;
                validations.Add(validation);

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskCountry).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.Country;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskCountry).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskCountry));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskState).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.State;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskState).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskCountry));
                    validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskState));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskCity).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.City;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskCity).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskCountry));
                    validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskState));
                    validation.ParameterValue3 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskCity));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskActivity).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.RiskActivity;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskActivity).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskActivity));
                    validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskLevelZone).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)CompanyValidationProperty.RiskLevelZone;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskLevelZone).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskTypeCode));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskTypeCode).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)CompanyValidationProperty.RiskTypeCode;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskTypeCode).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskTypeCode));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskUseCode).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)CompanyValidationProperty.RiskUseCode;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskUseCode).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskUseCode));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskIrregularHeight).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)CompanyValidationProperty.RiskIrregularHeight;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskIrregularHeight).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskIrregularHeight));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskIrregularPlant).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)CompanyValidationProperty.RiskIrregularPlant;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskIrregularPlant).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskIrregularPlant));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskPreviousDamage).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)CompanyValidationProperty.RiskPreviousDamage;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskPreviousDamage).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskPreviousDamage));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskRepairedCode).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)CompanyValidationProperty.RiskRepairedCode;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskRepairedCode).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskRepairedCode));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskLocation).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)CompanyValidationProperty.RiskLocation;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskLocation).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskLocation));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskNeighborhood).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)CompanyValidationProperty.RiskNeighborhood;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskNeighborhood).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskNeighborhood));
                    validations.Add(validation);
                }

                if (!String.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskCOfConstruction).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.RiskCOfConstruction;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskCOfConstruction).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskCOfConstruction));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskRatingZone).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRatingZone).Description;
                    validation.ValidationId = (int)ValidationProperty.RatingZone;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRatingZone));
                    validation.ParameterValue2 = collectiveEmission.Prefix.Id;
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskMicrozoningZone).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)CompanyValidationProperty.MicrozoningZone;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskMicrozoningZone).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskMicrozoningZone));
                    validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRatingZone));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskTypeOfProperty).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.RiskTypeOfProperty;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskTypeOfProperty).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskTypeOfProperty));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskReinforcedStructureType).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)CompanyValidationProperty.RiskReinforcedStructureType;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskReinforcedStructureType).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskReinforcedStructureType));
                    validations.Add(validation);
                }

                //Validación tipo estructural 
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)CompanyValidationProperty.RiskStructureCode;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskStructureCode).Description;
                validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskStructureCode));
                validations.Add(validation);

                //Validación Tipo de Asistencia
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskAsistType).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)CompanyValidationProperty.AssistanceType;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskAsistType).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskAsistType));
                    validation.ParameterValue2 = productId;
                    validations.Add(validation);
                }

                #endregion
            }
            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByInsuredObjectsTemplate(File file, Template template, int productId, int groupCoverageId)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();
            int countAdditional = 1;

            foreach (Row row in template.Rows)
            {
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredObjectCode).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.InsuredObject;
                    validation.AdditionalRow = countAdditional;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredObjectCode).Description;
                    validation.ParameterValue1 = groupCoverageId;
                    validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredObjectCode));
                    validation.ParameterValue3 = productId;
                    validations.Add(validation);
                }

                //Validación Periodo de Indemnización
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RecoupmentPeriodId).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)CompanyValidationProperty.RecoupmentPeriodId;
                    validation.AdditionalRow = countAdditional;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RecoupmentPeriodId).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RecoupmentPeriodId));
                    validations.Add(validation);
                }

                countAdditional++;
            }
            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByModifyCoveragesTemplate(File file, Template template, int productId, int groupCoverageId)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();
            int countAdditional = 1;

            foreach (Row row in template.Rows)
            {
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredObjectCode).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.InsuredObject;
                    validation.AdditionalRow = countAdditional;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredObjectCode).Description;
                    validation.ParameterValue1 = groupCoverageId;
                    validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredObjectCode));
                    validation.ParameterValue3 = productId;
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.DeductibleCodeDeductible).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.Deductible;
                    validation.AdditionalRow = countAdditional;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.DeductibleCodeDeductible).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.DeductibleCodeDeductible));
                    validation.ParameterValue2 = productId;
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.DeductibleCodeCoverage).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.Coverage;
                    validation.AdditionalRow = countAdditional;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage));
                    validation.ParameterValue2 = productId;
                    validations.Add(validation);
                }
                countAdditional++;
            }
            return validations;
        }
    }
}
