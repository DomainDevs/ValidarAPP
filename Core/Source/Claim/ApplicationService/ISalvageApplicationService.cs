using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Application.ClaimServices.DTOs.Salvage;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.ClaimServices
{
    [ServiceContract]
    public interface ISalvageApplicationService
    {
        /// <summary>
        /// Consultar Salvamentos por "claimId" y "subClaimId"
        /// </summary>
        /// <param name="claimId"></param>
        /// <param name="subClaimId"></param>
        /// <returns>List<SalvageDTO></returns>
        [OperationContract]
        List<SalvageDTO> GetSalvagesByClaimIdSubClaimId(int claimId, int subClaimId);

        /// <summary>
        /// Consultar los salvamentos de una denuncia por criterios de búsuqeda de la denuncia
        /// </summary>
        /// <param name="prefixId"></param>
        /// <param name="branchId"></param>
        /// <param name="policyDocumentNumber"></param>
        /// <param name="claimNumber"></param>
        /// <returns></returns>
        [OperationContract]
        List<SalvageDTO> GetSalvagesByClaim(int? prefixId, int? branchId, string policyDocumentNumber, int claimNumber);

        /// <summary>
        /// Consultar los salvamentos por el reclamo
        /// </summary>
        /// <param name="claimId"></param>
        /// <returns></returns>
        [OperationContract]
        List<SalvageDTO> GetSalvagesByClaimId(int claimId);

        /// <summary>
        /// Consultar Salvamento por identificador
        /// </summary>
        /// <param name="salvageId"></param>
        /// <returns>List<SalvageDTO></returns>
        [OperationContract]
        SalvageDTO GetSalvageBySalvageId(int salvageId);

        /// <summary>
        /// Crea Salvamento
        /// </summary>
        /// <param name="salvage"></param>
        /// <returns></returns>
        [OperationContract]
        SalvageDTO CreateSalvage(SalvageDTO salvage);

        /// <summary>
        /// Actualiza Salvamento
        /// </summary>
        /// <param name="salvage"></param>
        /// <returns></returns>
        [OperationContract]
        SalvageDTO UpdateSalvage(SalvageDTO salvage);

        /// <summary>
        /// Crea Venta
        /// </summary>
        /// <param name="salvage"></param>
        /// <returns></returns>
        [OperationContract]
        SaleDTO CreateSale(SaleDTO sale, int salvageId);

        /// <summary>
        /// Actualiza Venta
        /// </summary>
        /// <param name="sale"></param>
        /// <param name="salvageId"></param>
        /// <returns></returns>
        [OperationContract]
        SaleDTO UpdateSale(SaleDTO sale, int salvageId);

        /// <summary>
        /// Elimina Salvamento
        /// </summary>
        /// <param name="salvageId"></param>
        [OperationContract]
        void DeleteSalvage(int salvageId);

        /// <summary>
        /// Consultar clases de pago
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SelectDTO> GetPaymentClasses();

        /// <summary>
        /// Colsulta las ventas por salmavento
        /// </summary>
        /// <param name="salvageId">ID salvamento</param>
        /// <returns>Listado de Sale</returns>
        [OperationContract]
        List<SaleDTO> GetSalesBySalvageId(int salvageId);

        /// <summary>
        /// Consulta los compradores por identificador, nombre y numero de documento
        /// </summary>
        /// <param name="description"></param>
        /// <param name="insuredSearchType"></param>
        /// <param name="customerType"></param>
        /// <returns></returns>
        [OperationContract]
        List<BuyerDTO> GetBuyersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType);

        /// <summary>
        /// Calcular las cuotas de un plan de pago
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="totalSale"></param>
        /// <param name="payments"></param>
        /// <param name="currencyDescription"></param>
        /// <returns></returns>
        [OperationContract]
        List<PaymentQuotaDTO> CalculateSaleAgreedPlan(DateTime dateStart, decimal totalSale, int payments, string currencyDescription);

        /// <summary>
        /// Obtener la venta por identificador de la venta
        /// </summary>
        /// <param name="saleId"></param>
        /// <returns></returns>
        [OperationContract]
        SaleDTO GetSaleBySaleId(int saleId);

        /// <summary>
        /// Obtener el número de salvamentos de una denuncia
        /// </summary>
        /// <param name="claimId"></param>
        /// <param name="subClaimId"></param>
        /// <returns></returns>
        [OperationContract]
        int GetSalvageNumberByClaimIdSubClaimId(int claimId, int subClaimId);
               
        /// <summary>
        /// Obtiene lo ramos que pueden tener salvamentos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<PrefixDTO> GetPrefixesSalvage();
    }
}
