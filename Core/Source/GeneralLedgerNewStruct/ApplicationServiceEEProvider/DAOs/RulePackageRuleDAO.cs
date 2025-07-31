//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

using System;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs
{
    internal class RulePackageRuleDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Instance variables

        #region Save

        /// <summary>
        /// SaveConceptParameterEntry
        /// </summary>
        /// <param name="accountingRulePackageId"></param>
        /// <param name="accountingRuleId"></param>
        public void SaveRulePackageRule(int accountingRulePackageId, int accountingRuleId)
        {
            try
            {
                // Convertir de model a entity
                GENERALLEDGEREN.RulePackageRule rulePackageRuleEntity = EntityAssembler.CreateRulePackageRule(accountingRulePackageId, accountingRuleId);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(rulePackageRuleEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Save

        #region Delete

        /// <summary>
        /// DeleteConceptParameterEntry
        /// </summary>
        /// <param name="rulePackageRuleId"></param>
        public void DeleteRulePackageRule(int rulePackageRuleId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = GENERALLEDGEREN.RulePackageRule.CreatePrimaryKey(rulePackageRuleId);

                // Realizar las operaciones con los entities utilizando DAF
                GENERALLEDGEREN.RulePackageRule rulePackageRuleEntity = (GENERALLEDGEREN.RulePackageRule)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(rulePackageRuleEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Delete
    }
}
