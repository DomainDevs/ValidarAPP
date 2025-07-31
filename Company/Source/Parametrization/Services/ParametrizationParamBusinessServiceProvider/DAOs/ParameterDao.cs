using Sistran.Company.Application.ParametrizationParamBusinessService.Model;
using Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.Assemblers;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.DAOs
{
    public class ParameterDAO
    {
        public CompanyParameters GetParameterByParameterId(int parameterId)
        {
            PrimaryKey primaryKey = Parameter.CreatePrimaryKey(parameterId);
            Parameter entityParameter = (Parameter)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

            if (entityParameter != null)
            {
                return ModelAssembler.CreateParameter(entityParameter);
            }
            else
            {
                return null;
            }
        }
    }
}
