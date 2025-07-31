using Newtonsoft.Json;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using Rules = Sistran.Core.Framework.Rules;
namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Business
{
    public class RiskBusiness
    {
        /// <summary>
        /// Runs the rules risk.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <param name="risk">The risk.</param>
        /// <param name="rulsetId">The rulset identifier.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public CompanyRisk RunRulesRisk(CompanyRisk risk, int rulsetId)
        {
            Rules.Facade facade = new Rules.Facade();
            try
            {
                if (risk != null && !risk.Policy.IsPersisted)
                {
                    PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(risk.Policy.Id);
                    if (pendingOperation != null)
                    {
                        risk.Policy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                    }
                    else
                    {
                        throw new ValidationException(Errors.ErrorFacadeGeneral);
                    }
                }
                EntityAssembler.CreateFacadeGeneral(risk.Policy, facade);
                EntityAssembler.CreatefacadeRisk(risk, facade);
                RulesEngineDelegate.ExecuteRules(rulsetId, facade);
                return ModelAssembler.CreateCiaRisk(risk, facade);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
