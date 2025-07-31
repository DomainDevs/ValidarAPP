using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using UPV1 = Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    public class CompanyCoInsuredDAO
    {
        /// <summary>
        /// Crea el CoAsegurador de la compañia.
        /// </summary>
        /// <param name="companyCoInsured">Datos de Coasegurador</param>
        /// <returns>Retorna la Creacion del Coasegurador</returns>
        public UPV1.CompanyCoInsured CreateCompanyCoInsured(UPV1.CompanyCoInsured companyCoInsured)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            CoInsuranceCompany companyInsured = null;
            if (companyCoInsured != null)
            {
                using (Transaction transaction = new Transaction())
                {
                    EntityAssembler.CreateCompanyCoInsured(companyCoInsured);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(companyInsured);
                    transaction.Complete();
                }
            }
            var result = ModelAssembler.CreateCoInsured(companyInsured);
            return result;
        }
        /// <summary>
        /// Actualiza el CoAsegurador de la Compañia. 
        /// </summary>
        /// <param name="companyCoInsured">Datos del Coasegurado.</param>
        /// <returns>retorna la Actualizacion del Coasegurado.</returns>
        public UPV1.CompanyCoInsured UpdateCompanyCoInsured(UPV1.CompanyCoInsured companyCoInsured)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            CoInsuranceCompany companyInsured = null;
            if (companyCoInsured != null)
            {
                EntityAssembler.CreateCompanyCoInsured(companyCoInsured);
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(companyInsured);

            }
            var result = ModelAssembler.CreateCoInsured(companyInsured);
            return result;
        }
        /// <summary>
        /// Obtiene los coasegurados por Compañia
        /// </summary>
        /// <param name="IndividualId">Codigo de la compañia</param>
        /// <returns>retorna los Dqatos de la Compañia</returns>
        public UPV1.CompanyCoInsured GetCompanyCoInsuredIndividualId(int IndividualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            CoInsuranceCompany coInsuranceCompany = new CoInsuranceCompany();
            PrimaryKey key = UniquePersonV1.Entities.Company.CreatePrimaryKey(IndividualId);
            if (IndividualId != 0)
            {
                filter.Property(CoInsuranceCompany.Properties.IndividualId);
                filter.Equal();
                filter.Constant(IndividualId);
            }            
            var companyEntity = (CoInsuranceCompany)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            return ModelAssembler.CreateCoInsured(companyEntity);
        }
        /// <summary>
        /// Obtiene los datos del Coseagurado por id Tributario.
        /// </summary>
        /// <param name="tributaryNo">Id Del Tributario.</param>
        /// <returns>Retorna Coasegurado del Tributario.</returns>
        public UPV1.CompanyCoInsured GetCompanyCoInsuredTributaryID(string tributaryNo)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            CoInsuranceCompany coInsuranceCompany = new CoInsuranceCompany();
            PrimaryKey key = UniquePersonV1.Entities.Company.CreatePrimaryKey(Convert.ToInt32(tributaryNo));
            if (tributaryNo != null)
            {
                filter.Property(CoInsuranceCompany.Properties.TributaryIdNo);
                filter.Equal();
                filter.Constant(tributaryNo + "%");
            }
            CoInsuranceCompany companyEntity = (CoInsuranceCompany)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            return ModelAssembler.CreateCoInsured(companyEntity);
        }
    }
}

