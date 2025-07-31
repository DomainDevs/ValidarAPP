using Sistran.Company.Application.Utilities.Constants;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using COENUM = Sistran.Core.Application.CommonService.Enums;
using Sistran.Company.Application.Vehicles.TPLCollectiveServices.EEProvider.Resources;

namespace Sistran.Company.Application.Vehicles.TPLCollectiveServices.EEProvider.DAOs
{
    public class CollectiveThirdPartyLiabilityValidationDAO
    {
        public List<Validation> GetValidationsByFiles(List<File> files, CollectiveEmission collectiveEmission, int productId, int policyType, long correlativePolicy, DateTime currentTo, DateTime currentFrom, Row emissionRow)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();
            List<ValidationLicensePlate> validationsLicencePlate = new List<ValidationLicensePlate>();

            foreach (File file in files)
            {
                if (!file.Templates.Any(t => t.Rows.Any(r => r.HasError)))
                {
                    foreach (Template template in file.Templates)
                    {
                        if (template.PropertyName == TemplatePropertyName.RiskDetail)
                        {
                            validations.AddRange(GetValidationsByRiskDetailTemplate(file, template, collectiveEmission, productId, policyType));
                            validationsLicencePlate.AddRange(GetValidationsByLicensePlate(file.Id, currentTo, currentFrom, template));

                            foreach (Row row in template.Rows)
                            {
                                validationRegularExpressions.AddRange(DelegateService.collectiveService.GetRegularExpressionValidationsByRiskTemplate(file.Id, row));
                            }
                        }
                        else if (template.PropertyName == CompanyTemplatePropertyName.AdditionalCoverages)
                        {
                            validations.AddRange(GetValidationsByAdditionalCoveragesTemplate(file, template, collectiveEmission, productId));
                        }
                        else if (template.PropertyName == TemplatePropertyName.Deductible)
                        {
                            validations.AddRange(GetValidationsByDeductibleTemplate(file, template, collectiveEmission, productId));
                        }
                        else if (template.PropertyName == TemplatePropertyName.AdditionalBeneficiaries)
                        {
                            validations.AddRange(DelegateService.collectiveService.GetValidationsByAdditionalBeneficiariesTemplate(file, template));
                            validationRegularExpressions.AddRange(DelegateService.collectiveService.GetRegularExpressionValidationsByAdditionalBeneficiaries(file.Id, template));
                        }
                        else if (template.PropertyName == TemplatePropertyName.AdditionalIntermediaries)
                        {
                            validations.AddRange(GetValidationsByAdditionalIntermediaries(file, template, collectiveEmission, productId));
                        }
                        else if (template.PropertyName == TemplatePropertyName.CoinsuranceAccepted)
                        {
                            validations.AddRange(GetValidationsByCoinsuranceAcceptedTemplate(file, template, productId));
                        }
                        else if (template.PropertyName == TemplatePropertyName.CoinsuranceAssigned)
                        {
                            validations.AddRange(GetValidationsByCoinsuranceAssignedTemplate(file, template, productId));
                        }
                        else if (template.PropertyName == TemplatePropertyName.Clauses)
                        {
                            validations.AddRange(DelegateService.massiveUnderwritingService.GetValidationsClauseLevel(file.Id, collectiveEmission.Prefix.Id, template, (int)COENUM.CoveredRiskType.Vehicle));
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
                result = DelegateService.tplService.GetVehicleLicensePlate(result, validationsLicencePlate);
            }

            return result;
        }

        public List<Validation> GetValidationsByEmissionTemplate(File file, Row row, CollectiveEmission collectiveEmission)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();
            List<ValidationPhoneType> validationsPhoneType = new List<ValidationPhoneType>();
            validations.AddRange(DelegateService.collectiveService.GetGeneralValidationsByEmissionTemplate(file, row, collectiveEmission.User.UserId));
            validationsPhoneType.AddRange(DelegateService.collectiveService.GetPhoneTypesValidationsByEmissionTemplate(file, row));
            validationRegularExpressions.AddRange(DelegateService.collectiveService.GetRegularExpressionValidationsByEmisionTemplate(file.Id, row));

            #region Póliza

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.PolicyCurrency).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.Currency;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrency).Description;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrency));
                validation.ParameterValue2 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode));
                validations.Add(validation);
            }

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.PolicyCurrency).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.Currency;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrency).Description;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrency));
                validation.ParameterValue2 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode));
                validations.Add(validation);
            }

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.AgentCode).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.Agent;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode).Description;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode));
                validation.ParameterValue2 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode));
                validation.ParameterValue3 = collectiveEmission.Prefix.Id;
                validation.ParameterValue4 = collectiveEmission.Branch.Id;
                validation.ParameterValue5 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));
                validations.Add(validation);
            }

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.AgentType).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.AgentType;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType).Description;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));
                validations.Add(validation);
            }
            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.HolderIndividualType).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.Insured;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderIndividualType).Description;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderIndividualType));
                validations.Add(validation);
            }

            validation = new ValidationIdentificator();
            validation.Id = file.Id;
            validation.ValidationId = (int)ValidationProperty.PolicyType;
            validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyType).Description;
            validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyType));
            validation.ParameterValue2 = collectiveEmission.Prefix.Id;
            validation.ParameterValue3 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode));
            validations.Add(validation);

            validation = new ValidationIdentificator();
            validation.Id = file.Id;
            validation.ValidationId = (int)ValidationProperty.Product;
            validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode).Description;
            validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode));
            validation.ParameterValue2 = collectiveEmission.Prefix.Id;
            validations.Add(validation);

            #endregion

            List<Validation> result = new List<Validation>();
            if (validations.Count > 0 || validationRegularExpressions.Count > 0)
            {
                result = DelegateService.utilitiesService.ExecuteValidations(validations, validationRegularExpressions);
            }

            return result;
        }

        private List<ValidationIdentificator> GetValidationsByRiskDetailTemplate(File file, Template template, CollectiveEmission collectiveEmission, int productId, int policyType)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();

            foreach (Row row in template.Rows)
            {
                validations.AddRange(DelegateService.collectiveService.GetGeneralValidationsByRiskTemplate(file.Id, row));

                #region Riesgo

                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskGroupCoverage).Description;
                validation.ValidationId = (int)ValidationProperty.GroupCoverage;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskGroupCoverage));
                validation.ParameterValue2 = productId;
                validations.Add(validation);

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskRatingZone)?.Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRatingZone).Description;
                    validation.ValidationId = (int)ValidationProperty.RatingZone;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRatingZone));
                    validation.ParameterValue2 = collectiveEmission.Prefix.Id;
                    validations.Add(validation);
                }
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskVehicleType)?.Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskVehicleType).Description;
                    validation.ValidationId = (int)ValidationProperty.VehicleType;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskVehicleType));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskUse)?.Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskUse).Description;
                    validation.ValidationId = (int)ValidationProperty.VehicleUse;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskUse));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskServiceType)?.Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskServiceType).Description;
                    validation.ValidationId = (int)ValidationProperty.ServiceType;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskServiceType));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskBody)?.Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskBody).Description;
                    validation.ValidationId = (int)ValidationProperty.VehicleBody;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskBody));
                    validations.Add(validation);
                }
                #endregion
            }

            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByAdditionalCoveragesTemplate(File file, Template template, CollectiveEmission collectiveEmission, int productId)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();
            int countAdditional = 1;

            foreach (Row row in template.Rows)
            {
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
            ValidationIdentificator validation = new ValidationIdentificator();
            int countAdditional = 1;

            foreach (Row row in template.Rows)
            {
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
                    validation.ParameterValue3 = productId;
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

        private List<ValidationIdentificator> GetValidationsByAdditionalIntermediaries(File file, Template template, CollectiveEmission collectiveEmission, int productId)
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
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode).Description;
                    validation.ValidationId = (int)ValidationProperty.Agent;
                    validation.AdditionalRow = countAdditional;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode));
                    validation.ParameterValue2 = productId;
                    validation.ParameterValue3 = collectiveEmission.Prefix.Id;
                    validation.ParameterValue4 = collectiveEmission.Branch.Id;
                    validation.ParameterValue5 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.AgentType).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType).Description;
                    validation.ValidationId = (int)ValidationProperty.AgentType;
                    validation.AdditionalRow = countAdditional;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));
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

            foreach (Row row in template.Rows)
            {
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.CoInsuranceAcceptedCompany).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.CoInsuranceCompany;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.CoInsuranceAcceptedCompany).Description;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.CoInsuranceAcceptedCompany));
                    validations.Add(validation);
                }
            }
            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByCoinsuranceAssignedTemplate(File file, Template template, int productId)
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
                    validation.ValidationId = (int)ValidationProperty.CoInsuranceCompany;
                    validation.AdditionalRow = countAdditional;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.CoInsuranceAssignedCompany).Description;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.CoInsuranceAssignedCompany));
                    validations.Add(validation);
                }
                countAdditional++;
            }
            return validations;
        }

        private List<ValidationLicensePlate> GetValidationsByLicensePlate(int fileId, DateTime currentTo, DateTime currentFrom, Template template)
        {
            List<ValidationLicensePlate> validationsLicensePlate = new List<ValidationLicensePlate>();
            ValidationLicensePlate validationLicensePlate = new ValidationLicensePlate();

            foreach (Row row in template.Rows)
            {
                validationLicensePlate = new ValidationLicensePlate();
                validationLicensePlate.Id = fileId;
                validationLicensePlate.LicensePlate = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLicensePlate));
                validationLicensePlate.Engine = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskEngine));
                validationLicensePlate.Chassis = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskChassis));
                validationLicensePlate.CurrentFrom = currentFrom;
                if (row.Fields.Any(y => y.PropertyName == FieldPropertyName.RequestGroup))
                {
                    validationLicensePlate.CurrentTo = null;
                    validationLicensePlate.ParameterValue = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RequestGroup));
                }
                else
                {
                    validationLicensePlate.CurrentTo = currentTo;
                    validationLicensePlate.ParameterValue = null;
                }
                validationsLicensePlate.Add(validationLicensePlate);
            }
            return validationsLicensePlate;
        }
    }
}
