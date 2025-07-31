using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.Utilities.Constants;
using Sistran.Company.Application.CommonServices.Enums;
using Sistran.Company.Application.Location.MassivePropertyServices.EEProvider.Resources;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Co.Application.Data;
using System.Data;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;

namespace Sistran.Company.Application.Location.MassivePropertyServices.EEProvider.DAOs
{
    public class MassiveRenewalPropertyValidationDAO
    {
        public List<Validation> GetValidationsByFiles(List<File> files, MassiveRenewal massiveRenewal)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            List<ValidationTemporalPolicy> validationsTemporalPolicies = new List<ValidationTemporalPolicy>();
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();
            List<ValidationInsuredObject> validationInsuredObjects = new List<ValidationInsuredObject>();

            int branchId = 0;
            int prefixId = 0;

            foreach (File file in files)
            {
                foreach (Template template in file.Templates)
                {
                    Row row = file.Templates.FirstOrDefault(x => x.PropertyName == template.PropertyName).Rows.FirstOrDefault();

                    //------Validación Objeto del Seguro
                    if (template.PropertyName == TemplatePropertyName.InsuredObjects)
                    {
                        ValidationsByInsuredObjectSum(file.Id, row, validationInsuredObjects, file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.InsuredObjects));
                        validationInsuredObjects = GetValidationsInsuredObjectPeriodAndRate(file.Id, file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.InsuredObjects));
                        GetValidationsByInsuredObjects(file.Id, row, validationInsuredObjects);

                    }

                    validationInsuredObjects = GetValidationsInsuredObjectSum(file.Id, file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.Renewal), file.Templates.FirstOrDefault(x => x.PropertyName == TemplatePropertyName.InsuredObjects));
                    //----------------------------------

                    if (!file.Templates.Any(t => t.Rows.Any(r => r.HasError)))
                    {
                        if (template.PropertyName == TemplatePropertyName.Renewal)
                        {
                            branchId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                            prefixId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode));
                            if (massiveRenewal.Prefix.Id != prefixId)
                            {
                                row.ErrorDescription = "El ramo no corresponde al seleccionado en la caratula";
                                row.HasError = true;
                            }
                            validations.AddRange(GetValidationsByEmissionTemplate(file, template, row, validationsTemporalPolicies));
                        }

                        if (template.PropertyName == TemplatePropertyName.InsuredObjects)
                        {
                            validations.AddRange(GetValidationsByInsuredObjectsTemplate(file, template));
                        }

                        if (template.PropertyName == TemplatePropertyName.ModifyCoverages)
                        {
                            validations.AddRange(GetValidationsByModifyCoveragesTemplate(file, template));
                        }

                        if (template.PropertyName == TemplatePropertyName.AdditionalIntermediaries)
                        {
                            validations.AddRange(GetValidationsByAdditionalIntermediaries(file, template, branchId, prefixId));
                        }

                        if (template.PropertyName == TemplatePropertyName.Clauses)
                        {
                            validations.AddRange(GetValidationsClauseLevel(file, template, prefixId, (int)CoveredRiskType.Location));
                        }
                    }
                }
            }

            List<Validation> result = new List<Validation>();

            result = DelegateService.commonService.ExecuteValidations(validations, validationRegularExpressions);

            return DelegateService.commonService.GetValidatedTemporalPolicies(result, validationsTemporalPolicies);
        }

        private List<ValidationIdentificator> GetValidationsByEmissionTemplate(File file, Template template, Row row, List<ValidationTemporalPolicy> validationsTemporalPolicies)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();

            // Validacion de temporales
            ValidationTemporalPolicy validationTemporalPolicy = new ValidationTemporalPolicy();
            validationTemporalPolicy.Id = file.Id;
            validationTemporalPolicy.PrefixId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode));
            validationTemporalPolicy.BranchId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
            validationTemporalPolicy.DocumentNum = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumberRenewal));
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

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.PrefixCode).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.ValidationId = (int)ValidationProperty.Prefix;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode).Description;
                validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode));
                validations.Add(validation);
            }

            if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.AgentCode).Value))
            {
                validation = new ValidationIdentificator();
                validation.Id = file.Id;
                validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode).Description;
                validation.ValidationId = (int)ValidationProperty.AgentRen;
                validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentCode));
                validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));
                validation.ParameterValue3 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                validation.ParameterValue4 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode));
                validations.Add(validation);
            }

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

        private List<ValidationIdentificator> GetValidationsByInsuredObjectsTemplate(File file, Template template)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();
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

                //Validación Periodo de Indemnización
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == CompanyFieldPropertyName.RecoupmentPeriodId).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)CompanyValidationProperty.RecoupmentPeriodId;
                    validation.AdditionalRow = countAdditional;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RecoupmentPeriodId).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == CompanyFieldPropertyName.RecoupmentPeriodId));
                    validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredObjectCode));
                    validations.Add(validation);
                }


                countAdditional++;
            }
            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByModifyCoveragesTemplate(File file, Template template)
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

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.DeductibleCodeDeductible).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.DeductibleRen;
                    validation.AdditionalRow = countAdditional;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.DeductibleCodeDeductible).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.DeductibleCodeDeductible));
                    validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage));
                    validations.Add(validation);
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.DeductibleCodeCoverage).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.ValidationId = (int)ValidationProperty.CoverageRen;
                    validation.AdditionalRow = countAdditional;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage).Description;
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.DeductibleCodeCoverage));
                    validations.Add(validation);
                }
                countAdditional++;
            }
            return validations;
        }

        private List<ValidationIdentificator> GetValidationsByAdditionalIntermediaries(File file, Template template, int branchId, int prefixId)
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
                    validation.ParameterValue2 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));
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
                    validation.ParameterValue1 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.AgentType));
                    validations.Add(validation);
                }

                countAdditional++;
            }
            return validations;
        }

        /// <summary>
        /// Validación suma asegudara de los objetos del seguro y Obligatoriedad Suma Asegurada
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <param name="validationInsuredObjects"></param>
        private void ValidationsByInsuredObjectSum(int id, Row row, List<ValidationInsuredObject> validationInsuredObjects, Template insuredObjectTemplate)
        {
            if (insuredObjectTemplate != null)
            {
                if (validationInsuredObjects.Count > 0)
                {
                    List<Validation> resultValidations = new List<Validation>();
                    NameValue[] parameters = new NameValue[1];
                    DataTable dtInsuredObjectsValidation = new DataTable("MSV.TYPE_GET_INSURED_OBJECT");
                    dtInsuredObjectsValidation.Columns.Add("IDENTIFICATOR", typeof(int));
                    dtInsuredObjectsValidation.Columns.Add("DOCUMENT_NUM", typeof(decimal));
                    dtInsuredObjectsValidation.Columns.Add("BRANCH_CD", typeof(int));
                    dtInsuredObjectsValidation.Columns.Add("PREFIX_CD", typeof(int));
                    dtInsuredObjectsValidation.Columns.Add("RISK_NUM", typeof(int));
                    dtInsuredObjectsValidation.Columns.Add("INSURED_OBJECT", typeof(int));
                    dtInsuredObjectsValidation.Columns.Add("AMOUNT", typeof(decimal));

                    foreach (ValidationInsuredObject validationInsuredObject in validationInsuredObjects)
                    {
                        DataRow dataRow = dtInsuredObjectsValidation.NewRow();
                        dataRow["IDENTIFICATOR"] = validationInsuredObject.Id;
                        dataRow["DOCUMENT_NUM"] = validationInsuredObject.DocumentNum;
                        dataRow["BRANCH_CD"] = validationInsuredObject.BranchId;
                        dataRow["PREFIX_CD"] = validationInsuredObject.PrefixId;
                        dataRow["RISK_NUM"] = DBNull.Value;
                        dataRow["INSURED_OBJECT"] = validationInsuredObject.InsuredObjectId;
                        dataRow["AMOUNT"] = validationInsuredObject.InsuredObjectSumAssured;

                        dtInsuredObjectsValidation.Rows.Add(dataRow);
                    }

                    parameters[0] = new NameValue("GET_INSURED_OBJECT", dtInsuredObjectsValidation);

                    DataTable result;
                    using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                    {
                        result = dynamicDataAccess.ExecuteSPDataTable("MSV.GET_INSURED_OBJECT", parameters);
                    }

                    if (result != null && result.Rows.Count > 0)
                    {
                        foreach (Row rowInsuredObject in insuredObjectTemplate.Rows)
                        {
                            int insuredObjectId = (int)DelegateService.commonService.GetValueByField<int>(rowInsuredObject.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredObjectCode));
                            decimal amountValidation = (decimal)DelegateService.commonService.GetValueByField<decimal>(row.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.InsuredObjectSumAssured));

                            if (insuredObjectId == 0)
                            {
                                row.HasError = true;
                                row.ErrorDescription += Errors.TheInsuredObjectColumnOfTheInsuranceObjectsTemplateIsRequired + KeySettings.ReportErrorSeparatorMessage();
                            }
                            else
                            {
                                foreach (DataRow dataRow in result.Rows)
                                {
                                    row.HasError = true;
                                    row.ErrorDescription = (string)dataRow[1] + KeySettings.ReportErrorSeparatorMessage();
                                }
                            }
                        }
                    }
                }
            }
        }

        private List<ValidationInsuredObject> GetValidationsInsuredObjectSum(int id, Template template, Template insuredObjectTemplate)
        {
            List<ValidationInsuredObject> validationInsuredObjects = new List<ValidationInsuredObject>();
            if (template != null && insuredObjectTemplate != null)
            {
                foreach (Row row in template.Rows)
                {
                    foreach (Row rowInsuredObject in insuredObjectTemplate.Rows)
                    {
                        ValidationInsuredObject validationInsuredObject = new ValidationInsuredObject();

                        validationInsuredObject.Id = id;
                        validationInsuredObject.DocumentNum = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumberRenewal));
                        validationInsuredObject.BranchId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                        validationInsuredObject.PrefixId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode));
                        validationInsuredObject.InsuredObjectId = (int)DelegateService.commonService.GetValueByField<int>(rowInsuredObject.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredObjectCode));
                        validationInsuredObject.InsuredObjectSumAssured = (decimal)DelegateService.commonService.GetValueByField<decimal>(rowInsuredObject.Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.InsuredObjectSumAssured));
                        validationInsuredObjects.Add(validationInsuredObject);
                    }
                }
            }

            return validationInsuredObjects;
        }

        /// <summary>
        /// Validación Periodo de Indemnización y Pct del Indice Variable
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <param name="validationInsuredObjects"></param>
        public void GetValidationsByInsuredObjects(int id, Row row, List<ValidationInsuredObject> validationInsuredObjects)
        {
            if (validationInsuredObjects.Count > 0)
            {
                List<Validation> resultValidations = new List<Validation>();
                NameValue[] parameters = new NameValue[1];
                DataTable dtInsuredObjectsValidation = new DataTable("MSV.VALIDATE_TABLE_INSURED_OBJECT");
                dtInsuredObjectsValidation.Columns.Add("IDENTIFICATOR", typeof(int));
                dtInsuredObjectsValidation.Columns.Add("INSURED_OBJECT_ID", typeof(int));
                dtInsuredObjectsValidation.Columns.Add("REQUIRED_PERIOD", typeof(int));
                dtInsuredObjectsValidation.Columns.Add("REQUIRED_RATE", typeof(int));

                foreach (ValidationInsuredObject validationInsuredObject in validationInsuredObjects)
                {
                    DataRow dataRow = dtInsuredObjectsValidation.NewRow();
                    dataRow["IDENTIFICATOR"] = validationInsuredObject.Id;
                    dataRow["INSURED_OBJECT_ID"] = validationInsuredObject.InsuredObjectId;
                    dataRow["REQUIRED_PERIOD"] = validationInsuredObject.RecoupmentPeriod;
                    dataRow["REQUIRED_RATE"] = validationInsuredObject.RequiredPercentageVariableIndex;

                    dtInsuredObjectsValidation.Rows.Add(dataRow);
                }

                parameters[0] = new NameValue("INSURED_OBJECT", dtInsuredObjectsValidation);

                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("MSV.VALIDATE_INSURED_OBJECT", parameters);
                }

                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in result.Rows)
                    {

                        if (id == (int)dataRow[0])
                        {
                            row.HasError = true;
                            row.ErrorDescription += (string)dataRow[1] + KeySettings.ReportErrorSeparatorMessage();
                        }

                    }
                }
            }
        }

        private List<ValidationInsuredObject> GetValidationsInsuredObjectPeriodAndRate(int id, Template template)
        {
            List<ValidationInsuredObject> validationInsuredObjects = new List<ValidationInsuredObject>();
            if (template != null)
            {
                foreach (Row row in template.Rows)
                {
                    ValidationInsuredObject validationInsuredObject = new ValidationInsuredObject();

                    validationInsuredObject.Id = id;
                    validationInsuredObject.InsuredObjectId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.InsuredObjectCode));
                    validationInsuredObject.RecoupmentPeriod = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == CompanyFieldPropertyName.RecoupmentPeriodId));
                    validationInsuredObject.RequiredPercentageVariableIndex = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.FirstOrDefault(x => x.PropertyName == CompanyFieldPropertyName.RiskPercentageVariableIndex));
                    validationInsuredObjects.Add(validationInsuredObject);
                }
            }

            return validationInsuredObjects;
        }

        public List<ValidationIdentificator> GetValidationsClauseLevel(File file, Template template, int prefixId, int coveredRiskType)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();
            int countAdditional = 1;

            foreach (Row row in template.Rows)
            {

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.LevelCode).Value))
                {
                    int emissionLevel = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.LevelCode));

                    validation = new ValidationIdentificator();
                    validation.Id = file.Id;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.LevelCode).Description;
                    validation.ValidationId = (int)ValidationProperty.ClauseLevel;
                    validation.AdditionalRow = countAdditional;
                    validation.ParameterValue1 = emissionLevel;

                    switch (emissionLevel)
                    {
                        case (int)EmissionLevel.General:
                            validation.ParameterValue2 = prefixId;
                            break;
                        case (int)EmissionLevel.Risk:
                            validation.ParameterValue2 = coveredRiskType;
                            break;
                        case (int)EmissionLevel.Coverage:
                            validation.ParameterValue2 = 0;
                            break;
                    }

                    validation.ParameterValue3 = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.ClauseCode));
                    validations.Add(validation);
                }
                countAdditional++;
            }

            return validations;
        }
    }
}
