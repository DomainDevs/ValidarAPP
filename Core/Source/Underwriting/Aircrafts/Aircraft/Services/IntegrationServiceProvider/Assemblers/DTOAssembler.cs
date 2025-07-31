using Sistran.Core.Application.Aircrafts.AircraftBusinessService.Models;
using Sistran.Core.Integration.AircraftServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.AirCraftServices.EEProvider.Assemblers
{
    public static class DTOAssembler
    {
        internal static AircraftMakeDTO CreateAircraftMake(Make make)
        {
            return new AircraftMakeDTO
            {
                Id = make.Id,
                Description = make.Description
            };
        }

        internal static List<AircraftMakeDTO> CreateAircraftMakes(List<Make> makes)
        {
            List<AircraftMakeDTO> aircraftMakesDTO = new List<AircraftMakeDTO>();

            foreach (Make make in makes)
            {
                aircraftMakesDTO.Add(CreateAircraftMake(make));
            }

            return aircraftMakesDTO;
        }

        internal static AicraftModelDTO CreateAircraftModel(Model model)
        {
            return new AicraftModelDTO
            {
                Id = model.Id,
                Description = model.Description
            };
        }

        internal static List<AicraftModelDTO> CreateAircraftModels(List<Model> models)
        {
            List<AicraftModelDTO> aircraftModelsDTO = new List<AicraftModelDTO>();

            foreach (Model model in models)
            {
                aircraftModelsDTO.Add(CreateAircraftModel(model));
            }

            return aircraftModelsDTO;
        }

        internal static AircraftOperatorDTO CreateAircraftOperator(Operator @perator)
        {
            return new AircraftOperatorDTO
            {
                Id = @perator.Id,
                Description = @perator.Description
            };
        }

        internal static List<AircraftOperatorDTO> CreateAircraftOperators(List<Operator> operators)
        {
            List<AircraftOperatorDTO> aircraftOperatorsDTO = new List<AircraftOperatorDTO>();

            foreach (Operator @operator in operators)
            {
                aircraftOperatorsDTO.Add(CreateAircraftOperator(@operator));
            }

            return aircraftOperatorsDTO;
        }

        internal static AircraftRegisterDTO CreateAircraftRegister(Register register)
        {
            return new AircraftRegisterDTO
            {
                Id = register.Id,
                Description = register.Description
            };
        }

        internal static List<AircraftRegisterDTO> CreateAircraftRegisters(List<Register> registers)
        {
            List<AircraftRegisterDTO> aircraftResgistersDTO = new List<AircraftRegisterDTO>();

            foreach (Register register in registers)
            {
                aircraftResgistersDTO.Add(CreateAircraftRegister(register));
            }

            return aircraftResgistersDTO;
        }

        internal static AircraftUseDTO CreateAircraftUse(Use use)
        {
            return new AircraftUseDTO
            {
                Id = use.Id,
                Description = use.Description
            };
        }

        internal static List<AircraftUseDTO> CreateAircraftUses(List<Use> uses)
        {
            List<AircraftUseDTO> aircraftUsesDTO = new List<AircraftUseDTO>();

            foreach (Use use in uses)
            {
                aircraftUsesDTO.Add(CreateAircraftUse(use));
            }

            return aircraftUsesDTO;
        }

        internal static AircraftTypeDTO CreateAircraftType(AircraftType type)
        {
            return new AircraftTypeDTO
            {
                Id = type.Id,
                Description = type.Description
            };
        }

        internal static List<AircraftTypeDTO> CreateAircraftTypes(List<AircraftType> types)
        {
            List<AircraftTypeDTO> aircraftTypesDTO = new List<AircraftTypeDTO>();

            foreach (AircraftType type in types)
            {
                aircraftTypesDTO.Add(CreateAircraftType(type));
            }

            return aircraftTypesDTO;
        }

        internal static AircraftDTO CreateAircraft(Aircraft aircraft)
        {
            return new AircraftDTO
            {
                MakeId = aircraft.MakeId,
                ModelId = aircraft.ModelId,
                TypeId = aircraft.TypeId,
                UseId = aircraft.UseId,
                RegisterId = aircraft.RegisterId,
                OperatorId = aircraft.OperatorId,
                RegisterNumber = aircraft.NumberRegister,
                RiskId = aircraft.Risk.RiskId,
                Risk = aircraft.Risk.Description,
                CoveredRiskType = (int)aircraft.Risk.CoveredRiskType,
                EndorsementId = aircraft.Risk.Policy.Endorsement.Id,
                InsuredId = aircraft.Risk.MainInsured.IndividualId,
                PolicyId = aircraft.Risk.Policy.Id,
                PolicyDocumentNumber = aircraft.Risk.Policy.DocumentNumber,
                InsuredAmount = aircraft.Risk.AmountInsured,
                RiskNumber = aircraft.Risk.Number
            };
        }


        internal static List<AircraftDTO> CreateAircrafts(List<Aircraft> aircrafts)
        {
            List<AircraftDTO> aircraftsDTO = new List<AircraftDTO>();

            foreach (Aircraft aircraft in aircrafts)
            {
                aircraftsDTO.Add(CreateAircraft(aircraft));
            }

            return aircraftsDTO;
        }
    }
}
