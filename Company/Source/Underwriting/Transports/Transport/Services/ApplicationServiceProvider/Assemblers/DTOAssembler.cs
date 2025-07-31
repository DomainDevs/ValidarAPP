using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Transports.TransportBusinessService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
using Sistran.Company.Application.Transports.TransportApplicationService.EEProvider.Resources;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.UnderwritingServices;
using AutoMapper;
using Sistran.Company.Application.SarlaftBusinessServices.Models;
using Sistran.Company.Application.Transports.TransportBusinessService.Models;

namespace Sistran.Company.Application.Transports.TransportApplicationService.EEProvider.Assemblers
{
    public class DTOAssembler
    {
        /// <summary>
        /// Crea la informacion del individuo
        /// </summary>
        /// <param name="phonesList">listado de numeros de telefono</param>
        /// <param name="emailsList">listado de direcciones electronicas</param>
        /// <param name="addressesList">listado de direcciones</param>
        /// <returns>grupo de notificaciones NotificacionAddressDTO</returns>
        public static NotificationAddressDTO CreateNotificationInfo(List<Phone> phonesList, List<Email> emailsList, List<Address> addressesList)
        {
            return new NotificationAddressDTO
            {
                PhoneList = CreatePhones(phonesList),
                EmailList = CreateEmails(emailsList),
                AddressList = CreateAdresses(addressesList)
            };
        }

        /// <summary>
        /// Crea un numero telefonico
        /// </summary>
        /// <param name="phone">numero telefonico</param>
        /// <returns>un numero telefonicoDTO PhoneDTO</returns>
        public static PhoneDTO CreatePhone(Phone phone)
        {
            if (phone == null)
            {
                return null;
            }

            return new PhoneDTO
            {
                Id = phone.Id,
                PhoneTypeDescription = phone.PhoneType.Description,
                Phone = phone.Description,
                Checked = phone.IsMain
            };
        }

        /// <summary>
        /// Crea una lista de numeros telefonicos
        /// </summary>
        /// <param name="phones">listado de telefonos</param>
        /// <returns>lista de telefonos DTO List<PhoneDTO></returns>
        public static List<PhoneDTO> CreatePhones(List<Phone> phones)
        {
            List<PhoneDTO> phoneDTOs = new List<PhoneDTO>();

            foreach (var phone in phones)
            {
                phoneDTOs.Add(CreatePhone(phone));
            }

            return phoneDTOs;
        }

        /// <summary>
        /// Crea una direccion de correo electronico 
        /// </summary>
        /// <param name="email">direccion de correo electronico</param>
        /// <returns>una direccionDTO EmailDTO</returns>
        public static EmailDTO CreateEmail(Email email)
        {
            if (email == null)
            {
                return null;
            }

            return new EmailDTO
            {
                Id = email.Id,
                EmailTypeDescription = email.EmailType.Description,
                Email = email.Description,
                Checked = email.IsPrincipal
            };
        }

        /// <summary>
        /// Crea una Lista de direcciones de correo electronicos
        /// </summary>
        /// <param name="emails">listado de direcciones electronicas</param>
        /// <returns>lista de direcciones electronicasDTO List<EmailDTO></returns>
        public static List<EmailDTO> CreateEmails(List<Email> emails)
        {
            List<EmailDTO> emailDTOs = new List<EmailDTO>();

            foreach (var email in emails)
            {
                emailDTOs.Add(CreateEmail(email));
            }

            return emailDTOs;
        }

        /// <summary>
        /// Crea una direccion
        /// </summary>
        /// <param name="address">una direccion</param>
        /// <returns>una direccionDTO AddressDTO</returns>
        public static AddressDTO CreateAddress(Address address)
        {
            if (address == null)
            {
                return null;
            }

            return new AddressDTO
            {
                Id = address.Id,
                AddressTypeDescription = address.AddressType.Description,
                Address = address.Description,
                CityName = address.City.Description,
                CountryName = address.City.State.Country.Description,
                StateName = address.City.State.Description,
                Checked = address.IsPrincipal
            };
        }

        /// <summary>
        /// Crea una lista de direcciones
        /// </summary>
        /// <param name="addresses">listado de direcciones</param>
        /// <returns>Lista de direcionesDTO List<AddressDTO></returns>
        public static List<AddressDTO> CreateAdresses(List<Address> addresses)
        {
            List<AddressDTO> addressDTOs = new List<AddressDTO>();

            foreach (var address in addresses)
            {
                addressDTOs.Add(CreateAddress(address));
            }

            return addressDTOs;
        }

        /// <summary>
        /// Crea un tipo de beneficiario
        /// </summary>
        /// <param name="beneficiaryType">tipo de beneficiario</param>
        /// <returns>un tipo de beneficiarioDTO BeneficiaryTypeDTO</returns>
        public static BeneficiaryTypeDTO CreateBenefiarytype(CompanyBeneficiaryType beneficiaryType)
        {
            if (beneficiaryType == null)
            {
                return null;
            }

            return new BeneficiaryTypeDTO
            {
                Id = beneficiaryType.Id,
                Description = beneficiaryType.TinyDescription
            };
        }

        /// <summary>
        /// Crea una lista de tipos de beneficiarios
        /// </summary>
        /// <param name="beneficiaryTypes">listado de tipos de beneficiarios</param>
        /// <returns>Lista de tipos de beneficiarioDTO List<BeneficiaryTypeDTO></returns>
        public static List<BeneficiaryTypeDTO> CreateBenefiarytypes(List<CompanyBeneficiaryType> beneficiaryTypes)
        {
            List<BeneficiaryTypeDTO> beneficiaryTypeDTOs = new List<BeneficiaryTypeDTO>();

            foreach (var beneficiaryType in beneficiaryTypes)
            {
                beneficiaryTypeDTOs.Add(CreateBenefiarytype(beneficiaryType));
            }

            return beneficiaryTypeDTOs;
        }

        /// <summary>
        /// Crea un Transporte
        /// </summary>
        /// <param name="companyTransport">un transporte</param>
        /// <returns>un transporteDTO TransportDTO</returns>
        public static TransportDTO CreateTransport(CompanyTransport companyTransport)
        {
            if (companyTransport == null)
            {
                return null;
            }
            // Completar con los datos necesario al momento de integrar e implementar con el launcher
            return new TransportDTO
            {
                Id = companyTransport.Risk.Id,
                Description = companyTransport.Risk.Description,
                CargoTypeId = companyTransport.CargoType.Id,
                FromCityId = companyTransport.CityFrom.Id,
                FromCountryId = companyTransport.CityFrom.State.Country.Id,
                FromStateId = companyTransport.CityFrom.State.Id,
                ToCityId = companyTransport.CityTo.Id,
                ToCountryId = companyTransport.CityTo.State.Country.Id,
                ToStateId = companyTransport.CityTo.State.Id,
                CoverageGroupId = companyTransport.Risk.GroupCoverage.Id,
                Target = companyTransport.Destiny,
                Source = companyTransport.Source,
                ViaId = companyTransport.ViaType.Id,
                BillingPeriodId = companyTransport.AdjustPeriod.Id,
                MinimumPremium = companyTransport.MinimumPremium,
                RiskId = companyTransport.Risk.RiskId,
                PolicyId = (companyTransport.Risk.Policy == null) ? 0 : companyTransport.Risk.Policy.Id,
                TransportTypeIds = CreateTransportTypeIds(companyTransport.Types),
                DeclarationPeriodId = companyTransport.DeclarationPeriod.Id,
                DeclarationDescription = companyTransport.DeclarationPeriod.Description,
                TransportPackagingId = companyTransport.PackagingType.Id,
                PackagingDescription = companyTransport.PackagingType.Description,
                LimitMaxRealeaseAmount = companyTransport.LimitMaxReleaseAmount,
                ReleaseAmount = companyTransport.ReleaseAmount,
                FreightAmount = companyTransport.FreightAmount,
                ViaDescription = companyTransport.ViaType.Description,
                CargoDescription = companyTransport.CargoType.Description,
                TransportTypeDescriptions = CreateTransportTypeDescriptions(companyTransport.Types),
                RiskCommercialClassId = companyTransport.Risk.RiskActivity.Id,
                RiskCommercialClassDescription = companyTransport.Risk.RiskActivity.Description,
                HolderTypeId = companyTransport.HolderType.Id,
                IsFacultative = Convert.ToBoolean(companyTransport.Risk.IsFacultative),
                IsRetention = companyTransport.Risk.IsRetention,
                InsuredObjects = CreateCompanyInsuredObjects(companyTransport.InsuredObjects),
                Beneficiaries = CreateBeneficiaries(companyTransport.Risk.Beneficiaries),
                Clauses = CreateClauses(companyTransport.Risk.Clauses),
                Text = CreateText(companyTransport.Risk.Text),
                //----------------------------------------
                BirthDate = companyTransport.Risk.MainInsured.BirthDate,
                Gender = companyTransport.Risk.MainInsured.Gender,
                IndividualId = companyTransport.Risk.MainInsured.IndividualId,
                Name = companyTransport.Risk.MainInsured.Name,
                DocumentNumber = companyTransport.Risk.MainInsured.IdentificationDocument.Number,
                DocumentType = companyTransport.Risk.MainInsured.IdentificationDocument.DocumentType.Id,
                DocumentTypeDescription = companyTransport.Risk.MainInsured.IdentificationDocument.DocumentType.Description,
                DocumentTypeSmallDescription = companyTransport.Risk.MainInsured.IdentificationDocument.DocumentType.SmallDescription,
                //DocumentExpedition = companyTransport.Risk.MainInsured.IdentificationDocument.ExpeditionDate,
                Profile = companyTransport.Risk.MainInsured.Profile,
                ScoreCredit = companyTransport.Risk.MainInsured.ScoreCredit != null ? companyTransport.Risk.MainInsured.ScoreCredit.ScoreCreditId : 0,
                CustomerType = (int)companyTransport.Risk.MainInsured.CustomerType,
                CustomerTypeDescription = companyTransport.Risk.MainInsured.CustomerTypeDescription,
                IndividualType = (int)companyTransport.Risk.MainInsured.IndividualType,
                CompanyName = companyTransport.Risk.MainInsured.CompanyName.TradeName,
                NameNum = companyTransport.Risk.MainInsured.CompanyName.NameNum,
                Address = companyTransport.Risk.MainInsured.CompanyName.Address.Description,
                Phone = companyTransport.Risk.MainInsured.CompanyName.Phone != null ? companyTransport.Risk.MainInsured.CompanyName.Phone.Description : "",
                Email = companyTransport.Risk.MainInsured.CompanyName.Email != null ? companyTransport.Risk.MainInsured.CompanyName.Email.Description : "",
                IsMain = companyTransport.Risk.MainInsured.CompanyName.IsMain,
                InfringementPolicies = companyTransport.Risk.InfringementPolicies,
            };
        }

        /// <summary>
        /// Crea una lista de Transportes
        /// </summary>
        /// <param name="companyTransports">una lista de transporte</param>
        /// <returns>listado de transporteDTO List<TransportDTO></returns>
        public static List<TransportDTO> CreateTransports(List<CompanyTransport> companyTransports)
        {
            List<TransportDTO> transportDTOs = new List<TransportDTO>();

            foreach (var companyTransport in companyTransports)
            {
                transportDTOs.Add(CreateTransport(companyTransport));
            }

            return transportDTOs;
        }

        /// <summary>
        /// Crea un periodo de ajuste
        /// </summary>
        /// <param name="adjustPeriod">un periodo de ajuste</param>
        /// <returns>un periodo de ajuste BillingPeriodDTO</returns>
        public static BillingPeriodDTO CreateBillingPeriod(AdjustPeriod adjustPeriod)
        {
            if (adjustPeriod == null)
            {
                return null;
            }

            return new BillingPeriodDTO
            {
                Id = adjustPeriod.Id,
                Description = adjustPeriod.Description
            };
        }

        /// <summary>
        /// Crea una lista de periodos de ajustes
        /// </summary>
        /// <param name="adjustPeriods">lista de periodos de ajustes</param>
        /// <returns>listado de periodos de ajustesDTO BillingPeriodDTO</returns>
        public static List<BillingPeriodDTO> CreateBillingPeriods(List<AdjustPeriod> adjustPeriods)
        {
            List<BillingPeriodDTO> billingPeriodsDTO = new List<BillingPeriodDTO>();

            foreach (var billingPeriod in adjustPeriods)
            {
                billingPeriodsDTO.Add(CreateBillingPeriod(billingPeriod));
            }

            return billingPeriodsDTO;
        }

        /// <summary>
        /// Crea un objeto CountryDTO a partir de un modelo Country
        /// </summary>
        /// <param name="country">Modelo Countes</param>
        /// <returns>Objeto CountryDTO</returns>
        public static CountryDTO CreateCountry(Country country)
        {
            if (country == null)
            {
                return null;
            }

            return new CountryDTO
            {
                Id = country.Id,
                Description = country.Description
            };
        }

        /// <summary>
        /// Crea una lista de modelo Country a un listado de CountryDTO
        /// </summary>
        /// <param name="countries">Listado de modelos Country</param>
        /// <returns>Listado de países</returns>
        public static List<CountryDTO> CreateCountries(List<Country> countries)
        {
            List<CountryDTO> countriesDTO = new List<CountryDTO>();

            foreach (var country in countries)
            {
                countriesDTO.Add(CreateCountry(country));
            }

            return countriesDTO;
        }

        /// <summary>
        /// Crea un objeto StateDTO a partir de un State
        /// </summary>
        /// <param name="state">Modelo State</param>
        /// <returns>Estado</returns>
        public static StateDTO CreateState(State state)
        {
            if (state == null)
            {
                return null;
            }

            return new StateDTO
            {
                Id = state.Id,
                Description = state.Description
            };
        }

        /// <summary>
        /// Convierte una lista de modelo State a un conjunto de objetos StateDTO
        /// </summary>
        /// <param name="states">Modelos State</param>
        /// <returns>Un listado de estados</returns>
        public static List<StateDTO> CreateStates(List<State> states)
        {
            List<StateDTO> statesDTO = new List<StateDTO>();

            foreach (var state in states)
            {
                statesDTO.Add(CreateState(state));
            }

            return statesDTO;
        }

        /// <summary>
        /// Crea un tipo de mercancia
        /// </summary>
        /// <param name="cargoType">tipo de mercancia</param>
        /// <returns>tipo de mercanciaDTO CargoTypeDTO</returns>
        public static CargoTypeDTO CreateCargoType(CargoType cargoType)
        {
            if (cargoType == null)
            {
                return null;
            }

            return new CargoTypeDTO
            {
                Id = cargoType.Id,
                Description = cargoType.Description
            };
        }

        /// <summary>
        /// Crea una lista del tipo de mercancia
        /// </summary>
        /// <param name="cargoTypes">listado de tipos de mercancias</param>
        /// <returns>listado de tipo de mercanciasDTO List<CargoTypeDTO></returns>
        public static List<CargoTypeDTO> CreateCargoTypes(List<CargoType> cargoTypes)
        {
            List<CargoTypeDTO> cargoTypesDTO = new List<CargoTypeDTO>();

            foreach (var cargoType in cargoTypes)
            {
                cargoTypesDTO.Add(CreateCargoType(cargoType));
            }

            return cargoTypesDTO;
        }

        /// <summary>
        /// Crea un tipo de transporte
        /// </summary>
        /// <param name="transportType">tipo de transporte</param>
        /// <returns>un tipo de transporteDTO TransporttypeDTO</returns>
        public static TransportTypeDTO CreateTransportType(TransportType transportType)
        {
            if (transportType == null)
            {
                return null;
            }

            return new TransportTypeDTO
            {
                Id = transportType.Id,
                Description = transportType.Description
            };
        }

        /// <summary>
        /// Crea una lista del tipo de transporte
        /// </summary>
        /// <param name="transportTypes">listado de transporte</param>
        /// <returns>Listado de tipos de transporte List<TransportTypeDTO></returns>
        public static List<TransportTypeDTO> CreateTransportTypes(List<TransportType> transportTypes)
        {
            List<TransportTypeDTO> transportTypeDTO = new List<TransportTypeDTO>();

            foreach (var transportType in transportTypes)
            {
                transportTypeDTO.Add(CreateTransportType(transportType));
            }

            return transportTypeDTO;
        }

        /// <summary>
        /// Trae la lista del tipo de transporte
        /// </summary>
        /// <param name="transportTypes"></param>
        /// <returns>Listado de identificadores</returns>
        public static List<int> CreateTransportTypeIds(List<TransportType> transportTypes)
        {
            List<int> transportTypeDTO = new List<int>();

            foreach (var transportType in transportTypes)
            {
                transportTypeDTO.Add(transportType.Id);
            }

            return transportTypeDTO;
        }

        /// <summary>
        /// Trae la lista del tipo de transporte
        /// </summary>
        /// <param name="transportTypes"></param>
        /// <returns>Listado de identificadores</returns>
        public static List<string> CreateTransportTypeDescriptions(List<TransportType> transportTypes)
        {
            List<string> transportTypeDTO = new List<string>();

            foreach (var transportType in transportTypes)
            {
                transportTypeDTO.Add(transportType.Description);
            }

            return transportTypeDTO;
        }


        /// <summary>
        /// Trae la lista del tipo de transporte
        /// </summary>
        /// <param name="transportTypes"></param>
        /// <returns>Listado de identificadores</returns>

        /// <summary>
        /// Crea un texto
        /// </summary>
        /// <param name="text">un texto</param>
        /// <returns>un textoDTO TextDTO</returns>
        public static TextDTO CreateText(CompanyText companyText)
        {
            if (companyText == null)
            {
                return null;
            }

            return new TextDTO
            {
                Id = companyText.Id,
                TextTitle = companyText.TextTitle,
                TextBody = companyText.TextBody,
                Observations = companyText.Observations
            };
        }

        /// <summary>
        /// Crea una lista de texto
        /// </summary>
        /// <param name="companyTexts">listado de texto</param>
        /// <returns>listado de textoDTO List<TextDTO></returns>
        public static List<TextDTO> CreateTexts(List<CompanyText> companyTexts)
        {
            List<TextDTO> textDTO = new List<TextDTO>();

            foreach (var companyText in companyTexts)
            {
                textDTO.Add(CreateText(companyText));
            }

            return textDTO;
        }

        /// <summary>
        /// Crea un tipo de empaque
        /// </summary>
        /// <param name="packagingType">un tipo de empaque</param>
        /// <returns>un tipo de empaque PackagingTypeDTO</returns>
        public static PackagingTypeDTO CreatePackagingType(PackagingType packagingType)
        {
            if (packagingType == null)
            {
                return null;
            }

            return new PackagingTypeDTO
            {
                Id = packagingType.Id,
                Description = packagingType.Description
            };
        }

        /// <summary>
        /// Crea una lista de tipos de empaque
        /// </summary>
        /// <param name="packagingTypes">listado de tipo de empaques</param>
        /// <returns>listado de tipos de empaqueDTO List<PackagingTypeDTO></returns>
        public static List<PackagingTypeDTO> CreatePackagingTypes(List<PackagingType> packagingTypes)
        {
            List<PackagingTypeDTO> packagingTypeDTO = new List<PackagingTypeDTO>();

            foreach (var packagingType in packagingTypes)
            {
                packagingTypeDTO.Add(CreatePackagingType(packagingType));
            }

            return packagingTypeDTO;
        }

        /// <summary>
        /// Crea un tipo de vía
        /// </summary>
        /// <param name="viaType">tipo de via</param>
        /// <returns>un tipo de viaDTO ViaTypeDTO</returns>
        public static ViaTypeDTO CreateViaType(ViaType viaType)
        {
            if (viaType == null)
            {
                return null;
            }

            return new ViaTypeDTO
            {
                Id = viaType.Id,
                Description = viaType.Description
            };
        }

        /// <summary>
        /// Crea una lista de tipo vía
        /// </summary>
        /// <param name="viaTypes">listado de tipos de via</param>
        /// <returns>listado de tipos de viasDTO List<ViaTypeDTO></returns>
        public static List<ViaTypeDTO> CreateViaTypes(List<ViaType> viaTypes)
        {
            List<ViaTypeDTO> viaTypeDTO = new List<ViaTypeDTO>();

            foreach (var viaType in viaTypes)
            {
                viaTypeDTO.Add(CreateViaType(viaType));
            }

            return viaTypeDTO;
        }

        /// <summary>
        /// Crea un modelo CompanyCoverage a CoverageDTO
        /// </summary>
        /// <param name="coverage">Modelo CoompanyCoverage</param>
        /// <returns>Cobertura</returns>
        public static CoverageDTO CreateCoverageDTO(CompanyCoverage companyCoverage)
        {
            if (companyCoverage == null)
            {
                return null;
            }

            CoverageDTO coverageDTO = new CoverageDTO
            {
                Id = companyCoverage.Id,
                CurrentFrom = companyCoverage.CurrentFrom,
                CurrentTo = companyCoverage.CurrentTo,
                LimitAmount = (long)companyCoverage.LimitAmount,
                DeclaredAmount = (long)companyCoverage.DeclaredAmount,
                Description = companyCoverage.Description,
                SubLimitAmount = (long)companyCoverage.SubLimitAmount,
                MaxLiabilityAmount = (long)companyCoverage.MaxLiabilityAmount,
                LimitOccurrenceAmount = (long)companyCoverage.LimitOccurrenceAmount,
                LimitClaimantAmount = (long)companyCoverage.LimitClaimantAmount,
                CalculationTypeId = (int)companyCoverage.CalculationType,
                Rate = companyCoverage.Rate != null ? (decimal)companyCoverage.Rate : 0,
                RateTypeId = (int)companyCoverage.RateType,
                PremiumAmount = companyCoverage.PremiumAmount,
                IsDeclarative = companyCoverage.IsDeclarative,
                IsPrimary = companyCoverage.IsPrimary,
                DeductibleId = (companyCoverage.Deductible == null) ? 0 : companyCoverage.Deductible.Id,
                IsMandatory = companyCoverage.IsMandatory,
                IsSelected = companyCoverage.IsSelected,
                CoverStatus = (int)companyCoverage.CoverStatus,
                CoverStatusName = companyCoverage.CoverStatusName,
                SubLineBusiness = CreateSubLineBusiness(companyCoverage.SubLineBusiness),
                DepositPremiumPercent = companyCoverage.DepositPremiumPercent,
                IsMinPremiumDeposit = companyCoverage.IsMinPremiumDeposit,
                OriginalSubLimitAmount = companyCoverage.OriginalSubLimitAmount,
                OriginalLimitAmount = companyCoverage.OriginalLimitAmount,
                CurrentFromOriginal = companyCoverage.CurrentFromOriginal,
                CurrentToOriginal = companyCoverage.CurrentToOriginal,
                RuleSetId = companyCoverage.RuleSetId,
                PosRuleSetId = companyCoverage.PosRuleSetId,
                AllyCoverageId = companyCoverage.AllyCoverageId,
                MainCoverageId = (companyCoverage.AllyCoverageId != null && companyCoverage.AllyCoverageId > 0) ? null : companyCoverage.MainCoverageId,
                SublimitPercentage = companyCoverage.SublimitPercentage,
                InsuredObject = new InsuredObjectDTO
                {
                    Id = companyCoverage.InsuredObject.Id,
                    Description = companyCoverage.InsuredObject.Description,
                    IsMandatory = companyCoverage.InsuredObject.IsMandatory,
                    IsSelected = companyCoverage.InsuredObject.IsSelected,
                    PremiumAmount = companyCoverage.InsuredObject.Premium,
                    InsuredLimitAmount = companyCoverage.InsuredObject.Amount,
                    Rate = Convert.ToDecimal(companyCoverage.InsuredObject.Rate),
                    DepositPremiumPercentage = Convert.ToDecimal(companyCoverage.InsuredObject.DepositPremiunPercent),
                    RateTypeId = (int)companyCoverage.RateType
                },
                Number = companyCoverage.Number,
            };
            if (companyCoverage.Deductible != null)
            {
                var imapper = ModelAssembler.CreateMapDeductible();
                Deductible deductible = imapper.Map<CompanyDeductible, Deductible>(companyCoverage.Deductible);
                coverageDTO.Deductible = CreateDeductible(deductible);
            }
            if (companyCoverage.Text != null)
            {
                coverageDTO.Text = CreateText(companyCoverage.Text);
            }
            if (companyCoverage.Clauses != null && companyCoverage.Clauses.Count > 0)
            {
                coverageDTO.Clauses = CreateClauses(companyCoverage.Clauses);
            }
            return coverageDTO;
        }
        public static SubLineBusiness CreateSubLineBusiness(CompanySubLineBusiness companySubLineBusiness)
        {
            if (companySubLineBusiness == null)
            {
                return null;
            }
            return new SubLineBusiness
            {
                Id = companySubLineBusiness.Id,
                Description = companySubLineBusiness.Description,
                ExtendedProperties = companySubLineBusiness.ExtendedProperties,
                LineBusinessDescription = companySubLineBusiness.LineBusinessDescription,
                LineBusinessId = companySubLineBusiness.LineBusinessId,
                SmallDescription = companySubLineBusiness.SmallDescription,
                Status = companySubLineBusiness.Status,
                LineBusiness = new LineBusiness
                {
                    Description = companySubLineBusiness.LineBusiness.Description,
                    Status = companySubLineBusiness.LineBusiness.Status,
                    ExtendedProperties = companySubLineBusiness.LineBusiness.ExtendedProperties,
                    Id = companySubLineBusiness.LineBusiness.Id,
                    IdLineBusinessbyRiskType = companySubLineBusiness.LineBusiness.IdLineBusinessbyRiskType,
                    ListInsurectObjects = companySubLineBusiness.LineBusiness.ListInsurectObjects,
                    ReportLineBusiness = companySubLineBusiness.LineBusiness.ReportLineBusiness,
                    ShortDescription = companySubLineBusiness.LineBusiness.ShortDescription,
                    TyniDescription = companySubLineBusiness.LineBusiness.TyniDescription
                }
            };
        }

        internal static EndorsementDTO CreateEndorsement(CompanyPolicy companyPolicy)
        {
            return new EndorsementDTO
            {
                PolicyNumber = companyPolicy.DocumentNumber,
                Number = companyPolicy.Endorsement.Number
            };
        }

        /// <summary>
        /// Crea una lista de modelo CompanyCoverage a un conjunto de objetos CoverageDTO
        /// </summary>
        /// <param name="coverages">Listado de CompanyCoverage</param>
        /// <returns>Listado de coberturas</returns>
        public static List<CoverageDTO> CreateCoveragesDtos(List<CompanyCoverage> coverages)
        {
            List<CoverageDTO> coverageDTO = new List<CoverageDTO>();

            foreach (var coverage in coverages)
            {
                coverageDTO.Add(CreateCoverageDTO(coverage));
            }

            return coverageDTO;
        }

        /// <summary>
        /// Crea un periodo de declaracion
        /// </summary>
        /// <param name="declarationPeriod">un perido de declaracion</param>
        /// <returns> un perido de declaracionDTO DeclarationPeriodDTO</returns>
        public static DeclarationPeriodDTO CreateDeclarationPeriod(DeclarationPeriod declarationPeriod)
        {
            if (declarationPeriod == null)
            {
                return null;
            }

            return new DeclarationPeriodDTO
            {
                Id = declarationPeriod.Id,
                Description = declarationPeriod.Description
            };
        }

        /// <summary>
        /// Crea una lista de los periodos de declaracion
        /// </summary>
        /// <param name="declarationPeriods">listado de periodos de declaracion</param>
        /// <returns>listado de periodos de declaracion List<DeclarationPeriodDTO></returns>
        public static List<DeclarationPeriodDTO> CreateDeclarationPeriods(List<DeclarationPeriod> declarationPeriods)
        {
            List<DeclarationPeriodDTO> declarationPeriodDTO = new List<DeclarationPeriodDTO>();

            foreach (var declarationPeriod in declarationPeriods)
            {
                declarationPeriodDTO.Add(CreateDeclarationPeriod(declarationPeriod));
            }

            return declarationPeriodDTO;
        }

        /// <summary>
        /// Crea un Beneficiario
        /// </summary>
        /// <param name="beneficiary">un beneficiario</param>
        /// <returns>un beneficiarioDTO BeneficiaryDTO</returns>
        public static BeneficiaryDTO CreateBeneficiary(Beneficiary beneficiary)
        {
            if (beneficiary == null)
            {
                return null;
            }

            return new BeneficiaryDTO
            {
                IndividualId = beneficiary.IndividualId,
                Description = beneficiary.Name,
                BeneficiaryType = new BeneficiaryTypeDTO
                {
                    Description = beneficiary.BeneficiaryTypeDescription
                },
                Participation = beneficiary.Participation
            };
        }

        /// <summary>
        /// Crea un BeneficiaryDTO a partir de un objeto CompanyBeneficiary
        /// </summary>
        /// <param name="companyBeneficiary">un company beneficiario</param>
        /// <returns>un beneficiarioDTO BeneficiaryDTO</returns>
        public static BeneficiaryDTO CreateBeneficiary(CompanyBeneficiary companyBeneficiary)
        {
            string cardnumber = "null";
            if (companyBeneficiary == null)
            {
                return null;
            }
            if (companyBeneficiary.IdentificationDocument != null)
            {
                cardnumber = companyBeneficiary.IdentificationDocument.Number;
            };
            return new BeneficiaryDTO
            {
                IndividualId = companyBeneficiary.IndividualId,
                IndividualType = companyBeneficiary.IndividualType,
                Description = companyBeneficiary.BeneficiaryTypeDescription,
                BeneficiaryType = new BeneficiaryTypeDTO
                {
                    Description = companyBeneficiary.BeneficiaryTypeDescription,
                    Id = companyBeneficiary.BeneficiaryType.Id
                },
                Participation = companyBeneficiary.Participation,
                Name = companyBeneficiary.Name,
                CustomerType = companyBeneficiary.CustomerType,
                CardNumber = cardnumber
            };
        }

        /// <summary>
        /// Crea una lista de modelo Beneficiary a una lista de objetos BeneficiaryDTO
        /// </summary>
        /// <param name="beneficiaries">Listado de modelos Beneficiary</param>
        /// <returns>Un listado de Beneficiarios List<BeneficiaryDTO></returns>
        public static List<BeneficiaryDTO> CreateBeneficiaries(List<Beneficiary> beneficiaries)
        {
            List<BeneficiaryDTO> beneficiaryDTO = new List<BeneficiaryDTO>();

            foreach (var beneficiary in beneficiaries)
            {
                beneficiaryDTO.Add(CreateBeneficiary(beneficiary));
            }

            return beneficiaryDTO;
        }

        /// <summary>
        /// Crea una lista BeneficiaryDTO a partir de una lista CompanyBeneficiary
        /// </summary>
        /// <param name="companyBeneficiaries">listado de company beneficiarios</param>
        /// <returns>Un listado de Beneficiarios List<BeneficiaryDTO></returns>
        public static List<BeneficiaryDTO> CreateBeneficiaries(List<CompanyBeneficiary> companyBeneficiaries)
        {
            List<BeneficiaryDTO> beneficiaryDTO = new List<BeneficiaryDTO>();

            if (companyBeneficiaries.Count > 0 && companyBeneficiaries != null)
            {
                foreach (var companyBeneficiarie in companyBeneficiaries)
                {
                    beneficiaryDTO.Add(CreateBeneficiary(companyBeneficiarie));
                }
            }

            return beneficiaryDTO;
        }

        /// <summary>
        /// Crea un CityDTO a partir de un modelo City
        /// </summary>
        /// <param name="city">Modelo City</param>
        /// <returns>una cuidad CityDTO</returns>
        public static CityDTO CreateCity(City city)
        {
            if (city == null)
            {
                return null;
            }

            return new CityDTO
            {
                Id = city.Id,
                Description = city.Description
            };
        }

        /// <summary>
        /// Crea una lista de modelo City a un conjunto de objetos CityDTO
        /// </summary>
        /// <param name="cities">Listado de modelos City</param>
        /// <returns>Un listado de ciudades List<CityDTO></returns>
        public static List<CityDTO> CreateCities(List<City> cities)
        {
            List<CityDTO> citiesDTO = new List<CityDTO>();

            foreach (var city in cities)
            {
                citiesDTO.Add(CreateCity(city));
            }

            return citiesDTO;
        }

        /// <summary>
        /// Crea un DeductibleDTO a partir de un modelo CompanyDeductible
        /// </summary>
        /// <param name="deductible">Modelo CompanyDeductible</param>
        /// <returns>un Deducible DeductibleDTO</returns>
        public static DeductibleDTO CreateDeductible(Deductible companyDeductible)
        {
            if (companyDeductible == null)
            {
                return null;
            }

            return new DeductibleDTO
            {
                Id = companyDeductible.Id,
                Description = companyDeductible.Description,
                AccDeductAmt = companyDeductible.AccDeductAmt,
                Currency = companyDeductible.Currency != null ? new CurrencyDTO
                {
                    Id = companyDeductible.Currency.Id,
                    Description = companyDeductible.Currency.Description,
                    SmallDescription = companyDeductible.Currency.SmallDescription,
                    TinyDescription = companyDeductible.Currency.TinyDescription
                } : null,
                DeductValue = companyDeductible.DeductValue,
                MaxDeductValue = companyDeductible.MaxDeductValue,
                MinDeductValue = companyDeductible.MinDeductValue,
                DeductibleSubject = new DeductibleSubjectDTO
                {
                    Id = companyDeductible.DeductibleSubject.Id,
                    Description = companyDeductible.DeductibleSubject.Description
                },
                DeductibleUnit = new DeductibleUnitDTO
                {
                    Id = companyDeductible.DeductibleUnit.Id,
                    Description = companyDeductible.DeductibleUnit.Description
                },
                DeductPremiumAmount = companyDeductible.DeductPremiumAmount,
                IsDefault = companyDeductible.IsDefault,
                Rate = companyDeductible.Rate,
                RateType = companyDeductible.RateType,
                MinDeductibleSubject = new DeductibleSubjectDTO
                {
                    Id = companyDeductible.MinDeductibleSubject.Id,
                    Description = companyDeductible.MinDeductibleSubject.Description
                },

                MinDeductibleUnit = companyDeductible.MinDeductibleUnit != null ? new DeductibleUnitDTO
                {
                    Id = companyDeductible.MinDeductibleUnit.Id,
                    Description = companyDeductible.MinDeductibleUnit.Description
                } : null,
                MaxDeductibleSubject = companyDeductible.MaxDeductibleSubject != null ? new DeductibleSubjectDTO
                {
                    Id = companyDeductible.MaxDeductibleSubject.Id,
                    Description = companyDeductible.MaxDeductibleSubject.Description
                } : null,
                MaxDeductibleUnit = companyDeductible.MaxDeductibleUnit != null ? new DeductibleUnitDTO
                {
                    Id = companyDeductible.MaxDeductibleUnit.Id,
                    Description = companyDeductible.MaxDeductibleUnit.Description
                } : null
            };
        }

        /// <summary>
        /// Crea una lista de modelo CompanyDeductible a un conjunto de objetos Deductible
        /// </summary>
        /// <param name="deductibles">Listado de modelos CompanyDeductible</param>
        /// <returns>Un listado de deducibles List<DeductibleDTO></returns>
        public static List<DeductibleDTO> CreateDeductibles(List<Deductible> deductibles)
        {
            List<DeductibleDTO> deductiblesDTO = new List<DeductibleDTO>();

            foreach (var deductible in deductibles)
            {
                deductiblesDTO.Add(CreateDeductible(deductible));
            }

            return deductiblesDTO;
        }

        /// <summary>
        /// Crea un ClauseDTO a partir de un Companyclause unico
        /// </summary>
        /// <param name="companyClause">un company clausula</param>
        /// <returns>una clausula ClauseDTO</returns>
        public static ClauseDTO CreateClause(CompanyClause companyClause)
        {
            if (companyClause == null)
            {
                return null;
            }

            return new ClauseDTO
            {
                Id = companyClause.Id,
                Text = companyClause.Text,
                Name = companyClause.Name,
                IsMandatory = companyClause.IsMandatory
            };
        }

        /// <summary>
        /// Crea una Lista de clausulas del DTO
        /// </summary>
        /// <param name="companyClauses">listado de company clausulas</param>
        /// <returns>un listado de clausulasDTO List<ClauseDTO></returns>
        public static List<ClauseDTO> CreateClauses(List<CompanyClause> companyClauses)
        {
            List<ClauseDTO> clauseDTOs = new List<ClauseDTO>();
            if (companyClauses != null && companyClauses.Count > 0)
            {
                foreach (var companyClause in companyClauses)
                {
                    clauseDTOs.Add(CreateClause(companyClause));
                }
            }
            return clauseDTOs;
        }

        /// <summary>
        /// Crea un modelo PolicyType a un tipo de póliza
        /// </summary>
        /// <param name="policyType">Modelo PolicyType</param>
        /// <returns>un Tipo de póliza PolicyTypeDTO</returns>
        public static PolicyTypeDTO CreatePolicyType(PolicyType policyType)
        {
            if (policyType == null)
            {
                return null;
            }

            return new PolicyTypeDTO
            {
                Id = policyType.Id,
                Description = policyType.Description,
                IsFloating = policyType.IsFloating
            };
        }

        /// <summary>
        /// Crea una lista de tipos de polizas
        /// </summary>
        /// <param name="policyTypes">listado de tipos de polizas</param>
        /// <returns>listado de tipos de polizasDTO List<PolicyTypeDTO></returns>
        public static List<PolicyTypeDTO> CreatePolicyTypes(List<PolicyType> policyTypes)
        {
            List<PolicyTypeDTO> policyTypeDTOs = new List<PolicyTypeDTO>();

            foreach (var companyClause in policyTypes)
            {
                policyTypeDTOs.Add(CreatePolicyType(companyClause));
            }

            return policyTypeDTOs;
        }

        /// <summary>
        /// Crea un objeto InsuredObjectDTO a partir de un modelo InsuredObject
        /// </summary>
        /// <param name="insuredObject">Modelo insuredObject</param>
        /// <returns>Objeto InsuredObjectDTO</returns>
        public static InsuredObjectDTO CreateInsuredObject(InsuredObject InsuredObject)
        {
            if (InsuredObject == null)
            {
                return null;
            }

            return new InsuredObjectDTO
            {
                Id = InsuredObject.Id,
                Description = InsuredObject.Description,
                InsuredLimitAmount = InsuredObject.Amount,
                PremiumAmount = InsuredObject.Premium,
                IsSelected = InsuredObject.IsSelected,
                IsMandatory = InsuredObject.IsMandatory,
            };
        }

        /// <summary>
        /// Crea una lista de InsuredObjectDTO a partir de un modelo InsuredObject
        /// </summary>
        /// <param name="insuredObject">Modelo insuredObject</param>
        /// <returns>listado Objeto InsuredObjectDTO List<InsuredObjectDTO></returns>
        public static List<InsuredObjectDTO> CreateInsuredObjects(List<InsuredObject> InsuredObjects)
        {
            List<InsuredObjectDTO> insuredObjectDTO = new List<InsuredObjectDTO>();
            if (InsuredObjects != null)
            {
                foreach (var insuredObject in InsuredObjects)
                {
                    insuredObjectDTO.Add(CreateInsuredObject(insuredObject));
                }
            }
            return insuredObjectDTO;
        }

        /// <summary>
        /// Crea una covertura de grupo
        /// </summary>
        /// <param name="coverageGroup"> un grupo de coverturas</param>
        /// <returns>una grupo de coverturasDTO GroupCoverageDTO</returns>
        public static GroupCoverageDTO CreateCoverageGroup(GroupCoverage coverageGroup)
        {
            if (coverageGroup == null)
            {
                return null;
            }

            return new GroupCoverageDTO
            {
                Id = coverageGroup.Id,
                Description = coverageGroup.Description
            };
        }

        /// <summary>
        /// Crea una lista de covertura de grupo
        /// </summary>
        /// <param name="groupCoverages">listado de coverturas</param>
        /// <returns>listado de grupos de coverturasDTO List<GroupCoverageDTO></returns>
        public static List<GroupCoverageDTO> CreateCoverageGroups(List<GroupCoverage> groupCoverages)
        {
            List<GroupCoverageDTO> coverageGroupDTO = new List<GroupCoverageDTO>();

            foreach (var groupCoverage in groupCoverages)
            {
                coverageGroupDTO.Add(CreateCoverageGroup(groupCoverage));
            }

            return coverageGroupDTO;
        }

        /// <summary>
        /// Crea un objeto de tipo Insured a IndividualDetailsDTO
        /// </summary>
        /// <param name="insured">Modelo Insured</param>
        /// <returns>Objeto de tipo IndividualDetailsDTO</returns>
        public static IndividualDetailsDTO CreateIndividualDetails(IssuanceInsured insured)
        {
            if (insured == null)
            {
                return null;
            }

            return new IndividualDetailsDTO
            {
                Id = insured.InsuredId,
                Name = insured.CompanyName.TradeName,
                CodeNumber = insured.IdentificationDocument.Number,
                CodeId = insured.IdentificationDocument.DocumentType.Id,
                PhoneId = insured.CompanyName.Phone.Id,
                AddresssId = insured.CompanyName.Address.Id,
                EmailId = insured.CompanyName.Email.Id
            };
        }

        /// <summary>
        /// Crea una lista de IndividualDetailsDTO a partir de un listado de Insured
        /// </summary>
        /// <param name="insuredList">Lista de objetos Insured</param>
        /// <returns>Listado de Beneficiarios</returns>
        public static List<IndividualDetailsDTO> CreateIndividualDetailsList(List<IssuanceInsured> insuredList)
        {
            List<IndividualDetailsDTO> individualDetailsDTOs = new List<IndividualDetailsDTO>();

            foreach (var insured in insuredList)
            {
                individualDetailsDTOs.Add(DTOAssembler.CreateIndividualDetails(insured));
            }

            return individualDetailsDTOs;
        }

        /// <summary>
        /// Crea el listado de tipos de cálculos a partir del enumerador CalculationType
        /// </summary>
        /// <returns>Listado de tipos de cálculo</returns>
        public static List<SelectObjectDTO> CreateCalculationTypes()
        {
            List<SelectObjectDTO> calculationTypeDTOs = new List<SelectObjectDTO>();
            SelectObjectDTO selectObjectDTO;
            foreach (CalculationType calculationType in Enum.GetValues(typeof(CalculationType)))
            {
                selectObjectDTO = new SelectObjectDTO()
                {
                    Id = (int)calculationType,
                    Description = Errors.ResourceManager.GetString("List" + calculationType.ToString())
                };
                calculationTypeDTOs.Add(selectObjectDTO);
            }
            return calculationTypeDTOs;
        }

        /// <summary>
        /// Crea lista de tarifas
        /// </summary>
        /// <param name="rateTypes"></param>
        /// <returns></returns>
        public static List<SelectObjectDTO> CreateRateTypes()
        {
            List<SelectObjectDTO> rateTypeDTOs = new List<SelectObjectDTO>();
            SelectObjectDTO selectObjectDTO;
            foreach (RateType rateType in Enum.GetValues(typeof(RateType)))
            {
                selectObjectDTO = new SelectObjectDTO()
                {
                    Id = (int)rateType,
                    Description = Errors.ResourceManager.GetString("List" + rateType.ToString())
                };
                rateTypeDTOs.Add(selectObjectDTO);
            }
            return rateTypeDTOs;
        }

        /// <summary>
        /// Crea una lista de riesgos por endoso
        /// </summary>
        /// <param name="companyTransports">una lista de transporte</param>
        /// <returns>listado de transporteDTO List<TransportDTO></returns>
        public static List<TransportDTO> CreateTransportsByEndorsementId(List<CompanyTransport> companyTransports)
        {
            List<TransportDTO> transportDTOs = new List<TransportDTO>();

            foreach (var companyTransport in companyTransports)
            {
                transportDTOs.Add(CreateTransport(companyTransport));
            }

            return transportDTOs;
        }


        /// <summary>
        /// ream
        /// </summary>
        /// <param name="companyEndorsements"></param>
        /// <returns></returns>
        public static List<EndorsementDTO> CreateEndorsements(List<CompanyEndorsement> companyEndorsements)
        {
            List<EndorsementDTO> EndorsementDTOs = new List<EndorsementDTO>();

            foreach (var companyEndorsement in companyEndorsements)
            {
                EndorsementDTOs.Add(CreateEndorsementDTO(companyEndorsement));
            }

            return EndorsementDTOs;
        }
        public static EndorsementDTO CreateEndorsementDTO(CompanyEndorsement companyEndorsement)
        {
            if (companyEndorsement == null)
            {
                return null;
            }

            return new EndorsementDTO
            {
                IdEndorsement = companyEndorsement.Id,
                EndorsementType = companyEndorsement.EndorsementType,
                CurrentFrom = companyEndorsement.CurrentFrom,
                CurrentTo = companyEndorsement.CurrentTo,
                IsCurrent = companyEndorsement.IsCurrent,
                RiskId = Convert.ToInt32(companyEndorsement.RiskId)

            };
        }

        public static EndorsementDTO CreateEndorsementDto(Endorsement endorsement)
        {
            if (endorsement == null)
            {
                return null;
            }

            return new EndorsementDTO
            {
                TemporalId = endorsement.TemporalId,
                EndorsementType = endorsement.EndorsementType,
                IdEndorsement = endorsement.Id,
                CurrentFrom = endorsement.CurrentFrom,
                CurrentTo = endorsement.CurrentTo
            };
        }

        public static RiskCommercialClassDTO CreateRiskCommercialClass(CompanyRiskCommercialClass companyRiskCommercialClass)
        {
            return new RiskCommercialClassDTO
            {
                Id = companyRiskCommercialClass.Id,
                Description = companyRiskCommercialClass.Description,
                SmallDescription = companyRiskCommercialClass.SmallDescription
            };
        }

        public static List<RiskCommercialClassDTO> CreateRiskCommercialClasses(List<CompanyRiskCommercialClass> companyRiskCommercialClasses)
        {
            List<RiskCommercialClassDTO> companyRiskCommercialClassDTOs = new List<RiskCommercialClassDTO>();

            foreach (CompanyRiskCommercialClass companyRiskCommercialClass in companyRiskCommercialClasses)
            {
                companyRiskCommercialClassDTOs.Add(CreateRiskCommercialClass(companyRiskCommercialClass));
            }

            return companyRiskCommercialClassDTOs;
        }

        public static HolderTypeDTO CreateHolderType(CompanyHolderType companyHolderType)
        {
            return new HolderTypeDTO
            {
                Id = companyHolderType.Id,
                Description = companyHolderType.Description,
                SmallDescription = companyHolderType.SmallDescription
            };
        }

        public static List<HolderTypeDTO> CreateHolderTypes(List<CompanyHolderType> companyHolderTypes)
        {
            List<HolderTypeDTO> holderTypeDTOs = new List<HolderTypeDTO>();

            foreach (CompanyHolderType companyHolderType in companyHolderTypes)
            {
                holderTypeDTOs.Add(CreateHolderType(companyHolderType));
            }

            return holderTypeDTOs;
        }

        /// <summary>
        /// Crea una lista de InsuredObjectDTO a partir de un modelo InsuredObject
        /// </summary>
        /// <param name="insuredObject">Modelo insuredObject</param>
        /// <returns>listado Objeto InsuredObjectDTO List<InsuredObjectDTO></returns>
        public static List<InsuredObjectDTO> CreateCompanyInsuredObjects(List<CompanyInsuredObject> InsuredObjects)
        {
            List<InsuredObjectDTO> insuredObjectDTO = new List<InsuredObjectDTO>();
            if (InsuredObjects != null)
            {
                foreach (var insuredObject in InsuredObjects)
                {
                    insuredObjectDTO.Add(CreateCompanyInsuredObject(insuredObject));
                }
            }
            return insuredObjectDTO;
        }

        /// <summary>
        /// Crea un objeto InsuredObjectDTO a partir de un modelo InsuredObject
        /// </summary>
        /// <param name="insuredObject">Modelo insuredObject</param>
        /// <returns>Objeto InsuredObjectDTO</returns>
        public static InsuredObjectDTO CreateCompanyInsuredObject(CompanyInsuredObject InsuredObject)
        {
            if (InsuredObject == null)
            {
                return null;
            }

            return new InsuredObjectDTO
            {
                Id = InsuredObject.Id,
                Description = InsuredObject.Description,
                InsuredLimitAmount = InsuredObject.Amount,
                PremiumAmount = InsuredObject.Premium,
                IsSelected = InsuredObject.IsSelected,
                IsMandatory = InsuredObject.IsMandatory,
                DepositPremiumPercentage = Convert.ToDecimal(InsuredObject.DepositPremiunPercent),
                Rate = Convert.ToDecimal(InsuredObject.Rate)

            };
        }

        public static EndorsementPeriod CreateEndorsementPeriod(CompanyEndorsementPeriod companyEndorsementPeriod)
        {
            if (companyEndorsementPeriod == null)
            {
                return null;
            }
            return new EndorsementPeriod()
            {
                Id = companyEndorsementPeriod.Id,
                PolicyId = companyEndorsementPeriod.PolicyId,
                CurrentFrom = companyEndorsementPeriod.CurrentFrom,
                CurrentTo = companyEndorsementPeriod.CurrentTo,
                AdjustPeriod = companyEndorsementPeriod.AdjustPeriod,
                DeclarationPeriod = companyEndorsementPeriod.DeclarationPeriod,
                TotalAdjust = companyEndorsementPeriod.TotalAdjustment,
                TotalDeclaration = companyEndorsementPeriod.TotalDeclarations,
                Version = companyEndorsementPeriod.Version
            };



        }

    }
}
