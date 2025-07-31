using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.MassiveServices.EEProvider.Business
{
    public class BusinessIndividual
    {
        public static IndividualType FindIndividualTypeByRow(Row row, string propertyNameIndividualType, string propertyNamePersonDocument, string propertyNameCompanyDocument)
        {
            int result = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(p => p.PropertyName == propertyNameIndividualType));
            if (result > 0)
            {
                return (IndividualType)result;
            }

            string companyDocNumber = row.Fields.Any(y => y.PropertyName == propertyNameCompanyDocument) ? row.Fields.Find(y => y.PropertyName == propertyNameCompanyDocument).Value : string.Empty;
            string personDocNumber = row.Fields.Any(y => y.PropertyName == propertyNamePersonDocument) ? row.Fields.Find(y => y.PropertyName == propertyNamePersonDocument).Value : string.Empty;

            if (!string.IsNullOrEmpty(companyDocNumber))
            {
                return IndividualType.Company;
            }
            else if (!string.IsNullOrEmpty(personDocNumber))
            {
                return IndividualType.Person;
            }

            return (IndividualType)result;

        }

    }
}
