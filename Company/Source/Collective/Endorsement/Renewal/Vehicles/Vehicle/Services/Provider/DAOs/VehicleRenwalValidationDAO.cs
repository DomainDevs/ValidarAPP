using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Services.UtilitiesServices;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.Application.UtilitiesServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Company.Application.Vehicles.CollectiveVehicleRenewalServiceEEProvider.Resources;

namespace Sistran.Company.Application.Vehicles.CollectiveVehicleRenewalServiceEEProvider.DAOs
{
    public class VehicleRenwalValidationDAO
    {
        public List<Validation> GetValidationsByFiles(List<File> files, CollectiveEmission collectiveEmission, int productId, List<CompanyVehicle> companyVehicleRisks)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();

            foreach (File file in files)
            {
                if (!file.Templates.Any(t => t.Rows.Any(r => r.HasError)))
                {
                    foreach (Template template in file.Templates)
                    {
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

                        if (template.PropertyName == TemplatePropertyName.RiskDetail)
                        {
                            validations.AddRange(GetValidationsByRiskTemplate(file, template, companyVehicleRisks));
                        }
                    }
                }
            }
            return DelegateService.utilitiesService.ExecuteValidations(validations, validationRegularExpressions);
        }

        public List<Validation> GetValidationsByEmissionTemplate(Template emissionTemplate, CollectiveEmission collectiveEmission)
        {
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();
            List<ValidationTemporalPolicy> validationsTemporalPolicies = new List<ValidationTemporalPolicy>();
            Row row = emissionTemplate.Rows.First();
            List<ValidationIdentificator> validations = GetValidationsByEmissionTemplate(new File { Id = 1 }, row, collectiveEmission, validationsTemporalPolicies);

            List<Validation> result = new List<Validation>();

            result = DelegateService.utilitiesService.ExecuteValidations(validations, validationRegularExpressions);
            return DelegateService.utilitiesService.GetValidatedTemporalPolicies(result, validationsTemporalPolicies);
        }

        public List<ValidationIdentificator> GetValidationsByEmissionTemplate(File file, Row row, CollectiveEmission collectiveEmission, List<ValidationTemporalPolicy> validationsTemporalPolicies)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();

            // Validacion de temporales
            ValidationTemporalPolicy validationTemporalPolicy = new ValidationTemporalPolicy();
            validationTemporalPolicy.Id = file.Id;
            validationTemporalPolicy.PrefixId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyPrefixCode));
            validationTemporalPolicy.BranchId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
            validationTemporalPolicy.DocumentNum = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumber));
            validationsTemporalPolicies.Add(validationTemporalPolicy);

            #region Renovación

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BranchId).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.Branch;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId).Description;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                validations.Add(validation);

                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.SalePoint;
                validation.FieldDescription = "Punto de venta";
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                validation.ParameterValue2 = collectiveEmission.User.UserId;
                validation.ParameterValue4 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumber));
                validation.ParameterValue5 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyPrefixCode));
                validations.Add(validation);
            }
            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.PolicyPrefixCode).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.Prefix;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyPrefixCode).Description;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyPrefixCode));
                validations.Add(validation);
            }

            #endregion

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

        private List<ValidationIdentificator> GetValidationsByRiskTemplate(File file, Template template, List<CompanyVehicle> companyVehicleRisks)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            //ValidationIdentificator validation = new ValidationIdentificator();
            //int countAdditional = 1;

            //foreach (Row row in template.Rows)
            //{
            //    if (!companyVehicleRisks.Any(r => r.LicensePlate == row.Fields.Single(y => y.PropertyName == FieldPropertyName.RiskText).Value))
            //    {
            //        validation = new ValidationIdentificator();
            //        validation.Id = file.Id;
            //        validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskText).Description;
            //        validation.ValidationId = (int)ValidationProperty.RiskActivity;
            //        validation.AdditionalRow = countAdditional;
            //        validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RiskText));
            //        validations.Add(validation);
            //    }
            //    countAdditional++;
            //}

            return validations;
        }
    }
}
