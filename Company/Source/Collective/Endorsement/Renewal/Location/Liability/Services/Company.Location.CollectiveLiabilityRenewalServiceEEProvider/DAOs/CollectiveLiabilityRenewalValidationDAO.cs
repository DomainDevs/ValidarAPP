using Sistran.Company.Application.CommonServices.Enums;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using UPENUM = Sistran.Core.Application.UniquePersonService.Enums;

namespace Sistran.Company.Application.Location.CollectiveLiabilityRenewalService.EEProvider.DAOs
{
    public class CollectiveLiabilityRenewalValidationDAO
    {
        public List<Validation> GetValidationsByFiles(List<File> files, CollectiveEmission collectiveEmission)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();

            foreach (File file in files)
            {
                if (!file.Templates.Any(t => t.Rows.Any(r => r.HasError)))
                {
                    bool withRequestGrouping = false;
                    int productId = 0;
                    int groupCoverageId = 0;

                    foreach (Template template in file.Templates)
                    {
                        Row row = file.Templates.FirstOrDefault(x => x.PropertyName == template.PropertyName).Rows.FirstOrDefault();

                        if (row.Fields.Any(y => y.PropertyName == FieldPropertyName.RequestGroup))
                            withRequestGrouping = true;

                        if (template.PropertyName == TemplatePropertyName.EmissionProperty)
                        {
                            productId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode));
                            groupCoverageId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskGroupCoverage));
                            validations.AddRange(GetValidationsByEmissionTemplate(file, template, row, collectiveEmission, withRequestGrouping));
                        }
                        if (template.PropertyName == TemplatePropertyName.InsuredObjects)
                        {
                            validations.AddRange(GetValidationsByInsuredObjectsTemplate(file, template, collectiveEmission, productId, withRequestGrouping, groupCoverageId));
                        }
                        if (template.PropertyName == TemplatePropertyName.AdditionalBeneficiaries)
                        {
                            validations.AddRange(GetValidationsByAdditionalBeneficiariesTemplate(file, template, productId));
                        }
                        if (template.PropertyName == TemplatePropertyName.AdditionalIntermediaries)
                        {
                            validations.AddRange(GetValidationsByAdditionalIntermediariesTemplate(file, template, collectiveEmission, productId, withRequestGrouping));
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
            return DelegateService.commonService.ExecuteValidations(validations, validationRegularExpressions);
        }

        private List<ValidationIdentificator> GetValidationsByEmissionTemplate(File file, Template template, Row row, CollectiveEmission collectiveEmission, bool withRequestGrouping)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();
            UPENUM.IndividualType individualType;
            #region Tomador
            validations = DelegateService.massiveUnderwritingService.GetGeneralValidationsByEmissionTempleteHolder(file.Id, collectiveEmission.Prefix.Id, template, row, validations);
            #endregion

            #region Asegurado
            validations = DelegateService.massiveUnderwritingService.GetGeneralValidationsByEmissionTempleteInsured(file.Id, collectiveEmission.Prefix.Id, template, row, validations);
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

            validations = DelegateService.massiveUnderwritingService.GetGeneralValidationsByEmissionTempleteCurrency(file.Id, collectiveEmission.Prefix.Id, template, row, validations);

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
                validation.ParameterValue2 = collectiveEmission.Prefix.Id;
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
                validations.Add(validation);
            }
            else
            {
                validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(p => p.PropertyName == FieldPropertyName.PolicyProductCode));
                validation.ParameterValue3 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(p => p.PropertyName == FieldPropertyName.RequestGroup));
                validations.Add(validation);
            }

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
                validations.Add(validation);
            }

            //if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskLevelZone).Value))
            //{
            //    validation = new ValidationIdentificator();
            //    validation.Id = file.Id;
            //    validation.ValidationId = (int)ValidationProperty.RiskLevelZone;
            //    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskLevelZone).Description;
            //    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskTypeCode));
            //    validations.Add(validation);
            //}

            //if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskTypeCode).Value))
            //{
            //    validation = new ValidationIdentificator();
            //    validation.Id = file.Id;
            //    validation.ValidationId = (int)ValidationProperty.RiskTypeCode;
            //    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskTypeCode).Description;
            //    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskTypeCode));
            //    validations.Add(validation);
            //}

            //if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskUseCode).Value))
            //{
            //    validation = new ValidationIdentificator();
            //    validation.Id = file.Id;
            //    validation.ValidationId = (int)ValidationProperty.RiskUseCode;
            //    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskUseCode).Description;
            //    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskUseCode));
            //    validations.Add(validation);
            //}

            //if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskIrregularHeight).Value))
            //{
            //    validation = new ValidationIdentificator();
            //    validation.Id = file.Id;
            //    validation.ValidationId = (int)ValidationProperty.RiskIrregularHeight;
            //    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskIrregularHeight).Description;
            //    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskIrregularHeight));
            //    validations.Add(validation);
            //}

            //if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskIrregularPlant).Value))
            //{
            //    validation = new ValidationIdentificator();
            //    validation.Id = file.Id;
            //    validation.ValidationId = (int)ValidationProperty.RiskIrregularPlant;
            //    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskIrregularPlant).Description;
            //    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskIrregularPlant));
            //    validations.Add(validation);
            //}

            //if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskPreviousDamage).Value))
            //{
            //    validation = new ValidationIdentificator();
            //    validation.Id = file.Id;
            //    validation.ValidationId = (int)ValidationProperty.RiskPreviousDamage;
            //    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskPreviousDamage).Description;
            //    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskPreviousDamage));
            //    validations.Add(validation);
            //}

            //if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskRepairedCode).Value))
            //{
            //    validation = new ValidationIdentificator();
            //    validation.Id = file.Id;
            //    validation.ValidationId = (int)ValidationProperty.RiskRepairedCode;
            //    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskRepairedCode).Description;
            //    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskRepairedCode));
            //    validations.Add(validation);
            //}

            //if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskLocation).Value))
            //{
            //    validation = new ValidationIdentificator();
            //    validation.Id = file.Id;
            //    validation.ValidationId = (int)ValidationProperty.RiskLocation;
            //    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskLocation).Description;
            //    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskLocation));
            //    validations.Add(validation);
            //}

            //if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskNeighborhood).Value))
            //{
            //    validation = new ValidationIdentificator();
            //    validation.Id = file.Id;
            //    validation.ValidationId = (int)ValidationProperty.RiskNeighborhood;
            //    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskNeighborhood).Description;
            //    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskNeighborhood));
            //    validations.Add(validation);
            //}

            if (!String.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskCOfConstruction).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.RiskCOfConstruction;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskCOfConstruction).Description;
                validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskCOfConstruction));
                validations.Add(validation);
            }
            //if (row.Fields.Exists(p => p.PropertyName == CompanyFieldPropertyName.RiskDanger))
            //{
            //    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskDanger).Value))
            //    {
            //        validation = new ValidationIdentificator();
            //        validation.Id = file.Id;
            //        validation.ValidationId = (int)ValidationProperty.RiskDanger;
            //        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskDanger).Description;
            //        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskDanger));
            //        validations.Add(validation);
            //    }
            //}

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskTypeOfProperty).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.RiskTypeOfProperty;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskTypeOfProperty).Description;
                validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskTypeOfProperty));
                validations.Add(validation);
            }

            //if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RiskReinforcedStructureType).Value))
            //{
            //    validation = new ValidationIdentificator();
            //    validation.Id = file.Id;
            //    validation.ValidationId = (int)ValidationProperty.RiskReinforcedStructureType;
            //    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskReinforcedStructureType).Description;
            //    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RiskReinforcedStructureType));
            //    validations.Add(validation);
            //}
            #endregion

            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByInsuredObjectsTemplate(File file, Template template, CollectiveEmission massiveEmission, int productId, bool withRequestGrouping, int groupCoverageId)
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

        private List<ValidationIdentificator> GetValidationsByAdditionalBeneficiariesTemplate(File file, Template template, int productId)
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
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryInsuredCode));
                    validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryBeneficiaryType));
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
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryBeneficiaryType));
                    validations.Add(validation);

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryEconomicActivity).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.EconomicActivity;
                        validation.AdditionalRow = countAdditional;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEconomicActivity).Description;
                        validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEconomicActivity));
                        validations.Add(validation);
                    }

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryAddressType).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.AddressType;
                        validation.AdditionalRow = countAdditional;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressType).Description;
                        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressType));
                        validations.Add(validation);
                    }

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryAddressCountry).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.Country;
                        validation.AdditionalRow = countAdditional;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCountry).Description;
                        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCountry));
                        validations.Add(validation);
                    }

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryAddressState).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.State;
                        validation.AdditionalRow = countAdditional;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCountry).Description;
                        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressCountry));
                        validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryAddressState));
                        validations.Add(validation);
                    }

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryAddressCity).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.City;
                        validation.AdditionalRow = countAdditional;
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
                        validation.AdditionalRow = countAdditional;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPhoneType).Description;
                        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPhoneType));
                        validations.Add(validation);
                    }

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryEmailType).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.EmailType;
                        validation.AdditionalRow = countAdditional;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailType).Description;
                        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEmailType));
                        validations.Add(validation);
                    }

                    if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryIndividualType).Value))
                    {
                        validation = new ValidationIdentificator();
                        validation.Id = file.Id;
                        validation.ValidationId = (int)ValidationProperty.IndividualType;
                        validation.AdditionalRow = countAdditional;
                        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryIndividualType).Description;
                        validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryIndividualType));
                        validations.Add(validation);

                        UPENUM.IndividualType individualType = (UPENUM.IndividualType)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryIndividualType));
                        switch (individualType)
                        {
                            case UPENUM.IndividualType.LegalPerson:

                                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentType).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = file.Id;
                                    validation.ValidationId = (int)ValidationProperty.TributaryType;
                                    validation.AdditionalRow = countAdditional;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentType).Description;
                                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryCompanyDocumentType));
                                    validations.Add(validation);
                                }

                                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryCompanyType).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = file.Id;
                                    validation.ValidationId = (int)ValidationProperty.CompanyType;
                                    validation.AdditionalRow = countAdditional;
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
                                    validation.AdditionalRow = countAdditional;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonDocumentType).Description;
                                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonDocumentType));
                                    validations.Add(validation);
                                }

                                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BeneficiaryPersonMaritalStatus).Value))
                                {
                                    validation = new ValidationIdentificator();
                                    validation.Id = file.Id;
                                    validation.ValidationId = (int)ValidationProperty.MaritalStatus;
                                    validation.AdditionalRow = countAdditional;
                                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonMaritalStatus).Description;
                                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryPersonMaritalStatus));
                                    validations.Add(validation);
                                }
                                break;
                        }
                    }
                    countAdditional++;
                }
            }
            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByAdditionalIntermediariesTemplate(File file, Template template, CollectiveEmission massiveEmission, int productId, bool withRequestGrouping)
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
