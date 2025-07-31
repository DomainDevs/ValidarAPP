using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using CLMEN = Sistran.Core.Application.Claims.Entities;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class PendingOperationsDAO
    {
        public PendingOperation CreatePendingOperation(PendingOperation pendingOperation)
        {
            CLMEN.PendingOperations entityPendingOperation = EntityAssembler.CreatePendingOperation(pendingOperation);
            DataFacadeManager.Insert(entityPendingOperation);

            return ModelAssembler.CreatePendingOperation(entityPendingOperation);
        }

        public PendingOperation GetPendingOperationByPendingOperationId(int pendingOperationId)
        {
            PrimaryKey key = CLMEN.PendingOperations.CreatePrimaryKey(pendingOperationId);
            CLMEN.PendingOperations entityPendingOperation = (CLMEN.PendingOperations)DataFacadeManager.GetObject(key);

            return ModelAssembler.CreatePendingOperation(entityPendingOperation);
        }

        public void DeletePendingOperationByPendingOperationId(int pendingOperationId)
        {
            PrimaryKey key = CLMEN.PendingOperations.CreatePrimaryKey(pendingOperationId);
            DataFacadeManager.Delete(key);
        }
    }
}
