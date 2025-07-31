using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using APEntity = Sistran.Core.Application.AuthorizationPolicies.Entities;
using RULModel = Sistran.Core.Application.RulesScriptsServices.Models;

namespace Sistran.Core.Application.AuthorizationPoliciesServices.EEProvider.DAOs
{
    public class AuthorizationFunctionDao
    {
        public Models.AuthorizationFunction GetFunctionByIdFunction(TypeFunction typeFunction)
        {
            PrimaryKey key = APEntity.AuthorizationFunction.CreatePrimaryKey((int)typeFunction);
            var functionEntity = DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key) as APEntity.AuthorizationFunction;

            return new AuthorizationFunction
            {
                FunctionId = functionEntity.FunctionId,
                Method = functionEntity.Method,
                Package = new RULModel.Package{ PackageId = functionEntity.PackageId},
                Type = functionEntity.Type
            };
        }
    }
}
