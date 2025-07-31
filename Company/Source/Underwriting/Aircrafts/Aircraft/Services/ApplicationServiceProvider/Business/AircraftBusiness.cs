using Sistran.Company.Application.Aircrafts.AircraftApplicationService.DTOs;
using Sistran.Company.Application.Aircrafts.AircraftApplicationService.EEProvider.Assemblers;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.Aircrafts.AircraftApplicationService.EEProvider.Business
{
    public class AircraftBusiness
    {
        

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
        /// Obtener listado de países
        /// </summary>
        /// <returns>Listado de países</returns>
        public List<CountryDTO> GetCountries()
        {
            return DTOAssembler.CreateCountries(DelegateService.commonService.GetCountriesLite());
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
        /// Trae el Aircrafte por temporal ID
        /// </summary>
        /// <returns></returns>
        public List<AircraftDTO> GetAircraftsByTemporalId(int temporalId)
        {
            return DTOAssembler.CreateAircrafts(DelegateService.AircraftBusinessService.GetCompanyAircraftsByTemporalId(temporalId));
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
        /// Hace el guardado de un riegos Aircrafte
        /// </summary>
        /// <param name="aircraftDTO">Modelo de Aircrafte</param>
        /// <returns>Modelo de Aircrafte</returns>
        public AircraftDTO SaveAircraft(AircraftDTO aircraftDTO)
        {
            CompanyAircraft companyAircraft = new CompanyAircraft();
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(aircraftDTO.PolicyId, false);

            if (aircraftDTO.Id.GetValueOrDefault() == 0)
            {
                companyAircraft = ModelAssembler.CreateAircraft(aircraftDTO);

                if (companyPolicy.Endorsement.EndorsementType == EndorsementType.Emission)
                {
                    companyAircraft.Risk.Status = RiskStatusType.Original;
                }

                if (companyPolicy.Endorsement.EndorsementType == EndorsementType.Modification)
                {
                    companyAircraft.Risk.Status = RiskStatusType.Included;
                }

                companyAircraft.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                companyAircraft.Risk.Beneficiaries.Add(
                new CompanyBeneficiary
                {
                    IndividualId = companyAircraft.Risk.MainInsured.IndividualId,
                    BeneficiaryType = new CompanyBeneficiaryType
                    {
                        Id = 1
                    },
                    Participation = 100,
                    IdentificationDocument = new IssuanceIdentificationDocument
                    {
                        DocumentType = new IssuanceDocumentType { Id = companyAircraft.Risk.MainInsured.IdentificationDocument.DocumentType.Id },
                        Number = companyAircraft.Risk.MainInsured.IdentificationDocument.Number,
                        //ExpeditionDate = companyAircraft.Risk.MainInsured.IdentificationDocument.ExpeditionDate

                    },
                    CustomerType = companyAircraft.Risk.MainInsured.CustomerType,
                    BeneficiaryTypeDescription = companyAircraft.Risk.MainInsured.CustomerTypeDescription,
                    CompanyName = companyAircraft.Risk.MainInsured.CompanyName,
                    Name = companyAircraft.Risk.MainInsured.Name,
                    IndividualType = companyAircraft.Risk.MainInsured.IndividualType
                });

                companyAircraft.Risk.Coverages = new List<CompanyCoverage>();

                foreach (InsuredObjectDTO insuredObjectDTO in aircraftDTO.InsuredObjects)
                {
                    List<CompanyCoverage> companyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(insuredObjectDTO.Id, aircraftDTO.CoverageGroupId, companyPolicy.Product.Id);
                    companyAircraft.Risk.Coverages.AddRange(companyCoverages.Where(x => x.IsSelected));
                }
            }
            else
            {
                companyAircraft = DelegateService.AircraftBusinessService.GetCompanyAircraftTemporalByRiskId(aircraftDTO.Id.Value);
                AssingCompanyAircraft(aircraftDTO, ref companyAircraft);
            }

            companyAircraft.Risk.Coverages.ForEach(x => x.CurrentFrom = companyPolicy.CurrentFrom);
            companyAircraft.Risk.Coverages.ForEach(x => x.CurrentTo = companyPolicy.CurrentTo);
            companyAircraft = DelegateService.AircraftBusinessService.QuotateCompanyAircraft(companyAircraft, true, true);

            if (companyAircraft.Risk.Id == 0)
            {
                companyAircraft = DelegateService.AircraftBusinessService.CreateCompanyAircraftTemporal(companyAircraft);
            }
            else
            {
                switch (companyPolicy.Endorsement.EndorsementType.Value)
                {
                    case EndorsementType.Emission:
                    case EndorsementType.Renewal:
                        companyAircraft = SetDataEmission(companyAircraft);
                        break;
                    case EndorsementType.Modification:
                        companyAircraft = SetDataModification(companyAircraft);
                        break;
                    default:
                        break;
                }

                companyAircraft = DelegateService.AircraftBusinessService.UpdateCompanyAircraftTemporal(companyAircraft);
            }

            return DTOAssembler.CreateAircraft(companyAircraft);
        }

        private CompanyAircraft SetDataModification(CompanyAircraft companyAircraft)
        {
            CompanyAircraft companyAircraftOld = GetCompanyAircraftByRiskId(companyAircraft.Risk.Id);

            companyAircraft.Risk.RiskId = companyAircraftOld.Risk.RiskId;
            companyAircraft.Risk.Number = companyAircraftOld.Risk.Number;
            companyAircraft.Risk.Description = companyAircraftOld.Risk.Description;
            //companyAircraft.Risk.Beneficiaries = companyAircraftOld.Risk.Beneficiaries;
            companyAircraft.Risk.Text = companyAircraftOld.Risk.Text;
            companyAircraft.Risk.Clauses = companyAircraftOld.Risk.Clauses;
            companyAircraft.Risk.Status = companyAircraftOld.Risk.Status;
            companyAircraft.Risk.OriginalStatus = companyAircraftOld.Risk.OriginalStatus;

            if (companyAircraft.Risk.Status != RiskStatusType.Included && companyAircraft.Risk.Status != RiskStatusType.Excluded)
            {
                companyAircraft.Risk.Status = RiskStatusType.Modified;
            }

            return companyAircraft;

        }


        private CompanyAircraft SetDataEmission(CompanyAircraft companyAircraft)
        {
            CompanyAircraft companyAircraftOld = GetCompanyAircraftByRiskId(companyAircraft.Risk.Id);

            //companyAircraft.Risk.Beneficiaries = companyAircraftOld.Risk.Beneficiaries;
            companyAircraft.Risk.Text = companyAircraftOld.Risk.Text;
            companyAircraft.Risk.SecondInsured = companyAircraftOld.Risk.SecondInsured;
            companyAircraft.Risk.Clauses = companyAircraftOld.Risk.Clauses;

            return companyAircraft;
        }


        /// <summary>
        /// Obtiene un listado de pólizas de Aircrafte a partir del identificador de la póliza
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <returns>Listado de AircraftDTO</returns>
        public List<AircraftDTO> GetAircraftsByPolicyId(int policyId)
        {
            return DTOAssembler.CreateAircrafts(DelegateService.AircraftBusinessService.GetCompanyAircraftsByPolicyId(policyId));
        }

        /// <summary>
        /// Retorna un objeto de trasporte a partir del identificador del riesgo
        /// </summary>
        /// <param name="riskId">Identificador del riesgo</param>
        /// <returns>Objeto de Aircrafte</returns>
        public AircraftDTO GetAircraftByRiskId(int riskId)
        {
            return DTOAssembler.CreateAircraft(DelegateService.AircraftBusinessService.GetCompanyAircraftTemporalByRiskId(riskId));
        }

        public CompanyAircraft GetCompanyAircraftByRiskId(int riskId)
        {
            return DelegateService.AircraftBusinessService.GetCompanyAircraftTemporalByRiskId(riskId);
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
        /// Trae exclusión de Aircraftes
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="riskId"></param>
        /// <param name="riskAircraftId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public AircraftDTO ExcludeAircraft(int temporalId, int riskId)
        {
            return DTOAssembler.CreateAircraft(DelegateService.AircraftBusinessService.ExcludeCompanyAircraft(temporalId, riskId));
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
            /// DelegateService.AircraftBusinessService.QuotateInsuredObject();
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
            CompanyAircraft companyAircraft = DelegateService.AircraftBusinessService.GetCompanyAircraftTemporalByRiskId(riskId);

            companyAircraft.Risk.Beneficiaries = beneficiaries;
            companyAircraft = DelegateService.AircraftBusinessService.UpdateCompanyAircraftTemporal(companyAircraft);

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
            CompanyAircraft companyAircraft = DelegateService.AircraftBusinessService.GetCompanyAircraftTemporalByRiskId(riskId);

            companyAircraft.Risk.Clauses = ModelAssembler.CreateClauses(clauses);
            companyAircraft = DelegateService.AircraftBusinessService.UpdateCompanyAircraftTemporal(companyAircraft);

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
            CompanyAircraft companyAircraft = DelegateService.AircraftBusinessService.GetCompanyAircraftTemporalByRiskId(riskId);

            companyAircraft.Risk.Text = ModelAssembler.CreateText(textDTO);
            companyAircraft = DelegateService.AircraftBusinessService.UpdateCompanyAircraftTemporal(companyAircraft);

            return textDTO;
        }

        public AircraftDTO RunRulesRisk(CompanyAircraft AircraftDTO, int ruleId)
        {
            CompanyAircraft companyAircraft = DelegateService.AircraftBusinessService.RunRulesRisk(AircraftDTO, ruleId);
            if (!companyAircraft.Risk.Policy.IsPersisted)
            {
                companyAircraft.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyAircraft.Risk.Policy.Id, false);
            }

            return DTOAssembler.CreateAircraft(DelegateService.AircraftBusinessService.RunRulesRisk(companyAircraft, ruleId));
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
        /// Guarda las coberturas asociadas aún riesgo del ramo de Aircraftes
        /// </summary>
        /// <param name="policyId">Poliza</param>
        /// <param name="riskId">Riesgo</param>
        /// /// <param name="coverages">Listado de Coberturas</param>
        /// <returns>AircraftDTO</returns>
        public AircraftDTO SaveCoverages(int policyId, int riskId, List<CompanyCoverage> coverages, int insuredObjectId)
        {
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(policyId, false);
            CompanyAircraft companyAircraft = DelegateService.AircraftBusinessService.GetCompanyAircraftTemporalByRiskId(riskId);


            if (companyPolicy != null && companyAircraft != null && coverages != null)
            {
                companyAircraft.Risk.IsPersisted = true;
                companyAircraft.Risk.Coverages.RemoveAll(x => x.InsuredObject.Id == insuredObjectId);
                companyAircraft.Risk.Coverages.AddRange(coverages);
                companyAircraft.Risk.Policy = companyPolicy;
                companyAircraft = DelegateService.AircraftBusinessService.QuotateCompanyAircraft(companyAircraft, false, true);

                if (companyAircraft.Risk.Id == 0)
                {
                    companyAircraft = DelegateService.AircraftBusinessService.CreateCompanyAircraftTemporal(companyAircraft);
                }
                else
                {
                    companyAircraft = DelegateService.AircraftBusinessService.UpdateCompanyAircraftTemporal(companyAircraft);
                }
            }

            return DTOAssembler.CreateAircraft(companyAircraft);
        }

        public EndorsementDTO CreateEndorsement(int temporalId)
        {
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
            List<CompanyAircraft> companyAircrafts = DelegateService.AircraftBusinessService.GetCompanyAircraftsByTemporalId(temporalId);

            companyPolicy = DelegateService.AircraftBusinessService.CreateEndorsement(companyPolicy, companyAircrafts);

            return DTOAssembler.CreateEndorsement(companyPolicy);
        }
        public List<AircraftDTO> GetCompanyAircraftsByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            return DTOAssembler.CreateAircraftsByEndorsementId(DelegateService.AircraftBusinessService.GetCompanyAircraftsByPolicyIdEndorsementId(policyId, endorsementId));
        }

        public List<EndorsementDTO> GetEndorsementByEndorsementTypeIdPolicyId(int endorsementTypeId, int policyId)
        {

            List<CompanyEndorsement> companyEndorsements = DelegateService.AircraftBusinessService.GetCompanyEndorsementByEndorsementTypeIdPolicyId(endorsementTypeId, policyId);
            return DTOAssembler.CreateEndorsements(companyEndorsements);
        }

        public EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId)
        {

            Endorsement endorsement = DelegateService.endorsementBaseService.GetTemporalEndorsementByPolicyId(policyId);
            return DTOAssembler.CreateEndorsementDto(endorsement);
        }

        public List<CoverageDTO> GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(int policyId, int endorsementId, int riskId)
        {
            List<CompanyCoverage> companyCoverages = DelegateService.AircraftBusinessService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementId, riskId);
            return DTOAssembler.CreateCoverages(companyCoverages);
        }

        public TextDTO GetTextsByRiskId(int riskId)
        {
            CompanyAircraft companyAircraft = DelegateService.AircraftBusinessService.GetCompanyAircraftTemporalByRiskId(riskId);
            return DTOAssembler.CreateText(companyAircraft.Risk.Text);
        }

       
        /// <summary>
        /// Se elimina el riesgo asociado
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="riskId"></param>
        /// <returns></returns>
        internal bool DeleteCompanyRisk(int temporalId, int riskId)
        {
            return DelegateService.AircraftBusinessService.DeleteCompanyRisk(temporalId, riskId);
        }
        /// <summary>
        /// Coberturas asociadas a un objeto del seguro perteneciente a un riesgo
        /// </summary>
        /// <param name="insuredObjectId"></param>
        /// <returns></returns>
        public List<CoverageDTO> GetTemporalCoveragesByRiskIdInsuredObjectId(int riskId, int insuredObjectId)
        {
            List<CompanyCoverage> companyCoverages = DelegateService.AircraftBusinessService.GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId);
            return DTOAssembler.CreateCoverages(companyCoverages);
        }

        public bool saveInsuredObject(int riskId, InsuredObjectDTO insuredObjectDTO, int tempId, int groupCoverageId)
        {
            InsuredObject insuredObject = ModelAssembler.CreateInsuredObject(insuredObjectDTO);
            bool saveInsuredObject = DelegateService.AircraftBusinessService.saveInsuredObject(riskId, insuredObject, tempId, groupCoverageId);
            return saveInsuredObject;
        }


        /// <summary>
        /// Obtiene la marca
        /// </summary>
        /// <returns></returns>
        public List<MakeDTO> GetMakes()
        {
            return DTOAssembler.CreateMakes(DelegateService.AircraftBusinessService.GetMakes());
        }

        public List<ModelDTO> GetModelsByMakeId(int makeId)
        {
            return DTOAssembler.CreateModelsByMakesId(DelegateService.AircraftBusinessService.GetModelByMakeId(makeId));            
        }

        public List<OperatorDTO> GetOperators()
        {
            return DTOAssembler.CreateOperators(DelegateService.AircraftBusinessService.GetOperators());
        }

        public List<RegisterDTO> GetRegisters()
        {
            return DTOAssembler.CreateRegisters(DelegateService.AircraftBusinessService.GetRegisters());
        }

         public List<UseDTO> GetUsesByPrefixId(int prefixId)
        {
            return null;
          
        }

        public List<AircraftTypeDTO> GetTybeByTypesByPrefixId(int prefixId)
        {
            return DTOAssembler.CreateAircraftTypes(DelegateService.AircraftBusinessService.GetTybeByTypesByPrefixId(prefixId));
        }
        public List<UseDTO> GetUseByusessByPrefixId(int prefixId)
        {
            return DTOAssembler.CreateAircraftUses(DelegateService.AircraftBusinessService.GetUseByusessByPrefixId(prefixId));
        }

        private void AssingCompanyAircraft(AircraftDTO aircraftDTO, ref CompanyAircraft companyAircraft)
        {
            if (companyAircraft == null)
            {
                companyAircraft = new CompanyAircraft();
            }
            else
            {
                companyAircraft.Risk = AssignCompanyRisk(aircraftDTO, companyAircraft.Risk);
                companyAircraft.MakeId = (int)aircraftDTO.MakeId;
                companyAircraft.ModelId = (int)aircraftDTO.ModelId;
                companyAircraft.TypeId = (int)aircraftDTO.TypeId;
                companyAircraft.UseId = (int)aircraftDTO.UseId;
                companyAircraft.OperatorId = (int)aircraftDTO.OperatorId;
                companyAircraft.CurrentManufacturing = (int)aircraftDTO.CurrentManufacturing;
                companyAircraft.RegisterId = (int)aircraftDTO.RegisterId;
                companyAircraft.NumberRegister = aircraftDTO.NumberRegister;

                companyAircraft.InsuredObjects = ModelAssembler.CreateInsuredObjects(aircraftDTO.InsuredObjects);
            }
        }

        private CompanyRisk AssignCompanyRisk(AircraftDTO AircraftDTO, CompanyRisk companyRisk)
        {
            if (companyRisk == null)
            {
                companyRisk = new CompanyRisk();
            }

            companyRisk.Id = AircraftDTO.Id.GetValueOrDefault();
            companyRisk.RiskId = AircraftDTO.RiskId.GetValueOrDefault();
            companyRisk.Description = AircraftDTO.Description;
            companyRisk.Policy = new CompanyPolicy
            {
                Id = AircraftDTO.PolicyId,
            };
            companyRisk.GroupCoverage = new GroupCoverage
            {
                Id = AircraftDTO.CoverageGroupId
            };
            companyRisk.MainInsured = new CompanyIssuanceInsured
            {
                BirthDate = AircraftDTO.BirthDate,
                Gender = AircraftDTO.Gender,
                IdentificationDocument = new IssuanceIdentificationDocument
                {
                    Number = AircraftDTO.DocumentNumber,
                    DocumentType = new IssuanceDocumentType
                    {
                        Id = AircraftDTO.DocumentType,
                        Description = AircraftDTO.DocumentTypeDescription,
                        SmallDescription = AircraftDTO.DocumentTypeSmallDescription
                    },
                    //ExpeditionDate = AircraftDTO.DocumentExpedition
                },
                Name = AircraftDTO.Name,
                CompanyName = new IssuanceCompanyName
                {
                    TradeName = AircraftDTO.CompanyName,
                    NameNum = AircraftDTO.NameNum,
                    Address = new IssuanceAddress { Description = AircraftDTO.Address },
                    Phone = new IssuancePhone { Description = AircraftDTO.Phone },
                    Email = new IssuanceEmail { Description = AircraftDTO.Email },
                    IsMain = AircraftDTO.IsMain
                },
                IndividualId = AircraftDTO.IndividualId,
                IndividualType = (IndividualType)AircraftDTO.IndividualType,
                CustomerType = (CustomerType)AircraftDTO.CustomerType,
                CustomerTypeDescription = AircraftDTO.CustomerTypeDescription,
                Profile = AircraftDTO.Profile,
                ScoreCredit = new ScoreCredit
                {
                    ScoreCreditId = AircraftDTO.ScoreCredit.GetValueOrDefault()
                }
            };
            companyRisk.CoveredRiskType = CoveredRiskType.Aircraft;
            companyRisk.Status = Core.Application.UnderwritingServices.Enums.RiskStatusType.Original;
            companyRisk.OriginalStatus = Core.Application.UnderwritingServices.Enums.RiskStatusType.Original;
            companyRisk.IsPersisted = true;

            return companyRisk;
        }
    }
}
