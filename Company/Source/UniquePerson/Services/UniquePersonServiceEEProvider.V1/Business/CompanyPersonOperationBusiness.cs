using Sistran.Company.Application.UniquePersonAplicationServices.DTOs;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.DAOs
{
    public class CompanyPersonOperationBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyPersonOperationBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        public CompanyPersonOperation GetCompanyPersonOperation(int operationId)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperPersonOperation();
                var result = coreProvider.GetPersonOperation(operationId);
                return imapper.Map<PersonOperation, CompanyPersonOperation>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyPersonOperation CreateCompanyPersonOperation(CompanyPersonOperation personOperation)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperPersonOperation();
                var personOperationCore = imapper.Map<CompanyPersonOperation, PersonOperation>(personOperation);
                //var result = coreProvider.CreatePersonOperation(personOperationCore);
                var result = DelegateService.UniquePersonServiceCore.CreatePersonOperation(personOperationCore);
                return imapper.Map<PersonOperation, CompanyPersonOperation>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public List<AuthorizationRequest> GetAuthorizationRequestByIndividualId(int individualId)
        {
            try
            {

                List<PersonOperation> CoreOperation = coreProvider.GetOperationTmp(individualId);
                List<AuthorizationRequest> authorizationRequests = new List<AuthorizationRequest>();

                foreach (PersonOperation operation in CoreOperation)
                {
                    AuthorizationRequest request = new AuthorizationRequest
                    {
                        Status = (Core.Application.AuthorizationPoliciesServices.Enums.TypeStatus)Enum.Parse(typeof(Core.Application.AuthorizationPoliciesServices.Enums.TypeStatus), operation.StatusId.ToString()),
                        AuthorizationRequestId = operation.RequestId,
                        FunctionType = (Core.Application.AuthorizationPoliciesServices.Enums.TypeFunction)Enum.Parse(typeof(Core.Application.AuthorizationPoliciesServices.Enums.TypeFunction), operation.FunctionId.ToString()),

                    };
                    authorizationRequests.Add(request);
                }

                return authorizationRequests;



            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public PersonDTO CreatePerson2gOperation(int personId, bool company)
        {
            try
            {
                PersonDTO personDTO = DelegateService.uniquePersonAplicationService.CreateAplicationPerson(
                ModelAssembler.CreatePerson2G(DelegateService.externalProxyService.GetPerson2gByPersonId(personId, company)));
                
                int providerId = DelegateService.uniquePersonAplicationService.GetAplicationSupplierByIndividualId(personDTO.Id).Id;
                CreateProvider2gOperation(providerId, personId, personDTO.Id);
                CreateBankTransfer2gOperation(personId, personDTO.Id);
                CreateTax2gOperation(personId, personDTO.Id);

                return personDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyDTO CreateCompany2gOperation(int personId, bool company)
        {
            try
            {
                CompanyDTO companyDTO = DelegateService.uniquePersonAplicationService.CreateAplicationCompany(
                ModelAssembler.CreateCompany2G(DelegateService.externalProxyService.GetCompany2gByPersonId(personId, company)));
                
                int providerId = DelegateService.uniquePersonAplicationService.GetAplicationSupplierByIndividualId(companyDTO.Id).Id;
                CreateProvider2gOperation(providerId, personId, companyDTO.Id);
                CreateBankTransfer2gOperation(personId, companyDTO.Id);
                CreateTax2gOperation(personId, companyDTO.Id);

                return companyDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public void CreateProvider2gOperation(int providerId, int personId, int individualId)
        {
            try
            {
                ProviderDTO providerDTO = null;
                if (providerId > 0)
                { 
                    providerDTO = ModelAssembler.CreateProvider2G(DelegateService.externalProxyService.GetProvider2g(personId), individualId);
                    if (providerDTO.ProviderTypeId > 0)
                    {
                        providerDTO.ModificationDate = DateTime.Now;
                        DelegateService.uniquePersonAplicationService.UpdateAplicationSupplier(providerDTO);
                    }
                }
                else
                {
                    providerDTO = ModelAssembler.CreateProvider2G(DelegateService.externalProxyService.GetProvider2g(personId), individualId);
                    if (providerDTO.ProviderTypeId > 0)
                    {
                        DelegateService.uniquePersonAplicationService.CreateAplicationSupplier(providerDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public void CreateBankTransfer2gOperation(int personId, int individualId)
        {
            try
            {
                var bankTransfers = DelegateService.externalProxyService.GetBankTransfer2g(personId);
                if (bankTransfers.Count > 0)
                {
                    DelegateService.uniquePersonAplicationService.CreateBankTransfers(ModelAssembler.CreateBankTransfersDTO(bankTransfers, individualId));
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public void CreateTax2gOperation(int personId, int individualId)
        {
            try
            {
                var taxes = DelegateService.externalProxyService.GetTax2g(personId);
                if (taxes.Count > 0)
                {
                    List<IndividualTaxExeptionDTO> individualTaxExeptionDTOs = ModelAssembler.CreateIndividualTaxExeptionDTO(taxes, individualId);
                    List<int> listTaxRateId = new List<int>();

                    foreach (IndividualTaxExeptionDTO tax in individualTaxExeptionDTOs)
                    {
                        var taxRate = DelegateService.underwritingServices.GetBusinessTaxRateByTaxIdbyAttributes
                            (tax.TaxId, tax.TaxCondition, null, null, null, null, null, null, null, null);

                        if (taxRate != null && listTaxRateId.Where(x => x == taxRate.Id).ToList().Count == 0)
                        {
                            tax.TaxRateId = taxRate.Id;
                            listTaxRateId.Add(taxRate.Id);
                        }
                    }

                    if (individualTaxExeptionDTOs.Where(x => x.TaxRateId > 0).ToList().Count > 0)
                    {
                        DelegateService.uniquePersonAplicationService.CreateIndividualTax(individualTaxExeptionDTOs.Where(x => x.TaxRateId > 0).ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
