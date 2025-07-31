//Sistran Core
using TempModels = Sistran.Core.Application.TempCommonServices.Models;

//Sistran FWK
using Sistran.Core.Framework.DAF;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.TempCommonServices.Provider.Assemblers
{
    internal static class ModelAssembler
    {
        #region ModuleDate

        /// <summary>
        /// CreateModuleDate
        /// </summary>
        /// <param name="moduleDate"></param>
        /// <returns></returns>
        public static TempModels.ModuleDate CreateModuleDate(Entities.ModuleDate moduleDate)
        {
            return new TempModels.ModuleDate()
            {
                CeilingMm = Convert.ToInt32(moduleDate.CeilingMm),
                CeilingYyyy = Convert.ToInt32(moduleDate.CeilingYyyy),
                Description = moduleDate.Description,
                Id = Convert.ToInt32(moduleDate.ModuleCode),
                LastClosingMm = Convert.ToInt32(moduleDate.LastClosingMm),
                LastClosingYyyy = Convert.ToInt32(moduleDate.LastClosingYyyy),
                OfflineCeilingMm = Convert.ToInt32(moduleDate.OfflineCeilingMm),
                OfflineCeilingYyyy = Convert.ToInt32(moduleDate.OfflineCeilingYyyy),
                //TypifiedSeatLastNum = Convert.ToInt32(moduleDate.TypifiedSeatLastNum)
            };
        }

        /// <summary>
        /// CreateModuleDates
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<TempModels.ModuleDate> CreateModuleDates(BusinessCollection businessCollection)
        {
            List<TempModels.ModuleDate> moduleDates = new List<TempModels.ModuleDate>();
            foreach (Entities.ModuleDate moduleDateEntity in businessCollection.OfType<Entities.ModuleDate>())
            {
                moduleDates.Add(CreateModuleDate(moduleDateEntity));
            }
            return moduleDates;
        }


        #endregion

    }
}
