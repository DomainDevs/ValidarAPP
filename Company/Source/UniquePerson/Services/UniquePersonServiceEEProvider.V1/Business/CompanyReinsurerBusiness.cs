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
    public class CompanyReinsurerBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyReinsurerBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        #region person
        #region Reinsured V1
        /// <summary>
        /// Crear una nueva Persona
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public CompanyReInsurer GetCompanyReInsurerByIndividualId(int id)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperReInsurer();
                var result = coreProvider.GetReInsurerByIndividualId(id);
                return imapper.Map<ReInsurer, CompanyReInsurer>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyReInsurer CreateCompanyReinsurer(CompanyReInsurer insurer)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperReInsurer();
                var reInsurerCore = imapper.Map<CompanyReInsurer, ReInsurer>(insurer);
                var result = coreProvider.CreateReinsurer(reInsurerCore);
                return imapper.Map<ReInsurer, CompanyReInsurer>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyReInsurer UpdateCompanyReInsurerBusiness(CompanyReInsurer insurer)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperReInsurer();
                var reInsurerCore = imapper.Map<CompanyReInsurer, ReInsurer>(insurer);
                var result = coreProvider.UpdateReinsurer(reInsurerCore);
                return imapper.Map<ReInsurer, CompanyReInsurer>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        
        #endregion Reinsured V1

        #endregion person



    }
}
