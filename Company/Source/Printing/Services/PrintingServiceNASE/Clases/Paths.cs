using Sistran.Company.Application.PrintingServicesNase;
using Sistran.Company.Application.PrintingServicesNase.Enum;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Utilities.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UTILHELPER = Sistran.Core.Application.Utilities.Helper;

namespace Sistran.Company.PrintingService.NASE.Clases
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
            string fileName = string.Empty;

            if (data.PolicyId > 0)
            {
                fileName = "P" + Convert.ToString(data.PolicyId + "_" + data.EndorsementId) +"_"+ DateTime.Now.ToString("yyyy_MM_dd-HH-mm-ss")+ Convert.ToString(PathId) + fileExtension;
            }
            else if (data.QuotationId > 0)
            {
                fileName = "C" + Convert.ToString(data.TempNum) + "_" + DateTime.Now.ToString("yyyy_MM_dd-HH-mm-ss")+ Convert.ToString(PathId) + fileExtension;
            }
            else if (data.TempAuthorization)
            {
                fileName = "A" + Convert.ToString(data.TempNum) + "_" + DateTime.Now.ToString("yyyy_MM_dd-HH-mm-ss")+ Convert.ToString(PathId) + fileExtension;
            }
            else 
            {
                fileName = "T" + Convert.ToString(data.TempNum) + "_" + DateTime.Now.ToString("yyyy_MM_dd-HH-mm-ss")+ Convert.ToString(PathId) + fileExtension;
            }
            //string fileName = Convert.ToString(data.ProcessId) +
            //                  Convert.ToString(data.PolicyId) +
            //                  Convert.ToString(data.EndorsementId) +
            //                  Convert.ToString(PathId) +
            //                  DateTime.Now.ToString("dd_MM_yyyy_HH_mm")
            //                  + fileExtension;

            FilePath = userRptPath + fileName;
            return FilePath;
        }


        /// <summary>
        /// Devuelve el nombre del archivo para la impresión de datos sin formato
        /// </summary>
        /// <param name="data">objeto de tipo Policy que contiene los datos de la póliza</param>
        /// <param name="fileExtension">Estensión del archivo</param>
        /// <param name="identifier">Cadena para distinguir el archivo con datos sin formato del archivo normal</param>
        /// <returns>Cadena con el nombre del archivo</returns>
        public string setBlankFileName(Policy data, string fileExtension, int processId, string identifier)
        {
            string userRptPath = setPath(data);

            string fileName = Convert.ToString(processId) +
                              Convert.ToString(data.PolicyId) +
                              Convert.ToString(data.EndorsementId) +
                              Convert.ToString(PathId) + identifier +
                              fileExtension;

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
            Parameter paramDigitalFirm = new Parameter();
            string userRptPath = string.Empty;
            paramDigitalFirm =  DelegateService.commonService.GetParameterByDescription("Enabled_DigitalFirm");
            var printStatus = DelegateService.printingServiceCore.GetLogPrintStatus(data.EndorsementId);

            if (paramDigitalFirm.NumberParameter == 1 && data.ReportType == 1 && printStatus?.StatusId ==2) 
                {
                userRptPath = ConfigurationSettings.AppSettings["ReportExportPath"]+ "NotSigned" + @"\";

                ReportServiceHelper.validateDirectory(userRptPath);
                }
            else if (paramDigitalFirm.NumberParameter == 1 && data.ReportType == 1)
            {
                userRptPath = ConfigurationSettings.AppSettings["ReportExportPath"];

                ReportServiceHelper.validateDirectory(userRptPath);
            }
            else
                {
                userRptPath = ConfigurationSettings.AppSettings["ReportExportPath"] +
                                 data.User + @"\";

                ReportServiceHelper.validateDirectory(userRptPath);
                }
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
            //TODO:  <<Autor: Luisa Fernanda Ramírez; Fecha: 23/12/2010; Asunto: OT-0051 Renovacion de Autos Individuales. Compañía: 1 
            string renewal = reports + ConfigurationSettings.AppSettings["MassiveRenewalPoliciesPath"];
            /* Autor: Luisa Fernanda Ramírez, Fecha: 23/12/2010 >>*/

            switch (type)
            {
                case "CoverVehicleCover":
                    if (multiRisk)
                    {
                        return collective + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_COVER_VEHICLE_COVER_FILE);
                    }
                    else
                    {
                        return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_COVER_VEHICLE_COVER_FILE); 
                    }
                case "CoverVehicleCoverAppendix":
                    if (multiRisk)
                    {
                        return collective + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_COVER_VEHICLE_COVER_APPENDIX_FILE); 
                    }
                    else
                    {
                        return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_COVER_VEHICLE_COVER_APPENDIX_FILE); 
                    }
                case "VehicleCover":
                    if (multiRisk)
                    {
                        return collective + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_VEHICLE_COVER_FILE); 
                    }
                    else
                    {
                        return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_VEHICLE_COVER_FILE); 
                    }
                case "VehicleCoverAppendix":
                    if (multiRisk)
                    {
                        return collective + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_VEHICLE_COVER_APPENDIX_FILE); 
                    }
                    else
                    {
                        return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_VEHICLE_COVER_APPENDIX_FILE); 
                    }
                case "VehicleConvection":
                    if (multiRisk)
                    {
                        return collective + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_VEHICLE_CONVECTION_FILE);
                    }
                    else
                    {
                        return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_VEHICLE_CONVECTION_FILE); 
                    }
                case "CropCover":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_CROP_COVER_FILE);
                case "CropCoverAutho":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_CROP_COVER_AUTHO_FILE);
                case "CropConvection":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_CROP_CONVECTION_FILE); 
                case "CropCoverAppendix":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_CROP_COVER_APPENDIX_FILE); 
                case "SuretyCover":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_SURETY_COVER_FILE);
                case "SuretyCoverAutho":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_SURETY_COVER_AUTHO_FILE);
                case "SuretyCoverAppendix":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_SURETY_COVER_APPENDIX_FILE);
                case "CautionCoverObservationAppendix":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_CAUTION_COVER_OBSERVATION_APPENDIX);
                case "SuretyConvection":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_SURETY_CONVECTION_FILE); 
                case "FormatCollect":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_FORMAT_COLLECT_FILE); 
                case "VehicleQuotationCover":
                    return quotation + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_VEHICLE_QUOTATION_COVER_FILE); 
                case "VehicleQuotationAppendix":
                    return quotation + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_VEHICLE_QUOTATION_APPENDIX_FILE);
                case "SuretyQuotationCover":
                    return quotation + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_SURETY_QUOTATION_COVER_FILE); 
                case "CropQuotationCover":
                    return quotation + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_CROP_QUOTATION_COVER_FILE); 
                case "RequestConvection":
                    return request + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_REQUEST_CONVECTION_FILE); 
                case "RequestCover":
                    return request + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_REQUEST_COVER_FILE); 
                case "MigratedPoliciesList":
                    return migrated + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_MIGRATED_POLICIES_LIST_FILE); 
                case "LocationCover":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_LOCATION_COVER_FILE);
                //TODO: <<Autor: Edgar Cervantes De Los Rios; Fecha: 29/11/2010; Asunto: Al imprimir un temporario del ramo multiriesgo, se genera desbordamiento por la cantidad coberturas.; Compañía: 1.
                case "LocationCoverAppendix":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_LOCATION_COVER_APPENDIX_FILE); 
                // Autor: Edgar Cervantes De Los Rios, Fecha: 29/11/2010 >>//
                //TODO: <<Autor: Edgar O. Piraneque E.; Fecha: 12/01/2011; Asunto: Se crea reporte para imprimir Convenio de pago; Compañía: 1.
                case "LocationConvection":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_LOCATION_CONVECTION_FILE);
                // Autor: Edgar O. Piraneque E.; Fecha: 12/01/2011 >>//
                case "LocationQuotationCover":
                    return quotation + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_LOCATION_QUOTATION_COVER_FILE); 
                case "LocationQuotationAppendix":
                    return quotation + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_LOCATION_QUOTATION_APPENDIX_FILE); 
                case "PassengerCover":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_PASSENGER_COVER_FILE); 
                case "PassengerCoverAppendix":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_PASSENGER_COVER_APPENDIX_FILE);
                case "PassengerConvection":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_PASSENGER_CONVECTION_FILE);
                case "PassengerLicense":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_PASSENGER_LICENSE_FILE); 
                case "PassengerLicenseBlank":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_PASSENGER_LICENSE_BLANK_FILE); 
                case "CautionCover":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_CAUTION_COVER_FILE); 
                case "CautionArticleCover":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_CAUTION_ARTICLE_COVER_FILE);
                case "CautionCoverAutho":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_CAUTION_COVER_AUTHO_FILE);
                case "CautionCoverAppendix":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_CAUTION_COVER_APPENDIX_FILE);
                case "CautionCoverTextAppendix":
                    return individual + ConfigurationSettings.AppSettings["CautionCoverTextAppendixFile"];
                case "CautionConvection":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_CAUTION_CONVECTION_FILE); 
                //TODO:  <<Autor: Luisa Fernanda Ramírez; Fecha: 27/12/2010; Asunto: OT-0051 Renovacion de Autos Individuales. Compañía: 1 
                case "TemplateAgent":
                    return renewal + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_TEMPLATE_AGENT_FILE); 
                /* Autor: Luisa Fernanda Ramírez, Fecha: 21/12/2010 >>*/
                //TODO: <<Autor: Miguel López; Fecha: 23/03/2011; Asunto: Referencias a archivos de reporte de pagarés
                case "PromissoryNoteCoverConsortium":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_PROMISSORY_NOTE_COVER_CONSORTIUM_FILE); 
                case "PromissoryNoteCoverPerson":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_PROMISSORY_NOTE_COVER_PERSON_FILE); 
                case "PromissoryNoteCoverCompany":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_PROMISSORY_NOTE_COVER_COMPANY_FILE); 
                case "PromissoryNoteConsortium":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_PROMISSORY_NOTE_CONSORTIUM_FILE); 
                case "PromissoryNotePerson":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_PROMISSORY_NOTE_PERSON_FILE); 
                case "PromissoryNoteCompany":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_PROMISSORY_NOTE_COMPANY_FILE); 
                case "PromissoryNoteOpenPerson":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_PROMISSORY_NOTE_OPEN_PERSON_FILE); 
                case "PromissoryNoteOpenCompany":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_PROMISSORY_NOTE_OPEN_COMPANY_FILE); 
                case "PromissoryNoteClosedPerson":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_PROMISSORY_NOTE_CLOSED_PERSON_FILE); 
                case "PromissoryNoteClosedCompany":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_PROMISSORY_NOTE_CLOSED_COMPANY_FILE); 
                case "PromissoryNoteCoverConsortiumIndividual":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_PROMISSORY_NOTE_COVER_CONSORTIUM_INDIVIDUAL); 
                case "PromissoryNoteConsortiumIndividual":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_PROMISSORY_NOTE_CONSORTIUM_INDIVIDUAL); 
                case "PromissoryNoteOpenLetterConsortium":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_PROMISSORY_NOTE_OPEN_LETTER_CONSORTIUM_FILE); 
                case "PromissoryNoteClosedLetterConsortium":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_PROMISSORY_NOTE_CLOSED_LETTER_CONSORTIUM_FILE); 
                case "PromissoryNoteClosedConsortium":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_PROMISSORY_NOTE_CLOSED_CONSORTIUM_FILE); 
                case "PromissoryNoteOpenConsortium":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_PROMISSORY_NOTE_OPEN_CONSORTIUM_FILE);
                case "CropCoverAppendixDeductible":
                    return individual + (string)UTILHELPER.EnumHelper.GetEnumParameterValue<ReportKeys>(ReportKeys.RPT_CROP_COVER_APPENDIX_DEDUCTIBLE);
                case "PromissoryNoteClosedNew":
                    return individual + "PROMISSORY_NOTE_CLOSED.RPT";
                case "PromissoryNoteOpenedNew":
                    return individual + "PROMISSORY_NOTE_OPENED.RPT";
                case "PromissoryNoteArrendamiento":
                    return individual + "PROMISSORY_NOTE_ARRENDAMIENTO.RPT";
                case "PromissoryNoteLetterOfEngagement":
                    return individual + "PROMISSORY_NOTE_LETTER_OF_ENGAGEMENT.RPT";
                case "PromissoryNoteCoinsurance":
                    return individual + "PROMISSORY_NOTE_COINSURANCE.RPT";
                case "AuthoAppendix":
                    return individual + "AUTHO_APPENDIX.RPT";
                default:
                    break;
            }
            return string.Empty;
        }
    }
}
