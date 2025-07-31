using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sistran.Core.Framework.UIF.Web.Helpers
{
    public static class GenerateFile
    {
        private static string DefinitionHtml()
        {
            StringBuilder strHtml = new StringBuilder();

            strHtml.Append("<!DOCTYPE HTML>");
            strHtml.Append("<html>");
            strHtml.Append("  <head>");
            strHtml.Append("<title>excel test</title>");
            strHtml.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />");
            strHtml.Append("  </head>");
            strHtml.Append("<body>");
            strHtml.Append("<table>");
            strHtml.Append("<thead> <tr>");
            strHtml.Append("{{HEAD}}");
            strHtml.Append("</tr> </thead>");
            strHtml.Append("<tbody>");
            strHtml.Append("{{BODY}}");
            strHtml.Append("</tbody>");
            strHtml.Append("</table>");
            strHtml.Append(" </body>");
            strHtml.Append("</html>");

            return strHtml.ToString();
        }

        public static byte[] WriteExcel(List<object[]> Data, List<string> Headers)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                StreamWriter streamWriter = new StreamWriter(memoryStream);
                StringBuilder stringBuilder = new StringBuilder();
                StringBuilder strHeader = new StringBuilder();
                StringBuilder strData = new StringBuilder();

                stringBuilder.Append(DefinitionHtml());


                foreach (var item in Headers)
                {
                    strHeader.Append(string.Format("<th>{0}</th>", item.ToUpper()));
                }

                foreach (var item in Data)
                {
                    strData.Append("<tr>");
                    foreach (var value in item)
                    {
                        strData.Append(string.Format("<td style='vertical-align:middle;'>{0}</td>", value != null ? value : ""));
                    }
                    strData.Append("</tr>");
                }

                string cadenaExcel = stringBuilder.ToString();

                cadenaExcel = cadenaExcel.Replace("{{HEAD}}", strHeader.ToString());
                cadenaExcel = cadenaExcel.Replace("{{BODY}}", strData.ToString());

                cadenaExcel = cadenaExcel.Replace("True", "Si");
                cadenaExcel = cadenaExcel.Replace("False", "No");

                cadenaExcel = cadenaExcel.Replace("á", "&aacute;");
                cadenaExcel = cadenaExcel.Replace("Á", "&Aacute;");
                cadenaExcel = cadenaExcel.Replace("é", "&eacute;");
                cadenaExcel = cadenaExcel.Replace("É", "&Eacute;");
                cadenaExcel = cadenaExcel.Replace("í", "&iacute;");
                cadenaExcel = cadenaExcel.Replace("Í", "&Iacute;");
                cadenaExcel = cadenaExcel.Replace("ó", "&oacute;");
                cadenaExcel = cadenaExcel.Replace("Ó", "&Oacute;");
                cadenaExcel = cadenaExcel.Replace("ú", "&uacute;");
                cadenaExcel = cadenaExcel.Replace("Ú", "&Uacute;");
                cadenaExcel = cadenaExcel.Replace("ñ", "&ntilde;");
                cadenaExcel = cadenaExcel.Replace("Ñ", "&Ntilde;");

                streamWriter.Write(cadenaExcel);
                streamWriter.Close();

                byte[] archivo = memoryStream.ToArray();
                memoryStream.Dispose();
                streamWriter.Dispose();
                return archivo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}