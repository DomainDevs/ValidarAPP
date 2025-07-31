using Sistran.Company.Application.CommonServices.Enums;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Company.Application.Utilities.Enums;
using Sistran.Company.Application.Vehicle.ModificationService.EEProvider.Resources;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.UtilitiesServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
//using UPENUM = Sistran.Core.Application.UniquePersonService.Enums;

namespace Sistran.Company.Application.Vehicle.ModificationService.EEProvider.DAOs
{
    public class VehicleInclutionValidationDAO
    {
        public List<Validation> GetValidationsByFiles(List<File> files, CollectiveEmission collectiveEmission, int productId, int policyType, Row emissionRow, DateTime CurrentFrom, DateTime CurrentTo, int salePointId)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();
            List<ValidationLicensePlate> validationsLicencePlate = new List<ValidationLicensePlate>();
            List<ValidationTemporalPolicy> validationsTemporalPolicies = new List<ValidationTemporalPolicy>();

            if (!emissionRow.HasError)
            {
                var emissionFile = new File { Id = files.Count > 0 ? files[0].Id : files.Count + 1 };
                validations.AddRange(GetValidationsByEmissionTemplate(emissionFile, emissionRow, validationsTemporalPolicies, salePointId, collectiveEmission.User.UserId));
            }

            foreach (File file in files)
            {
                if (!file.Templates.Any(t => t.Rows.Any(r => r.HasError)))
                {
                    foreach (Template template in file.Templates)
                    {
                        if (template.PropertyName == TemplatePropertyName.RiskDetail)
                        {
                            validations.AddRange(GetValidationsByRiskDetailTemplate(file, template, collectiveEmission, productId, policyType));
                            validationsLicencePlate.AddRange(GetValidationsByLicensePlate(file.Id, template, CurrentFrom, CurrentTo));

                            foreach (Row row in template.Rows)
                            {
                                if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.InsuredEmailDescription).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredEmailDescription).Value))
                                {
                                    ValidationRegularExpression validation = new ValidationRegularExpression();
                                    validation.Id = file.Id;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailDescription).Description;
                                    validation.ValidationId = (int)RegularExpression.email;
                                    validation.ParameterValue = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailDescription));
                                    validationRegularExpressions.Add(validation);
                                }
                                if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.BeneficiaryEmailDescription).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryEmailDescription).Value))
                                {
                                    ValidationRegularExpression validation = new ValidationRegularExpression();
                                    validation.Id = file.Id;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailDescription).Description;
                                    validation.ValidationId = (int)RegularExpression.email;
                                    validation.ParameterValue = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailDescription));
                                    validationRegularExpressions.Add(validation);
                                }
                                if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.RiskRate).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskRate).Value))
                                {
                                    ValidationRegularExpression validation = new ValidationRegularExpression();
                                    validation.Id = file.Id;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRate).Description;
                                    validation.ValidationId = (int)RegularExpression.RiskRate;
                                    validation.ParameterValue = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRate));
                                    validationRegularExpressions.Add(validation);
                                }

                                if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.RiskLicensePlate).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskLicensePlate).Value))
                                {
                                    ValidationRegularExpression validation = new ValidationRegularExpression();
                                    validation.Id = file.Id;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLicensePlate).Description;
                                    validation.ValidationId = (int)RegularExpression.RiskLicensePlate;
                                    validation.ParameterValue = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLicensePlate));
                                    validationRegularExpressions.Add(validation);
                                }
                            }
                        }

                        if (template.PropertyName == CompanyTemplatePropertyName.AdditionalCoverages)
                        {
                            validations.AddRange(GetValidationsByAdditionalCoveragesTemplate(file, template, collectiveEmission, productId));
                        }

                        if (template.PropertyName == TemplatePropertyName.Deductible)
                        {
                            validations.AddRange(GetValidationsByDeductibleTemplate(file, template, collectiveEmission, productId));
                        }

                        if (template.PropertyName == TemplatePropertyName.Accesories)
                        {
                            validations.AddRange(GetValidationsByAccesoriesTemplate(file, template, collectiveEmission));
                        }

                        if (template.PropertyName == TemplatePropertyName.AdditionalBeneficiaries)
                        {
                            validations.AddRange(DelegateService.collectiveService.GetValidationsByAdditionalBeneficiariesTemplate(file, template));

                            int countAdditional = 1;
                            foreach (Row row in template.Rows)
                            {
                                if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.BeneficiaryEmailDescription).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryEmailDescription).Value))
                                {
                                    ValidationRegularExpression validation = new ValidationRegularExpression();
                                    validation.Id = file.Id;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailDescription).Description;
                                    validation.ValidationId = (int)RegularExpression.email;
                                    validation.ParameterValue = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailDescription));
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
                result = DelegateService.utilitiesService.ExecuteValidations(validations, validationRegularExpressions);
            }
            if (validationsLicencePlate.Count > 0)
            {
                result = DelegateService.vehicleService.GetVehicleLicensePlate(result, validationsLicencePlate);
            }
            if (validationsTemporalPolicies.Count > 0)
            {
                result = DelegateService.utilitiesService.GetValidatedTemporalPolicies(result, validationsTemporalPolicies);
            }
            return result;
        }

        public List<Validation> GetValidationsByFilesPolicy(File file, CollectiveEmission collectiveEmission)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();
            List<ValidationTemporalPolicy> validationsTemporalPolicies = new List<ValidationTemporalPolicy>();

            if (!file.Templates.Any(t => t.Rows.Any(r => r.HasError)))
            {
                foreach (Template template in file.Templates)
                {
                    Row row = file.Templates.FirstOrDefault(x => x.PropertyName == template.PropertyName).Rows.FirstOrDefault();

                    if (template.PropertyName == TemplatePropertyName.Policy)
                    {
                        validations.AddRange(GetValidationsByEmissionTemplate(file, row, validationsTemporalPolicies));
                    }

                }
            }

            List<Validation> result = new List<Validation>();

            result = DelegateService.utilitiesService.ExecuteValidations(validations, validationRegularExpressions);
            return DelegateService.utilitiesService.GetValidatedTemporalPolicies(result, validationsTemporalPolicies);
        }

        private List<ValidationIdentificator> GetValidationsByEmissionTemplate(File file, Row row, List<ValidationTemporalPolicy> validationsTemporalPolicies, int? salePointId = null, int? userId = null)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation;

            // Validacion de temporales
            ValidationTemporalPolicy validationTemporalPolicy = new ValidationTemporalPolicy();
            validationTemporalPolicy.Id = file.Id;
            validationTemporalPolicy.PrefixId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode));
            validationTemporalPolicy.BranchId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
            validationTemporalPolicy.DocumentNum = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumber));
            validationsTemporalPolicies.Add(validationTemporalPolicy);

            #region Póliza

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BranchId).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.Branch;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId).Description;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                validations.Add(validation);
            }
            if (salePointId != null)
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.SalePoint;
                validation.FieldDescription = "Punto de venta";
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                validation.ParameterValue2 = userId.GetValueOrDefault(0);
                validation.ParameterValue3 = salePointId.Value;
                validations.Add(validation);
            }
            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.PrefixCode).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.Prefix;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode).Description;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode));
                validations.Add(validation);
            }

            #endregion

            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByRiskDetailTemplate(File file, Template template, CollectiveEmission collectiveEmission, int productId, int policyType)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation;
            IndividualType individualType;

            foreach (Row row in template.Rows)
            {
                #region Asegurado

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredCode).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCode).Description;
                    validation.ValidationId = (int)ValidationProperty.Insured;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCode));
                    validations.Add(validation);
                }
                else
                {
                    if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.InsuredEconomicActivity).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredEconomicActivity).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEconomicActivity).Description;
                        validation.ValidationId = (int)ValidationProperty.EconomicActivity;
                        validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEconomicActivity));
                        validations.Add(validation);
                    }

                    if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.InsuredEmailType).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredEmailType).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailType).Description;
                        validation.ValidationId = (int)ValidationProperty.AddressType;
                        validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailType));
                        validations.Add(validation);
                    }

                    if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.InsuredAddressCountry).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredAddressCountry).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCountry).Description;
                        validation.ValidationId = (int)ValidationProperty.Country;
                        validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCountry));
                        validations.Add(validation);
                    }

                    if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.InsuredAddressState).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredAddressState).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressState).Description;
                        validation.ValidationId = (int)ValidationProperty.State;
                        validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCountry));
                        validation.ParameterValue2 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressState));
                        validations.Add(validation);
                    }

                    if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.InsuredAddressCity).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredAddressCity).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCity).Description;
                        validation.ValidationId = (int)ValidationProperty.City;
                        validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCountry));
                        validation.ParameterValue2 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressState));
                        validation.ParameterValue3 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredAddressCity));
                        validations.Add(validation);
                    }

                    if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.InsuredPhoneType).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredPhoneType).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPhoneType).Description;
                        validation.ValidationId = (int)ValidationProperty.PhoneType;
                        validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPhoneType));
                        validations.Add(validation);
                    }

                    if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.InsuredEmailType).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredEmailType).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailType).Description;
                        validation.ValidationId = (int)ValidationProperty.EmailType;
                        validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredEmailType));
                        validations.Add(validation);
                    }

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredIndividualType).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
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
                                    validation.Id = file.Id;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyDocumentType).Description;
                                    validation.ValidationId = (int)ValidationProperty.TributaryType;
                                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredCompanyDocumentType));
                                    validations.Add(validation);
                                }

                                if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.InsuredCompanyType).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredCompanyType).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = file.Id;
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
                                    validation.Id = file.Id;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonDocumentType).Description;
                                    validation.ValidationId = (int)ValidationProperty.DocumentType;
                                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonDocumentType));
                                    validations.Add(validation);
                                }

                                if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.InsuredPersonMaritalStatus).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredPersonMaritalStatus).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = file.Id;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonMaritalStatus).Description;
                                    validation.ValidationId = (int)ValidationProperty.MaritalStatus;
                                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredPersonMaritalStatus));
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
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryInsuredCode).Description;
                    validation.ValidationId = (int)ValidationProperty.Beneficiary;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryInsuredCode));
                    validation.ParameterValue2 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryBeneficiaryType));
                    validations.Add(validation);
                }
                else
                {
                    if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.BeneficiaryEconomicActivity).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryEconomicActivity).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEconomicActivity).Description;
                        validation.ValidationId = (int)ValidationProperty.EconomicActivity;
                        validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEconomicActivity));
                        validations.Add(validation);
                    }

                    if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.BeneficiaryEmailType).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryEmailType).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailType).Description;
                        validation.ValidationId = (int)ValidationProperty.AddressType;
                        validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailType));
                        validations.Add(validation);
                    }

                    if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.BeneficiaryAddressCountry).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryAddressCountry).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCountry).Description;
                        validation.ValidationId = (int)ValidationProperty.Country;
                        validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCountry));
                        validations.Add(validation);
                    }

                    if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.BeneficiaryAddressState).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryAddressState).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressState).Description;
                        validation.ValidationId = (int)ValidationProperty.State;
                        validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCountry));
                        validation.ParameterValue2 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressState));
                        validations.Add(validation);
                    }

                    if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.BeneficiaryAddressCity).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryAddressCity).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCity).Description;
                        validation.ValidationId = (int)ValidationProperty.City;
                        validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCountry));
                        validation.ParameterValue2 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressState));
                        validation.ParameterValue3 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCity));
                        validations.Add(validation);
                    }

                    if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.BeneficiaryPhoneType).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryPhoneType).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPhoneType).Description;
                        validation.ValidationId = (int)ValidationProperty.PhoneType;
                        validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPhoneType));
                        validations.Add(validation);
                    }

                    if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.BeneficiaryEmailType).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryEmailType).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailType).Description;
                        validation.ValidationId = (int)ValidationProperty.EmailType;
                        validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailType));
                        validations.Add(validation);
                    }

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryIndividualType).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
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
                                    validation.Id = file.Id;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentType).Description;
                                    validation.ValidationId = (int)ValidationProperty.TributaryType;
                                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentType));
                                    validations.Add(validation);
                                }

                                if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.BeneficiaryCompanyType).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryCompanyType).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = file.Id;
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
                                    validation.Id = file.Id;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonDocumentType).Description;
                                    validation.ValidationId = (int)ValidationProperty.DocumentType;
                                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonDocumentType));
                                    validations.Add(validation);
                                }

                                if (row.Fields.Where(y => y.PropertyName == FieldPropertyName.BeneficiaryPersonMaritalStatus).FirstOrDefault() != null && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryPersonMaritalStatus).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = file.Id;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonMaritalStatus).Description;
                                    validation.ValidationId = (int)ValidationProperty.MaritalStatus;
                                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonMaritalStatus));
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
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskGroupCoverage).Description;
                validation.ValidationId = (int)ValidationProperty.GroupCoverage;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskGroupCoverage));
                validation.ParameterValue2 = productId;
                validations.Add(validation);

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskRatingZone).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRatingZone).Description;
                    validation.ValidationId = (int)ValidationProperty.RatingZone;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRatingZone));
                    validation.ParameterValue2 = collectiveEmission.Prefix.Id;
                    validations.Add(validation);
                }

                //Validar si se aplica la validación
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLimitRc).Description;
                validation.ValidationId = (int)ValidationProperty.RcLimitProduct;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLimitRc));
                validation.ParameterValue2 = collectiveEmission.Prefix.Id;
                validation.ParameterValue3 = productId;
                validation.ParameterValue4 = policyType;
                validations.Add(validation);

                //validation = new ValidationIdentificator();
                //validation.Id = file.Id;
                //validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskAsistType).Description;
                //validation.ValidationId = (int)CompanyValidationProperty.AssistanceType;
                //validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskAsistType));
                //validation.ParameterValue2 = productId;
                //validations.Add(validation);

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskVehicleType).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskVehicleType).Description;
                    validation.ValidationId = (int)ValidationProperty.VehicleType;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskVehicleType));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskUse).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskUse).Description;
                    validation.ValidationId = (int)ValidationProperty.VehicleUse;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskUse));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskServiceType).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskServiceType).Description;
                    validation.ValidationId = (int)ValidationProperty.ServiceType;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskServiceType));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskBody).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskBody).Description;
                    validation.ValidationId = (int)ValidationProperty.VehicleBody;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskBody));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskColor).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskColor).Description;
                    validation.ValidationId = (int)ValidationProperty.VehicleColor;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskColor));
                    validations.Add(validation);
                }

                //if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskFuel).Value))
                //{
                //    validation = new ValidationIdentificator();
                //    validation.Id = file.Id;
                //    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskFuel).Description;
                //    validation.ValidationId = (int)ValidationProperty.VehicleFuel;
                //    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskFuel));
                //    validations.Add(validation);
                //}

                //if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskWorkerType).Value))
                //{
                //    validation = new ValidationIdentificator();
                //    validation.Id = file.Id;
                //    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskWorkerType).Description;
                //    validation.ValidationId = (int)CompanyValidationProperty.WorkerType;
                //    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskWorkerType));
                //    validations.Add(validation);
                //}

                #endregion
            }

            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByAdditionalCoveragesTemplate(File file, Template template, CollectiveEmission collectiveEmission, int productId)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            int countAdditional = 1;

            foreach (Row row in template.Rows)
            {
                ValidationIdentificator validation = new ValidationIdentificator();
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.DeductibleCodeCoverage).Value))
                {
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage).Description;
                    validation.ValidationId = (int)ValidationProperty.Coverage;
                    validation.AdditionalRow = countAdditional;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage));
                    validation.ParameterValue2 = productId;
                    validations.Add(validation);
                }
                countAdditional++;
            }
            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByDeductibleTemplate(File file, Template template, CollectiveEmission collectiveEmission, int productId)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            int countAdditional = 1;

            foreach (Row row in template.Rows)
            {
                ValidationIdentificator validation = new ValidationIdentificator();
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.DeductibleCodeCoverage).Value))
                {
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage).Description;
                    validation.ValidationId = (int)ValidationProperty.Coverage;
                    validation.AdditionalRow = countAdditional;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage));
                    validations.Add(validation);
                }
                else
                {
                    row.HasError = true;
                    row.ErrorDescription += " " + string.Format(Errors.ErrorCoverageCode);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.DeductibleCodeDeductible).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.DeductibleCodeDeductible).Description;
                    validation.ValidationId = (int)ValidationProperty.Deductible;
                    validation.AdditionalRow = countAdditional;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.DeductibleCodeDeductible));
                    validation.ParameterValue2 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage));
                    validations.Add(validation);
                }
                else
                {
                    row.HasError = true;
                    row.ErrorDescription += " " + string.Format(Errors.ErrorDeductCode);
                }
                countAdditional++;
            }
            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByAccesoriesTemplate(File file, Template template, CollectiveEmission collectiveEmission)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            int countAdditional = 1;

            foreach (Row row in template.Rows)
            {
                ValidationIdentificator validation = new ValidationIdentificator();
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.AccesoriesAccessoryId).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.AccesoriesAccessoryId).Description;
                    validation.ValidationId = (int)ValidationProperty.AccesoryDetail;
                    validation.AdditionalRow = countAdditional;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AccesoriesAccessoryId));
                    validations.Add(validation);
                }
                countAdditional++;
            }

            return validations;
        }

        private List<ValidationLicensePlate> GetValidationsByLicensePlate(int fileId, Template template, DateTime CurrentFrom, DateTime CurrentTo)
        {
            List<ValidationLicensePlate> validationsLicensePlate = new List<ValidationLicensePlate>();
            foreach (Row row in template.Rows)
            {
                ValidationLicensePlate validationLicensePlate = new ValidationLicensePlate();
                validationLicensePlate = new ValidationLicensePlate();
                validationLicensePlate.Id = fileId;
                validationLicensePlate.LicensePlate = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLicensePlate));
                validationLicensePlate.Engine = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskEngine));
                validationLicensePlate.Chassis = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskChassis));
                validationLicensePlate.CurrentFrom = CurrentFrom;
                validationLicensePlate.CurrentTo = CurrentTo;
                validationsLicensePlate.Add(validationLicensePlate);
            }
            return validationsLicensePlate;
        }
    }
}
