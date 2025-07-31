using Sistran.Co.Application.Data;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.UtilitiesServices.Models;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.UtilitiesServicesEEProvider.Assemblers;
using Sistran.Core.UtilitiesServicesEEProvider.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using COMMENT = Sistran.Core.Application.Common.Entities;


namespace Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs
{
    public class ValidationDAO
    {
        public List<Validation> ExecuteValidations(List<ValidationIdentificator> validationIdentificators, List<ValidationRegularExpression> validationRegularExpressions)
        {
            List<Validation> resultValidations = new List<Validation>();
            DataTable dtValidationParameters = new DataTable("VALIDATION_PARAMETERS");
            dtValidationParameters.Columns.Add("IDENTIFICATOR", typeof(int));
            dtValidationParameters.Columns.Add("ADDITIONAL_ROW", typeof(int));
            dtValidationParameters.Columns.Add("FIELD_DESCRIPTION", typeof(string));
            dtValidationParameters.Columns.Add("VALIDATION_ID", typeof(int));
            dtValidationParameters.Columns.Add("PARAMETER_VALUE1", typeof(int));
            dtValidationParameters.Columns.Add("PARAMETER_VALUE2", typeof(int));
            dtValidationParameters.Columns.Add("PARAMETER_VALUE3", typeof(int));
            dtValidationParameters.Columns.Add("PARAMETER_VALUE4", typeof(int));
            dtValidationParameters.Columns.Add("PARAMETER_VALUE5", typeof(int));
            dtValidationParameters.Columns.Add("PARAMETER_VALUE6", typeof(int));

            List<int> packages = DataFacadeManager.GetPackageProcesses(validationIdentificators.Count, "MaxNumberValidationsMSV");

            foreach (int package in packages)
            {

                List<ValidationIdentificator> validations = validationIdentificators.Take(package).ToList();
                validationIdentificators.RemoveRange(0, package);

                foreach (ValidationIdentificator validation in validations)
                {
                    DataRow dataRow = dtValidationParameters.NewRow();

                    dataRow["IDENTIFICATOR"] = validation.Id;
                    dataRow["VALIDATION_ID"] = validation.ValidationId;
                    dataRow["FIELD_DESCRIPTION"] = validation.FieldDescription;

                    if (validation.AdditionalRow != null)
                    {
                        dataRow["ADDITIONAL_ROW"] = validation.AdditionalRow;
                    }
                    dataRow["PARAMETER_VALUE1"] = validation.ParameterValue1;

                    if (validation.ParameterValue2 > 0)
                    {
                        dataRow["PARAMETER_VALUE2"] = validation.ParameterValue2;
                    }

                    if (validation.ParameterValue3 > 0)
                    {
                        dataRow["PARAMETER_VALUE3"] = validation.ParameterValue3;
                    }

                    if (validation.ParameterValue4 > 0)
                    {
                        dataRow["PARAMETER_VALUE4"] = validation.ParameterValue4;
                    }

                    if (validation.ParameterValue5 > 0)
                    {
                        dataRow["PARAMETER_VALUE5"] = validation.ParameterValue5;
                    }

                    if (validation.ParameterValue6 > 0)
                    {
                        dataRow["PARAMETER_VALUE6"] = validation.ParameterValue6;
                    }
                    dtValidationParameters.Rows.Add(dataRow);
                }

                NameValue[] parameters = new NameValue[1];
                parameters[0] = new NameValue("@VALIDATION_PARAMETERS", dtValidationParameters);

                DataTable dataTable;
                using (DynamicDataAccess dataAccess = new DynamicDataAccess())
                {
                    dataTable = dataAccess.ExecuteSPDataTable("COMM.EXECUTE_VALIDATIONS", parameters);
                }
                dtValidationParameters.Clear();

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        resultValidations.Add(new Validation
                        {
                            Id = (int)dataRow[0],
                            ErrorMessage = (string)dataRow[1]
                        });
                    }
                }

                if (validationRegularExpressions.Count > 0)
                {
                    resultValidations = ValidateRegularExpressions(validationRegularExpressions, resultValidations);
                }
            }

            return resultValidations;
        }

        private List<Validation> ValidateRegularExpressions(List<ValidationRegularExpression> validationRegularExpressions, List<Validation> resultValidations)
        {
            List<ValidationRegularExpression> resultExpressions = GetAllValidationRegularExpressions();

            foreach (ValidationRegularExpression item in validationRegularExpressions)
            {
                Match match = Regex.Match(item.ParameterValue, resultExpressions.Find(x => x.Id == item.ValidationId).ParameterValue, RegexOptions.IgnoreCase);

                if (!match.Success)
                {

                    string message = resultExpressions.Find(x => x.Id == item.ValidationId).ErrorMessage;

                    if (resultValidations.Find(x => x.Id == item.Id) != null)
                    {
                        if (message != null)
                        {
                            resultValidations.Find(x => x.Id == item.Id).ErrorMessage += ", " + string.Format(message, item.FieldDescription);
                        }
                        else
                        {
                            resultValidations.Find(x => x.Id == item.Id).ErrorMessage += ", " + string.Format(Errors.ErrorValidateRegularExpression, item.FieldDescription);
                        }
                    }
                    else
                    {
                        Validation validation = new Validation();
                        validation.Id = item.Id;
                        if (message != null)
                        {
                            validation.ErrorMessage = string.Format(message, item.FieldDescription);
                        }
                        else
                        {
                            validation.ErrorMessage = string.Format(Errors.ErrorValidateRegularExpression, item.FieldDescription);
                        }

                        resultValidations.Add(validation);
                    }
                }
            }
            return resultValidations;
        }

        public  List<ValidationRegularExpression> GetAllValidationRegularExpressions()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMENT.ValidationRegularExpression)));
            List<ValidationRegularExpression> validationRegularExpressions = ModelAssembler.CreateValidationRegularExpressions(businessCollection);
            return validationRegularExpressions;
        }

        public List<Validation> GetValidatedTemporalPolicies(List<Validation> validations, List<ValidationTemporalPolicy> temporalpolicies)
        {
            if (temporalpolicies.Count > 0)
            {
                List<Validation> resultValidationsTemporalPolicy = new List<Validation>();
                NameValue[] parameters = new NameValue[1];
                DataTable dtTemporalPolicy = new DataTable("VALIDATE_TEMPORAL_POLICY");
                dtTemporalPolicy.Columns.Add("IDENTIFICATOR", typeof(int));
                dtTemporalPolicy.Columns.Add("DOCUMENT_NUMBER", typeof(decimal));
                dtTemporalPolicy.Columns.Add("BRANCH_CD", typeof(int));
                dtTemporalPolicy.Columns.Add("PREFIX_CD", typeof(int));


                foreach (ValidationTemporalPolicy temporal in temporalpolicies)
                {
                    DataRow dataRow = dtTemporalPolicy.NewRow();
                    dataRow["IDENTIFICATOR"] = temporal.Id;
                    dataRow["DOCUMENT_NUMBER"] = temporal.DocumentNum;
                    dataRow["BRANCH_CD"] = temporal.BranchId;
                    dataRow["PREFIX_CD"] = temporal.PrefixId;

                    dtTemporalPolicy.Rows.Add(dataRow);
                }

                parameters[0] = new NameValue("VALIDATE_TEMPORAL_POLICY", dtTemporalPolicy);

                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("MSV.VALIDATE_TEMPORAL_POLICY", parameters);
                }
                if (result != null && result.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in result.Rows)
                    {
                        resultValidationsTemporalPolicy.Add(new Validation
                        {
                            Id = (int)dataRow.ItemArray[0],
                            ErrorMessage = (string)dataRow.ItemArray[1]
                        });
                    }
                    if (validations.Count > 0)
                    {
                        foreach (Validation item in resultValidationsTemporalPolicy)
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
                        validations.AddRange(resultValidationsTemporalPolicy);
                    }
                }
            }

            return validations;
        }
    }
}