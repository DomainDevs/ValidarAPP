using Sistran.Co.Application.Data;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Entities.views;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Utility;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    /// <summary>
    /// Cupo Operativo Cumulo
    /// </summary>
    public class AggregateDAO
    {
        private CoConsortiumViewV1 view;
        private Boolean IsConsortium = false;
        //private List<CoConsortium> CoconsortiumList;        
        private List<ConsortiumEvent> CoconsortiumList;
        private List<EconomicGroupEvent> economicGroupList;
        /// <summary>
        /// Gets the aggregate by individual identifier.
        /// </summary>
        /// <param name="ParamList">The parameter list.</param>
        /// <returns></returns>
        public ArrayList GetAggregateByIndividualId(Dictionary<String, Object> ParamList)
        {
            ArrayList response = new ArrayList();

            DataTable result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("QUO.GET_CALCULATE_CUMULUS", GetParameters(ParamList));
            }

            if (result != null && result.Rows.Count > 0)
            {
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    response.Add((Object[])result.Rows[i].ItemArray);
                }
            }

            return response;
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="oldParams">The old parameters.</param>
        /// <returns></returns>
        private NameValue[] GetParameters(Dictionary<string, object> oldParams)
        {
            NameValue[] parameters = new NameValue[oldParams.Count];

            int index = 0;
            foreach (KeyValuePair<string, object> nm in oldParams)
            {
                NameValue p = new NameValue();
                p.Name = nm.Key;
                p.Value = nm.Value;
                parameters[index] = p;
                index++;
            }
            return parameters;
        }

        /// <summary>
        /// Gets the quota.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <param name="lineBusinessCode">The line business code.</param>
        /// <param name="issueDate">The issue date.</param>
        /// <returns></returns>
        public decimal GetQuota(int individualId, int lineBusinessCode, DateTime issueDate)
        {
            decimal operatingQuota = 0;
            //Consorcios

            var primaryInsured = Insured.CreatePrimaryKey(individualId);
            Insured insured = (Insured)DataFacadeManager.GetObject(primaryInsured);


            //ObjectCriteriaBuilder filterconsotium = new ObjectCriteriaBuilder();
            //filterconsotium.Property(OperatingQuota.Properties.IndividualId);
            //filterconsotium.Equal();
            //filterconsotium.Constant(individualId);
            //var coConsortiumList = DataFacadeManager.GetObjects(typeof(CoConsortium), filterconsotium.GetPredicate());
            //CoconsortiumList = coConsortiumList.Cast<CoConsortium>().ToList();


            ObjectCriteriaBuilder filterconsotium = new ObjectCriteriaBuilder();
            filterconsotium.Property(ConsortiumEvent.Properties.ConsortiumId);
            filterconsotium.Equal();
            filterconsotium.Constant(individualId);

            var coConsortiumList = DataFacadeManager.GetObjects(typeof(ConsortiumEvent), filterconsotium.GetPredicate());
            CoconsortiumList = coConsortiumList.Cast<ConsortiumEvent>().ToList();

            ObjectCriteriaBuilder filterGroup = new ObjectCriteriaBuilder();
            filterGroup.Property(EconomicGroupEvent.Properties.IndividualId);
            filterGroup.Equal();
            filterGroup.Constant(individualId);

            var groupList = DataFacadeManager.GetObjects(typeof(EconomicGroupEvent), filterGroup.GetPredicate());
            economicGroupList = groupList.Cast<EconomicGroupEvent>().ToList();
            if (economicGroupList.Count > 0)
            {
                var economicGroupL = economicGroupList.Last();
                ObjectCriteriaBuilder filterGroupE = new ObjectCriteriaBuilder();
                filterGroupE.Property(EconomicGroupEvent.Properties.EconomicGroupId);
                filterGroupE.Equal();
                filterGroupE.Constant(economicGroupL.EconomicGroupId);
            }

            var groupPart = DataFacadeManager.GetObjects(typeof(EconomicGroupEvent), filterGroup.GetPredicate());
            economicGroupList = groupList.Cast<EconomicGroupEvent>().ToList();


            //traer Cupo Consorcios
            //Valdiar si la persona es un consorcio
            if (economicGroupList != null && economicGroupList.Count > 0)
            {
                foreach (EconomicGroupEvent cco in economicGroupList)
                {
                    ObjectCriteriaBuilder filterQuota = new ObjectCriteriaBuilder();
                    filterQuota.Property(OperatingQuota.Properties.IndividualId);
                    filterQuota.Equal();
                    filterQuota.Constant(cco.IndividualId);
                    filterQuota.And();
                    filterQuota.Property(OperatingQuota.Properties.CurrencyCode);
                    filterQuota.Equal();
                    filterQuota.Constant(0); //Se debe recuperar el Cupo Operativo en Pesos($)
                    filterQuota.And();
                    filterQuota.Property(OperatingQuota.Properties.LineBusinessCode);
                    filterQuota.Equal();
                    filterQuota.Constant(lineBusinessCode);
                    filterQuota.And();
                    filterQuota.Property(OperatingQuota.Properties.CurrentTo);
                    filterQuota.GreaterEqual();
                    filterQuota.Constant(issueDate);
                    OperatingQuota operatingQuotaInd = (OperatingQuota)DataFacadeManager.Instance.GetDataFacade().List(typeof(OperatingQuota), filterQuota.GetPredicate()).FirstOrDefault();
                    if (operatingQuotaInd != null)
                    {
                        operatingQuota = operatingQuota + operatingQuotaInd.OperatingQuotaAmount;
                    }
                }

            }
            if (coConsortiumList != null && coConsortiumList.Count > 0)
            {
                IsConsortium = true;
                CoconsortiumList.RemoveAll(x => x.IndividualId == 0);
                foreach (ConsortiumEvent cco in coConsortiumList)
                {
                    ObjectCriteriaBuilder filterQuota = new ObjectCriteriaBuilder();
                    filterQuota.Property(OperatingQuota.Properties.IndividualId);
                    filterQuota.Equal();
                    filterQuota.Constant(cco.IndividualId);
                    filterQuota.And();
                    filterQuota.Property(OperatingQuota.Properties.CurrencyCode);
                    filterQuota.Equal();
                    filterQuota.Constant(0); //Se debe recuperar el Cupo Operativo en Pesos($)
                    filterQuota.And();
                    filterQuota.Property(OperatingQuota.Properties.LineBusinessCode);
                    filterQuota.Equal();
                    filterQuota.Constant(lineBusinessCode);
                    filterQuota.And();
                    filterQuota.Property(OperatingQuota.Properties.CurrentTo);
                    filterQuota.GreaterEqual();
                    filterQuota.Constant(issueDate);
                    OperatingQuota operatingQuotaInd = (OperatingQuota)DataFacadeManager.Instance.GetDataFacade().List(typeof(OperatingQuota), filterQuota.GetPredicate()).FirstOrDefault();
                    if (operatingQuotaInd != null)
                    {
                        operatingQuota = operatingQuota + operatingQuotaInd.OperatingQuotaAmount;
                    }
                }

            }
            else
            {
                //No es consorcio
                ObjectCriteriaBuilder filterQuota = new ObjectCriteriaBuilder();
                filterQuota.Property(OperatingQuota.Properties.IndividualId);
                filterQuota.Equal();
                filterQuota.Constant(individualId);
                filterQuota.And();
                filterQuota.Property(OperatingQuota.Properties.CurrencyCode);
                filterQuota.Equal();
                filterQuota.Constant(0); //Se debe recuperar el Cupo Operativo en Pesos($)
                filterQuota.And();
                filterQuota.Property(OperatingQuota.Properties.LineBusinessCode);
                filterQuota.Equal();
                filterQuota.Constant(lineBusinessCode);
                filterQuota.And();
                filterQuota.Property(OperatingQuota.Properties.CurrentTo);
                filterQuota.GreaterEqual();
                filterQuota.Constant(issueDate);
                OperatingQuota operatingQuotaInd = (OperatingQuota)DataFacadeManager.Instance.GetDataFacade().List(typeof(OperatingQuota), filterQuota.GetPredicate()).FirstOrDefault();
                if (operatingQuotaInd != null)
                {
                    operatingQuota = operatingQuota + operatingQuotaInd.OperatingQuotaAmount;
                }

            }
            return operatingQuota;
        }

        /// <summary>
        /// Obtiene el Cupo Operativo y el Cumulo
        ///     <para>&#160;</para>
        ///     <para>
        ///         Valores Salida
        ///     </para>
        ///     <para>&#160;</para>
        ///     <para>Decimal  Cupo Operativo</para>       
        ///     <para>Decimal Cumulo</para>
        /// </summary>
        /// para
        ///<code>
        ///AggregateDAO aggregateDAO = new AggregateDAO();
        ///</code>      
        /// <param name="individualId">Individual Id Asegurado</param>
        /// <param name="currencyCode">Moneda</param>
        /// <param name="lineBusinessCode">Linea del Negocio</param>
        /// <param name="issueDate">Fecha Hasta</param>       
        /// <returns>Lista de sumas</returns>       
        public List<Amount> GetAvailableAmountByIndividualId(int individualId, int lineBusinessCode, DateTime issueDate)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<Amount> lstAmount = new List<Amount>();
            decimal aggregate = 0;
            decimal aggregateCo = 0;
            //Cupo Operativo
            decimal quota = GetQuota(individualId, lineBusinessCode, issueDate);
            //Cumulo
            if (IsConsortium)
            {
                Dictionary<string, object> ParamList = new Dictionary<string, object>();
                ParamList.Add("INDIVIDUAL_ID", individualId);
                ArrayList Aggreate = GetAggregateByIndividualId(ParamList);
                if (Aggreate != null)
                {
                    aggregate = (((decimal)((object[])Aggreate[0])[0])) + ((decimal)((object[])Aggreate[0])[1]) - ((decimal)((object[])Aggreate[0])[2]);
                }
            }
            else
            {
                Dictionary<string, object> ParamList = new Dictionary<string, object>();
                ParamList.Add("INDIVIDUAL_ID", individualId);
                ArrayList Aggreate = GetAggregateByIndividualId(ParamList);
                if (Aggreate != null)
                {
                    aggregate = (decimal)((object[])Aggreate[0])[0] + (decimal)((object[])Aggreate[0])[1] - (decimal)((object[])Aggreate[0])[2];
                }
                else
                {
                    aggregate = 0;
                }
                ///Verifica si el Afianzado esta en algun consorcio para tomar cumuloadicional
                if (IsConsortiumindividualId(individualId))
                {
                    if (view.CoConsortiumList != null && view.CoConsortiumList.Count > 0)
                    {

                        var individualIds = (from insured in view.InsuredList.Cast<Insured>()
                                             join coConsortium in view.CoConsortiumList.Cast<CoConsortium>()
                                            on insured.InsuredCode equals coConsortium.InsuredCode
                                             select new { ParticipationRate = coConsortium.ParticipationRate, individualId = insured.IndividualId }).ToList();
                        // List<int> individualIds = view.InsuredList.Cast<Insured>().Where(m => view.CoConsortiumList.Cast<CoConsortium>().Select(s => s.InsuredCode).Contains(m.InsuredCode)).Select(a => a.IndividualId).ToList();
                        for (int a = 0; a < individualIds.ToList().Count(); a++)
                        {
                            Dictionary<string, object> ParamListCo = new Dictionary<string, object>();
                            ParamListCo.Add("INDIVIDUAL_ID", individualIds[a].individualId);
                            ArrayList AggreateCo = GetAggregateByIndividualId(ParamListCo);
                            if (AggreateCo != null)
                            {
                                aggregateCo = aggregateCo + (((decimal)((object[])AggreateCo[0])[0]) * individualIds[a].ParticipationRate) / 100 + (((decimal)((object[])AggreateCo[0])[1]) * individualIds[a].ParticipationRate) / 100 - (((decimal)((object[])AggreateCo[0])[2]) * individualIds[a].ParticipationRate) / 100;
                            }
                            else
                            {
                                aggregateCo = 0;
                            }
                        }
                    }
                }

            }
            lstAmount.Add(new Amount() { Value = quota });
            lstAmount.Add(new Amount() { Value = aggregate + aggregateCo });

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetAvailableAmountByIndividualId");
            return lstAmount;
        }


        /// <summary>
        /// determinar si el individuales un cosrciado buscarlo en los consorcios
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        private bool IsConsortiumindividualId(int individualId)
        {
            //Consorcios
            view = new CoConsortiumViewV1();
            ViewBuilder builder = new ViewBuilder("CoConsortiumViewV1");
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CoConsortium.Properties.IndividualId, typeof(CoConsortium).Name).Equal().Constant(individualId);
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            if (view.CoConsortiumList != null && view.CoConsortiumList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
