using AutoMapper;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    class CompanyLabourPersonBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreLabourPerson;

        public CompanyLabourPersonBusiness()
        {
            coreLabourPerson = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        /// <summary>
        /// Guardar la informacion laboral de una persona
        /// </summary>
        /// <param name="personJob">Modelo PersonJob</param>
        /// <returns></returns>
        public Models.CompanyLabourPerson CreateLabourPerson(Models.CompanyLabourPerson personJob, int individualId)
        {
            
            var imapper = ModelAssembler.CreateMapperPerson();
            var personJobCore = imapper.Map<Models.CompanyLabourPerson, LabourPerson>(personJob);
            var result = coreLabourPerson.CreateLabourPerson(personJobCore, individualId);
            return imapper.Map<LabourPerson, Models.CompanyLabourPerson>(result);
        }
        /// <summary>
        /// Actualizar los datos la borales de una persona
        /// </summary>
        /// <param name="personJob">Modelo PersonJob</param>
        /// <returns></returns>
        public Models.CompanyLabourPerson UpdateLabourPerson(Models.CompanyLabourPerson personJob)
        {

            var imapper = ModelAssembler.CreateMapperPerson();
            var personJobCore = imapper.Map<Models.CompanyLabourPerson, LabourPerson>(personJob);
            var result = coreLabourPerson.UpdateLabourPerson(personJobCore);
            return imapper.Map<LabourPerson, Models.CompanyLabourPerson>(result);

        }
        /// <summary>
        /// Buscar la informacion laboral de una persona
        /// </summary>
        /// <param name="individualId">IndividualId</param>
        /// <returns></returns>
        public Models.CompanyLabourPerson GetLabourPersonByIndividualId(int individualId)
        {

            try
            {
                var imapper = ModelAssembler.CreateMapperPerson();
                var result = coreLabourPerson.GetLabourPersonByIndividualId(individualId);
                return imapper.Map<LabourPerson, Models.CompanyLabourPerson>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
