using Sistran.Company.Application.UniquePersonServices.V1.DAOs;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.DAOs;
using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CoreDao = Sistran.Core.Application.UniquePersonService.V1.DAOs;

namespace Sistran.Company.Application.UniquePersonServices.V1.Entities.DAOs
{
    /// <summary>
    /// Personas
    /// </summary>
    public class EmployeeDAO
    {

        /// <summary>
        /// Busca datos de una persona
        /// </summary>
        /// <param name="individualId">IndividualId</param>
        /// <returns></returns>
        /*public Models.CompanyEmployee GetEmployeeByIndividualId(int individualId)
        {
            CoreDao.EmployeeDAO employeeDAO = new CoreDao.EmployeeDAO();
            //return ModelAssembler.CreateEmployeeAssembler(employeeDAO.GetEmployeePersonByIndividualId(individualId));
            return ModelAssembler.CreateEmployeeAssembler(employeeDAO.GetEmployeeByIndividualId(individualId));
        }

        /// <summary>
        /// crear nueva persona
        /// </summary>
        /// <param name="person">datos persona</param>
        /// <returns></returns>
        public Models.CompanyEmployee CreateEmployee(Models.CompanyEmployee employee)
        {
            try
            {
                CoreDao.EmployeeDAO employeeDAO = new CoreDao.EmployeeDAO();
                return ModelAssembler.CreateEmployeeAssembler(employeeDAO.CreateEmployee(EntityAssembler.CreateEmployeeEntity(employee)));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Actualizar persona
        /// </summary>
        /// <param name="person">datos persona</param>
        /// <returns></returns>
        public Models.CompanyEmployee UpdateEmployee(Models.CompanyEmployee employee)
        {
            CoreDao.EmployeeDAO employeeDAO = new CoreDao.EmployeeDAO();
            return ModelAssembler.CreateEmployeeAssembler(employeeDAO.UpdateEmployee(EntityAssembler.CreateEmployeeEntity(employee)));
        }*/
    }
}
