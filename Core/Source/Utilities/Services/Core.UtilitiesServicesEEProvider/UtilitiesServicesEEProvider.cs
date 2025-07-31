using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
      
using Sistran.Core.Application.UtilitiesServicesEEProvider.Helper;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.UtilitiesServicesEEProvider.Resources;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using PTH= System.IO;
using System.Linq;
using System.Net.Mail;
using System.ServiceModel;
using System.Threading.Tasks;
using Sistran.Core.Application.UtilitiesServices.Models;
using Sistran.Core.Application.Utilities.Models;
using Sistran.Core.Application.Utilities.Configuration;

namespace Sistran.Core.Services.UtlilitiesServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class UtlilitiesServicesEEProvider : IUtilitiesServiceCore
    {
        public UtlilitiesServicesEEProvider()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        /// <summary>
        /// Obtener Proceso Asíncrono Por Identificador
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Proceso Asíncrono</returns>
        public AsynchronousProcess GetAsynchronousProcessById(int id)
        {
            try
            {
                AsynchronousProcessDAO asynchronousProcessDAO = new AsynchronousProcessDAO();
                return asynchronousProcessDAO.GetAsynchronousProcessById(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetAsynchronousProcessByIdentifier), ex);
            }
        }

        /// <summary>
        /// Actualizar Proceso Asíncrono
        /// </summary>
        /// <param name="asynchronousProcess">Proceso Asíncrono</param>
        /// <returns>Proceso Asíncrono</returns>
        public AsynchronousProcess UpdateAsynchronousProcess(AsynchronousProcess asynchronousProcess)
        {
            try
            {
                AsynchronousProcessDAO dao = new AsynchronousProcessDAO();
                return dao.UpdateAsynchronousProcess(asynchronousProcess);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorUpdateAsynchronousProcess), ex);
            }
        }

        /// <summary>
        /// Generar Id Proceso Asíncrono
        /// </summary>
        /// <param name="userId">Id Usuario</param>
        /// <param name="description">Descripción Proceso</param>
        /// <returns>Id Proceso</returns>
        public int GenerateAsynchronousProcessId(int userId, string description, int statusId)
        {
            try
            {
                AsynchronousProcessDAO dao = new AsynchronousProcessDAO();
                return dao.GenerateAsynchronousProcessId(userId, description, statusId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGenerateAsynchronousProcessId), ex);
            }
        }

        /// <summary>
        /// Eliminar JSON
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Eliminado Si/No</returns>
        public bool DeletePendingOperation(int id)
        {
            try
            {
                PendingOperationsDAO pendingOperationDAO = new PendingOperationsDAO();
                return pendingOperationDAO.DeletePendingOperation(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorDeleteJSON);
            }
        }

        /// <summary>
        /// Eliminar Hijos de un JSON
        /// </summary>
        /// <param name="parentId">Id Padre</param>
        /// <returns>Eliminados Si/No</returns>
        public bool DeletePendingOperationsByParentId(int parentId)
        {
            try
            {
                PendingOperationsDAO pendingOperationsDAO = new PendingOperationsDAO();
                return pendingOperationsDAO.DeletePendingOperationsByParentId(parentId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorDeleteJSON);
            }
        }


        /// <summary>
        /// Guardar JSON
        /// </summary>
        /// <param name="pendingOperation">Datos operacion</param>
        /// <returns>Modelo PendingOperation</returns>
        public PendingOperation CreatePendingOperation(PendingOperation pendingOperation)
        {
            try
            {
                PendingOperationsDAO pendingOperationDAO = new PendingOperationsDAO();
                return pendingOperationDAO.CreatePendingOperation(pendingOperation);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSaveJSON), ex);
            }
        }

        /// <summary>
        /// Actualizar JSON
        /// </summary>
        /// <param name="pendingOperation">Datos operacion</param>
        /// <returns>Modelo PendingOperation</returns>
        public PendingOperation UpdatePendingOperation(PendingOperation pendingOperation)
        {
            try
            {
                PendingOperationsDAO pendingOperationDAO = new PendingOperationsDAO();
                return pendingOperationDAO.UpdatePendingOperation(pendingOperation);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorUpdateJSON), ex);
            }
        }

        /// <summary>
        /// Obtener JSON
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Modelo PendingOperation</returns>
        public PendingOperation GetPendingOperationById(int id)
        {
            try
            {
                PendingOperationsDAO pendingOperationDAO = new PendingOperationsDAO();
                return pendingOperationDAO.GetPendingOperationById(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetJSON), ex);
            }
        }

        /// <summary>
        /// Obtener JSON hijo
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <param name="parentId">Id padre</param>
        /// <returns>Modelo PendingOperation</returns>
        public PendingOperation GetPendingOperationByIdParentId(int id, int parentId)
        {
            try
            {
                PendingOperationsDAO pendingOperationDAO = new PendingOperationsDAO();
                return pendingOperationDAO.GetPendingOperationByIdParentId(id, parentId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetJSONChild), ex);
            }
        }

        /// <summary>
        /// Obtener lista de JSON
        /// </summary>
        /// <param name="parentId">Id padre</param>
        /// <returns>Lista de JSONs</returns>
        public List<PendingOperation> GetPendingOperationsByParentId(int parentId)
        {
            try
            {
                PendingOperationsDAO pendingOperationDAO = new PendingOperationsDAO();
                return pendingOperationDAO.GetPendingOperationsByParentId(parentId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetListJSON), ex);
            }
        }

        public bool SendMail(string FilePath, string email)
        {
            try
            {
                List<MailAddress> To = new List<MailAddress>();
                MailAddress destiny = new MailAddress(email, "Sistran email test");
                To.Add(destiny);
                //Quemamos la ruta, para prueba
                FilePath = @"\\COBOPRVR1D1\ReportFiles\10\7\10730015110.pdf";



                //Objetos SMTP para envio de mensaje
                string SmtpServer = ConfigurationManager.AppSettings.Get("SMTPServer"); //Servidor SMTP.
                int PortNum = Convert.ToInt32(ConfigurationManager.AppSettings.Get("SMTPServerPort"));
                //Puerto Servidor SMTP.
                string strUserName = ConfigurationManager.AppSettings.Get("SenderEMail"); //UserName
                string strSenderName = ConfigurationManager.AppSettings.Get("SenderName"); //UserName
                string strPassword = ConfigurationManager.AppSettings.Get("SenderEMailPass"); //Password
                string strEmailFrom = ConfigurationManager.AppSettings.Get("EmailFrom");
                bool bolUseDefaultCredential = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("UseDefaultCredential"));
                System.Net.NetworkCredential Auth = new System.Net.NetworkCredential(strUserName, strPassword);
                MailAddress From = new MailAddress(strEmailFrom, strSenderName);

                // Se Configura el servidor SMTP.
                SmtpClient SC = new SmtpClient(SmtpServer, PortNum);

                // Se establece en False la Conexión segura ya que el servidor no la soporta.
                SC.EnableSsl = false;
                SC.UseDefaultCredentials = bolUseDefaultCredential;
                // Se Crea el mensaje de email.
                using (MailMessage message = new MailMessage())
                {

                    message.From = From; // Se añade el remitente.

                    // Se añaden los destinatarios.
                    foreach (MailAddress ma in To)
                    {
                        message.To.Add(ma);
                    }

                    string file = PTH.Path.GetFileName(FilePath); //Se obtiene el nombre del archivo que se va a adjuntar.
                    message.Subject = "Enviando " + file; // Se establece el Asunto del email.

                    // Se establece el cuerpo del correo.
                    message.Body = string.Format("Archivo {0} enviado el {1} a las {2}.",
                                                 file, DateTime.Now.ToShortDateString(),
                                                 DateTime.Now.ToShortTimeString());

                    message.Attachments.Add(new Attachment(FilePath)); // Se agrega el archivo como archivo adjunto.
                    if (SC.UseDefaultCredentials)
                    {
                        SC.Credentials = Auth; // Se indican las credenciales para autenticarse.
                    }
                    SC.DeliveryMethod = SmtpDeliveryMethod.Network; // Se indica el modo de envío.
                                                                    //SC.Timeout = 10000;//Se establece el tiempo de espera.
                    SC.Send(message); // Se envia el email.
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// realiza el envio de un email
        /// </summary>
        /// <param name="email">detalles del email</param>
        /// <returns></returns>
        public Task<bool> SendEmailAsync(EmailCriteria email)
        {
            try
            {
                return Task.Run(() => HelperEmail.SendEmail(email));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSendEmail), ex);
            }
        }

        /// <summary>
        /// realiza el envio de emails
        /// </summary>
        /// <param name="emails">detalles de los emails</param>
        /// <returns></returns>
        public Task<List<bool>> SendEmailsAsync(List<EmailCriteria> emails)
        {
            try
            {
                return Task.Run(() =>
                {
                    var result = new ConcurrentQueue<bool>();

                    Parallel.ForEach(emails, email =>
                    {
                        result.Enqueue(HelperEmail.SendEmail(email));
                    });
                    return result.ToList();
                });
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSendEmail), ex);
            }
        }

        /// <summary>
        /// Obtiene la lista de archivos de la busqueda avanzada por descripcion y tipo
        /// </summary>
        /// <returns>Lista de archivos consultadas por descripcion y tipo </returns>
        public List<File> GetFiles(string Description, FileType fileType)
        {
            try
            {
                FileDAO fileprovider = new FileDAO();
                return fileprovider.GetFiles(Description, fileType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetFiles), ex);
            }
        }

        /// <summary>
        /// Obtiene la lista de archivos por descripcion
        /// </summary>
        /// <returns>Lista de archivos consultadas por descripcion</returns>
        public List<File> GetFileByDescription(string Description)
        {
            try
            {
                FileDAO fileprovider = new FileDAO();
                return fileprovider.GetFileByDescription(Description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetFileByDescription), ex);
            }
        }

        /// <summary>
        /// Realiza los procesos del CRUD para el Archivo
        /// </summary>
        /// <param name="file">Archivo para crear</param>
        public string CreateFile(File file)
        {
            try
            {
                FileDAO fileDAO = new FileDAO();
                return fileDAO.CreateFile(file);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateFile), ex);
            }
        }

        public FileProcessValue GetCollectiveFileByLoadTypeHardRiskTypeId(int loadTypeId, int riskTypeId)
        {
            try
            {
                FileDAO fileDAO = new FileDAO();
                return fileDAO.GetFileProcess(loadTypeId, riskTypeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCollectiveFile), ex);
            }
        }

        /// <summary>
        /// Realiza los procesos del CRUD para el Archivo
        /// </summary>
        /// <param name="file">Archivo para actualizar</param>
        public string UpdateFile(File file)
        {
            try
            {
                FileDAO fileDAO = new FileDAO();
                return fileDAO.UpdateFile(file);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorUpdateFile), ex);
            }
        }

        /// <summary>
        /// Obtener Archivo Por Identificador
        /// </summary>
        /// <param name="structureType">Identificador</param>
        /// <returns>Archivo</returns>
        public File GetFileByFileId(int fileId)
        {
            try
            {
                FileDAO fileDAO = new FileDAO();
                return fileDAO.GetFileByFileId(fileId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetFileByFileId), ex);
            }
        }

        /// <summary>
        /// Crear Archivo
        /// </summary>
        /// <param name="file">Información</param>
        /// <returns>Ruta Archivo</returns>
        public string GenerateFile(File file)
        {
            try
            {
                FileDAO fileDAO = new FileDAO();
                return fileDAO.GenerateFile(file);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateFileByType), ex);
            }
        }

        /// <summary>
        /// Obtiene la lista de plantillas
        /// </summary>
        /// <returns>Lista de plantillas consultadas</returns>
        public List<Template> GetTemplates()
        {
            try
            {
                TemplateDAO templateprovider = new TemplateDAO();
                return templateprovider.GetTemplates();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetTemplates), ex);
            }
        }

        /// <summary>
        /// Obtiene la lista de campos
        /// </summary>
        /// <returns>Lista de campos consultados</returns>
        public List<Field> GetFields()
        {
            try
            {
                TemplateDAO fileprovider = new TemplateDAO();
                return fileprovider.GetFields();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetFields), ex);
            }
        }

        /// <summary>
        /// Validar Archivo
        /// </summary>
        /// <param name="fileName">Nombre Archivo</param>
        /// <param name="userName">Nombre Usuario</param>
        /// <param name="templates">Plantillas</param>
        /// <returns>Datos</returns>
        public File ValidateDataFile(File file, string userName)
        {
            try
            {
                TemplateDAO templateDAO = new TemplateDAO();
                return templateDAO.ValidateDataFile(file, userName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorValidateFile), ex);
            }
        }

        /// <summary>
        /// Validar Archivo
        /// </summary>
        /// <param name="fileName">Nombre Archivo</param>
        /// <param name="userName">Nombre Usuario</param>
        /// <param name="templates">Plantillas</param>
        /// <returns>Datos</returns>
        public Template ValidateDataTemplate(string fileName, string userName, Template template)
        {
            try
            {
                TemplateDAO templateDAO = new TemplateDAO();
                return templateDAO.ValidateDataTemplate(fileName, userName, template);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorValidateFile), ex);
            }
        }


        public object GetValueByField<T>(Field field)
        {
            try
            {
                TemplateDAO templateDAO = new TemplateDAO();
                return templateDAO.GetValueByField<T>(field);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetValueByField), ex);
            }
        }

        /// <summary>
        /// Obtener Archivo Por Filtros
        /// </summary>
        /// <param name="fileProcessValue">Filtros</param>
        /// <returns>Archivo</returns>
        public File GetFileByFileProcessValue(FileProcessValue fileProcessValue)
        {
            try
            {
                FileDAO fileDAO = new FileDAO();
                return fileDAO.GetFileByFileProcessValue(fileProcessValue);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetFileByFilters), ex);
            }
        }


        public List<Validation> ExecuteValidations(List<ValidationIdentificator> validationIdentificators, List<ValidationRegularExpression> validationRegularExpressions)
        {
            ValidationDAO validationDAO = new ValidationDAO();
            return validationDAO.ExecuteValidations(validationIdentificators, validationRegularExpressions);
        }

        /// <summary>
        /// Validar Archivo
        /// </summary>
        /// <param name="file">Archivo</param>
        /// <param name="userName">Nombre Usuario</param>
        /// <returns>Archivo</returns>
        public File ValidateFile(File file, string userName)
        {
            try
            {
                TemplateDAO templateDAO = new TemplateDAO();
                return templateDAO.ValidateFile(file, userName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Datos De Plantillas Relacionados Por Identificador
        /// </summary>
        /// <param name="templates">Plantillas</param>
        /// <returns>Datos</returns>
        public List<File> GetDataTemplates(List<Template> templates)
        {
            try
            {
                TemplateDAO templateDAO = new TemplateDAO();
                return templateDAO.GetDataTemplates(templates);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crear Archivo Excel
        /// </summary>
        /// <param name="file">Información</param>
        /// <returns>Información</returns>
        public string CreateExcelFile(File file)
        {
            try
            {
                FileDAO fileDAO = new FileDAO();
                return fileDAO.CreateExcelFile(file);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateFile), ex);
            }
        }

        public List<Validation> GetValidatedTemporalPolicies(List<Validation> validations, List<ValidationTemporalPolicy> temporalpolicies)
        {
            try
            {
                ValidationDAO validationDAO = new ValidationDAO();
                return validationDAO.GetValidatedTemporalPolicies(validations, temporalpolicies);

            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorFindTemporalPolicies, ex);
            }
        }

        /// <summary>
        ///Metodo de consulta parametro de validacion para definir obligatoriedad de contragarantias - previsora
        /// </summary>
        /// <returns>bool</returns>
        public bool EnableCrossGuarantees()
        {
            string ValueCrossGuarantee = ConfigurationManager.AppSettings.Get("EnableCrossGuarantees"); 
            if (ValueCrossGuarantee == "1")
                return true;
            else
                return false;
        }

        #region EndorsementControl

        /// <summary>
        /// Insert userId y endorsementId para validacion de poliza en uso
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void CreateEndorsementControl(int id, int userId)
        {
            EndorsementControl endorsementControlModel= new EndorsementControl();
            endorsementControlModel.Id = id;
            endorsementControlModel.UserId = userId;

            EndorsementControlDAO endorsementContolDao = new EndorsementControlDAO();
            try
            {
                endorsementContolDao.CreateEndorsementControl(id, userId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateEndorsementControl), ex);
            }           
        }

        /// <summary>
        /// Elimina registro de poliza ne uso por endorsement id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteEndorsementControl(int id)
        {
            try
            {
                EndorsementControlDAO endorsementContolDao = new EndorsementControlDAO();
                return endorsementContolDao.DeleteEndorsementControl(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorDeleteEndorsementControl);
            }
        }

        public bool GetEndorsementControlById(int id, int userId)
        {
            try
            {
                EndorsementControlDAO endorsementContolDao = new EndorsementControlDAO();
                return endorsementContolDao.GetEndorsementControlById(id,userId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetJSON), ex);
            }
        }

        public List<ValidationRegularExpression> GetAllValidationRegularExpressions() {
            try
            {
                ValidationDAO validationDAO = new ValidationDAO();
                return validationDAO.GetAllValidationRegularExpressions();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetJSON), ex);
            }
        }


        #endregion


        public Template GetTextFieldsByFileNameUserName(string fileName, string userName, Template template, int typeFile)
        {
            try
            {
                TemplateDAO templateDAO = new TemplateDAO();
                return templateDAO.GetTextFieldsByFileNameUserName(fileName, userName, template, typeFile);

            }
            catch (Exception ex)
            {
                throw new BusinessException("", ex);
            }
        }
              
        private static ConcurrentDictionary<string, EnumParameter>  enumParameterCache = new ConcurrentDictionary<string, EnumParameter>();

        public EnumParameter LoadEnumParameterValuesFromDB()
        {
            throw new NotImplementedException();
        }

        public EnumParameter GetEnumParameterValue<T>(T keys) where T : struct
        {
            EnumParameter enumparameter = new EnumParameter() ;
            var key = keys.ToString();
            if (enumParameterCache.ContainsKey(key))
            {
                EnumParameter enumparameterOut;
                enumParameterCache.TryGetValue(key, out enumparameterOut);
                enumparameter =enumparameterOut;
            }
            else
            {
                throw new BusinessException("");
            }
            return enumparameter;
        }
        
    }
}
