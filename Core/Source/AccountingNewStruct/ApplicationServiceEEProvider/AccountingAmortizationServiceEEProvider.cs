using Sistran.Core.Application.AccountingServices.Assemblers;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Amortizations;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Application.AccountingServices.EEProvider
{
    public class AccountingAmortizationServiceEEProvider : IAccountingAmortizationService
    {
        #region Instance Viarables

        #region Interfaz

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Interfaz

        #region DAOs

        private readonly AmortizationDAO _amortizationDAO = new AmortizationDAO();

        #endregion DAOs

        #endregion Instance Viarables

        #region Public Methods

        #region Amortization

        /// <summary>
        /// GenerateAmortization
        /// Genera el proceso de Amortizacion
        /// </summary>
        /// <param name="operationType"></param>
        /// <param name="branch"></param>
        /// <param name="prefix"></param>
        /// <param name="policy"></param>
        /// <param name="insured"></param>
        /// <param name="amount"></param>
        /// <returns>Amortization</returns>
        public AmortizationDTO GenerateAmortization(int operationType, BranchDTO branch, PrefixDTO prefix, PolicyDTO policy, IndividualDTO insured, decimal amount)
        {
            try
            {
                return _amortizationDAO.GenerateAmortization(operationType, branch.ToModel(), prefix.ToModel(), policy.ToModel(), insured.ToModel(), amount).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// UpdateAmortization
        /// Actualiza Amortizacion: elimina o aplica items
        /// </summary>
        /// <param name="amortization"></param>
        /// <returns>Amortization</returns>
        public AmortizationDTO UpdateAmortization(AmortizationDTO amortization)
        {
            try
            {
                return _amortizationDAO.UpdateAmortization(amortization.ToModel()).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAmortizations: Obtener todas las Amortizaciones
        /// </summary>        
        /// <returns>List<Amortization/></returns>
        public List<AmortizationDTO> GetAmortizations()
        {
            try
            {
                return _amortizationDAO.GetAmortizations().ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// ApplyAmortization
        /// Aplicar todas las Amortizaciones
        /// </summary>        
        /// <returns><Amortization/></returns>
        public AmortizationDTO ApplyAmortization(AmortizationDTO amortization)
        {
            try
            {
                return _amortizationDAO.ApplyAmortization(amortization.ToModel()).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAmortizationById
        /// Obtener Amortizacion por id
        /// </summary>
        /// <param name="amortizationId"></param>
        /// <returns>Amortization</returns>
        public AmortizationDTO GetAmortizationById(int amortizationId)
        {
            try
            {
                return _amortizationDAO.GetAmortizationById(amortizationId).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Amortization

        #endregion Public Methods
    }
}

