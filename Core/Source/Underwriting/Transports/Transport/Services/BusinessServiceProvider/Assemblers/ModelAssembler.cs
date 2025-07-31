using System;
using System.Collections.Generic;
using Sistran.Core.Application.Transports.TransportBusinessService.Models;
using Sistran.Core.Framework.DAF;
using COMMEN = Sistran.Core.Application.Common.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;

namespace Sistran.Core.Application.Transports.TransportBusinessService.EEProvider.Assemblers
{
    public class ModelAssembler
    {

        /// <summary>
        /// Trae el tipo de mercancia
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        internal static List<CargoType> CreateCargoTypes(BusinessCollection businessCollection)
        {
            List<CargoType> cargoTypes = new List<CargoType>();

            foreach (COMMEN.TransportCargoType entityCargoType in businessCollection)
            {
                cargoTypes.Add(CreateCargoType(entityCargoType));
            }
            return cargoTypes;
        }

        /// <summary>
        /// Convierte en modelo las mercancias
        /// </summary>
        /// <param name="entityCargoType"></param>
        /// <returns></returns>
        public static CargoType CreateCargoType(BusinessObject businessObject)
        {
            if (businessObject != null)
            {
                COMMEN.TransportCargoType entityCargoType = (COMMEN.TransportCargoType)businessObject;
                return new CargoType
                {
                    Id = entityCargoType.TransportCargoTypeCode,
                    SmallDescription = entityCargoType.SmallDescription,
                    Description = entityCargoType.Description,
                    IsEnabled = entityCargoType.Enabled
                };
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Trae el tipo de transporte
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        internal static List<TransportType> CreateTransportTypes(BusinessCollection businessCollection)
        {
            List<TransportType> TransportType = new List<TransportType>();
            foreach (COMMEN.TransportMean entityTransportType in businessCollection)
            {
                TransportType.Add(CreateTransporType(entityTransportType));
            }
            return TransportType;
        }

        /// <summary>
        /// Convierte en modelo los transportes
        /// </summary>
        /// <param name="entityTransportType"></param>
        /// <returns></returns>
        private static TransportType CreateTransporType(BusinessObject businessObject)
        {
            COMMEN.TransportMean entityTransportType = (COMMEN.TransportMean)businessObject;
            if (businessObject != null)
            {
                return new TransportType
                {
                    Id = entityTransportType.TransportMeanCode,
                    SmallDescription = entityTransportType.SmallDescription,
                    Description = entityTransportType.Description,
                    IsEnabled = entityTransportType.Enabled
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Trae el tipo de periodo
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        internal static List<DeclarationPeriod> CreateDeclarationPeriodTypes(BusinessCollection businessCollection)
        {
            List<DeclarationPeriod> DeclarationPeriod = new List<DeclarationPeriod>();
            foreach (PARAMEN.DeclarationPeriod entityDeclarationPriod in businessCollection)
            {
                DeclarationPeriod.Add(CreateDeclarationPeriodType(entityDeclarationPriod));
            }
            return DeclarationPeriod;
        }

        /// <summary>
        /// Convierte el modelo en periodo
        /// </summary>
        /// <param name="entityDeclarationPriod"></param>
        /// <returns></returns>
        private static DeclarationPeriod CreateDeclarationPeriodType(BusinessObject businessObject)
        {
            PARAMEN.DeclarationPeriod entityDeclarationPriod = (PARAMEN.DeclarationPeriod)businessObject;
            if (businessObject != null)
            {
                return new DeclarationPeriod
                {
                    Id = entityDeclarationPriod.DeclarationPeriodCode,
                    Description = entityDeclarationPriod.Description,
                    IsEnabled = entityDeclarationPriod.IsEnabled
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Convierte lista via types
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        internal static List<ViaType> CreateViaTypes(BusinessCollection businessCollection)
        {
            List<ViaType> viaTypes = new List<ViaType>();
            foreach (COMMEN.TransportViaType entityViaTypes in businessCollection)
            {
                viaTypes.Add(CreateViaType(entityViaTypes));
            }
            return viaTypes;
        }

        /// <summary>
        /// Convierte via type
        /// </summary>
        /// <param name="businessObject"></param>
        /// <returns></returns>
        private static ViaType CreateViaType(BusinessObject businessObject)
        {
            COMMEN.TransportViaType entityViaType = (COMMEN.TransportViaType)businessObject;
            if (businessObject != null)
            {
                return new ViaType
                {
                    Id = entityViaType.TransportViaTypeCode,
                    Description = entityViaType.Description,
                    SmallDescription = entityViaType.SmallDescription,
                    IsEnabled = entityViaType.Enabled
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Convierte lista adjust period
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        internal static List<AdjustPeriod> CreateAdjustPeriods(BusinessCollection businessCollection)
        {
            List<AdjustPeriod> adjustPeriods = new List<AdjustPeriod>();
            foreach (PARAMEN.BillingPeriod entityAdjustPeriod in businessCollection)
            {
                adjustPeriods.Add(CreateAdjustPeriod(entityAdjustPeriod));
            }
            return adjustPeriods;
        }

        /// <summary>
        /// Convierte adjust period
        /// </summary>
        /// <param name="businessObject"></param>
        /// <returns></returns>
        private static AdjustPeriod CreateAdjustPeriod(BusinessObject businessObject)
        {
            PARAMEN.BillingPeriod entityAdjustPeriod = (PARAMEN.BillingPeriod)businessObject;
            if (businessObject != null)
            {
                return new AdjustPeriod
                {
                    Id = entityAdjustPeriod.BillingPeriodCode,
                    Description = entityAdjustPeriod.Description,
                    IsEnabled = entityAdjustPeriod.IsEnabled
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Crea lista de packaging type
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        internal static List<PackagingType> CreatePackagingTypes(BusinessCollection businessCollection)
        {
            List<PackagingType> packagingType = new List<PackagingType>();
            foreach (COMMEN.TransportPackagingType entityPackagingType in businessCollection)
            {
                packagingType.Add(CreatePackagingType(entityPackagingType));
            }
            return packagingType;
        }

        /// <summary>
        /// Crea packaging type
        /// </summary>
        /// <param name="businessObject"></param>
        /// <returns></returns>
        private static PackagingType CreatePackagingType(BusinessObject businessObject)
        {
            COMMEN.TransportPackagingType entityPackagingType = (COMMEN.TransportPackagingType)businessObject;
            if (businessObject != null)
            {
                return new PackagingType
                {
                    Id = entityPackagingType.TransportPackagingTypeCode,
                    SmallDescription = entityPackagingType.SmallDescription,
                    Description = entityPackagingType.Description,
                    IsEnabled = entityPackagingType.Enabled
                };
            }
            else
            {
                return null;
            }
        }

        internal static Transport CreateTransport(ISSEN.RiskTransport entityRiskTransport, ISSEN.Risk entityRisk, ISSEN.EndorsementRisk entityEndorsementRisk, ISSEN.Policy entityPolicy, COMMEN.TransportCargoType entityTransportCargoType, COMMEN.City entityCityFrom, COMMEN.City entityCityTo, COMMEN.TransportViaType entityTransportViaType, COMMEN.TransportPackagingType entityTransportPackagingType)
        {
            return new Transport
            {
                Risk = new Risk
                {
                    RiskId = entityRiskTransport.RiskId,
                    Number = entityEndorsementRisk.RiskNum,
                    CoveredRiskType = (CoveredRiskType)entityRisk.CoveredRiskTypeCode,
                    MainInsured = new IssuanceInsured
                    {
                        IndividualId = entityRisk.InsuredId
                    },
                    Policy = new Policy
                    {
                        Id = entityPolicy.PolicyId,
                        DocumentNumber = entityPolicy.DocumentNumber,
                        Endorsement = new Endorsement
                        {
                            Id = entityEndorsementRisk.EndorsementId
                        }
                    }
                },
                CargoType = new CargoType
                {
                    Id = entityRiskTransport.TransportCargoTypeCode,
                    Description = entityTransportCargoType?.Description
                },
                PackagingType = new PackagingType
                {
                    Id = entityRiskTransport.TransportPackagingTypeCode,
                    Description = entityTransportPackagingType?.Description
                },
                CityFrom = new City
                {
                    Id = Convert.ToInt32(entityRiskTransport.CityFromCode),
                    Description = entityCityFrom?.Description
                },
                CityTo = new City
                {
                    Id = Convert.ToInt32(entityRiskTransport.CityToCode),
                    Description = entityCityTo?.Description
                },
                ViaType = new ViaType
                {
                    Id = Convert.ToInt32(entityRiskTransport.TransportViaTypeCode),
                    Description = entityTransportViaType?.Description
                },
                Types = new List<TransportType>()
            };
        }
    }
}
