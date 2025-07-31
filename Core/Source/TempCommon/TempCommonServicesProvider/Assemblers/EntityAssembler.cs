//Sistran Core
using TempModels = Sistran.Core.Application.TempCommonServices.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.TempCommonServices.Provider.Assemblers
{
    internal static class EntityAssembler
    {
        #region ModuleDate

        /// <summary>
        /// CreateModuleDate
        /// </summary>
        /// <param name="moduleDate"></param>
        /// <returns></returns>
        public static Entities.ModuleDate CreateModuleDate(TempModels.ModuleDate moduleDate)
        {
            return new Entities.ModuleDate(moduleDate.Id)
            {
                CeilingMm = moduleDate.CeilingMm,
                CeilingYyyy = moduleDate.CeilingYyyy,
                Description = moduleDate.Description,
                LastClosingMm = moduleDate.LastClosingMm,
                LastClosingYyyy = moduleDate.LastClosingYyyy,
                ModuleCode = moduleDate.Id,
                OfflineCeilingMm = moduleDate.OfflineCeilingMm,
                OfflineCeilingYyyy = moduleDate.OfflineCeilingYyyy,
                //TypifiedSeatLastNum = moduleDate.TypifiedSeatLastNum
            };
        }

        #endregion

    }
}
