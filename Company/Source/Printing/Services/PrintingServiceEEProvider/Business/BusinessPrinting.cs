using Sistran.Company.Application.PrintingServices.Enums;
using System;
using System.Text.RegularExpressions;

namespace Sistran.Company.Application.PrintingServicesEEProvider.Business
{
    public class BusinessPrinting
    {
        #region Barcode
        /// <summary>
        /// Crea el código de barras a partir del número de póliza y IdPv dados.
        /// </summary>
        /// <param name="idPV">Id PV</param>
        /// <param name="strDocument">Documento del tomador</param>
        /// <returns>Código de barras</returns>

        public string[] GetBarCode128(string strDocument, string idPV)
        {
            string strBarcode = string.Empty;
            string strBarcodeA = string.Empty;
            string[] resultBarCode = new string[2];
            
            //strDocument = companyVehicle.CompanyRisk.CompanyInsured.IdentificationDocument.Number;

            strBarcode = "(";
       //     strBarcode += ((int)BarCode.CONSTANT).ToString();
            strBarcode += ")";
          //  strBarcode += ((int)BarCode.COUNTRY_CD).ToString();
          //  strBarcode += ((int)BarCode.COMPANY_CD).ToString();
          //  strBarcode += ((int)BarCode.SERVICE_CD).ToString().PadLeft(3, '0');
          //  strBarcode += ((int)BarCode.CONVENTION).ToString();
            strBarcode += "(";
         //   strBarcode += ((int)BarCode.FIELD_ID).ToString();
            strBarcode += ")";
            strBarcode += BuildValueIdPv(idPV);
            strBarcode += "(";
         //   strBarcode += ((int)BarCode.FIELD_ID).ToString();
            strBarcode += ")";
            strBarcodeA = strBarcode;
            strBarcode += strDocument.PadLeft(10, '0');
            resultBarCode[0] = strBarcode;
            strBarcode = strBarcodeA + BuildDocNum(strDocument).PadLeft(10, '0');
            strBarcode = strBarcode.Replace("(", "");
            strBarcode = strBarcode.Replace(")", "");
            resultBarCode[1] = UccEan128(strBarcode);
            return resultBarCode;
        }

        /// <summary>
        /// Función que convierte cadena numérica en formato EAN 128
        /// para representar en código de barras
        /// </summary>
        /// <param name="strToEncode">Recibe cadena numérica con datos de endoso y número
        /// id usuario</param>
        /// <returns>Cadena convertida en formato EAN 128</returns>
        private string UccEan128(string strToEncode)
        {
            string charSet = "";
            int i = 0;
            string charToEncode = "";
            strToEncode = AsciiToChar(strToEncode);
            strToEncode = strToEncode.ToUpper();

            if (strToEncode.Substring(0, 1) != (Convert.ToChar(247)).ToString())
            {
                strToEncode = (Convert.ToChar(247)).ToString() + strToEncode;
            }

            charSet = strToEncode.Substring(0, 1);
            for (i = 1; i < strToEncode.Length; i++)
            {
                charToEncode = strToEncode.Substring(i, 1);
                if ((Convert.ToInt32(charToEncode[0]) >= 48 && Convert.ToInt32(charToEncode[0]) <= 57) ||
                    (Convert.ToInt32(charToEncode[0]) >= 65 && Convert.ToInt32(charToEncode[0]) <= 90) ||
                    (charToEncode == (Convert.ToChar(247)).ToString()))
                {
                    charSet = charSet + charToEncode;
                }
            }
            //codeBarstr = Code128Auto(charSet);
            return Code128Auto(charSet);
        }

        /// <summary>
        /// Función que convierte la cadena numérica a ascii
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns>Cadena caracteres ascii</returns>
        private string AsciiToChar(string strInput)
        {
            string tempAscii2Char = "";
            int i = 0;
            string strTemp = "";
            int nPos = 0;
            int nValue = 0;

            i = 1;
            nPos = (strInput.ToUpper().IndexOf(("&#").ToUpper(), i - 1) + 1);
            while (nPos > 0)
            {
                tempAscii2Char = tempAscii2Char + strInput.Substring(0, nPos - 1);

                strInput = strInput.Substring(strInput.Length - (strInput.Length - nPos + 1));
                i = 3;
                strTemp = "";
                while ((i <= strInput.Length) && (strTemp.Length < 3))
                {
                    strTemp = strTemp + strInput.Substring(i - 1, 1);
                    i = i + 1;
                }
                nValue = 0;
                if (strTemp != "")
                {
                    nValue = Convert.ToInt32(strTemp);
                }
                if (nValue >= 0 && nValue < 128)
                {
                    tempAscii2Char = tempAscii2Char + (Convert.ToChar(nValue)).ToString();
                }
                else if (nValue > 127 & nValue < 256)
                {
                    tempAscii2Char = tempAscii2Char + (Convert.ToChar(nValue)).ToString();
                }
                else
                {
                    tempAscii2Char = tempAscii2Char + strInput.Substring(0, i - 1);
                }
                if (i <= strInput.Length && strInput.Substring(i - 1, 1) == ";")
                {
                    i = i + 1;
                }
                strInput = strInput.Substring(strInput.Length - (strInput.Length - i + 1));
                nPos = (strInput.ToUpper().IndexOf(("&#").ToUpper(), 0) + 1);
            }
            if (strInput.Length > 0)
            {
                tempAscii2Char = tempAscii2Char + strInput;
            }
            return tempAscii2Char;
        }

        /// <summary>
        /// Genera cadena de caracteres válidos para conversión
        /// </summary>
        /// <returns>Cadena de caracteres código 128</returns>
        private string Code128cCharset()
        {
            string functionReturnValue = null;
            int i = 0;
            for (i = 0; i <= 9; i++)
            {
                functionReturnValue = functionReturnValue + Convert.ToChar(i + (int)(Convert.ToChar(48)));
            }
            for (i = 245; i <= 247; i++)
            {
                functionReturnValue = functionReturnValue + Convert.ToChar(i);
            }
            return functionReturnValue;
        }

        /// <summary>
        /// Convierte cadena de entrada en formato 128 para generar código de barras
        /// </summary>
        /// <param name="strToEncode"></param>
        /// <returns>Cadena codificada 128</returns>
        private string Code128Auto(string strToEncode)
        {
            int i = 0;
            string charToEncode = null;
            int charPos = 0;
            int checkSum = 0;
            string checkDigit = null;
            string AcharSet = null;
            string BcharSet = null;
            string CcharSet = null;
            string mappingSet = null;
            string curCharSet = null;
            string dataCode128 = "";
            int iFunction1 = 0;
            int strLen = 0;
            int charVal = 0;
            int weight = 0;

            if (strToEncode == string.Empty)
            {
                return charToEncode;
            }

            CcharSet = Code128cCharset();
            mappingSet = Code128MappingSet();
            strToEncode = AsciiToChar(strToEncode);
            strLen = strToEncode.Length;
            ///
            iFunction1 = strLen - 15;
            ///
            charVal = (int)(Convert.ToChar(strToEncode.Substring(0, 1)));
            if (charVal <= 31) curCharSet = AcharSet;
            if (charVal >= 32 & charVal <= 126) curCharSet = BcharSet;
            if (charVal == 242) curCharSet = BcharSet;
            if (charVal == 247) curCharSet = CcharSet;
            if (strLen > 4) curCharSet = CcharSet;

            dataCode128 = dataCode128 + Convert.ToChar(250);

            for (i = 0; i < strLen; i++)
            {
                charToEncode = strToEncode.Substring(i, 1);
                charVal = (int)(Convert.ToChar(charToEncode));

                if ((charVal == 242))
                {
                    if (curCharSet == CcharSet)
                    {
                        dataCode128 = dataCode128 + Convert.ToChar(249);
                        curCharSet = BcharSet;
                    }
                    dataCode128 = dataCode128 + Convert.ToChar(242);
                    i = i + 1;
                    charToEncode = strToEncode.Substring(i, 1);
                    charVal = (int)(Convert.ToChar(charToEncode));
                }

                if ((charVal == 247))
                {
                    dataCode128 = dataCode128 + Convert.ToChar(247);
                }
                else if ((i < strLen - 2) || ((i < strLen) && (curCharSet == CcharSet)))
                {
                    if (curCharSet != CcharSet)
                    {
                        dataCode128 = dataCode128 + Convert.ToChar(244);
                        curCharSet = CcharSet;
                    }
                    charToEncode = strToEncode.Substring(i, 2);
                    charVal = Convert.ToInt32(charToEncode);
                    dataCode128 = dataCode128 + mappingSet.Substring(charVal, 1);
                    ///Estas instrucciones se utilizan para incluir
                    ///el caracter de función igual al generado al inicio de la trama
                    ///que permite la lectura correcta del código de barras
                    if (i == iFunction1 - 1)
                    {
                        dataCode128 = dataCode128 + Convert.ToChar(247);
                    }
                    ///****************************************************************
                    i = i + 1;
                }
                else if ((((i <= strLen) & (charVal < 31)) | ((curCharSet == AcharSet) & (charVal > 32 & charVal < 96))))
                {
                    if (curCharSet != AcharSet)
                    {
                        dataCode128 = dataCode128 + Convert.ToChar(246);
                        curCharSet = AcharSet;
                    }
                    charPos = curCharSet.IndexOf(charToEncode);
                    dataCode128 = dataCode128 + mappingSet.Substring(charPos, 1);
                }
                else if ((i <= strLen) & (charVal > 31 & charVal < 127))
                {
                    if (curCharSet != BcharSet)
                    {
                        dataCode128 = dataCode128 + Convert.ToChar(245);
                        curCharSet = BcharSet;
                    }
                    charPos = curCharSet.IndexOf(charToEncode);
                    dataCode128 = dataCode128 + mappingSet.Substring(charPos, 1);
                }
            }

            strLen = dataCode128.Length;
            for (i = 0; i < strLen; i++)
            {
                charVal = (int)(Convert.ToChar(dataCode128.Substring(i, 1)));
                if (charVal == 252)
                {
                    charVal = 0;
                }
                else if (charVal <= 126)
                {
                    charVal = charVal - 32;
                }
                else if (charVal >= 240)
                {
                    charVal = charVal - 145;
                }
                if (i > 1)
                {
                    weight = i;
                }
                else
                {
                    weight = 1;
                }
                checkSum = checkSum + charVal * weight;
            }
            checkSum = checkSum % 103;
            checkDigit = mappingSet.Substring(checkSum, 1);
            dataCode128 = dataCode128 + checkDigit + Convert.ToChar(251);
            return dataCode128;
        }

        /// <summary>
        /// Cadena de caracteres 128
        /// </summary>
        /// <returns></returns>
        private string Code128MappingSet()
        {
            string tempcode128MappingSet = null;
            int i = 0;
            tempcode128MappingSet = Convert.ToChar(252).ToString();
            for (i = 33; i <= 126; i++)
            {
                tempcode128MappingSet = tempcode128MappingSet + ((char)(i)).ToString();
            }
            for (i = 240; i <= 251; i++)
            {
                tempcode128MappingSet = tempcode128MappingSet + ((char)(i)).ToString();
            }
            return tempcode128MappingSet;
        }

        #endregion Barcode

        #region BuildPropertiesBarcode
        /// <summary>
        /// Método BuildValueIdPv:
        /// - Número reservado por el servicio en SISE2G para la emisión del endoso, 
        /// - si el número es menor a 10 caracteres se deben completar con 0 (ceros) a la izquierda
        /// </summary>
        /// <param name="id_Pv2g"></param>
        /// <returns>id_Pv2g</returns>
        private string BuildValueIdPv(string id_Pv2g)
        {
            if (!string.IsNullOrEmpty(id_Pv2g))
            {
                if (id_Pv2g.ToString().Length < 10)
                {
                    return id_Pv2g.PadLeft(10, '0');
                }
                else
                {
                    return id_Pv2g;
                }
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// Método BuildDocNum:
        /// - Si el valor es menor a 10 caracteres se deben complentar con 0 (ceros) a la izquierda
        /// - Si el Número de documento posee caracteres no numericos estos deben ser eliminados de la cadena
        /// </summary>
        /// <param name="documentNum"></param>
        /// <returns>documentNum</returns>
        private string BuildDocNum(string documentNum)
        {
            return Regex.Replace(documentNum, "([^0-9]|-)", ""); //"([^0-9]|[^a-zA-Z]|-)"
        }
        #endregion BuildPropertiesBarcode
    }
}
