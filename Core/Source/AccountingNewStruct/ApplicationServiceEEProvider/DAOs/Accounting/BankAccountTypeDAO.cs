//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.BankAccounts;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using System.Collections.Generic;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    internal class BankAccountTypeDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// GetBankAccountTypes
        /// </summary>
        /// <returns>List<BankAccountType/></returns>
        public List<BankAccountType> GetBankAccountTypes()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(
                    _dataFacadeManager.GetDataFacade().SelectObjects(typeof(
                    COMMEN.AccountType)));

                // Return  como Lista
                return ModelAssembler.CreateAccountTypes(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
