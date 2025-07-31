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
    public class TaxCompanyBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public TaxCompanyBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        #region Individual Tax
        /// <summary>
        /// Crear una nueva Persona
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public List<CompanyIndividualTax> GetCompanyIndividualTaxExeptionByIndividualId(int  individualId)
        {
            try
            {
               var imapper = ModelAssembler.CreateMapperIndividualTax();
               
                var result = coreProvider.GetIndividualTaxByIndividualId(individualId);
                return imapper.Map<List<IndividualTax>, List<CompanyIndividualTax>>(result);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyIndividualTax CreateCompanyIndividualTax(CompanyIndividualTax companyIndividualTaxExeption)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperIndividualTax();

                var IndividualTaxExeption = imapper.Map<CompanyIndividualTax, IndividualTax>(companyIndividualTaxExeption);
                var result = coreProvider.CreateIndividualTax(IndividualTaxExeption);
                return imapper.Map<IndividualTax, CompanyIndividualTax>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public CompanyIndividualTaxExeption CreateCompanyIndividualTaxEx(CompanyIndividualTax companyIndividualTaxExeption)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperIndividualTax();

                var IndividualTaxExeption = imapper.Map<CompanyIndividualTax, IndividualTax>(companyIndividualTaxExeption);
                var result = coreProvider.CreateIndividualTax(IndividualTaxExeption);
                return imapper.Map<IndividualTax, CompanyIndividualTaxExeption>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyIndividualTaxExeption CreateCompanyIndividualTaxExeption(CompanyIndividualTaxExeption companyIndividualTaxExeption)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperIndividualTax();

                var IndividualTaxExeption = imapper.Map<CompanyIndividualTaxExeption, IndividualTaxExeption>(companyIndividualTaxExeption);
                var result = coreProvider.CreateIndividualTaxExeption(IndividualTaxExeption);
                return imapper.Map<IndividualTaxExeption, CompanyIndividualTaxExeption>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyIndividualTax UpdateCompanyIndividualTax(CompanyIndividualTax companyIndividualTaxExeption)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperIndividualTax();
                var IndividualTaxExeption = imapper.Map<CompanyIndividualTax, IndividualTax>(companyIndividualTaxExeption);
                var result = coreProvider.UpdateIndividualTax(IndividualTaxExeption);
                return imapper.Map<IndividualTax, CompanyIndividualTax>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyIndividualTaxExeption UpdateCompanyIndividualTaxExeption(CompanyIndividualTaxExeption companyIndividualTaxExeption)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperIndividualTax();

                var IndividualTaxExeption = imapper.Map<CompanyIndividualTaxExeption, IndividualTaxExeption>(companyIndividualTaxExeption);
                var result = coreProvider.UpdateIndividualTaxExeption(IndividualTaxExeption);
                return imapper.Map<IndividualTaxExeption, CompanyIndividualTaxExeption>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public void DeleteCompanyIndividualTax(CompanyIndividualTaxExeption companyIndividualTaxExeption)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperIndividualTax();
                var IndividualTaxExeption = imapper.Map<CompanyIndividualTaxExeption, IndividualTaxExeption>(companyIndividualTaxExeption);
                coreProvider.DeleteIndividualTaxExeption(IndividualTaxExeption);
              
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public void DeleteCompanyIndividualTaxExeption(CompanyIndividualTax companyIndividualTax)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperIndividualTax();
                var IndividualTax= imapper.Map<CompanyIndividualTax, IndividualTax>(companyIndividualTax);
                coreProvider.DeleteIndividualTax(IndividualTax);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion IndividualTax



    }
}
