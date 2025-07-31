using System;
using System.Configuration;
using System.IO;

namespace Sistran.Core.Application.Utilities.Managers
{
    public class LogManager
    {
        public static void CreateLog(string nameLog, string reportTime)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["ReportLogTime"]))
            {
                string pathFolder = ConfigurationManager.AppSettings["FolderLogTime"];

                if (!Directory.Exists(Path.GetDirectoryName(pathFolder)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(pathFolder));
                }

                pathFolder += nameLog + ".txt";

                FileStream fileStream = File.Create(pathFolder);

                StreamWriter streamWriter = new StreamWriter(fileStream);
                string[] lines = reportTime.Split(';');

                for (int i = 0; i < lines.Length; i++)
                {
                    streamWriter.WriteLine(lines[i]);
                }

                streamWriter.Close();
            }
        }
    }
}