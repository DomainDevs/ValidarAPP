using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using UP = Sistran.Core.Application.UniquePersonV1.Entities;
using cl = Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business;
namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    public class CompanyEmployeeBusiness
    {

        /// <summary>
        /// Crear información basica empleado
        /// </summary>
        public CompanyEmployee CreateEmployee(CompanyEmployee employee)
        {
            if (GetEmployeeByIndividualId(employee.IndividualId) == null || GetEmployeeByIndividualId(employee.IndividualId)?.IndividualId == 0)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                UP.Employee entityEmploye = EntityAssembler.CreateEmployeeEntity(employee);
                DataFacadeManager.Insert(entityEmploye);

               
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.CreateSupplierInformationBasic");
                return ModelAssembler.CreateEmployeeAssembler(entityEmploye);
            }
            else 
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                UpdateEmployeeBasic(employee);
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.UpdateProvider");
                return employee;
            }
           
        }

        /// <summary>
        /// Actualizar empleado
        /// </summary>        
        public CompanyEmployee UpdateEmployee(CompanyEmployee employee)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            UpdateEmployeeBasic(employee);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.UpdateProvider");
            return employee;
        }

        /// <summary>
        /// actualizar empleado
        /// </summary>
        /// <param name="supplier">modelo supplier</param>
        /// <returns></returns>
        private void UpdateEmployeeBasic(CompanyEmployee employee)
        {

            
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = UP.Employee.CreatePrimaryKey(employee.IndividualId);
            UP.Employee entityEmploye = (UP.Employee)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            if (entityEmploye != null)
            {
                entityEmploye.BranchCode = employee.BranchId;
                entityEmploye.FileNumber = employee.FileNumber;
                entityEmploye.EgressDate = employee.EgressDate;
                entityEmploye.ModificationDate = employee.ModificationDate;
                entityEmploye.DeclinedTypeCode = employee.DeclinedTypeId;
                entityEmploye.Annotation = employee.Annotation;

            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.UpdateSupplierInformationBasic");
            DataFacadeManager.Update(entityEmploye);
        }


        /// <summary>
        /// Consultar empleado por individualID
        /// </summary>
        public CompanyEmployee GetEmployeeByIndividualId(int individualId)
        {
            try
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                CompanyEmployee supplier = new CompanyEmployee();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(UP.Employee.Properties.IndividualId, typeof(UP.Employee).Name);
                filter.Equal();
                filter.Constant(individualId);
                BusinessCollection businessCollection = new BusinessCollection();
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessCollection = new BusinessCollection(daf.SelectObjects(typeof(UP.Employee), filter.GetPredicate()));
                }

                supplier = ModelAssembler.CreateEmployeeBusiness(businessCollection).FirstOrDefault();
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetSupplierByIndividualId");
                return supplier;
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }

        /// <summary>
        /// Consultar empleado por individualID
        /// </summary>
        public CompanyEmployee GetEmployeeByIndividualAndCode(int individualId, string codeEmployee)
        {
            try
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                CompanyEmployee supplier = new CompanyEmployee();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(UP.Employee.Properties.IndividualId, typeof(UP.Employee).Name);
                filter.Equal();
                filter.Constant(individualId);
                filter.And();
                filter.Property(UP.Employee.Properties.FileNumber, typeof(UP.Employee).Name);
                filter.Equal();
                filter.Constant(codeEmployee);
                BusinessCollection businessCollection = new BusinessCollection();
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessCollection = new BusinessCollection(daf.SelectObjects(typeof(UP.Employee), filter.GetPredicate()));
                }

                supplier = ModelAssembler.CreateEmployeeBusiness(businessCollection).FirstOrDefault();
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetSupplierByIndividualId");
                return supplier;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }
    }
}
