using Sistran.Company.Application.CommonService.Assemblers;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.CommonServices.EEProvider.DAOs
{
    public class ParameterDAO
    {
        /// <summary>
        /// Obtiene el cptparameter por código
        /// </summary>
        /// <param name="parameterId">Código</param>
        /// <returns>Parameter</returns>
        public CompanyParameter FindCoParameter(int parameterId)
        {
            CoParameter coParameter = null;
            PrimaryKey key = CoParameter.CreatePrimaryKey(parameterId);
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                coParameter = (CoParameter)daf.GetObjectByPrimaryKey(key);
            }
            
            CompanyParameter parameter = ModelAssembler.CreateParameter(coParameter);

            return parameter;
        }
    }
}
