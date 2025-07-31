using Sistran.Co.Application.Data;
using Sistran.Company.Application.CollectionFormBusinessService.Models;
using Sistran.Company.Application.CollectionFormBusinessService.Types;
using Sistran.Company.Application.CollectionFormBusinessServiceProvider.Clases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.CollectionFormBusinessServiceProvider.Dao
{
    public class ReportPolicyDAO
    {
        /// <summary>
        /// Metodo que busca la informacion correspondiente a las cuotas de pagos de la poliza y sus endosos
        /// </summary>
        /// <param name="branchId">Codigo Sucursal</param>
        /// <param name="prefixId">Codigo Ramo</param>
        /// <param name="documentNumber">Numero Poliza</param>
        /// <returns>Objeto tipo Policy</returns>
        public ReportPolicy GetPolicybyBranchPrefixDocument(int branchId, int prefixId, int documentNumber)
        {
            ReportPolicy policy = new ReportPolicy();
            try
            {
                ArrayList arrayDataPolicy2G = new ArrayList();
                ArrayList arrayDataPolicy3G = new ArrayList();
                //SubscriptionText subscriptionText = new SubscriptionText();

                List<EndorsementQuote> listEndorsementQuote = new List<EndorsementQuote>();
                List<EndorsementQuote> listEndorsementQuote2G = new List<EndorsementQuote>();
                List<EndorsementQuote> listEndorsementQuote3G = new List<EndorsementQuote>();

                EndorsementQuoteSearch Endorsement2GSearch = null;
                EndorsementQuoteSearch Endorsement3GSearch = null;

                Endorsement2GSearch = searchEndorsementQuotes(Types.SystranSystem.Sise2G, branchId, prefixId, documentNumber);
                Endorsement3GSearch = searchEndorsementQuotes(Types.SystranSystem.Sise3G, branchId, prefixId, documentNumber);

                //Si no vinieron resultados de 2G pero si de 3G, solo usamos los de 3G
                if (Endorsement2GSearch.searchResult == Types.EndorsementQuoteSearchResults.PolicyNotExists &&
                    Endorsement3GSearch.searchResult == Types.EndorsementQuoteSearchResults.RecordsFound)
                {
                    listEndorsementQuote = Endorsement3GSearch.list;
                }

                //Si no vinieron resultados de 3G pero si de 2G, solo usamos los de 2G
                if (Endorsement3GSearch.searchResult == Types.EndorsementQuoteSearchResults.PolicyNotExists &&
                    Endorsement2GSearch.searchResult == Types.EndorsementQuoteSearchResults.RecordsFound)
                {
                    listEndorsementQuote = Endorsement2GSearch.list;
                }

                //Si no vinieron resultados de 3G ni de 2G
                if (Endorsement3GSearch.searchResult == Types.EndorsementQuoteSearchResults.PolicyNotExists &&
                    Endorsement2GSearch.searchResult == Types.EndorsementQuoteSearchResults.PolicyNotExists)
                {
                    policy.Textbody = "La póliza buscada no existe";
                    return policy;
                }

                //Si en 3G y 2G la poliza no fue emitida en una moneda valida
                if (Endorsement3GSearch.searchResult == Types.EndorsementQuoteSearchResults.PolicyNotEmitedInValidCurrency &&
                    Endorsement2GSearch.searchResult == Types.EndorsementQuoteSearchResults.PolicyNotEmitedInValidCurrency)
                {
                    policy.Textbody = "La póliza se encuentra emitida en una moneda no permitida para Recaudos";
                    return policy;
                }

                //Si en 3G la póliza no fue emitida con una moneda valida y en 2G no hay resultados
                if (Endorsement3GSearch.searchResult == Types.EndorsementQuoteSearchResults.PolicyNotEmitedInValidCurrency &&
                    Endorsement2GSearch.searchResult == Types.EndorsementQuoteSearchResults.PolicyNotExists)
                {
                    policy.Textbody = "La póliza se encuentra emitida en una moneda no permitida para Recaudos";
                    return policy;
                }

                //Si en 2G la póliza no fue emitida con una moneda valida y en 3G no hay resultados
                if (Endorsement2GSearch.searchResult == Types.EndorsementQuoteSearchResults.PolicyNotEmitedInValidCurrency &&
                    Endorsement3GSearch.searchResult == Types.EndorsementQuoteSearchResults.PolicyNotExists)
                {
                    policy.Textbody = "La póliza se encuentra emitida en una moneda no permitida para Recaudos";
                    return policy;
                }

                List<EndorsementQuote> finalList = null;
                EndorsementQuoteSet endorsementsSetTmp = null;

                // Cuando se encuentran resultados en ambas bases de datos, se unifica el resultado
                if (Endorsement3GSearch.searchResult == Types.EndorsementQuoteSearchResults.RecordsFound &&
                    Endorsement2GSearch.searchResult == Types.EndorsementQuoteSearchResults.RecordsFound)
                {
                    listEndorsementQuote2G = Endorsement2GSearch.list;
                    listEndorsementQuote3G = Endorsement3GSearch.list;

                    EndorsementQuoteSet endorsementsSet3G = null;

                    endorsementsSetTmp = new EndorsementQuoteSet();

                    if (Endorsement3GSearch.searchResult == Types.EndorsementQuoteSearchResults.RecordsFound)
                    {
                        //Agregamos los endosos de 3G
                        endorsementsSet3G = new EndorsementQuoteSet(Endorsement3GSearch.list);
                        endorsementsSetTmp = new EndorsementQuoteSet(Endorsement3GSearch.list);
                    }

                    if (Endorsement2GSearch.searchResult == Types.EndorsementQuoteSearchResults.RecordsFound)
                    {
                        foreach (EndorsementQuote tmpQuote2G in Endorsement2GSearch.list)
                        {
                            EndorsementQuote oEndorsementQuote3G = endorsementsSet3G.getEndorsementQuote(tmpQuote2G.EndorsementNumber.ToString(), tmpQuote2G.QuoteNum.ToString());
                            endorsementsSetTmp.addOrReplace(tmpQuote2G);
                        }
                    }
                }

                if (endorsementsSetTmp != null)
                {
                    finalList = endorsementsSetTmp.getList();
                }
                else
                {
                    finalList = listEndorsementQuote;
                }

                // Se retiran de la lista, las cuotas cuyo estado sea 2 (pagado) y que tengan valores de pago y/o de prima menores a 0.
                finalList = (from value in (IEnumerable<EndorsementQuote>)finalList.ToArray()
                             where value.Status != 2
                             && (value.TotalPremium > 0)
                             && (value.Amount > 0)
                             //&& (value.Payerexpdate >= DateTime.Now)
                             select value).ToList<EndorsementQuote>();

                if (finalList.Count == 0)
                {
                    policy.Textbody = "La opción buscada no tiene cuotas pendientes";
                    return policy;
                }

                // Se obtienen los endosos cuyas cuotas no estén canceladas
                var query = (from item in finalList.AsEnumerable()
                             orderby item.EndorsementNumber
                             select item.EndorsementNumber).Distinct();

                List<int> endosos = new List<int>();
                foreach (var endorsementId in query)
                    endosos.Add(endorsementId);

                policy.Id = 1;
                policy.Endorsements = new List<ReportEndorsement>();
                for (int i = 0; i < endosos.Count; i++)
                {
                    ReportEndorsement endorsement = new ReportEndorsement();
                    endorsement.Id = endosos[i];
                    endorsement.Payers = new List<ReportPayer>();
                    ReportPayer payer = new ReportPayer();
                    payer.IndividualId = finalList[i].PayerId;
                    payer.Name = finalList[i].Name;
                    payer.PaymentSchedule = new ReportPaymentSchedule();
                    List<ReportInstallment> listInstallment = new List<ReportInstallment>();
                    for (int j = 0; j < finalList.Count; j++)
                    {
                        if (endosos[i] == finalList[j].EndorsementNumber)
                        {
                            ReportInstallment installment = new ReportInstallment();
                            ReportAmount amount = new ReportAmount();
                            amount.Value = Convert.ToDecimal(finalList[j].TotalPremium);
                            installment.Amount = amount;
                            installment.DueDate = finalList[j].Payerexpdate;
                            installment.InstallmentNumber = finalList[j].QuoteNum;
                            if (Convert.ToBoolean(finalList[j].Status))
                                installment.IsPartialPaid = true;
                            else
                                installment.IsPartialPaid = false;
                            ReportAmount paidAmount = new ReportAmount();
                            paidAmount.Value = Convert.ToDecimal(finalList[j].Amount);
                            installment.PaidAmount = paidAmount;
                            listInstallment.Add(installment);
                        }
                    }
                    payer.PaymentSchedule.Installments = listInstallment;
                    endorsement.Payers.Add(payer);
                    policy.Endorsements.Add(endorsement);
                }
            }
            catch (Exception ex)
            {
                policy.Textbody = ex.Message.ToString();
            }
            return policy;
        }

        private EndorsementQuoteSearch searchEndorsementQuotes(Types.SystranSystem SS, int branchId, int prefixId, int documentNumber)
        {
            EndorsementQuoteSearch ans = new EndorsementQuoteSearch();
            List<EndorsementQuote> list = null;

            ArrayList arrayDataPolicy = new ArrayList();
            switch (SS)
            {
                case Types.SystranSystem.Sise2G:
                    {
                        arrayDataPolicy = findDataInSiseDB2G(branchId, prefixId, documentNumber); //Busca en 2G
                        if (arrayDataPolicy.Count == 1)
                        {
                            if (((DataRow)arrayDataPolicy[0]).ItemArray[0].ToString().Equals("2"))
                            {
                                list = null;
                                ans.searchResult = Types.EndorsementQuoteSearchResults.PolicyNotExists;
                            }
                            else
                            {
                                if (((DataRow)arrayDataPolicy[0]).ItemArray[0].ToString().Equals("3"))
                                {
                                    list = null;
                                    ans.searchResult = Types.EndorsementQuoteSearchResults.PolicyNotEmitedInValidCurrency;
                                }
                                else
                                {
                                    list = order2GDataByPolicy(arrayDataPolicy);
                                    ans.searchResult = Types.EndorsementQuoteSearchResults.RecordsFound;
                                }
                            }

                        }
                        else
                        {
                            //Si no ha habido resultado de "No datos" asumimos que éstos existen y se pueden cargar.
                            if (ans.searchResult == Types.EndorsementQuoteSearchResults.NotDefined)
                            {
                                list = order2GDataByPolicy(arrayDataPolicy);
                                ans.searchResult = Types.EndorsementQuoteSearchResults.RecordsFound;
                            }

                        }
                        ans.searchResult = Types.EndorsementQuoteSearchResults.PolicyNotExists;
                        ans.list = list;
                    }
                    break;
                case Types.SystranSystem.Sise3G:
                    {
                        DataSet dsResult = findDataInSiseDB3G(branchId, prefixId, documentNumber); // Busca en 3G
                        if (dsResult.Tables[0].Rows.Count == 1)
                        {
                            if (dsResult.Tables[0].Rows[0][0].ToString().Equals("2"))
                            {
                                list = null;
                                ans.searchResult = Types.EndorsementQuoteSearchResults.PolicyNotExists;
                            }
                            else
                            {
                                if (dsResult.Tables[0].Rows[0][0].ToString().Equals("3"))
                                {
                                    list = null;
                                    ans.searchResult = Types.EndorsementQuoteSearchResults.PolicyNotEmitedInValidCurrency;
                                }
                                else
                                {
                                    list = order3GDataByPolicy(dsResult);
                                    ans.searchResult = Types.EndorsementQuoteSearchResults.RecordsFound;
                                }
                            }
                        }
                        else
                        {
                            //Si no ha habido resultado de "No datos" asumimos que éstos existen y se pueden cargar.
                            if (ans.searchResult == Types.EndorsementQuoteSearchResults.NotDefined)
                            {
                                list = order3GDataByPolicy(dsResult);
                                ans.searchResult = Types.EndorsementQuoteSearchResults.RecordsFound;
                            }
                        }
                        ans.list = list;
                    }
                    break;
                case Types.SystranSystem.NotDefined:
                    {
                        throw new Exception("No se puede buscar una poliza si no se define el sistema para búsqueda: NotDefined");
                    }
                default:
                    {
                        throw new Exception("No se puede buscar una poliza si no se define el sistema para búsqueda: NotDefined");
                    }
            }
            return ans;
        }

        /// <summary>
        /// Metodo que realiza la consulta a la base de datos de SISE 2G
        /// </summary>
        /// <param name="branchId">Sucursal</param>
        /// <param name="prefixId">Ramo</param>
        /// <param name="documentNumber">Poliza</param>
        /// <returns>Array List</returns>
        private ArrayList findDataInSiseDB2G(int branchId, int prefixId, int documentNumber)
        {
            try
            {
                DataSet dsResult = new DataSet();
                ArrayList myArrayList = new ArrayList();

                using (DynamicDataAccess pdb = new DynamicDataAccess(ConfigurationManager.AppSettings["Sise2GDB"].ToString()))
                {
                    NameValue[] parametersp = new NameValue[3];
                    parametersp[0] = new NameValue("BRANCH", branchId);
                    parametersp[1] = new NameValue("PREFIX", prefixId);
                    parametersp[2] = new NameValue("NUMPOL", documentNumber);
                    dsResult = pdb.ExecuteSPDataSet("CIA_GET_DATA_POLICY", parametersp);
                }

                foreach (DataRow dtRow in dsResult.Tables[0].Rows)
                    myArrayList.Add(dtRow);

                return myArrayList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Metodo que realiza la consulta a la base de datos de SISE 3G
        /// </summary>
        /// <param name="branchId">Sucursal</param>
        /// <param name="prefixId">Ramo</param>
        /// <param name="documentNumber">Poliza</param>
        /// <returns>Array List</returns>
        private DataSet findDataInSiseDB3G(int branchId, int prefixId, int documentNumber)
        {
            try
            {
                using (DynamicDataAccess dda = new DynamicDataAccess())
                {
                    NameValue[] parameters = new NameValue[3];
                    parameters[0] = new NameValue("BRANCH", branchId);
                    parameters[1] = new NameValue("PREFIX", prefixId);
                    parameters[2] = new NameValue("DOCNUM", documentNumber);
                    DataSet dsResult = dda.ExecuteSPSerDataSet("REPORT.CIA_GET_DATA_POLICY", parameters);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Metodo que organiza los datos retornados de 2G
        /// </summary>
        /// <param name="arrayDataPolicy"></param>
        /// <returns></returns>
        private List<EndorsementQuote> order2GDataByPolicy(ArrayList arrayDataPolicy)
        {
            try
            {
                List<EndorsementQuote> listEndorsementQuote = new List<EndorsementQuote>();
                for (int i = 0; i < arrayDataPolicy.Count; i++)
                {
                    EndorsementQuote endorsementQuote = new EndorsementQuote();
                    endorsementQuote.EndorsementId = Convert.ToInt32(((DataRow)arrayDataPolicy[i]).ItemArray[0].ToString());
                    endorsementQuote.EndorsementNumber = Convert.ToInt32(((DataRow)arrayDataPolicy[i]).ItemArray[1].ToString());
                    endorsementQuote.QuoteNum = Convert.ToInt32(((DataRow)arrayDataPolicy[i]).ItemArray[2].ToString());
                    endorsementQuote.PayerId = Convert.ToInt32(((DataRow)arrayDataPolicy[i]).ItemArray[3].ToString());
                    endorsementQuote.Name = ((DataRow)arrayDataPolicy[i]).ItemArray[4].ToString();
                    endorsementQuote.Payerexpdate = Convert.ToDateTime(((DataRow)arrayDataPolicy[i]).ItemArray[5]);
                    endorsementQuote.Status = Convert.ToInt32(((DataRow)arrayDataPolicy[i]).ItemArray[6]);
                    endorsementQuote.Amount = Convert.ToDecimal(((DataRow)arrayDataPolicy[i]).ItemArray[7]);
                    endorsementQuote.TotalPremium = Convert.ToDecimal(((DataRow)arrayDataPolicy[i]).ItemArray[8]);
                    listEndorsementQuote.Add(endorsementQuote);
                }
                return listEndorsementQuote;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Metodo que organiza los datos retornados de 3G
        /// </summary>
        /// <param name="arrayDataPolicy"></param>
        /// <returns></returns>
        private List<EndorsementQuote> order3GDataByPolicy(DataSet dsyDataPolicy)
        {
            try
            {
                List<EndorsementQuote> listEndorsementQuote = new List<EndorsementQuote>();
                for (int i = 0; i < dsyDataPolicy.Tables[0].Rows.Count; i++)
                {
                    EndorsementQuote endorsementQuote = new EndorsementQuote();
                    endorsementQuote.EndorsementId = Convert.ToInt32(dsyDataPolicy.Tables[0].Rows[i][0].ToString());
                    endorsementQuote.EndorsementNumber = Convert.ToInt32(dsyDataPolicy.Tables[0].Rows[i][1].ToString());
                    endorsementQuote.QuoteNum = Convert.ToInt32(dsyDataPolicy.Tables[0].Rows[i][2].ToString());
                    endorsementQuote.PayerId = Convert.ToInt32(dsyDataPolicy.Tables[0].Rows[i][3].ToString());
                    endorsementQuote.Name = dsyDataPolicy.Tables[0].Rows[i][4].ToString();
                    endorsementQuote.Payerexpdate = Convert.ToDateTime(dsyDataPolicy.Tables[0].Rows[i][5]);
                    endorsementQuote.Status = Convert.ToInt32(dsyDataPolicy.Tables[0].Rows[i][6]);
                    endorsementQuote.Amount = Convert.ToDecimal(dsyDataPolicy.Tables[0].Rows[i][7]);
                    endorsementQuote.TotalPremium = Convert.ToDecimal(dsyDataPolicy.Tables[0].Rows[i][8]);
                    listEndorsementQuote.Add(endorsementQuote);
                }
                return listEndorsementQuote;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
