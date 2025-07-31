using Sistran.Company.Application.UniquePerson.Entities;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPET = Sistran.Core.Application.UniquePersonV1.Entities;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    public class CompanySarlaftIndividualBusiness
    {
        /// <summary>
        /// Creates the individual sarlaft.
        /// </summary>
        /// <param name="individualSarlaft">The individual sarlaft.</param>
        /// <param name="individualId">The individual identifier.</param>
        /// <param name="economiActivity">The economi activity.</param>
        /// <returns></returns>
        public Models.IndividualSarlaft CreateIndividualSarlaft(Models.IndividualSarlaft individualSarlaft, int individualId, int economiActivity)
        {
            Models.IndividualSarlaft individualSarlaftmodel = new Models.IndividualSarlaft();
            CompanyFinancialSarlaftBusiness companyFinancialSarlaftBusiness = new CompanyFinancialSarlaftBusiness();

            UPET.IndividualSarlaft individualsft = EntityAssembler.CreateIndividualSarlaftV1(individualSarlaft);
            individualsft.IndividualId = individualId;
            individualsft.FormNum = companyFinancialSarlaftBusiness.GetSarlaftYear(DateTime.Now.Year).ToString();
            individualsft.Year = DateTime.Now.Year;
            individualsft.EconomicActivityCode = economiActivity;
            individualsft.SecondEconomicActivityCode = economiActivity;
            individualsft.UserId = individualSarlaft.UserId;
            individualsft.BranchCode = individualSarlaft.BranchCode;
            individualsft.FillingDate = DateTime.Now;

            DataFacadeManager.Insert(individualsft);
            individualSarlaftmodel = ModelAssembler.CreateIndividualSarlaft(individualsft);

            if (individualSarlaft.FinancialSarlaft != null)
            {
                individualSarlaft.FinancialSarlaft.SarlaftId = individualSarlaftmodel.Id;
                individualSarlaftmodel.FinancialSarlaft = companyFinancialSarlaftBusiness.CreateFinancialSarlaft(individualSarlaft.FinancialSarlaft);
            }

            return individualSarlaftmodel;
        }

        /// <summary>
        /// Gets the individual sarlaft by individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public List<Models.IndividualSarlaft> GetIndividualSarlaftByIndividualId(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UPET.IndividualSarlaft.Properties.IndividualId, typeof(UPET.IndividualSarlaft).Name);
            filter.Equal();
            filter.Constant(individualId);

            var businessCollection =  DataFacadeManager.GetObjects(typeof(UPET.IndividualSarlaft), filter.GetPredicate());

            List<Models.IndividualSarlaft> IndividualSarlafts = ModelAssembler.CreateIndividualSarlafts(businessCollection);
            return IndividualSarlafts;
        }

        /// <summary>
        /// Actualizar Sarlaft
        /// </summary>
        /// <param name="individualSarlaft">The individual sarlaft.</param>
        /// <returns></returns>
        public Models.IndividualSarlaft UpdateIndividualSarlaft(Models.IndividualSarlaft individualSarlaft)
        {
            CompanyFinancialSarlaftBusiness companyFinancialSarlaftBusiness = new CompanyFinancialSarlaftBusiness();
            Models.IndividualSarlaft financialSarlafModel = new Models.IndividualSarlaft();
            financialSarlafModel.FinancialSarlaft = companyFinancialSarlaftBusiness.UpdateFinancialSarlaf(individualSarlaft.FinancialSarlaft);
            return financialSarlafModel;
        }



    }
}
