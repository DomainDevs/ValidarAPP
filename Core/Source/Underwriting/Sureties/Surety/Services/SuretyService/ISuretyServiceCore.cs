using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.Sureties.SuretyServices.Models;

namespace Sistran.Core.Application.Sureties.SuretyServices
{
    [ServiceContract]
    public interface ISuretyServiceCore : ISuretiesCore
    {
        /// <summary>
        /// Obtiene el Cupo Operativo y el Cumulo
        ///     <para>&#160;</para>
        ///     <para>
        ///         Valores Salida
        ///     </para>
        ///     <para>&#160;</para>
        ///     <para>Decimal  Cupo Operativo</para>       
        ///     <para>Decimal Cumulo</para>
        /// </summary>
        /// para
        ///<code>
        ///AggregateDAO aggregateDAO = new AggregateDAO();
        ///</code>      
        /// <param name="individualId">Individual Id Asegurado</param>
        /// <param name="currencyCode">Moneda</param>
        /// <param name="lineBusinessCode">Linea del Negocio</param>
        /// <param name="issueDate">Fecha Hasta</param>       
        /// <returns>Lista de sumas</returns>       
        [OperationContract]
        List<Amount> GetAvailableAmountByIndividualId(int individualId, int PrefixCd, DateTime issueDate);

        [OperationContract]
        List<Contract> GetSuretiesByEndorsementIdModuleType(int endorsementId, Services.UtilitiesServices.Enums.ModuleType moduleType);

        [OperationContract]
        List<Contract> GetRisksSuretyByInsuredId(int insuredId);
        [OperationContract]
        List<Contract> GetRisksSuretyBySuretyId(int suretyId);
        [OperationContract]
        Contract GetSuretyByRiskIdModuleType(int riskId, Services.UtilitiesServices.Enums.ModuleType moduleType);

        [OperationContract]
        List<Contract> GetRisksBySurety(string description);
    }

}
