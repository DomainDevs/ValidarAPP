using AutoMapper;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.DAOs;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using entities = Sistran.Core.Application.UniquePersonV1.Entities;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    public class CompanyIndividualRoleBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyIndividualRoleBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        #region IndividualRole V1

        public List<CompanyIndividualRole> GetCompanyIndividualRoleByIndividualId(int id)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperIndividualRole();
                var result = coreProvider.GetIndividualRoleByIndividualId(id);
                return imapper.Map<List<IndividualRole>, List<CompanyIndividualRole>>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public void CretateCompanyIndividualRoleByIndividualId(CompanyIndividualRole companyIndividualRole)
        {
            entities.IndividualRole entityIndividualRole = EntityAssembler.CreateIndividualRole(companyIndividualRole);
            DataFacadeManager.Insert(entityIndividualRole);
        }

        #endregion IndividualRole



    }
}
