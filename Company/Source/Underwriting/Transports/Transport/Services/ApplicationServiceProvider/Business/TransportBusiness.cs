using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;
using Sistran.Company.Application.Transports.TransportApplicationService.EEProvider.Assemblers;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.Transports.TransportApplicationService.EEProvider.Business
{
    public class TransportBusiness
    {
        /// <summary>
        /// Trae del delegado los periodos de ajustes 
        /// </summary>
        /// <returns></returns>
        public List<BillingPeriodDTO> GetBillingPeriods()
        {
            return DTOAssembler.CreateBillingPeriods(DelegateService.transportBusinessService.GetAdjustPeriods());
        }

        /// <summary>
        /// Obtener una lista de ciudades a partir del país y el departamento
        /// </summary>
        /// <param name="countryId">Identificador del país</param>
        /// <param name="stateId">Identificador del estado (departamento)</param>
        /// <returns>Listado de ciudades</returns>
        public List<CityDTO> GetCitiesByContryIdStateId(int countryId, int stateId)
        {
            return DTOAssembler.CreateCities(DelegateService.commonService.GetCitiesByCountryIdStateId(countryId, stateId));
        }

        /// <summary>
        /// Retorna una búsqueda de beneficiarios por descripción
        /// </summary>
        /// <param name="description">Texto de búsqueda</param>
        /// <param name="insuredSearchType">Insured search type</param>
        /// <param name="custormerType">Customer type</param>
        /// <returns>Listado de beneficiarios</returns>
        public List<CompanyBeneficiary> GetBeneficiariesByDescriptionInsuredSearchTypeCustomer(string description, InsuredSearchType insuredSearchType, CustomerType custormerType)
        {
            return null;
            //DelegateService.underwritingService.GetBeneficiariesByDescription(description, insuredSearchType);
        }

        /// <summary>
        /// Retorna un listado de tipos de beneficiarios
        /// </summary>
        /// <returns>Listado de tipos de beneficiarios</returns>
        public List<BeneficiaryTypeDTO> GetBeneficiaryTypes()
        {
            return DTOAssembler.CreateBenefiarytypes(DelegateService.underwritingService.GetCompanyBeneficiaryTypes());
        }

        /// <summary>
        /// Trae del delegado el tipo de mercancia
        /// </summary>
        /// <returns></returns>
        public List<CargoTypeDTO> GetCargoTypes()
        {
            return DTOAssembler.CreateCargoTypes(DelegateService.transportBusinessService.GetCargoTypes());
        }


        /// <summary>
        /// Obtener listado de países
        /// </summary>
        /// <returns>Listado de países</returns>
        public List<CountryDTO> GetCountries()
        {
            return DTOAssembler.CreateCountries(DelegateService.commonService.GetCountriesLite());
        }

        /// <summary>
        /// Trae con el delegado los periodos declarados
        /// </summary>
        /// <returns></returns>
        public List<DeclarationPeriodDTO> GetDeclarationPeriods()
        {
            return DTOAssembler.CreateDeclarationPeriods(DelegateService.transportBusinessService.GetDeclarationPeriods());
        }

        /// <summary>
        /// Trae con el delegado el tipo de transporte
        /// </summary>
        /// <returns></returns>
        public List<TransportTypeDTO> GetTransportTypes()
        {
            return DTOAssembler.CreateTransportTypes(DelegateService.transportBusinessService.GetTransportTypes());
        }

        /// <summary>
        /// Retorna el listado de tipos de empaque
        /// </summary>
        /// <returns>Listado de tipos de empaque</returns>
        public List<PackagingTypeDTO> GetPackagingTypes()
        {
            return DTOAssembler.CreatePackagingTypes(DelegateService.transportBusinessService.GetPackagingTypes());
        }

        /// <summary>
        /// Obtiene un listado de departamentos a partir del identificador del país
        /// </summary>
        /// <param name="countryId">Identificador del país</param>
        /// <returns>Listado de departamentos</returns>
        public List<StateDTO> GetStatesByCountryId(int countryId)
        {
            return DTOAssembler.CreateStates(DelegateService.commonService.GetStatesByCountryId(countryId));
        }

        /// <summary>
        /// Trae el transporte por temporal ID
        /// </summary>
        /// <returns></returns>
        public List<TransportDTO> GetTransportsByTemporalId(int temporalId)
        {
            return DTOAssembler.CreateTransports(DelegateService.transportBusinessService.GetCompanyTransportsByTemporalId(temporalId));
        }

        /// <summary>
        /// Obtiene un listado de tipos de vía
        /// </summary>
        /// <returns>Listado de tipos de vía</returns>
        public List<ViaTypeDTO> GetViaTypes()
        {
            return DTOAssembler.CreateViaTypes(DelegateService.transportBusinessService.GetViaTypes());
        }

        /// <summary>
        /// Retorna una búsqueda de indiviuos por descripción
        /// </summary>
        /// <param name="description">Texto de búsqueda</param>
        /// <param name="insuredSearchType">Insured search type</param>
        /// <param name="customerType">customer type</param>
        /// <returns>Listado de individuos</returns>
        public List<IndividualDetailsDTO> GetInsuredByDescriptionInsuredSearchTypeCustomer(string description, InsuredSearchType insuredSearchType, CustomerType customerType)
        {
            return DTOAssembler.CreateIndividualDetailsList(
                DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(
                    description, insuredSearchType, customerType));
        }

        /// <summary>
        /// Obtiene la información de contacto para un indiviudo
        /// </summary>
        /// <param name="individualId">Idenitificador del individuo</param>
        /// <returns>Información de contacto: direcciones, teléfonos y correos electrónicos</returns>
        public NotificationAddressDTO GetNotificationAddressesByIndividualId(int individualId)
        {
            return DTOAssembler.CreateNotificationInfo(
                DelegateService.uniquePersonService.GetPhonesByIndividualId(individualId),
                DelegateService.uniquePersonService.GetEmailsByIndividualId(individualId),
                DelegateService.uniquePersonService.GetAddressesByIndividualId(individualId));
        }

        /// <summary>
        /// Return un listado de objetos de seguro filtrados por identificador de
        ///     producto y grupo de cobertura
        /// </summary>
        /// <param name="productId">Identificador del producto</param>
        /// <param name="groupCoverageId">Identificador del grupo de cobertura</param>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <returns>Listado de objetos de seguro</returns>
        public List<InsuredObjectDTO> GetInsuredObjectsByProductIdGroupCoverageId(int productId, int groupCoverageId, int prefixId)
        {
            return DTOAssembler.CreateInsuredObjects(DelegateService.underwritingService.GetInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId));
        }


        /// <summary>
        /// Hace el guardado de un riegos transporte
        /// </summary>
        /// <param name="transportDTO">Modelo de transporte</param>
        /// <returns>Modelo de transporte</returns>
        public TransportDTO SaveTransport(int temporalId, TransportDTO transportDTO)
        {
            CompanyTransport companyTransport = new CompanyTransport();
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
            List<TransportDTO> CompanyTransportRisks = GetTransportsByTemporalId(transportDTO.PolicyId);
            if (transportDTO.Id.GetValueOrDefault() == 0)
            {
                companyTransport = ModelAssembler.CreateTransport(transportDTO, companyTransport);
                companyTransport.Risk.Policy = companyPolicy;
                if (companyPolicy.Endorsement.EndorsementType == EndorsementType.Emission)
                {
                    companyTransport.Risk.Status = RiskStatusType.Original;
                }
                companyTransport.Risk.Policy.PolicyType = new CompanyPolicyType
                {
                    IsFloating = transportDTO.IsFloating
                };

                companyTransport.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                if (companyPolicy.DefaultBeneficiaries != null && companyPolicy.DefaultBeneficiaries.Count > 0)
                {
                    companyTransport.Risk.Beneficiaries = companyPolicy.DefaultBeneficiaries;
                }
                else
                {
                    var companyBeneficiaryType = DelegateService.underwritingService.GetCompanyBeneficiaryTypes();
                    companyTransport.Risk.Beneficiaries.Add(
                    new CompanyBeneficiary
                    {
                        IndividualId = companyTransport.Risk.MainInsured.IndividualId,
                        IndividualType = companyTransport.Risk.MainInsured.IndividualType,
                        BeneficiaryType = new CompanyBeneficiaryType { Id = KeySettings.OnerousBeneficiaryTypeId, SmallDescription = companyBeneficiaryType.First(x => x.Id == KeySettings.OnerousBeneficiaryTypeId).SmallDescription },
                        BeneficiaryTypeDescription = companyBeneficiaryType.First(x => x.Id == KeySettings.OnerousBeneficiaryTypeId).SmallDescription,
                        Participation = 100,
                        IdentificationDocument = new IssuanceIdentificationDocument
                        {
                            DocumentType = new IssuanceDocumentType { Id = companyTransport.Risk.MainInsured.IdentificationDocument.DocumentType.Id },
                            Number = companyTransport.Risk.MainInsured.IdentificationDocument.Number                          

                        },
                        CustomerType = CustomerType.Individual,
                        CompanyName = companyTransport.Risk.MainInsured.CompanyName,
                        Name = companyTransport.Risk.MainInsured.Name
                    });
                }

                companyTransport.Risk.Coverages = new List<CompanyCoverage>();
                foreach (InsuredObjectDTO insuredObjectDTO in transportDTO.InsuredObjects)
                {
                    List<CompanyCoverage> companyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(insuredObjectDTO.Id, transportDTO.CoverageGroupId, companyPolicy.Product.Id);
                    companyTransport.Risk.Coverages.AddRange(companyCoverages.Where(x => x.IsSelected));
                }
            }
            else
            {
                companyTransport = DelegateService.transportBusinessService.GetCompanyTransportTemporalByRiskId(transportDTO.Id.Value);
                companyTransport = ModelAssembler.CreateTransport(transportDTO, companyTransport);
                companyTransport.Risk.Policy = companyPolicy;
                companyTransport.Risk.Policy.PolicyType = new CompanyPolicyType
                {
                    IsFloating = transportDTO.IsFloating
                };
                if (companyPolicy.Endorsement.EndorsementType == EndorsementType.Modification)
                {
                    companyTransport.Risk.Status = RiskStatusType.Modified;
                }
            }
            companyTransport.Risk.Coverages.AsParallel().ForAll(x => { x.CurrentFrom = companyPolicy.CurrentFrom; x.CurrentFromOriginal = companyPolicy.CurrentFrom; x.CurrentTo = companyPolicy.CurrentTo; x.CurrentToOriginal = companyPolicy.CurrentTo; });
            if (companyTransport.Risk.Id == 0)
            {
                if (CompanyTransportRisks.Count > 0)
                {
                    companyTransport.Risk.Number = CompanyTransportRisks.Count + 1;
                }
                else
                {
                    companyTransport.Risk.Number = 1;
                }
                companyTransport = DelegateService.transportBusinessService.CreateCompanyTransportTemporal(companyTransport);
            }
            else
            {
                switch (companyPolicy.Endorsement.EndorsementType.Value)
                {
                    case EndorsementType.Emission:
                    case EndorsementType.Renewal:
                        companyTransport = SetDataEmission(companyTransport);
                        break;
                    case EndorsementType.Modification:
                        companyTransport = SetDataModification(companyTransport);
                        break;
                    default:
                        break;
                }
                companyTransport = DelegateService.transportBusinessService.UpdateCompanyTransportTemporal(companyTransport);
            }
            return DTOAssembler.CreateTransport(companyTransport);
        }

        private CompanyTransport SetDataModification(CompanyTransport companyTransport)
        {
            CompanyTransport companyTransportOld = GetCompanyTransportByRiskId(companyTransport.Risk.Id);

            companyTransport.Risk.RiskId = companyTransportOld.Risk.RiskId;
            companyTransport.Risk.Number = companyTransportOld.Risk.Number;
            companyTransport.Risk.Description = companyTransportOld.Risk.Description;
            companyTransport.Risk.Text = companyTransportOld.Risk.Text;
            companyTransport.Risk.Clauses = companyTransportOld.Risk.Clauses;
            companyTransport.Risk.Status = companyTransportOld.Risk.Status;
            companyTransport.Risk.OriginalStatus = companyTransportOld.Risk.OriginalStatus;

            if (companyTransport.Risk.Status != RiskStatusType.Included && companyTransport.Risk.Status != RiskStatusType.Excluded)
            {
                companyTransport.Risk.Status = RiskStatusType.Modified;
            }
            return companyTransport;
        }

        private CompanyTransport SetDataEmission(CompanyTransport companyTransport)
        {
            CompanyTransport companyTransportOld = GetCompanyTransportByRiskId(companyTransport.Risk.Id);

            companyTransport.Risk.Text = companyTransportOld.Risk.Text;
            companyTransport.Risk.SecondInsured = companyTransportOld.Risk.SecondInsured;
            companyTransport.Risk.Clauses = companyTransportOld.Risk.Clauses;
            companyTransport.Risk.Number = companyTransportOld.Risk.Number;
            return companyTransport;
        }

        public CompanyTransport GetCompanyTransportByRiskId(int riskId)
        {
            return DelegateService.transportBusinessService.GetCompanyTransportTemporalByRiskId(riskId);
        }

        /// <summary>
        /// Obtiene un listado de pólizas de transporte a partir del identificador de la póliza
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <returns>Listado de TransportDTO</returns>
        public List<TransportDTO> GetTransportsByPolicyId(int policyId)
        {
            return DTOAssembler.CreateTransports(DelegateService.transportBusinessService.GetCompanyTransportsByPolicyId(policyId));
        }

        /// <summary>
        /// Retorna un objeto de trasporte a partir del identificador del riesgo
        /// </summary>
        /// <param name="riskId">Identificador del riesgo</param>
        /// <returns>Objeto de transporte</returns>
        public TransportDTO GetTransportByRiskId(int riskId)
        {
            return DTOAssembler.CreateTransport(DelegateService.transportBusinessService.GetCompanyTransportTemporalByRiskId(riskId));
        }

        /// <summary>
        /// Obtener Tipo de poliza por ramo y código de tipo de póliza
        /// </summary>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <param name="policyTypeCode">Identificador del tipo de poliza</param>
        /// <returns>Tipo de póliza</returns>
        public PolicyTypeDTO GetPolicyTypeByPolicyTypeIdPrefixId(int policyTypeId, int prefixId)
        {
            return DTOAssembler.CreatePolicyType(DelegateService.commonService.GetPolicyTypesByPrefixIdById(prefixId, policyTypeId));
        }

        /// <summary>
        /// Trae exclusión de transportes
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="riskId"></param>
        /// <param name="risktransportId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public TransportDTO ExcludeTransport(int temporalId, int riskId)
        {
            return DTOAssembler.CreateTransport(DelegateService.transportBusinessService.ExcludeCompanyTransport(temporalId, riskId));
        }

        /// <summary>
        /// Retorna un listado de individuos a partir de un texto de búsqueda
        /// </summary>
        /// <param name="description">Texto a buscar</param>
        /// <param name="insuredSearchType">Insured search type</param>
        /// <param name="customerType">Customer Type</param>
        /// <returns></returns>
        public List<IndividualDetailsDTO> GetInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType customerType)
        {
            return DTOAssembler.CreateIndividualDetailsList(DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType));
        }

        /// <summary>
        /// Genera cálculo a partir del objeto del seguro
        /// </summary>
        /// <param name="insuredObject">Identificador del objeto de seguro</param>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <param name="riskId">Identificador del riesgo</param>
        /// <returns>Objeto de seguro</returns>
        public InsuredObjectDTO QuotateInsuredObject(InsuredObjectDTO insuredObject, int policyId, int riskId)
        {
            /// jhgomez
            /// DelegateService.transportBusinessService.QuotateInsuredObject();
            return null;
        }

        /// <summary>
        /// Guardar lista de beneficiarios
        /// </summary>
        /// <param name="riskId"></param>
        /// <param name="beneficiariesDTOs"></param>
        /// <returns></returns>
        public List<CompanyBeneficiary> SaveBeneficiaries(int riskId, List<CompanyBeneficiary> beneficiaries)
        {
            CompanyTransport companyTransport = DelegateService.transportBusinessService.GetCompanyTransportTemporalByRiskId(riskId);

            //companyTransport.Risk.Beneficiaries = ModelAssembler.CreateBeneficiaries(beneficiaries);
            companyTransport.Risk.Beneficiaries = beneficiaries;
            companyTransport = DelegateService.transportBusinessService.UpdateCompanyTransportTemporal(companyTransport);

            return beneficiaries;
        }

        /// <summary>
        /// Guardar lista de beneficiarios
        /// </summary>
        /// <param name="riskId"></param>
        /// <param name="beneficiariesDTOs"></param>
        /// <returns></returns>
        public List<BeneficiaryDTO> SaveBeneficiaries(int riskId, List<BeneficiaryDTO> beneficiariesDTOs, bool isColective)
        {
            return DTOAssembler.CreateBeneficiaries(DelegateService.underwritingService.SaveCompanyBeneficiary(riskId, ModelAssembler.CreateBeneficiaries(beneficiariesDTOs)));
        }

        /// <summary>
        /// Guardar la lista de clusulas a partir del ID del riesgo
        /// </summary>
        /// <param name="riskId"></param>
        /// <param name="clauseDTOs"></param>
        /// <returns></returns>
        public List<ClauseDTO> SaveClauses(int temporalId, int riskId, List<ClauseDTO> clauses)
        {
            CompanyTransport companyTransport = DelegateService.transportBusinessService.GetCompanyTransportTemporalByRiskId(riskId);

            companyTransport.Risk.Clauses = ModelAssembler.CreateClauses(clauses);
            companyTransport = DelegateService.transportBusinessService.UpdateCompanyTransportTemporal(companyTransport);

            return clauses;
        }

        /// <summary>
        /// Guardar el texto a partir del ID del riesgo
        /// </summary>
        /// <param name="riskId"></param>
        /// <param name="textDTO"></param>
        /// <returns></returns>
        public TextDTO SaveText(int riskId, TextDTO textDTO)
        {
            CompanyTransport companyTransport = DelegateService.transportBusinessService.GetCompanyTransportTemporalByRiskId(riskId);

            companyTransport.Risk.Text = ModelAssembler.CreateText(textDTO);
            companyTransport = DelegateService.transportBusinessService.UpdateCompanyTransportTemporal(companyTransport);

            return textDTO;
        }

        public TransportDTO RunRulesRisk(CompanyTransport transportDTO, int ruleId)
        {
            CompanyTransport companyTransport = DelegateService.transportBusinessService.RunRulesRisk(transportDTO, ruleId);
            if (!companyTransport.Risk.Policy.IsPersisted)
            {
                companyTransport.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyTransport.Risk.Policy.Id, false);
            }

            return DTOAssembler.CreateTransport(DelegateService.transportBusinessService.RunRulesRisk(companyTransport, ruleId));
        }

        /// <summary>
        /// Trae las coverturas el producto
        /// </summary>
        /// <param name="prefixId">Ramo</param>
        /// <param name="productId">Producto</param>
        /// <returns></returns>
        public List<GroupCoverageDTO> GetGroupCoveragesByPrefixIdProductId(int prefixId, int productId)
        {
            return DTOAssembler.CreateCoverageGroups(DelegateService.underwritingService.GetGroupCoverages(productId));
        }

        /// <summary>
        /// Guarda las coberturas asociadas aún riesgo del ramo de Transportes
        /// </summary>
        /// <param name="policyId">Poliza</param>
        /// <param name="riskId">Riesgo</param>
        /// /// <param name="coverages">Listado de Coberturas</param>
        /// <returns>TransportDTO</returns>
        public TransportDTO SaveCoverages(int policyId, int riskId, List<CompanyCoverage> coverages, int insuredObjectId)
        {
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(policyId, false);
            CompanyTransport companyTransport = DelegateService.transportBusinessService.GetCompanyTransportTemporalByRiskId(riskId);


            if (companyPolicy != null && companyTransport != null && coverages != null)
            {
                companyTransport.Risk.IsPersisted = true;
                companyTransport.Risk.Coverages.RemoveAll(x => x.InsuredObject.Id == insuredObjectId);
                companyTransport.Risk.Coverages.AddRange(coverages);
                companyTransport.Risk.Policy = companyPolicy;
                companyTransport = DelegateService.transportBusinessService.QuotateCompanyTransport(companyTransport, false, true);

                if (companyTransport.Risk.Id == 0)
                {
                    companyTransport = DelegateService.transportBusinessService.CreateCompanyTransportTemporal(companyTransport);
                }
                else
                {
                    companyTransport = DelegateService.transportBusinessService.UpdateCompanyTransportTemporal(companyTransport);
                }
            }

            return DTOAssembler.CreateTransport(companyTransport);
        }

        public EndorsementDTO CreateEndorsement(int temporalId)
        {
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
            List<CompanyTransport> companyTransports = DelegateService.transportBusinessService.GetCompanyTransportsByTemporalId(temporalId);

            companyPolicy = DelegateService.transportBusinessService.CreateEndorsement(companyPolicy, companyTransports);

            return DTOAssembler.CreateEndorsement(companyPolicy);
        }
        public List<TransportDTO> GetCompanyTransportsByPolicyIdEndorsementId(int policyId, int endorsementId)
        {

            return DTOAssembler.CreateTransportsByEndorsementId(DelegateService.transportBusinessService.GetCompanyTransportsByPolicyIdEndorsementId(policyId, endorsementId));
        }

        public List<EndorsementDTO> GetEndorsementByEndorsementTypeIdPolicyId(int endorsementTypeId, int policyId)
        {

            List<CompanyEndorsement> companyEndorsements = DelegateService.transportBusinessService.GetCompanyEndorsementByEndorsementTypeIdPolicyId(endorsementTypeId, policyId);
            return DTOAssembler.CreateEndorsements(companyEndorsements);
        }

        public EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId)
        {

            Endorsement endorsement = DelegateService.endorsementBaseService.GetTemporalEndorsementByPolicyId(policyId);
            return DTOAssembler.CreateEndorsementDto(endorsement);
        }

        public List<CoverageDTO> GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(int policyId, int endorsementId, int riskId)
        {
            List<CompanyCoverage> companyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementId, riskId);
            return DTOAssembler.CreateCoveragesDtos(companyCoverages);
        }

        public TextDTO GetTextsByRiskId(int riskId)
        {
            CompanyTransport companyTransport = DelegateService.transportBusinessService.GetCompanyTransportTemporalByRiskId(riskId);
            return DTOAssembler.CreateText(companyTransport.Risk.Text);
        }

        public bool GetLeapYear()
        {
            return DelegateService.transportBusinessService.GetLeapYear();
        }
        /// <summary>
        /// Se elimina el riesgo asociado
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="riskId"></param>
        /// <returns></returns>
        internal bool DeleteCompanyRisk(int temporalId, int riskId)
        {
            return DelegateService.transportBusinessService.DeleteCompanyRisk(temporalId, riskId);
        }
        /// <summary>
        /// Coberturas asociadas a un objeto del seguro perteneciente a un riesgo
        /// </summary>
        /// <param name="insuredObjectId"></param>
        /// <returns></returns>
        public List<CoverageDTO> GetTemporalCoveragesByRiskIdInsuredObjectId(int riskId, int insuredObjectId)
        {
            List<CompanyCoverage> companyCoverages = DelegateService.transportBusinessService.GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId);
            return DTOAssembler.CreateCoveragesDtos(companyCoverages);
        }

        public bool saveInsuredObject(int riskId, InsuredObjectDTO insuredObjectDTO, int tempId, int groupCoverageId)
        {
            CompanyInsuredObject insuredObject = ModelAssembler.CreateInsuredObject(insuredObjectDTO);
            bool saveInsuredObject = DelegateService.transportBusinessService.saveInsuredObject(riskId, insuredObject, tempId, groupCoverageId);
            return saveInsuredObject;
        }

        public CompanyCoverage GetCoverageByCoverageId(int coverageId, int temporalId, int groupCoverageId)
        {
            return DelegateService.transportBusinessService.GetCoverageByCoverageId(coverageId,temporalId,groupCoverageId);
        }

        /// <summary>
        /// Genera el próximo endoso de declaración para una póliza
        /// </summary>
        /// <param name="policyId">Identificador de una póliza</param>
        /// <returns>Endoso de declaración</returns>

        public EndorsementDTO GetNextDeclarationEndorsementByPolicyId(int policyId)
        {
            return DTOAssembler.CreateEndorsementDTO(DelegateService.transportBusinessService.GetNextDeclarationEndorsementByPolicyId(policyId));
        }
        public List<RiskCommercialClassDTO> GetRiskCommercialClasses(string description)
        {
            return DTOAssembler.CreateRiskCommercialClasses(DelegateService.transportBusinessService.GetRiskCommercialClasses(description));
        }

        public List<HolderTypeDTO> GetHolderTypes()
        {
            return DTOAssembler.CreateHolderTypes(DelegateService.transportBusinessService.GetHolderTypes());
        }

        /// <summary>
        /// Genera el próximo endoso de ajuste para una póliza
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <returns>Endoso de ajuste</returns>
        public EndorsementDTO GetNextAdjustmentEndorsementByPolicyId(int policyId)
        {
            return DTOAssembler.CreateEndorsementDTO(DelegateService.transportBusinessService.GetNextAdjustmentEndorsementByPolicyId(policyId));
        }

        /// <summary>
        /// Indica si es posible hacer un endoso de declaración
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public bool CanMakeDeclarationEndorsement(int policyId)
        {
            return DelegateService.transportBusinessService.CanMakeDeclarationEndorsement(policyId);
        }

        /// <summary>
        /// Indica si es posible hacer un endoso de Ajuste
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public bool CanMakeAdjustmentEndorsement(int policyId)
        {
            return DelegateService.transportBusinessService.CanMakeAdjustmentEndorsement(policyId);
        }

        public bool CanMakeEndorsement(int policyId, out Dictionary<string, object> endorsementValidate)
        {
            return DelegateService.transportBusinessService.CanMakeEndorsement(policyId, out endorsementValidate);
        }

        public List<CompanyCoverage> GetCoveragesByCoveragesAdd(int productId, int coverageGroupId, int prefixId, string coveragesAdd, int insuredObjectId)
        {
            return DelegateService.transportBusinessService.GetCoveragesByCoveragesAdd(productId, coverageGroupId, prefixId, coveragesAdd, insuredObjectId);
        }
    }
}
