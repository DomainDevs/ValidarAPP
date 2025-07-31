using AutoMapper;
using Sistran.Company.Application.MassiveVehicleParamBusinessService.Model;
using Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.DTOs;
using Sistran.Core.Application.Utilities.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.MassiveVehicleParamApplicationServiceProvider.Assembler
{
    public class AutoMapperAssembler
    {
        public static CompanyFasecoldaCode CreateFasecoldaCode(VehicleFasecoldaCodeDTO vehicleFasecoldaCodeDTO)
        {
            var mapper = MappCompanyFasecoldaCode();
            return mapper.Map<VehicleFasecoldaCodeDTO, CompanyFasecoldaCode>(vehicleFasecoldaCodeDTO);
        }

        public static IMapper MappCompanyFasecoldaCode()
        {
            var config = MapperCache.GetMapper<CompanyFasecoldaCode, VehicleFasecoldaCodeDTO>(cfg =>
            {
                cfg.CreateMap<CompanyFasecoldaCode, VehicleFasecoldaCodeDTO>();
            });
            return config;
        }

        public static CompanyFasecoldaPrice CreateFasecoldaPrice(VehicleFasecoldaValueDTO vehicleFasecoldaValueDTO)
        {
            var mapper = MappCompanyFasecoldaPrice();
            return mapper.Map<VehicleFasecoldaValueDTO, CompanyFasecoldaPrice>(vehicleFasecoldaValueDTO);
        }

        public static IMapper MappCompanyFasecoldaPrice()
        {
            var config = MapperCache.GetMapper<CompanyFasecoldaPrice, VehicleFasecoldaValueDTO>(cfg =>
            {
                cfg.CreateMap<CompanyFasecoldaPrice, VehicleFasecoldaValueDTO>();
            });
            return config;
        }
    }
}
