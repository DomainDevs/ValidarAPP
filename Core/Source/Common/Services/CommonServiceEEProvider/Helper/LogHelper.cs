using System;
using System.Configuration;
using System.IO;

namespace Sistran.Core.Application.CommonService.Helper
{
    public class LogHelper
    {
        /// <summary>
        /// Crear Log
        /// </summary>
        /// <param name="methodName">Nombre Metodo</param>
        /// <param name="textLog">Log</param>
        public static void CreateLog(string methodName, string textLog)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["ReportLogTime"]))
            {
                string pathFolder = ConfigurationManager.AppSettings["FolderLogTime"];

                if (!Directory.Exists(Path.GetDirectoryName(pathFolder)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(pathFolder));
                }

                pathFolder += Guid.NewGuid() + ".txt";

                FileStream fileStream = File.Create(pathFolder);

                StreamWriter streamWriter = new StreamWriter(fileStream);

                streamWriter.WriteLine(methodName);
                streamWriter.WriteLine(textLog);

                streamWriter.Close();
            }
        }
    }
}