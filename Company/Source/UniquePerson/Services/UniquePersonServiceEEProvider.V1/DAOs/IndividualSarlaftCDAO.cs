using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using UPET = Sistran.Company.Application.UniquePerson.Entities;

namespace Sistran.Company.Application.UniquePersonServices.V1.DAOs
{

    /// <summary>
    /// Sarlaft Persona
    /// </summary>
    public class IndividualSarlaftDAO
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
            SarlaftDAO sarlaftDAO = new SarlaftDAO();

            UPET.IndividualSarlaft individualsft = EntityAssembler.CreateIndividualSarlaft(individualSarlaft);
            individualsft.IndividualId = individualId;
            individualsft.FormNum = sarlaftDAO.GetSarlaftYear(DateTime.Now.Year).ToString();
            individualsft.Year = DateTime.Now.Year;
            individualsft.EconomicActivityCode = economiActivity;
            individualsft.SecondEconomicActivityCode = economiActivity;
            individualsft.UserId = individualSarlaft.UserId;
            individualsft.BranchCode = individualSarlaft.BranchCode;
            DataFacadeManager.Instance.GetDataFacade().InsertObject(individualsft);
            individualSarlaftmodel = ModelAssembler.CreateIndividualSarlaft(individualsft);

            if (individualSarlaft.FinancialSarlaft != null)
            {
                individualSarlaft.FinancialSarlaft.SarlaftId = individualSarlaftmodel.Id;
                individualSarlaftmodel.FinancialSarlaft = sarlaftDAO.CreateFinancialSarlaft(individualSarlaft.FinancialSarlaft);
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

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPET.IndividualSarlaft), filter.GetPredicate()));

            List<Models.IndividualSarlaft> IndividualSarlafts = ModelAssembler.CreateIndividualSarlafts(businessCollection);
            return IndividualSarlafts;


        }

        /// <summary>
        /// Actualizar Sarlaft
        /// </summary>
        /// <param name="individualSarlaft">The individual sarlaft.</param>
        /// <returns></returns>
        public Models.FinancialSarlaf UpdateIndividualSarlaft(Models.IndividualSarlaft individualSarlaft)
        {
            SarlaftDAO sarlaftDAO = new SarlaftDAO();
            Models.FinancialSarlaf financialSarlafModel = new Models.FinancialSarlaf();
            financialSarlafModel = sarlaftDAO.UpdateFinancialSarlaf(individualSarlaft.FinancialSarlaft);
            return financialSarlafModel;

        }

        public Models.IndividualSarlaft UpdateIndividualSarlafts(Models.IndividualSarlaft individualSarlaft)
        {
            SarlaftDAO sarlaftDAO = new SarlaftDAO();
            Models.IndividualSarlaft financialSarlafModel = new Models.IndividualSarlaft();
            financialSarlafModel.FinancialSarlaft = sarlaftDAO.UpdateFinancialSarlaf(individualSarlaft.FinancialSarlaft);
            return financialSarlafModel;

        }

    }
}
