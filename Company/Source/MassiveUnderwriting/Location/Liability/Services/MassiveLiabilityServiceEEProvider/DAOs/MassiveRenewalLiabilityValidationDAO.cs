using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.Utilities.Constants;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.Location.MassiveLiabilityServices.EEProvider.DAOs
{
    public class MassiveRenewalLiabilityValidationDAO
    {
        public List<Validation> GetValidationsByFiles(List<File> files, MassiveRenewal massiveRenewal)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();

            decimal policyId = 0;
            int branchId = 0;
            int prefixId = 0;

            foreach (File file in files)
            {
                if (!file.Templates.Any(t => t.Rows.Any(r => r.HasError)))
                {
                    foreach (Template template in file.Templates)
                    {
                        Row row = file.Templates.FirstOrDefault(x => x.PropertyName == template.PropertyName).Rows.FirstOrDefault();
                        if (template.PropertyName == TemplatePropertyName.Renewal)
                        {
                            branchId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                            prefixId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode));
                            policyId = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumberRenewal));
                            validations.AddRange(GetValidationsByEmissionTemplate(file, template, row, massiveRenewal));
                        }

                        if (template.PropertyName == TemplatePropertyName.InsuredObjects)
                        {
                            validations.AddRange(GetValidationsByInsuredObjectsTemplate(file, template, massiveRenewal, branchId, prefixId, policyId));
                        }

                        if (template.PropertyName == TemplatePropertyName.AdditionalIntermediaries)
                        {
                            validations.AddRange(GetValidationsByAdditionalIntermediaries(file, template, massiveRenewal, branchId, prefixId, policyId));
                        }
                    }
                }
            }
            return DelegateService.commonService.ExecuteValidations(validations, validationRegularExpressions);
        }

        private List<ValidationIdentificator> GetValidationsByEmissionTemplate(File file, Template template, Row row, MassiveRenewal massiveRenewal)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();

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

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.PrefixCode).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.Prefix;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode).Description;
                validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode));
                validations.Add(validation);
            }

            //if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.AgentCode).Value))
            //{
            //    validation = new Validation();
            //    validation.Id = file.Id;
            //    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode).Description;
            //    validation.ValidationId = (int)ValidationProperty.AgentRen;
            //    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode));
            //    validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
            //    validation.ParameterValue3 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode));
            //    validation.ParameterValue4 = (int)((decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumberRenewal)));
            //    validations.Add(validation);
            //}

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.AgentType).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType).Description;
                validation.ValidationId = (int)ValidationProperty.AgentType;
                validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));
                validations.Add(validation);
            }

            #endregion

            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByInsuredObjectsTemplate(File file, Template template, MassiveRenewal massiveRenewal, int branchId, int prefixId, decimal policyId)
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
                    validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredObjectCode));
                    validation.ParameterValue3 = (int)policyId;
                    validation.ParameterValue4 = branchId;
                    validation.ParameterValue5 = prefixId;
                    validations.Add(validation);
                }
                countAdditional++;
            }
            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByAdditionalIntermediaries(File file, Template template, MassiveRenewal massiveRenewal, int branchId, int prefixId, decimal policyId)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();
            int countAdditional = 1;
            Row rowRenewal = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.Renewal).Rows.FirstOrDefault();

            foreach (Row row in template.Rows)
            {
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.AgentCode).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode).Description;
                    validation.ValidationId = (int)ValidationProperty.AgentRen;
                    validation.AdditionalRow = countAdditional;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode));
                    validation.ParameterValue2 = branchId;
                    validation.ParameterValue3 = prefixId;
                    validation.ParameterValue4 = (int)policyId;
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.AgentType).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType).Description;
                    validation.ValidationId = (int)ValidationProperty.AgentType;
                    validation.AdditionalRow = countAdditional;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));
                    validations.Add(validation);
                }

                countAdditional++;
            }
            return validations;
        }
    }
}
