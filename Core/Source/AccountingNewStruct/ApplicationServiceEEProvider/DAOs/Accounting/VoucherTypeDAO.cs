//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using System.Collections.Generic;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    internal class VoucherTypeDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance variables

        #region Public Methods

        /// <summary>
        /// GetVoucherTypes
        /// </summary>
        /// <returns>List<VoucherType/></returns>
        public List<VoucherType> GetVoucherTypes()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(
                _dataFacadeManager.GetDataFacade().SelectObjects(typeof(PARAMEN.VoucherType)));

                // Return  como Lista
                return ModelAssembler.CreateVoucherTypes(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Public Methods
    }
}
