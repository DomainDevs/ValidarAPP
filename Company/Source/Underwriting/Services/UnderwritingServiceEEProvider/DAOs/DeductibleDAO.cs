using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs
{
    public class DeductibleDAO
    {
        /// <summary>
        /// Obtiene los deducibles
        /// </summary>
        /// <returns>Model de deducible</returns>
        public List<Deductible> GetAllCompanyDeductibles()
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            CompanyDeductibleView view = new CompanyDeductibleView();
            ViewBuilder builder = new ViewBuilder("CompanyDeductibleView");
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            List<Deductible> deductibles = ModelAssembler.CreateDeductibles(view.Deductibles);

            foreach (Deductible deductible in deductibles)
            {
                PARAMEN.DeductibleUnit deductibleUnit = view.DeductibleUnits.Cast<PARAMEN.DeductibleUnit>().FirstOrDefault(x => x.DeductUnitCode == deductible.DeductibleUnit.Id);
                if (deductibleUnit != null && deductible.DeductibleUnit != null)
                {
                    deductible.DeductibleUnit.Description = deductibleUnit.Description;
                    deductible.Description = deductible.DeductValue.ToString() + " " + deductibleUnit.Description;
                }
                deductibleUnit = view.MinimumDeductibleUnits.Cast<PARAMEN.DeductibleUnit>().FirstOrDefault(x => x.DeductUnitCode == deductible.MinDeductibleUnit.Id);
                if (deductibleUnit != null && deductible.MinDeductibleUnit != null)
                {
                    deductible.MinDeductibleUnit.Description = deductibleUnit.Description;
                }
                PARAMEN.DeductibleSubject deductibleSubject = view.DeductibleSubjects.Cast<PARAMEN.DeductibleSubject>().FirstOrDefault(x => x.DeductSubjectCode == deductible.DeductibleSubject.Id);
                if (deductibleSubject != null && deductible.DeductibleSubject != null)
                {
                    deductible.DeductibleSubject.Description = deductibleSubject.Description;
                }
                deductibleSubject = view.MinimumDeductibleSubjects.Cast<PARAMEN.DeductibleSubject>().FirstOrDefault(x => x.DeductSubjectCode == deductible.MinDeductibleSubject.Id);
                if (deductibleSubject != null && deductible.MinDeductibleSubject != null)
                {
                    deductible.MinDeductibleSubject.Description = deductibleSubject.Description;
                }
                COMMEN.Currency currency = view.Currencies.Cast<COMMEN.Currency>().FirstOrDefault(x => x.CurrencyCode == deductible.Currency.Id);
                if (currency != null && deductible.Currency != null)
                {
                    deductible.Currency.Description = currency.Description;
                    deductible.Description += "(" + deductible.Currency.Description + ")";
                }
                deductible.Description += " - " + deductible.MinDeductValue == null ? "" : deductible.MinDeductValue.ToString() + " " + deductible.MinDeductibleUnit == null ? "" : deductible.MinDeductibleUnit.Description;
            }
            return deductibles;
        }
    }
}