using Sistran.Company.Application.PrintingServices.Models;
using Sistran.Company.PrintingService.JetForm.Clases;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.PrintingServices.Clases
{
    public class Paths
    {
        private int _pathId;
        private string _filePath;

        public int PathId
        {
            get { return _pathId; }
            set { _pathId = value; }
        }

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        /// <summary>
        /// Constructor 1
        /// </summary>
        public Paths()
        {
            PathId = 0;
            FilePath = string.Empty;
        }

        /// <summary>
        /// Constructor 2
        /// </summary>
        /// <param name="id">Id de la nueva ruta de archivo</param>
        public Paths(int id)
        {
            PathId = id;
            FilePath = string.Empty;
        }

        /// <summary>
        /// Extrae el nombre del archivo de la ruta dada
        /// </summary>
        /// <param name="path">Ruta del archivo que contiene el nombre</param>
        /// <returns>Nombre del archivo</returns>
        public string getFileName(string path)
        {
            string[] fileName = path.Split('\\');
            return fileName[fileName.Length - 1];
        }

        /// <summary>
        /// Extrae la ruta del archivo. Elimina el nombre en la misma.
        /// </summary>
        /// <param name="path">Ruta del archivo que contiene el nombre</param>
        /// <returns>Ruta donde se almacenan los archivos relacionados</returns>
        public string getFilePath(string path)
        {
            string filePath = path.Replace(getFileName(path), "");
            return filePath;
        }

        /// <summary>
        /// Devuelve el nombre del archivo según los datos de la póliza
        /// </summary>
        /// <param name="data">objeto de tipo Policy que contiene los datos de la póliza</param>
        /// <param name="fileExtension">Estensión del archivo</param>
        /// <returns>Cadena con el nombre del archivo</returns>
        public string setFileName(Policy data, string fileExtension)
        {
            string userRptPath = setPath(data);

            string fileName = Convert.ToString(data.ProcessId) +
                              Convert.ToString(data.PolicyId) +
                              Convert.ToString(data.EndorsementId) +
                              Convert.ToString(PathId)
                              + fileExtension;

            FilePath = userRptPath + fileName;
            return FilePath;
        }

        /// <summary>
        /// Devuelve la ruta de almacenamiento del archivo
        /// </summary>
        /// <param name="data">objeto de tipo Policy que contiene los datos de la póliza</param>
        /// <returns>Ruta</returns>
        public string setPath(Policy data)
        {
            string userRptPath = ConfigurationSettings.AppSettings["ReportExportPath"];

            ReportServiceHelper.validateDirectory(userRptPath);
            return userRptPath;
        }

        /// <summary>
        /// Devuelve la ruta de la plantilla del reporte que se va a imprimir.
        /// </summary>
        /// <param name="type">Tipo del reporte</param>
        /// <param name="multiRisk">Indicador de póliza multiRiesgo</param>
        /// <returns></returns>
        public string getPath(string type, bool multiRisk)
        {
            string reports = ConfigurationSettings.AppSettings["ReportsPath"];
            string collective = reports + ConfigurationSettings.AppSettings["CollectivePolicyReportPath"];
            string individual = reports + ConfigurationSettings.AppSettings["IndividualPolicyReportPath"] + ConfigurationSettings.AppSettings["ReportVersionPath"];
            string quotation = reports + ConfigurationSettings.AppSettings["QuotationReportPath"];
            string migrated = reports + ConfigurationSettings.AppSettings["MigratedPoliciesReportPath"];
            string request = reports + ConfigurationSettings.AppSettings["RequestReportPath"];

            switch (type)
            {
                case "CoverVehicleCover":
                    if (multiRisk)
                    {
                        return collective + ConfigurationSettings.AppSettings["CoverVehicleCoverFile"];
                    }
                    else
                    {
                        return individual + ConfigurationSettings.AppSettings["CoverVehicleCoverFile"];
                    }
                case "CoverVehicleCoverAppendix":
                    if (multiRisk)
                    {
                        return collective + ConfigurationSettings.AppSettings["CoverVehicleCoverAppendixFile"];
                    }
                    else
                    {
                        return individual + ConfigurationSettings.AppSettings["CoverVehicleCoverAppendixFile"];
                    }
                case "VehicleCover":
                    if (multiRisk)
                    {
                        return collective + ConfigurationSettings.AppSettings["VehicleCoverFile"];
                    }
                    else
                    {
                        return individual + ConfigurationSettings.AppSettings["VehicleCoverFile"];
                    }
                case "VehicleCoverAppendix":
                    if (multiRisk)
                    {
                        return collective + ConfigurationSettings.AppSettings["VehicleCoverAppendixFile"];
                    }
                    else
                    {
                        return individual + ConfigurationSettings.AppSettings["VehicleCoverAppendixFile"];
                    }
                case "VehicleConvection":
                    if (multiRisk)
                    {
                        return collective + ConfigurationSettings.AppSettings["VehicleConvectionFile"];
                    }
                    else
                    {
                        return individual + ConfigurationSettings.AppSettings["VehicleConvectionFile"];
                    }
                case "CropCover":
                    return individual + ConfigurationSettings.AppSettings["CropCoverFile"];
                case "CropConvection":
                    return individual + ConfigurationSettings.AppSettings["CropConvectionFile"];
                case "CropCoverAppendix":
                    return individual + ConfigurationSettings.AppSettings["CropCoverAppendixFile"];
                case "SuretyCover":
                    return individual + ConfigurationSettings.AppSettings["SuretyCoverFile"];
                case "SuretyCoverAppendix":
                    return individual + ConfigurationSettings.AppSettings["SuretyCoverAppendixFile"];
                case "SuretyConvection":
                    return individual + ConfigurationSettings.AppSettings["SuretyConvectionFile"];
                case "FormatCollect":
                    return individual + ConfigurationSettings.AppSettings["FormatCollectFile"];
                case "VehicleQuotationCover":
                    return quotation + ConfigurationSettings.AppSettings["VehicleQuotationCoverFile"];
                case "VehicleQuotationAppendix":
                    return quotation + ConfigurationSettings.AppSettings["VehicleQuotationAppendixFile"];
                case "SuretyQuotationCover":
                    return quotation + ConfigurationSettings.AppSettings["SuretyQuotationCoverFile"];
                case "CropQuotationCover":
                    return quotation + ConfigurationSettings.AppSettings["CropQuotationCoverFile"];
                case "RequestConvection":
                    return request + ConfigurationSettings.AppSettings["RequestConvectionFile"];
                case "RequestCover":
                    return request + ConfigurationSettings.AppSettings["RequestCoverFile"];
                case "MigratedPoliciesList":
                    return migrated + ConfigurationSettings.AppSettings["MigratedPoliciesListFile"];
                default:
                    break;
            }
            return string.Empty;
        }
    }
}
