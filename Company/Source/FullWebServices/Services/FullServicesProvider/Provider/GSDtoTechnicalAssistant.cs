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
using System.Data.Common;

namespace Sistran.Co.Previsora.Application.FullServicesProvider.Provider
{
    public class GSDtoTechnicalAssistant
    {
        #region constructores

        public GSDtoTechnicalAssistant()
        {

        }

        public GSDtoTechnicalAssistant(string user, int idApp, int idRol)
        {
            User = user;
            IdApp = idApp;
            IdRol = idRol;
        }

        #endregion

        #region Propiedades

        private string PrefixConn = ConfigurationManager.AppSettings["SUPAPP"];
        private static string User { get; set; }
        private static int IdApp { get; set; }
        private static int IdRol { get; set; }
        private static string Prefix2G { get; set; }

        GetDto getDto = new GetDto(IdRol);
        DataGenericExecute DGeneric;

        #endregion

        #region Funciones y Procedimientos

        public DtoTechnicalAssistant GetDtoTechnicalAssistant(int id_rol, string cod_rol, string entity)
        {
            DtoTechnicalAssistant dtoTechnicalAssistant = new DtoTechnicalAssistant();

            //Carga el Dto solo con una entidad especifica
            if (entity != "")
            {
                dtoTechnicalAssistant = GetEntity(id_rol, cod_rol, entity);
            }
            else
            {
                //Carga el Dto Totalmente nuevo
                if (cod_rol == "new")
                    dtoTechnicalAssistant = getDto.DtoTechnicalAssistant();
                //Carga el Dto con toda la informacion del Rol buscado
                else
                {
                    dtoTechnicalAssistant = GetDto(id_rol, cod_rol);
                }
            }
            return dtoTechnicalAssistant;
        }

        private DtoTechnicalAssistant GetDto(int id_rol, string cod_rol)
        {
            DtoTechnicalAssistant dtoTechnicalAssistant = new DtoTechnicalAssistant();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataTable DtRolesAPP = DGeneric.GetListRolApp(IdRol);
            string App = string.Empty;

            foreach (DataRow dr in DtRolesAPP.Rows)
            {
                App = dr[0].ToString();
                switch (App)
                {
                    case "SISE2G":
                        GetDtoSISE2G(id_rol, cod_rol, ref dtoTechnicalAssistant);
                        break;

                    case "SISE3G":
                        GetDtoSISE3G(id_rol, cod_rol, ref dtoTechnicalAssistant);
                        break;

                }
            }

            return dtoTechnicalAssistant;
        }

        private void GetDtoSISE2G(int id_rol, string cod_rol, ref DtoTechnicalAssistant dtoTechnicalAssistant)
        {
            int id_persona = getDto.id_persona(id_rol, cod_rol);
            dtoTechnicalAssistant.dtoDataTA = getDto.dtoDataTA(Convert.ToInt32(cod_rol));
            dtoTechnicalAssistant.List_Mpersona_dir = getDto.List_Mpersona_dir(id_persona);
            dtoTechnicalAssistant.List_Mpersona_telef = getDto.List_Mpersona_telef(id_persona);
            dtoTechnicalAssistant.List_Mpersona_email = getDto.List_Mpersona_email(id_persona);
            dtoTechnicalAssistant.List_Logbook = getDto.List_Logbook(id_persona, id_rol);
            dtoTechnicalAssistant.dtoSarlaft = getDto.dtoSarlaft(id_persona);
        }

        private void GetDtoSISE3G(int id_rol, string cod_rol, ref DtoTechnicalAssistant DtoTechnicalAssistant)
        {
            //logica de consulta para anexar campos de 3G
        }

        private DtoTechnicalAssistant GetEntity(int id_rol, string cod_rol, string entity)
        {
            DtoTechnicalAssistant dtoTechnicalAssistant = new DtoTechnicalAssistant();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataTable DtRolesAPP = DGeneric.GetListRolApp(IdRol);
            string App = string.Empty;

            foreach (DataRow dr in DtRolesAPP.Rows)
            {
                App = dr[0].ToString();
                switch (App)
                {
                    case "SISE2G":
                        GetEntitySISE2G(id_rol, cod_rol, entity, ref dtoTechnicalAssistant);
                        break;

                    case "SISE3G":
                        GetEntitySISE3G(id_rol, cod_rol, entity, ref dtoTechnicalAssistant);
                        break;

                }
            }

            return dtoTechnicalAssistant;
        }

        private void GetEntitySISE2G(int id_rol, string cod_rol, string entity, ref DtoTechnicalAssistant dtoTechnicalAssistant)
        {
            int id_persona = getDto.id_persona(id_rol, cod_rol);
            switch (entity)
            {
                case "dtoDataTA":
                    dtoTechnicalAssistant.dtoDataTA = getDto.dtoDataTA(Convert.ToInt32(cod_rol));
                    break;
                case "List_Mpersona_dir":
                    dtoTechnicalAssistant.List_Mpersona_dir = getDto.List_Mpersona_dir(id_persona);
                    break;
                case "List_Mpersona_telef":
                    dtoTechnicalAssistant.List_Mpersona_telef = getDto.List_Mpersona_telef(id_persona);
                    break;
                case "List_Mpersona_email":
                    dtoTechnicalAssistant.List_Mpersona_email = getDto.List_Mpersona_email(id_persona);
                    break;
                case "List_Logbook":
                    dtoTechnicalAssistant.List_Logbook = getDto.List_Logbook(id_persona, id_rol);
                    break;
                case "dtoSarlaft":
                    dtoTechnicalAssistant.dtoSarlaft = getDto.dtoSarlaft(id_persona);
                    break;
                case "dtoSarlaftForm":
                    dtoTechnicalAssistant.dtoSarlaft = getDto.dtoSarlaftForm(Convert.ToInt32(cod_rol));
                    break;

            }
        }

        private void GetEntitySISE3G(int id_rol, string cod_rol, string entity, ref DtoTechnicalAssistant dtoTechnicalAssistant)
        {
            //aqui codigo de otra base de datos llenando campos extras

        }

        public SUPMessages SetDtoTechnicalAssistant(DtoMaster dtoMaster)
        {
            DGeneric = new DataGenericExecute(PrefixConn);
            DataTable DtRolesAPP = DGeneric.GetListRolApp(IdRol);
            string App = string.Empty;
            int cod_rol = Convert.ToInt32(dtoMaster.cod_Rol);
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
                            ListTableMessage = setDto.UpdateBasicDataInSise2G(dtoMaster.DtoTechnicalAssistant.dtoDataTA.mpersona, ListTableMessage); //UPDATE mpersona
                            //Se valida que el unico rol que posee es de SISE 2G, de ser asi se actualizan los datos basicos en la data de SISE 3G
                            if (DtRolesAPP.Rows.Count == 1)
                            {
                                individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoTechnicalAssistant.dtoDataTA.mpersona.id_persona);
                                if (individualId3G > 0)
                                {
                                    ListTableMessage = setDto.UpdateBasicDataInSise3G(dtoMaster.DtoTechnicalAssistant.dtoDataTA.mpersona, individualId3G, ListTableMessage); //UPDATE UP.PERSON || UP.COMPANY, UP.CO_COMPANY

                                }
                            }
                        }
                        //Autor: Edward Rubiano; Fecha: 10/05/2016;
                        SetSISE2G(dtoMaster, App, ListTableMessage, ref cod_rol, ref id_persona);
                        _SUPMessages.cod_rol = cod_rol.ToString();
                        _SUPMessages.id_persona = id_persona.ToString();
                        break;

                    case "SISE3G":
                        //Autor: Edward Rubiano; Fecha: 10/05/2016; Asunto: C11226; Descripcion: Validaciones para guardado unico de datos basicos
                        if (dtoMaster.GlobalOperation != "UpdateD")
                        {
                            if (cod_rol != -1)
                                SetSISE3G(dtoMaster, App, ListTableMessage);
                        }
                        else
                        {
                            setDto = new SetDto(User, App, IdRol);
                            individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoTechnicalAssistant.dtoDataTA.mpersona.id_persona);
                            if (individualId3G > 0)
                            {
                                ListTableMessage = setDto.UpdateBasicDataInSise3G(dtoMaster.DtoTechnicalAssistant.dtoDataTA.mpersona, individualId3G, ListTableMessage);

                            }
                        }
                        //Autor: Edward Rubiano; Fecha: 10/05/2016; Asunto: C11226; Descripcion: Validaciones para guardado unico de datos basicos
                        break;

                }
            }

            //Autor: Edward Rubiano; Fecha: 13/04/2016; Asunto: C11226; Descripcion: Se valida si la persona existe en 3g y de ser asi se ejecuta el proceso de guardado
            if (dtoMaster.GlobalOperation != "UpdateD")
            {
                setDto = new SetDto(User, App, IdRol);
                individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoTechnicalAssistant.dtoDataTA.mpersona.id_persona);
                if (individualId3G > 0)
                {
                    ListTableMessage = setDto.UpdateAddressAndPhoneSise3G(dtoMaster.DtoTechnicalAssistant.List_Mpersona_dir, dtoMaster.DtoTechnicalAssistant.List_Mpersona_telef, dtoMaster.DtoTechnicalAssistant.List_Mpersona_email, individualId3G, ListTableMessage);
                }
            }
            //Autor: Edward Rubiano; Fecha: 13/04/2016; Asunto: C11226; Descripcion: Se valida si la persona existe en 3g y de ser asi se ejecuta el proceso de guardado

            _SUPMessages.cod_rol = cod_rol.ToString();
            _SUPMessages.ListMessages = ListTableMessage;

            return _SUPMessages;
        }

        private List<TableMessage> SetSISE2G(DtoMaster dtoMaster, string App, List<TableMessage> ListTableMessage, ref int cod_rolref, ref int id_personaref)
        {
            TableMessage tableMessage = new TableMessage();
            try
            {

                int id_persona = id_personaref;
                int cod_rol = cod_rolref;
                //validamos si existen el id_persona y el cod_rol
                //Buscamos el codigo del Rol
                if (dtoMaster.cod_Rol != "0")
                {
                    cod_rol = Convert.ToInt32(dtoMaster.cod_Rol);
                    cod_rolref = cod_rol;
                    id_persona = getDto.id_persona(IdRol, cod_rol.ToString());
                }
                else
                {
                    if (dtoMaster.DtoTechnicalAssistant.dtoDataTA.mpersona != null)
                    {
                        if (dtoMaster.DtoTechnicalAssistant.dtoDataTA.mpersona.id_persona == 0)
                        {
                            DGeneric = new DataGenericExecute(App);
                            id_persona = DGeneric.GetConsecutive("mpersona");
                        }
                        else
                        {
                            id_persona = dtoMaster.DtoTechnicalAssistant.dtoDataTA.mpersona.id_persona;

                        }
                    }

                    DGeneric = new DataGenericExecute(App);
                    cod_rol = DGeneric.GetConsecutive("tasist_tecnico");
                    cod_rolref = cod_rol;
                }

                id_personaref = id_persona;

                //Insertamos o Actualizamos id_persona y cod_rol
                if (dtoMaster.DtoTechnicalAssistant.dtoDataTA != null)
                {
                    if (dtoMaster.DtoTechnicalAssistant.dtoDataTA.mpersona != null)
                        dtoMaster.DtoTechnicalAssistant.dtoDataTA.mpersona.Update("id_persona", id_persona);

                    if (dtoMaster.DtoTechnicalAssistant.dtoDataTA.mpersona_ciiu != null)
                        dtoMaster.DtoTechnicalAssistant.dtoDataTA.mpersona_ciiu.Update("id_persona", id_persona);

                    if (dtoMaster.DtoTechnicalAssistant.dtoDataTA.mpersona_requiere_sarlaft != null)
                        dtoMaster.DtoTechnicalAssistant.dtoDataTA.mpersona_requiere_sarlaft.Update("id_persona", id_persona);

                    if (dtoMaster.DtoTechnicalAssistant.dtoDataTA.tasist_tecnico != null)
                        dtoMaster.DtoTechnicalAssistant.dtoDataTA.tasist_tecnico.Update("id_persona", id_persona);

                    if (dtoMaster.DtoTechnicalAssistant.dtoDataTA.tasist_tecnico != null)
                        dtoMaster.DtoTechnicalAssistant.dtoDataTA.tasist_tecnico.Update("cod_asist_tecnico", cod_rol);
                }

                if (dtoMaster.DtoTechnicalAssistant.List_Mpersona_dir != null)
                    dtoMaster.DtoTechnicalAssistant.List_Mpersona_dir.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoTechnicalAssistant.List_Mpersona_telef != null)
                    dtoMaster.DtoTechnicalAssistant.List_Mpersona_telef.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoTechnicalAssistant.List_Mpersona_email != null)
                    dtoMaster.DtoTechnicalAssistant.List_Mpersona_email.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoTechnicalAssistant.List_Logbook != null)
                    dtoMaster.DtoTechnicalAssistant.List_Logbook.Update(c => { c.ID_PERSONA = id_persona; });

                if (dtoMaster.DtoTechnicalAssistant.dtoSarlaft != null)
                {
                    if (dtoMaster.DtoTechnicalAssistant.dtoSarlaft.List_detalle != null)
                        dtoMaster.DtoTechnicalAssistant.dtoSarlaft.List_detalle.Update(c => { c.id_persona = id_persona; });

                    if (dtoMaster.DtoTechnicalAssistant.dtoSarlaft.frm_sarlaft_vinculos != null)
                        dtoMaster.DtoTechnicalAssistant.dtoSarlaft.frm_sarlaft_vinculos.Update("id_persona", id_persona);

                    if (dtoMaster.DtoTechnicalAssistant.dtoSarlaft.frm_sarlaft_aut_incrementos != null)
                        dtoMaster.DtoTechnicalAssistant.dtoSarlaft.frm_sarlaft_aut_incrementos.Update("id_persona", id_persona);

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

                        #region Insert - Update
                        SetDto setDto = new SetDto(User, App, IdRol, Command);

                        if (dtoMaster.DtoTechnicalAssistant.dtoDataTA != null)
                        {
                            ListTableMessage = setDto.dtoDataTA(dtoMaster.DtoTechnicalAssistant.dtoDataTA, ListTableMessage);
                            if (ListTableMessage.Count(c => c.NameTable == "Tasist_tecnico" && c.Message != "True") > 0)
                                if (dtoMaster.DtoTechnicalAssistant.dtoDataTA.mpersona.State.Equals('V') || !dtoMaster.DtoTechnicalAssistant.dtoDataTA.tasist_tecnico.State.Equals('V'))
                                    cod_rolref = -1;
                        }

                        if (cod_rolref != -1)
                        {
                            if (dtoMaster.DtoTechnicalAssistant.List_Mpersona_dir != null)
                                ListTableMessage = setDto.List_Mpersona_dir(dtoMaster.DtoTechnicalAssistant.List_Mpersona_dir, ListTableMessage);

                            if (dtoMaster.DtoTechnicalAssistant.List_Mpersona_telef != null)
                                ListTableMessage = setDto.List_Mpersona_telef(dtoMaster.DtoTechnicalAssistant.List_Mpersona_telef, ListTableMessage);

                            if (dtoMaster.DtoTechnicalAssistant.List_Mpersona_email != null)
                                ListTableMessage = setDto.List_Mpersona_email(dtoMaster.DtoTechnicalAssistant.List_Mpersona_email, ListTableMessage);

                            if (dtoMaster.DtoTechnicalAssistant.List_Logbook != null)
                                ListTableMessage = setDto.List_Logbook(dtoMaster.DtoTechnicalAssistant.List_Logbook, ListTableMessage);

                            if (dtoMaster.DtoTechnicalAssistant.dtoSarlaft != null)
                                ListTableMessage = setDto.dtoSarlaft(dtoMaster.DtoTechnicalAssistant.dtoSarlaft, ListTableMessage);

                            if (
                                    (
                                        (dtoMaster.DtoTechnicalAssistant.List_Mpersona_dir != null && dtoMaster.DtoTechnicalAssistant.List_Mpersona_dir.Count > 0) ||
                                        (dtoMaster.DtoTechnicalAssistant.List_Mpersona_telef != null && dtoMaster.DtoTechnicalAssistant.List_Mpersona_telef.Count > 0) ||
                                        (dtoMaster.DtoTechnicalAssistant.List_Mpersona_email != null && dtoMaster.DtoTechnicalAssistant.List_Mpersona_email.Count > 0)
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

        private List<TableMessage> SetSISE3G(DtoMaster dtoMaster, string App, List<TableMessage> ListTableMessage)
        {
            SetDto setDto = new SetDto(User, App, IdRol);
            return ListTableMessage;
        }

        #endregion

    }
}
