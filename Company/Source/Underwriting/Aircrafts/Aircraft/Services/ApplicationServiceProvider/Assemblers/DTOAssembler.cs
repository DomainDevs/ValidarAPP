using Sistran.Company.Application.Aircrafts.AircraftApplicationService.DTOs;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Aircrafts.AircraftBusinessService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService.Models.Base;
using Sistran.Company.Application.Aircrafts.AircraftApplicationService.EEProvider.Resources;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.UnderwritingServices;
using AutoMapper;

namespace Sistran.Company.Application.Aircrafts.AircraftApplicationService.EEProvider.Assemblers
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
                Id = beneficiaryType.Id.ToString(),
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
        /// Crea un Aircrafte
        /// </summary>
        /// <param name="companyAircraft">un Aircrafte</param>
        /// <returns>un AircrafteDTO AircraftDTO</returns>
        public static AircraftDTO CreateAircraft(CompanyAircraft companyAircraft)
        {
            if (companyAircraft == null)
            {
                return null;
            }

            // Completar con los datos necesario al momento de integrar e implementar con el launcher
            return new AircraftDTO
            {

                Id = companyAircraft.Risk.Id,
                Description = companyAircraft.Risk.Description,
                MakeId = companyAircraft.MakeId,
                ModelId = companyAircraft.ModelId,
                TypeId = companyAircraft.TypeId,
                UseId = companyAircraft.UseId,
                RegisterId = companyAircraft.RegisterId,
                CurrentManufacturing = companyAircraft.CurrentManufacturing,
                OperatorId = companyAircraft.OperatorId,
                NumberRegister = companyAircraft.NumberRegister,
                CoverageGroupId = companyAircraft.Risk.GroupCoverage.Id,
                RiskId = companyAircraft.Risk.RiskId,
                PolicyId = (companyAircraft.Risk.Policy == null) ? 0 : companyAircraft.Risk.Policy.Id,
                InsuredObjects = CreateInsuredObjects(companyAircraft.InsuredObjects),
                MainInsured = companyAircraft.Risk.MainInsured.Name,
                Risk = companyAircraft.Risk
            };
        }

        /// <summary>
        /// Crea una lista de Aircraftes
        /// </summary>
        /// <param name="companyAircrafts">una lista de Aircrafte</param>
        /// <returns>listado de AircrafteDTO List<AircraftDTO></returns>
        public static List<AircraftDTO> CreateAircrafts(List<CompanyAircraft> companyAircrafts)
        {
            List<AircraftDTO> AircraftDTOs = new List<AircraftDTO>();

            foreach (var companyAircraft in companyAircrafts)
            {
                AircraftDTOs.Add(CreateAircraft(companyAircraft));
            }

            return AircraftDTOs;
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
        /// Crea un tipo de Aircrafte
        /// </summary>
        /// <param name="AircraftType">tipo de Aircrafte</param>
        /// <returns>un tipo de AircrafteDTO AircrafttypeDTO</returns>
        public static AircraftTypeDTO CreateAircraftType(AircraftType AircraftType)
        {
            if (AircraftType == null)
            {
                return null;
            }

            return new AircraftTypeDTO
            {
                Id = AircraftType.Id,
                Description = AircraftType.Description
            };
        }

        /// <summary>
        /// Crea una lista del tipo de Aircrafte
        /// </summary>
        /// <param name="AircraftTypes">listado de Aircrafte</param>
        /// <returns>Listado de tipos de Aircrafte List<AircraftTypeDTO></returns>
        public static List<AircraftTypeDTO> CreateAircraftTypes(List<AircraftType> AircraftTypes)
        {
            List<AircraftTypeDTO> AircraftTypeDTO = new List<AircraftTypeDTO>();

            foreach (var AircraftType in AircraftTypes)
            {
                AircraftTypeDTO.Add(CreateAircraftType(AircraftType));
            }

            return AircraftTypeDTO;
        }
        public static UseDTO CreateAircraftUse(Use AircraftUse)
        {
            if (AircraftUse == null)
            {
                return null;
            }

            return new UseDTO
            {
                Id = AircraftUse.Id,
                Description = AircraftUse.Description
            };
        }

        /// <summary>
        /// Crea una lista del uso de Aircrafte
        /// </summary>
        /// <param name="AircraftTypes">listado de Aircrafte</param>
        /// <returns>Listado de tipos de Aircrafte List<AircraftTypeDTO></returns>
        public static List<UseDTO> CreateAircraftUses(List<Use> AircraftUses)
        {
            List<UseDTO> useDTOs = new List<UseDTO>();

            foreach (var AircraftUse in AircraftUses)
            {
                useDTOs.Add(CreateAircraftUse(AircraftUse));
            }

            return useDTOs;
        }
        /// <summary>
        /// Trae la lista del tipo de Aircrafte
        /// </summary>
        /// <param name="AircraftTypes"></param>
        /// <returns>Listado de identificadores</returns>
        public static List<int> CreateAircraftTypeIds(List<AircraftType> AircraftTypes)
        {
            List<int> AircraftTypeDTO = new List<int>();

            foreach (var AircraftType in AircraftTypes)
            {
                AircraftTypeDTO.Add(AircraftType.Id);
            }

            return AircraftTypeDTO;
        }

        /// <summary>
        /// Trae la lista del tipo de Aircrafte
        /// </summary>
        /// <param name="AircraftTypes"></param>
        /// <returns>Listado de identificadores</returns>
        public static List<string> CreateAircraftTypeDescriptions(List<AircraftType> AircraftTypes)
        {
            List<string> AircraftTypeDTO = new List<string>();

            foreach (var AircraftType in AircraftTypes)
            {
                AircraftTypeDTO.Add(AircraftType.Description);
            }

            return AircraftTypeDTO;
        }

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
        /// Crea un modelo CompanyCoverage a CoverageDTO
        /// </summary>
        /// <param name="coverage">Modelo CoompanyCoverage</param>
        /// <returns>Cobertura</returns>
        public static CoverageDTO CreateCoverage(CompanyCoverage coverage)
        {
            if (coverage == null)
            {
                return null;
            }

            return new CoverageDTO
            {
                Id = coverage.Id,
                CurrentFrom = coverage.CurrentFrom,
                CurrentTo = coverage.CurrentTo,
                LimitAmount = (long)coverage.LimitAmount,
                DeclaredAmount = (long)coverage.DeclaredAmount,
                Description = coverage.Description,
                SubLimitAmount = (long)coverage.SubLimitAmount,
                MaxLiabilityAmount = (long)coverage.MaxLiabilityAmount,
                LimitOccurrenceAmount = (long)coverage.LimitOccurrenceAmount,
                LimitClaimantAmount = (long)coverage.LimitClaimantAmount,
                CalculationTypeId = (int)coverage.CalculationType,
                Rate = (long)coverage.Rate,
                RateTypeId = (int)coverage.RateType,
                PremiumAmount = coverage.PremiumAmount,
                IsDeclarative = coverage.IsDeclarative,
                IsPrimary = coverage.IsPrimary,
                DeductibleId = (coverage.Deductible == null) ? 0 : coverage.Deductible.Id,
                IsMandatory = coverage.IsMandatory,
                IsSelected = coverage.IsSelected,
                CoverStatus = (int)coverage.CoverStatus,
                CoverStatusName = Errors.ResourceManager.GetString(coverage.CoverStatus.ToString()),
                SubLineBusiness = CreateSubLineBusiness(coverage.SubLineBusiness),
                DepositPremiumPercent = coverage.DepositPremiumPercent,
                IsMinPremiumDeposit = coverage.IsMinPremiumDeposit,
                OriginalLimitAmount = coverage.OriginalLimitAmount,
                OriginalSubLimitAmount = coverage.OriginalSubLimitAmount,
                CurrentFromOriginal = coverage.CurrentFromOriginal,
                CurrentToOriginal = coverage.CurrentToOriginal,
                InsuredObject = new InsuredObjectDTO
                {
                    Id = coverage.InsuredObject.Id,
                    Description = coverage.InsuredObject.Description,
                    IsMandatory = coverage.InsuredObject.IsMandatory,
                    IsSelected = coverage.InsuredObject.IsSelected,
                    PremiumAmount = coverage.InsuredObject.Premium,
                    InsuredLimitAmount = coverage.InsuredObject.Amount
                },
                Number = coverage.Number
            };
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
        public static List<CoverageDTO> CreateCoverages(List<CompanyCoverage> coverages)
        {
            List<CoverageDTO> coverageDTO = new List<CoverageDTO>();

            foreach (var coverage in coverages)
            {
                coverageDTO.Add(CreateCoverage(coverage));
            }

            return coverageDTO;
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
                Id = beneficiary.IndividualId,
                Description = beneficiary.Name,
                BeneficiaryType = new BeneficiaryTypeDTO
                {
                    Description = beneficiary.BeneficiaryTypeDescription
                },
                ParticipationPercent = beneficiary.Participation
            };
        }

        /// <summary>
        /// Crea un BeneficiaryDTO a partir de un objeto CompanyBeneficiary
        /// </summary>
        /// <param name="companyBeneficiary">un company beneficiario</param>
        /// <returns>un beneficiarioDTO BeneficiaryDTO</returns>
        public static BeneficiaryDTO CreateBeneficiary(CompanyBeneficiary companyBeneficiary)
        {
            if (companyBeneficiary == null)
            {
                return null;
            }

            return new BeneficiaryDTO
            {
                Id = companyBeneficiary.IndividualId,
                Description = companyBeneficiary.BeneficiaryTypeDescription,
                BeneficiaryType = new BeneficiaryTypeDTO
                {
                    Description = companyBeneficiary.BeneficiaryTypeDescription
                },
                ParticipationPercent = companyBeneficiary.Participation
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

            foreach (var companyBeneficiarie in companyBeneficiaries)
            {
                beneficiaryDTO.Add(CreateBeneficiary(companyBeneficiarie));
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
        public static DeductibleDTO CreateDeductible(Deductible deductible)
        {
            if (deductible == null)
            {
                return null;
            }

            return new DeductibleDTO
            {
                Id = deductible.Id,
                Description = deductible.Description
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

            foreach (var companyClause in companyClauses)
            {
                clauseDTOs.Add(CreateClause(companyClause));
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

            foreach (var insuredObject in InsuredObjects)
            {
                insuredObjectDTO.Add(CreateInsuredObject(insuredObject));
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
        /// <param name="companyAircrafts">una lista de Aircrafte</param>
        /// <returns>listado de AircrafteDTO List<AircraftDTO></returns>
        public static List<AircraftDTO> CreateAircraftsByEndorsementId(List<CompanyAircraft> companyAircrafts)
        {
            List<AircraftDTO> AircraftDTOs = new List<AircraftDTO>();

            foreach (var companyAircraft in companyAircrafts)
            {
                AircraftDTOs.Add(CreateAircraft(companyAircraft));
            }

            return AircraftDTOs;
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

        /// <summary>
        /// Crea lista de Marcas
        /// </summary>
        /// <param name="rateTypes"></param>
        /// <returns></returns>
        public static List<MakeDTO> CreateMakes(List<Make> makes)
        {
            List<MakeDTO> makeDTO = new List<MakeDTO>();

            foreach (var make in makes)
            {
                makeDTO.Add(CreateMake(make));

            }

            return makeDTO;
        }

        /// <summary>
        /// Crea lista de marcas por id
        /// </summary>
        /// <param name="viaType">tipo de via</param>
        /// <returns>un tipo de viaDTO ViaTypeDTO</returns>
        public static MakeDTO CreateMake(Make make)
        {
            if (make == null)
            {
                return null;
            }

            return new MakeDTO
            {
                Id = make.Id,
                Description = make.Description
            };
        }

        public static List<ModelDTO> CreateModelsByMakesId(List<Model> modelsByMakes)
        {
            List<ModelDTO> modelByMakeIdDTO = new List<ModelDTO>();

            foreach (var modelByMakeId in modelsByMakes)
            {
                modelByMakeIdDTO.Add(CreateModelByMake(modelByMakeId));

            }

            return modelByMakeIdDTO;
        }
        /// <summary>
        /// Crea una marca por Id
        /// </summary>
        /// <param name="modelByMake"></param>
        /// <returns></returns>
        public static ModelDTO CreateModelByMake(Model modelByMake)
        {
            if (modelByMake == null)
            {
                return null;
            }

            return new ModelDTO
            {
                Id = modelByMake.Id,
                Description = modelByMake.Description
            };
        }

        /// <summary>
        /// Crea una lista de operadores de grupo
        /// </summary>
        /// <param name="operators">listado de operadores</param>
        /// <returns>listado de grupos de OperatorDTO List<GroupCoverageDTO></returns>
        public static List<OperatorDTO> CreateOperators(List<Operator> operators)
        {
            List<OperatorDTO> operatorDTO = new List<OperatorDTO>();

            foreach (var Operator in operators)
            {
                operatorDTO.Add(CreateOperator(Operator));
            }

            return operatorDTO;
        }

        /// <summary>
        /// Crea un operador de grupo
        /// </summary>
        /// <param name="operators"> un grupo de operadores</param>
        /// <returns>una grupo de operadoresDTO OperatorDTO</returns>
        public static OperatorDTO CreateOperator(Operator operators)
        {
            if (operators == null)
            {
                return null;
            }

            return new OperatorDTO
            {
                Id = operators.Id,
                Description = operators.Description
            };
        }

        /// <summary>
        /// Crea una lista de matriculas
        /// </summary>
        /// <param name="registers">listado de matriculas</param>
        /// <returns>listado de grupos de RegisterDTO List</returns>
        public static List<RegisterDTO> CreateRegisters(List<Register> registers)
        {
            List<RegisterDTO> registerDTO = new List<RegisterDTO>();

            foreach (var register in registers)
            {
                registerDTO.Add(CreateRegister(register));
            }

            return registerDTO;
        }
        /// <summary>
        /// Crea una matricula
        /// </summary>
        /// <param name="registers">un grupo de matriculas</param>
        /// <returns>un grupo de RegisterDTO</returns>
        public static RegisterDTO CreateRegister(Register registers)
        {
            if (registers == null)
            {
                return null;
            }

            return new RegisterDTO
            {
                Id = registers.Id,
                Description = registers.Description
            };
        }


        /// <summary>
        /// Crea listado de usos por ramo comercial
        /// </summary>
        /// <param name="usesByPrefixId">Listado de usos por ramo comercial</param>
        /// <returns>Listado de usos por usesByPrefixIdDTO</returns>
        public static List<UseDTO> CreateUsesByPrefixId(List<UsePrefix> usesByPrefixId)
        {
            List<UseDTO> usesByPrefixIdDTO = new List<UseDTO>();

            foreach (var useByPrefixId in usesByPrefixId)
            {
                usesByPrefixIdDTO.Add(CreateUseByPrefixId(useByPrefixId));
            }

            return usesByPrefixIdDTO;
        }

        /// <summary>
        /// Crea uso por ramo comercial
        /// </summary>
        /// <param name="usesByPrefixId">Uso por ramo comercial</param>
        /// <returns>Ramo de UseDTO</returns>
        public static UseDTO CreateUseByPrefixId(UsePrefix usesByPrefixId)
        {
            if (usesByPrefixId == null)
            {
                return null;
            }

            return new UseDTO
            {
                Id = usesByPrefixId.Id,
                Description = usesByPrefixId.Description
            };
        }
    }
}
