using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Sistran.Co.Previsora.Application.FullServices.Models;
using Sybase.Data.AseClient;
using Sistran.Co.Previsora.Application.FullServicesProvider.BussinesLayer;
using Sistran.Co.Previsora.Application.FullServicesProvider.DataLayer;
using Sistran.Co.Previsora.Application.FullServicesProvider.Helpers;
using Sistran.Co.Previsora.Application.FullServices.DTOs;
using ExtensionMethods;
using System.Configuration;
using System.Transactions;
using System.Net.Mail;
using System.Net;
using System.Collections;
using System.Data.Common;

namespace Sistran.Co.Previsora.Application.FullServicesProvider.Provider
{
    public class GSDtoUser
    {
        #region constructores

        public GSDtoUser()
        {

        }

        public GSDtoUser(string user, int idApp, int idRol)
        {
            User = user;
            IdApp = idApp;
            IdRol = idRol;
        }

        #endregion

        #region Propiedades

        private string PrefixConn = ConfigurationManager.AppSettings["SUPAPP"];
        private string PrefixConn2 = ConfigurationManager.AppSettings["SUPAPP2"];
        private static string User { get; set; }
        private static int IdApp { get; set; }
        private static int IdRol { get; set; }
        private static string Prefix2G { get; set; }
        string IsSave3G;
        GetDto getDto = new GetDto(IdRol);
        DataGenericExecute DGeneric;

        #endregion

        #region Funciones y Procedimientos

        public DtoUser GetDtoUser(int id_rol, string cod_rol, string entity)
        {
            DtoUser dtoUser = new DtoUser();

            //Carga el Dto solo con una entidad especifica
            if (entity != "")
            {
                dtoUser = GetEntity(id_rol, cod_rol, entity);
            }
            else
            {
                //Carga el Dto Totalmente nuevo
                if (cod_rol == "new")
                    dtoUser = getDto.DtoUser();
                //Carga el Dto con toda la informacion del Rol buscado
                else
                {
                    dtoUser = GetDto(id_rol, cod_rol);
                }
            }
            return dtoUser;
        }

        private DtoUser GetDto(int id_rol, string cod_rol)
        {
            DtoUser dtoUser = new DtoUser();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataTable DtRolesAPP = DGeneric.GetListRolApp(IdRol);
            string App = string.Empty;

            foreach (DataRow dr in DtRolesAPP.Rows)
            {
                App = dr[0].ToString();
                switch (App)
                {
                    case "SISE2G":
                        GetDtoSISE2G(id_rol, cod_rol, ref dtoUser);
                        break;

                    case "SISE3G":
                        GetDtoSISE3G(id_rol, cod_rol, ref dtoUser);
                        break;
                }
            }

            return dtoUser;
        }

        private void GetDtoSISE2G(int id_rol, string cod_rol, ref DtoUser dtoUser)
        {
            int id_persona = getDto.id_persona(id_rol, cod_rol);
            dtoUser.dtoDataUser = getDto.dtoDataUser(cod_rol);
            dtoUser.dtoDataPerson = getDto.dtoDataPerson(id_persona);
            dtoUser.List_Mpersona_dir = getDto.List_Mpersona_dir(id_persona);
            dtoUser.List_Mpersona_telef = getDto.List_Mpersona_telef(id_persona);
            dtoUser.List_Mpersona_email = getDto.List_Mpersona_email(id_persona);
            dtoUser.List_Logbook = getDto.List_Logbook(id_persona, id_rol);
            dtoUser.dtoSarlaft = getDto.dtoSarlaft(id_persona);
            dtoUser.List_Tusuario_limites = getDto.List_Tusuario_limites(cod_rol);
            dtoUser.List_Tusuario_modulo_imputacion = getDto.List_Tusuario_modulo_imputacion(cod_rol);
            //dtoUser.List_Tusuario_concepto = getDto.List_Tusuario_concepto(cod_rol);
            dtoUser.List_Tusuario_centro_costo = getDto.List_Tusuario_centro_costo(cod_rol);
            dtoUser.List_Tpj_usuarios_email = getDto.List_Tpj_usuarios_email(cod_rol);

        }

        private void GetDtoSISE3G(int id_rol, string cod_rol, ref DtoUser dtoUser)
        {
            //logica de consulta para anexar campos de 3G
            int id_persona = getDto.id_persona(id_rol, cod_rol);
            //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Se obtiene el id de sise 3g directamente de la tabla de equivalencias
            int individualId3G = getDto.ValidateExistPersonIn3G(id_persona);
            if (individualId3G > 0 && id_persona != individualId3G)
            {
                id_persona = individualId3G;
            }
            //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Se obtiene el id de sise 3g directamente de la tabla de equivalencias
            dtoUser.List_Mpersona_dir = getDto.dtoDataAddress3g(id_persona, dtoUser.List_Mpersona_dir);
            dtoUser.List_Mpersona_telef = getDto.dtoDataPhone3g(id_persona, dtoUser.List_Mpersona_telef);
            dtoUser.List_Mpersona_email = getDto.dtoDataMail3g(id_persona, dtoUser.List_Mpersona_email);

        }

        private DtoUser GetEntity(int id_rol, string cod_rol, string entity)
        {
            DtoUser dtoUser = new DtoUser();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataTable DtRolesAPP = DGeneric.GetListRolApp(IdRol);
            string App = string.Empty;

            foreach (DataRow dr in DtRolesAPP.Rows)
            {
                App = dr[0].ToString();
                switch (App)
                {
                    case "SISE2G":
                        GetEntitySISE2G(id_rol, cod_rol, entity, ref dtoUser);
                        break;

                    case "SISE3G":
                        GetEntitySISE3G(id_rol, cod_rol, entity, ref dtoUser);
                        break;

                }
            }

            return dtoUser;
        }

        private void GetEntitySISE2G(int id_rol, string cod_rol, string entity, ref DtoUser dtoUser)
        {
            int id_persona = getDto.id_persona(id_rol, cod_rol);
            switch (entity)
            {
                case "dtoDataPerson":
                    dtoUser.dtoDataPerson = getDto.dtoDataPerson(id_persona);
                    break;
                case "dtoDataUser":
                    dtoUser.dtoDataUser = getDto.dtoDataUser(cod_rol);
                    break;
                case "List_Mpersona_dir":
                    dtoUser.List_Mpersona_dir = getDto.List_Mpersona_dir(id_persona);
                    break;
                case "List_Mpersona_telef":
                    dtoUser.List_Mpersona_telef = getDto.List_Mpersona_telef(id_persona);
                    break;
                case "List_Mpersona_email":
                    dtoUser.List_Mpersona_email = getDto.List_Mpersona_email(id_persona);
                    break;
                case "List_Logbook":
                    dtoUser.List_Logbook = getDto.List_Logbook(id_persona, id_rol);
                    break;
                case "dtoSarlaft":
                    dtoUser.dtoSarlaft = getDto.dtoSarlaft(id_persona);
                    break;
                case "dtoSarlaftForm":
                    dtoUser.dtoSarlaft = getDto.dtoSarlaftForm(Convert.ToInt32(cod_rol));
                    break;
                case "List_Tusuario_limites":
                    dtoUser.List_Tusuario_limites = getDto.List_Tusuario_limites(cod_rol);
                    break;
                case "List_Tusuario_modulo_imputacion":
                    dtoUser.List_Tusuario_modulo_imputacion = getDto.List_Tusuario_modulo_imputacion(cod_rol);
                    break;
                case "List_Tusuario_concepto":
                    dtoUser.List_Tusuario_concepto = getDto.List_Tusuario_concepto(cod_rol);
                    break;
                case "List_Tusuario_centro_costo":
                    dtoUser.List_Tusuario_centro_costo = getDto.List_Tusuario_centro_costo(cod_rol);
                    break;
                case "List_Tpj_usuarios_email":
                    dtoUser.List_Tpj_usuarios_email = getDto.List_Tpj_usuarios_email(cod_rol);
                    break;
            }
        }

        private void GetEntitySISE3G(int id_rol, string cod_rol, string entity, ref DtoUser dtoUser)
        {
            //aqui codigo de otra base de datos llenando campos extras
            int id_persona = getDto.id_persona(id_rol, cod_rol);
            //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Se obtiene el id de sise 3g directamente de la tabla de equivalencias
            int individualId3G = getDto.ValidateExistPersonIn3G(id_persona);
            if (individualId3G > 0 && id_persona != individualId3G)
            {
                id_persona = individualId3G;
            }
            //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Se obtiene el id de sise 3g directamente de la tabla de equivalencias
            switch (entity)
            {

                case "List_Mpersona_dir":
                    dtoUser.List_Mpersona_dir = getDto.dtoDataAddress3g(id_persona, dtoUser.List_Mpersona_dir);
                    break;
                case "List_Mpersona_telef":
                    dtoUser.List_Mpersona_telef = getDto.dtoDataPhone3g(id_persona, dtoUser.List_Mpersona_telef);
                    break;

                case "List_Mpersona_email":
                    dtoUser.List_Mpersona_email = getDto.dtoDataMail3g(id_persona, dtoUser.List_Mpersona_email);
                    break;
            }

        }

        public SUPMessages SetDtoUser(DtoMaster dtoMaster)
        {
            DGeneric = new DataGenericExecute(PrefixConn);
            DataTable DtRolesAPP = DGeneric.GetListRolApp(IdRol);
            string App = string.Empty;
            string cod_rol = string.Empty;
            int id_persona = 0;
            SUPMessages _SUPMessages = new SUPMessages();
            List<TableMessage> ListTableMessage = new List<TableMessage>();
            SetDto setDto; //Para guardado de datos basicos
            int individualId3G; //Para guardado de datos basicos
            foreach (DataRow dr in DtRolesAPP.Rows)
            {
                App = dr[0].ToString();

                switch (App)
                {
                    case "SISE2G":
                        //Autor: Edward Rubiano; Fecha: 10/05/2016; Asunto: C11226; Descripcion: Validaciones para guardado unico de datos basicos
                        if (dtoMaster.GlobalOperation != "UpdateD")
                        {
                            SetSISE2G(dtoMaster, App, ListTableMessage, ref cod_rol, ref id_persona);
                        }
                        else
                        {
                            setDto = new SetDto(User, App, IdRol);
                            ListTableMessage = setDto.UpdateBasicDataInSise2G(dtoMaster.DtoUser.dtoDataUser.mpersona, ListTableMessage); //UPDATE mpersona
                            //Se valida que el unico rol que posee es de SISE 2G, de ser asi se actualizan los datos basicos en la data de SISE 3G
                            if (DtRolesAPP.Rows.Count == 1)
                            {
                                individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoUser.dtoDataUser.mpersona.id_persona);
                                if (individualId3G > 0)
                                {
                                    ListTableMessage = setDto.UpdateBasicDataInSise3G(dtoMaster.DtoUser.dtoDataUser.mpersona, individualId3G, ListTableMessage); //UPDATE UP.PERSON || UP.COMPANY, UP.CO_COMPANY

                                }
                            }
                        }
                        //Autor: Edward Rubiano; Fecha: 10/05/2016;
                        _SUPMessages.cod_rol = cod_rol.ToString();
                        _SUPMessages.id_persona = id_persona.ToString();
                        break;

                    case "SISE3G":
                        //Autor: Edward Rubiano; Fecha: 10/05/2016; Asunto: C11226; Descripcion: Validaciones para guardado unico de datos basicos
                        if (dtoMaster.GlobalOperation != "UpdateD")
                        {
                            if (cod_rol != "-1")
                                SetSISE3G(dtoMaster, PrefixConn2, ListTableMessage, ref cod_rol, ref id_persona);
                        }
                        else
                        {
                            setDto = new SetDto(User, App, IdRol);
                            individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoUser.dtoDataUser.mpersona.id_persona);
                            if (individualId3G > 0)
                            {
                                ListTableMessage = setDto.UpdateBasicDataInSise3G(dtoMaster.DtoUser.dtoDataUser.mpersona, individualId3G, ListTableMessage);

                            }
                        }
                        //Autor: Edward Rubiano; Fecha: 10/05/2016; Asunto: C11226; Descripcion: Validaciones para guardado unico de datos basicos
                        _SUPMessages.INDIVIDUAL_ID = id_persona.ToString();
                        break;
                }
            }

            SendMail(ListTableMessage, dtoMaster);

            _SUPMessages.cod_rol = cod_rol.ToString();
            _SUPMessages.ListMessages = ListTableMessage;

            return _SUPMessages;
        }

        private List<TableMessage> SendMail(List<TableMessage> ListTableMessage, DtoMaster dtoMaster)
        {

            if (dtoMaster.DtoUser.dtoDataUser.tusuario.State.Equals('C') ||
               (dtoMaster.DtoUser.dtoDataUser.tusuario.State.Equals('U') && dtoMaster.DtoUser.dtoDataUser.tusuario.Restart_Password == true))
            {
                TableMessage tableMessage = new TableMessage();
                tableMessage.NameTable = "Envio Correo";
                tableMessage.Message = "True";

                /*TableMessage result2G = ListTableMessage.Find(delegate(TableMessage bk) { return bk.NameTable == "Login2G"; });
                string IsSave2G = result2G.Message;

                TableMessage result3G = ListTableMessage.Find(delegate(TableMessage bk) { return bk.NameTable == "UNIQUE_USER_LOGIN"; });
               
                if (result3G != null)
                {
                    IsSave3G = result3G.Message;
                }*/

                List<MailAddress> lstEmails = new List<MailAddress>();
                bool MailIsSend = false;
                if (dtoMaster.DtoUser.List_Mpersona_email != null)
                {
                    foreach (Mpersona_email em in dtoMaster.DtoUser.List_Mpersona_email)
                        lstEmails.Add(new MailAddress(em.txt_direccion_email, em.txt_direccion_email));

                    string SubjectMessage = "Cambio/Asignación de su contraseña de SISE";
                    string username = dtoMaster.DtoUser.dtoDataUser.tusuario.cod_usuario;
                    string password = Decrypt2G(dtoMaster.DtoUser.dtoDataUser.tusuario.txt_password);
                    string Systems = string.Empty;
                    /*if (IsSave2G == "True") Systems = " SISE2G ";
                    if (IsSave3G == "True") Systems = Systems + "- SISE3G ";*/
                    string Body = BodyMail(username, password, Systems);
                    MailIsSend = sendMail(lstEmails, SubjectMessage, Body, null, ref tableMessage);
                }

                ListTableMessage.Add(tableMessage);
            }

            return ListTableMessage;
        }

        public static string BodyMail(string user_name, string password, string systems)
        {
            StringBuilder Body = new StringBuilder();
            Body.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html><body>");
            Body.Append("<h2>Se ha realizado el cambio/asignación de su contraseña de SISE!</h2>");
            Body.Append("<p><strong>Usuario : </strong> " + user_name + " <p>");
            Body.Append("<p><strong>Contraseña Nueva : </strong> " + password + "<p>");
            Body.Append("Usted podrá acceder al sistema una vez finalice la configuración de su usuario.");
            /*Body.Append("<p><strong>Sistemas : </strong> " + systems + "<p>");*/
            Body.Append("<p>Recuerde cambiar su contraseña</p><BR>");
            Body.Append("Este mensaje ha sido generado automáticamente por favor no responda este correo.");
            Body.Append("</body></html>");
            return Body.ToString();
        }

        public static bool sendMail(List<MailAddress> To, string SubjectMessage, string BodyMessage, ArrayList FilePathList, ref TableMessage tableMessage)
        {
            bool bSendMail = false;
            string Message = string.Empty;
            try
            {

                //System.IO.File.WriteAllText(@"C:\Users\guzmanju\Music\mail.txt", BodyMessage);
                //Objetos SMTP para envio de mensaje
                string SmtpServer = ConfigurationManager.AppSettings.Get("SMTPServer"); //Servidor SMTP.
                int PortNum = Convert.ToInt32(ConfigurationManager.AppSettings.Get("SMTPServerPort"));
                //Puerto Servidor SMTP.
                string strUserName = ConfigurationManager.AppSettings.Get("SenderEMail"); //UserName
                string strSenderName = ConfigurationManager.AppSettings.Get("SenderName"); //UserName
                string strPassword = ConfigurationManager.AppSettings.Get("SenderEMailPass"); //Password
                string strEmailFrom = ConfigurationManager.AppSettings.Get("EmailFrom");
                bool bolUseDefaultCredential = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("UseDefaultCredential"));
                NetworkCredential Auth = new NetworkCredential(strUserName, strPassword);
                MailAddress From = new MailAddress(strEmailFrom, strSenderName);

                // Se Configura el servidor SMTP.
                SmtpClient SC = new SmtpClient(SmtpServer, PortNum);

                // Se establece en False la Conexión segura ya que el servidor no la soporta.
                SC.EnableSsl = false;
                SC.UseDefaultCredentials = bolUseDefaultCredential;
                // Se Crea el mensaje de email.
                using (MailMessage message = new MailMessage())
                {
                    // Se añade el remitente.
                    message.From = From;

                    // Se añaden los destinatarios.
                    foreach (MailAddress ma in To)
                        message.To.Add(ma);

                    // Se establece el Asunto del email.
                    message.Subject = SubjectMessage;
                    // Se establece el cuerpo del correo.
                    System.Net.Mail.AlternateView htmlView = AlternateView.CreateAlternateViewFromString(BodyMessage, null, "text/html");
                    message.AlternateViews.Add(htmlView);

                    if (FilePathList != null)
                        foreach (Object ObjFilePath in FilePathList)
                            message.Attachments.Add(new Attachment(ObjFilePath.ToString())); // Se agrega el archivo como archivo adjunto.

                    if (SC.UseDefaultCredentials)
                        SC.Credentials = Auth; // Se indican las credenciales para autenticarse.

                    // Se indica el modo de envío.
                    SC.DeliveryMethod = SmtpDeliveryMethod.Network;
                    // Se envia el email.
                    SC.Send(message);

                    bSendMail = true;
                    Message = "True";

                }
            }
            catch (Exception ex)
            {
                Message = "hubo un error al enviar el correo"; //ex.Message;
                bSendMail = false;
            }
            finally
            {
                tableMessage.Message = Message;
            }

            return bSendMail;
        }

        private List<TableMessage> SetSISE2G(DtoMaster dtoMaster, string App, List<TableMessage> ListTableMessage, ref string cod_rolref, ref int id_personaref)
        {
            TableMessage tableMessage = new TableMessage();
            tableMessage.NameTable = "Proceso 2G";
            tableMessage.Message = "True";
            try
            {
                string cod_rol = cod_rolref;
                int id_persona = id_personaref;
                //validamos si existen el id_persona y el cod_rol
                //Buscamos el codigo del Rol
                if (dtoMaster.cod_Rol != "" && dtoMaster.cod_Rol != null)
                {
                    cod_rol = dtoMaster.cod_Rol;
                    cod_rolref = cod_rol;
                    id_persona = getDto.id_persona(IdRol, cod_rol.ToString());
                }
                else
                {
                    if (dtoMaster.DtoUser.dtoDataUser.mpersona != null)
                    {
                        if (dtoMaster.DtoUser.dtoDataUser.mpersona.id_persona == 0)
                        {
                            DGeneric = new DataGenericExecute(App);
                            id_persona = DGeneric.GetConsecutive("mpersona");
                        }
                        else
                        {
                            id_persona = dtoMaster.DtoUser.dtoDataUser.mpersona.id_persona;
                        }
                    }
                    cod_rol = dtoMaster.DtoUser.dtoDataUser.tusuario.cod_usuario;
                    cod_rolref = cod_rol;
                }
                id_personaref = id_persona;
                //Insertamos o Actualizamos id_persona y cod_rol
                if (dtoMaster.DtoUser.dtoDataUser != null)
                {
                    if (dtoMaster.DtoUser.dtoDataUser.mpersona != null)
                        dtoMaster.DtoUser.dtoDataUser.mpersona.Update("id_persona", id_persona);

                    if (dtoMaster.DtoUser.dtoDataUser.tusuario != null)
                        dtoMaster.DtoUser.dtoDataUser.tusuario.Update("cod_usuario", cod_rol);

                    if (dtoMaster.DtoUser.dtoDataUser.log_usuario != null)
                        dtoMaster.DtoUser.dtoDataUser.log_usuario.Update("cod_usuario", cod_rol);

                    if (dtoMaster.DtoUser.dtoDataUser.mpersona_usuario != null)
                        dtoMaster.DtoUser.dtoDataUser.mpersona_usuario.Update("id_persona", id_persona);

                    if (dtoMaster.DtoUser.dtoDataUser.mpersona_usuario != null)
                        dtoMaster.DtoUser.dtoDataUser.mpersona_usuario.Update("cod_usuario", cod_rol);

                    if (dtoMaster.DtoUser.dtoDataUser.mpersona_ciiu != null)
                        dtoMaster.DtoUser.dtoDataUser.mpersona_ciiu.Update("id_persona", id_persona);

                    if (dtoMaster.DtoUser.dtoDataUser.mpersona_requiere_sarlaft != null)
                        dtoMaster.DtoUser.dtoDataUser.mpersona_requiere_sarlaft.Update("id_persona", id_persona);
                }

                if (dtoMaster.DtoUser.List_Mpersona_dir != null)
                    dtoMaster.DtoUser.List_Mpersona_dir.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoUser.List_Mpersona_telef != null)
                    dtoMaster.DtoUser.List_Mpersona_telef.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoUser.List_Mpersona_email != null)
                    dtoMaster.DtoUser.List_Mpersona_email.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoUser.List_Logbook != null)
                    dtoMaster.DtoUser.List_Logbook.Update(c => { c.ID_PERSONA = id_persona; });

                if (dtoMaster.DtoUser.List_Tusuario_limites != null)
                    dtoMaster.DtoUser.List_Tusuario_limites.Update(c => { c.cod_usuario = cod_rol; });

                if (dtoMaster.DtoUser.List_Tusuario_modulo_imputacion != null)
                    dtoMaster.DtoUser.List_Tusuario_modulo_imputacion.Update(c => { c.cod_usuario = cod_rol; });

                if (dtoMaster.DtoUser.List_Tusuario_concepto != null)
                    dtoMaster.DtoUser.List_Tusuario_concepto.Update(c => { c.cod_usuario = cod_rol; });

                if (dtoMaster.DtoUser.List_Tusuario_centro_costo != null)
                    dtoMaster.DtoUser.List_Tusuario_centro_costo.Update(c => { c.cod_usuario = cod_rol; });

                if (dtoMaster.DtoUser.List_Tpj_usuarios_email != null)
                    dtoMaster.DtoUser.List_Tpj_usuarios_email.Update(c => { c.cod_usuario = cod_rol; });

                if (dtoMaster.DtoUser.dtoSarlaft != null)
                {
                    if (dtoMaster.DtoUser.dtoSarlaft.List_detalle != null)
                        dtoMaster.DtoUser.dtoSarlaft.List_detalle.Update(c => { c.id_persona = id_persona; });

                    if (dtoMaster.DtoUser.dtoSarlaft.frm_sarlaft_vinculos != null)
                        dtoMaster.DtoUser.dtoSarlaft.frm_sarlaft_vinculos.Update("id_persona", id_persona);

                    if (dtoMaster.DtoUser.dtoSarlaft.frm_sarlaft_aut_incrementos != null)
                        dtoMaster.DtoUser.dtoSarlaft.frm_sarlaft_aut_incrementos.Update("id_persona", id_persona);

                }

                //Insertamos o Actualizamos la Tabla

                string cnnString = ConfigurationManager.ConnectionStrings[PrefixConn].ToString();

                using (AseConnection cnn = new AseConnection(cnnString))
                {
                    AseCommand Command = new AseCommand();
                    Command.Connection = cnn;
                    cnn.Open();
                    AseTransaction trans;
                    trans = cnn.BeginTransaction();
                    Command.Transaction = trans;
                    try
                    {
                        #region Insert Update
                        SetDto setDto = new SetDto(User, App, IdRol, Command);
                        if (dtoMaster.DtoUser.dtoDataUser != null)
                        {
                            ListTableMessage = setDto.dtoDataUser(dtoMaster.DtoUser.dtoDataUser, ListTableMessage);
                            if (ListTableMessage.Count(c => c.NameTable == "Tusuario" && c.Message != "True") > 0)
                            {
                                if (!dtoMaster.DtoUser.dtoDataUser.tusuario.State.Equals('V') && ListTableMessage.Count(c => c.NameTable == "Tusuario" && c.Message != "True") > 0)
                                {
                                    cod_rolref = "-1";
                                }
                                if (dtoMaster.DtoUser.dtoDataUser.mpersona.State.Equals('V'))
                                {
                                    cod_rolref = "-1";
                                    if (!dtoMaster.DtoUser.dtoDataUser.mpersona.State.Equals('V'))
                                    {
                                        cod_rolref = "-1";
                                    }
                                    else
                                    {
                                        cod_rolref = cod_rol;
                                    }
                                }
                                else
                                {
                                    cod_rolref = cod_rol;
                                }
                            }
                            else
                            {
                                string Conex = GetConn(App, User, Command);
                                int IDbIndex = Conex.IndexOf("db=");
                                string DbComma = Conex.Substring(IDbIndex, 14);
                                //int IDbComma = DbComma.IndexOf(";");
                                DbComma = DbComma.Substring(3);

                                if (dtoMaster.DtoUser.dtoDataUser.tusuario.State.Equals('C'))
                                    ListTableMessage = addlogin(App, dtoMaster.DtoUser.dtoDataUser.tusuario.cod_usuario, dtoMaster.DtoUser.dtoDataUser.tusuario.txt_password, DbComma, "us_english", dtoMaster.DtoUser.dtoDataUser.tusuario.txt_nombre, Conex, ListTableMessage);
                                else if (dtoMaster.DtoUser.dtoDataUser.tusuario.State.Equals('U') && dtoMaster.DtoUser.dtoDataUser.tusuario.Restart_Password == true)
                                {
                                    DataSet pass = passworduserlogin(App, User, Command);
                                    if (pass.Tables[0].Rows.Count > 0)
                                    {
                                        string password = Convert.ToString(pass.Tables[0].Rows[0][0]);
                                        ListTableMessage = cambio_password(App, password, dtoMaster.DtoUser.dtoDataUser.tusuario.txt_password, dtoMaster.DtoUser.dtoDataUser.tusuario.cod_usuario, Conex, ListTableMessage);
                                    }
                                }

                                if (dtoMaster.DtoUser.dtoDataUser.tusuario.State.Equals('U') && dtoMaster.DtoUser.dtoDataUser.tusuario.sn_habilitado.Equals("0"))
                                {
                                    DataSet ultimo = ultimo_perfiluser(App, dtoMaster.DtoUser.dtoDataUser.tusuario.cod_usuario, User, Command);
                                    if (ultimo.Tables[0].Rows.Count > 0)
                                    {
                                        string ultimo_perfil = Convert.ToString(ultimo.Tables[0].Rows[0][0]);
                                        ListTableMessage = retiro_usuario(App, dtoMaster.DtoUser.dtoDataUser.tusuario.cod_usuario, User, ultimo_perfil, Conex, ListTableMessage);
                                    }
                                }

                            }
                        }

                        if (cod_rolref != "-1")
                        {
                            if (dtoMaster.DtoUser.List_Mpersona_dir != null)
                                ListTableMessage = setDto.List_Mpersona_dir(dtoMaster.DtoUser.List_Mpersona_dir, ListTableMessage);

                            if (dtoMaster.DtoUser.List_Mpersona_telef != null)
                                ListTableMessage = setDto.List_Mpersona_telef(dtoMaster.DtoUser.List_Mpersona_telef, ListTableMessage);

                            if (dtoMaster.DtoUser.List_Mpersona_email != null)
                                ListTableMessage = setDto.List_Mpersona_email(dtoMaster.DtoUser.List_Mpersona_email, ListTableMessage);

                            if (dtoMaster.DtoUser.List_Logbook != null)
                                ListTableMessage = setDto.List_Logbook(dtoMaster.DtoUser.List_Logbook, ListTableMessage);

                            if (dtoMaster.DtoUser.dtoSarlaft != null)
                                ListTableMessage = setDto.dtoSarlaft(dtoMaster.DtoUser.dtoSarlaft, ListTableMessage);

                            if (dtoMaster.DtoUser.List_Tusuario_limites != null)
                                ListTableMessage = setDto.List_Tusuario_limites(dtoMaster.DtoUser.List_Tusuario_limites, ListTableMessage);

                            if (dtoMaster.DtoUser.List_Tusuario_modulo_imputacion != null && dtoMaster.DtoUser.List_Tusuario_modulo_imputacion.Count > 0)
                                ListTableMessage = setDto.List_Tusuario_modulo_imputacion(dtoMaster.DtoUser.List_Tusuario_modulo_imputacion, ListTableMessage);

                            if (dtoMaster.DtoUser.List_Tusuario_concepto != null && dtoMaster.DtoUser.List_Tusuario_concepto.Count > 0)
                                ListTableMessage = setDto.List_Tusuario_concepto(dtoMaster.DtoUser.List_Tusuario_concepto, ListTableMessage);

                            if (dtoMaster.DtoUser.List_Tusuario_centro_costo != null && dtoMaster.DtoUser.List_Tusuario_centro_costo.Count > 0)
                                ListTableMessage = setDto.List_Tusuario_centro_costo(dtoMaster.DtoUser.List_Tusuario_centro_costo, ListTableMessage);

                            if (dtoMaster.DtoUser.List_Tpj_usuarios_email != null)
                                ListTableMessage = setDto.List_Tpj_usuarios_email(dtoMaster.DtoUser.List_Tpj_usuarios_email, ListTableMessage);

                            if (
                                    (
                                        (dtoMaster.DtoUser.List_Mpersona_dir != null && dtoMaster.DtoUser.List_Mpersona_dir.Count > 0) ||
                                        (dtoMaster.DtoUser.List_Mpersona_telef != null && dtoMaster.DtoUser.List_Mpersona_telef.Count > 0) ||
                                        (dtoMaster.DtoUser.List_Mpersona_email != null && dtoMaster.DtoUser.List_Mpersona_email.Count > 0)
                                    )
                                )
                            {
                                ListTableMessage = setDto.set_Juridical_Person_Register(id_persona, ListTableMessage);
                            }
                        }
                        #endregion

                        if (ListTableMessage.Count(c => c.Message != "True") > 0)
                        {
                            tableMessage.Message = "Proceso 2G Fallido";
                            Command.Transaction.Rollback();
                            Command.Dispose();
                        }
                        else
                        {
                            Command.Transaction.Commit();
                            Command.Dispose();
                        }
                    }
                    catch
                    {
                        Command.Transaction.Rollback();
                        Command.Dispose();
                        tableMessage.Message = "Proceso 2G Fallido";
                    }
                }
            }
            catch (Exception errSup)
            {
                tableMessage.Message = errSup.Message;
            }
            finally
            {
                ListTableMessage.Add(tableMessage);
            }

            return ListTableMessage;
        }

        private string GetConn(string App, string User, AseCommand command)
        {
            try
            {
                string newConne = string.Empty;
                //paso 1 llamar al procedemiento y traer el campo txt_passwor
                //paso 2 con el algoritmo desencriptar el password           
                string cnnString = ConfigurationManager.ConnectionStrings[PrefixConn].ToString();
                //paso3 realizar un renplace usuario y con el nuevo password 
                DGeneric = new DataGenericExecute(App);
                List<Parameters> listParameter = new List<Parameters>();
                Parameters parameter = new Parameters();
                parameter.ParameterType = "AseDbType.VarChar";
                parameter.Parameter = "cod_usuario";
                parameter.Value = (User.ToUpper());
                listParameter.Add(parameter);
                DataSet passwd = DGeneric.ExecuteStoreProcedure("SUP.GET_USER_PASSWORD", listParameter, command);
                if (passwd.Tables[0].Rows.Count > 0)
                {
                    string password = Convert.ToString(passwd.Tables[0].Rows[0][0]);
                    string PassEncrypt = String.Empty;
                    //PassEncrypt = Decrypt(password);

                    int IuserIndex = cnnString.IndexOf("User ID=");
                    string UserText = cnnString.Substring(IuserIndex + 8);
                    int IUserComma = UserText.IndexOf(";");
                    UserText = UserText.Substring(0, IUserComma);

                    int IPassIndex = cnnString.IndexOf("Password=");
                    string PassText = cnnString.Substring(IPassIndex + 9);
                    int IPassComma = PassText.IndexOf(";");
                    PassText = PassText.Substring(0, IPassComma);


                    newConne = cnnString.Replace("ID=" + UserText, "ID=" + User.ToLower());
                    newConne = newConne.Replace("Password=" + PassText, "Password=" + password);
                    newConne = newConne + "EncryptPassword=1;";

                }

                return newConne;
            }
            catch (Exception ex)
            {
                throw new SupException("Conexion::Error occured.", ex);
            }
        }


        private static string Decrypt(string txtpassw)
        {
            string Password = "Ña25%fy&";
            string p = "", b = "", S = "";
            int J = 0;
            int A1 = 0, A2 = 0, A3 = 0;

            for (var i = 0; i < Password.Length; i++)
            {
                p = p + System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(Password.Substring(i, 1))[0];
            }

            for (var i = 0; i < txtpassw.Length; i = i + 2)
            {
                A1 = System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(p.Substring(J, 1))[0];
                J = (J > p.Length ? 1 : J + 1);
                b = txtpassw.Substring(i, 2);
                A3 = Convert.ToInt32(b, 16);
                A2 = A1 ^ A3;
                S = S + (char)A2;
            }
            return S;
        }

        private List<TableMessage> addlogin(string App, string loginame, string passwd, string defdb, string deflanguage, string fullname, string Conex, List<TableMessage> ListTableMessage)
        {
            TableMessage tMessage = new TableMessage();
            tMessage.Message = "True";
            tMessage.NameTable = "Login2G";
            try
            {

                DGeneric = new DataGenericExecute(App);
                List<Parameters> listParameter = new List<Parameters>();
                Parameters parameter = new Parameters();
                parameter.ParameterType = "AseDbType.VarChar";
                parameter.Parameter = "loginame";
                parameter.Value = loginame.ToLower();
                listParameter.Add(parameter);
                parameter = new Parameters();
                parameter.ParameterType = "AseDbType.VarChar";
                parameter.Parameter = "passwd";
                parameter.Value = passwd;
                listParameter.Add(parameter);
                parameter = new Parameters();
                parameter.ParameterType = "AseDbType.VarChar";
                parameter.Parameter = "defdb";
                parameter.Value = defdb;
                listParameter.Add(parameter);
                parameter = new Parameters();
                parameter.ParameterType = "AseDbType.VarChar";
                parameter.Parameter = "deflanguage";
                parameter.Value = deflanguage;
                listParameter.Add(parameter);
                parameter = new Parameters();
                parameter.ParameterType = "AseDbType.VarChar";
                parameter.Parameter = "fullname";
                parameter.Value = fullname;
                listParameter.Add(parameter);
                tMessage.Message = (DGeneric.ExecuteStoreProcedureScalar("sp_addlogin", listParameter, Conex)).ToString();

            }
            catch (SupException ex)
            {
                tMessage.Message = ex.Source;
            }
            finally
            {
                ListTableMessage.Add(tMessage);
            }

            return ListTableMessage;
        }

        private DataSet passworduserlogin(string App, string User, AseCommand command)
        {
            DGeneric = new DataGenericExecute(App);
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.VarChar";
            parameter.Parameter = "cod_usuario";
            parameter.Value = (User.ToUpper());
            listParameter.Add(parameter);
            DataSet passwd = DGeneric.ExecuteStoreProcedure("SUP.GET_USER_PASSWORD", listParameter, command);
            return passwd;
        }

        private int adduser(string App, string loginame, string name_in_db, string grpname, string Conex)
        {
            DGeneric = new DataGenericExecute(App);
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.VarChar";
            parameter.Parameter = "loginame";
            parameter.Value = loginame.ToLower();
            listParameter.Add(parameter);
            parameter = new Parameters();
            parameter.ParameterType = "AseDbType.VarChar";
            parameter.Parameter = "name_in_db";
            parameter.Value = loginame.ToLower();
            listParameter.Add(parameter);
            parameter = new Parameters();
            parameter.ParameterType = "AseDbType.VarChar";
            parameter.Parameter = "grpname";
            parameter.Value = grpname;
            listParameter.Add(parameter);
            //int iReturn = DGeneric.ExecuteStoreProcedureScalar("sp_adduser", listParameter, Conex);
            return 0;
        }

        private List<TableMessage> cambio_password(string App, string pwd1, string pwd2, string login1, string Conex, List<TableMessage> ListTableMessage)
        {
            TableMessage tMessage = new TableMessage();
            tMessage.Message = "True";
            tMessage.NameTable = "Login2G";
            try
            {

                DGeneric = new DataGenericExecute(App);
                List<Parameters> listParameter = new List<Parameters>();
                Parameters parameter = new Parameters();
                parameter.ParameterType = "AseDbType.VarChar";
                parameter.Parameter = "pwd1";
                parameter.Value = pwd1;
                listParameter.Add(parameter);
                parameter = new Parameters();
                parameter.ParameterType = "AseDbType.VarChar";
                parameter.Parameter = "pwd2";
                parameter.Value = pwd2;
                listParameter.Add(parameter);
                parameter = new Parameters();
                parameter.ParameterType = "AseDbType.VarChar";
                parameter.Parameter = "login1";
                parameter.Value = login1.ToLower();
                listParameter.Add(parameter);
                tMessage.Message = (DGeneric.ExecuteStoreProcedureScalar("spu_cambio_password_syb", listParameter, Conex)).ToString();

            }
            catch (SupException ex)
            {
                tMessage.Message = ex.Source;
            }
            finally
            {
                ListTableMessage.Add(tMessage);
            }

            return ListTableMessage;
        }


        private DataSet ultimo_perfiluser(string App, string cod_usuario, string usuario_modifica, AseCommand command)
        {
            DGeneric = new DataGenericExecute(App);
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.VarChar";
            parameter.Parameter = "cod_usuario";
            parameter.Value = cod_usuario;
            listParameter.Add(parameter);
            parameter = new Parameters();
            parameter.ParameterType = "AseDbType.VarChar";
            parameter.Parameter = "usuario_modifica";
            parameter.Value = usuario_modifica;
            listParameter.Add(parameter);
            DataSet ds = DGeneric.ExecuteStoreProcedure("SUP.SP_RETIRO_USUARIO", listParameter, command);
            return ds;
        }

        private List<TableMessage> retiro_usuario(string App, string cod_usuario, string usuario_modifica, string ultimo_perfil, string Conex, List<TableMessage> ListTableMessage)
        {
            TableMessage tMessage = new TableMessage();
            tMessage.Message = "True";
            tMessage.NameTable = "sp_perfil_retiro_usuario";

            try
            {
                DGeneric = new DataGenericExecute(App);
                List<Parameters> listParameter = new List<Parameters>();
                Parameters parameter = new Parameters();
                /*parameter.ParameterType = "AseDbType.VarChar";
                parameter.Parameter = "cod_usuario";
                parameter.Value = cod_usuario;
                listParameter.Add(parameter);
                parameter = new Parameters();
                parameter.ParameterType = "AseDbType.VarChar";
                parameter.Parameter = "usuario_modifica";
                parameter.Value = usuario_modifica;
                listParameter.Add(parameter);
                parameter = new Parameters();
                parameter.ParameterType = "AseDbType.VarChar";
                parameter.Parameter = "ultimo_perfil";
                parameter.Value = ultimo_perfil;
                listParameter.Add(parameter);
                tMessage.Message = (DGeneric.ExecuteStoreProcedureNonQuery("SUP.get_retiro_usuario", listParameter, Conex)).ToString();*/


                TableMessage tMessage1 = new TableMessage();
                tMessage1.Message = "True";
                tMessage1.NameTable = "sp_locklogin";
                try
                {

                    listParameter.Clear();
                    parameter = new Parameters();
                    parameter.ParameterType = "AseDbType.VarChar";
                    parameter.Parameter = "loginame";
                    parameter.Value = cod_usuario.ToLower();
                    listParameter.Add(parameter);
                    parameter = new Parameters();
                    parameter.ParameterType = "AseDbType.VarChar";
                    parameter.Parameter = "locktype";
                    parameter.Value = "lock";
                    listParameter.Add(parameter);

                    tMessage1.Message = (DGeneric.ExecuteStoreProcedureScalar("sp_locklogin", listParameter, Conex)).ToString();
                }
                catch (SupException ex)
                {
                    tMessage1.Message = ex.Source;
                }
                finally
                {
                    ListTableMessage.Add(tMessage1);
                }

            }
            catch (SupException ex)
            {
                tMessage.Message = ex.Source;
            }
            finally
            {
                ListTableMessage.Add(tMessage);
            }

            return ListTableMessage;
        }

        private List<TableMessage> SetSISE3G(DtoMaster dtoMaster, string App, List<TableMessage> ListTableMessage, ref string cod_rolref, ref int id_personaref)
        {

            if (ListTableMessage.Count(c => c.Message != "True") == 0)
            {
                TableMessage tableMessage = new TableMessage();
                TableMessage tableMessage2G = new TableMessage();
                tableMessage.NameTable = "Proceso 3G";
                tableMessage.Message = "True";
                try
                {

                    string cod_rol = cod_rolref;
                    int id_persona = id_personaref;
                    DGeneric = new DataGenericExecute(App);
                    string cod_tipo_persona = string.Empty;
                    string cod_tipo_doc = string.Empty;
                    string nro_doc = string.Empty;
                    int individual_id = 0;
                    int id = 0;
                    char state_psr = 'C';
                    char state_usr = 'C';
                    char state_individual = 'C';
                    string IsSave = string.Empty;
                    DataSet ds = new DataSet();
                    int userid = 0;
                    int? iduseru = 0;

                    string cnnString = ConfigurationManager.ConnectionStrings[PrefixConn2].ToString();

                    using (AseConnection cnn = new AseConnection(cnnString))
                    {
                        AseCommand Command = new AseCommand();
                        Command.Connection = cnn;
                        cnn.Open();
                        AseTransaction trans;
                        trans = cnn.BeginTransaction();
                        Command.Transaction = trans;
                        try
                        {
                            SetDto setDto = new SetDto(User, App, IdRol, Command);

                            DGeneric = new DataGenericExecute(PrefixConn);
                            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_USER_DATA", cod_rol, "cod_usuario");

                            if (DsReturn.Tables.Count > 0)
                            {
                                if (DsReturn.Tables[0].Rows.Count > 0)
                                {
                                    cod_tipo_persona = Convert.ToString(DsReturn.Tables[0].Rows[0][0]);
                                    cod_tipo_doc = Convert.ToString(DsReturn.Tables[0].Rows[0][1]);
                                    nro_doc = Convert.ToString(DsReturn.Tables[0].Rows[0][2]);

                                    ds = new DataSet();
                                    ds = statefor3g(App, cod_tipo_persona == "F" ? "1" : "2", cod_tipo_doc, nro_doc, cod_rol,id_persona);

                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        individual_id = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                                        if (individual_id != 0) { id_persona = individual_id; } //Edward Rubiano -- HD3500 -- 09/11/2015
                                        id = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                                        state_psr = Convert.ToChar(ds.Tables[0].Rows[0][2]);
                                        state_usr = Convert.ToChar(ds.Tables[0].Rows[0][3]);
                                    }
                                }
                            }

                            #region dtoMaster.DtoUser.dtoDataUser != null'
                            if (dtoMaster.DtoUser.dtoDataUser != null)
                            {
                                if (dtoMaster.DtoUser.dtoDataUser.mpersona != null)
                                {
                                    if (!dtoMaster.DtoUser.dtoDataUser.mpersona.State.Equals('V'))
                                    {
                                        INDIVIDUAL _INDIVIDUAL = new INDIVIDUAL();
                                        _INDIVIDUAL.State = state_psr;
                                        if (!state_psr.Equals('U'))
                                        {
                                            _INDIVIDUAL.INDIVIDUAL_ID = id_persona;
                                        }

                                        if (id_persona == individual_id)
                                        {
                                            _INDIVIDUAL.INDIVIDUAL_ID = id_persona;
                                        }
                                        else
                                        {
                                            _INDIVIDUAL.INDIVIDUAL_ID = individual_id;
                                        }

                                        _INDIVIDUAL.INDIVIDUAL_TYPE_CD = (dtoMaster.DtoUser.dtoDataUser.mpersona.cod_tipo_persona == "F" ? 1 : 2);
                                        //campos a validar de donde se sacan
                                        if (_INDIVIDUAL.State == 'C')
                                            _INDIVIDUAL.AT_DATA_ID = 0;
                                        else if (_INDIVIDUAL.State == 'U')
                                        {
                                            DatatableToList Dtl = new DatatableToList();
                                            List<Parameters> listParameter = new List<Parameters>();
                                            Parameters parameter = new Parameters();
                                            parameter.ParameterType = "AseDbType.Integer";
                                            parameter.Parameter = "id_persona";

                                            if (id_persona == individual_id)
                                            {
                                                parameter.Value = id_persona.ToString();
                                            }
                                            else
                                            {
                                                parameter.Value = individual_id.ToString();
                                            }

                                            listParameter.Add(parameter);

                                            DGeneric = new DataGenericExecute(PrefixConn);
                                            ds = DGeneric.ExecuteStoreProcedure("SUP.GET_DATAID3G", listParameter, Command);
                                            INDIVIDUAL INDIV = Dtl.ConvertTo<INDIVIDUAL>(ds.Tables[0])[0];
                                            _INDIVIDUAL.AT_DATA_ID = INDIV.AT_DATA_ID;

                                        }

                                        _INDIVIDUAL.AT_PAYMENT_ID = 1;
                                        _INDIVIDUAL.AT_AGENT_AGENCY_ID = 0;
                                        //_INDIVIDUAL.OWNER_ROLE_CD = null;

                                        /*if (dtoMaster.DtoUser.dtoDataUser.mpersona_ciiu != null)
                                        {
                                            _INDIVIDUAL.ECONOMIC_ACTIVITY_CD = Convert.ToString(dtoMaster.DtoUser.dtoDataUser.mpersona_ciiu.cod_ciiu_ppal_nuevo);
                                        }*/
                                        _INDIVIDUAL.ECONOMIC_ACTIVITY_CD = null;

                                        List<INDIVIDUAL> List_INDIVIDUAL = new List<INDIVIDUAL>();
                                        List_INDIVIDUAL.Add(_INDIVIDUAL);
                                        ListTableMessage = setDto.List_INDIVIDUAL(List_INDIVIDUAL, ListTableMessage);

                                        TableMessage result = ListTableMessage.Find(delegate(TableMessage bk) { return bk.NameTable == "INDIVIDUAL"; });
                                        IsSave = result.Message;

                                        if (IsSave == "True")
                                        {
                                            if (dtoMaster.DtoUser.dtoDataUser.mpersona_ciiu != null)
                                            {
                                                CO_PRV_INDIVIDUAL _CO_PRV_INDIVIDUAL = new CO_PRV_INDIVIDUAL();
                                                _CO_PRV_INDIVIDUAL.State = state_psr;
                                                if (!state_psr.Equals('U'))
                                                {
                                                    _CO_PRV_INDIVIDUAL.INDIVIDUAL_ID = id_persona;
                                                }

                                                if (id_persona == individual_id)
                                                {
                                                    _CO_PRV_INDIVIDUAL.INDIVIDUAL_ID = id_persona;
                                                }
                                                else
                                                {
                                                    _CO_PRV_INDIVIDUAL.INDIVIDUAL_ID = individual_id;
                                                }

                                                _CO_PRV_INDIVIDUAL.ECONOMIC_ACTIVITY_CD_NEW = dtoMaster.DtoUser.dtoDataUser.mpersona_ciiu.cod_ciiu_ppal_nuevo;
                                                _CO_PRV_INDIVIDUAL.SECOND_ECONOMIC_ACTIVITY_CD_NEW = dtoMaster.DtoUser.dtoDataUser.mpersona_ciiu.cod_ciiu_scndria_nuevo;

                                                List<CO_PRV_INDIVIDUAL> List_CO_PRV_INDIVIDUAL = new List<CO_PRV_INDIVIDUAL>();
                                                List_CO_PRV_INDIVIDUAL.Add(_CO_PRV_INDIVIDUAL);
                                                ListTableMessage = setDto.List_CO_PRV_INDIVIDUAL(List_CO_PRV_INDIVIDUAL, ListTableMessage);
                                            }

                                            if (dtoMaster.DtoUser.dtoDataUser.mpersona.cod_tipo_persona == "F")
                                            {
                                                _INDIVIDUAL.INDIVIDUAL_TYPE_CD = 1;
                                                PERSON _PERSON = new PERSON();
                                                //Edward Rubiano -- C11256 -- 11/08/2016 -- This validation was added when table Person not exists in 3G because exists in 2G || Esta valilidacion se agrega cuando no existe person en 3G pero si en 2G
                                                if (state_psr == 'C') 
                                                {
                                                    _PERSON.State = state_psr;
                                                }
                                                else
                                                {
                                                //Edward Rubiano -- C11256 -- 11/08/2016
                                                    //Edward Rubiano -- C11256 -- 14/04/2016 -- Usuarios Multiples
                                                    if (dtoMaster.DtoUser.dtoDataUser.mpersona.State == 'U')
                                                    {
                                                        _PERSON.State = 'U';
                                                    }
                                                    else
                                                    {
                                                        _PERSON.State = state_psr;
                                                    }
                                                    //Edward Rubiano -- C11256 -- 14/04/2016 -- Usuarios Multiples
                                                }
                                                //Edward Rubiano -- C11256 -- 14/04/2016 -- Usuarios Multiples
                                                if (dtoMaster.DtoUser.dtoDataUser.mpersona.State == 'U'
                                                    && state_psr != 'C' //Edward Rubiano -- C11256 -- 11/08/2016 -- J
                                                    )
                                                {
                                                    _PERSON.State = 'U';
                                                }
                                                else
                                                { 
                                                    _PERSON.State = state_psr;
                                                }
                                                //Edward Rubiano -- C11256 -- 14/04/2016 -- Usuarios Multiples
                                                if (!state_psr.Equals('U'))
                                                {
                                                    _PERSON.INDIVIDUAL_ID = id_persona;
                                                }

                                                if (id_persona == individual_id)
                                                {
                                                    _PERSON.INDIVIDUAL_ID = id_persona;
                                                }
                                                else
                                                {
                                                    _PERSON.INDIVIDUAL_ID = individual_id;
                                                }

                                                _PERSON.SURNAME = dtoMaster.DtoUser.dtoDataUser.mpersona.txt_apellido1;
                                                _PERSON.NAME = dtoMaster.DtoUser.dtoDataUser.mpersona.txt_nombre;
                                                _PERSON.GENDER = dtoMaster.DtoUser.dtoDataUser.mpersona.txt_sexo;
                                                _PERSON.ID_CARD_TYPE_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoUser.dtoDataUser.mpersona.cod_tipo_doc));
                                                _PERSON.ID_CARD_NO = dtoMaster.DtoUser.dtoDataUser.mpersona.nro_doc;
                                                _PERSON.MARITAL_STATUS_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoUser.dtoDataUser.mpersona.cod_est_civil));
                                                _PERSON.BIRTH_DATE = dtoMaster.DtoUser.dtoDataUser.mpersona.fec_nac == null ? Convert.ToString(DateTime.Now) : dtoMaster.DtoUser.dtoDataUser.mpersona.fec_nac;
                                                _PERSON.CHILDREN = "0";
                                                _PERSON.MOTHER_LAST_NAME = dtoMaster.DtoUser.dtoDataUser.mpersona.txt_apellido2;

                                                List<PERSON> List_PERSON = new List<PERSON>();
                                                List_PERSON.Add(_PERSON);
                                                ListTableMessage = setDto.List_PERSON(List_PERSON, ListTableMessage);
                                            }
                                            else if (dtoMaster.DtoUser.dtoDataUser.mpersona.cod_tipo_persona == "J")
                                            {
                                                _INDIVIDUAL.INDIVIDUAL_TYPE_CD = 2;
                                                COMPANY _COMPANY = new COMPANY();
                                                _COMPANY.State = state_psr;
                                                _COMPANY.INDIVIDUAL_ID = id_persona;
                                                _COMPANY.TRADE_NAME = dtoMaster.DtoUser.dtoDataUser.mpersona.txt_apellido1 + " " + dtoMaster.DtoUser.dtoDataUser.mpersona.txt_apellido2 + " " + dtoMaster.DtoUser.dtoDataUser.mpersona.txt_nombre;
                                                _COMPANY.TRIBUTARY_ID_TYPE_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoUser.dtoDataUser.mpersona.cod_tipo_doc));
                                                _COMPANY.TRIBUTARY_ID_NO = dtoMaster.DtoUser.dtoDataUser.mpersona.nro_nit;
                                                _COMPANY.COUNTRY_CD = 1;
                                                _COMPANY.COMPANY_TYPE_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoInsured.dtoDataInsured.mpersona.COMPANY_TYPE_CD == "-1" ? "1" : dtoMaster.DtoInsured.dtoDataInsured.mpersona.COMPANY_TYPE_CD));

                                                List<COMPANY> List_COMPANY = new List<COMPANY>();
                                                List_COMPANY.Add(_COMPANY);
                                                ListTableMessage = setDto.List_COMPANY(List_COMPANY, ListTableMessage);

                                                result = ListTableMessage.Find(delegate(TableMessage bk) { return bk.NameTable == "COMPANY"; });
                                                IsSave = result.Message;

                                                if (IsSave == "True")
                                                {
                                                    CO_COMPANY _CO_COMPANY = new CO_COMPANY();
                                                    _CO_COMPANY.State = state_psr;
                                                    _CO_COMPANY.INDIVIDUAL_ID = id_persona;
                                                    _CO_COMPANY.VERIFY_DIGIT = dtoMaster.DtoUser.dtoDataUser.mpersona.VERIFY_DIGIT;
                                                    _CO_COMPANY.ASSOCIATION_TYPE_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoUser.dtoDataUser.mpersona.ASSOCIATION_TYPE_CD));

                                                    List<CO_COMPANY> List_CO_COMPANY = new List<CO_COMPANY>();
                                                    List_CO_COMPANY.Add(_CO_COMPANY);
                                                    ListTableMessage = setDto.List_CO_COMPANY(List_CO_COMPANY, ListTableMessage);
                                                }
                                            }
                                        }
                                    }
                                }

                                if (IsSave == "True")
                                {
                                    if (dtoMaster.DtoUser.dtoDataUser.tusuario != null)
                                    {
                                        if (!dtoMaster.DtoUser.dtoDataUser.tusuario.State.Equals('V'))
                                        {
                                            ds = new DataSet();
                                            ds = iduser(App, User);

                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                userid = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                                            }

                                            UNIQUE_USERS _UNIQUE_USERS = new UNIQUE_USERS();
                                            _UNIQUE_USERS.State = state_usr;
                                            _UNIQUE_USERS.USER_ID = id;
                                            _UNIQUE_USERS.ACCOUNT_NAME = dtoMaster.DtoUser.dtoDataUser.tusuario.cod_usuario;
                                            if (!state_usr.Equals('U'))
                                            {
                                                _UNIQUE_USERS.PERSON_ID = id_persona;
                                            }

                                            if (id_persona == individual_id)
                                            {
                                                _UNIQUE_USERS.PERSON_ID = id_persona;
                                            }
                                            else
                                            {
                                                _UNIQUE_USERS.PERSON_ID = individual_id;
                                            }
                                            _UNIQUE_USERS.AUTHENTICATION_TYPE_CD = 2;
                                            //_UNIQUE_USERS.USER_DOMAIN =  ;
                                            _UNIQUE_USERS.DISABLED_DATE = dtoMaster.DtoUser.dtoDataUser.tusuario.fec_baja;
                                            //_UNIQUE_USERS.LOCK_DATE =  ;
                                            //_UNIQUE_USERS.EXPIRATION_DATE =;
                                            _UNIQUE_USERS.LOCK_PASSWORD = false;
                                            _UNIQUE_USERS.ACTIVATION_DATE = false;
                                            _UNIQUE_USERS.CREATED_DATE = dtoMaster.DtoUser.dtoDataUser.tusuario.fec_alta;
                                            if (state_usr == 'C')
                                            {
                                                _UNIQUE_USERS.CREATED_USER_ID = Convert.ToString(userid);
                                            }
                                            else
                                            {
                                                ds = new DataSet();
                                                ds = idusercreate(App, id_persona);

                                                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows.Count == 1)
                                                {
                                                    if (Convert.ToString(ds.Tables[0].Rows[0][0]) == "")
                                                    {
                                                        iduseru = null;
                                                    }
                                                    else
                                                    {
                                                        iduseru = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                                                    }
                                                }
                                                else if (ds.Tables[0].Rows.Count == 2)
                                                {
                                                    if (Convert.ToString(ds.Tables[0].Rows[1][0]) == "")
                                                    {
                                                        iduseru = null;
                                                    }
                                                    else
                                                    {
                                                        iduseru = Convert.ToInt32(ds.Tables[0].Rows[1][0]);
                                                    }
                                                }

                                                _UNIQUE_USERS.CREATED_USER_ID = iduseru.ToString();
                                                _UNIQUE_USERS.MODIFIED_DATE = DateTime.Now.ToString("d");
                                                _UNIQUE_USERS.MODIFIED_USER_ID = Convert.ToString(userid);
                                            }

                                            List<UNIQUE_USERS> List_UNIQUE_USERS = new List<UNIQUE_USERS>();
                                            List_UNIQUE_USERS.Add(_UNIQUE_USERS);
                                            ListTableMessage = setDto.List_UNIQUE_USERS(List_UNIQUE_USERS, ListTableMessage);

                                            TableMessage result = ListTableMessage.Find(delegate(TableMessage bk) { return bk.NameTable == "UNIQUE_USERS"; });
                                            IsSave = result.Message;

                                            if (IsSave == "True")
                                            {
                                                if (dtoMaster.DtoUser.dtoDataUser.tusuario.State.Equals('C') ||
                                                    (dtoMaster.DtoUser.dtoDataUser.tusuario.State.Equals('U') && dtoMaster.DtoUser.dtoDataUser.tusuario.Restart_Password == true
                                                    || dtoMaster.DtoUser.dtoDataUser.tusuario.State.Equals('U')))
                                                {
                                                    // Grabar el Password del Usuario
                                                    string password = Decrypt2G(dtoMaster.DtoUser.dtoDataUser.tusuario.txt_password);
                                                    String password3G = Encrypt3G(password);
                                                    UNIQUE_USER_LOGIN _UNIQUE_USER_LOGIN = new UNIQUE_USER_LOGIN();
                                                    _UNIQUE_USER_LOGIN.State = state_usr;
                                                    _UNIQUE_USER_LOGIN.USER_ID = id;
                                                    _UNIQUE_USER_LOGIN.PASSWORD = password3G;
                                                    _UNIQUE_USER_LOGIN.PASSWORD_EXPIRATION_DATE = dtoMaster.DtoUser.dtoDataUser.tusuario.fec_vto_password;
                                                    _UNIQUE_USER_LOGIN.PASSWORD_EXPIRATION_DAYS = dtoMaster.DtoUser.dtoDataUser.tusuario.cnt_dias_validez_pwd;
                                                    _UNIQUE_USER_LOGIN.PASSWORD_NEVER_EXPIRE = false;
                                                    _UNIQUE_USER_LOGIN.MUST_CHANGE_PASSWORD = false;
                                                    _UNIQUE_USER_LOGIN.CAN_CHANGE_PASSWORD = true;

                                                    List<UNIQUE_USER_LOGIN> List_UNIQUE_USER_LOGIN = new List<UNIQUE_USER_LOGIN>();
                                                    List_UNIQUE_USER_LOGIN.Add(_UNIQUE_USER_LOGIN);
                                                    ListTableMessage = setDto.List_UNIQUE_USER_LOGIN(List_UNIQUE_USER_LOGIN, ListTableMessage);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion//fin dtoMaster.DtoUser.dtoDataUser != null

                            #region Datos Complementarios

                            ds = new DataSet();
                            if (id_persona == individual_id)
                            {
                                ds = useridperson(App, Convert.ToString(id_persona));
                            }
                            else
                            {
                                ds = useridperson(App, Convert.ToString(individual_id));
                            }
                            

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                state_individual = Convert.ToChar(ds.Tables[0].Rows[0][0]);
                                if (state_individual.Equals('U'))
                                    IsSave = "True";
                            }

                            //Validacion Almacenamiento Direccion
                            if (dtoMaster.DtoUser.List_Mpersona_dir != null && IsSave == "True")
                            {
                                List<ADDRESS> List_ADDRESS = new List<ADDRESS>();

                                foreach (Mpersona_dir dir in dtoMaster.DtoUser.List_Mpersona_dir)
                                {
                                    if (Convert.ToInt32(dir.cod_tipo_dir) != 99)
                                    {
                                        ADDRESS _ADDRESS = new ADDRESS();
                                        _ADDRESS.State = dir.State_3g;

                                        if (id_persona == individual_id)
                                        {
                                            _ADDRESS.INDIVIDUAL_ID = id_persona;
                                        }
                                        else
                                        {
                                            _ADDRESS.INDIVIDUAL_ID = individual_id;
                                        }

                                        _ADDRESS.DATA_ID = dir.DATA_ID;
                                        _ADDRESS.ADDRESS_TYPE_CD = Convert.ToInt32(dir.cod_tipo_dir);
                                        _ADDRESS.STREET_TYPE_CD = 1;
                                        _ADDRESS.STREET = dir.txt_direccion;
                                        _ADDRESS.ZIP_CODE = dir.nro_cod_postal;
                                        _ADDRESS.COUNTRY_CD = dir.cod_pais.ToString();
                                        _ADDRESS.STATE_CD = dir.cod_dpto.ToString();
                                        _ADDRESS.CITY_CD = dir.cod_municipio.ToString();
                                        _ADDRESS.IS_MAILING_ADDRESS = dir.IS_MAILING_ADDRESS;
                                        _ADDRESS.IS_HOME = dir.IS_HOME;
                                        List_ADDRESS.Add(_ADDRESS);
                                    }

                                }
                                ListTableMessage = setDto.List_ADDRESS(List_ADDRESS, ListTableMessage);
                            }

                            if (dtoMaster.DtoUser.List_Mpersona_telef != null && IsSave == "True")
                            {
                                List<PHONE> List_PHONE = new List<PHONE>();

                                foreach (Mpersona_telef phone in dtoMaster.DtoUser.List_Mpersona_telef)
                                {
                                    if (phone.txt_telefono != " " && phone.txt_telefono != ".")
                                    {
                                        PHONE _PHONE = new PHONE();
                                        _PHONE.State = phone.State_3g;

                                        if (id_persona == individual_id)
                                        {
                                            _PHONE.INDIVIDUAL_ID = id_persona;
                                        }
                                        else
                                        {
                                            _PHONE.INDIVIDUAL_ID = individual_id;
                                        }

                                        _PHONE.DATA_ID = phone.DATA_ID;
                                        _PHONE.PHONE_TYPE_CD = Convert.ToInt32(phone.cod_tipo_telef);

                                        string telefono = "";
                                        if (phone.txt_telefono.Contains(" "))
                                        {
                                            telefono = phone.txt_telefono.Replace(" ", "");
                                        }
                                        else if (phone.txt_telefono.Contains("/"))
                                        {
                                            telefono = phone.txt_telefono.Replace("/", "");
                                        }
                                        else if (phone.txt_telefono.Contains("-"))
                                        {
                                            telefono = phone.txt_telefono.Replace("-", "");
                                        }

                                        _PHONE.PHONE_NUMBER = Convert.ToInt64(FullServicesProviderHelper.ToNullInt(telefono == "" ? phone.txt_telefono : telefono));
                                        _PHONE.EXTENSION = phone.EXTENSION;
                                        _PHONE.SCHEDULE_AVAILABILITY = phone.SCHEDULE_AVAILABILITY;
                                        _PHONE.COUNTRY_CD = Convert.ToInt32(phone.cod_pais);
                                        _PHONE.STATE_CD = Convert.ToInt32(phone.cod_dpto);
                                        _PHONE.CITY_CD = Convert.ToInt32(phone.cod_municipio);
                                        _PHONE.IS_HOME = phone.IS_HOME;

                                        List_PHONE.Add(_PHONE);
                                    }
                                }

                                ListTableMessage = setDto.List_PHONE(List_PHONE, ListTableMessage);
                            }

                            if (dtoMaster.DtoUser.List_Mpersona_email != null && IsSave == "True")
                            {
                                List<EMAIL> List_EMAIL = new List<EMAIL>();

                                foreach (Mpersona_email mail in dtoMaster.DtoUser.List_Mpersona_email)
                                {
                                    EMAIL _EMAIL = new EMAIL();

                                    _EMAIL.State = mail.State_3g;

                                    if (id_persona == individual_id)
                                    {
                                        _EMAIL.INDIVIDUAL_ID = id_persona;
                                    }
                                    else
                                    {
                                        _EMAIL.INDIVIDUAL_ID = individual_id;
                                    }

                                    _EMAIL.DATA_ID = mail.DATA_ID;
                                    _EMAIL.ADDRESS = mail.txt_direccion_email;
                                    _EMAIL.EMAIL_TYPE_CD = mail.cod_tipo_email;
                                    _EMAIL.IS_MAILING_ADDRESS = mail.IS_MAILING_ADDRESS;

                                    List_EMAIL.Add(_EMAIL);
                                }

                                ListTableMessage = setDto.List_EMAIL(List_EMAIL, ListTableMessage);
                            }

                            #endregion

                            if (ListTableMessage.Count(c => c.Message != "True") > 0)
                            {
                                tableMessage.Message = "Proceso 3G Fallido";
                                tableMessage2G.Message = "Proceso 2G Exitoso";
                                Command.Transaction.Rollback();
                                Command.Dispose();
                            }
                            else
                            {
                                Command.Transaction.Commit();
                                Command.Dispose();
                            }
                        }
                        catch
                        {
                            Command.Transaction.Rollback();
                            Command.Dispose();
                            tableMessage.Message = "Proceso 3G Fallido";
                            tableMessage2G.Message = "Proceso 2G Exitoso";
                        }
                    }
                }
                catch (Exception errSup)
                {
                    tableMessage.Message = errSup.Message;
                    tableMessage2G.Message = errSup.Message;
                }
                finally
                {
                    ListTableMessage.Add(tableMessage);
                    ListTableMessage.Add(tableMessage2G);
                }
            }

            return ListTableMessage;
        }

        private DataSet statefor3g(string App, string cod_tipo_persona, string cod_tipo_doc, string nro_doc, string cod_rol,int id_persona)
        {
            DGeneric = new DataGenericExecute(App);
            DataSet ds = new DataSet();
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "INDIVIDUAL_TYPE_CD";
            parameter.Value = (cod_tipo_persona);
            listParameter.Add(parameter);
            parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "TYPE_CD";
            parameter.Value = cod_tipo_doc;
            listParameter.Add(parameter);
            parameter = new Parameters();
            parameter.ParameterType = "AseDbType.VarChar";
            parameter.Parameter = "ID";
            parameter.Value = nro_doc;
            listParameter.Add(parameter);
            parameter = new Parameters();
            parameter.ParameterType = "AseDbType.VarChar";
            parameter.Parameter = "ACCOUNT_NAME";
            parameter.Value = cod_rol;
            listParameter.Add(parameter);
            parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "INDIVIDUAL";
            parameter.Value = id_persona.ToString();
            listParameter.Add(parameter);
            ds = DGeneric.ExecuteStoreProcedure("SUP.GET_CONSECUTIVE_USER", listParameter);
            return ds;
        }

        private DataSet useridperson(string App, string idpersona)
        {
            DGeneric = new DataGenericExecute(App);
            DataSet ds = new DataSet();
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "id_persona";
            parameter.Value = (idpersona);
            listParameter.Add(parameter);

            ds = DGeneric.ExecuteStoreProcedure("SUP.GET_AGENT", listParameter);
            return ds;
        }

        private DataSet iduser(string App, string cod_usuario)
        {
            DGeneric = new DataGenericExecute(App);
            DataSet ds = new DataSet();
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.VarChar";
            parameter.Parameter = "ID";
            parameter.Value = (cod_usuario);
            listParameter.Add(parameter);
            ds = DGeneric.ExecuteStoreProcedure("SUP.GET_IDUSER", listParameter);
            return ds;
        }

        private DataSet idusercreate(string App, int id_persona)
        {
            DGeneric = new DataGenericExecute(App);
            DataSet ds = new DataSet();
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "id_persona";
            parameter.Value = (id_persona.ToString());
            listParameter.Add(parameter);
            ds = DGeneric.ExecuteStoreProcedure("SUP.GET_IDUSERCREATE", listParameter);
            return ds;
        }

        /*public string md5(string password)
        {
            //Declaraciones
            System.Security.Cryptography.MD5 md5;
            md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            //Conversion
            Byte[] encodedBytes = md5.ComputeHash(ASCIIEncoding.Default.GetBytes(password));  //genero el hash a partir de la password original

            //Resultado

            //return BitConverter.ToString(encodedBytes);      //esto, devuelve el hash con "-" cada 2 char
            return System.Text.RegularExpressions.Regex.Replace(BitConverter.ToString(encodedBytes).ToLower(), @"-", "");     //devuelve el hash continuo y en minuscula. (igual que en php)
        }*/

        private static string Encrypt2G(string txtpassw)
        {
            string Password = "Ña25%fy&";
            string p = "", b = "", S = "";
            int J = 0;
            int A1 = 0, A2 = 0, A3 = 0;

            for (int i = 0; i < Password.Length; i++)
            {
                p = p + System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(Password.Substring(i, 1))[0];
            }

            for (int i = 0; i < txtpassw.Length; i++)
            {
                A1 = System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(p.Substring(J, 1))[0];
                J = (J > p.Length ? 1 : J + 1);
                A2 = System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(txtpassw.Substring(i, 1))[0];
                A3 = A1 ^ A2;
                b = String.Format("{0:X}", A3);
                if (b.Length < 2) { b = "0" + b; }
                S = S + b;
            }
            return S;
        }

        private static string Decrypt2G(string txtpassw)
        {
            string Password = "Ña25%fy&";
            string p = "", b = "", S = "";
            int J = 0;
            int A1 = 0, A2 = 0, A3 = 0;

            for (var i = 0; i < Password.Length; i++)
            {
                p = p + System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(Password.Substring(i, 1))[0];
            }

            for (var i = 0; i < txtpassw.Length; i = i + 2)
            {
                A1 = System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(p.Substring(J, 1))[0];
                J = (J > p.Length ? 1 : J + 1);
                b = txtpassw.Substring(i, 2);
                A3 = Convert.ToInt32(b, 16);
                A2 = A1 ^ A3;
                S = S + (char)A2;
            }
            return S;
        }

        private static String Encrypt3G(String txtpassw)
        {
            CryptoEngine ce = new CryptoEngine();
            ce.EncryptionKey = txtpassw;
            ce.InClearText = txtpassw;
            ce.Encrypt();
            return ce.CryptedText;
        }
        #endregion

    }
}
