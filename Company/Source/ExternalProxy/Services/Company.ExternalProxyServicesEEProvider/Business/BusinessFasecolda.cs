using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.ExternalProxyServices.ExperienciaAseguradoBiztalkService;
using Sistran.Company.Application.ExternalProxyServices.Models;

namespace Sistran.Company.Application.ExternalProxyServicesEEProvider.Business
{
    public class BusinessFasecolda
    {
        /// <summary>
        /// Obtener homologación tipo documento Fasecolda por tipo de documento SISE
        /// </summary>
        /// <param name="documentTypeId"></param>
        /// <returns>documentTypeFasecoldaId</returns>
        public int GetTypeDocumentFasecolda(int documentTypeId)
        {
            switch (documentTypeId)
            {
                case 1:
                case 4:
                    return documentTypeId;
                case 2:
                    return 3;
                case 3:
                    return 2;
                default:
                    return 5;
            }
        }

        internal HistoricoPolizasResponse InvokeMethod(HistoricoPolizas historicoPolizas, Guid guid)
        {
            HistoricoPolizasResponse historicoPolizasResponse = null;
            Stopwatch stopWatch = new Stopwatch();
            try
            {
                using (ExperienciaAseguradoClient client = new ExperienciaAseguradoClient())
                {
                    stopWatch.Start();
                    historicoPolizasResponse = client.ConsultarHistorialPolizas(historicoPolizas);
                    stopWatch.Stop();
                }

                BusinessLog.RegisterSuccessfulRequest(stopWatch, "ConsultarHistorialPolizas", guid.ToString(), historicoPolizas);
                return historicoPolizasResponse;
            }
            catch (Exception ex)
            {
                stopWatch.Stop();
                BusinessLog.RegisterFailRequest(stopWatch, "ConsultarHistorialPolizas", guid.ToString(), ex.Message, historicoPolizas);
                throw;
            }
        }

        internal HistoricoSiniestrosResponse InvokeMethod(HistoricoSiniestros historicoSiniestros, Guid guid)
        {
            HistoricoSiniestrosResponse historicoSiniestrosResponse = null;
            Stopwatch stopWatch = new Stopwatch();
            try
            {
                using (ExperienciaAseguradoClient client = new ExperienciaAseguradoClient())
                {
                    stopWatch.Start();
                    historicoSiniestrosResponse = client.ConsultarHistorialSiniestros(historicoSiniestros);
                    stopWatch.Stop();
                }

                BusinessLog.RegisterSuccessfulRequest(stopWatch, "ConsultarHistorialSiniestros", guid.ToString(), historicoSiniestros);
                return historicoSiniestrosResponse;
            }
            catch (Exception ex)
            {
                stopWatch.Stop();
                BusinessLog.RegisterFailRequest(stopWatch, "ConsultarHistorialSiniestros", guid.ToString(), ex.Message, historicoSiniestros);
                throw;
            }
        }

        #region GoodExperienceYears
        internal static bool ResetGoodExperienceYearWhenIsNotInForce(int resetYears, ResponseGoodExperienceYear responseGoodExperienceYear, DateTime maxDate)
        {
            bool response = false;
            if (maxDate < DateTime.Now)
            {
                int diffYears = DateDifference(DateTime.Now, maxDate);
                if (diffYears >= resetYears)
                {
                    responseGoodExperienceYear.GoodExperienceNum = 0;
                    responseGoodExperienceYear.GoodExpNumRate = "0P";
                    responseGoodExperienceYear.GoodExpNumPrinter = 0;
                    response = true;
                }
            }
            return response;
        }

        private static int DateDifference(DateTime dateIni, DateTime dateEnd)
        {
            try
            {
                int difDays = Math.Abs((dateIni - dateEnd).Days);
                return difDays / 365;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        internal static ResponseGoodExperienceYear BuildGoodExperienceYears(HistoricoSiniestroCexper[] historicoSiniestrosResult, int resetYears, IList<HistoricoPolizaCexper> tempHistoricoPolicy)
        {
            int numDays = 180;

            IList<HistoricoSiniestroCexper> validSiniesterList = ValidateSiniesterStatus(historicoSiniestrosResult, null);
            IList<CalcGoodExpYearsDTO> tempCalcGoodExpYearsDtoObj = BuildCollectionCalcGoodExpYearsDTO(numDays, validSiniesterList, tempHistoricoPolicy);
            IList<CalcGoodExpYearsDTO> validPolicyList = BuildCollectionCalcGoodExpYearsDto(numDays, tempCalcGoodExpYearsDtoObj);

            ResponseGoodExperienceYear responseGoodExperienceYear = new ResponseGoodExperienceYear();
            if (validPolicyList != null && validPolicyList.Count > 0)
            {
                DateTime maxDateValid = validPolicyList.AsEnumerable()
                                                       .Max(x => x.HistoryPolicy.FechaFinVigencia);

                bool shouldReturnResetExperienceYears = ResetGoodExperienceYearWhenIsNotInForce(resetYears, responseGoodExperienceYear, maxDateValid);
                if (shouldReturnResetExperienceYears)
                {
                    return responseGoodExperienceYear;
                }

                ComparePolicySiniester(validPolicyList, resetYears, responseGoodExperienceYear);
            }

            return responseGoodExperienceYear;
        }

        private static IList<HistoricoSiniestroCexper> ValidateSiniesterStatus(HistoricoSiniestroCexper[] sinisterHistoricalSet, IList<HistoricoPolizaCexper> policyList)
        {
            IList<HistoricoSiniestroCexper> validSiniesterList = new List<HistoricoSiniestroCexper>();

            if (sinisterHistoricalSet.Length > 0)
            {
                List<HistoricoSiniestroCexper> tempList = sinisterHistoricalSet.Where(siniestro => siniestro.Amparos != null).ToList<HistoricoSiniestroCexper>();

                foreach (HistoricoSiniestroCexper rowSiniester in tempList)
                {
                    if (rowSiniester.Amparos != null)
                    {
                        foreach (AmparoCexper rowCoverage in rowSiniester.Amparos)
                        {
                            if ((!rowCoverage.Estado.Equals("Desistido")) && (!rowCoverage.Estado.Equals("Anulado")))
                            {
                                validSiniesterList.Add(rowSiniester);
                                break;
                            }
                        }
                    }
                }
            }

            return validSiniesterList;
        }

        private static IList<CalcGoodExpYearsDTO> BuildCollectionCalcGoodExpYearsDTO(int numDays, IList<HistoricoSiniestroCexper> validSiniesterList, IList<HistoricoPolizaCexper> tempHistoricoPolicy)
        {
            IList<CalcGoodExpYearsDTO> tempCalcGoodExpYearsDtoObj = new List<CalcGoodExpYearsDTO>();

            IEnumerable<HistoricoPolizaCexper> temp = (from hp in tempHistoricoPolicy.AsEnumerable()
                                                       orderby hp.FechaVigencia
                                                       select hp);

            foreach (HistoricoPolizaCexper tempPolicy in temp)
            {
                CalcGoodExpYearsDTO tmp;

                int newNumDays = numDays;
                bool validarCiclos = false;
                string[] vigente = tempPolicy.Vigente.Split('/');
                DateTime iniVig = tempPolicy.FechaVigencia;
                DateTime finVig = GetFinVig(tempPolicy, vigente);
                DateTime newIniVig = iniVig;
                DateTime newFinVig = finVig;
                int totalVigDays = (finVig - iniVig).Days;
                validarCiclos = ((newFinVig - newIniVig).Days) > ((newIniVig.AddYears(1) - newIniVig).Days);
                if (validarCiclos)
                {
                    newIniVig = iniVig;
                    newFinVig = iniVig.AddYears(1);
                    while (validarCiclos)
                    {
                        tmp = new CalcGoodExpYearsDTO();
                        tmp.HistoryPolicy = new HistoricoPolizaCexper();
                        tmp.HistoryPolicy.Placa = tempPolicy.Placa;
                        tmp.HistoryPolicy.NumeroPoliza = tempPolicy.NumeroPoliza;
                        tmp.HistoryPolicy.TipoDocumentoAsegurado = tempPolicy.TipoDocumentoAsegurado;
                        tmp.HistoryPolicy.NumeroDocumento = tempPolicy.NumeroDocumento;
                        if (newFinVig.CompareTo(finVig) < 0)
                        {
                            tmp.HistoryPolicy.FechaVigencia = newIniVig;
                            tmp.HistoryPolicy.FechaFinVigencia = newFinVig;
                            tmp.YearsWithPolicy = 1;

                            int countSiniester = 0;
                            foreach (HistoricoSiniestroCexper rowSiniester in validSiniesterList)
                            {
                                if (ValidateSiniesterPolicy(rowSiniester, tmp.HistoryPolicy))
                                {
                                    countSiniester++;
                                }
                            }
                            tmp.PolicyWithSiniester = countSiniester;
                            tempCalcGoodExpYearsDtoObj.Add(tmp);

                            newIniVig = newFinVig;
                            newFinVig = newFinVig.AddYears(1);
                        }
                        else
                        {
                            tmp.HistoryPolicy.FechaVigencia = newIniVig;
                            tmp.HistoryPolicy.FechaFinVigencia = finVig;
                            if (ValidateDatesPolicy(tmp.HistoryPolicy.FechaVigencia, tmp.HistoryPolicy.FechaFinVigencia, numDays))
                            {
                                tmp.YearsWithPolicy = 1;
                            }
                            else
                            {
                                tmp.YearsWithPolicy = 0;
                            }
                            int countSiniester = 0;
                            foreach (HistoricoSiniestroCexper rowSiniester in validSiniesterList)
                            {
                                if (ValidateSiniesterPolicy(rowSiniester, tmp.HistoryPolicy))
                                {
                                    countSiniester++;
                                }
                            }
                            tmp.PolicyWithSiniester = countSiniester;
                            tempCalcGoodExpYearsDtoObj.Add(tmp);

                            validarCiclos = false;
                        }
                    }
                }
                else
                {
                    tmp = new CalcGoodExpYearsDTO();
                    tmp.HistoryPolicy = tempPolicy;
                    tmp.HistoryPolicy.FechaVigencia = iniVig;
                    tmp.HistoryPolicy.FechaFinVigencia = finVig;
                    if (ValidateDatesPolicy(newIniVig, newFinVig, numDays))
                    {
                        tmp.YearsWithPolicy = 1;
                    }
                    else
                    {
                        tmp.YearsWithPolicy = 0;
                    }
                    int countSiniester = 0;
                    foreach (HistoricoSiniestroCexper rowSiniester in validSiniesterList)
                    {
                        if (ValidateSiniesterPolicy(rowSiniester, tmp.HistoryPolicy))
                        {
                            countSiniester++;
                        }
                    }
                    tmp.PolicyWithSiniester = countSiniester;
                    tempCalcGoodExpYearsDtoObj.Add(tmp);
                }
            }
            return tempCalcGoodExpYearsDtoObj;
        }

        private static IList<CalcGoodExpYearsDTO> BuildCollectionCalcGoodExpYearsDto(int numDays, IList<CalcGoodExpYearsDTO> tempCalcGoodExpYearsDtoObj)
        {
            IList<CalcGoodExpYearsDTO> validPolicyList = new List<CalcGoodExpYearsDTO>();

            foreach (CalcGoodExpYearsDTO tmpCalcGood in tempCalcGoodExpYearsDtoObj)
            {
                if (!(tmpCalcGood.HistoryPolicy.FechaVigencia.ToString().Equals(string.Empty))
                        && !(tmpCalcGood.HistoryPolicy.FechaFinVigencia.ToString().Equals(string.Empty)))
                {
                    var tmpValid = validPolicyList.Where(tcg => ((tcg.HistoryPolicy.FechaVigencia <= tmpCalcGood.HistoryPolicy.FechaVigencia
                                                                    && tcg.HistoryPolicy.FechaFinVigencia > tmpCalcGood.HistoryPolicy.FechaVigencia)
                                                                    || (tcg.HistoryPolicy.FechaVigencia < tmpCalcGood.HistoryPolicy.FechaFinVigencia
                                                                    && tcg.HistoryPolicy.FechaFinVigencia >= tmpCalcGood.HistoryPolicy.FechaFinVigencia)))
                                                                    .ToList();
                    if (tmpValid.Count == 0 && tmpCalcGood.YearsWithPolicy > 0)
                    {
                        validPolicyList.Add(tmpCalcGood);
                    }
                    else
                    {
                        bool exists = false;
                        DateTime DateMin;
                        DateTime DateMax;
                        if (tmpValid.Count > 0)
                        {
                            DateMin = tmpValid.Select(tcg => tcg.HistoryPolicy.FechaVigencia).Min();
                            DateMax = tmpValid.Select(tcg => tcg.HistoryPolicy.FechaFinVigencia).Max();
                            exists = true;
                        }
                        else
                        {
                            DateMin = DateTime.Now;
                            DateMax = DateTime.Now;
                        }
                        if (tmpCalcGood.PolicyWithSiniester > 0)
                        {
                            DateTime date = DateMax.AddDays(numDays);
                            tmpCalcGood.YearsWithPolicy = 0;
                            bool validar = true;
                            while (validar)
                            {
                                if (tmpCalcGood.HistoryPolicy.FechaFinVigencia > date)
                                {
                                    tmpCalcGood.YearsWithPolicy++;
                                    date = date.AddYears(1);
                                }
                                else
                                {
                                    validar = false;
                                }
                            }
                            validPolicyList.Add(tmpCalcGood);
                        }
                        else
                        {
                            if (exists)
                            {
                                if (DateMin > tmpCalcGood.HistoryPolicy.FechaVigencia)
                                {
                                    if ((DateMin - tmpCalcGood.HistoryPolicy.FechaVigencia).Days > numDays)
                                    {
                                        tmpCalcGood.YearsWithPolicy = 0;
                                        bool validar = true;
                                        DateTime date = tmpCalcGood.HistoryPolicy.FechaVigencia.AddDays(numDays);
                                        while (validar)
                                        {
                                            if (DateMin > date)
                                            {
                                                tmpCalcGood.YearsWithPolicy++;
                                                date = date.AddYears(1);
                                            }
                                            else
                                            {
                                                validar = false;
                                            }
                                        }
                                        if (tmpCalcGood.YearsWithPolicy > 0)
                                        {
                                            validPolicyList.Add(tmpCalcGood);
                                        }
                                    }
                                }
                                else if (DateMax < tmpCalcGood.HistoryPolicy.FechaFinVigencia)
                                {
                                    if ((tmpCalcGood.HistoryPolicy.FechaFinVigencia - DateMax).Days > numDays)
                                    {
                                        tmpCalcGood.YearsWithPolicy = 0;
                                        bool validar = true;
                                        DateTime date = DateMax.AddDays(numDays);
                                        DateTime dateComp = (tmpCalcGood.HistoryPolicy.FechaFinVigencia > DateTime.Now ? DateTime.Now
                                                                                                                       : tmpCalcGood.HistoryPolicy.FechaFinVigencia);
                                        while (validar)
                                        {
                                            if (dateComp > date)
                                            {
                                                tmpCalcGood.YearsWithPolicy++;
                                                date = date.AddYears(1);
                                            }
                                            else
                                            {
                                                validar = false;
                                            }
                                        }
                                        if (tmpCalcGood.YearsWithPolicy > 0)
                                        {
                                            validPolicyList.Add(tmpCalcGood);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return validPolicyList;
        }

        private static void ComparePolicySiniester(IList<CalcGoodExpYearsDTO> validPolicyList, int resetYears, ResponseGoodExperienceYear responseGoodExperienceYear)
        {
            int contYears = 0;
            string contYearsRate = "0";
            int contYearsPrint = 0;
            DateTime tmpEnd = new DateTime();

            if (validPolicyList != null && validPolicyList.Count > 0)
            {
                foreach (CalcGoodExpYearsDTO rowPolicy in validPolicyList)
                {
                    int yearsBetween = DateDifference(tmpEnd, (DateTime)rowPolicy.HistoryPolicy.FechaVigencia);

                    if (yearsBetween >= resetYears)
                    {
                        contYears = 0;
                        contYearsRate = "0";
                        contYearsPrint = 0;
                    }
                    //>> Autor: Juan Manuel Gómez Duarte Fecha: 21/06/2016; Asunto: CVS34161 Años de Buena Experiencia
                    if (rowPolicy.PolicyWithSiniester > 0)
                    {
                        int contYearsAnt = contYears;
                        contYears = contYears - rowPolicy.PolicyWithSiniester;
                        contYearsRate = contYears.ToString() + "N";
                        contYearsPrint = contYearsPrint - 1;
                        if (contYearsPrint < 0)
                        {
                            contYearsPrint = 0;
                        }
                    }
                    else
                    {
                        contYears = contYears + rowPolicy.YearsWithPolicy;
                        contYearsRate = contYears.ToString() + "P";
                        contYearsPrint = contYearsPrint + rowPolicy.YearsWithPolicy;
                    }
                    //<< Autor: Juan Manuel Gómez Duarte Fecha: 21/06/2016;
                    tmpEnd = (DateTime)rowPolicy.HistoryPolicy.FechaFinVigencia;
                }
            }

            responseGoodExperienceYear.GoodExperienceNum = contYears;
            responseGoodExperienceYear.GoodExpNumRate = contYearsRate;
            responseGoodExperienceYear.GoodExpNumPrinter = contYearsPrint;
        }

        private static DateTime GetFinVig(HistoricoPolizaCexper tempPolicy, string[] vigente)
        {
            DateTime finVig = tempPolicy.FechaFinVigencia;

            if (vigente.Count() > 3)
            {
                finVig = Convert.ToDateTime(vigente[1].Trim() + "/" + vigente[2].Trim() + "/" + vigente[3].Trim());
            }
            else if (tempPolicy.FechaFinVigencia > DateTime.Now)
            {
                finVig = DateTime.Now;
            }

            return finVig;
        }

        protected static bool ValidateSiniesterPolicy(HistoricoSiniestroCexper siniester, HistoricoPolizaCexper historyPolicy)
        {
            bool response = false;
            if (ValidateDateSiniester((DateTime)siniester.FechaSiniestro, historyPolicy.FechaVigencia, historyPolicy.FechaFinVigencia))
            {
                response = siniester.NumeroPoliza.Equals(historyPolicy.NumeroPoliza) &&
                           siniester.Placa.Equals(historyPolicy.Placa);
            }
            return response;
        }

        private static bool ValidateDateSiniester(DateTime dateSiniester, DateTime currentFrom, DateTime currentTo)
        {
            return dateSiniester >= currentFrom &&
                   dateSiniester <= currentTo;
        }

        private static bool ValidateDatesPolicy(DateTime currentFrom, DateTime currentTo, int numDays)
        {
            //Variable donde almaceno el rango de fecha para verificar si es valida o no la vigencia
            TimeSpan endDate = new TimeSpan();
            DateTime axiliarCurrentEnd = currentTo;

            if (axiliarCurrentEnd > DateTime.Now)
            {
                axiliarCurrentEnd = DateTime.Now;
            }

            endDate = axiliarCurrentEnd.Subtract(currentFrom);

            return endDate.Days >= numDays;
        }

        #endregion GoodExperienceYears
    }
}
