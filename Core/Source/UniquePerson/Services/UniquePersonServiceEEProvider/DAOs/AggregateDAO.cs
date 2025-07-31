using Sistran.Co.Application.Data;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.Entities.views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Sistran.Core.Application.UniquePerson.Entities;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    /// <summary>
    /// Cupo Operativo Cumulo
    /// </summary>
    public class AggregateDAO
    {
        private ConsorcioViewCo view;
        private Boolean IsConsortium = false;

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
            view = new ConsorcioViewCo();
            ViewBuilder builder = new ViewBuilder("ConsorcioViewCo");
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Insured.Properties.IndividualId, typeof(Insured).Name);
            filter.Equal();
            filter.Constant(individualId);
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            //traer Cupo Consorcios
            //Valdiar si la persona es un consorcio
            if (view.CoAssociationTypeList != null && view.CoAssociationTypeList.Count > 0)
            {
                foreach (CoAssociationType coAsocType in view.CoAssociationTypeList)
                {
                    IsConsortium = coAsocType.IsConsortium;
                }
            }
            if (view.CoConsortiumList != null && view.CoConsortiumList.Count > 0)
            {
                foreach (CoConsortium cco in view.CoConsortiumList)
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
                        foreach (CoConsortium cco in view.CoConsortiumList)
                        {
                            Insured lstInsured = (from p in view.InsuredList.Cast<Insured>().ToList()
                                                  where p.InsuredCode == cco.InsuredCode
                                                  select p).FirstOrDefault();

                            if (lstInsured != null)
                            {
                                Dictionary<string, object> ParamListCo = new Dictionary<string, object>();
                                ParamListCo.Add("INDIVIDUAL_ID", lstInsured.IndividualId);
                                ArrayList AggreateCo = GetAggregateByIndividualId(ParamListCo);
                                if (AggreateCo != null)
                                {
                                    aggregateCo = (((decimal)((object[])AggreateCo[0])[0]) * cco.ParticipationRate) / 100 + (((decimal)((object[])AggreateCo[0])[1]) * cco.ParticipationRate) / 100 - (((decimal)((object[])AggreateCo[0])[2]) * cco.ParticipationRate) / 100;
                                }
                                else
                                {
                                    aggregateCo = 0;
                                }
                            }
                        }

                    }
                }

            }
            lstAmount.Add(new Amount() { Value = quota });
            lstAmount.Add(new Amount() { Value = aggregate + aggregateCo });

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAvailableAmountByIndividualId");
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
            view = new ConsorcioViewCo();
            ViewBuilder builder = new ViewBuilder("ConsorcioViewCo");
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
