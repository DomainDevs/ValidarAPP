using System.Collections.Generic;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Company.Application.MassiveVehicleParamBusinessService.Model;
using System.Linq;

namespace Sistran.Company.Application.MassiveVehicleParamBusinessServiceProvider.Dao
{
    public class FaseColdaValidationDAO
    {
        public List<Validation> GetValidationsByFaseColdaTemplate(File file, Row row, CompanyProcessFasecolda processFasecolda)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();
            List<Validation> result = new List<Validation>();
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();
            //validationRegularExpressions.AddRange(DelegateService.collectiveService.GetRegularExpressionValidationsByEmisionTemplate(file.Id, row));

            if (validations.Count > 0 || validationRegularExpressions.Count > 0)
            {
                result = DelegateService.utilitiesService.ExecuteValidations(validations, validationRegularExpressions);
            }

            return result;
        }

        public List<Validation> GetValidationsByFiles(List<File> files, CompanyProcessFasecolda processFasecolda)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();
            List<ValidationFaseColda> validationFaseColda = new List<ValidationFaseColda>();
            //ESTAS VALIDACIONES SE DEBEN HACER SI EXISTE UNA REGLA DE NEGOCIO, POR EJEMPLO, QUE EXISTA EL CODIGO FASECOLDA EN EL TEMPLATE CODIGO
            int errrorCount = 0;
            try
            {
                foreach (File file in files)
                {
                    if (file.Templates.Any(x => x.Rows.Any()))
                    {
                        foreach (Template template in file.Templates)
                        {
                            int templateIndex = file.Templates.IndexOf(template);
                            foreach (Row row in template.Rows)
                            {
                                int rowIndex = template.Rows.IndexOf(row);
                                foreach (Field field in row.Fields)
                                {
                                    if (field.Description != "" && field.Value == ""  && field.IsMandatory)
                                    {
                                       // file.Templates[templateIndex].Rows[rowIndex].HasError = true;
                                        validations.Add(new ValidationIdentificator
                                        {
                                            Id = file.Id,
                                            ValidationId = field.Id,
                                            ErrorMessage = "El campo no puede estar vacio",
                                            FieldDescription = field.Description,
                                            AdditionalRow = 0,
                                        });
                                        errrorCount++;
                                    }

                                }
                            }
                        }
                    }

                }



                //foreach (File file in files)
                //{
                //    if (!file.Templates.Any(t => t.Rows.Any(r => r.HasError)))
                //    {
                //        foreach (Template template in file.Templates)
                //        {
                //            if (template.PropertyName == TemplatePropertyName.RiskDetail)
                //            {
                //                validations.AddRange(GetValidationsByRiskDetailTemplate(file, template, collectiveEmission, productId, policyType));
                //                if (correlativePolicy <= 0)
                //                {
                //                    validationsLicencePlate.AddRange(GetValidationsByLicensePlate(file.Id, currentTo, currentFrom, template));
                //                }
                //                else
                //                {
                //                    validationsCorrelativePolicy.AddRange(GetValidationsByCorrelativePolicy(file.Id, collectiveEmission, correlativePolicy, currentTo, currentFrom, template));
                //                }
                //                validationsLicencePlate.AddRange(GetValidationsByLicensePlate(file.Id, currentTo, currentFrom, template));
                //                validationsPhoneType.AddRange(DelegateService.collectiveService.GetPhoneTypesValidationsByRiskDetailTemplate(file.Id, template));

                //                foreach (Row row in template.Rows)
                //                {
                //                    validationRegularExpressions.AddRange(DelegateService.collectiveService.GetRegularExpressionValidationsByRiskTemplate(file.Id, row));
                //                }
                //            }
                //        }
                //    }
                //}
                List<Validation> result = new List<Validation>();
                if (validations.Count > 0 || validationRegularExpressions.Count > 0)
                {
                    //   result = DelegateService.utilitiesService.ExecuteValidations(validations, validationRegularExpressions);
                    validations.ForEach(x => result.Add(new Validation
                    {
                        Id = x.Id,
                        ValidationId = x.ValidationId,
                        AdditionalRow = x.AdditionalRow,
                        FieldDescription = x.FieldDescription,
                        ErrorMessage = x.ErrorMessage
                    }));
                }
                //Si alguna de las listas contiene algun valor, agregarlo a result
                //if (validationsLicencePlate.Count > 0)
                //{
                //    result = DelegateService.vehicleService.GetVehicleByLicensePlate(result, validationsLicencePlate);
                //}

                //Validar utilities
                //if (validationsPhoneType.Count > 0)
                //{
                //    result = DelegateService.utilitiesService.GetValidatedPhoneTypes(validationsPhoneType, result);
                //}*/

                return result;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
    }
}
