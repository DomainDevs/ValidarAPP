using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.Utilities.Managers
{
    public class ManagerDAO
    {
        /// <summary>
        /// Obtener Parametro Por Id
        /// </summary>
        /// <param name="parameterId">Id Parametro</param>
        /// <returns>Parametro</returns>
        public static Parameter GetParameterByParameterId(int parameterId)
        {
            PrimaryKey key = Parameter.CreatePrimaryKey(parameterId);
            return (Parameter)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        }
    }
}