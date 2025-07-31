using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using CiaPersonModel = Sistran.Company.Application.UniquePersonServices.V1.Models;
using UPMB = Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using AutoMapper;
using CVEMO = Sistran.Company.Application.Vehicles.Models;
using Sistran.Core.Application.Vehicles.Models;
using VEMO = Sistran.Core.Application.Vehicles.Models;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using CVM = Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;

namespace Sistran.Company.Application.Vehicles.MassiveThirdPartyLiabilityServices.EEProvider.Assemblers
{
    public class AutoMapperAssembler
    {
        #region Automapper
        #region Insured
        public static IMapper CreateMapInsured()
        {
            var config = MapperCache.GetMapper<CompanyInsured, CompanyIssuanceInsured>(cfg =>
            {
                cfg.CreateMap<CompanyInsured, CompanyIssuanceInsured>();
                cfg.CreateMap<CompanyName, IssuanceCompanyName>();
                cfg.CreateMap<Address, IssuanceAddress>();
                cfg.CreateMap<Phone, IssuancePhone>();
                cfg.CreateMap<Email, IssuanceEmail>();
                cfg.CreateMap<IndividualPaymentMethod, CiaPersonModel.CiaIndividualPaymentMethod>();
                cfg.CreateMap<UPMB.BaseIndividualPaymentMethod, BaseIndividualPaymentMethod>();
                cfg.CreateMap<EconomicActivity, BaseEconomicActivity>();
            });
            return config;
        }
        #endregion

        #region Model
        public static IMapper CreateMapModel()
        {
            var config = MapperCache.GetMapper<Make, CVEMO.CompanyMake>(cfg =>
            {
                cfg.CreateMap<Model, CVEMO.CompanyModel>();
                cfg.CreateMap<Make, CVEMO.CompanyMake>();
            });
            return config;
        }
        #endregion

        #region Version
        public static IMapper CreateMapVersion()
        {
            var config = MapperCache.GetMapper<VEMO.Version, CVEMO.CompanyVersion>(cfg =>
            {
                cfg.CreateMap<VEMO.Version, CVEMO.CompanyVersion>();
                cfg.CreateMap<VEMO.Model, CVEMO.CompanyModel>();
                cfg.CreateMap<VEMO.Make, CVEMO.CompanyMake>();
                cfg.CreateMap<VEMO.Type, CVEMO.CompanyType>();
                cfg.CreateMap<VEMO.Fuel, CVEMO.CompanyFuel>();
                cfg.CreateMap<VEMO.Engine, CVEMO.CompanyEngine>();
                cfg.CreateMap<VEMO.EngineType, CVEMO.CompanyEngineType>();
                cfg.CreateMap<VEMO.TransmissionType, CVEMO.CompanyTransmissionType>();
                cfg.CreateMap<VEMO.ServiceType, CVEMO.CompanyServiceType>();
                cfg.CreateMap<VEMO.Body, CVEMO.CompanyBody>();
            });
            return config;
        }
        #endregion

        #region Fasecolda
        public static IMapper CreateMapFasecolda()
        {
            var config = MapperCache.GetMapper<CVM.Fasecolda, Fasecolda>(cfg =>
            {
                cfg.CreateMap<CVM.Fasecolda, Fasecolda>();
            });
            return config;
        }
        #endregion
        #region Componentes
        public static IMapper CreateMapCompanyComponentValueDTO()
        {
            IMapper config = MapperCache.GetMapper<CompanySummary, ComponentValueDTO>(cfg =>
            {
                cfg.CreateMap<CompanySummary, ComponentValueDTO>();
            });
            return config;
        }
        #endregion Componentes
        #endregion
    }
}