using Sistran.Company.Application.CommonServices.Enums;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Company.Application.Vehicles.MassiveTPLServices.EEProvider.Resources;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.Vehicles.MassiveThirdPartyLiabilityServices.EEProvider.DAOs
{
    public class MassiveVehicleValidationDAO
    {
        public List<Validation> GetValidationsByFiles(List<File> files, MassiveEmission massiveEmission, int agentId, int agentTypeId, int productRequestId, int requestId, int billingGroupId)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();
            List<ValidationLicensePlate> validationsLicencePlate = new List<ValidationLicensePlate>();

            foreach (File file in files)
            {
                int productId = 0;
                int prefixId = 0;

                foreach (Template template in file.Templates)
                {
                    Row row = file.Templates.FirstOrDefault(x => x.PropertyName == template.PropertyName).Rows.FirstOrDefault();
                    //--------Validacion personas
                    if (template.PropertyName == TemplatePropertyName.EmissionThirdPartyLiability)
                    {
                        productId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode));
                        prefixId = massiveEmission.Prefix.Id;
                        validations.AddRange(DelegateService.massiveUnderwritingService.GetPersonValidationsByEmissionTemplate(file.Id, massiveEmission.Prefix.Id, template, row));
                    }
                    //----------------------------

                    if (!template.Rows.Any(r => r.HasError))
                    {
                        if (template.PropertyName == TemplatePropertyName.EmissionThirdPartyLiability)
                        {
                            validations.AddRange(DelegateService.massiveUnderwritingService.GetGeneralValidationsByEmissionTemplate(file.Id, massiveEmission.Prefix.Id, template, row, agentId, agentTypeId, productRequestId, requestId, billingGroupId, massiveEmission.User.UserId));
                            validations.AddRange(GetSpecificValidationsByEmissionTemplate(file.Id, massiveEmission.Prefix.Id, template, row));
                            validationsLicencePlate.AddRange(GetValidationsByLicensePlate(file.Id, row));
                            validationRegularExpressions.AddRange(DelegateService.massiveUnderwritingService.GetRegularExpressionValidationsByEmisionTemplate(file.Id, row));
                        }
                        if (template.PropertyName == CompanyTemplatePropertyName.AdditionalCoverages)
                        {
                            validations.AddRange(GetValidationsByAdditionalCoveragesTemplate(file.Id, template, productId));
                        }

                        if (template.PropertyName == TemplatePropertyName.Deductible)
                        {
                            validations.AddRange(GetValidationsByDeductibleTemplate(file.Id, template, productId));
                        }

                        if (template.PropertyName == TemplatePropertyName.AdditionalBeneficiaries)
                        {
                            validations.AddRange(DelegateService.massiveUnderwritingService.GetValidationsByAdditionalBeneficiaries(file.Id, template));
                        }

                        if (template.PropertyName == TemplatePropertyName.AdditionalIntermediaries)
                        {
                            validations.AddRange(GetValidationsByAdditionalIntermediaries(file.Id, template, productId, prefixId, massiveEmission.Branch.Id));
                        }

                        if (template.PropertyName == TemplatePropertyName.CoinsuranceAccepted)
                        {
                            validations.AddRange(GetValidationsByCoinsuranceAccepted(file.Id, template));
                        }

                        if (template.PropertyName == TemplatePropertyName.CoinsuranceAssigned)
                        {
                            validations.AddRange(GetValidationsByCoinsuranceAssigned(file.Id, template));
                        }
                        if (template.PropertyName == TemplatePropertyName.Clauses)
                        {
                            validations.AddRange(DelegateService.massiveUnderwritingService.GetValidationsClauseLevel(file.Id, prefixId, template, (int)CoveredRiskType.Vehicle));
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
                //result = DelegateService.tplService.GetVehicleLicensePlate(result, validationsLicencePlate);
            }

            return result;
        }

        private List<ValidationIdentificator> GetSpecificValidationsByEmissionTemplate(int fileId, int prefixId, Template template, Row row)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();

            if (row.Fields.Any(y => y.PropertyName == FieldPropertyName.RequestGroup))
            {
                validations.AddRange(GetRiskValidationsByEmissionTemplateWithRequest(fileId, prefixId, template, row));
            }
            else
            {
                validations.AddRange(GetRiskValidationsByEmissionTemplate(fileId, prefixId, template, row));
            }

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskRatingZone).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = fileId;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRatingZone).Description;
                validation.ValidationId = (int)ValidationProperty.RatingZone;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskRatingZone));
                validation.ParameterValue2 = prefixId;
                validations.Add(validation);
            }

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskVehicleType).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = fileId;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskVehicleType).Description;
                validation.ValidationId = (int)ValidationProperty.VehicleType;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskVehicleType));
                validations.Add(validation);
            }


            if (row.Fields.Exists(y => y.PropertyName == FieldPropertyName.RiskUse) && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskUse).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = fileId;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskUse).Description;
                validation.ValidationId = (int)ValidationProperty.VehicleUse;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskUse));
                validations.Add(validation);
            }

            if (row.Fields.Exists(y => y.PropertyName == FieldPropertyName.RiskBody) && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskBody).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = fileId;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskBody).Description;
                validation.ValidationId = (int)ValidationProperty.VehicleBody;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskBody));
                validation.ParameterValue2 = DelegateService.commonService.GetParameterByParameterId((int)CompanyParameterType.WithOutBodyVehicle).NumberParameter.GetValueOrDefault();
                validations.Add(validation);
            }

            return validations;
        }

        private List<ValidationIdentificator> GetRiskValidationsByEmissionTemplate(int fileId, int prefixId, Template template, Row row)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();

            if (row.Fields.Exists(y => y.PropertyName == FieldPropertyName.RiskLimitRc) && !string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskLimitRc).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = fileId;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLimitRc).Description;
                validation.ValidationId = (int)ValidationProperty.RcLimitProduct;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLimitRc));
                validation.ParameterValue2 = prefixId;
                validation.ParameterValue3 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyProductCode));
                validation.ParameterValue4 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyType));
                validations.Add(validation);
            }
            return validations;
        }

        private List<ValidationIdentificator> GetRiskValidationsByEmissionTemplateWithRequest(int fileId, int prefixId, Template template, Row row)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.RiskLimitRc).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = fileId;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLimitRc).Description;
                validation.ValidationId = (int)ValidationProperty.RcLimitProduct;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLimitRc));
                validation.ParameterValue2 = prefixId;
                validation.ParameterValue5 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RequestGroup));
                validations.Add(validation);
            }
            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByAdditionalCoveragesTemplate(int fileId, Template template, int productId)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            int countAdditional = 1;

            foreach (Row row in template.Rows)
            {
                ValidationIdentificator validation = new ValidationIdentificator();
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.DeductibleCodeCoverage).Value))
                {
                    validation.Id = fileId;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage).Description;
                    validation.ValidationId = (int)ValidationProperty.Coverage;
                    validation.AdditionalRow = countAdditional;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage));

                    if (!row.Fields.Any(y => y.PropertyName == FieldPropertyName.RequestGroup))
                    {
                        validation.ParameterValue2 = productId;
                    }
                    else
                    {
                        validation.ParameterValue3 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RequestGroup));
                    }
                    validations.Add(validation);
                }
                countAdditional++;
            }
            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByDeductibleTemplate(int fileId, Template template, int productId)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            int countAdditional = 1;

            foreach (Row row in template.Rows)
            {
                ValidationIdentificator validation = new ValidationIdentificator();
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.DeductibleCodeCoverage).Value))
                {
                    validation.Id = fileId;
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
                    validation.Id = fileId;
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

        private List<ValidationIdentificator> GetValidationsByAdditionalIntermediaries(int fileId, Template template, int productId, int prefixId, int branchId)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            int countAdditional = 1;

            foreach (Row row in template.Rows)
            {
                ValidationIdentificator validation = new ValidationIdentificator();
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.AgentCode).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = fileId;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode).Description;
                    validation.ValidationId = (int)ValidationProperty.Agent;
                    validation.AdditionalRow = countAdditional;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode));
                    validation.ParameterValue2 = productId;
                    validation.ParameterValue3 = prefixId;
                    validation.ParameterValue4 = branchId;
                    validation.ParameterValue5 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.AgentType).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = fileId;
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

        private List<ValidationIdentificator> GetValidationsByCoinsuranceAccepted(int fileId, Template template)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            int countAdditional = 1;

            foreach (Row row in template.Rows)
            {
                ValidationIdentificator validation = new ValidationIdentificator();
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.CoInsuranceAcceptedCompany).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = fileId;
                    validation.ValidationId = (int)ValidationProperty.CoInsuranceCompany;
                    validation.AdditionalRow = countAdditional;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.CoInsuranceAcceptedCompany).Description;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.CoInsuranceAcceptedCompany));
                    validations.Add(validation);
                }
                countAdditional++;
            }
            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByCoinsuranceAssigned(int fileId, Template template)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            int countAdditional = 1;

            foreach (Row row in template.Rows)
            {
                ValidationIdentificator validation = new ValidationIdentificator();
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.CoInsuranceAssignedCompany).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = fileId;
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

        private List<ValidationLicensePlate> GetValidationsByLicensePlate(int fileId, Row row)
        {
            List<ValidationLicensePlate> validationsLicensePlate = new List<ValidationLicensePlate>();
            ValidationLicensePlate validationLicensePlate = new ValidationLicensePlate();

            validationLicensePlate = new ValidationLicensePlate();
            validationLicensePlate.Id = fileId;
            validationLicensePlate.LicensePlate = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskLicensePlate));
            validationLicensePlate.Engine = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskEngine));
            validationLicensePlate.Chassis = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskChassis));
            validationLicensePlate.CurrentFrom = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));
            if (row.Fields.Any(y => y.PropertyName == FieldPropertyName.RequestGroup))
            {
                validationLicensePlate.CurrentTo = null;
                validationLicensePlate.ParameterValue = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RequestGroup));
            }
            else
            {
                validationLicensePlate.CurrentTo = (DateTime)DelegateService.utilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentTo));
                validationLicensePlate.ParameterValue = null;
            }
            validationsLicensePlate.Add(validationLicensePlate);

            return validationsLicensePlate;
        }
    }
}