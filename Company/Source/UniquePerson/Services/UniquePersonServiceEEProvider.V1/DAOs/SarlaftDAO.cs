using Sistran.Company.Application.UniquePerson.Entities;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.UniquePersonServices.V1.DAOs
{
    /// <summary>
    /// Sarlaft
    /// </summary>
    public class SarlaftDAO
    {
        /// <summary>
        /// Crear Sarlaft
        /// </summary>
        /// <param name="sarlaft">Sarlaft</param>
        /// <returns></returns>
        public Models.FinancialSarlaf CreateFinancialSarlaft(Models.FinancialSarlaf sarlaft)
        {
            FinancialSarlaft sarlaftEntity = EntityAssembler.CreateSarlaft(sarlaft);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(sarlaftEntity);
            return ModelAssembler.CreateFinancialSarlaft(sarlaftEntity);
        }

        /// <summary>
        /// Actualizar Financial Sarlaft
        /// </summary>
        /// <param name="financialSarlaft">Financial Sarlaft</param>
        /// <returns></returns>
        public Models.FinancialSarlaf UpdateFinancialSarlaf(Models.FinancialSarlaf financialSarlaft)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(FinancialSarlaft.Properties.SarlaftId, typeof(FinancialSarlaft).Name);
            filter.Equal();
            filter.Constant(financialSarlaft.SarlaftId);

            List<Models.FinancialSarlaf> financialSarlafts = GetFinancialSarlaftByFilter(filter);

            if (financialSarlafts.Count > 0)
            {
                PrimaryKey key = FinancialSarlaft.CreatePrimaryKey(financialSarlaft.SarlaftId);
                FinancialSarlaft financialSarlafEntity = (FinancialSarlaft)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);



                financialSarlafEntity.IncomeAmount = financialSarlaft.IncomeAmount;
                financialSarlafEntity.ExpenseAmount = financialSarlaft.ExpenseAmount;
                financialSarlafEntity.ExtraIncomeAmount = financialSarlaft.ExtraIncomeAmount;
                financialSarlafEntity.AssetsAmount = financialSarlaft.AssetsAmount;
                financialSarlafEntity.LiabilitiesAmount = financialSarlaft.LiabilitiesAmount;
                financialSarlafEntity.Description = financialSarlaft.Description;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(financialSarlafEntity);
                return ModelAssembler.CreateFinancialSarlaft(financialSarlafEntity);
            }
            else
            {
                return CreateFinancialSarlaft(financialSarlaft);

            }
        }

        /// <summary>
        /// Gets the financial sarlaft by filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public List<Models.FinancialSarlaf> GetFinancialSarlaftByFilter(ObjectCriteriaBuilder filter)
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(FinancialSarlaft), filter.GetPredicate()));
            return ModelAssembler.CreateFinancialSarlaft(businessCollection);
        }

        /// <summary>
        /// Gets the financial sarlaft by sarlaft identifier.
        /// </summary>
        /// <param name="sarlaftId">The sarlaft identifier.</param>
        /// <returns></returns>
        public Models.FinancialSarlaf GetFinancialSarlaftBySarlaftId(int sarlaftId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(FinancialSarlaft.Properties.SarlaftId, typeof(FinancialSarlaft).Name);
            filter.Equal();
            filter.Constant(sarlaftId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(FinancialSarlaft), filter.GetPredicate()));
            return ModelAssembler.CreateFinancialSarlaft(businessCollection).FirstOrDefault();
        }

        /// <summary>
        /// Obtener exoneracion asociada a un individuo
        /// </summary>
        /// <param name="individualId">Id del individuo</param>
        /// <returns></returns>
        public Models.CompanySarlaftExoneration GetSarlaftExonerationByIndividualId(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(IndividualSarlaftExoneration.Properties.IndividualId, typeof(IndividualSarlaftExoneration).Name);
            filter.Equal();
            filter.Constant(individualId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(IndividualSarlaftExoneration), filter.GetPredicate()));
            Models.CompanySarlaftExoneration sarlaftExoneration = ModelAssembler.CreateSarlaftExonerations(businessCollection).FirstOrDefault();

            return sarlaftExoneration;
        }


        /// <summary>
        /// Crear nuevo email
        /// </summary>
        /// <param name="email">Modelo email</param>
        /// <returns></returns>
        public Models.CompanySarlaftExoneration CreateSarlaftExoneration(Models.CompanySarlaftExoneration sarlaftExoneration, int individualId)
        {
            IndividualSarlaftExoneration sarlaftExonerationEntity = EntityAssembler.CreateSarlaftExoneration(sarlaftExoneration);
            sarlaftExonerationEntity.IndividualId = individualId;
            DataFacadeManager.Instance.GetDataFacade().InsertObject(sarlaftExonerationEntity);
            return ModelAssembler.CreateSarlaftExoneration(sarlaftExonerationEntity);

        }

        /// <summary>
        /// Actualizar sarlaftexoneration
        /// </summary>
        /// <param name="email">Modelo sarlaftexoneration</param>
        /// <returns></returns>
        public Models.CompanySarlaftExoneration UpdateSarlaftExoneration(Models.CompanySarlaftExoneration sarlaftExoneration, int individualId)
        {
            PrimaryKey key = IndividualSarlaftExoneration.CreatePrimaryKey(individualId);
            IndividualSarlaftExoneration sarlaftExonerationEntity = (IndividualSarlaftExoneration)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            if (sarlaftExonerationEntity != null)
            {
                sarlaftExonerationEntity.ExonerationTypeCode = sarlaftExoneration.ExonerationType.Id;
                sarlaftExonerationEntity.IsExonerated = sarlaftExoneration.IsExonerated;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(sarlaftExonerationEntity);
                return ModelAssembler.CreateSarlaftExoneration(sarlaftExonerationEntity);
            }
            else
            {
                return CreateSarlaftExoneration(sarlaftExoneration, individualId);

            }
        }


        /// <summary>
        /// Crear SarlaftYear
        /// </summary>
        /// <param name="sarlaft">Sarlaft</param>
        /// <returns></returns>
        public int GetSarlaftYear(int year)
        {
            PrimaryKey key = SarlaftYear.CreatePrimaryKey(year);
            SarlaftYear sarlaftYearEntity = (SarlaftYear)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            if (sarlaftYearEntity == null)
            {
                sarlaftYearEntity = EntityAssembler.CreateSarlaftYear(year);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(sarlaftYearEntity);
            }
            else
            {
                sarlaftYearEntity.FormNum = sarlaftYearEntity.FormNum + 1;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(sarlaftYearEntity);
            }

            return (int)sarlaftYearEntity.FormNum;
        }

    }
}
