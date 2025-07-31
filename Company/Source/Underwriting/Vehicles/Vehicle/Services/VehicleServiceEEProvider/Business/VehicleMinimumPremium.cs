using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Business
{
    /// <summary>
    /// Calculo de Prima Minima Autos Previsora
    /// </summary>
    public class VehicleMinimumPremium
    {

        private Parameter coverageHeritage;

        private int daysDifferenceCurrencyFromTo = 0;
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleMinimumPremium"/> class.
        /// </summary>
        public VehicleMinimumPremium()
        {
           
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleMinimumPremium"/> class.
        /// </summary>
        /// <param name="vehicleCoverageHeritage">The vehicle coverage heritage.</param>
        public VehicleMinimumPremium(Parameter vehicleCoverageHeritage)
        {
            coverageHeritage = vehicleCoverageHeritage;
         }

        /// <summary>
        /// Calculates the vehicle minimum premium.
        /// </summary>
        /// <param name="companyVehicle">The company vehicle.</param>
        /// <returns></returns>
        public CompanyVehicle CalculateVehicleMinimumPremium(CompanyVehicle companyVehicle)
        {

            if (companyVehicle?.Risk?.Coverages.Sum(x => x.PremiumAmount) < 1)
            {
                return companyVehicle;
            }
            DAOs.VehicleDAO companyVehicleDAO = new DAOs.VehicleDAO();
            List<CompanyCoverage> quotateCoverages = new List<CompanyCoverage>();
            EndorsementType tempEndorsement = new EndorsementType();

            //Se valida el endoso que se esta ejecutando, si el endoso es de modificacion o prorroga se usa la prima minima de emision
            if (companyVehicle.Risk.Policy.Endorsement.EndorsementType == EndorsementType.Modification || companyVehicle.Risk.Policy.Endorsement.EndorsementType == EndorsementType.EffectiveExtension)
            {
                tempEndorsement = EndorsementType.Emission;
            }
            else
            {
                tempEndorsement = companyVehicle.Risk.Policy.Endorsement.EndorsementType.Value;
            }

            CompanyMinPremium companyMinPremium = companyVehicleDAO.GetMinimumPremium(companyVehicle.Risk.Policy.Prefix.Id, tempEndorsement, companyVehicle.Risk.Policy.Product.Id, companyVehicle.Risk.GroupCoverage.Id);
            List<CompanyCiaGroupCoverage> companyCiaGroupCoverage = companyVehicleDAO.GetGroupCoverage(companyVehicle.Risk.Policy.Product.Id, companyVehicle.Risk.GroupCoverage.Id);
            var companyCoveragePremium = companyCiaGroupCoverage.Where(x => x.IsPremiumMin != 0 && x.NoCalculate == 0).ToList();
            var companyCoveragePremiumExclude = companyCiaGroupCoverage.Where(x => x.IsPremiumMin == 0 && x.NoCalculate == 1).ToList();
            //Coberturas que no estan en grupo de coberturas, Vehiculo reemplazo y RC en exceso
            var companyCoverageNotInclude = companyVehicle.Risk.Coverages.Where(x => !companyCiaGroupCoverage.Select(a => a.CoverageId).Contains(x.Id)).ToList();
            var companyCoverageInclude = companyVehicle.Risk.Coverages.Where(x => !companyCoveragePremium.Select(a => a.CoverageId).Contains(x.Id)).ToList();
            if (companyMinPremium != null && companyMinPremium.RiskMinPremium.GetValueOrDefault() > companyCoverageInclude.Sum(x => x.PremiumAmount) && companyVehicle.Risk.Coverages.Where(x => x.Rate.HasValue && x.Rate.Value > 0 && x.PremiumAmount > 0).Any())
            {
                if (companyCiaGroupCoverage?.Count < 1)
                {
                    return companyVehicle;
                }
                if (companyMinPremium != null && companyVehicle.Risk.Policy.Endorsement.EndorsementType == EndorsementType.Modification)
                {
                    decimal newPercentage = 0;

                    //Validamos primero que oldRiskValue no sea 0 ya que si es asi quiere decir que lo que se modifico fue la tasa unica solamente y se debe evaluar todo el valor del riesgo
                    if (companyVehicle.OriginalPrice != 0)
                    {
                        decimal priceDiff = decimal.Round(companyVehicle.Price - companyVehicle.OriginalPrice, 2);

                        if (companyVehicle.OriginalPrice == companyVehicle.Price || companyVehicle.Price == 0)
                        {
                            newPercentage = decimal.Round((priceDiff * 100) / companyVehicle.OriginalPrice, 2);
                        }
                        else
                        {
                            newPercentage = decimal.Round((priceDiff * 100) / companyVehicle.Price, 2);
                        }
                    }
                    else
                    {
                        newPercentage = 100;
                    }
                    if (newPercentage != 0)
                    {
                        companyMinPremium.RiskMinPremium = decimal.Round((companyMinPremium.RiskMinPremium.Value / 100) * newPercentage, 2);
                    }
                }
              
      
                foreach (var item in companyCoverageNotInclude)
                {
                    companyCoveragePremiumExclude.Add(new CompanyCiaGroupCoverage() { CoverageId = item.Id });
                }

                List<CompanyCoverage> coveragesApply = companyVehicle.Risk.Coverages.Where(x => x.Rate.HasValue && x.Rate.Value > 0 && x.PremiumAmount > 0 && companyCoveragePremium.Select(a => a.CoverageId).Contains(x.Id)).ToList();
                companyVehicle.CalculateMinPremium = true;

                decimal CoveragePatProtecAmount = 0;
                //Suma de la prima base de las coberturas que aplican prima minima
                decimal premiumBase = coveragesApply.Sum(z => z.PremiumAmount);
                //Suma de la prima base de las coberturas que NO aplican prima minima
                decimal miniumPremiumNot = companyVehicle.Risk.Coverages.Where(x => x.PremiumAmount > 0 && !companyCoveragePremium.Select(a => a.CoverageId).Contains(x.Id) && !companyCoveragePremiumExclude.Select(a => a.CoverageId).Contains(x.Id)).Sum(z => z.PremiumAmount);
                //Diferencia entre la prima minima y la suma de la prima minima de todas las coberturas -> se obtiene para saber el valor a redistribuir en las coberturas que aplica prima minima
                decimal difPremium = companyMinPremium.RiskMinPremium.Value - miniumPremiumNot - premiumBase;
                decimal totalAmountOldMinPremium = 0;
                decimal totalAmountFinalPartial = 0;
                bool roundPremiumFinal = false;
                decimal minPremiumMax = 0;
                decimal totalPremiumAmount = premiumBase + miniumPremiumNot;
                bool appliedMinimumPremiumPreviusly = false;

                bool isYearComplete = true;
                isYearComplete = IsYearComplete(companyVehicle.Risk.Policy.CurrentFrom, companyVehicle.Risk.Policy.CurrentTo);

                if (!isYearComplete)
                {
                    if (Math.Abs(totalPremiumAmount) >= decimal.Round((companyMinPremium.SubsMinPremium.Value / 365) * daysDifferenceCurrencyFromTo, 2)
                               && Math.Abs(totalPremiumAmount) < decimal.Round((companyMinPremium.RiskMinPremium.Value / 365) * daysDifferenceCurrencyFromTo, 2))
                    {
                        minPremiumMax = decimal.Round((Convert.ToDecimal(companyMinPremium.RiskMinPremium.Value) / 365) * daysDifferenceCurrencyFromTo, 2);
                    }
                    else if (Math.Abs(totalPremiumAmount) >= decimal.Round((companyMinPremium.SubsMinPremium.Value / 365) * daysDifferenceCurrencyFromTo, 2)
                            && Math.Abs(totalPremiumAmount) <= decimal.Round((companyMinPremium.RiskMinPremium.Value / 365) * daysDifferenceCurrencyFromTo, 2))
                    {
                        appliedMinimumPremiumPreviusly = true;
                    }
                }
                else
                {
                    if (Math.Abs(totalPremiumAmount) >= companyMinPremium.SubsMinPremium.Value
                                   && Math.Abs(totalPremiumAmount) < companyMinPremium.RiskMinPremium.Value)
                    {
                        minPremiumMax = companyMinPremium.RiskMinPremium.Value;
                    }
                    else if (Math.Abs(totalPremiumAmount) >= companyMinPremium.SubsMinPremium.Value
                        && Math.Abs(totalPremiumAmount) <= companyMinPremium.RiskMinPremium.Value)
                    {
                        appliedMinimumPremiumPreviusly = true;
                    }
                }

                if (minPremiumMax > 0)
                {
                    minPremiumMax -= miniumPremiumNot;
                    //Se distribuye prima minima entre las coberturas que aplican prima minima
                    foreach (CompanyCoverage coverage in coveragesApply)
                    {

                        decimal rate = decimal.Round((coverage.PremiumAmount == 0) ? 0 : coverage.PremiumAmount / premiumBase, QuoteManager.DecimalRound); //Se obtiene el porcentaje
                        decimal premiumAmt = decimal.Round((rate * difPremium) + coverage.PremiumAmount, QuoteManager.DecimalRound);

                        if (coverage.RateType == RateType.FixedValue)
                        {
                            coverage.Rate = premiumAmt;
                        }
                        else if (premiumAmt != 0 && coverage.DeclaredAmount != 0)
                        {
                            coverage.Rate = decimal.Round(premiumAmt * 100 / coverage.DeclaredAmount, QuoteManager.PremiumRoundValue);
                            ///////////////////////////
                            decimal premiumAmtFinal = decimal.Round((decimal)coverage.Rate * coverage.DeclaredAmount / 100, QuoteManager.DecimalRound);
                            premiumAmt = premiumAmtFinal;
                            ///////////////////////////
                        }
                        else
                        {
                            coverage.Rate = 0;
                        }
                        totalAmountOldMinPremium += coverage.PremiumAmount; //Suma de las primas anteriores para validar cuando llegue a la ultima cobertura y aproximar
                        totalAmountFinalPartial += decimal.Round(premiumAmt, QuoteManager.DecimalRound); //Sumamos las primas resultantes para validar al final si sobra o falta y agregarlo o quitarlo
                        if (totalAmountOldMinPremium == premiumBase && !roundPremiumFinal)
                        {
                            //El total se paso o le falta
                            if ((totalAmountFinalPartial - minPremiumMax) != 0)
                            {
                                premiumAmt -= (totalAmountFinalPartial - minPremiumMax);
                            }
                            //Ya se aproximo la ultima covertura que puede hacer eso
                            roundPremiumFinal = true;
                        }
                        coverage.PremiumAmount = premiumAmt;

                        if (coverage.Id == coverageHeritage.NumberParameter && coverage.PremiumAmount != 0)
                        {
                            CoveragePatProtecAmount = premiumAmt;
                        }
                    }

                    //Se calcula la diferencia y se aplica a la ultima cobertura
                    CompanyCoverage lastCoverage = coveragesApply.Last();
                    if (CoveragePatProtecAmount > 0)
                    {
                        CalculatePremiumAmount(companyVehicle.Risk.Coverages);
                        CalculateCoveragePatrimonial(coveragesApply, lastCoverage, CoveragePatProtecAmount);
                    }
                    decimal premiumDiff = decimal.Round(companyMinPremium.RiskMinPremium.Value - companyVehicle.Risk.Coverages.Where(x => !companyCoveragePremiumExclude.Select(a => a.CoverageId).Contains(x.Id)).Sum(x => x.PremiumAmount), 2);
                    lastCoverage.PremiumAmount += premiumDiff;
                    if (lastCoverage.PremiumAmount != 0 && lastCoverage.Rate != null)
                    {
                        lastCoverage.Rate = decimal.Round(((decimal)lastCoverage.Rate * lastCoverage.PremiumAmount) / lastCoverage.PremiumAmount, QuoteManager.PremiumRoundValue);
                    }

                }
                else
                {
                    companyVehicle.CalculateMinPremium = false;
                }
            }
            return companyVehicle;
        }

        /// <summary>
        /// Calculates the vehicle minimum premium modification.
        /// </summary>
        /// <param name="companyVehicle">The company vehicle.</param>
        /// <returns></returns>
        public CompanyVehicle CalculateVehicleMinimumPremiumModification(CompanyVehicle companyVehicle)
        {
            DAOs.VehicleDAO companyVehicleDAO = new DAOs.VehicleDAO();
            //Se valida si existe en la tabla de primas minimas por producto y grupo de coberturas
            CompanyMinPremium companyMinPremium = companyVehicleDAO.GetMinimumPremium(companyVehicle.Risk.Policy.Prefix.Id, companyVehicle.Risk.Policy.Endorsement.EndorsementType, companyVehicle.Risk.Policy.Product.Id, companyVehicle.Risk.GroupCoverage.Id);

            if (companyMinPremium == null)
            {
                companyMinPremium = companyVehicleDAO.GetMinimumPremium(companyVehicle.Risk.Policy.Prefix.Id, EndorsementType.Emission, companyVehicle.Risk.Policy.Product.Id, companyVehicle.Risk.GroupCoverage.Id);

                if (companyMinPremium == null)
                {
                    companyMinPremium = companyVehicleDAO.GetMinimumPremium(companyVehicle.Risk.Policy.Prefix.Id, companyVehicle.Risk.Policy.Endorsement.EndorsementType, companyVehicle.Risk.Policy.Product.Id, companyVehicle.Risk.GroupCoverage.Id, false);
                }
            }

            if (companyMinPremium != null && companyMinPremium.RiskMinPremium.GetValueOrDefault() > companyVehicle.Risk.Coverages.Sum(x => x.PremiumAmount) && companyVehicle.Risk.Coverages.Where(x => x.Rate.HasValue && x.Rate.Value > 0 && x.PremiumAmount > 0).Any())
            {
                #region variables
                decimal minPremiumMax = 0;
                decimal totalPremiumAmount = 0;
                decimal oldRiskValue = 0;
                decimal newRiskValue = 0;
                decimal differenceRiskValue = 0;
                decimal percentageNewRiskValue = 0;
                bool appliedMinimumPremiumPreviusly = false;
                bool isYearComplete = true;
                #endregion

                //traer todas las coberturas por producto y grupo de coberturas
                List<CompanyCiaGroupCoverage> companyCiaGroupCoverage = companyVehicleDAO.GetGroupCoverage(companyVehicle.Risk.Policy.Product.Id, companyVehicle.Risk.GroupCoverage.Id);

                bool applied = false;
                //coberturas que aplican para prima minima
                var companyCoveragePremium = companyCiaGroupCoverage.Where(x => x.IsPremiumMin != 0 && x.NoCalculate == 0).ToList();
                //coberturas que no se calculan en prima minima
                var companyCoveragePremiumExclude = companyCiaGroupCoverage.Where(x => x.IsPremiumMin == 0 && x.NoCalculate == 1).ToList();
                //coberturas que aplican, tienen tasa y el valor asegurado es mayor a 0
                List<CompanyCoverage> coveragesApply = companyVehicle.Risk.Coverages.Where(x => x.PremiumAmount != 0 && companyCoveragePremium.Select(a => a.CoverageId).Contains(x.Id)).ToList();
                List<CompanyCoverage> coveragesNotApply = companyVehicle.Risk.Coverages.Where(x => x.PremiumAmount > 0 && !companyCoveragePremium.Select(a => a.CoverageId).Contains(x.Id) && !companyCoveragePremiumExclude.Select(a => a.CoverageId).Contains(x.Id)).ToList();

                //Suma de la prima base de las coberturas que aplican prima minima
                decimal premiumBase = coveragesApply.Sum(z => z.PremiumAmount);
                //Total de prima de coverturas que no inclyen prima minima y no estan marcadas como no calcular
                decimal totalPremiumAmountWithoutMinPremium = coveragesNotApply.Sum(z => z.PremiumAmount);
                //Diferencia entre la prima minima y la suma de la prima minima de todas las coberturas -> se obtiene para saber el valor a redistribuir en las coberturas que aplica prima minima
                decimal difPremium = companyMinPremium.RiskMinPremium.Value - totalPremiumAmountWithoutMinPremium - premiumBase;

                totalPremiumAmount = premiumBase + totalPremiumAmountWithoutMinPremium;

                isYearComplete = IsYearComplete(companyVehicle.Risk.Policy.CurrentFrom, companyVehicle.Risk.Policy.CurrentTo);

                //Validamos que el total de primas sea mayor a 0 haciendo entender que hay un valor de prima que recalcular si es que aplica
                if (totalPremiumAmount != 0)
                {
                    //Validamos que no sea año completo, en este caso se calcula el nuevo rango de prima minima dentro del cual el total debe encontrarse para ser recalculado
                    if (!isYearComplete)
                    {
                        if (Math.Abs(totalPremiumAmount) >= decimal.Round((companyMinPremium.SubsMinPremium.Value / 365) * (decimal)this.daysDifferenceCurrencyFromTo, 2)
                            && Math.Abs(totalPremiumAmount) < decimal.Round((companyMinPremium.RiskMinPremium.Value / 365) * (decimal)this.daysDifferenceCurrencyFromTo, 2))
                        {
                            minPremiumMax = decimal.Round((Convert.ToDecimal(companyMinPremium.RiskMinPremium.Value) / 365) * (decimal)this.daysDifferenceCurrencyFromTo, 2);
                        }
                        else if (Math.Abs(totalPremiumAmount) >= decimal.Round((companyMinPremium.SubsMinPremium.Value / 365) * (decimal)this.daysDifferenceCurrencyFromTo, 2)
                            && Math.Abs(totalPremiumAmount) <= decimal.Round((companyMinPremium.RiskMinPremium.Value / 365) * (decimal)this.daysDifferenceCurrencyFromTo, 2))
                        {
                            appliedMinimumPremiumPreviusly = true;
                        }
                    }
                    else
                    {
                        if (Math.Abs(totalPremiumAmount) >= companyMinPremium.SubsMinPremium.Value
                            && Math.Abs(totalPremiumAmount) < companyMinPremium.RiskMinPremium.Value)
                        {
                            minPremiumMax = companyMinPremium.RiskMinPremium.Value;
                        }
                        else if (Math.Abs(totalPremiumAmount) >= companyMinPremium.SubsMinPremium.Value
                            && Math.Abs(totalPremiumAmount) <= companyMinPremium.RiskMinPremium.Value)
                        {
                            appliedMinimumPremiumPreviusly = true;
                        }
                    }

                    //Vamos a obtener el valor de antes y despues y asi determinar si hay variaciones para el calculo
                    oldRiskValue = companyVehicle.OriginalPrice;
                    newRiskValue = companyVehicle.Price;

                    differenceRiskValue = decimal.Round(newRiskValue - oldRiskValue, 2);

                    //Se obtiene el porcentaje de prima minima que se debe evaluar para posteriormente recalcular
                    //Validamos primero que oldRiskValue no sea 0 ya que si es asi quiere decir que lo que se modifico fue la tasa unica solamente y se debe evaluar todo el valor del riesgo
                    if (oldRiskValue != 0)
                    {
                        if (oldRiskValue == newRiskValue || newRiskValue == 0)
                            percentageNewRiskValue = decimal.Round((differenceRiskValue * 100) / oldRiskValue, 2);
                        else
                            percentageNewRiskValue = decimal.Round((differenceRiskValue * 100) / newRiskValue, 2);
                    }
                    else
                    {
                        percentageNewRiskValue = 100;
                    }

                    //Se establece entonces el nuevo valor maximo de prima minima con respecto al porcentaje obtenido
                    if (percentageNewRiskValue != 0)
                        minPremiumMax = decimal.Round((minPremiumMax / 100) * percentageNewRiskValue, 2);

                    //Solamente se hace el recalculo de prima minima si el valor obtenido de prima minima en el que se encuentra es mayor a 0 significando que aplica
                    if (minPremiumMax > 0)
                    {
                        //Hacemos que la prima minima a alcanzar sea el total obtenido pero descontando las coberturas que no incluyen prima minima y no estan marcadas como no calcular
                        minPremiumMax -= totalPremiumAmountWithoutMinPremium;
                        totalPremiumAmount -= totalPremiumAmountWithoutMinPremium;

                        //Variables de redondeo final
                        decimal totalAmountFinalPartial = 0;
                        decimal totalAmountOldMinPremium = 0;
                        bool roundPremiumFinal = false;

                        foreach (CompanyCoverage coverage in coveragesApply)
                        {
                            decimal diffBeforeAfter = 0;
                            decimal percentageDiffBeforeAfter = 0;
                            decimal premiumAmt = 0;
                            decimal Rate = 0;
                            decimal Diff = 0;
                            if (coverage.PremiumAmount < 0 && coverage.CoverStatus != CoverageStatusType.Excluded)
                            {
                                diffBeforeAfter = Math.Abs(Math.Abs(coverage.PremiumAmount) - Math.Abs(coverage.AccumulatedPremiumAmount));
                                //Validamos que trc.AccumulatedPremiumAmount no sea 0
                                if (coverage.AccumulatedPremiumAmount != 0)
                                {
                                    percentageDiffBeforeAfter = (diffBeforeAfter * 100) / Math.Abs(coverage.AccumulatedPremiumAmount);
                                }
                                else
                                {
                                    percentageDiffBeforeAfter = 100;
                                }
                                Diff = Math.Abs(minPremiumMax) - Math.Abs(totalPremiumAmount); //Diferencia entre la prima minima y el total de primas que incluyen prima minima
                                decimal premiumAmount = Math.Abs((coverage.AccumulatedPremiumAmount == 0) ? coverage.PremiumAmount : coverage.AccumulatedPremiumAmount);
                                //Rate = (premiumAmount == 0) ? 0 : premiumAmount / Math.Abs(totalPremiumAmount); //Se obtiene el porcentaje
                                Rate = decimal.Round((premiumAmount == 0) ? 0 : premiumAmount / Math.Abs(totalPremiumAmount), QuoteManager.PremiumRoundValue); //Se obtiene el porcentaje --||
                                premiumAmt = decimal.Round((Rate * Diff) + premiumAmount, 2);
                                if (percentageDiffBeforeAfter > 0)
                                {
                                    premiumAmt += (premiumAmt / 100) * percentageDiffBeforeAfter;
                                }
                                if (percentageDiffBeforeAfter < 0)
                                {
                                    premiumAmt -= (premiumAmt / 100) * percentageDiffBeforeAfter;
                                }
                                premiumAmt = premiumAmt * -1;
                                applied = true;
                            }
                            else if (coverage.CoverStatus != CoverageStatusType.Excluded)
                            {
                                Diff = Math.Abs(minPremiumMax) - Math.Abs(totalPremiumAmount); //Diferencia entre la prima minima y el total de primas que incluyen prima minima
                                Rate = decimal.Round((coverage.PremiumAmount == 0) ? 0 : Math.Abs(coverage.PremiumAmount) / Math.Abs(totalPremiumAmount), 2); //Se obtiene el porcentaje
                                premiumAmt = decimal.Round((Rate * Diff) + Math.Abs(coverage.PremiumAmount), 2);
                                applied = true;
                            }
                            else
                            {
                                CalculatePremiumAmount(companyVehicle.Risk.Coverages);
                            }
                            //Inicion para redondeo final
                            totalAmountOldMinPremium += coverage.PremiumAmount; //Suma de las primas anteriores para validar cuando llegue a la ultima covertura y aproximar
                            totalAmountFinalPartial += decimal.Round(premiumAmt, 2); //Sumamos las primas resultantes para validar al final si sobra o falta y agregarlo o quitarlo
                            if (totalAmountOldMinPremium == totalPremiumAmount && !roundPremiumFinal)
                            {
                                //El total se paso o le falta
                                if ((totalAmountFinalPartial - minPremiumMax) != 0)
                                {
                                    premiumAmt -= (totalAmountFinalPartial - minPremiumMax);
                                }
                                //Ya se aproximo la ultima covertura que puede hacer eso
                                roundPremiumFinal = true;
                            }
                            //Fin para redondeo final
                            if (coverage.CoverStatus != CoverageStatusType.Excluded)
                            {
                                coverage.PremiumAmount = premiumAmt;
                            }
                        }
                        if (applied)
                        {
                            companyVehicle.CalculateMinPremium = true;
                        }
                        else
                        {
                            companyVehicle.CalculateMinPremium = false;
                        }
                    }
                    else if (appliedMinimumPremiumPreviusly)
                    {
                        foreach (CompanyCoverage trc in companyVehicle.Risk.Coverages)
                        {
                            if (trc.CoverStatus != CoverageStatusType.Excluded)
                            {
                                applied = true;
                                break;
                            }
                        }
                        if (applied)
                        {
                            companyVehicle.CalculateMinPremium = true;//Ya habia sido calculada, unicamente se informa
                        }
                        else
                        {
                            companyVehicle.CalculateMinPremium = false;
                        }
                    }
                    else
                    {
                        companyVehicle.CalculateMinPremium = false;
                    }
                }
            }
            else
            {
                companyVehicle.CalculateMinPremium = false;
            }

            return companyVehicle;
        }
        #region Calculo de Prima Minima para tasa Unica

        /// <summary>
        /// Calcula la prima minima del vehiculo
        /// </summary>
        /// <param name="companyVehicle">Modelo que contiene las coberturas</param>
        /// <param name="companyVehiclePolicy"> Modelo utilizado para capturar los dias de vigencia de la poliza</param>
        /// <param name="minimumPremiumAmount"></param>
        /// <param name="prorate"></param>
        private void CalculateMinimumPremiumVehicle(CompanyVehicle companyVehicle, CompanyPolicy companyVehiclePolicy, decimal minimumPremiumAmount, bool prorate, bool assistance,int CoverageIdAccNoOriginal )
        {
            var policyPeriod = companyVehiclePolicy.CurrentTo - companyVehiclePolicy.CurrentFrom;
            var flatRate = companyVehicle.Rate;
            var declaredAmount = companyVehicle.Price;
            if (flatRate == 0 || declaredAmount == 0 || minimumPremiumAmount == 0)
            {
                return;
            }
            var accessoryCoverageId = CoverageIdAccNoOriginal;
            decimal initialRiskPremium = declaredAmount * flatRate / 100 * policyPeriod.Days / 365M;
            List<CompanyCoverage> assistanceCoverages = companyVehicle.Risk.Coverages.Where(x => x.IsAssistance && x.IsAccMinPremium && x.RateType != Sistran.Core.Services.UtilitiesServices.Enums.RateType.FixedValue).ToList();
            decimal initialAssistancePremiumValue = assistanceCoverages.Sum(x => x.PremiumAmount);
            var a = companyVehicle.Risk.Coverages.FirstOrDefault(x => x.Id == accessoryCoverageId);
            var noOriginalPremiumAmount = (a != null ? a.PremiumAmount : 0);
            var riskPremium = initialRiskPremium + noOriginalPremiumAmount;
            if (assistance)
            {
                riskPremium += initialAssistancePremiumValue;
            }
            var applyForMinimumPremium = riskPremium < minimumPremiumAmount;
            if (!applyForMinimumPremium)
            {
                return;
            }
            var accessoriesPremiumParticipation = noOriginalPremiumAmount / riskPremium;
            var newAccessoriesPremiumAmount = minimumPremiumAmount * accessoriesPremiumParticipation;
            var newPremium = minimumPremiumAmount - newAccessoriesPremiumAmount;
            var assistanceParticipation = initialAssistancePremiumValue / riskPremium;
            var newAssistancePremiumAmount = minimumPremiumAmount * assistanceParticipation;
            if (assistance)
            {
                newPremium -= newAssistancePremiumAmount;
            }
            var newFlatRate = newPremium / declaredAmount;
            foreach (CompanyCoverage companyCoverage in companyVehicle.Risk.Coverages)
            {
                if (companyCoverage.RateType!=null && companyCoverage.RateType.Value == Sistran.Core.Services.UtilitiesServices.Enums.RateType.FixedValue)
                {
                    newPremium += companyCoverage.PremiumAmount;
                    continue;
                }
                if (!companyCoverage.IsAccMinPremium)
                {
                    continue;
                }
                if (companyCoverage.IsAssistance)
                {
                    if (assistance)
                    {
                        var coverageParticipationPercentage = companyCoverage.PremiumAmount / initialAssistancePremiumValue;
                        var newPremiumCoverage = coverageParticipationPercentage * newAssistancePremiumAmount;
                        var newRateCoverage = 100 * newPremiumCoverage / companyCoverage.SubLimitAmount;
                        companyCoverage.DiffMinPremiumAmount = newPremiumCoverage - companyCoverage.PremiumAmount;
                        companyCoverage.Rate = newRateCoverage;
                        companyCoverage.PremiumAmount = newPremiumCoverage;
                        newPremium += newPremiumCoverage;
                    }
                    else
                    {
                        continue;
                    }

                }
                if (companyCoverage.Id == accessoryCoverageId)
                {
                    UpdateAccessoriesPremium(companyVehicle.Accesories, newFlatRate, policyPeriod.Days, prorate);
                    companyCoverage.Rate = newFlatRate;
                    if (prorate)
                    {
                        companyCoverage.PremiumAmount = newFlatRate * companyCoverage.SubLimitAmount * policyPeriod.Days / 365M;
                    }
                    else
                    {
                        companyCoverage.PremiumAmount = newFlatRate * companyCoverage.SubLimitAmount;
                    }
                    newPremium += newAccessoriesPremiumAmount;
                    continue;
                }
                var flatRatePercentage = companyCoverage.FlatRatePorcentage;
                var newRate = newFlatRate * flatRatePercentage / 100;
                companyCoverage.Rate = newRate;
                if (prorate)
                {
                    companyCoverage.PremiumAmount = newRate * companyCoverage.SubLimitAmount * policyPeriod.Days / 365M;
                }
                else
                {
                    companyCoverage.PremiumAmount = newRate * companyCoverage.SubLimitAmount;
                }
            }
            companyVehicle.Risk.Premium = newPremium;
            companyVehicle.Rate = Math.Round(newFlatRate * 100, QuoteManager.PremiumRoundValue);
        }

        /// <summary>
        /// Actualiza la prima de los accesorios no originales 
        /// </summary>
        /// <param name="accessories"> Lista de los accesorios cargados </param>
        /// <param name="newFlatRate"></param>
        /// <param name="days"></param>
        /// <param name="prorate"></param>
        private void UpdateAccessoriesPremium(List<CompanyAccessory> accessories, decimal newFlatRate, int days, bool prorate)
        {
            foreach (var accessory in accessories.Where(x => !x.IsOriginal))
            {
                accessory.Rate = newFlatRate;
                if (prorate)
                {
                    accessory.Premium = accessory.Amount * newFlatRate * (days / 365M);
                }
                else
                {
                    accessory.Premium = newFlatRate * accessory.Amount;
                }
            }
        }

        #endregion

        public bool IsYearComplete(DateTime currentFrom, DateTime currentTo)
        {
            int dayFrom = currentFrom.Day;
            int monthFrom = currentFrom.Month;
            int yearFrom = currentFrom.Year;
            int dayTo = currentTo.Day;
            int monthTo = currentTo.Month;
            int yearTo = currentTo.Year;
            //Aqui obtenemos la diferencia en dias para la variable global de diferencia
            TimeSpan ts = currentTo - currentFrom;
            this.daysDifferenceCurrencyFromTo = ts.Days;

            if ((yearTo - yearFrom) == 1 && monthFrom == monthTo && dayFrom == dayTo)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Calculates the premium amount.
        /// </summary>
        /// <param name="listCompanyCoverage">The list company coverage.</param>
        private void CalculatePremiumAmount(List<CompanyCoverage> listCompanyCoverage)
        {
            decimal protectionAmt = 0;

            List<int> coverageProtection = new List<int> { Convert.ToInt32(coverageHeritage.TextParameter.Split('|')[0]), Convert.ToInt32(coverageHeritage.TextParameter.Split('|')[1]), Convert.ToInt32(coverageHeritage.TextParameter.Split('|')[2]) };
            CompanyCoverage protectionPatrimonial = listCompanyCoverage.FirstOrDefault(x => x.Id == coverageHeritage.NumberParameter);

            foreach (CompanyCoverage coverage in listCompanyCoverage.Where(x => coverageProtection.Contains(x.Id)))
            {
                protectionAmt = protectionAmt + coverage.PremiumAmount;
            }
            if (coverageProtection != null)
            {
                protectionPatrimonial.PremiumAmount = decimal.Round((protectionAmt * coverageHeritage.PercentageParameter.Value) / 100, 2);
                protectionPatrimonial.RateType = RateType.FixedValue;
                protectionPatrimonial.Rate = protectionPatrimonial.PremiumAmount;
            }
        }

        #region proteccion patrimonial
        /// <summary>
        /// Calculates the coverage patrimonial.
        /// </summary>
        /// <param name="listCompanyCoverage">The list company coverage.</param>
        /// <param name="lastCoverage">The last coverage.</param>
        /// <param name="CoveragePatProtecAmount">The coverage pat protec amount.</param>
        private void CalculateCoveragePatrimonial(List<CompanyCoverage> listCompanyCoverage, CompanyCoverage lastCoverage, decimal CoveragePatProtecAmount)
        {
            decimal diffPatProtect = 0;
            foreach (CompanyCoverage coverage in listCompanyCoverage)
            {
                if (coverage.Id == coverageHeritage.NumberParameter && CoveragePatProtecAmount != 0)
                {
                    diffPatProtect = decimal.Round(coverage.PremiumAmount - CoveragePatProtecAmount, 2);
                }

                if (lastCoverage.Id == coverage.Id)
                {
                    coverage.PremiumAmount = coverage.PremiumAmount - diffPatProtect;

                    if (coverage.RateType == RateType.FixedValue)
                    {
                        coverage.Rate = coverage.PremiumAmount;
                    }
                    else
                    {
                        coverage.Rate = decimal.Round(coverage.PremiumAmount * 100 / ((coverage.DeclaredAmount == 0) ? coverage.SubLimitAmount : coverage.DeclaredAmount), QuoteManager.PremiumRoundValue);
                    }

                    //Reverso
                    decimal diffFinal = 0;
                    ///////////////////////////
                    decimal premiumAmtFinal = decimal.Round((decimal)coverage.Rate * coverage.DeclaredAmount / 100, 2);
                    diffFinal = decimal.Round(premiumAmtFinal - coverage.PremiumAmount, 2);
                    //trc.PremiumAmount = premiumAmtFinal;
                    if (diffFinal > 0)
                    {
                        string rateString = Convert.ToString(coverage.Rate);
                        int lastDecimalRate = 0;// Convert.ToInt32(Convert.ToString(rateString[rateString.Length - 1]));
                        string lastDecimalRateTmp = Convert.ToString(rateString[rateString.Length - 1]);
                        int i = 1;
                        string number = "";
                        while (lastDecimalRateTmp != "," && lastDecimalRateTmp != ".")
                        {
                            if (lastDecimalRateTmp == "," || lastDecimalRateTmp == ".")
                            {
                                break;
                            }
                            lastDecimalRate = Convert.ToInt32(Convert.ToString(rateString[rateString.Length - i]));
                            if (lastDecimalRate > 0)
                            {
                                number = Convert.ToString(lastDecimalRate) + number;
                                break;
                            }
                            else
                            {
                                number += Convert.ToString(lastDecimalRate);
                            }
                            i++;
                            lastDecimalRateTmp = Convert.ToString(rateString[rateString.Length - i]);
                        }
                        lastDecimalRate = Convert.ToInt32(number);
                        if (lastDecimalRate > 0)
                            lastDecimalRate--;
                        rateString = rateString.Remove(rateString.Length - number.Length);
                        rateString += lastDecimalRate;
                        coverage.Rate = Convert.ToDecimal(rateString);
                    }
                }
            }
        }
        #endregion

    }
}
