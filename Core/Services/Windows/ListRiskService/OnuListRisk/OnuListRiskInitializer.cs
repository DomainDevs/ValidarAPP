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

namespace OnuListRisk
{
    public class OnuListRiskInitializer
    {
        #region Properties and construct
        private static List<OnuList> _onuListRisks { get; set; }
        private static bool _processing { get; set; }
        private static int _currentProcessId { get; set; }
        public OnuListRiskInitializer()
        {
            OnuListRiskBusiness onuListRiskBusiness = new OnuListRiskBusiness();
            _onuListRisks = onuListRiskBusiness.GetOnuLists();
        }

        #endregion

        #region File

        private static async Task<XmlDocument> GetOnuListRiskAsync()
        {
            XmlDocument xmlDoc = new XmlDocument();
            using (WebClient wc = new WebClient())
            {
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
        public void GenerateProcess(int processFrequency, int processNodes, int processRetries)
        {
            TimeSpan interval = new TimeSpan(processFrequency, 0, 0);

            while (true)
            {
                while (processRetries > 0)
                {

                    if (!_processing)
                    {
                        _processing = true;
                     

                        OnuList onuList = new OnuList();
                        OnuListRiskBusiness onuListRiskBusiness = new OnuListRiskBusiness();
                        onuList.StatusId = (int)ProcessStatusEnum.Cargando;
                        onuList.RegistrationDate = DateTime.Now;
                        onuList = onuListRiskBusiness.CreateOnuList(onuList);
                        onuListRiskBusiness.RegisterOnuListLog(onuList);

                        try
                        {
                            XmlDocument xmlDoc = GetOnuListRiskAsync().Result;
                            XmlNodeList individuals = xmlDoc.GetElementsByTagName("INDIVIDUAL");
                            XmlNodeList entities = xmlDoc.GetElementsByTagName("ENTITY");

                            int totalRows = individuals.Count + entities.Count;

                            onuList.ProcessId = onuListRiskBusiness.CreateAsyncProcess(onuList.Guid.ToString(), totalRows);
                            _currentProcessId = onuList.ProcessId;
                            onuList.ShaCode = GenerateFileSha(xmlDoc.InnerXml);

                            if (!_onuListRisks.Any(x => x.ShaCode == onuList.ShaCode) && SaveFile(xmlDoc, onuList.ShaCode))
                            {
                                onuList.StatusId = (int)ProcessStatusEnum.Cargado;
                                onuListRiskBusiness.RegisterOnuListLog(onuList);
                                onuListRiskBusiness.UpdateOnuList(onuList);

                                Task.Run(() => InitializeProcess(onuList, individuals, entities, processNodes));
                            }
                                                        
                            break;
                        }
                        catch (Exception ex)
                        {
                            processRetries--;
                            if (processRetries == 0)
                            {
                                onuList.Error = ex.Message;
                                onuList.StatusId = (int)ProcessStatusEnum.ConErrores;
                                onuListRiskBusiness.RegisterOnuListLog(onuList);
                                onuListRiskBusiness.UpdateOnuList(onuList);
                            }
                        }
                    }

                }

                Thread.Sleep(interval);
            }
        }

        #endregion

        #region Data

        private static void InitializeProcess(OnuList onuList, XmlNodeList individuals, XmlNodeList entities, int processNodes)
        {
            OnuListRiskBusiness onuListRiskBusiness = new OnuListRiskBusiness();
            onuList.StatusId = (int)ProcessStatusEnum.Procesando;
            onuListRiskBusiness.RegisterOnuListLog(onuList);
            onuListRiskBusiness.UpdateOnuList(onuList);

            Task.Run(() => (from XmlNode N in individuals select N).AsParallel().WithDegreeOfParallelism(processNodes).ForAll(InsertIndividual));
            Task.Run(() => (from XmlNode E in entities select E).AsParallel().WithDegreeOfParallelism(processNodes).ForAll(InsertEntity));

            while (true)
            {
                if (onuListRiskBusiness.ValidateProcessFinish(onuList.ProcessId))
                {
                    onuList.StatusId = (int)ProcessStatusEnum.Procesado;
                    onuListRiskBusiness.RegisterOnuListLog(onuList);
                    onuListRiskBusiness.UpdateOnuList(onuList);
                    break;
                }
                Thread.Sleep(5000);
            }
        }

        private static void InsertIndividual(XmlNode N)
        {

            TextReader textReader = new StringReader(@"<INDIVIDUAL>" + N.InnerXml + "</INDIVIDUAL>");
            XmlSerializer serializer = new XmlSerializer(typeof(OnuPerson));
            OnuPerson person = new OnuPerson();
            person = (OnuPerson)serializer.Deserialize(textReader);
            OnuListRiskBusiness listRiskBusines = new OnuListRiskBusiness();

            person.Document.Sort((a, b) => b.IssueDate.CompareTo(a.IssueDate));

            listRiskBusines.CreateListRiskPersonTemporal(JsonConvert.SerializeObject(person), person.DataId.ToString(), "OnuService", DateTime.Today, _currentProcessId, (int)RiskListTypeEnum.ONU, (int)RiskListEventEnum.INCLUDED, RiskListConstants.ONU);
            listRiskBusines.IssueRecordListRiskReal(JsonConvert.SerializeObject(person), person.DataId.ToString(), "OnuService", DateTime.Today, _currentProcessId, (int)RiskListTypeEnum.ONU, (int)RiskListEventEnum.INCLUDED, RiskListConstants.ONU);
            textReader.Close();

        }

        private static void InsertEntity(XmlNode E)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(OnuEntity));
            TextReader textReader = new StringReader(@"<ENTITY>" + E.InnerXml + "</ENTITY>");
            OnuEntity entity = new OnuEntity();
            entity = (OnuEntity)serializer.Deserialize(textReader);
            OnuListRiskBusiness listRiskBusines = new OnuListRiskBusiness();

            entity.Document.Sort((a, b) => b.IssueDate.CompareTo(a.IssueDate));

            listRiskBusines.CreateListRiskPersonTemporal(JsonConvert.SerializeObject(entity), entity.DataId.ToString(), "OnuService", DateTime.Today, _currentProcessId, (int)RiskListTypeEnum.ONU, (int)RiskListEventEnum.INCLUDED, RiskListConstants.ONU);
            listRiskBusines.IssueRecordListRiskReal(JsonConvert.SerializeObject(entity), entity.DataId.ToString(), "OnuService", DateTime.Today, _currentProcessId, (int)RiskListTypeEnum.ONU, (int)RiskListEventEnum.INCLUDED, RiskListConstants.ONU);
            textReader.Close();
        }

        #endregion


    }

}

