using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using INTEN = Sistran.Core.Application.Integration.Entities;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Integration2G
{
    internal class CollectApplicationControlDAO
    {
        public Models.Integration2G.CollectApplicationControl Insert(Models.Integration2G.CollectApplicationControl collectApplicationControl)
        {
            // Convertir de model a entity
            INTEN.CollectApplicationControl entityIntegration= EntityAssembler.CreateCollectApplicationControl(collectApplicationControl);

            // Realizar las operaciones con los entities utilizando DAF
            DataFacadeManager.Insert(entityIntegration);

            // Return del model
            return ModelAssembler.CreateCollectApplicationControl(entityIntegration);
        }
    }
}
