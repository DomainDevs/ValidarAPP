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
    public class CompanyInsuredBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyInsuredBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }


        #region Insured V1

        public List<Models.CompanyInsuredSegment> GetInsuredSegment()
        {
            var imapper = ModelAssembler.CreateMapperInsuredSegment();
            var result = coreProvider.GetInsuredSegment();
            return imapper.Map<List<InsuredSegment>, List<CompanyInsuredSegment>>(result);
        }

        public List<Models.CompanyInsuredProfile> GetInsuredProfile()
        {
            var imapper = ModelAssembler.CreateMapperInsuredProfile();
            var result = coreProvider.GetInsuredProfile();
            return imapper.Map<List<InsuredProfile>, List<CompanyInsuredProfile>>(result);
        }

        /// <summary>
        /// Crear un nuevo Asegurado
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public CompanyInsured CreateCompanyInsured(CompanyInsured companyInsured)
        {
            try
            {
                var imapper =  ModelAssembler.CreateMapperInsured();
                var insuredCore = imapper.Map<CompanyInsured, Insured>(companyInsured);
                var result = coreProvider.CreateInsured(insuredCore);
                return imapper.Map<Insured, CompanyInsured>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Actualiza el asegurado
        /// </summary>
        /// <param name="companyInsured"></param>
        /// <returns></returns>
        public CompanyInsured UpdateCompanyInsured(CompanyInsured companyInsured)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperInsured();
                var insuredCore = imapper.Map<CompanyInsured, Insured>(companyInsured);
                var result = coreProvider.UpdateInsured(insuredCore);
                return imapper.Map<Insured, CompanyInsured>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        

        /// <summary>
        /// Actualiza el asegurado para facturacion electronica
        /// </summary>
        /// <param name="companyInsured"></param>
        /// <returns></returns>
        public CompanyInsured UpdateCompanyInsuredElectronicBilling(CompanyInsured companyInsured)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperInsured();
                var insuredCore = imapper.Map<CompanyInsured, Insured>(companyInsured);
                var result = coreProvider.UpdateInsuredElectronicBilling(insuredCore);
                return imapper.Map<Insured, CompanyInsured>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }
        /// <summary>
        /// Trae Asegurado por IndividualId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CompanyInsured GetCompanyInsuredByIndividualId(int id)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperInsured();
                var result = coreProvider.GetInsuredByIndividualId(id);
                return imapper.Map<Insured, CompanyInsured>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        
        /// <summary>
        /// Trae Asegurado por IndividualId para facturacion electronica
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CompanyInsured GetCompanyInsuredElectronicBillingByIndividualId(int id)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperInsured();
                var result = coreProvider.GetInsuredElectronicBillingByIndividualId(id);
                return imapper.Map<Insured, CompanyInsured>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public CompanyInsuredConcept CreateCompanyInsuredConcept(CompanyInsuredConcept insuredConcept)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperInsured();
                var insuredCore = imapper.Map<CompanyInsuredConcept, InsuredConcept>(insuredConcept);
                var result = coreProvider.CreateInsuredConcept(insuredCore);
                return imapper.Map<InsuredConcept, CompanyInsuredConcept>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public CompanyInsuredConcept UpdateCompanyInsuredConcept(CompanyInsuredConcept insuredConcept)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperInsured();
                var insuredCore = imapper.Map<CompanyInsuredConcept, InsuredConcept>(insuredConcept);
                var result = coreProvider.UpdateInsuredConcept(insuredCore);
                return imapper.Map<InsuredConcept, CompanyInsuredConcept>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyInsuredAgent CreateCompanyInsuredAgent(CompanyAgency insuredAgency, int individualId)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperInsured();
                var insuredCore = imapper.Map<CompanyAgency, Agency>(insuredAgency);
                var result = coreProvider.CreateInsuredAgent(insuredCore, individualId);
                return imapper.Map<InsuredAgent, CompanyInsuredAgent>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyInsuredAgent UpdateCompanyInsuredAgent(CompanyAgency insuredAgency, int individualId)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperInsured();
                var insuredCore = imapper.Map<CompanyAgency, Agency>(insuredAgency);
                var result = coreProvider.UpdateInsuredAgent(insuredCore, individualId);
                return imapper.Map<InsuredAgent, CompanyInsuredAgent>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion Insured V1
    }
}
