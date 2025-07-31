using Sistran.Company.Application.CollectionFormBusinessService;
using Sistran.Company.Application.CollectionFormBusinessService.Models;
using System;
using System.Configuration;
using System.Data;
using System.Collections;
using System.IO;
using Sistran.Company.Application.CollectionFormBusinessServiceProvider.Clases;
using Sistran.Co.Application.Data;
using System.Globalization;
using Sistran.Company.Application.CollectionFormBusinessServiceProvider.Dao;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Sistran.Company.Application.CollectionFormBusinessServiceProvider
{
    public class CompanyCollectionFormBusinessServiceProvider : ICompanyCollectionFormBusinessService
    {
        public CultureInfo co = new CultureInfo("es-CO");
        /// <summary>
        /// Metodo que genera el reporte y con ello el formato en pdf
        /// </summary>
        /// <param name="collectionForm">Datos del endoso y sus cuotas</param>
        /// <returns>Ruta del archvo en PDF</returns>
        public string[] GenerateCollectionForm(CollectionForm collectionForm)
        {
            String[] paths = new String[2];
            try
            {
                String pathReporte = ConfigurationManager.AppSettings["FormReport"].ToString();
                String pathPdf = ConfigurationManager.AppSettings["pathPdf"].ToString();
                String pathBarCode = ConfigurationManager.AppSettings["pathBarCode"].ToString();
                String pathHttp = ConfigurationManager.AppSettings["pathHttp"].ToString();
                String pathBarCodeImage = String.Empty;
                String namepdf = String.Empty;
                String newDocumentName = String.Empty;
                String pathNavigate = String.Empty;

                if (collectionForm.Policy.DocumentNumber == collectionForm.Policy.EndorsementId)
                {
                    //llama accion para consultar y guardar datos de reporte
                    ArrayList objectdata = GetDataReport(collectionForm);
                    Int32 quotesNumber = collectionForm.Policy.Endorsements[0].Payers.Count;
                    String[] generatedImagesBarCode = new String[quotesNumber];

                    DataSet dataset = new DataSet();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("refNumber", typeof(String));
                    dt.Columns.Add("payerName", typeof(String));
                    dt.Columns.Add("branch", typeof(Int32));
                    dt.Columns.Add("prefix", typeof(Int32));
                    dt.Columns.Add("quoteNumber", typeof(Int32));
                    dt.Columns.Add("policyEndorsement", typeof(String));
                    dt.Columns.Add("payValue", typeof(String));
                    dt.Columns.Add("dateLimitDay", typeof(String));
                    dt.Columns.Add("dateLimitMonth", typeof(String));
                    dt.Columns.Add("dateLimitYear", typeof(String));
                    dt.Columns.Add("paymentCode", typeof(String));
                    dt.Columns.Add("textSpace1", typeof(String));
                    dt.Columns.Add("textSpace2", typeof(String));
                    dt.Columns.Add("barCodeData", typeof(String));
                    dt.Columns.Add("barCodeString", typeof(String));

                    for (int i = 0; i < quotesNumber; i++)
                    {
                        String[] date = Convert.ToString(collectionForm.Policy.Endorsements[0].Payers[i].PaymentSchedule.Installments[0].DueDate, co).Split(' ', '/');

                        String day = (date[0].Length == 1) ? "0" + date[0] : date[0];
                        String month = (date[1].Length == 1) ?  "0" + date[1] : date[1];
                        String year = date[2];
                        String dataBarCode = String.Empty;
                        String fileBarCode = String.Empty;
                        Boolean imageBarCodeGenerated = false;

                        //armado de codigo de barras
                        String pack1 = objectdata[0].ToString() + objectdata[1].ToString();
                        String pack2 = objectdata[2].ToString() + objectdata[8].ToString().PadLeft(12, '0') + objectdata[3].ToString();
                        String pack3 = objectdata[4].ToString() + Convert.ToInt64(collectionForm.Policy.Endorsements[0].Payers[i].PaymentSchedule.Installments[0].PaidAmount.Value).ToString().PadLeft(12, '0');
                        String pack4 = objectdata[5].ToString() + year + month + day;

                        String clientNumber = pack1.Replace("(", string.Empty).Replace(")", string.Empty);
                        String consecutive = pack2.Replace("(", string.Empty).Replace(")", string.Empty);
                        String paymentValue = pack3.Replace("(", string.Empty).Replace(")", string.Empty);
                        String datepayment = pack4.Replace("(", string.Empty).Replace(")", string.Empty);

                        pathBarCodeImage = String.Empty;
                        fileBarCode = collectionForm.Policy.BranchId.ToString() +
                                    collectionForm.Policy.PrefixId.ToString() +
                                    collectionForm.Policy.DocumentNumber.ToString() +
                                    collectionForm.Policy.Endorsements[0].EndorsementNumber.ToString() +
                                    collectionForm.Policy.Endorsements[0].Payers[i].PaymentSchedule.Installments[0].InstallmentNumber.ToString() + ".png";
                        pathBarCodeImage = pathBarCode + fileBarCode;

                        BarcodeGenerator Barcode = new BarcodeGenerator();
                        dataBarCode = clientNumber + consecutive + ";" + paymentValue + ";" + datepayment;

                        // Nueva funcionalidad, conforme a características definidas por GS1
                        imageBarCodeGenerated = Barcode.createBarCodeGS1Image(dataBarCode, pathBarCodeImage);

                        DataRow dr = dt.NewRow();
                        dr["refNumber"] = objectdata[8].ToString() + objectdata[3].ToString();
                        dr["payerName"] = collectionForm.Policy.Endorsements[0].Payers[i].Name;
                        dr["branch"] = collectionForm.Policy.BranchId;
                        dr["prefix"] = collectionForm.Policy.PrefixId;
                        dr["quoteNumber"] = collectionForm.Policy.Endorsements[0].Payers[i].PaymentSchedule.Installments[0].InstallmentNumber;
                        dr["policyEndorsement"] = collectionForm.Policy.DocumentNumber + "-" + collectionForm.Policy.Endorsements[0].EndorsementNumber;
                        dr["payValue"] = Convert.ToInt64(collectionForm.Policy.Endorsements[0].Payers[i].PaymentSchedule.Installments[0].PaidAmount.Value).ToString("C0", co);
                        dr["dateLimitDay"] = day;
                        dr["dateLimitMonth"] = month;
                        dr["dateLimitYear"] = year;
                        dr["paymentCode"] = collectionForm.Policy.Endorsements[0].Payers[i].IndividualId.ToString();
                        dr["textSpace1"] = objectdata[6].ToString();
                        dr["textSpace2"] = objectdata[7].ToString();
                        generatedImagesBarCode[i] = pathBarCodeImage;
                        dr["barCodeData"] = pathBarCodeImage;
                        dr["barCodeString"] = pack1 + pack2 + pack3 + pack4;
                        dt.Rows.Add(dr);
                    }

                    dataset.Tables.Add(dt);
                    ReportDocument myReportDocument = new ReportDocument();
                    myReportDocument.Load(pathReporte);
                    myReportDocument.Database.Tables[0].SetDataSource(dataset);
                    namepdf = collectionForm.Policy.BranchId.ToString() + collectionForm.Policy.PrefixId.ToString() + collectionForm.Policy.DocumentNumber.ToString() + collectionForm.Policy.Endorsements[0].EndorsementNumber.ToString();

                    String[] filePaths = Directory.GetFiles(pathPdf, namepdf + "_*.pdf");
                    foreach (string path in filePaths)
                        File.Delete(path);

                    String sDate = String.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now);

                    newDocumentName = pathPdf + namepdf + "_" + sDate + ".pdf";

                    myReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, newDocumentName);
                    myReportDocument.Close();

                    foreach (String path in generatedImagesBarCode)
                        File.Delete(path);

                    pathNavigate = pathHttp + namepdf + "_" + sDate + ".pdf";
                    paths[0] = newDocumentName;
                    paths[1] = pathNavigate;
                }
                else
                {
                    paths[0] = pathPdf;
                    paths[1] = pathHttp;
                }

                // Escritura de evento de impresión de póliza
                if (collectionForm.Policy.userId != 0)
                    addEventPrintPolicy(collectionForm);
            }
            catch (Exception ex)
            {
                paths[0] = "-1";
                paths[1] = ex.Message.ToString();
            }
            return paths;
        }

        /// <summary>
        /// Metodo que busca el id_pv del endoso y los valores parametrizados para el formato de recaudo
        /// </summary>
        /// <param name="collect">Objeto con datos de la poliza</param>
        /// <returns>Arreglo de valores</returns>
        private ArrayList GetDataReport(CollectionForm collect)
        {
            try
            {
                //llamado al sp para id de consecutivo
                ArrayList arr = new ArrayList();
                DataSet da = new DataSet();
                DataSet d2 = new DataSet();

                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    NameValue[] paramsp2 = new NameValue[1];
                    paramsp2[0] = new NameValue("PARAMETER_ID", 0);
                    d2 = pdb.ExecuteSPDataSet("REPORT.PRV_GET_COLLECTIONS_DATA_FORMAT", paramsp2);
                    for (int i = 0; i < d2.Tables[0].Rows.Count; i++)
                    {
                        arr.Add(d2.Tables[0].Rows[i]["TEXT_PARAMETER"].ToString());
                    }
                }

                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    NameValue[] paramsp = new NameValue[4];
                    paramsp[0] = new NameValue("BRANCH", collect.Policy.BranchId);
                    paramsp[1] = new NameValue("PREFIX", collect.Policy.PrefixId);
                    paramsp[2] = new NameValue("DOCNUM", collect.Policy.DocumentNumber);
                    paramsp[3] = new NameValue("ENDOID", collect.Policy.Endorsements[0].EndorsementNumber);
                    da = pdb.ExecuteSPDataSet("REPORT.PRV_SEARCH_COLLECTION_ID", paramsp);
                    arr.Add(Convert.ToInt32(da.Tables[0].Rows[0].ItemArray[0].ToString()));
                } 

                return arr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método que inserta el registro de evento de impresion de póliza, cuando se genera formato de recaudo con la póliza completa
        /// </summary>
        /// <param name="collectionForm">Objeto con datos de la poliza</param>
        private void addEventPrintPolicy(CollectionForm collectionForm)
        {
            try
            {
                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    NameValue[] parameters = new NameValue[5];
                    parameters[0] = new NameValue("BRANCH", collectionForm.Policy.BranchId);
                    parameters[1] = new NameValue("PREFIX", collectionForm.Policy.PrefixId);
                    parameters[2] = new NameValue("DOCNUM", collectionForm.Policy.DocumentNumber);
                    parameters[3] = new NameValue("ENDORS", collectionForm.Policy.Endorsements[0].EndorsementNumber);
                    parameters[4] = new NameValue("USR_ID", collectionForm.Policy.userId);
                    pdb.ExecuteSPDataSet("REPORT.PRV_ADD_PRINT_POLICY_EVENT", parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ReportPolicy GetPolicybyBranchPrefixDocument(int branchId, int prefixId, int documentNumber)
        {
            ReportPolicyDAO reportPolicyDAO = new ReportPolicyDAO();
            
            try
            {
                ReportPolicy reportPolicy = new ReportPolicy();
                reportPolicy = reportPolicyDAO.GetPolicybyBranchPrefixDocument(branchId, prefixId, documentNumber);

                return reportPolicy;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
