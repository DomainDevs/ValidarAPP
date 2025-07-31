using Sistran.Co.Application.Data;
using Sistran.Company.Application.CancellationMsvEndorsementServices.EEProvider.Resources;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Core.Application.CancellationEndorsement.Enums;
using Sistran.Core.Application.CancellationMassiveEndorsement.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.UtilitiesServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sistran.Company.Application.CancellationMsvEndorsementServicesServices.EEProvider.DAOs
{
    public class CancellationMassiveValidationDAO
    {
        public List<Validation> GetValidationsByFiles(Template cancellationTemplate, MassiveLoad massiveLoad)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();
            List<CancellationValidation> cancellationValidations = new List<CancellationValidation>();
            List<ValidationTemporalPolicy> validationsTemporalPolicies = new List<ValidationTemporalPolicy>();

            validations = GetValidationsByEmissionTemplate(cancellationTemplate, cancellationValidations, validationsTemporalPolicies);
            List<Validation> result = new List<Validation>();
            result = DelegateService.UtilitiesService.ExecuteValidations(validations, validationRegularExpressions);
            return DelegateService.UtilitiesService.GetValidatedTemporalPolicies(result, validationsTemporalPolicies);
        }

        private List<ValidationIdentificator> GetValidationsByEmissionTemplate(Template template, List<CancellationValidation> cancellationValidations, List<ValidationTemporalPolicy> validationsTemporalPolicies)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();

            #region Póliza
            foreach (Row row in template.Rows.Where(u => !u.HasError))
            {
                // Validacion de temporales
                ValidationTemporalPolicy validationTemporalPolicy = new ValidationTemporalPolicy();
                validationTemporalPolicy.Id = row.Number;
                validationTemporalPolicy.PrefixId = (int)DelegateService.UtilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode));
                validationTemporalPolicy.BranchId = (int)DelegateService.UtilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                validationTemporalPolicy.DocumentNum = (decimal)DelegateService.UtilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumber));
                validationsTemporalPolicies.Add(validationTemporalPolicy);


                CancellationValidation cancellationValidation = new CancellationValidation();
                cancellationValidation.Id = row.Number;
                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.BranchId).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = row.Number;
                    validation.ValidationId = (int)ValidationProperty.Branch;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId).Description;
                    validation.ParameterValue1 = (int)DelegateService.UtilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BranchId));
                    validations.Add(validation);
                    cancellationValidation.BranchId = validation.ParameterValue1;
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.PrefixCode).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = row.Number;
                    validation.ValidationId = (int)ValidationProperty.Prefix;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode).Description;
                    validation.ParameterValue1 = (int)DelegateService.UtilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PrefixCode));
                    validations.Add(validation);
                    cancellationValidation.PrefixId = validation.ParameterValue1;
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.TypeCancellation).Value))
                {
                    //validation = new ValidationIdentificator();
                    //validation.Id = row.Number;
                    //validation.ValidationId = (int)ValidationProperty.TypeCancellation;
                    //validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.TypeCancellation).Description;
                    //validation.ParameterValue1 = (int)DelegateService.UtilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.TypeCancellation));
                    //validations.Add(validation);
                    //cancellationValidation.CancellationType = validation.ParameterValue1;

                    var cancellationType = (int)DelegateService.UtilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.TypeCancellation));

                    if (cancellationType != (int)CancellationType.BeginDate && cancellationType != (int)CancellationType.FromDate &&
                        cancellationType != (int)CancellationType.ShortTerm && cancellationType != (int)CancellationType.Nominative)
                    {
                        row.HasError = true;
                        row.ErrorDescription += Errors.ErrorCancellationType + KeySettings.ReportErrorSeparatorMessage();
                    }
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.CancellationReason).Value))
                {
                    validation = new ValidationIdentificator();
                    validation.Id = row.Number;
                    //validation.ValidationId = (int)ValidationProperty.CancellationReason;
                    validation.ValidationId = (int)ValidationProperty.EndorsemenrReason;
                    validation.FieldDescription = row.Fields.Find(x => x.PropertyName == FieldPropertyName.CancellationReason).Description;
                    validation.ParameterValue1 = (int)DelegateService.UtilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.TypeCancellation));
                    if (validation.ParameterValue1 == (int)CancellationType.BeginDate || validation.ParameterValue1 == (int)CancellationType.FromDate || validation.ParameterValue1 == (int)CancellationType.ShortTerm)
                    {
                        validation.ParameterValue1 = (int)EndorsementType.Cancellation;
                    }
                    else
                    {
                        if (validation.ParameterValue1 == (int)CancellationType.Nominative)
                        {
                            validation.ParameterValue1 = (int)EndorsementType.Nominative_cancellation;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(row.Fields.Find(y => y.PropertyName == FieldPropertyName.PolicyCurrentFrom).Value))
                {
                    cancellationValidation.CurrentFrom = (DateTime)DelegateService.UtilitiesService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom));
                }
                validation.ParameterValue2 = (int)DelegateService.UtilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.CancellationReason));
                validations.Add(validation);
                cancellationValidation.PolicyNumber = (decimal)DelegateService.UtilitiesService.GetValueByField<decimal>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.PolicyNumber));
                cancellationValidations.Add(cancellationValidation);

            }

            #endregion

            return validations;
        }

        public List<Validation> GetCancellationValidate(List<Validation> validations, List<CancellationValidation> cancellationValidations)
        {

            if (cancellationValidations.Count > 0)
            {
                List<Validation> resultValidations = new List<Validation>();
                NameValue[] parameters = new NameValue[1];
                DataTable dtCancellationValidation = new DataTable("MSV.VALIDATE_CANCELLATION");
                dtCancellationValidation.Columns.Add("IDENTIFICATOR", typeof(int));
                dtCancellationValidation.Columns.Add("PREFIX_ID", typeof(string));
                dtCancellationValidation.Columns.Add("BRANCH_ID", typeof(string));
                dtCancellationValidation.Columns.Add("DOCUMENT_NUM", typeof(string));
                dtCancellationValidation.Columns.Add("CURRENT_FROM", typeof(DateTime));
                dtCancellationValidation.Columns.Add("CANCELLATION_TYPE", typeof(int));


                foreach (CancellationValidation cancellationValidation in cancellationValidations)
                {
                    DataRow dataRow = dtCancellationValidation.NewRow();
                    dataRow["IDENTIFICATOR"] = cancellationValidation.Id;
                    dataRow["PREFIX_ID"] = cancellationValidation.PrefixId;
                    dataRow["BRANCH_ID"] = cancellationValidation.BranchId;
                    dataRow["DOCUMENT_NUM"] = cancellationValidation.PolicyNumber;
                    dataRow["CURRENT_FROM"] = cancellationValidation.CurrentFrom;
                    dataRow["CANCELLATION_TYPE"] = cancellationValidation.CancellationType;

                    dtCancellationValidation.Rows.Add(dataRow);

                }

                parameters[0] = new NameValue("VALIDATE_CANCELLATION", dtCancellationValidation);

                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("ISS.VALIDATE_MASSIVE_CANCELLATION", parameters);
                }
                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in result.Rows)
                    {
                        resultValidations.Add(new Validation
                        {
                            Id = (int)dataRow[0],
                            ErrorMessage = (string)dataRow[1]
                        });
                    }
                }
                if (validations.Count > 0)
                {
                    foreach (Validation item in resultValidations)
                    {
                        if (validations.Find(x => x.Id == item.Id) != null)
                        {
                            validations.Find(x => x.Id == item.Id).ErrorMessage += " " + item.ErrorMessage;
                        }
                        else
                        {
                            Validation validation = new Validation();
                            validation.Id = item.Id;
                            validation.ErrorMessage = item.ErrorMessage;
                            validations.Add(validation);
                        }
                    }
                }
                else
                {
                    validations.AddRange(resultValidations);
                }
            }

            return validations;
        }
    }
}

