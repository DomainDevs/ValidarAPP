using Sistran.Company.Application.CommonServices.Enums;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using UPENUM = Sistran.Core.Application.UniquePersonService.Enums;

namespace Sistran.Company.Application.Location.MassiveLiabilityServices.EEProvider.DAOs
{
    public class MassiveLoadLiabilityValidationDAO
    {
        public List<Validation> GetValidationsByFiles(List<File> files, MassiveEmission massiveEmission)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();
            List<ValidationPhoneType> validationsPhoneType = new List<ValidationPhoneType>();

            foreach (File file in files)
            {

                bool withRequestGrouping = false;
                int productId = 0;
                int groupCoverageId = 0;
                productId = (int)DelegateService.commonService.GetValueByField<int>(file.Templates.FirstOrDefault(
                   x => x.PropertyName == TemplatePropertyName.EmissionLiability).
                   Rows.FirstOrDefault().Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode));
                groupCoverageId = (int)DelegateService.commonService.GetValueByField<int>(file.Templates.FirstOrDefault(
                    x => x.PropertyName == TemplatePropertyName.EmissionLiability).
                    Rows.FirstOrDefault().Fields.Find(x => x.PropertyName == FieldPropertyName.RiskGroupCoverage));

                foreach (Template template in file.Templates)
                {
                    Row row = file.Templates.FirstOrDefault(x => x.PropertyName == template.PropertyName).Rows.FirstOrDefault();
                    if (row.Fields.Any(y => y.PropertyName == FieldPropertyName.RequestGroup))
                        withRequestGrouping = true;

                    //--------Validacion personas
                    if (template.PropertyName == TemplatePropertyName.EmissionLiability)
                    {
                        validations.AddRange(DelegateService.massiveUnderwritingService.GetPersonValidationsByEmissionTemplate(file.Id, massiveEmission.Prefix.Id, template, row));
                    }
                    //----------------------------

                    if (!template.Rows.Any(r => r.HasError))
                    {
                        if (template.PropertyName == TemplatePropertyName.EmissionLiability)
                        {
                            validations.AddRange(GetValidationsByEmissionTemplate(file, template, row, massiveEmission, withRequestGrouping));
                            validationsPhoneType.AddRange(DelegateService.massiveUnderwritingService.GetPhoneTypesValidationsByEmissionTemplate(file.Id, massiveEmission.Prefix.Id, template, row));
                            validationRegularExpressions.AddRange(DelegateService.massiveUnderwritingService.GetRegularExpressionValidationsByEmisionTemplate(file.Id, row));
                        }
                        if (template.PropertyName == TemplatePropertyName.InsuredProperty)
                        {
                            validations.AddRange(GetValidationsByInsuredObjectsTemplate(file, template, massiveEmission, productId, withRequestGrouping, groupCoverageId));
                        }
                        if (template.PropertyName == TemplatePropertyName.AdditionalBeneficiaries)
                        {
                            validations.AddRange(DelegateService.massiveUnderwritingService.GetValidationsByAdditionalBeneficiaries(file.Id, template));
                            validationsPhoneType.AddRange(DelegateService.massiveUnderwritingService.GetPhoneTypeValidationsByAdditionalBeneficiaries(file.Id, template));
                            validationRegularExpressions.AddRange(DelegateService.massiveUnderwritingService.GetRegularExpressionValidationsByAdditionalBeneficiaries(file.Id, template));
                        }
                        if (template.PropertyName == TemplatePropertyName.AdditionalIntermediaries)
                        {
                            validations.AddRange(GetValidationsByAdditionalIntermediariesTemplate(file, template, massiveEmission, productId, withRequestGrouping));
                        }
                        if (template.PropertyName == TemplatePropertyName.CoinsuranceAccepted)
                        {
                            validations.AddRange(GetValidationsByCoinsuranceAcceptedTemplate(file, template, productId));
                        }
                        if (template.PropertyName == TemplatePropertyName.CoinsuranceAssigned)
                        {
                            validations.AddRange(GetValidationsByCoinsuranceAssignedTemplate(file, template, productId, withRequestGrouping));
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
            return result;
        }

        private List<ValidationIdentificator> GetValidationsByEmissionTemplate(File file, Template template, Row row, MassiveEmission massiveEmission, bool withRequestGrouping)
        {

            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();
            UPENUM.IndividualType individualType;
            #region Tomador
            validations = DelegateService.massiveUnderwritingService.GetGeneralValidationsByEmissionTempleteHolder(file.Id, massiveEmission.Prefix.Id, template, row, validations);
            #endregion

            #region Asegurado
            validations = DelegateService.massiveUnderwritingService.GetGeneralValidationsByEmissionTempleteInsured(file.Id, massiveEmission.Prefix.Id, template, row, validations);
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

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryAddressType).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.AddressType;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressType).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressType));
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

            #region Póliza

            validations = DelegateService.massiveUnderwritingService.GetGeneralValidationsByEmissionTempleteCurrency(file.Id, massiveEmission.Prefix.Id, template, row, validations);

            if (!withRequestGrouping)
            {
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.PolicyBusinessType).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.BusinessType;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyBusinessType).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyBusinessType));
                    validations.Add(validation);
                }
            }
            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.PolicyAllianceCode).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)CompanyValidationProperty.Alliance;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.PolicyAllianceCode).Description;
                validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.PolicyAllianceCode));
                validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(p => p.PropertyName == CompanyFieldPropertyName.PolicyAllianceCode));

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.PolicyAllianceBranchId).Value))
                {
                    validation.ParameterValue3 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.PolicyAllianceBranchId));
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.PolicyAllianceSalePointId).Value))
                {
                    validation.ParameterValue3 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.PolicyAllianceSalePointId));
                }
                validations.Add(validation);
            }
            if (!withRequestGrouping)
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.CoPolicyType;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyType).Description;
                validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyType));
                validation.ParameterValue2 = massiveEmission.Prefix.Id;
                validation.ParameterValue3 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode));
                validations.Add(validation);

                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.Product;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode).Description;
                validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode));
                validations.Add(validation);
            }

            #endregion

            #region Riesgo

            validation = new ValidationIdentificator();
            validation.Id = file.Id;
            validation.ValidationId = (int)ValidationProperty.GroupCoverage;
            validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskGroupCoverage).Description;
            validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskGroupCoverage));
            if (!withRequestGrouping)
            {
                validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode));
            }
            else
            {
                validation.ParameterValue3 = massiveEmission.RequestId.Value;
            }
            validations.Add(validation);

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

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskInspectionRecomendation).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)CompanyValidationProperty.RiskInspectionType;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskInspectionRecomendation).Description;
                validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskInspectionRecomendation));
                validations.Add(validation);
            }

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskAsistType).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskAsistType).Description;
                validation.ValidationId = (int)CompanyValidationProperty.AssistanceType;
                validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskAsistType));

                if (!withRequestGrouping)
                {
                    validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode));
                }
                else
                {
                    validation.ParameterValue3 = massiveEmission.RequestId.Value;
                }
                validations.Add(validation);
            }
            #endregion

            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByInsuredObjectsTemplate(File file, Template template, MassiveEmission massiveEmission, int productId, bool withRequestGrouping, int groupCoverageId)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();
            int countAdditional = 1;

            foreach (Row row in template.Rows)
            {
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.InsuredObjectPropertyCode).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.InsuredObject;
                    validation.AdditionalRow = countAdditional;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredObjectPropertyCode).Description;
                    validation.ParameterValue1 = groupCoverageId;
                    validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredObjectPropertyCode));

                    if (!withRequestGrouping)
                    {
                        validation.ParameterValue3 = productId;
                    }
                    else
                    {
                        validation.ParameterValue4 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(p => p.PropertyName == FieldPropertyName.RequestGroup));
                    }
                    validations.Add(validation);
                }
                countAdditional++;
            }
            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByAdditionalIntermediariesTemplate(File file, Template template, MassiveEmission massiveEmission, int productId, bool withRequestGrouping)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();
            int countAdditional = 1;

            foreach (Row row in template.Rows)
            {
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.AgentCode).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.Agent;
                    validation.AdditionalRow = countAdditional;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode));

                    if (!withRequestGrouping)
                    {
                        validation.ParameterValue2 = productId;
                        validation.ParameterValue3 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(p => p.PropertyName == FieldPropertyName.BranchId));
                    }
                    else
                    {
                        validation.ParameterValue3 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(p => p.PropertyName == FieldPropertyName.BranchId));
                        validation.ParameterValue4 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(p => p.PropertyName == FieldPropertyName.RequestGroup));
                    }
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.AgentType).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.AgentType;
                    validation.AdditionalRow = countAdditional;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));
                    validations.Add(validation);
                }
                countAdditional++;
            }
            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByCoinsuranceAcceptedTemplate(File file, Template template, int productId)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();
            int countAdditional = 1;

            foreach (Row row in template.Rows)
            {
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.CoInsuranceAcceptedCompany).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.CoInsurance;
                    validation.AdditionalRow = countAdditional;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.CoInsuranceAcceptedCompany).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.CoInsuranceAcceptedCompany));
                    validations.Add(validation);
                }
                countAdditional++;
            }
            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByCoinsuranceAssignedTemplate(File file, Template template, int productId, bool withRequestGrouping)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();
            int countAdditional = 1;

            foreach (Row row in template.Rows)
            {
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.CoInsuranceAssignedCompany).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.CoInsurance;
                    validation.AdditionalRow = countAdditional;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.CoInsuranceAssignedCompany).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.CoInsuranceAssignedCompany));
                    validations.Add(validation);
                }
                countAdditional++;
            }
            return validations;
        }
    }
}