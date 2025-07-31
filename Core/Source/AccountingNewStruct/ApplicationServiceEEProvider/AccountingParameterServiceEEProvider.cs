using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Framework.Views;

//Sistran Core
using Sistran.Core.Application.CommonService;
using SEARCH = Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.AccountingServices.DTOs.CancellationPolicies;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using AccountingModels = Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.AccountingServices.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.AccountingServices.EEProvider
{
    public class AccountingParameterServiceEEProvider : IAccountingParameterService
    {
        #region Constants

        // Variable para lectura de UIViews
        private int Rows = 50;

        #endregion

        #region Instance Variables

        #region Interfaz

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        #endregion Interfaz

        #region DAOs

        readonly CollectConceptDAO _collectConceptDAO = new CollectConceptDAO();
        readonly CollectPaymentMethodTypeDAO _collectPaymentMethodTypeDAO = new CollectPaymentMethodTypeDAO();
        readonly RejectionDAO _rejectionDAO = new RejectionDAO();
        readonly ActionTypeDAO _actionTypeDAO = new ActionTypeDAO();
        readonly CurrencyDifferenceDAO _currencyDifferenceDAO = new CurrencyDifferenceDAO();
        readonly RangeDAO _rangeDAO = new RangeDAO();
        readonly CancellationLimitDAO _cancellationLimitDAO = new CancellationLimitDAO();
        readonly ExclusionDAO _exclusionDAO = new ExclusionDAO();
        readonly BankAccountTypeDAO _bankAccountTypeDAO = new BankAccountTypeDAO();
        readonly BankAccountPersonDAO _bankAccountPersonDAO = new BankAccountPersonDAO();
        readonly BankAccountCompanyDAO _bankAccountCompanyDAO = new BankAccountCompanyDAO();

        #endregion DAOs

        #endregion

        #region Public Methods

        #region PaymentMethodType

        /// <summary>
        /// SaveCollectPaymentMethodType 
        /// Se guarda los conceptos de facturación
        /// </summary>
        /// <param name="id"></param>
        /// <param name="methodType"></param>
        /// <param name="enabledTicket"></param>
        /// <param name="collectEnabled"></param>
        /// <param name="enabledPaymentOrder"></param>
        /// <param name="enabledPaymentRequest"></param>
        /// <returns>int</returns>
        public int SaveCollectPaymentMethodType(int id, int methodType, int enabledTicket, int collectEnabled,
                                             int enabledPaymentOrder, int enabledPaymentRequest)
        {
            try
            {
                return _collectPaymentMethodTypeDAO.SaveCollectPaymentMethodType(id, methodType, enabledTicket,
                                                       collectEnabled, enabledPaymentOrder, enabledPaymentRequest);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateCollectPaymentMethodType 
        /// Se actualiza los conceptos de facturación
        /// </summary>
        /// <param name="id"></param>
        /// <param name="methodType"></param>
        /// <param name="enabledTicket"></param>
        /// <param name="collectEnabled"></param>
        /// <param name="enabledPaymentOrder"></param>
        /// <param name="enabledPaymentRequest"></param>
        /// <returns>int</returns>
        public int UpdateCollectPaymentMethodType(int id, int methodType, int? enabledTicket, int? collectEnabled,
            int? enabledPaymentOrder, int? enabledPaymentRequest)
        {
            try
            {
                return _collectPaymentMethodTypeDAO.UpdateCollectPaymentMethodType(id, methodType, enabledTicket,
                                                     collectEnabled, enabledPaymentOrder, enabledPaymentRequest);

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// DeleteCollectPaymentMethodType 
        /// Se elimina los conceptos de facturación
        /// </summary>
        /// <param name="id"></param>
        public void DeleteCollectPaymentMethodType(int id)
        {
            try
            {
                _collectPaymentMethodTypeDAO.DeleteCollectPaymentMethodType(id);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPaymentMethodType
        /// Devuelve una lista de métodos de pago
        /// </summary>
        /// <returns>List<PaymentMethodTypeDTO/></returns>
        public List<SEARCH.PaymentMethodTypeDTO> GetPaymentMethodType()
        {
            try
            {
                List<SEARCH.PaymentMethodTypeDTO> paymentMethodTypeDTOs = new List<SEARCH.PaymentMethodTypeDTO>();

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.PaymentType.Properties.PaymentTypeCode);
                criteriaBuilder.Distinct();
                criteriaBuilder.Constant(0);

                BusinessCollection paymentMethodTypes = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.PaymentType),
                                        criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.PaymentType entityPaymentType in paymentMethodTypes)
                {
                    paymentMethodTypeDTOs.Add(new SEARCH.PaymentMethodTypeDTO()
                    {
                        PaymentTypeCode = entityPaymentType.PaymentTypeCode,
                        Description = entityPaymentType.Description,
                        EnabledTicket = entityPaymentType.EnabledTicket,
                        EnabledPaymentOrder = entityPaymentType.EnabledPaymentOrder,
                        CollectEnabled = entityPaymentType.EnabledBilling,
                        EnabledPaymentRequest = entityPaymentType.EnabledPaymentRequest
                    });
                }

                return paymentMethodTypeDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetEnablePaymentMethodType
        /// Trae métodos de pago habilitados
        /// </summary>
        /// <param name="enabledPaymentRequest"></param>
        /// <param name="enabledPaymentOrder"></param>
        /// <param name="enabledTicket"></param>
        /// <returns>List<PaymentMethodTypeDTO/></returns>
        public List<SEARCH.PaymentMethodTypeDTO> GetEnablePaymentMethodType(bool enabledPaymentRequest, bool enabledPaymentOrder, bool enabledTicket)
        {
            try
            {
                List<SEARCH.PaymentMethodTypeDTO> paymentMethodTypeDTOs = new List<SEARCH.PaymentMethodTypeDTO>();

                // Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                // Boletas de depósito
                if (enabledTicket)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentType.Properties.EnabledTicket, 1).And();
                }

                // Órdenes de Pago
                if (enabledPaymentOrder)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentType.Properties.EnabledPaymentOrder, 1).And();
                }

                // Solicitud de Pagos
                if (enabledPaymentRequest)
                {
                    criteriaBuilder.PropertyEquals(ACCOUNTINGEN.PaymentType.Properties.EnabledPaymentRequest, 1).And();
                }

                criteriaBuilder.Property(ACCOUNTINGEN.PaymentType.Properties.PaymentTypeCode);
                criteriaBuilder.Distinct();
                criteriaBuilder.Constant(0);

                BusinessCollection paymentMethodTypes = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.PaymentType),
                                        criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.PaymentType entityPaymentType in paymentMethodTypes)
                {
                    paymentMethodTypeDTOs.Add(new SEARCH.PaymentMethodTypeDTO()
                    {
                        PaymentTypeCode = entityPaymentType.PaymentTypeCode,
                        Description = entityPaymentType.Description,
                        EnabledTicket = entityPaymentType.EnabledTicket,
                        EnabledPaymentOrder = entityPaymentType.EnabledPaymentOrder,
                        CollectEnabled = entityPaymentType.EnabledBilling,
                        EnabledPaymentRequest = entityPaymentType.EnabledPaymentRequest
                    });
                }

                return paymentMethodTypeDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region CollectConcept

        /// <summary>
        /// SaveCollectConcept
        /// Se guarda los conceptos de facturación
        /// </summary>
        /// <param name="collectConcept"></param>
        /// <returns>CollectConcept</returns>
        public CollectConceptDTO SaveCollectConcept(CollectConceptDTO collectConcept)
        {
            try
            {
                return _collectConceptDAO.SaveCollectConcept(collectConcept.ToModel()).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateCollectConcept
        /// Se guarda los conceptos de facturación
        /// </summary>
        /// <param name="collectConcept"></param>
        /// <returns>CollectConcept</returns>
        public CollectConceptDTO UpdateCollectConcept(CollectConceptDTO collectConcept)
        {
            try
            {
                return _collectConceptDAO.UpdateCollectConcept(collectConcept.ToModel()).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteCollectConcept
        /// Se elimina los conceptos de facturación
        /// </summary>
        /// <param name="collectConcept"></param>
        public void DeleteCollectConcept(CollectConceptDTO collectConcept)
        {
            try
            {
                _collectConceptDAO.DeleteCollectConcept(collectConcept.ToModel());
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCollectConcepts
        /// Se obtiene un concepto de facturación
        /// </summary>
        /// <returns>List<CollectConcept></returns>
        public List<CollectConceptDTO> GetCollectConcepts()
        {
            try
            {
                return _collectConceptDAO.GetCollectConcepts().ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region ItemType

        /// <summary>
        /// GetItemTypeByDescription
        /// Obtiene tipos de ítem
        /// </summary>
        /// <param name="description"></param>
        /// <returns>int</returns>
        public static int GetItemTypeByDescription(string description)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.ItemType.Properties.Description);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(description);

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects
                    (typeof(ACCOUNTINGEN.ItemType), criteriaBuilder.GetPredicate()));

                int itemTypeId = 0;

                if (businessCollection.Count > 0)
                {
                    itemTypeId = Convert.ToInt32(businessCollection.OfType<ACCOUNTINGEN.ItemType>().First().ItemTypeCode);
                }

                return itemTypeId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Rejection

        /// <summary>
        /// GetRejections
        /// </summary>
        /// <returns>List<Rejection/></returns>
        public List<RejectionDTO> GetRejections()
        {
            try
            {
                return _rejectionDAO.GetRejections().ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region ActionType

        ///<summary>
        /// SaveActionType
        /// </summary>
        /// <param name="actionType"></param>        
        /// <returns>ActionType</returns>
        public ActionTypeDTO SaveActionType(ActionTypeDTO actionType)
        {
            try
            {
                return _actionTypeDAO.SaveActionType(actionType.ToModel()).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateActionType
        /// </summary>
        /// <param name="actionType"></param>        
        /// <returns>ActionType</returns>
        public ActionTypeDTO UpdateActionType(ActionTypeDTO actionType)
        {
            try
            {
                return _actionTypeDAO.UpdateActionType(actionType.ToModel()).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteActionType
        /// </summary>
        /// <param name="actionType"> </param>
        public void DeleteActionType(ActionTypeDTO actionType)
        {
            try
            {
                _actionTypeDAO.DeleteActionType(actionType.ToModel());
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetActionTypes
        /// </summary>
        /// <returns>List<ActionType/></returns>
        public List<ActionTypeDTO> GetActionTypes()
        {
            try
            {
                return _actionTypeDAO.GetActionTypes().ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region CurrencyDifference

        ///<summary>
        /// SaveCurrencyDifference
        /// </summary>
        ///<param name="currencyCode"> </param>
        ///<param name="maxDifference"> </param>
        ///<param name="percentageDifference"> </param>
        ///<returns>int</returns>
        public int SaveCurrencyDifference(int currencyCode, decimal maxDifference, decimal percentageDifference)
        {
            try
            {
                return _currencyDifferenceDAO.SaveCurrencyDifference(currencyCode, maxDifference, percentageDifference);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// UpdateCurrencyDifference
        /// </summary>
        ///<param name="currencyCode"> </param>
        ///<param name="maxDifference"> </param>
        ///<param name="percentageDifference"> </param>
        ///<returns>int</returns>
        public int UpdateCurrencyDifference(int currencyCode, decimal maxDifference, decimal percentageDifference)
        {
            try
            {
                return _currencyDifferenceDAO.UpdateCurrencyDifference(currencyCode, maxDifference, percentageDifference);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// DeleteCurrencyDifference
        /// </summary>
        ///<param name="currencyCode"> </param>
        ///<returns>bool</returns>
        public bool DeleteCurrencyDifference(int currencyCode)
        {

            try
            {
                return _currencyDifferenceDAO.DeleteCurrencyDifference(currencyCode);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// GetCurrencyDifferences
        /// </summary>
        ///<returns> List<CurrencyDifferenceDTO/></returns>
        public List<SEARCH.CurrencyDifferenceDTO> GetCurrencyDifferences()
        {
            try
            {
                UIView currencies = _dataFacadeManager.GetDataFacade().GetView("CurrencyDifferencesView", null, null,
                                                                         0, -1, null, true, out Rows);

                if (currencies.Rows.Count > 0)
                {
                    currencies.Columns.Add("rows", typeof(int));
                    currencies.Rows[0]["rows"] = Rows;
                }

                // Load DTO
                List<SEARCH.CurrencyDifferenceDTO> currencyDifferenceDTOs = new List<SEARCH.CurrencyDifferenceDTO>();

                foreach (DataRow dataRow in currencies)
                {
                    currencyDifferenceDTOs.Add(new SEARCH.CurrencyDifferenceDTO()
                    {
                        CurrencyCode = Convert.ToInt32(dataRow["CurrencyCode"]),
                        Description = Convert.ToString(dataRow["Description"]),
                        MaximumDifference = Convert.ToDecimal((dataRow["MaximumDifference"]) is DBNull ? 0 : dataRow["MaximumDifference"]),
                        PercentageDifference = Convert.ToDecimal(dataRow["PercentageDifference"])
                    });
                }

                return currencyDifferenceDTOs;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region AccountingDate

        /// <summary>
        /// GetAccountingDate
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>DateTime></returns>
        public DateTime GetAccountingDate(int moduleId)
        {
            try
            {
                return DelegateService.commonService.GetModuleDateIssue(moduleId, DateTime.Now);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Range

        /// <summary>
        /// SaveRange
        /// </summary>
        /// <param name="range"></param>
        /// <returns>Range</returns>
        public RangeDTO SaveRange(RangeDTO range)
        {
            try
            {
                return _rangeDAO.SaveRange(range.ToModel()).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteRange
        /// </summary>
        /// <param name="range"></param>
        /// <returns>bool</returns>
        public bool DeleteRange(RangeDTO range)
        {
            try
            {

                bool isDeleted = false;

                RangeDTO newRange;
                newRange = _rangeDAO.GetRange(range.ToModel()).ToDTO();

                if (newRange.RangeItems.Count == 0)
                {
                    isDeleted = _rangeDAO.DeleteRange(range.Id);
                }

                return isDeleted;

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// UpdateRange
        /// </summary>
        /// <param name="range"></param>
        /// <returns>Range</returns>
        public RangeDTO UpdateRange(RangeDTO range)
        {
            try
            {
                return _rangeDAO.UpdateRange(range.ToModel()).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// GetRanges
        /// </summary>
        /// <returns>List<Range/></returns>
        public List<RangeDTO> GetRanges()
        {
            try
            {
                return _rangeDAO.GetRanges().ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetRange
        /// </summary>
        /// <param name="range"></param>
        /// <returns>Range</returns>
        public RangeDTO GetRange(RangeDTO range)
        {
            try
            {
                return _rangeDAO.GetRange(range.ToModel()).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// DeleteRangeItem
        /// </summary>
        /// <param name="rangeItem"></param>
        /// <returns>bool</returns>
        public bool DeleteRangeItem(RangeItemDTO rangeItem)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.RangeItem.CreatePrimaryKey(rangeItem.Id, rangeItem.Order);
                ACCOUNTINGEN.RangeItem rangeDetailDeleteEntity = (ACCOUNTINGEN.RangeItem)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
                if (rangeDetailDeleteEntity != null)
                {
                    _dataFacadeManager.GetDataFacade().DeleteObject(rangeDetailDeleteEntity);
                }
            }
            catch (BusinessException)
            {
                return false;
            }

            return true;
        }

        #endregion Range

        #region CancellationPolicies

        #region CancellationLimit

        /// <summary>
        /// SaveCancellationLimit
        /// </summary>
        /// <param name="cancellationLimit"></param>
        /// <returns>CancellationLimit</returns>
        public CancellationLimitDTO SaveCancellationLimit(CancellationLimitDTO cancellationLimit)
        {
            try
            {
                return _cancellationLimitDAO.SaveCancellationLimit(cancellationLimit.ToModel()).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateCancellationLimit
        /// </summary>
        /// <param name="cancellationLimit"></param>
        /// <returns>CancellationLimit</returns>
        public CancellationLimitDTO UpdateCancellationLimit(CancellationLimitDTO cancellationLimit)
        {
            try
            {
                return _cancellationLimitDAO.UpdateCancellationLimit(cancellationLimit.ToModel()).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteCancellationLimit
        /// </summary>
        /// <param name="cancellationLimit"></param>
        public void DeleteCancellationLimit(CancellationLimitDTO cancellationLimit)
        {
            try
            {
                _cancellationLimitDAO.DeleteCancellationLimit(cancellationLimit.ToModel());
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// GetGetCancellationLimits
        /// </summary>
        /// <returns>List<CancellationLimit/></returns>
        public List<CancellationLimitDTO> GetCancellationLimits()
        {
            try
            {
                return _cancellationLimitDAO.GetCancellationLimits().ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Exclusion

        /// <summary>
        /// SaveExclusion
        /// </summary>
        /// <param name="exclusion"></param>
        /// <returns>Exclusion</returns>
        public ExclusionDTO SaveExclusion(ExclusionDTO exclusion)
        {
            try
            {
                return _exclusionDAO.SaveExclusion(exclusion.ToModel()).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteExclusion
        /// </summary>
        /// <param name="exclusion"></param>
        /// <returns></returns>
        public void DeleteExclusion(ExclusionDTO exclusion)
        {
            try
            {
                _exclusionDAO.DeleteExclusion(exclusion.ToModel());
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetExclusions
        /// </summary>
        /// <returns>List<Exclusion/></returns>
        public List<ExclusionDTO> GetExclusions(int exclusionType)
        {
            try
            {
                return _exclusionDAO.GetExclusions((ExclusionTypes)exclusionType).ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #endregion

        #region BankAccountType

        /// <summary>
        /// GetBankAccountTypes
        /// </summary>
        /// <returns>List<BankAccountType/></returns>
        public List<BankAccountTypeDTO> GetBankAccountTypes()
        {
            try
            {
                return _bankAccountTypeDAO.GetBankAccountTypes().ToDTOs().ToList();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion

        #region BankAccountCompany

        /// <summary>
        /// SaveBankAccountCompany
        /// <param name="bankAccountCompany"></param>
        /// </summary>
        /// <returns>BankAccountCompany</returns>
        public BankAccountCompanyDTO SaveBankAccountCompany(BankAccountCompanyDTO bankAccountCompany)
        {
            try
            {

                BankAccountCompanyDTO newBankAccountCompany = new BankAccountCompanyDTO();
                newBankAccountCompany.Id = _bankAccountCompanyDAO.SaveBankAccountCompany(bankAccountCompany.ToModel());

                return newBankAccountCompany;

            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

        }

        /// <summary>
        /// UpdateBankAccountCompany
        /// <param name="bankAccountCompany"></param>
        /// </summary>
        /// <returns>BankAccountCompany</returns>
        public BankAccountCompanyDTO UpdateBankAccountCompany(BankAccountCompanyDTO bankAccountCompany)
        {
            try
            {

                BankAccountCompanyDTO newBankAccountCompany = new BankAccountCompanyDTO();
                newBankAccountCompany.Id = _bankAccountCompanyDAO.UpdateBankAccountCompany(bankAccountCompany.ToModel());

                return newBankAccountCompany;

            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetBankAccountCompany
        /// <param name="bankAccountCompany"></param>
        /// </summary>
        /// <returns>BankAccountCompany</returns>
        public BankAccountCompanyDTO GetBankAccountCompany(BankAccountCompanyDTO bankAccountCompany)
        {
            try
            {
                return _bankAccountCompanyDAO.GetBankAccountCompany(bankAccountCompany.ToModel()).ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetBankAccountCompanies
        /// </summary>
        /// <returns>List<BankAccountCompany/></returns>
        public List<BankAccountCompanyDTO> GetBankAccountCompanies()
        {
            try
            {
                return _bankAccountCompanyDAO.GetBankAccountCompanies().ToDTOs().ToList();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetBankAccountCompanies
        /// </summary>
        /// <returns>List<BankAccountCompany/></returns>
        public List<BankAccountCompanyDTO> GetBankAccountCompaniesByCurrencyCode(int currencyCode)
        {
            try
            {
                return _bankAccountCompanyDAO.GetBankAccountCompaniesByCurrencyCode(currencyCode).ToDTOs().ToList();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// DeleteBankAccountCompany
        /// </summary>
        /// <returns></returns>
        public void DeleteBankAccountCompany(BankAccountCompanyDTO bankAccountCompany)
        {
            try
            {
                _bankAccountCompanyDAO.DeleteBankAccountCompany(bankAccountCompany.ToModel());
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion

        #region BankAccountPerson

        /// <summary>
        /// SaveBankAccountPerson
        /// <param name="bankAccountPerson"></param>
        /// </summary>
        /// <returns>BankAccountPerson</returns>
        public BankAccountPersonDTO SaveBankAccountPerson(BankAccountPersonDTO bankAccountPerson)
        {
            try
            {
                return _bankAccountPersonDAO.SaveBankAccountPerson(bankAccountPerson.ToModel()).ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// UpdateBankAccountPerson
        /// <param name="bankAccountPerson"></param>
        /// </summary>
        /// <returns>BankAccountPerson</returns>
        public BankAccountPersonDTO UpdateBankAccountPerson(BankAccountPersonDTO bankAccountPerson)
        {
            try
            {
                return _bankAccountPersonDAO.UpdateBankAccountPerson(bankAccountPerson.ToModel()).ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// DeleteBankAccountPerson
        /// <param name="bankAccountPerson"></param>
        /// </summary>
        /// <returns></returns>
        public void DeleteBankAccountPerson(BankAccountPersonDTO bankAccountPerson)
        {
            try
            {
                _bankAccountPersonDAO.DeleteBankAccountPerson(bankAccountPerson.ToModel());
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetBankAccountPerson
        /// </summary>
        /// <returns>BankAccountPerson</returns>
        public BankAccountPersonDTO GetBankAccountPerson(BankAccountPersonDTO bankAccountPerson)
        {
            try
            {
                return _bankAccountPersonDAO.GetBankAccountPerson(bankAccountPerson.ToModel()).ToDTO();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetBankAccountPersons
        /// </summary>
        /// <returns>List<BankAccountPerson/></returns>
        public List<BankAccountPersonDTO> GetBankAccountPersons()
        {
            try
            {
                return _bankAccountPersonDAO.GetBankAccountPersons().ToDTOs().ToList();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion

        #region PaymentMethod

        /// <summary>
        /// GetPaymentMethods
        /// </summary>
        /// <returns>List<PaymentMethod/></returns>
        public List<AccountingModels.PaymentMethodDTO> GetPaymentMethods()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(
                    _dataFacadeManager.GetDataFacade().SelectObjects(typeof(PARAMEN.PaymentMethodType)));

                // Return como lista
                return ModelAssembler.CreatePaymentMethodTypes(businessCollection).ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region CreditCardType

        /// <summary>
        /// GetCreditCardType
        /// </summary>
        /// <param name="CreditCardTypeId"></param>
        /// <returns>CreditCardType</returns>
        public CreditCardTypeDTO GetCreditCardType(int CreditCardTypeId)
        {
            try
            {
                //Crea la Primary key con el codigo de la entidad
                PrimaryKey primaryKey = COMMEN.CreditCardType.CreatePrimaryKey(CreditCardTypeId);

                //realizar las operaciones con los entities utilizando DAF
                COMMEN.CreditCardType creditCardTypeEntity = (COMMEN.CreditCardType)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                //return del model
                return ModelAssembler.CreateCreditCardType(creditCardTypeEntity).ToDTO();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetCreditCardTypes
        /// </summary>
        /// <returns>List<CreditCardType/></returns>
        public List<CreditCardTypeDTO> GetCreditCardTypes()
        {
            try
            {
                //Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(COMMEN.CreditCardType)));

                //return  como Lista
                return ModelAssembler.CreateCreditCardTypes(businessCollection).ToDTOs().ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #endregion Public Methods

        #region Private Methods

        #region AccountingDate

        /// <summary>
        /// GetFirstAndLastDayOfMonth
        /// </summary>
        /// <param name="currentDate"></param>
        /// <param name="lastClosingMonth"></param>
        /// <param name="lastClosingYear"></param>
        /// <returns>DateTime</returns>
        private DateTime GetFirstAndLastDayOfMonth(DateTime currentDate, int lastClosingMonth, int lastClosingYear)
        {
            DateTime operationDate = new DateTime();
            DateTime closeFirstDay;
            DateTime closeLastDay;

            DateTime closeDate = new DateTime(lastClosingYear, lastClosingMonth, 15);
            closeDate = closeDate.AddMonths(1);
            closeFirstDay = closeDate.AddDays(-(closeDate.Day - 1));
            closeDate = closeDate.AddMonths(1);
            closeLastDay = closeDate.AddDays(-(closeDate.Day));

            // Si la fecha actual esta entre el mes del cierre, se retorna la fecha actual
            if (currentDate >= closeFirstDay && currentDate <= closeLastDay)
            {
                operationDate = currentDate;
            }
            else
            {
                // Si la fecha atual es menor a la fecha del primer día del mes del cierre
                if (currentDate < closeFirstDay)
                {
                    operationDate = closeFirstDay;
                }
                // Si la fecha atual es mayor a la fecha del último día del mes del cierre
                if (currentDate > closeLastDay)
                {
                    operationDate = closeLastDay;
                }
            }

            return operationDate;
        }



        #endregion AccountingDate

        #endregion Private Methods

    }
}
