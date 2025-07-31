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
    public class CompanyOperatingQuotaBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyOperatingQuotaBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        #region OperatingQuota V1
       
        public List<CompanyOperatingQuota> GetCompanyOperatingQuotaByIndividualId(int id)
        {
            try
            {
                var imapper = ModelAssembler.CreateMappertOperatingQuota();
                var result = coreProvider.GetOperatingQuotaByIndividualId(id);
                return imapper.Map<List<OperatingQuota>, List<CompanyOperatingQuota>>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyOperatingQuota> CreateCompanyOperatingQuota(List<Models.CompanyOperatingQuota> listOperatingQuota)
        {
            try
            {
                var imapper = ModelAssembler.CreateMappertOperatingQuota();
                var coreOperatingQuota = imapper.Map<List<CompanyOperatingQuota>, List<OperatingQuota>>(listOperatingQuota);
                var result = coreProvider.CreateOperatingQuota(coreOperatingQuota);
                return imapper.Map<List<OperatingQuota>, List<CompanyOperatingQuota>>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyOperatingQuota UpdateCompanyOperatingQuota(Models.CompanyOperatingQuota OperatingQuota)
        {
            try
            {
                var imapper = ModelAssembler.CreateMappertOperatingQuota();
                var coreOperatingQuota = imapper.Map<CompanyOperatingQuota, OperatingQuota>(OperatingQuota);
                var result = coreProvider.UpdateOperatingQuota(coreOperatingQuota);
                return imapper.Map<OperatingQuota, CompanyOperatingQuota>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public bool DeleteCompanyOperatingQuota(Models.CompanyOperatingQuota OperatingQuota)
        {
            try
            {
                var imapper = ModelAssembler.CreateMappertOperatingQuota();
                var coreOperatingQuota = imapper.Map<CompanyOperatingQuota, OperatingQuota>(OperatingQuota);
                var result = coreProvider.DeleteOperatingQuota(coreOperatingQuota);
                return true;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        #endregion OperatingQuota



    }
}
