using Sistran.Company.Application.PrintingServices.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.PrintingServices.Clases
{
    public class JetFormPrinterHelper
    {
        public JetFormPrinterHelper()
        {
        }

        private static List<DatRecord> records;

        public static void initializeList()
        {
            records = new List<DatRecord>();

        }

        public static void AddNewRecord(string fieldName, object fieldValue)
        {
            DatRecord obj = new DatRecord();
            obj.FieldName = fieldName;
            obj.FieldValue = fieldValue;

            records.Add(obj);
        }

        public static void WriteFile(string fileName, string filePath)
        {
            System.IO.StreamWriter writer = new StreamWriter(filePath + fileName);
            foreach (DatRecord obj in records)
            {
                if (obj.FieldName != string.Empty)
                {
                    writer.WriteLine(obj.FieldName);
                }
                if (obj.FieldValue != null)
                {
                    writer.WriteLine(obj.FieldValue);
                }


            }

            writer.Close();

        }


        public static void PrintHeader(DataSet ds)
        {
            string value = "SEGURO " + ds.Tables[0].Rows[0]["BRANCH"].ToString() + " POLIZA " + ds.Tables[0].Rows[0]["POLICY_TYPE"].ToString();
            AddNewRecord("^field seguro", value);
            AddNewRecord("^field titulo_no_poliza", null);
            AddNewRecord("PÓLIZA N°", null);
            AddNewRecord("^field no_poliza", ds.Tables[0].Rows[0]["DOCUMENT_NUM"].ToString());
            AddNewRecord("^field certificado_de", ds.Tables[0].Rows[0]["ENDORSEMENT_TYPE"].ToString());
            AddNewRecord("^field n_transaccion", ds.Tables[0].Rows[0]["TRANSACTION_ID"].ToString());
            AddNewRecord("^field poliza_lider", ds.Tables[0].Rows[0]["LEAD_POLICY_ID"].ToString());
            AddNewRecord("^field certificado_lider", ds.Tables[0].Rows[0]["LEAD_DOCUMENT_NUM"].ToString());


        }

        public static void PrintTitle(string fileName, string printerName, string branchCode)
        {
            AddNewRecord("^job " + printerName + " -ass5", null);
            AddNewRecord("^form " + fileName, null);
            AddNewRecord("^reformat no", null);
            AddNewRecord("^field codigo_ramo", branchCode);
        }

        public static void PrintPolicyHolderData(Policy obj)
        {
            AddNewRecord("^field firma_tomador", "EL TOMADOR");
            AddNewRecord("^field tomador_titulo", "TOMADOR");
            //AddNewRecord("^field tomador", obj.PolicyHolderId);//Pendinete de verificar este atributo de la clase
            AddNewRecord("^field direccion_titulo", "DIRECCIÓN");
            AddNewRecord("^field direccion", "HOLDER_ADDRESS");
            AddNewRecord("^field telefono_titulo", "TELÉFONO");
            AddNewRecord("^field tel_tomador", "HOLDER_PHONE");
            AddNewRecord("^field nit_titulo", "NIT");
            AddNewRecord("^field nit_tomador", "TRIBUTARY_ID");
        }

        public static void PrintIssuanceData(DataSet ds)
        {
            AddNewRecord("^field emitido", ds.Tables[0].Rows[0]["POLICY_CITY"]);
            AddNewRecord("^field moneda", ds.Tables[0].Rows[0]["CURRENCY"]);
            AddNewRecord("^field tipo_cambio", ds.Tables[0].Rows[0]["EXCHANGE_CD"]);
            AddNewRecord("^field sucursal", ds.Tables[0].Rows[0]["PREFIX"]);
            AddNewRecord("^field centro_oper", "PREFIX_ID");
            AddNewRecord("^field apropiacion_pres", "APPROPIATE");
            AddNewRecord("^field cargar_a", ds.Tables[0].Rows[0]["LOAD_TO"]);
            AddNewRecord("^field FECHA_EMISION", ds.Tables[0].Rows[0]["ISSUANCE_DATE"]);
            AddNewRecord("^field dia_desde", ds.Tables[0].Rows[0]["DAY_FROM"]);
            AddNewRecord("^field mes_desde", ds.Tables[0].Rows[0]["MONTH_FROM"]);
            AddNewRecord("^field anio_desde", ds.Tables[0].Rows[0]["YEAR _FROM"]);
            AddNewRecord("^field hora_desde", ds.Tables[0].Rows[0]["HOUR_FROM"]);
            AddNewRecord("^field dia_hasta", ds.Tables[0].Rows[0]["DAY_TO"]);
            AddNewRecord("^field mes_hasta", ds.Tables[0].Rows[0]["MONTH_TO"]);
            AddNewRecord("^field anio_hasta", ds.Tables[0].Rows[0]["YEAR_TO"]);
            AddNewRecord("^field hora_hasta", ds.Tables[0].Rows[0]["HOUR_TO"]);
            AddNewRecord("^field no_dias", ds.Tables[0].Rows[0]["DAYS"]);



        }

        public static void PrintCommissionsAndCoinsuranceData(DataSet ds)
        {
            AddNewRecord("^field ACUERDO_DE_PAGO", ds.Tables[0].Rows[0]["PAYMENT_METHOD"]);
            AddNewRecord("^field valor_asegurado", ("$" + ds.Tables[0].Rows[0]["INSURED_AMOUNT"]));

            //TODO: RECORRER LISTA DE INTERMEDIARIOS Y ESCRIBIR TODOS LOS DATOS
            AddNewRecord("^field intermediarios", null);
            //foreach()
            AddNewRecord(string.Empty, ds.Tables[0].Rows[0]["INTERMEDIARY_NAME"]);

            AddNewRecord("^field prima_pesos", null);
            //foreach()
            AddNewRecord(string.Empty, ds.Tables[0].Rows[0]["PREMIUM AMOUNT"]);

            AddNewRecord("^field iva_pesos", null);
            //foreach()
            AddNewRecord(string.Empty, ds.Tables[0].Rows[0]["TAX_AMOUNT"]);

            AddNewRecord("^field gastos_pesos", null);
            //foreach()n
            AddNewRecord(string.Empty, ds.Tables[0].Rows[0]["EXPENSES_AMOUNT"]);

            AddNewRecord("^field total_valor_pesos", null);
            //foreach()
            AddNewRecord(string.Empty, ds.Tables[0].Rows[0]["TOTAL"]);

            AddNewRecord("^field ajustes_al_peso", null);
            //foreach()
            AddNewRecord(string.Empty, ds.Tables[0].Rows[0]["ADJUST"]);

            //TODO: POSIBLEMENTE RECORRER DATOS DE COASEGUROS
            AddNewRecord("^field distri_coaseguro", null);

            AddNewRecord("^field FECHA_EMISION", ds.Tables[0].Rows[0]["ISSUANCE_DATE"]);

        }

        /// <summary>
        /// OJO Pendiente verificar la clase Insured
        /// </summary>
        /// <param name="ds"></param>
        //public static void PrintInsuredData(Insured obj)
        //{
        //    AddNewRecord("^field ASEGURADO_TITULO", "ASEGURADO");
        //    AddNewRecord("^field NOMBRE_ASEGURADO", obj.IndividualId);
        //    AddNewRecord("^field DIRECCION_TIT_ASEG", "DIRECCIÓN");
        //    AddNewRecord("^field DIRECCION_ASEGURADO", obj.IndividualId/*address*/);
        //    AddNewRecord("^field NIT_TITULO_ASEG", "NIT");
        //    AddNewRecord("^field NIT_ASEGURADO", obj.IndividualId/*tributary_id*/);
        //    AddNewRecord("^field TEL_TITULO_ASEG", "TELÉFONO");
        //    AddNewRecord("^field TEL_ASEGURADO", obj.IndividualId/*phone*/);
        //}

        public static void PrintRiskVehicleData(DataSet ds)
        {
            int counter = 0;
            AddNewRecord("^field AMPAROS", null);

            //TODO: RECORRER DATOS DE RIESGOS

            //foreach()
            AddNewRecord("DESCRIPCION DEL VEHICULO No. :" + counter, null);
            AddNewRecord("Codigo Fasecolda: " + ds.Tables[0].Rows[0]["FASECOLDA_CODE"], null);
            AddNewRecord("Marca: " + ds.Tables[0].Rows[0]["MAKE"] + "     Modelo: " + ds.Tables[0].Rows[0]["MODEL"] + "     Color: " + ds.Tables[0].Rows[0]["MODEL"], null);
            AddNewRecord("Estilo: " + ds.Tables[0].Rows[0]["REFERENCE"] + "     Tipo: " + ds.Tables[0].Rows[0]["TYPE"] + "     Servicio: " + ds.Tables[0].Rows[0]["SERVICE_TYPE"], null);
            AddNewRecord("Placas: " + ds.Tables[0].Rows[0]["LICENSE_PLATE"] + "     Motor No.: " + ds.Tables[0].Rows[0]["ENGINE_SERIAL"] + "     Chasis No.: " + ds.Tables[0].Rows[0]["CHASSIS_SERIAL"], null);

            AddNewRecord("AMPAROS CONTRATADOS", null);
            AddNewRecord("No Amparo                                 Valor Asegurado  Deducible ", null);
            //TODO: DENTRO DEL RECORRIDO DE RIESGOS DE VEHÍCULOS REALIZAR UN FOREACH ANIDADO PARA ESCRIBIR LOS DATOS DE LAS COBERTURAS
            //      LOS DATOS DE LAS COBERTURAS INCLUYEN DETALLES Y DEDUCIBLES
            //foreach()
            AddNewRecord(ds.Tables[0].Rows[0]["COVERAGE"] + " " + ds.Tables[0].Rows[0]["AMOUNT"] + " " + ds.Tables[0].Rows[0]["DEDUCT"], null);

        }
    }
}
