using Sistran.Co.Application.Data;
using System;
using System.Collections.Generic;
using Sistran.Co.Previsora.Application.FullServices.Models;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Co.Previsora.Application.FullServicesProvider.Enum;

namespace Sistran.Co.Previsora.Application.FullServicesProvider.Helpers
{
    public class OperatingQuotaHelper
    {
        OperatingQuotaResponse response = new OperatingQuotaResponse();
        private string IndividualString;
        private static string user = "OPERATINGQUOTA_WS";
        private static int idApplication = 0;

        private DatatableToList DatatableToList = new DatatableToList();
        public double GetExchangeRateDate(DateTime operatingQuotaExchangeRateDate, int currencyCd)
        {
            NameValue[] parameters = new NameValue[2];
            double rateDate = 0;
            parameters[0] = new NameValue("@EXCHANGE_DATE", operatingQuotaExchangeRateDate);
            parameters[1] = new NameValue("@CURRENCY_CD", currencyCd);
            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataTable("UP.CO_GET_EXCHANGE_RATE", parameters);
            }

            if (result != null && result.Rows.Count > 0)
            {
                if (double.TryParse(result.Rows[0][0].ToString(), out rateDate))
                {
                    rateDate = Convert.ToDouble(result.Rows[0][0]);
                }
            }
            return rateDate;


        }
        public List<OperatingQuotaIndividual> GetIndividualOperatingQuota(int identificationType, string identificationId)
        {
            NameValue[] parameters = new NameValue[2];
            List<OperatingQuotaIndividual> ListOperatingQuotaIndividual = new List<OperatingQuotaIndividual>();
            int rowcount = 0;
            parameters[0] = new NameValue("@IDENTIFICATION_TYPE", identificationType);
            parameters[1] = new NameValue("@IDENTIFICATION_ID", identificationId);
            DataSet result;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataSet("UP.CO_GET_INDIVIDUAL_OPERATING_QUOTA", parameters);
            }
            if (result != null && result.Tables[0].Rows.Count > 0)
            {
                ListOperatingQuotaIndividual = DatatableToList.ConvertTo<OperatingQuotaIndividual>(result.Tables[0]);
                ListOperatingQuotaIndividual.ForEach(delegate (OperatingQuotaIndividual element) { element.Identity = rowcount++; element.State = 'R'; });
            }
            return ListOperatingQuotaIndividual;

        }
        public List<TableMessage> ModifyOperatingQuotaProcess(List<OPERATING_QUOTA> businessObject, List<TableMessage> ListTableMessage)
        {
            foreach (OPERATING_QUOTA mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
                tMessage.Message = "True";
                tMessage.NameTable = "OPERATING_QUOTA";
                try
                {
                    switch ((OperatingProcessType)mp.State)
                    {
                        case OperatingProcessType.Create:
                            tMessage.Message = InsertOperatingQuota(mp).ToString();
                            break;
                        case OperatingProcessType.Update:
                            tMessage.Message = UpdateOperatingQuota(mp).ToString();
                            break;
                        case OperatingProcessType.Delete:
                            tMessage.Message = DeleteOperatingQuota(mp).ToString();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    tMessage.Message = ex.Source;
                    continue;
                }
                finally
                {
                    ListTableMessage.Add(tMessage);
                }

            }
            return ListTableMessage;
        }
        public bool InsertOperatingQuota(OPERATING_QUOTA businessObject)
        {
            NameValue[] parameters = new NameValue[7];
            bool resultOperation = false;
            List<OperatingQuotaIndividual> ListOperatingQuotaIndividual = new List<OperatingQuotaIndividual>();
            parameters[0] = new NameValue("@INDIVIDUAL_ID", businessObject.INDIVIDUAL_ID);
            parameters[1] = new NameValue("@LINE_BUSINESS_CD", businessObject.LINE_BUSINESS_CD);
            parameters[2] = new NameValue("@CURRENCY_CD", businessObject.CURRENCY_CD);
            parameters[3] = new NameValue("@OPERATING_QUOTA_AMT", businessObject.OPERATING_QUOTA_AMT);
            parameters[4] = new NameValue("@CURRENT_TO", businessObject.CURRENT_TO);

            parameters[5] = new NameValue("@usser", user);
            parameters[6] = new NameValue("@id_aplication", idApplication);
            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataTable("SUP.OPERATING_QUOTA_Insert", parameters);
            }
            if (result != null)
            {
                resultOperation = true;
            }

            return resultOperation;
        }
        public bool UpdateOperatingQuota(OPERATING_QUOTA businessObject)
        {
            NameValue[] parameters = new NameValue[7];
            bool resultOperation = false;
            List<OperatingQuotaIndividual> ListOperatingQuotaIndividual = new List<OperatingQuotaIndividual>();
            parameters[0] = new NameValue("@INDIVIDUAL_ID", businessObject.INDIVIDUAL_ID);
            parameters[1] = new NameValue("@LINE_BUSINESS_CD", businessObject.LINE_BUSINESS_CD);
            parameters[2] = new NameValue("@CURRENCY_CD", businessObject.CURRENCY_CD);
            parameters[3] = new NameValue("@OPERATING_QUOTA_AMT", businessObject.OPERATING_QUOTA_AMT);
            parameters[4] = new NameValue("@CURRENT_TO", businessObject.CURRENT_TO);

            parameters[5] = new NameValue("@usser", user);
            parameters[6] = new NameValue("@id_aplication", idApplication);
            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataTable("SUP.OPERATING_QUOTA_Update", parameters);
            }
            if (result != null)
            {
                resultOperation = true;
            }

            return resultOperation;
        }
        public bool DeleteOperatingQuota(OPERATING_QUOTA businessObject)
        {
            NameValue[] parameters = new NameValue[7];
            bool resultOperation = false;
            List<OperatingQuotaIndividual> ListOperatingQuotaIndividual = new List<OperatingQuotaIndividual>();
            parameters[0] = new NameValue("@INDIVIDUAL_ID", businessObject.INDIVIDUAL_ID);
            parameters[1] = new NameValue("@LINE_BUSINESS_CD", businessObject.LINE_BUSINESS_CD);
            parameters[2] = new NameValue("@CURRENCY_CD", businessObject.CURRENCY_CD);
            parameters[3] = new NameValue("@OPERATING_QUOTA_AMT", businessObject.OPERATING_QUOTA_AMT);
            parameters[4] = new NameValue("@CURRENT_TO", businessObject.CURRENT_TO);

            parameters[5] = new NameValue("@usser", user);
            parameters[6] = new NameValue("@id_aplication", idApplication);
            DataTable result;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataTable("SUP.OPERATING_QUOTA_Delete", parameters);
            }
            if (result != null)
            {
                resultOperation = true;
            }

            return resultOperation;
        }
        public int ExecModifyOperatingQuota(OperatingQuotaObj OperatingQuotaObj, int typeTransaction)
        {
            NameValue[] parameters = new NameValue[7];
            List<OperatingQuotaIndividual> ListOperatingQuotaIndividual = new List<OperatingQuotaIndividual>();
            parameters[0] = new NameValue("@INDIVIDUAL_ID", OperatingQuotaObj.IndividualId);
            parameters[1] = new NameValue("@LINE_BUSINESS_CD", OperatingQuotaObj.LineBusinessCd);
            parameters[2] = new NameValue("@CURRENCY_CD", OperatingQuotaObj.CurrencyCd);
            parameters[3] = new NameValue("@OPERATING_QUOTA_AMT", OperatingQuotaObj.OperatingQuotaAmt);
            parameters[4] = new NameValue("@CURRENT_TO", OperatingQuotaObj.CurrentTo);

            parameters[5] = new NameValue("@usser", user);
            parameters[6] = new NameValue("@id_aplication", idApplication);
            var result = 0;
            string spAction = string.Empty;
            if (typeTransaction == 1)
            {
                spAction = "Update";
            }
            else
            {
                spAction = "Insert";
            }
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = Convert.ToInt32(pdb.ExecuteSPScalar(string.Format("SUP.OPERATING_QUOTA_{0}", spAction), parameters));
            }
            return result;
        }
        public int ExecModifyOperatingQuotalog(OperatingQuotaObj OperatingQuotaObj, OperatingQuotalog OperatingQuotalog)
        {
            NameValue[] parameters = new NameValue[9];
            List<OperatingQuotaIndividual> ListOperatingQuotaIndividual = new List<OperatingQuotaIndividual>();
            parameters[0] = new NameValue("@INDIVIDUAL_ID", OperatingQuotaObj.IndividualId);
            parameters[1] = new NameValue("@LINE_BUSINESS_CD", OperatingQuotaObj.LineBusinessCd);
            parameters[2] = new NameValue("@CURRENCY_CD", OperatingQuotaObj.CurrencyCd);
            parameters[3] = new NameValue("@OPERATING_QUOTA_AMT", OperatingQuotaObj.OperatingQuotaAmt);
            parameters[4] = new NameValue("@OLD_OPERATING_QUOTA_AMT", OperatingQuotalog.OldOperatingQuotaAmt);
            parameters[5] = new NameValue("@CURRENT_TO", OperatingQuotaObj.CurrentTo);
            parameters[6] = new NameValue("@ACTION", OperatingQuotalog.TransactionType);
            parameters[7] = new NameValue("@REGISTRATION_DATE", OperatingQuotalog.TransactionDate);
            parameters[8] = new NameValue("@IS_WS", OperatingQuotalog.IsWs);

            var result = 0;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = Convert.ToInt32(pdb.ExecuteSPScalar("OPERATING_QUOTA_LOG_INSERT", parameters));
            }
            return result;
        }

        public double ValidateExistOperatingQuota(int individualId)
        {
            NameValue[] parameters = new NameValue[1];
            double resultOperation = 0;
            parameters[0] = new NameValue("@individual_id", individualId);
            DataSet result;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataSet("SUP.GET_OPERATING_QUOTA", parameters);
            }
            if (result != null && result.Tables[0].Rows.Count > 0)
            {
                resultOperation = Convert.ToDouble(result.Tables[0].Rows[0][0].ToString());
            }
            return resultOperation;
        }

        public OperatingQuotaResponse RegisterOperativeQuota(WSOperatingQuota operatingQuota)
        {

            double OperatingExchangeRate;
            double OperatingQuotaDollarVal = 0;
            double ValidateOldOperatingQuota;
            OperatingQuotalog OperatingQuotalog = new OperatingQuotalog();
            OperatingQuotaObj OperatingQuotaObj = new OperatingQuotaObj();
            double ValidateTransaction;
            int ValidateTransactionLog;
            int IndiviualLenght;

            try
            {
                List<OperatingQuotaIndividual> listOperatingQuotaIndividual = new List<OperatingQuotaIndividual>();
                listOperatingQuotaIndividual = GetIndividualOperatingQuota(operatingQuota.codTipoId, operatingQuota.identificacion);
                if (listOperatingQuotaIndividual.Count == 0)
                {
                    response.ResponseId = OperatingQuotaIntResponse.PersonNotExists_int;
                    response.ResponseDescription = OperatingQuotaEnumResponse.PersonNotExists;
                    return response;
                }
                OperatingExchangeRate = GetExchangeRateDate(operatingQuota.fechaCambio, OperatingQuotaIntResponse.currency_Cd_D);

                if (OperatingExchangeRate != 0)
                {
                    OperatingQuotaDollarVal = operatingQuota.cupoOperativo / Convert.ToDouble(OperatingExchangeRate);
                }
                else
                {
                    response.ResponseId = OperatingQuotaIntResponse.DatabaseError_int;
                    response.ResponseDescription = OperatingQuotaEnumResponse.ExchangeTaskError;
                    return response;
                }
                foreach (OperatingQuotaIndividual individual in listOperatingQuotaIndividual)
                {
                    OperatingQuotaObj.IndividualId = individual.Individual_id;
                    OperatingQuotaObj.LineBusinessCd = OperatingQuotaIntResponse.lineBusiness;
                    OperatingQuotaObj.CurrencyCd = OperatingQuotaIntResponse.currency_Cd_D;
                    OperatingQuotaObj.OperatingQuotaAmt = OperatingQuotaDollarVal;
                    OperatingQuotaObj.CurrentTo = operatingQuota.fechaVencimiento.Date;
                    ValidateOldOperatingQuota = ValidateExistOperatingQuota(individual.Individual_id);
                    if (ValidateOldOperatingQuota != OperatingQuotaIntResponse.failint)
                    {
                        try
                        {
                            ValidateTransaction = ExecModifyOperatingQuota(OperatingQuotaObj, OperatingQuotaIntResponse.Sucessfull_int);
                            if (ValidateTransaction == OperatingQuotaIntResponse.Sucessfull_int)
                            {
                                OperatingQuotalog.OldOperatingQuotaAmt = ValidateOldOperatingQuota;
                                OperatingQuotalog.IsWs = OperatingQuotaIntResponse.Sucessfull_int;
                                OperatingQuotalog.TransactionType = OperatingQuotaEnumResponse.update;
                                OperatingQuotalog.TransactionDate = DateTime.Now;
                                ValidateTransactionLog = ExecModifyOperatingQuotalog(OperatingQuotaObj, OperatingQuotalog);

                            }

                        }
                        catch (Exception ex)
                        {
                            response.ResponseId = OperatingQuotaIntResponse.DatabaseError_int;
                            response.ResponseDescription = Convert.ToString(ex);
                            return response;
                        }

                    }
                    else
                    {
                        try
                        {
                            ValidateTransaction = ExecModifyOperatingQuota(OperatingQuotaObj, OperatingQuotaIntResponse.failureint);
                            if (ValidateTransaction == 1)
                            {
                                OperatingQuotalog.OldOperatingQuotaAmt = OperatingQuotaIntResponse.failureint;
                                OperatingQuotalog.IsWs = OperatingQuotaIntResponse.Sucessfull_int;
                                OperatingQuotalog.TransactionType = OperatingQuotaEnumResponse.insert;
                                OperatingQuotalog.TransactionDate = DateTime.Now;
                                ValidateTransactionLog = ExecModifyOperatingQuotalog(OperatingQuotaObj, OperatingQuotalog);

                            }
                        }
                        catch (Exception ex)
                        {
                            response.ResponseId = OperatingQuotaIntResponse.DatabaseError_int;
                            response.ResponseDescription = Convert.ToString(ex);
                            return response;
                        }

                    }
                    OperatingQuotaObj.CurrencyCd = OperatingQuotaIntResponse.currency_Cd_P;
                    OperatingQuotaObj.OperatingQuotaAmt = operatingQuota.cupoOperativo;
                    ValidateOldOperatingQuota = ValidateExistOperatingQuota(individual.Individual_id);
                    if (ValidateOldOperatingQuota != OperatingQuotaIntResponse.failint)
                    {
                        try
                        {
                            ValidateTransaction = ExecModifyOperatingQuota(OperatingQuotaObj, OperatingQuotaIntResponse.Sucessfull_int);
                            if (ValidateTransaction == 1)
                            {
                                OperatingQuotalog.OldOperatingQuotaAmt = ValidateOldOperatingQuota;
                                OperatingQuotalog.IsWs = OperatingQuotaIntResponse.Sucessfull_int;
                                OperatingQuotalog.TransactionType = OperatingQuotaEnumResponse.update;
                                OperatingQuotalog.TransactionDate = DateTime.Now;
                                ValidateTransactionLog = ExecModifyOperatingQuotalog(OperatingQuotaObj, OperatingQuotalog);

                            }
                        }
                        catch (Exception ex)
                        {
                            response.ResponseId = OperatingQuotaIntResponse.DatabaseError_int;
                            response.ResponseDescription = Convert.ToString(ex);
                            return response;
                        }

                    }
                    else
                    {
                        try
                        {
                            ValidateTransaction = ExecModifyOperatingQuota(OperatingQuotaObj, OperatingQuotaIntResponse.failureint);
                            OperatingQuotalog.OldOperatingQuotaAmt = OperatingQuotaIntResponse.failureint;
                            OperatingQuotalog.IsWs = OperatingQuotaIntResponse.Sucessfull_int;
                            OperatingQuotalog.TransactionType = OperatingQuotaEnumResponse.insert;
                            OperatingQuotalog.TransactionDate = DateTime.Now;
                            ValidateTransactionLog = ExecModifyOperatingQuotalog(OperatingQuotaObj, OperatingQuotalog);
                        }
                        catch (Exception ex)
                        {
                            response.ResponseId = OperatingQuotaIntResponse.DatabaseError_int;
                            response.ResponseDescription = Convert.ToString(ex);
                            return response;
                        }
                    }

                    IndividualString = IndividualString + Convert.ToString(individual.Individual_id) + OperatingQuotaEnumResponse.coma;
                }
                IndiviualLenght = IndividualString.Length;
                IndividualString = IndividualString.Substring(0, (IndiviualLenght - 1));
                response.ResponseId = OperatingQuotaIntResponse.Sucessfull_int;
                response.ResponseDescription = IndividualString;
                return response;
            }
            catch (Exception ex)
            {
                response.ResponseId = OperatingQuotaIntResponse.DatabaseError_int;
                response.ResponseDescription = Convert.ToString(ex);
                return response;
            }
        }
    }
}
