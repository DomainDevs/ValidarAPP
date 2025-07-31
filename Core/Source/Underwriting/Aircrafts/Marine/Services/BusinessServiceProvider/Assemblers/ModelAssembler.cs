using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.Marines.MarineBusinessService.Models;
using MARI=Sistran.Core.Application.Marines.MarineBusinessService.Models;
using Sistran.Core.Framework.DAF;
using COMMEN = Sistran.Core.Application.Common.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Newtonsoft.Json;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.RulesScriptsServices.Models;

namespace Sistran.Core.Application.Marines.MarineBusinessService.EEProvider.Assemblers
{
    public class ModelAssembler
    {


        #region CreateMarineTypes
        /// <summary>
        /// Trae el tipo de Marine
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        internal static List<MarineType> CreateMarineTypes(BusinessCollection businessCollection)
        {
            List<MarineType> MarineType = new List<MarineType>();
            foreach (COMMEN.AircraftType entityAircraftType in businessCollection)
            {
                MarineType.Add(CreateMarinetype(entityAircraftType));
            }
            return MarineType;
        }
        /// <summary>
        /// Convierte en modelo los Marines
        /// </summary>
        /// <param name="entityAircraftType"></param>
        /// <returns></returns>
        private static MarineType CreateMarinetype(BusinessObject businessObject)
        {
            COMMEN.AircraftType entityAircraftType = (COMMEN.AircraftType)businessObject;
            if (businessObject != null)
            {
                return new MarineType
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
        #region CreateMarinetType
        internal static List<MarineType> CreateMarineType(List<COMMEN.AircraftType> entityAircraftType,List<COMMEN.AircraftTypePrefix> entityAircraftTypePrefix)
        {
            List<MarineType> coreMarineTypes = new List<MarineType>();
            foreach (COMMEN.AircraftType aircraftType in entityAircraftType)
            {
                coreMarineTypes.Add(new MarineType
                {
                    Id = entityAircraftType.First(x => x.AircraftTypeCode == aircraftType.AircraftTypeCode).AircraftTypeCode,
                    Description = entityAircraftType.First(x => x.Description == aircraftType.Description).Description,
                    SmallDescription = entityAircraftType.First(x => x.SmallDescription == aircraftType.SmallDescription).SmallDescription,
                    PrefixCode = entityAircraftTypePrefix.Last(x => x.AircraftTypeCode == aircraftType.AircraftTypeCode).PrefixCode
                });
            }
            return coreMarineTypes;
        }
        #endregion
        #region CreateMarineUse
        internal static List<Use> CreateMarineUse(List<COMMEN.AircraftUse> entityAircraftUse,List<COMMEN.AircraftUsePrefix> entityAircraftUsePrefix)
        {
            List<Use> coreMarineUse = new List<Use>();
            foreach (COMMEN.AircraftUse aircraftUse in entityAircraftUse)
            {
                coreMarineUse.Add(new Use
                {
                    Id = entityAircraftUse.First(x => x.AircraftUseCode == aircraftUse.AircraftUseCode).AircraftUseCode,
                    Description = entityAircraftUse.First(x => x.Description == aircraftUse.Description).Description,
                    SmallDescription = entityAircraftUse.First(x => x.SmallDescription == aircraftUse.SmallDescription).SmallDescription,
                    PrefixCode = entityAircraftUsePrefix.Last(x => x.AircraftUseCode == aircraftUse.AircraftUseCode).PrefixCode
                });
            }
            return coreMarineUse;
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
            List<Make> marineMakes = new List<Make>();
            foreach (COMMEN.AircraftMake field in businessCollection)
            {
                marineMakes.Add(CreateMake(field));
            }
            return marineMakes;
        }
        /// <summary>
        /// trae una marca de avion
        /// </summary>
        /// <param name="AircraftMake"></param>
        /// <returns></returns>
        internal static Make CreateMake(COMMEN.AircraftMake aircraftMake)
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
            List<Model> marineModel = new List<Model>();
            foreach (COMMEN.AircraftModel field in businessCollection)
            {
                marineModel.Add(CreateModelByMakeId(field));
            }
            return marineModel;
        }
        /// <summary>
        /// trae un modelo por marca de avion
        /// </summary>
        /// <param name="AircraftModel"></param>
        /// <returns></returns>
        internal static Model CreateModelByMakeId(COMMEN.AircraftModel aircraftModel)
        {
            if (aircraftModel == null)
                return null;
            return new Model
            {
                Id = aircraftModel.AircraftModelCode,
                MarineMakeCode = aircraftModel.AircraftMakeCode,
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
        public static List<MARI.Operator> CreateOperators(BusinessCollection businessCollection)
        {
            List<MARI.Operator> aircraftOperator = new List<MARI.Operator>();
            foreach (COMMEN.AircraftOperator field in businessCollection)
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
        internal static MARI.Operator CreateOperator(COMMEN.AircraftOperator aircraftOperator)
        {
            if (aircraftOperator == null)
                return null;
            return new MARI.Operator
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
            List<Register> marineRegister = new List<Register>();
            foreach (COMMEN.AircraftRegister field in businessCollection)
            {
                marineRegister.Add(CreateRegister(field));
            }
            return marineRegister;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AircraftRegister"></param>
        /// <returns></returns>
        internal static Register CreateRegister(COMMEN.AircraftRegister aircraftRegister)
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
        public static List<MarineType> CreateTypes(BusinessCollection businessCollection)
        {
            List<MarineType> aircraftTypes = new List<MarineType>();
            foreach (COMMEN.AircraftType field in businessCollection)
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
        internal static MarineType CreateType(COMMEN.AircraftType aircraftTypes)
        {
            if (aircraftTypes == null)
                return null;
            return new MarineType
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
        public static List<MarineTypePrefix> CreateTypesByPrefixIds(BusinessCollection businessCollection)
        {
            List<MarineTypePrefix> marineTypesByPrefixId = new List<MarineTypePrefix>();
            foreach (COMMEN.AircraftTypePrefix field in businessCollection)
            {
                marineTypesByPrefixId.Add(CreateTypesByPrefixId(field));
            }
            return marineTypesByPrefixId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AircraftRegister"></param>
        /// <returns></returns>
        internal static MarineTypePrefix CreateTypesByPrefixId(COMMEN.AircraftTypePrefix aircraftTypesByPrefixId)
        {
            if (aircraftTypesByPrefixId == null)
                return null;
            return new MarineTypePrefix
            {
                Id = aircraftTypesByPrefixId.AircraftTypeCode,
                PrefixCode = aircraftTypesByPrefixId.PrefixCode

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
            List<Use> marineUse = new List<Use>();
            foreach (COMMEN.AircraftUse field in businessCollection)
            {
                marineUse.Add(CreateUse(field));
            }
            return marineUse;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AircraftRegister"></param>
        /// <returns></returns>
        internal static Use CreateUse(COMMEN.AircraftUse aircraftUse)
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
        public static List<UsePrefix> CreateUsesByPrefixIds(BusinessCollection businessCollection)
        {
            List<UsePrefix> marineUseByPrefixId = new List<UsePrefix>();
            foreach (COMMEN.AircraftUsePrefix field in businessCollection)
            {
                marineUseByPrefixId.Add(CreateUsesByPrefixId(field));
            }
            return marineUseByPrefixId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AircraftRegister"></param>
        /// <returns></returns>
        internal static UsePrefix CreateUsesByPrefixId(COMMEN.AircraftUsePrefix aircraftUseByPrefixId)
        {
            if (aircraftUseByPrefixId == null)
                return null;
            return new UsePrefix
            {
                Id = aircraftUseByPrefixId.AircraftUseCode,
                PrefixCode = aircraftUseByPrefixId.PrefixCode

            };
        }
        #endregion


        public static List<Marine> CreateMarines(BusinessCollection businessCollection)
        {
            List<Marine> marines = new List<Marine>();

            foreach (ISSEN.EndorsementOperation entityEndorsementOperation in businessCollection)
            {
                marines.Add(CreateMarine(entityEndorsementOperation));
            }

            return marines;
        }

        private static Marine CreateMarine(ISSEN.EndorsementOperation entityEndorsementOperation)
        {
            Marine marine= new Marine();

            if (!string.IsNullOrEmpty(entityEndorsementOperation.Operation))
            {
                marine = JsonConvert.DeserializeObject<Marine>(entityEndorsementOperation.Operation);
                marine.Risk.Id = 0;
                marine.Risk.Number = entityEndorsementOperation.RiskNumber.Value;
                if (marine.Risk.Coverages != null)
                {
                    marine.Risk.Coverages.ForEach(x => x.CoverageOriginalStatus = x.CoverStatus);
                }
            }
            return marine;
        }

        public static Marine CreateMarine(ISSEN.Risk risk, ISSEN.RiskAircraft riskMarine, ISSEN.EndorsementRisk endorsementRisk)
        {
            Marine Marine = new Marine
            {
                Risk = new Risk
                {
                    RiskId = risk.RiskId,
                    Number = endorsementRisk.RiskNum,
                    CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode,
                    GroupCoverage = new GroupCoverage
                    {
                        Id = risk.CoverGroupId.Value,
                        CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode
                    },
                    MainInsured = new IssuanceInsured
                    {
                        IndividualId = risk.InsuredId,
                        CompanyName = new IssuanceCompanyName
                        {
                            NameNum = risk.NameNum.GetValueOrDefault(),
                            Address = new IssuanceAddress
                            {
                                Id = risk.AddressId.GetValueOrDefault()
                            }
                        }
                    },

                    Status = RiskStatusType.NotModified,
                    OriginalStatus = (RiskStatusType)endorsementRisk.RiskStatusCode,
                    DynamicProperties = new List<DynamicConcept>()
                },
            };

            foreach (DynamicProperty item in risk.DynamicProperties)
            {
                DynamicProperty dynamicProperty = (DynamicProperty)item.Value;
                DynamicConcept dynamicConcept = new DynamicConcept();
                dynamicConcept.Id = dynamicProperty.Id;
                dynamicConcept.Value = dynamicProperty.Value;
                Marine.Risk.DynamicProperties.Add(dynamicConcept);
            }

            return Marine;
        }
    }
}
 