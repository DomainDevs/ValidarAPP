using Sistran.Company.Application.Marines.MarineApplicationService.DTOs;
using Sistran.Company.Application.Marines.MarineApplicationService.EEProvider.Assemblers;
using Sistran.Company.Application.Marines.MarineBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.Marines.MarineApplicationService.EEProvider.Business
{
    public class MarineBusiness
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
        /// Trae el Marinee por temporal ID
        /// </summary>
        /// <returns></returns>
        public List<MarineDTO> GetMarinesByTemporalId(int temporalId)
        {
            return DTOAssembler.CreateMarines(DelegateService.marineBusinessService.GetCompanyMarinesByTemporalId(temporalId));
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

        private void AssingCompanyMarine(MarineDTO marineDTO, ref CompanyMarine companyMarine)
        {
            if (companyMarine == null)
            {
                companyMarine = new CompanyMarine();
            }
            else
            {
                companyMarine.Risk = AssignCompanyRisk(marineDTO, companyMarine.Risk);
                companyMarine.UseId = (int)marineDTO.UseId;
                companyMarine.NumberRegister = marineDTO.NameNum.ToString();

                companyMarine.InsuredObjects = ModelAssembler.CreateInsuredObjects(marineDTO.InsuredObjects);
            }
        }

        /// <summary>
        /// Hace el guardado de un riegos Marinee
        /// </summary>
        /// <param name="marineDTO">Modelo de Marinee</param>
        /// <returns>Modelo de Marinee</returns>
        public MarineDTO SaveMarine(MarineDTO marineDTO)
        {
            CompanyMarine companyMarine = new CompanyMarine();
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(marineDTO.PolicyId, false);

            if (marineDTO.Id.GetValueOrDefault() == 0)
            {
                companyMarine = ModelAssembler.CreateMarine(marineDTO);

                if (companyPolicy.Endorsement.EndorsementType == EndorsementType.Emission)
                {
                    companyMarine.Risk.Status = RiskStatusType.Original;
                }

                companyMarine.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                companyMarine.Risk.Beneficiaries.Add(
                new CompanyBeneficiary
                {
                    IndividualId = companyMarine.Risk.MainInsured.IndividualId,
                    BeneficiaryType = new CompanyBeneficiaryType
                    {
                        Id = 1
                    },
                    Participation = 100,
                    IdentificationDocument = new IssuanceIdentificationDocument
                    {
                        DocumentType = new IssuanceDocumentType { Id = companyMarine.Risk.MainInsured.IdentificationDocument.DocumentType.Id },
                        Number = companyMarine.Risk.MainInsured.IdentificationDocument.Number,
                        //ExpeditionDate = companyMarine.Risk.MainInsured.IdentificationDocument.ExpeditionDate

                    },
                    CustomerType = companyMarine.Risk.MainInsured.CustomerType,
                    BeneficiaryTypeDescription = companyMarine.Risk.MainInsured.CustomerTypeDescription,
                    CompanyName = companyMarine.Risk.MainInsured.CompanyName,
                    Name = companyMarine.Risk.MainInsured.Name,
                    IndividualType = companyMarine.Risk.MainInsured.IndividualType
                });

                companyMarine.Risk.Coverages = new List<CompanyCoverage>();

                foreach (InsuredObjectDTO insuredObjectDTO in marineDTO.InsuredObjects)
                {
                    List<CompanyCoverage> companyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(insuredObjectDTO.Id, marineDTO.CoverageGroupId, companyPolicy.Product.Id);
                    companyMarine.Risk.Coverages.AddRange(companyCoverages.Where(x => x.IsSelected));
                }
            }
            else
            {
                companyMarine = DelegateService.marineBusinessService.GetCompanyMarineTemporalByRiskId(marineDTO.Id.Value);
                AssingCompanyMarine(marineDTO, ref companyMarine);
            }

            companyMarine.Risk.Coverages.ForEach(x => x.CurrentFrom = companyPolicy.CurrentFrom);
            companyMarine.Risk.Coverages.ForEach(x => x.CurrentTo = companyPolicy.CurrentTo);
            companyMarine = DelegateService.marineBusinessService.QuotateCompanyMarine(companyMarine, true, true);

            if (companyMarine.Risk.Id == 0)
            {
                companyMarine = DelegateService.marineBusinessService.CreateCompanyMarineTemporal(companyMarine);
            }
            else
            {
                companyMarine = DelegateService.marineBusinessService.UpdateCompanyMarineTemporal(companyMarine);
            }

            return DTOAssembler.CreateMarine(companyMarine);
        }

        /// <summary>
        /// Obtiene un listado de pólizas de Marinee a partir del identificador de la póliza
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <returns>Listado de MarineDTO</returns>
        public List<MarineDTO> GetMarinesByPolicyId(int policyId)
        {
            return DTOAssembler.CreateMarines(DelegateService.marineBusinessService.GetCompanyMarinesByPolicyId(policyId));
        }

        /// <summary>
        /// Retorna un objeto de trasporte a partir del identificador del riesgo
        /// </summary>
        /// <param name="riskId">Identificador del riesgo</param>
        /// <returns>Objeto de Marinee</returns>
        public MarineDTO GetMarineByRiskId(int riskId)
        {
            return DTOAssembler.CreateMarine(DelegateService.marineBusinessService.GetCompanyMarineTemporalByRiskId(riskId));
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
        /// Trae exclusión de Marinees
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="riskId"></param>
        /// <param name="riskMarineId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public MarineDTO ExcludeMarine(int temporalId, int riskId)
        {
            return DTOAssembler.CreateMarine(DelegateService.marineBusinessService.ExcludeCompanyMarine(temporalId, riskId));
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
            /// DelegateService.MarineBusinessService.QuotateInsuredObject();
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
            CompanyMarine companyMarine = DelegateService.marineBusinessService.GetCompanyMarineTemporalByRiskId(riskId);
            companyMarine.Risk.Beneficiaries = beneficiaries;
            companyMarine = DelegateService.marineBusinessService.UpdateCompanyMarineTemporal(companyMarine);

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
            CompanyMarine companyMarine = DelegateService.marineBusinessService.GetCompanyMarineTemporalByRiskId(riskId);

            companyMarine.Risk.Clauses = ModelAssembler.CreateClauses(clauses);
            companyMarine = DelegateService.marineBusinessService.UpdateCompanyMarineTemporal(companyMarine);

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
            CompanyMarine companyMarine = DelegateService.marineBusinessService.GetCompanyMarineTemporalByRiskId(riskId);

            companyMarine.Risk.Text = ModelAssembler.CreateText(textDTO);
            companyMarine = DelegateService.marineBusinessService.UpdateCompanyMarineTemporal(companyMarine);

            return textDTO;
        }

        public MarineDTO RunRulesRisk(CompanyMarine MarineDTO, int ruleId)
        {
            CompanyMarine companyMarine = DelegateService.marineBusinessService.RunRulesRisk(MarineDTO, ruleId);
            if (!companyMarine.Risk.Policy.IsPersisted)
            {
                companyMarine.Risk.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyMarine.Risk.Policy.Id, false);
            }

            return DTOAssembler.CreateMarine(DelegateService.marineBusinessService.RunRulesRisk(companyMarine, ruleId));
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
        /// Guarda las coberturas asociadas aún riesgo del ramo de Marinees
        /// </summary>
        /// <param name="policyId">Poliza</param>
        /// <param name="riskId">Riesgo</param>
        /// /// <param name="coverages">Listado de Coberturas</param>
        /// <returns>MarineDTO</returns>
        public MarineDTO SaveCoverages(int policyId, int riskId, List<CompanyCoverage> coverages, int insuredObjectId)
        {
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(policyId, false);
            CompanyMarine companyMarine = DelegateService.marineBusinessService.GetCompanyMarineTemporalByRiskId(riskId);


            if (companyPolicy != null && companyMarine != null && coverages != null)
            {
                companyMarine.Risk.IsPersisted = true;
                companyMarine.Risk.Coverages.RemoveAll(x => x.InsuredObject.Id == insuredObjectId);
                companyMarine.Risk.Coverages.AddRange(coverages);
                companyMarine.Risk.Policy = companyPolicy;
                companyMarine = DelegateService.marineBusinessService.QuotateCompanyMarine(companyMarine, false, true);

                if (companyMarine.Risk.Id == 0)
                {
                    companyMarine = DelegateService.marineBusinessService.CreateCompanyMarineTemporal(companyMarine);
                }
                else
                {
                    companyMarine = DelegateService.marineBusinessService.UpdateCompanyMarineTemporal(companyMarine);
                }
            }

            return DTOAssembler.CreateMarine(companyMarine);
        }

        public EndorsementDTO CreateEndorsement(int temporalId)
        {
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
            List<CompanyMarine> companyMarines = DelegateService.marineBusinessService.GetCompanyMarinesByTemporalId(temporalId);

            companyPolicy = DelegateService.marineBusinessService.CreateEndorsement(companyPolicy, companyMarines);

            return DTOAssembler.CreateEndorsement(companyPolicy);
        }
        public List<MarineDTO> GetCompanyMarinesByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            return DTOAssembler.CreateMarinesByEndorsementId(DelegateService.marineBusinessService.GetCompanyMarinesByPolicyIdEndorsementId(policyId, endorsementId));
        }

        public List<EndorsementDTO> GetEndorsementByEndorsementTypeIdPolicyId(int endorsementTypeId, int policyId)
        {

            List<CompanyEndorsement> companyEndorsements = DelegateService.marineBusinessService.GetCompanyEndorsementByEndorsementTypeIdPolicyId(endorsementTypeId, policyId);
            return DTOAssembler.CreateEndorsements(companyEndorsements);
        }

        public EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId)
        {

            Endorsement endorsement = DelegateService.endorsementBaseService.GetTemporalEndorsementByPolicyId(policyId);
            return DTOAssembler.CreateEndorsementDto(endorsement);
        }

        public List<CoverageDTO> GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(int policyId, int endorsementId, int riskId)
        {
            List<CompanyCoverage> companyCoverages = DelegateService.marineBusinessService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementId, riskId);
            return DTOAssembler.CreateCoverages(companyCoverages);
        }

        public TextDTO GetTextsByRiskId(int riskId)
        {
            CompanyMarine companyMarine = DelegateService.marineBusinessService.GetCompanyMarineTemporalByRiskId(riskId);
            return DTOAssembler.CreateText(companyMarine.Risk.Text);
        }

        public bool GetLeapYear()
        {
            return DelegateService.marineBusinessService.GetLeapYear();
        }
        /// <summary>
        /// Se elimina el riesgo asociado
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="riskId"></param>
        /// <returns></returns>
        internal bool DeleteCompanyRisk(int temporalId, int riskId)
        {
            return DelegateService.marineBusinessService.DeleteCompanyRisk(temporalId, riskId);
        }
        /// <summary>
        /// Coberturas asociadas a un objeto del seguro perteneciente a un riesgo
        /// </summary>
        /// <param name="insuredObjectId"></param>
        /// <returns></returns>
        public List<CoverageDTO> GetTemporalCoveragesByRiskIdInsuredObjectId(int riskId, int insuredObjectId)
        {
            List<CompanyCoverage> companyCoverages = DelegateService.marineBusinessService.GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId);
            return DTOAssembler.CreateCoverages(companyCoverages);
        }

        public bool saveInsuredObject(int riskId, InsuredObjectDTO insuredObjectDTO, int tempId, int groupCoverageId)
        {
            InsuredObject insuredObject = ModelAssembler.CreateInsuredObject(insuredObjectDTO);
            bool saveInsuredObject = DelegateService.marineBusinessService.saveInsuredObject(riskId, insuredObject, tempId, groupCoverageId);
            return saveInsuredObject;
        }

        public List<UsePrefixDTO> GetMarineUsesByPrefixId(int prefixId)
        {
            return DTOAssembler.CreateMarineUses(DelegateService.marineBusinessService.GetUseByusessByPrefixId(prefixId));
        }

        private CompanyRisk AssignCompanyRisk(MarineDTO marineDTO, CompanyRisk companyRisk)
        {
            if (companyRisk == null)
            {
                companyRisk = new CompanyRisk();
            }

            companyRisk.Id = marineDTO.Id.GetValueOrDefault();
            companyRisk.RiskId = marineDTO.RiskId.GetValueOrDefault();
            companyRisk.Description = marineDTO.Description;
            companyRisk.Policy = new CompanyPolicy
            {
                Id = marineDTO.PolicyId,
            };
            companyRisk.GroupCoverage = new GroupCoverage
            {
                Id = marineDTO.CoverageGroupId
            };
            companyRisk.MainInsured = new CompanyIssuanceInsured
            {
                BirthDate = marineDTO.BirthDate,
                Gender = marineDTO.Gender,
                IdentificationDocument = new IssuanceIdentificationDocument
                {
                    Number = marineDTO.DocumentNumber,
                    DocumentType = new IssuanceDocumentType
                    {
                        Id = marineDTO.DocumentType,
                        Description = marineDTO.DocumentTypeDescription,
                        SmallDescription = marineDTO.DocumentTypeSmallDescription
                    },
                    //ExpeditionDate = marineDTO.DocumentExpedition
                },
                Name = marineDTO.Name,
                CompanyName = new IssuanceCompanyName
                {
                    TradeName = marineDTO.CompanyName,
                    NameNum = marineDTO.NameNum,
                    Address = new IssuanceAddress { Description = marineDTO.Address },
                    Phone = new IssuancePhone { Description = marineDTO.Phone },
                    Email = new IssuanceEmail { Description = marineDTO.Email },
                    IsMain = marineDTO.IsMain
                },
                IndividualId = marineDTO.IndividualId,
                IndividualType = (IndividualType)marineDTO.IndividualType,
                CustomerType = (CustomerType)marineDTO.CustomerType,
                CustomerTypeDescription = marineDTO.CustomerTypeDescription,
                Profile = marineDTO.Profile,
                ScoreCredit = new ScoreCredit
                {
                    ScoreCreditId = marineDTO.ScoreCredit.GetValueOrDefault()
                }
            };
            companyRisk.CoveredRiskType = CoveredRiskType.Aircraft;
            companyRisk.Status = RiskStatusType.Original;
            companyRisk.OriginalStatus = RiskStatusType.Original;
            companyRisk.IsPersisted = true;

            return companyRisk;
        }
    }
}
