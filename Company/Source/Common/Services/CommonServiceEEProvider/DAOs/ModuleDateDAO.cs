
using COMENT = Sistran.Company.Application.Common.Entities;
using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;
using System;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Company.Application.UtilitiesService
{
    public class ModuleDateDAO
    {
        /// <summary>
        /// Obtener la fecha de cotización.
        /// </summary>
        /// <param name="moduleCode"></param>
        /// <param name="issueDate"></param>
        /// <returns>fecha de cotización</returns>
        public DateTime GetQuotationDate(int moduleCode, DateTime issueDate)
        {
            int month;
            int year;
            int day;
            int hour;
            int minute;
            DateTime closedDate;
            int diat;
            int mest;
            DateTime dateMonth;
            DateTime dateDeviationMinor;

            PrimaryKey key = COMENT.CoModuleDate.CreatePrimaryKey(moduleCode);
            COMENT.CoModuleDate coModuleDate = (COMENT.CoModuleDate)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            if (coModuleDate == null)
            {
                return issueDate;
            }

            month = Convert.ToInt32(coModuleDate.CeilingMm);
            year = Convert.ToInt32(coModuleDate.CeilingYyyy);
            day = DateTime.DaysInMonth(DateTime.Today.Year, Convert.ToInt32(coModuleDate.CeilingMm));
            hour = issueDate.Hour;
            minute = issueDate.Minute;

            closedDate = new DateTime(year, month, day);
            dateMonth = closedDate.AddMonths(1);
            mest = dateMonth.Month;
            diat = DateTime.DaysInMonth(DateTime.Today.Year, mest);
            dateMonth = new DateTime(dateMonth.Year, dateMonth.Month, diat, hour, minute, 0);

            dateDeviationMinor = issueDate;

            if (dateDeviationMinor.CompareTo(closedDate) < 0)
            {
                dateDeviationMinor = closedDate;
            }

            if (issueDate.CompareTo(dateDeviationMinor) < 0)
            {
                issueDate = dateDeviationMinor;
            }

            if (issueDate.CompareTo(dateMonth) > 0)
            {
                issueDate = dateMonth;
            }

            if (issueDate.CompareTo(BusinessServices.GetDate()) > 0)
            {
                issueDate = BusinessServices.GetDate();
            }

            if (issueDate.CompareTo(closedDate) <= 0)
            {
                issueDate = new DateTime(dateMonth.Year, dateMonth.Month, 1);

                if (issueDate.CompareTo(dateMonth) < 0 && dateMonth.CompareTo(BusinessServices.GetDate()) < 0)
                {
                    issueDate = dateMonth;
                }
            }

            return issueDate;
        }
    }
}