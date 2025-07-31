using AutoMapper;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.DAOs;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    public class CompanySupplierBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanySupplierBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }



        /// <summary>
        /// Obtener todos los SupplierAccountingConcept por Supplier
        /// </summary> 
        public List<CompanySupplierAccountingConcept> GetCompanySupplierAccountingConceptsBySupplierId(int SupplierId)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperSupplierAccountingConcept();
                var result = coreProvider.GetSupplierAccountingConceptsBySupplierId(SupplierId);
                return imapper.Map<List<SupplierAccountingConcept>, List<CompanySupplierAccountingConcept>>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener todos los AccountingConcept
        /// </summary> 
        public List<CompanyAccountingConcept> GetCompanyAccountingConcepts()
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperAccountingConcept();
                var result = coreProvider.GetAccountingConcepts();
                return imapper.Map<List<AccountingConcept>, List<CompanyAccountingConcept>>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }



        public List<CompanySupplierProfile> GetCompanySupplierProfiles(int suppilierTypeId)
        {

            try
            {
                var imapper = ModelAssembler.CreateMapperSupplierProfile();
                var result = coreProvider.GetSupplierTypeProfileById(suppilierTypeId);
                return imapper.Map<List<Core.Application.UniquePersonService.V1.Models.SupplierProfile>, List<CompanySupplierProfile>>(result);             
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public CompanySupplier GetCompanySupplierById(int SupplierId)
        {

            try
            {
                var imapper = ModelAssembler.CreateMapperSupplier();
                var result = coreProvider.GetSupplierById(SupplierId);
                return imapper.Map<Supplier, CompanySupplier>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }



        public List<CompanySupplier> GetCompanySuppliers()
        {

            try
            {
                var imapper = ModelAssembler.CreateMapperSupplier();
                var result = coreProvider.GetSuppliers();
                return imapper.Map<List<Supplier>, List<CompanySupplier>>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }


        public List<CompanySupplierType> GetCompanySupplierTypes()
        {

            try
            {
                var imapper = ModelAssembler.CreateMapperSupplier();
                var result = coreProvider.GetSupplierTypes();
                return imapper.Map<List<SupplierType>, List<CompanySupplierType>>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public List<CompanySupplierDeclinedType> GetCompanySupplierDeclinedType()
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperSupplier();
                var result = coreProvider.GetSupplierDeclinedTypes();
                return imapper.Map<List<SupplierDeclinedType>,List<CompanySupplierDeclinedType>>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public List<CompanyGroupSupplier> GetCompanyGroupSupplier()
        {
            try
            {
                var imapper = ModelAssembler.GetCompanyGroupSupplier();
                var result = coreProvider.GetGroupSupplier();
                return imapper.Map<List<GroupSupplier>, List<CompanyGroupSupplier>>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public CompanySupplier CreateCompanySupplier(CompanySupplier companySupplier)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperSupplier();
                var resultMapper = imapper.Map<CompanySupplier,Supplier>(companySupplier);
                var result = coreProvider.CreateSupplier(resultMapper);
                return imapper.Map<Supplier, CompanySupplier>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanySupplier UpdateCompanySupplier(CompanySupplier companySupplier)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperSupplier();
                var resultMapper = imapper.Map<CompanySupplier, Supplier>(companySupplier);
                var result = coreProvider.UpdateSupplier(resultMapper);
                return imapper.Map<Supplier, CompanySupplier>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanySupplier GetCompanySupplierByIndividualId(int individualId)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperSupplier();
                var result = coreProvider.GetSupplierByIndividualId(individualId);
                return imapper.Map<Supplier, CompanySupplier>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
