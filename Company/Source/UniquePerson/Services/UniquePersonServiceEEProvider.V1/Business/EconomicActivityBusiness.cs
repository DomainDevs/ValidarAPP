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
    public class EconomicActivityBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public EconomicActivityBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        /// <summary>
        /// Crear una nueva Persona
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public CompanyEconomicActivity GetEconomicActivitiesById(int id)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperPerson();
                var result = coreProvider.GetEconomicActivitiesById(id);
                return imapper.Map<EconomicActivity, CompanyEconomicActivity>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


    }
}
