using Sistran.Company.Application.CommonServices.Models;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Utilities.Constants;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.Location.CollectivePropertyRenewalServiceEEProvider.DAOs
{
    public class RenewalValidationDAO
    {
        public List<Validation> GetValidationsByFiles(List<File> files, CollectiveEmission collectiveEmission, int productId)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();

            foreach (File file in files)
            {
                if (!file.Templates.Any(t => t.Rows.Any(r => r.HasError)))
                {
                    foreach (Template template in file.Templates)
                    {
                        if (template.PropertyName == TemplatePropertyName.InsuredObjects)
                        {
                            validations.AddRange(GetValidationsByInsuredObjectsTemplate(file, template, productId));
                        }

                        if (template.PropertyName == TemplatePropertyName.ModifyCoverages)
                        {
                            validations.AddRange(GetValidationsByModifyCoveragesTemplate(file, template, productId));
                        }
                    }
                }
            }
            return DelegateService.commonService.ExecuteValidations(validations, validationRegularExpressions);
        }


        public List<Validation> GetValidationsByEmissionTemplate(Template emissionTemplate)
        {
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();
            List<ValidationTemporalPolicy> validationsTemporalPolicies = new List<ValidationTemporalPolicy>();
            Row row = emissionTemplate.Rows.First();
            List<ValidationIdentificator> validations = GetValidationsByEmissionTemplate(new File { Id = 1 }, row, validationsTemporalPolicies);

            List<Validation> result = new List<Validation>();

            result = DelegateService.commonService.ExecuteValidations(validations, validationRegularExpressions);
            return DelegateService.commonService.GetValidatedTemporalPolicies(result, validationsTemporalPolicies);
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

            #region Renovación

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

        private List<ValidationIdentificator> GetValidationsByInsuredObjectsTemplate(File file, Template template, int productId)
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
                    validation.ValidationId = (int)ValidationProperty.InsuredObjectRen;
                    validation.AdditionalRow = countAdditional;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredObjectCode).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredObjectCode));
                    validations.Add(validation);
                }
                countAdditional++;
            }
            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByModifyCoveragesTemplate(File file, Template template, int productId)
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
