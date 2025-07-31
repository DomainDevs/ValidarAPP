//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

using System;
using COMMEN = Sistran.Core.Application.Common.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    internal class TempVoucherConceptDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance variables

        #region Public Methods

        /// <summary>
        /// DeleteTempVoucherConcept
        /// </summary>
        /// <param name="voucherConceptId"></param>
        public void DeleteTempVoucherConcept(int voucherConceptId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = COMMEN.TempVoucherConcept.CreatePrimaryKey(voucherConceptId);

                // Realizar las operaciones con los entities utilizando DAF
                COMMEN.TempVoucherConcept tempVoucherConceptEntity = (COMMEN.TempVoucherConcept)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(tempVoucherConceptEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Public Methods
    }
}
