using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.CommonService.Models;
using System.Diagnostics;

namespace Sistran.Core.Application.Sureties.SuretyServices.EEProvider.DAOs
{
    public class Aggregate
    {

        /// <summary>
        ///  Obtiene el Cupo Operativo y el Cumulo
        /// </summary>
        /// <param name="individualId">>Individual Id Asegurado</param>
        /// <param name="PrefixCd">Ramo del Negocio</param>
        /// <param name="issueDate">Fecha Hasta</param>
        /// <returns>Lista de sumas</returns>
        public List<Amount> GetAvailableAmountByIndividualId(int IndividualId, int PrefixCd, DateTime issueDate)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int lineBusinessSurety = 0;
            List<LineBusiness> lineBusiness = DelegateService.commonServiceCore.GetLinesBusinessByPrefixId(PrefixCd);
            if (lineBusiness != null)
            {
                lineBusinessSurety = lineBusiness.FirstOrDefault().Id;
            }
            List<Amount> amt = DelegateService.uniquePersonServiceCore.GetAvailableAmountByIndividualId(IndividualId, lineBusinessSurety, issueDate);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Sureties.SuretyServices.EEProvider.DAOs.GetAvailableAmountByIndividualId");
            return amt;
        }
    }
}
