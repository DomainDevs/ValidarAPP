using System;
using System.Collections.Generic;
using Sistran.Core.Application.Transports.TransportBusinessService.Models;
using Sistran.Core.Application.Transports.TransportBusinessService.Enum;
using Sistran.Core.Application.Transports.TransportBusinessService.EEProvider.Assemblers;
using Sistran.Core.Application.Transports.TransportBusinessService.EEProvider.Resources;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.Managers;
using COMMEN = Sistran.Core.Application.Common.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.Transports.TransportBusinessService.EEProvider.View;
using Sistran.Core.Framework.DAF.Engine;
using System.Linq;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Application.Transports.TransportBusinessService.EEProvider
{
    public class TransportBusinessServiceProvider : ITransportBusinessService
    {
        #region Transport
        /// <summary>
        /// Obtiener la lista de tipos de mercancias
        /// </summary>
        /// <returns>Lista de Mercancias</returns>
        public List<CargoType> GetCargoTypes()
        {
            try
            {
                var result = ModelAssembler.CreateCargoTypes(DataFacadeManager.GetObjects(typeof(COMMEN.TransportCargoType)));
                DataFacadeManager.Dispose();
                return result;
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCargoTypes), ex);
            }
        }

        /// <summary>
        /// Obtiener la lista de tipos de mercancias
        /// </summary>
        /// <returns>Lista de Mercancias</returns>
        public List<TransportType> GetTransportTypes()
        {
            try
            {
                var result = ModelAssembler.CreateTransportTypes(DataFacadeManager.GetObjects(typeof(COMMEN.TransportMean)));
                DataFacadeManager.Dispose();
                return result;
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetTransportTypes), ex);
            }
        }

        /// <summary>
        /// Obtiener la lista de tipos de empaques
        /// </summary>
        /// <returns>Lista de Empaques</returns>
        public List<Models.PackagingType> GetPackagingTypes()
        {
            try
            {
                var result = ModelAssembler.CreatePackagingTypes(DataFacadeManager.GetObjects(typeof(COMMEN.TransportPackagingType)));
                DataFacadeManager.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetPackagingTypes), ex);
            }
        }

        /// <summary>
        /// Obtiener la lista de Periodos de Declaración
        /// </summary>
        /// <returns>Lista de periodos de declaracion </returns>
        public List<DeclarationPeriod> GetDeclarationPeriods()
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(PARAMEN.DeclarationPeriod.Properties.IsEnabled, typeof(PARAMEN.DeclarationPeriod).Name);
                filter.Equal();
                filter.Constant(Status.Enabled);

                BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(PARAMEN.DeclarationPeriod), filter.GetPredicate());
                DataFacadeManager.Dispose();
                return ModelAssembler.CreateDeclarationPeriodTypes(businessCollection);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorDeclarationPeriodTypes), ex);
            }
        }

        /// <summary>
        /// Obtiener la lista de tipos de vías
        /// </summary>
        /// <returns>Lista de tipos de vías</returns>
        public List<ViaType> GetViaTypes()
        {
            try
            {
                var result = ModelAssembler.CreateViaTypes(DataFacadeManager.GetObjects(typeof(COMMEN.TransportViaType)));
                DataFacadeManager.Dispose();
                return result;
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetViaTypes), ex);
            }
        }

        /// <summary>
        /// Obtiener la lista de periodos de ajuste
        /// </summary>
        /// <returns>Lista de tipos de periodos de ajuste</returns>
        public List<AdjustPeriod> GetAdjustPeriods()
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(PARAMEN.BillingPeriod.Properties.IsEnabled, typeof(PARAMEN.BillingPeriod).Name);
                filter.Equal();
                filter.Constant(Status.Enabled);

                BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(PARAMEN.BillingPeriod), filter.GetPredicate());
                DataFacadeManager.Dispose();
                return ModelAssembler.CreateAdjustPeriods(businessCollection);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetAdjustPeriods), ex);
            }

        }
        #endregion

        #region Integration

        public List<Transport> GetTransportByEndorsementIdModuleType(int endorsementId, Core.Services.UtilitiesServices.Enums.ModuleType moduleType)
        {
            switch (moduleType)
            {
                case ModuleType.Emission:
                    return new List<Transport>();
                case ModuleType.Claim:

                    List<Transport> transports = new List<Transport>();

                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
                    filter.Equal();
                    filter.Constant(endorsementId);

                    ClaimRiskTransportView claimRiskTransportView = new ClaimRiskTransportView();
                    ViewBuilder builder = new ViewBuilder("ClaimRiskTransportView");
                    builder.Filter = filter.GetPredicate();

                    DataFacadeManager.Instance.GetDataFacade().FillView(builder, claimRiskTransportView);

                    if (claimRiskTransportView.RiskTransports.Count > 0)
                    {
                        foreach (ISSEN.RiskTransport entityRiskTransport in claimRiskTransportView.RiskTransports)
                        {
                            ISSEN.EndorsementRisk entityEndorsementRisk = claimRiskTransportView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().First(x => x.RiskId == entityRiskTransport.RiskId);
                            ISSEN.Risk entityRisk = claimRiskTransportView.Risks.Cast<ISSEN.Risk>().First(x => x.RiskId == entityRiskTransport.RiskId);
                            ISSEN.Policy entityPolicy = claimRiskTransportView.Policies.Cast<ISSEN.Policy>().FirstOrDefault(x => x.PolicyId == entityEndorsementRisk.PolicyId);
                            COMMEN.TransportCargoType entityTransportCargoType = claimRiskTransportView.TransportsCargoTypes.Cast<COMMEN.TransportCargoType>().FirstOrDefault(x => x.TransportCargoTypeCode == entityRiskTransport.TransportCargoTypeCode);
                            COMMEN.City entityCityFrom = claimRiskTransportView.Cities.Cast<COMMEN.City>().FirstOrDefault(x => x.CityCode == entityRiskTransport.CityFromCode);
                            COMMEN.City entityCityTo = claimRiskTransportView.Cities.Cast<COMMEN.City>().FirstOrDefault(x => x.CityCode == entityRiskTransport.CityToCode);
                            COMMEN.TransportViaType entityTransportViaType = claimRiskTransportView.TransportsViaTypes.Cast<COMMEN.TransportViaType>().FirstOrDefault(x => x.TransportViaTypeCode == entityRiskTransport.TransportViaTypeCode);
                            COMMEN.TransportPackagingType entityTransportPackagingType = claimRiskTransportView.TransportsPackagingTypes.Cast<COMMEN.TransportPackagingType>().FirstOrDefault(x => x.TransportPackagingTypeCode == entityRiskTransport.TransportPackagingTypeCode);
                            COMMEN.TransportMean entityTransportMean = claimRiskTransportView.TransportMeans.Cast<COMMEN.TransportMean>().FirstOrDefault();

                            Transport transport = ModelAssembler.CreateTransport(entityRiskTransport, entityRisk, entityEndorsementRisk, entityPolicy, entityTransportCargoType, entityCityFrom, entityCityTo, entityTransportViaType, entityTransportPackagingType);

                            ObjectCriteriaBuilder filterSumAssured = new ObjectCriteriaBuilder();
                            filterSumAssured.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                            filterSumAssured.Equal();
                            filterSumAssured.Constant(endorsementId);

                            SumAssuredTransportView assuredView = new SumAssuredTransportView();
                            ViewBuilder builderAssured = new ViewBuilder("SumAssuredTransportView");
                            builderAssured.Filter = filterSumAssured.GetPredicate();
                            DataFacadeManager.Instance.GetDataFacade().FillView(builderAssured, assuredView);

                            decimal insuredAmount = 0;

                            foreach (ISSEN.RiskCoverage entityRiskCoverage in assuredView.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList())
                            {
                                insuredAmount += entityRiskCoverage.LimitAmount;
                            }

                            transport.Risk.Description = transport.CargoType.Description + " - " + entityTransportMean?.Description;
                            transport.ReleaseAmount = insuredAmount;

                            transports.Add(transport);
                        }
                    }

                    return transports;
                default:
                    return new List<Transport>();
            }
        }
        public List<Transport> GetTransportsByInsuredId(int insuredId)
        {
            List<Transport> transports = new List<Transport>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Risk.Properties.InsuredId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(insuredId);

            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);

            ClaimRiskTransportView claimRiskTransportView = new ClaimRiskTransportView();
            ViewBuilder builder = new ViewBuilder("ClaimRiskTransportView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, claimRiskTransportView);

            if (claimRiskTransportView.RiskTransports.Count > 0)
            {
                foreach (ISSEN.RiskTransport entityRiskTransport in claimRiskTransportView.RiskTransports)
                {
                    ISSEN.EndorsementRisk entityEndorsementRisk = claimRiskTransportView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().First(x => x.RiskId == entityRiskTransport.RiskId);
                    ISSEN.Risk entityRisk = claimRiskTransportView.Risks.Cast<ISSEN.Risk>().First(x => x.RiskId == entityRiskTransport.RiskId);
                    ISSEN.Policy entityPolicy = claimRiskTransportView.Policies.Cast<ISSEN.Policy>().FirstOrDefault(x => x.PolicyId == entityEndorsementRisk.PolicyId);
                    COMMEN.TransportCargoType entityTransportCargoType = claimRiskTransportView.TransportsCargoTypes.Cast<COMMEN.TransportCargoType>().FirstOrDefault(x => x.TransportCargoTypeCode == entityRiskTransport.TransportCargoTypeCode);
                    COMMEN.City entityCityFrom = claimRiskTransportView.Cities.Cast<COMMEN.City>().FirstOrDefault(x => x.CityCode == entityRiskTransport.CityFromCode);
                    COMMEN.City entityCityTo = claimRiskTransportView.Cities.Cast<COMMEN.City>().FirstOrDefault(x => x.CityCode == entityRiskTransport.CityToCode);
                    COMMEN.TransportViaType entityTransportViaType = claimRiskTransportView.TransportsViaTypes.Cast<COMMEN.TransportViaType>().FirstOrDefault(x => x.TransportViaTypeCode == entityRiskTransport.TransportViaTypeCode);
                    COMMEN.TransportPackagingType entityTransportPackagingType = claimRiskTransportView.TransportsPackagingTypes.Cast<COMMEN.TransportPackagingType>().FirstOrDefault(x => x.TransportPackagingTypeCode == entityRiskTransport.TransportPackagingTypeCode);
                    COMMEN.TransportMean entityTransportMean = claimRiskTransportView.TransportMeans.Cast<COMMEN.TransportMean>().FirstOrDefault();

                    Transport transport = ModelAssembler.CreateTransport(entityRiskTransport, entityRisk, entityEndorsementRisk, entityPolicy, entityTransportCargoType, entityCityFrom, entityCityTo, entityTransportViaType, entityTransportPackagingType);

                    ObjectCriteriaBuilder filterSumAssured = new ObjectCriteriaBuilder();
                    filterSumAssured.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                    filterSumAssured.Equal();
                    filterSumAssured.Constant(entityEndorsementRisk.EndorsementId);

                    SumAssuredTransportView assuredView = new SumAssuredTransportView();
                    ViewBuilder builderAssured = new ViewBuilder("SumAssuredTransportView");
                    builderAssured.Filter = filterSumAssured.GetPredicate();
                    DataFacadeManager.Instance.GetDataFacade().FillView(builderAssured, assuredView);

                    decimal insuredAmount = 0;

                    foreach (ISSEN.RiskCoverage entityRiskCoverage in assuredView.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList())
                    {
                        insuredAmount += entityRiskCoverage.LimitAmount;
                    }

                    transport.Risk.Description = transport.CargoType.Description + " - " + entityTransportMean?.Description;
                    transport.ReleaseAmount = insuredAmount;

                    transports.Add(transport);
                }
            }
            return transports;
        }

        public Transport GetTransportByRiskId(int riskId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.RiskTransport.Properties.RiskId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(riskId);

            ClaimRiskTransportView claimRiskTransportView = new ClaimRiskTransportView();
            ViewBuilder builder = new ViewBuilder("ClaimRiskTransportView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, claimRiskTransportView);

            if (claimRiskTransportView.RiskTransports.Count > 0)
            {
                ISSEN.RiskTransport entityRiskTransport = claimRiskTransportView.RiskTransports.Cast<ISSEN.RiskTransport>().First();
                ISSEN.EndorsementRisk entityEndorsementRisk = claimRiskTransportView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().First(x => x.RiskId == entityRiskTransport.RiskId);
                ISSEN.Risk entityRisk = claimRiskTransportView.Risks.Cast<ISSEN.Risk>().First(x => x.RiskId == entityRiskTransport.RiskId);
                ISSEN.Policy entityPolicy = claimRiskTransportView.Policies.Cast<ISSEN.Policy>().FirstOrDefault(x => x.PolicyId == entityEndorsementRisk.PolicyId);
                COMMEN.TransportCargoType entityTransportCargoType = claimRiskTransportView.TransportsCargoTypes.Cast<COMMEN.TransportCargoType>().FirstOrDefault(x => x.TransportCargoTypeCode == entityRiskTransport.TransportCargoTypeCode);
                COMMEN.City entityCityFrom = claimRiskTransportView.Cities.Cast<COMMEN.City>().FirstOrDefault(x => x.CityCode == entityRiskTransport.CityFromCode);
                COMMEN.City entityCityTo = claimRiskTransportView.Cities.Cast<COMMEN.City>().FirstOrDefault(x => x.CityCode == entityRiskTransport.CityToCode);
                COMMEN.TransportViaType entityTransportViaType = claimRiskTransportView.TransportsViaTypes.Cast<COMMEN.TransportViaType>().FirstOrDefault(x => x.TransportViaTypeCode == entityRiskTransport.TransportViaTypeCode);
                COMMEN.TransportPackagingType entityTransportPackagingType = claimRiskTransportView.TransportsPackagingTypes.Cast<COMMEN.TransportPackagingType>().FirstOrDefault(x => x.TransportPackagingTypeCode == entityRiskTransport.TransportPackagingTypeCode);
                COMMEN.TransportMean entityTransportMean = claimRiskTransportView.TransportMeans.Cast<COMMEN.TransportMean>().FirstOrDefault();

                Transport transport = ModelAssembler.CreateTransport(entityRiskTransport, entityRisk, entityEndorsementRisk, entityPolicy, entityTransportCargoType, entityCityFrom, entityCityTo, entityTransportViaType, entityTransportPackagingType);

                ObjectCriteriaBuilder filterSumAssured = new ObjectCriteriaBuilder();
                filterSumAssured.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                filterSumAssured.Equal();
                filterSumAssured.Constant(entityEndorsementRisk.EndorsementId);

                SumAssuredTransportView assuredView = new SumAssuredTransportView();
                ViewBuilder builderAssured = new ViewBuilder("SumAssuredTransportView");
                builderAssured.Filter = filterSumAssured.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builderAssured, assuredView);

                decimal insuredAmount = 0;

                foreach (ISSEN.RiskCoverage entityRiskCoverage in assuredView.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList())
                {
                    insuredAmount += entityRiskCoverage.LimitAmount;
                }

                transport.Risk.Description = transport.CargoType.Description + " - " + entityTransportMean?.Description;
                transport.ReleaseAmount = insuredAmount;

                return transport;
            }

            return null;
        }
        #endregion
    }
}