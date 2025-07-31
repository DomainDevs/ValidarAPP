using Sistran.Company.Application.UniquePersonListRiskBusinessService.Model;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonListRiskBusinessServiceProvider.DAO
{
    public class FileValidationDAO
    {

        public List<Validation> GetValidationsByListRiskTemplate(File file, Row row, CompanyListRiskLoad processListRisk)
        {
            List<ValidationIdentificator> validations = new List<ValidationIdentificator>();
            ValidationIdentificator validation = new ValidationIdentificator();
            List<Validation> result = new List<Validation>();
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();

            if (validations.Count > 0 || validationRegularExpressions.Count > 0)
            {
                result = Delegate.utilitiesService.ExecuteValidations(validations, validationRegularExpressions);
            }

            return result;
        }

        public File GetValidationsByFiles(File file)
        {

            string errorMessage = string.Empty;

            foreach (Template template in file.Templates)
            {

                foreach (Row row in template.Rows)
                {

                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        row.ErrorDescription = errorMessage;
                        row.HasError = true;
                    }
                }

            }


            return file;
        }
    }
}
