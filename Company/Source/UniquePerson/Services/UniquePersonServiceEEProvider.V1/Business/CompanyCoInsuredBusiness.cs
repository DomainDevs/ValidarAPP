using AutoMapper;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Company.Application.UniquePersonServices.V1.Entities.DAOs;


namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    public class CompanyCoInsuredBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyCoInsuredBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        public CompanyCoInsured GetCompanyCoInsured(int id)
        {
            try
            {
                var imapper = ModelAssembler.CretaeCoInsured();
                var result = coreProvider.GetCompanyCoInsuredIndividualId(id);
                var companyCoInsured = imapper.Map<models.CompanyCoInsured, CompanyCoInsured>(result);
                return companyCoInsured;
            }
            catch (Exception ex )
            {
                throw new BusinessException(ex.Message, ex);
            }
                
        }
        public CompanyCoInsured CreateCompanyCoInsured(CompanyCoInsured companyCoInsured)
        {
            try
            {   
                var mapper = ModelAssembler.CretaeCoInsured();
                var coInsured = mapper.Map<CompanyCoInsured, models.CompanyCoInsured>(companyCoInsured);
                var result = coreProvider.CreateCompanyCoInsured(coInsured);
                CompanyCoInsured companyCoInsureds = mapper.Map<models.CompanyCoInsured, CompanyCoInsured>(result);
                return companyCoInsureds;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }
        public CompanyCoInsured UpdateCompanyCoInsured(CompanyCoInsured companyCoInsured)
        {
            try
            {
                
                var map = ModelAssembler.CretaeCoInsured();
                var coInsured = map.Map<CompanyCoInsured, models.CompanyCoInsured>(companyCoInsured);
                var result = coreProvider.UpdateCompanyCoInsured(coInsured);
                var companyCoInsureds = map.Map<models.CompanyCoInsured, CompanyCoInsured>(result);
                return companyCoInsureds;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

    }
}
