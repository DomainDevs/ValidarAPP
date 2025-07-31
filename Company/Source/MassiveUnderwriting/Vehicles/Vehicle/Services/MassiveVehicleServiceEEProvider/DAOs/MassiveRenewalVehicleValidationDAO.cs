using Sistran.Company.Application.Utilities.Constants;
using Sistran.Company.Application.Vehicles.MassiveVehicleServices.EEProvider.Resources;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.UtilitiesServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleServices.EEProvider.DAOs
{
    public class MassiveRenewalVehicleValidationDAO
    {
        public List<Validation> GetValidationsByFiles(List<File> files, MassiveRenewal massiveRenewal)
        {
            List<ValidationTemporalPolicy> validationsTemporalPolicies = new List<ValidationTemporalPolicy>();
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            List<ValidationRegularExpression> validationsRegularExpressions = new List<ValidationRegularExpression>();

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
                            branchId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                            prefixId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode));
                            if (massiveRenewal.Prefix.Id != prefixId)
                            {
                                row.ErrorDescription = "El ramo no corresponde al seleccionado en la caratula";
                                row.HasError = true;
                            }

                            validations.AddRange(GetValidationsByEmissionTemplate(file, template, row, validationsTemporalPolicies, massiveRenewal.User.UserId));
                        }

                        if (template.PropertyName == CompanyTemplatePropertyName.AdditionalCoverages)
                        {
                            validations.AddRange(GetValidationsByAdditionalCoveragesTemplate(file, template));
                        }

                        if (template.PropertyName == TemplatePropertyName.Deductible)
                        {
                            validations.AddRange(GetValidationsByDeductibleTemplate(file, template));
                        }

                        if (template.PropertyName == TemplatePropertyName.Accesories)
                        {
                            validations.AddRange(GetValidationsByAccesoriesTemplate(file, template));
                        }

                        if (template.PropertyName == TemplatePropertyName.AdditionalIntermediaries)
                        {
                            validations.AddRange(GetValidationsByAdditionalIntermediaries(file, template, branchId, prefixId));
                        }
                        if (template.PropertyName == TemplatePropertyName.Clauses)
                        {
                            validations.AddRange(GetValidationsClauseLevel(file, template, prefixId, (int)CoveredRiskType.Vehicle));
                        }
                    }
                }
            }

            List<Validation> result = new List<Validation>();
            result = DelegateService.utilitiesService.ExecuteValidations(validations, validationsRegularExpressions);

            return DelegateService.utilitiesService.GetValidatedTemporalPolicies(result, validationsTemporalPolicies);
        }

        private List<ValidationIdentificator> GetValidationsByEmissionTemplate(File file, Template template, Row row, List<ValidationTemporalPolicy> validationsTemporalPolicies, int userId)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();

            // Validacion de temporales
            ValidationTemporalPolicy validationTemporalPolicy = new ValidationTemporalPolicy();
            validationTemporalPolicy.Id = file.Id;
            validationTemporalPolicy.PrefixId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode));
            validationTemporalPolicy.BranchId = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
            validationTemporalPolicy.DocumentNum = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumberRenewal));
            validationsTemporalPolicies.Add(validationTemporalPolicy);

            #region Renovación

            validation = new ValidationIdentificator();
            validation.Id = file.Id;
            validation.ValidationId = (int)ValidationProperty.SalePoint;
            validation.FieldDescription = "Punto de venta";
            validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
            validation.ParameterValue2 = userId;
            validation.ParameterValue4 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumberRenewal));
            validation.ParameterValue5 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode));
            validations.Add(validation);

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BranchId).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.Branch;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId).Description;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                validation.ParameterValue2 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.SalePoint));
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

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.AgentCode).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode).Description;
                validation.ValidationId = (int)ValidationProperty.AgentAgencyRenewal;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode));
                validation.ParameterValue2 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));
                validation.ParameterValue3 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                validation.ParameterValue4 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode));

                validations.Add(validation);
            }

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.AgentType).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType).Description;
                validation.ValidationId = (int)ValidationProperty.AgentType;
                validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));
                validations.Add(validation);
            }

            #endregion

            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByAdditionalCoveragesTemplate(File file, Template template)
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
                    validation.ValidationId = (int)ValidationProperty.CoverageRenewal;
                    validation.AdditionalRow = countAdditional;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage));
                    validations.Add(validation);
                }
                countAdditional++;
            }
            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByDeductibleTemplate(File file, Template template)
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

        private List<ValidationIdentificator> GetValidationsByAccesoriesTemplate(File file, Template template)
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

        private List<ValidationIdentificator> GetValidationsByAdditionalIntermediaries(File file, Template template, int branchId, int prefixId)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            int countAdditional = 1;
            Row rowRenewal = file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.Renewal).Rows.FirstOrDefault();

            foreach (Row row in template.Rows)
            {
                ValidationIdentificator validation = new ValidationIdentificator();
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.AgentCode).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode).Description;
                    validation.ValidationId = (int)ValidationProperty.AgentAgencyRenewal;
                    validation.AdditionalRow = countAdditional;
                    validation.ParameterValue1 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode));
                    validation.ParameterValue2 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));
                    validation.ParameterValue3 = branchId;
                    validation.ParameterValue4 = prefixId;

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

        public List<ValidationIdentificator> GetValidationsClauseLevel(File file, Template template, int prefixId, int coveredRiskType)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            int countAdditional = 1;
            List<ConditionLevel> conditionLevels = DelegateService.underwritingService.GetConditionLevels();

            foreach (Row row in template.Rows)
            {
                ValidationIdentificator validation = new ValidationIdentificator();
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.LevelCode).Value))
                {
                    int levelCode = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.LevelCode));
                    ConditionLevel conditionLevel = conditionLevels.FirstOrDefault(c => c.Id == levelCode);

                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.LevelCode).Description;
                    validation.ValidationId = (int)ValidationProperty.ClauseLevel;
                    validation.AdditionalRow = countAdditional;
                    validation.ParameterValue1 = levelCode;

                    if (conditionLevel != null)
                    {
                        switch (conditionLevel.EmissionLevel)
                        {
                            case EmissionLevel.General:
                                if (conditionLevel.Id == (int)ConditionLevelType.Independent)
                                    validation.ParameterValue2 = 0;
                                else
                                    validation.ParameterValue2 = prefixId;
                                break;
                            case EmissionLevel.Risk:
                                validation.ParameterValue2 = coveredRiskType;
                                break;
                            case EmissionLevel.Coverage:
                                validation.ParameterValue2 = 0;
                                break;
                        }
                    }

                    validation.ParameterValue3 = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.ClauseCode));
                    validations.Add(validation);
                }
                countAdditional++;
            }
            return validations;
        }
    }
}
