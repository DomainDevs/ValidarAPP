using Sistran.Core.Application.Utilities.Models;
using Sistran.Core.Application.UtilitiesServices.Models;
using Sistran.Core.Services.UtilitiesServices.Models;
using System.Collections.Generic;
using System.Net.Mail;
using System.ServiceModel;
using System.Threading.Tasks;
using COMMENUM = Sistran.Core.Services.UtilitiesServices.Enums;
using MODEL = Sistran.Core.Services.UtilitiesServices.Models;

namespace Sistran.Core.Services.UtilitiesServices
{
    [ServiceContract]
    public interface IUtilitiesServiceCore
    {

        /// <summary>
        /// Obtener Proceso Asíncrono Por Identificador
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Proceso Asíncrono</returns>
        [OperationContract]
        AsynchronousProcess GetAsynchronousProcessById(int id);

        /// <summary>
        /// Actualizar Proceso Asíncrono
        /// </summary>
        /// <param name="asynchronousProcess">Proceso Asíncrono</param>
        /// <returns>Proceso Asíncrono</returns>
        [OperationContract]
        AsynchronousProcess UpdateAsynchronousProcess(AsynchronousProcess asynchronousProcess);

        /// <summary>
        /// Generar Id Proceso Asíncrono
        /// </summary>
        /// <param name="userId">Id Usuario</param>
        /// <param name="description">Descripción Proceso</param>
        /// <returns>Id Proceso</returns>
        [OperationContract]
        int GenerateAsynchronousProcessId(int userId, string description, int statusId);


        /// <summary>
        /// Guardar JSON
        /// </summary>
        /// <param name="pendingOperation">Datos operacion</param>
        /// <returns>Modelo PendingOperation</returns>
        [OperationContract]
        PendingOperation CreatePendingOperation(Models.PendingOperation pendingOperation);

        /// <summary>
        /// Actualizar JSON
        /// </summary>
        /// <param name="pendingOperation">Datos operacion</param>
        /// <returns>Modelo PendingOperation</returns>
        [OperationContract]
        PendingOperation UpdatePendingOperation(Models.PendingOperation pendingOperation);

        /// <summary>
        /// Obtener JSON
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Modelo PendingOperation</returns>
        [OperationContract]
        PendingOperation GetPendingOperationById(int id);

        /// <summary>
        /// Obtener JSON hijo
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <param name="parentId">Id padre</param>
        /// <returns>Modelo PendingOperation</returns>
        [OperationContract]
        PendingOperation GetPendingOperationByIdParentId(int id, int parentId);

        /// <summary>
        /// Obtener lista de JSON
        /// </summary>
        /// <param name="parentId">Id padre</param>
        /// <returns>Lista de JSONs</returns>
        [OperationContract]
        List<PendingOperation> GetPendingOperationsByParentId(int parentId);

        /// <summary>
        /// Enviar email
        /// </summary>
        /// <param name="FilePath">Ruta</param>
        /// <returns>Lista de JSONs</returns>
        [OperationContract]
        bool SendMail(string FilePath, string email);

        /// <summary>
        /// realiza el envio de emails
        /// </summary>
        /// <param name="emails">detalles de los emails</param>
        /// <returns></returns>
        [OperationContract]
        Task<List<bool>> SendEmailsAsync(List<EmailCriteria> emails);

        /// <summary>
        /// realiza el envio de un email
        /// </summary>
        /// <param name="email">detalles del email</param>
        /// <returns></returns>
        [OperationContract]
        Task<bool> SendEmailAsync(EmailCriteria email);

        /// <summary>
        /// Obtiene la lista de archivos de la busqueda avanzada por descripcion y tipo
        /// </summary>
        /// <returns>Lista de archivos consultadas por descripcion y tipo </returns>
        [OperationContract]
        List<File> GetFiles(string Description, COMMENUM.FileType fileType);

        /// <summary>
        /// Obtener archivos
        /// </summary>
        /// <returns>Obtener Lista de archivos</returns>
        [OperationContract]
        List<File> GetFileByDescription(string Description);
        /// Obtener Archivo Por Identificador
        /// </summary>
        /// <param name="structureType">Identificador</param>
        /// <returns>Archivo</returns>
        [OperationContract]
        Models.File GetFileByFileId(int fileId);
        /// <summary>
        /// Realiza los procesos del CRUD para los Archivos
        /// </summary>
        /// <param name="file">Lista de files(archivos) para ser eliminados</param>
        /// <returns>Respuesta con el total de procesos realizados </returns>
        [OperationContract]
        string CreateFile(Models.File file);

        [OperationContract]
        FileProcessValue GetCollectiveFileByLoadTypeHardRiskTypeId(int loadTypeId, int riskTypeId);
        /// <summary>
        /// Realiza los procesos del CRUD para los Archivos
        /// </summary>
        /// <param name="file">Lista de files(archivos) para ser eliminados</param>
        /// <returns>Respuesta con el total de procesos realizados </returns>
        [OperationContract]
        string UpdateFile(Models.File file);



        /// <summary>
        /// Crear Archivo
        /// </summary>
        /// <param name="file">Información</param>
        /// <returns>Ruta Archivo</returns>
        [OperationContract]
        string GenerateFile(Models.File file);


        /// <summary>
        /// Obtener plantillas
        /// </summary>
        /// <returns>Obtener Lista de plantillas</returns>
        [OperationContract]
        List<Template> GetTemplates();

        /// <summary>
        /// Obtener campos
        /// </summary>
        /// <returns>Obtener Lista de campos</returns>
        [OperationContract]
        List<Field> GetFields();

        /// <summary>
        /// Validar Archivo
        /// </summary>
        /// <param name="fileName">Nombre Archivo</param>
        /// <param name="userName">Nombre Usuario</param>
        /// <param name="templates">Plantillas</param>
        /// <returns>Datos Archivo</returns>
        [OperationContract]
        Models.File ValidateDataFile(Models.File file, string userName);

        /// <summary>
        /// Validar Archivo
        /// </summary>
        /// <param name="fileName">Nombre Archivo</param>
        /// <param name="userName">Nombre Usuario</param>
        /// <param name="template">Plantilla</param>
        /// <returns>Datos Archivo</returns>
        [OperationContract]
        Models.Template ValidateDataTemplate(string fileName, string userName, Models.Template template);


        /// <summary>
        /// Eliminar JSON
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Eliminado Si/No</returns>
        [OperationContract]
        bool DeletePendingOperation(int id);
        /// <summary>
        /// Eliminar Hijos de un JSON
        /// </summary>
        /// <param name="parentId">Id Padre</param>
        /// <returns>Eliminados Si/No</returns>
        [OperationContract]
        bool DeletePendingOperationsByParentId(int parentId);


        [OperationContract]
        object GetValueByField<T>(Field field);

        /// <summary>
        /// Obtener Archivo Por Filtros
        /// </summary>
        /// <param name="fileProcessValue">Filtros</param>
        /// <returns>Archivo</returns>
        [OperationContract]
        File GetFileByFileProcessValue(FileProcessValue fileProcessValue);

        /// <summary>
        /// Crear Archivo Excel
        /// </summary>
        /// <param name="file">Información</param>
        /// <returns>Información</returns>
        [OperationContract]
        string CreateExcelFile(File file);

        [OperationContract]
        List<Validation> ExecuteValidations(List<ValidationIdentificator> validationIdentificators, List<ValidationRegularExpression> validationRegularExpressions);

        /// <summary>
        /// Validar Archivo
        /// </summary>
        /// <param name="file">Archivo</param>
        /// <param name="userName">Nombre Usuario</param>
        /// <returns>Archivo</returns>
        [OperationContract]
        Models.File ValidateFile(Models.File file, string userName);

        /// <summary>
        /// Obtener Datos De Plantillas Relacionados Por Identificador
        /// </summary>
        /// <param name="templates">Plantillas</param>
        /// <returns>Datos</returns>
        List<Models.File> GetDataTemplates(List<Models.Template> templates);

        /// <summary>
        /// Metodo de consulta parametro de validacion para definir obligatoriedad de contragarantias
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool EnableCrossGuarantees();

        [OperationContract]
        bool GetEndorsementControlById(int id, int userId);

        [OperationContract]
        bool DeleteEndorsementControl(int id);

        [OperationContract]
        void CreateEndorsementControl(int id, int userId);
		
        /// <summary>
        /// Validar Archivo
        /// </summary>
        /// <param name="fileName">Nombre Archivo</param>
        /// <param name="userName">Nombre Usuario</param>
        /// <returns>Datos Archivo</returns>
        [OperationContract]
        Models.Template GetTextFieldsByFileNameUserName(string fileName, string userName, Template template, int typeFile);

        [OperationContract]
        List<Validation> GetValidatedTemporalPolicies(List<Validation> validations, List<ValidationTemporalPolicy> temporalpolicies);

        [OperationContract]
        List<ValidationRegularExpression> GetAllValidationRegularExpressions();
        

        [OperationContract]
        EnumParameter LoadEnumParameterValuesFromDB();

        //[OperationContract]
        //EnumParameter GetEnumParameterValue<T>(T key) where T : struct;
        
    }

}
