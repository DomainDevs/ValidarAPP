using System;
using System.Collections.Generic;
using Sistran.Core.Application.Aircrafts.AircraftBusinessService.Models;
using Sistran.Core.Framework.DAF;
using COMMEN = Sistran.Core.Application.Common.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using System.Linq;
using COMMON = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.UnderwritingServices.Models;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.Aircrafts.AircraftBusinessService.EEProvider.Assemblers
{
    public class ModelAssembler
    {

        #region CreateAircraftTypes
        /// <summary>
        /// Trae el tipo de Aircrafte
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        internal static List<AircraftType> CreateAircraftTypes(BusinessCollection businessCollection)
        {
            List<AircraftType> AircraftType = new List<AircraftType>();
            foreach (COMMEN.AircraftType entityAircraftType in businessCollection)
            {
                AircraftType.Add(CreateAircraftype(entityAircraftType));
            }
            return AircraftType;
        }
        /// <summary>
        /// Convierte en modelo los Aircraftes
        /// </summary>
        /// <param name="entityAircraftType"></param>
        /// <returns></returns>
        private static AircraftType CreateAircraftype(BusinessObject businessObject)
        {
            COMMEN.AircraftType entityAircraftType = (COMMEN.AircraftType)businessObject;
            if (businessObject != null)
            {
                return new AircraftType
                {
                    Id = entityAircraftType.AircraftTypeCode,
                    SmallDescription = entityAircraftType.SmallDescription,
                    Description = entityAircraftType.Description,
                    IsEnabled = entityAircraftType.Enabled
                };
            }
            else
            {
                return null;
            }
        }
        #endregion
        #region CreateAircraftType
        internal static List<AircraftType> CreateAircraftType
            (List<COMMON.AircraftType> entityAircraftType,
             List<COMMON.AircraftTypePrefix> entityAircraftTypePrefix)
        {
            List<AircraftType> coreAircraftTypes = new List<AircraftType>();
            foreach (COMMON.AircraftType aircraftType in entityAircraftType)
            {
                coreAircraftTypes.Add(new AircraftType
                {
                    Id = entityAircraftType.First(x => x.AircraftTypeCode == aircraftType.AircraftTypeCode).AircraftTypeCode,
                    Description = entityAircraftType.First(x => x.Description == aircraftType.Description).Description,
                    SmallDescription = entityAircraftType.First(x => x.SmallDescription == aircraftType.SmallDescription).SmallDescription,
                    PrefixCode = entityAircraftTypePrefix.Last(x => x.AircraftTypeCode == aircraftType.AircraftTypeCode).PrefixCode
                });
            }
            return coreAircraftTypes;
        }
        #endregion
        #region CreateAircraftUse
        internal static List<Use> CreateAircraftUse
           (List<COMMON.AircraftUse> entityAircraftUse,
            List<COMMON.AircraftUsePrefix> entityAircraftUsePrefix)
        {
            List<Use> coreAircraftUse = new List<Use>();
            foreach (COMMON.AircraftUse aircraftUse in entityAircraftUse)
            {
                coreAircraftUse.Add(new Use
                {
                    Id = entityAircraftUse.First(x => x.AircraftUseCode == aircraftUse.AircraftUseCode).AircraftUseCode,
                    Description = entityAircraftUse.First(x => x.Description == aircraftUse.Description).Description,
                    SmallDescription = entityAircraftUse.First(x => x.SmallDescription == aircraftUse.SmallDescription).SmallDescription,
                    PrefixCode = entityAircraftUsePrefix.Last(x => x.AircraftUseCode == aircraftUse.AircraftUseCode).PrefixCode
                });
            }
            return coreAircraftUse;
        }
        #endregion
        #region GetMake
        /// <summary>
        /// Lista de las Marcas de aviones
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<Make> CreateMakes(BusinessCollection businessCollection)
        {
            List<Make> aircraftMakes = new List<Make>();
            foreach (COMMON.AircraftMake field in businessCollection)
            {
                aircraftMakes.Add(CreateMake(field));
            }
            return aircraftMakes;
        }
        /// <summary>
        /// trae una marca de avion
        /// </summary>
        /// <param name="AircraftMake"></param>
        /// <returns></returns>
        internal static Make CreateMake(COMMON.AircraftMake aircraftMake)
        {
            if (aircraftMake == null)
                return null;
            return new Make
            {
                Id = aircraftMake.AircraftMakeCode,
                SmallDescription = aircraftMake.SmallDescription,
                Description = aircraftMake.Description
            };
        }
        #endregion
        #region GetModel
        /// <summary>
        /// Lista de las Modelos por marca de aviones
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<Model> CreateModelByMakeIds(BusinessCollection businessCollection)
        {
            List<Model> aircraftModel = new List<Model>();
            foreach (COMMON.AircraftModel field in businessCollection)
            {
                aircraftModel.Add(CreateModelByMakeId(field));
            }
            return aircraftModel;
        }
        /// <summary>
        /// trae un modelo por marca de avion
        /// </summary>
        /// <param name="AircraftModel"></param>
        /// <returns></returns>
        internal static Model CreateModelByMakeId(COMMON.AircraftModel aircraftModel)
        {
            if (aircraftModel == null)
                return null;
            return new Model
            {
                Id = aircraftModel.AircraftModelCode,
                AircraftMakeCode = aircraftModel.AircraftMakeCode,
                Description = aircraftModel.Description,
                SmallDescription = aircraftModel.SmallDescription

            };
        }
        #endregion
        #region GetOperatin
        /// <summary>
        /// Retorna un listado de explotadores de aeronaves
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<Operator> CreateOperators(BusinessCollection businessCollection)
        {
            List<Operator> aircraftOperator = new List<Operator>();
            foreach (COMMON.AircraftOperator field in businessCollection)
            {
                aircraftOperator.Add(CreateOperator(field));
            }
            return aircraftOperator;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AircraftOperator"></param>
        /// <returns></returns>
        internal static Operator CreateOperator(COMMON.AircraftOperator aircraftOperator)
        {
            if (aircraftOperator == null)
                return null;
            return new Operator
            {
                Id = aircraftOperator.AircraftOperatorCode,
                Description = aircraftOperator.Description,
                SmallDescription = aircraftOperator.SmallDescription,
                DocumentNumber = aircraftOperator.DocumentNumber
            };
        }
        #endregion
        #region GetRegister
        /// <summary>
        /// Retorna el listado de matrículas que se encuentran registradas
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<Register> CreateRegisters(BusinessCollection businessCollection)
        {
            List<Register> aircraftRegister = new List<Register>();
            foreach (COMMON.AircraftRegister field in businessCollection)
            {
                aircraftRegister.Add(CreateRegister(field));
            }
            return aircraftRegister;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AircraftRegister"></param>
        /// <returns></returns>
        internal static Register CreateRegister(COMMON.AircraftRegister aircraftRegister)
        {
            if (aircraftRegister == null)
                return null;
            return new Register
            {
                Id = aircraftRegister.AircraftRegisterCode,
                Description = aircraftRegister.Description,
                SmallDescription = aircraftRegister.SmallDescription,
                CountryCode = aircraftRegister.CountryCode
            };
        }
        #endregion
        #region GetTypes
        /// <summary>
        /// Retorna un listado de tipos de aeronaves 
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<AircraftType> CreateTypes(BusinessCollection businessCollection)
        {
            List<AircraftType> aircraftTypes = new List<AircraftType>();
            foreach (COMMON.AircraftType field in businessCollection)
            {
                aircraftTypes.Add(CreateType(field));
            }
            return aircraftTypes;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AircraftRegister"></param>
        /// <returns></returns>
        internal static AircraftType CreateType(COMMON.AircraftType aircraftTypes)
        {
            if (aircraftTypes == null)
                return null;
            return new AircraftType
            {
                Id = aircraftTypes.AircraftTypeCode,
                Description = aircraftTypes.Description

            };
        }

        #endregion
        #region GetTypeByPrefix
        /// <summary>
        /// Retorna un listado de tipos de aeronaves por ramo
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<AircraftType> CreateTypesByPrefixIds(BusinessCollection businessCollection)
        {
            List<AircraftType> aircraftTypesByPrefixId = new List<AircraftType>();
            foreach (COMMON.AircraftType field in businessCollection)
            {
                aircraftTypesByPrefixId.Add(CreateTypesByPrefixId(field));
            }
            return aircraftTypesByPrefixId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AircraftRegister"></param>
        /// <returns></returns>
        internal static AircraftType CreateTypesByPrefixId(COMMON.AircraftType aircraftTypes)
        {
            if (aircraftTypes == null)
                return null;
            return new AircraftType
            {
                Id = aircraftTypes.AircraftTypeCode,
                Description = aircraftTypes.Description
            };
        }
        #endregion
        #region GetUse
        /// <summary>
        /// Retorna un listado de usos de aeronave 
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<Use> CreateUses(BusinessCollection businessCollection)
        {
            List<Use> aircraftUse = new List<Use>();
            foreach (COMMON.AircraftUse field in businessCollection)
            {
                aircraftUse.Add(CreateUse(field));
            }
            return aircraftUse;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AircraftRegister"></param>
        /// <returns></returns>
        internal static Use CreateUse(COMMON.AircraftUse aircraftUse)
        {
            if (aircraftUse == null)
                return null;
            return new Use
            {
                Id = aircraftUse.AircraftUseCode,
                Description = aircraftUse.Description,
                SmallDescription = aircraftUse.SmallDescription


            };
        }
        #endregion
        #region GetUseByPrefix
        /// <summary>
        /// Retorna un listado de usos de aeronave por ramo
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<Use> CreateUsesByPrefixIds(BusinessCollection businessCollection)
        {
            List<Use> aircraftUseByPrefixId = new List<Use>();
            foreach (COMMON.AircraftUse field in businessCollection)
            {
                aircraftUseByPrefixId.Add(CreateUsesByPrefixId(field));
            }
            return aircraftUseByPrefixId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AircraftRegister"></param>
        /// <returns></returns>
        internal static Use CreateUsesByPrefixId(COMMON.AircraftUse aircraftUseByPrefixId)
        {
            if (aircraftUseByPrefixId == null)
                return null;
            return new Use
            {
                Id = aircraftUseByPrefixId.AircraftUseCode,
                Description = aircraftUseByPrefixId.Description

            };
        }
        #endregion


        public static Aircraft CreateAircraft(ISSEN.Risk entityRisk, ISSEN.RiskAircraft entityRiskAircraft, ISSEN.EndorsementRisk entityEndorsementRisk, ISSEN.Policy entityPolicy)
        {
            Aircraft Aircraft = new Aircraft
            {
                Risk = new Risk
                {
                    RiskId = entityRisk.RiskId,
                    Number = entityEndorsementRisk.RiskNum,
                    CoveredRiskType = (CoveredRiskType)entityRisk.CoveredRiskTypeCode,
                    GroupCoverage = new GroupCoverage
                    {
                        Id = entityRisk.CoverGroupId.Value,
                        CoveredRiskType = (CoveredRiskType)entityRisk.CoveredRiskTypeCode
                    },
                    MainInsured = new IssuanceInsured
                    {
                        IndividualId = entityRisk.InsuredId,
                        CompanyName = new IssuanceCompanyName
                        {
                            NameNum = entityRisk.NameNum.GetValueOrDefault(),
                            Address = new IssuanceAddress
                            {
                                Id = entityRisk.AddressId.GetValueOrDefault()
                            }
                        }
                    },
                    Description = entityRiskAircraft.RegisterNo + " - " + entityRiskAircraft.AircraftYear,
                    Status = RiskStatusType.NotModified,
                    OriginalStatus = (RiskStatusType)entityEndorsementRisk.RiskStatusCode,
                    Policy = new Policy
                    {
                        Id = entityPolicy.PolicyId,
                        DocumentNumber = entityPolicy.DocumentNumber,
                        Endorsement = new Endorsement
                        {
                            Id = entityEndorsementRisk.EndorsementId
                        },
                        Prefix = new Prefix
                        {
                            Id = entityPolicy.PrefixCode
                        }
                    }
                },
                MakeId = Convert.ToInt32(entityRiskAircraft.AircraftMakeCode),
                ModelId = Convert.ToInt32(entityRiskAircraft.AircraftModelCode),
                TypeId = Convert.ToInt32(entityRiskAircraft.AircraftTypeCode),
                UseId = Convert.ToInt32(entityRiskAircraft.AircraftUseCode),
                RegisterId = Convert.ToInt32(entityRiskAircraft.AircraftRegisterCode),
                OperatorId = Convert.ToInt32(entityRiskAircraft.AircraftOperatorCode),
                NumberRegister = entityRiskAircraft.RegisterNo
            };

            return Aircraft;
        }
    }
}
