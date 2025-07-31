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

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    public class CompanyFinancialSarlaftBusiness
    {


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

                DataFacadeManager.Update(financialSarlafEntity);
                return ModelAssembler.CreateFinancialSarlaft(financialSarlafEntity);
            }
            else
            {
                return CreateFinancialSarlaft(financialSarlaft);

            }
        }


        public List<Models.FinancialSarlaf> GetFinancialSarlaftByFilter(ObjectCriteriaBuilder filter)
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(FinancialSarlaft), filter.GetPredicate()));
            return ModelAssembler.CreateFinancialSarlaft(businessCollection);
        }

        public Models.FinancialSarlaf CreateFinancialSarlaft(Models.FinancialSarlaf sarlaft)
        {
            FinancialSarlaft sarlaftEntity = EntityAssembler.CreateSarlaft(sarlaft);
            DataFacadeManager.Insert(sarlaftEntity);
            return ModelAssembler.CreateFinancialSarlaft(sarlaftEntity);
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
