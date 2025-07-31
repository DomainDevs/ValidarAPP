using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonServices.EEProvider.Assemblers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMEN =Sistran.Core.Application.Common.Entities;

namespace Sistran.Company.Application.CommonServices.EEProvider.DAOs
{
    public class AccountTypeDAO
    {
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        /// <summary>
        /// GetAccountTypes
        /// </summary>
        /// <returns>List<BankAccountType/></returns>
        public List<AccountType> GetBankAccountTypes()
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
