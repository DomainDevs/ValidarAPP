using OnuListRisk.Business;
using OnuListRisk.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using OnuListRisk.Enum;
using System.Security.Cryptography;
using System.Threading;
using System.Net.Mail;
using System.Globalization;

namespace OnuListRisk
{
    public class OnuListRiskInitializer
    {
        #region Properties and construct

        private static List<OnuList> _onuListRisks { get; set; }

        private static OnuPeopleProcessModel _SystemPeople { get; set; }

        private static bool _processing { get; set; }

        private static int _currentProcessId { get; set; }

        private static List<int> _currentRequestProcessing { get; set; }

        private static int _matchedOnu { get; set; }

        public OnuListRiskInitializer()
        {
            OnuListRiskDAO onuListRiskBusiness = new OnuListRiskDAO();

            _currentRequestProcessing = new List<int>();
        }

        #endregion

        #region File

        private static async Task<XmlDocument> GetOnuListRiskAsync()
        {
            XmlDocument xmlDoc = new XmlDocument();

            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                string OnuUrl = ConfigurationManager.AppSettings["OnuUrl"];
                var page = await wc.DownloadStringTaskAsync(OnuUrl);

                xmlDoc.LoadXml(page);
            }

            return xmlDoc;
        }

        private static bool SaveFile(XmlDocument xmlDocument, string shaCode)
        {

            string externalDirectoryPath = ConfigurationManager.AppSettings["ExternalFolderFiles"];

            if (!System.IO.Directory.Exists(externalDirectoryPath))
            {
                System.IO.Directory.CreateDirectory(externalDirectoryPath);
            }

            string user = ConfigurationManager.AppSettings["UserDomain"];
            string password = ConfigurationManager.AppSettings["DomainPassword"];
            string domain = ConfigurationManager.AppSettings["Domain"];

            Uri uri = new Uri(externalDirectoryPath, UriKind.Absolute);

            int retries = 0;

            while (retries < 5)
            {
                try
                {
                    xmlDocument.Save(externalDirectoryPath + "/" + shaCode + ".xml");
                    break;

                }
                catch (Exception)
                {
                    retries++;
                    if (retries == 5)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static string GenerateFileSha(string value)
        {
            string SALT = "*/-";
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] hashedBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(value + SALT));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        #endregion

        #region Process

        public void GenerateProcess(int processFrequency, int processNodes, int processRetries, int? requestId)
        {
            TimeSpan interval = new TimeSpan(processFrequency, 0, 0);
            GetSystemPeople();
            while (true)
            {
                while (processRetries > 0)
                {

                    if (!_processing)
                    {
                        _processing = true;
                        OnuListRiskDAO onuListRiskBusiness = new OnuListRiskDAO();
                        _onuListRisks = onuListRiskBusiness.GetOnuLists();
                        OnuList onuList = new OnuList();
                        onuList.RequestId = requestId == null ? 0 : requestId;
                        OnuListRiskDAO onuListRiskDao = new OnuListRiskDAO();
                        onuList.StatusId = (int)OnuProcessStatusEnum.Cargando;
                        onuList.RegistrationDate = DateTime.Now;
                        onuList = onuListRiskDao.CreateOnuList(onuList);
                        onuListRiskDao.RegisterOnuListLog(onuList);

                        try
                        {
                            XmlDocument xmlDoc = GetOnuListRiskAsync().Result;
                            XmlNodeList individuals = xmlDoc.GetElementsByTagName("INDIVIDUAL");
                            XmlNodeList entities = xmlDoc.GetElementsByTagName("ENTITY");

                            int totalRows = individuals.Count + entities.Count;
                            onuList.PersonCount = totalRows;
                            onuList.ShaCode = GenerateFileSha(xmlDoc.InnerXml);

                            if (!_onuListRisks.Any(x => x.ShaCode == onuList.ShaCode) && SaveFile(xmlDoc, onuList.ShaCode))
                            {
                                onuList.ProcessId = onuListRiskDao.CreateAsyncProcess(onuList.Guid.ToString(), totalRows);
                                _currentProcessId = onuList.ProcessId;

                                onuList.StatusId = (int)OnuProcessStatusEnum.Cargado;
                                onuListRiskDao.RegisterOnuListLog(onuList);
                                onuListRiskDao.UpdateOnuList(onuList);
                                if (onuList.RequestId > 0)
                                {
                                    onuListRiskDao.UpdateProcessRequest(onuList);
                                }

                                Task.Run(() => InitializeProcess(onuList, individuals, entities, processNodes));
                                break;

                            }
                            else
                            {
                                OnuList lastOnuProcess = new OnuList();
                                lastOnuProcess = _onuListRisks.Where(x => x.ShaCode == onuList.ShaCode && x.ProcessId != 0).FirstOrDefault();

                                onuList.StatusId = (int)OnuProcessStatusEnum.ValidadoSinCambios;
                                onuListRiskDao.UpdateOnuList(onuList);
                                onuListRiskDao.RegisterOnuListLog(onuList);
                                if (onuList.RequestId > 0)
                                {
                                    onuListRiskDao.UpdateProcessRequest(onuList);
                                    _currentRequestProcessing.Remove(onuList.RequestId.Value);
                                }
                                _processing = false;
                                int matchProcessId = onuListRiskDao.GenerateListRiskProcessRequest(true, true, false, string.Empty, true);
                                string messageFailBody = "La lista ONU se valido a las " + DateTime.Now + " hora del servidor, no hay cambios respecto a la version actual de la lista. " +
                                    "El proceso de coincidencias con Id: " + matchProcessId + " esta en proceso.";
                                string fileName = onuListRiskDao.CreateListReport(lastOnuProcess, ConfigurationManager.AppSettings["ExternalFolderFiles"]);
                                SendMailNotification(fileName, messageFailBody);

                                break;
                            }

                        }
                        catch (Exception)
                        {
                            processRetries--;
                            _processing = false;
                            if (processRetries == 0)
                            {
                                onuList.Error = "Error obteniendo lista de riesgo Onu";
                                onuList.StatusId = (int)OnuProcessStatusEnum.ConErrores;
                                onuListRiskDao.RegisterOnuListLog(onuList);
                                onuListRiskDao.UpdateOnuList(onuList);
                                if (onuList.RequestId > 0)
                                {
                                    onuListRiskDao.UpdateProcessRequest(onuList);
                                    _currentRequestProcessing.Remove(onuList.RequestId.Value);
                                }
                                string messageFailBody = "No se completó exitosamente el proceso del cargué automático de la lista ONU " + DateTime.Now + "(hora igual a SISE). Esto pudo generarse por: - Error de conexión de página - Error en el cargue de la lista(que no la cargue o no la carga completamente) .Para mayor información consulte los logs en BD con el Administrador.";
                                SendMailNotification("", messageFailBody);
                                break;
                            }
                        }
                    }

                }
                Thread.Sleep(interval);
            }
        }

        public void CheckProcessRequest(int processFrequency, int processNodes, int processRetries)
        {
            OnuListRiskDAO onuListRiskDao = new OnuListRiskDAO();
            while (true)
            {
                List<int> requestId = onuListRiskDao.GetProcessRequest();
                if (requestId.Count > 0)
                {
                    foreach (int id in requestId)
                    {
                        while (true)
                        {
                            if (_currentRequestProcessing.Contains(id))
                            {
                                break;
                            }
                            else if (!_processing)
                            {
                                _currentRequestProcessing.Add(id);
                                Task.Run(() => GenerateProcess(processFrequency, processNodes, processRetries, id));
                                break;
                            }
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }

        private static void SendMailNotification(string fileName, string messageBody)
        {
            OnuListRiskDAO onuListRiskDao = new OnuListRiskDAO();
            string filePath = string.Empty;
            if (!string.IsNullOrEmpty(fileName))
            {
                filePath = ConfigurationManager.AppSettings["ExternalFolderFiles"] + "\\" + fileName + ".xlsx";
            }
            string email = ConfigurationManager.AppSettings["EMailTo"];
            string SmtpServer = ConfigurationManager.AppSettings.Get("SMTPServer");
            int SMTPServerPort = Convert.ToInt32(ConfigurationManager.AppSettings.Get("SMTPServerPort"));
            string strUserName = ConfigurationManager.AppSettings.Get("EmailFrom");
            string strSenderName = "ONU List risk service";
            string strPassword = ConfigurationManager.AppSettings.Get("EMailPass");
            string strEmailFrom = ConfigurationManager.AppSettings.Get("EmailFrom");
            bool ews = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("EWS"));

            onuListRiskDao.SendMailNotification(ews, SmtpServer, SMTPServerPort, filePath, email, strUserName, strSenderName, strPassword,
            strEmailFrom, messageBody);
        }

        #endregion

        #region Data

        private static async Task InitializeProcess(OnuList onuList, XmlNodeList individuals, XmlNodeList entities, int processNodes)
        {
            OnuListRiskDAO onuListRiskDao = new OnuListRiskDAO();
            onuList.StatusId = (int)OnuProcessStatusEnum.Procesando;
            onuListRiskDao.RegisterOnuListLog(onuList);
            onuListRiskDao.UpdateOnuList(onuList);
            string fileName = string.Empty;
            bool processWithErrors = false;
            string messageFailBody = "";

            try
            {
                await Task.Run(() => (from XmlNode N in individuals select N).AsParallel().WithDegreeOfParallelism(processNodes).ForAll(InsertIndividual));
                await Task.Run(() => (from XmlNode E in entities select E).AsParallel().WithDegreeOfParallelism(processNodes).ForAll(InsertEntity));
            }
            catch (Exception)
            {
                onuList.StatusId = (int)OnuProcessStatusEnum.ConErrores;
                onuList.Error = "Error en formato del archivo";
                onuListRiskDao.RegisterOnuListLog(onuList);
                onuListRiskDao.UpdateOnuList(onuList);
                _processing = false;
                _onuListRisks = onuListRiskDao.GetOnuLists();
                processWithErrors = true;
                messageFailBody = "No se completó exitosamente el proceso del cargué automático de la lista ONU " + DateTime.Now + "(hora igual a SISE). Esto pudo generarse por: - Error de conexión de página - Error en el cargue de la lista(que no la cargue o no la carga completamente) .Para mayor información consulte los logs en BD con el Administrador.";
                SendMailNotification("", messageFailBody);
            }

            int matchProcessId = onuListRiskDao.GenerateListRiskProcessRequest(true, true, false, string.Empty, true);

            if (!processWithErrors)
            {
                onuList.StatusId = (int)OnuProcessStatusEnum.Procesado;
                onuListRiskDao.RegisterOnuListLog(onuList);
                onuListRiskDao.UpdateOnuList(onuList);
                _processing = false;
                _onuListRisks = onuListRiskDao.GetOnuLists();
                fileName = onuListRiskDao.CreateListReport(onuList, ConfigurationManager.AppSettings["ExternalFolderFiles"]);
            }

            _currentRequestProcessing.Remove(onuList.RequestId.Value);

            var refeshCache = onuListRiskDao.SendToRefreshOnMemoryListRisks();

            string messageSuccessBody = "Finalizó exitosamente el proceso del cargué automático de la lista ONU " + DateTime.Now + " (hora igual a SISE), " +
                "<br/>El proceso de coincidencias con Id: " + matchProcessId + " esta en proceso." +
                $"<br/>{refeshCache}";
            SendMailNotification(fileName, messageSuccessBody);
            _matchedOnu = 0;
        }

        private static void InsertIndividual(XmlNode N)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(OnuPerson));
            OnuPerson person = new OnuPerson();
            using (TextReader textReader = new StringReader(@"<INDIVIDUAL>" + N.InnerXml + "</INDIVIDUAL>"))
            {
                person = (OnuPerson)serializer.Deserialize(textReader);
            }
            OnuListRiskDAO listRiskBusines = new OnuListRiskDAO();

            person.Document.Sort((a, b) => b.IssueDate.CompareTo(a.IssueDate));

            listRiskBusines.CreateListRiskPersonTemporal(JsonConvert.SerializeObject(person), person.DataId.ToString(), "OnuService", DateTime.Now, _currentProcessId, (int)RiskListTypeEnum.ONU, (int)RiskListEventEnum.INCLUDED, RiskListConstants.ONU);
            listRiskBusines.IssueRecordListRiskReal(JsonConvert.SerializeObject(person), person.DataId.ToString(), "OnuService", DateTime.Now, _currentProcessId, (int)RiskListTypeEnum.ONU, (int)RiskListEventEnum.INCLUDED, RiskListConstants.ONU);

        }

        private static void InsertEntity(XmlNode E)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(OnuEntity));
            OnuEntity entity = new OnuEntity();
            using (TextReader textReader = new StringReader(@"<ENTITY>" + E.InnerXml + "</ENTITY>"))
            {
                entity = (OnuEntity)serializer.Deserialize(textReader);
            }
            OnuListRiskDAO listRiskBusines = new OnuListRiskDAO();

            entity.Document.Sort((a, b) => b.IssueDate.CompareTo(a.IssueDate));

            listRiskBusines.CreateListRiskPersonTemporal(JsonConvert.SerializeObject(entity), entity.DataId.ToString(), "OnuService", DateTime.Now, _currentProcessId, (int)RiskListTypeEnum.ONU, (int)RiskListEventEnum.INCLUDED, RiskListConstants.ONU);
            listRiskBusines.IssueRecordListRiskReal(JsonConvert.SerializeObject(entity), entity.DataId.ToString(), "OnuService", DateTime.Now, _currentProcessId, (int)RiskListTypeEnum.ONU, (int)RiskListEventEnum.INCLUDED, RiskListConstants.ONU);

        }

        public void GetSystemPeople()
        {
            OnuListRiskDAO onuListRiskDAO = new OnuListRiskDAO();
            OnuPeopleProcessModel matchingProcess = onuListRiskDAO.GetSystemPeople();
            _SystemPeople = new OnuPeopleProcessModel();
            _SystemPeople.People = new List<PersonModel>();
            _SystemPeople.Company = new List<CompanyModel>();
            _SystemPeople.People = matchingProcess.People;
            _SystemPeople.Company = matchingProcess.Company;
        }

        #endregion

    }

}

