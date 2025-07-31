using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.Transactions;
using System;
using INTEN = Sistran.Core.Application.Integration.Entities;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs.Integration2G
{
    public class CollectApplicationControlDAO
    {
        public int Insert(Models.Integration2G.CollectApplicationControl collectApplicationControl)
        {
            // Convertir de model a entity
            INTEN.CollectApplicationControl entityIntegration = EntityAssembler.CreateCollectApplicationControl(collectApplicationControl);

            // Realizar las operaciones con los entities utilizando DAF
            DataFacadeManager.Insert(entityIntegration);

            // Return del model
            return entityIntegration.CollectApplicationControlId;
        }

        public int InsertOnce(Models.Integration2G.CollectApplicationControl collectApplicationControl)
        {
            // Convertir de model a entity
            INTEN.CollectApplicationControl entityIntegration = EntityAssembler.CreateCollectApplicationControl(collectApplicationControl);

            using (Context.Current)
            {
                using (Transaction transaction = new Transaction())
                {
                    try
                    {
                        // Realizar las operaciones con los entities utilizando DAF
                        DataFacadeManager.Insert(entityIntegration);
                        transaction.Complete();

                        // Return del model
                        return entityIntegration.CollectApplicationControlId;
                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        return 0;
                    }
                }
            }
        }
    }
}
