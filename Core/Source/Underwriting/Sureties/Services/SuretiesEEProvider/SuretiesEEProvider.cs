using Sistran.Core.Application.Sureties;
using Sistran.Core.Application.Sureties.Models;
using Sistran.Core.Application.SuretiesEEProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using UNDTO = Sistran.Core.Application.UnderwritingServices.DTOs;
using UniquePersonModel = Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Application.SuretiesEEProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.Sureties.ISuretiesCore" />
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class SuretiesEEProvider : ISuretiesCore
    {
        /// <summary>
        /// Obtener  las contragarantias que han sido asignadas
        /// </summary>
        /// <param name="guarantees">Listado de contragarantias</param>
        /// <returns>Lista de contragarantías cumplimiento</returns>
        public List<RiskSuretyGuarantee> GetRiskSuretyGuaranteesByGuarantees(List<IssuanceGuarantee> guarantees)
        {
            try
            {
                RiskSuretyGuaranteeDAO riskSuretyGuaranteeProvider = new RiskSuretyGuaranteeDAO();
                return riskSuretyGuaranteeProvider.GetRiskSuretyGuaranteesByGuarantees(guarantees);                
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Consulta los datos basicos de las polizas que esten asociadas al afianzado
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns>poliza con datos basicos</returns>
        public List<UNDTO.PolicyRiskDTO> GetPolicyRiskDTOsByIndividualId(int individualId)
        {
            try
            {
                SuretyDAO suretyDAO = new SuretyDAO();
                return suretyDAO.GetPolicyRiskDTOsByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public List<IssuanceInsuredGuarantee> GetRiskSuretyGuaranteesByRiskId(int riskId)
        {
            try
            {
                RiskSuretyGuaranteeDAO riskSuretyGuaranteeProvider = new RiskSuretyGuaranteeDAO();
                return riskSuretyGuaranteeProvider.GetRiskSuretyGuaranteesByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
