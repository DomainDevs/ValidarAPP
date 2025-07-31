using Sistran.Core.Application.CommonService.Helper;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Diagnostics;
using System.Globalization;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    public class ModuleDateDAO
    {
        /// <summary>
        /// Obtener fecha de emisión
        /// </summary>
        /// <param name="moduleCode">Codigo de Modulo</param>
        /// <param name="issueDate">Fecha de Emisión</param>
        /// <returns>Fecha real de emisión</returns>
        //public DateTime GetModuleDateIssue(int moduleCode, DateTime issueDate)
        //{
        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();
        //    string myDate;
        //    DateTime closingDate, issueDateMonth, departureDate, realDate;
        //    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //    filter.Property(COMMEN.ModuleDate.Properties.ModuleCode, typeof(COMMEN.ModuleDate).Name);
        //    filter.Equal();
        //    filter.Constant(moduleCode);
        //    realDate = DateTime.Today;
        //    departureDate = DateTime.Today;

        //    PrimaryKey key = COMMEN.CoModuleDate.CreatePrimaryKey(moduleCode);
        //    COMMEN.CoModuleDate coModuleDate = (COMMEN.CoModuleDate)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);


        //    if (coModuleDate != null)
        //    {
        //        myDate = "01" + HelperDate.ShortDateSeparator + coModuleDate.CeilingMm.ToString().PadLeft(2, '0') + HelperDate.ShortDateSeparator + coModuleDate.CeilingYyyy.ToString();
        //        closingDate = DateTime.ParseExact(myDate, HelperDate.ShortDatePattern, CultureInfo.InvariantCulture);
        //        closingDate = closingDate.AddMonths(1);
        //        issueDateMonth = closingDate.AddMonths(1);
        //        closingDate = closingDate.AddDays(-1);
        //        issueDateMonth = issueDateMonth.AddDays(-1);


        //        if (issueDate <= closingDate)
        //        {
        //            issueDate = DateTime.Today;
        //        }

        //        if (issueDate > issueDateMonth)
        //        {
        //            departureDate = issueDateMonth;
        //        }

        //        if ((issueDate > closingDate) && (issueDate <= issueDateMonth))
        //        {
        //            departureDate = issueDate;
        //        }

        //        if (issueDate <= closingDate)
        //        {
        //            myDate = "01" + HelperDate.ShortDateSeparator + issueDateMonth.Month.ToString().PadLeft(2, '0') + HelperDate.ShortDateSeparator + issueDateMonth.Year;
        //            departureDate = DateTime.ParseExact(myDate, HelperDate.ShortDatePattern, CultureInfo.InvariantCulture);
        //        }

        //        realDate = departureDate;
        //    }
        //    stopWatch.Stop();
        //    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.DAOs.GetModuleDateIssue");
        //    return realDate;
        //}

        /// <summary>
        /// Obtener fecha de emisión
        /// </summary>
        /// <param name="moduleCode">Codigo de Modulo</param>
        /// <param name="issueDate">Fecha de Emisión</param>
        /// <returns>Fecha de emisión</returns>
        public DateTime GetDateIssue(int moduleCode, DateTime issueDate)
        {
            issueDate = DateTime.Now;
            COMMEN.CoModuleDate coModuleDate = null;
            PrimaryKey key = COMMEN.CoModuleDate.CreatePrimaryKey(moduleCode);
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                coModuleDate = (COMMEN.CoModuleDate)daf.GetObjectByPrimaryKey(key);
            }

            if (coModuleDate != null)
            {
                DateTime dBDate = new DateTime((int)coModuleDate.CeilingYyyy, (int)coModuleDate.CeilingMm, 01, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                if (dBDate != null)
                {
                    dBDate.ToString(KeySettings.FullDatePattern, CultureInfo.InvariantCulture);
                    dBDate = dBDate.AddMonths(2);
                    dBDate = dBDate.AddDays(-1);

                    // jhgomez: Se valida que se cerró el mes, pero el mes no ha terminado
                    if ((dBDate.Year == issueDate.Year && dBDate.Month >= issueDate.Month + 1)
                        || (dBDate.Year >= issueDate.Year + 1))
                    {
                        issueDate = new DateTime(dBDate.Year, dBDate.Month, 01, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    }

                    if (issueDate > dBDate)
                    {
                        issueDate = dBDate;
                    }
                }

            }
            return issueDate;
        }
		
		public DateTime GetModuleDateByModuleTypeMovementDate(ModuleType moduleType, DateTime movementDate)
        {
            string myDate;
            DateTime closingDate, issueDateMonth, departureDate, realDate;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.ModuleDate.Properties.ModuleCode, typeof(COMMEN.ModuleDate).Name);
            filter.Equal();
            filter.Constant(moduleType);
            realDate = DateTime.Today;
            departureDate = DateTime.Today;

            PrimaryKey key = COMMEN.ModuleDate.CreatePrimaryKey((int)moduleType);
            COMMEN.ModuleDate coModuleDate = (COMMEN.ModuleDate)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);


            if (coModuleDate != null)
            {
                myDate = "01" + HelperDate.ShortDateSeparator + coModuleDate.CeilingMm.ToString().PadLeft(2, '0') + HelperDate.ShortDateSeparator + coModuleDate.CeilingYyyy.ToString();
                closingDate = DateTime.ParseExact(myDate, HelperDate.ShortDatePattern, CultureInfo.InvariantCulture);
                closingDate = closingDate.AddMonths(1);
                issueDateMonth = closingDate.AddMonths(1);
                closingDate = closingDate.AddDays(-1);
                issueDateMonth = issueDateMonth.AddDays(-1);


                if (movementDate <= closingDate)
                {
                    movementDate = DateTime.Today;
                }

                if (movementDate > issueDateMonth)
                {
                    departureDate = issueDateMonth;
                }

                if ((movementDate > closingDate) && (movementDate <= issueDateMonth))
                {
                    departureDate = movementDate;
                }

                if (movementDate <= closingDate)
                {
                    myDate = "01" + HelperDate.ShortDateSeparator + issueDateMonth.Month.ToString().PadLeft(2, '0') + HelperDate.ShortDateSeparator + issueDateMonth.Year;
                    departureDate = DateTime.ParseExact(myDate, HelperDate.ShortDatePattern, CultureInfo.InvariantCulture);
                }

                realDate = departureDate;
            }

            return realDate;
        }
    }
}

