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
    public class GSDtoAssigneed
    {
        #region constructores

        public GSDtoAssigneed()
        {

        }

        public GSDtoAssigneed(string user, int idApp, int idRol)
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

        public DtoAssigneed GetDtoAssigneed(int id_rol, string cod_rol, string entity)
        {
            DtoAssigneed dtoAssigneed = new DtoAssigneed();

            //Carga el Dto solo con una entidad especifica
            if (entity != "")
            {
                dtoAssigneed = GetEntity(id_rol, cod_rol, entity);
            }
            else
            {
                //Carga el Dto Totalmente nuevo
                if (cod_rol == "new")
                    dtoAssigneed = getDto.DtoAssigneed();
                //Carga el Dto con toda la informacion del Rol buscado
                else
                {
                    dtoAssigneed = GetDto(id_rol, cod_rol);
                }
            }
            return dtoAssigneed;
        }

        private DtoAssigneed GetDto(int id_rol, string cod_rol)
        {
            DtoAssigneed dtoAssigneed = new DtoAssigneed();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataTable DtRolesAPP = DGeneric.GetListRolApp(IdRol);
            string App = string.Empty;

            foreach (DataRow dr in DtRolesAPP.Rows)
            {
                App = dr[0].ToString();
                switch (App)
                {
                    case "SISE2G":
                        GetDtoSISE2G(id_rol, cod_rol, ref dtoAssigneed);
                        break;

                    case "SISE3G":
                        GetDtoSISE3G(id_rol, cod_rol, ref dtoAssigneed);
                        break;

                }
            }

            return dtoAssigneed;
        }

        private void GetDtoSISE2G(int id_rol, string cod_rol, ref DtoAssigneed dtoAssigneed)
        {
            int id_persona = getDto.id_persona(id_rol, cod_rol);
            dtoAssigneed.DtoDataAssigneed = getDto.dtoDataAssigneed(Convert.ToInt32(cod_rol));
            dtoAssigneed.List_CesionarioDe = getDto.List_CesionarioDe(Convert.ToInt32(cod_rol));
            dtoAssigneed.List_Mpersona_dir = getDto.List_Mpersona_dir(id_persona);
            dtoAssigneed.List_Mpersona_telef = getDto.List_Mpersona_telef(id_persona);
            dtoAssigneed.List_Mpersona_email = getDto.List_Mpersona_email(id_persona);
            dtoAssigneed.List_Logbook = getDto.List_Logbook(id_persona, id_rol);
            dtoAssigneed.dtoSarlaft = getDto.dtoSarlaft(id_persona);
            dtoAssigneed.List_Frm_sarlaft_accionistas_asoc = getDto.List_Frm_sarlaft_accionistas_asoc(id_persona);
            dtoAssigneed.List_Mcesionario = getDto.List_Mcesionario(Convert.ToInt32(cod_rol));
            dtoAssigneed.List_Mpersona_cuentas_bancarias = getDto.List_Mpersona_cuentas_bancarias(id_persona);
            dtoAssigneed.List_Referencias = getDto.List_Referencias(Convert.ToInt32(cod_rol), id_rol);
            dtoAssigneed.List_CesionarioDe = getDto.List_CesionarioDe(Convert.ToInt32(cod_rol));
            dtoAssigneed.List_Mcesio_trans_bancarias = getDto.List_Mcesio_trans_bancarias();
            dtoAssigneed.dtoRep = getDto.dtoRep(id_persona);

        }

        private void GetDtoSISE3G(int id_rol, string cod_rol, ref DtoAssigneed dtoAssigneed)
        {
            //logica de consulta para anexar campos de 3G
        }

        private DtoAssigneed GetEntity(int id_rol, string cod_rol, string entity)
        {
            DtoAssigneed dtoAssigneed = new DtoAssigneed();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataTable DtRolesAPP = DGeneric.GetListRolApp(IdRol);
            string App = string.Empty;

            foreach (DataRow dr in DtRolesAPP.Rows)
            {
                App = dr[0].ToString();
                switch (App)
                {
                    case "SISE2G":
                        GetEntitySISE2G(id_rol, cod_rol, entity, ref dtoAssigneed);
                        break;

                    case "SISE3G":
                        GetEntitySISE3G(id_rol, cod_rol, entity, ref dtoAssigneed);
                        break;

                }
            }

            return dtoAssigneed;
        }

        private void GetEntitySISE2G(int id_rol, string cod_rol, string entity, ref DtoAssigneed dtoAssigneed)
        {
            int id_persona = getDto.id_persona(id_rol, cod_rol);
            switch (entity)
            {
                case "DtoDataAssigneed":
                    dtoAssigneed.DtoDataAssigneed = getDto.dtoDataAssigneed(Convert.ToInt32(cod_rol));
                    break;
                case "List_Mpersona_dir":
                    dtoAssigneed.List_Mpersona_dir = getDto.List_Mpersona_dir(id_persona);
                    break;
                case "List_Mpersona_telef":
                    dtoAssigneed.List_Mpersona_telef = getDto.List_Mpersona_telef(id_persona);
                    break;
                case "List_Mpersona_email":
                    dtoAssigneed.List_Mpersona_email = getDto.List_Mpersona_email(id_persona);
                    break;
                case "List_Logbook":
                    dtoAssigneed.List_Logbook = getDto.List_Logbook(id_persona, id_rol);
                    break;
                case "dtoSarlaft":
                    dtoAssigneed.dtoSarlaft = getDto.dtoSarlaft(id_persona);
                    break;
                case "dtoSarlaftForm":
                    dtoAssigneed.dtoSarlaft = getDto.dtoSarlaftForm(Convert.ToInt32(cod_rol));
                    break;
                case "List_Frm_sarlaft_accionistas_asoc":
                    dtoAssigneed.List_Frm_sarlaft_accionistas_asoc = getDto.List_Frm_sarlaft_accionistas_asoc(id_persona);
                    break;
                case "List_Mcesionario":
                    dtoAssigneed.List_Mcesionario = getDto.List_Mcesionario(id_persona);
                    break;
                case "List_Mpersona_cuentas_bancarias":
                    dtoAssigneed.List_Mpersona_cuentas_bancarias = getDto.List_Mpersona_cuentas_bancarias(id_persona);
                    break;
                case "dtoRep":
                    dtoAssigneed.dtoRep = getDto.dtoRep(id_persona);
                    break;
            }
        }

        private void GetEntitySISE3G(int id_rol, string cod_rol, string entity, ref DtoAssigneed dtoAssigneed)
        {
            //aqui codigo de otra base de datos llenando campos extras

        }

        public SUPMessages SetDtoAssigneed(DtoMaster dtoMaster)
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
                            ListTableMessage = setDto.UpdateBasicDataInSise2G(dtoMaster.DtoAssigneed.DtoDataAssigneed.mpersona, ListTableMessage); //UPDATE mpersona
                            //Se valida que el unico rol que posee es de SISE 2G, de ser asi se actualizan los datos basicos en la data de SISE 3G
                            if (DtRolesAPP.Rows.Count == 1)
                            {
                                individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoAssigneed.DtoDataAssigneed.mpersona.id_persona);
                                if (individualId3G > 0)
                                {
                                    ListTableMessage = setDto.UpdateBasicDataInSise3G(dtoMaster.DtoAssigneed.DtoDataAssigneed.mpersona, individualId3G, ListTableMessage); //UPDATE UP.PERSON || UP.COMPANY, UP.CO_COMPANY

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
                            if (cod_rol != -1)
                                SetSISE3G(dtoMaster, App, ListTableMessage);
                        }
                        else
                        {
                            setDto = new SetDto(User, App, IdRol);
                            individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoAssigneed.DtoDataAssigneed.mpersona.id_persona);
                            if (individualId3G > 0)
                            {
                                ListTableMessage = setDto.UpdateBasicDataInSise3G(dtoMaster.DtoAssigneed.DtoDataAssigneed.mpersona, individualId3G, ListTableMessage);

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
                individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoAssigneed.DtoDataAssigneed.mpersona.id_persona);
                if (individualId3G > 0)
                {
                    ListTableMessage = setDto.UpdateAddressAndPhoneSise3G(dtoMaster.DtoAssigneed.List_Mpersona_dir, dtoMaster.DtoAssigneed.List_Mpersona_telef, dtoMaster.DtoAssigneed.List_Mpersona_email, individualId3G, ListTableMessage);
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
            tableMessage.NameTable = "Proceso 2G";
            tableMessage.Message = "True";
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
                    if (dtoMaster.DtoAssigneed.DtoDataAssigneed.mpersona != null)
                    {
                        if (dtoMaster.DtoAssigneed.DtoDataAssigneed.mpersona.id_persona == 0)
                        {
                            DGeneric = new DataGenericExecute(App);
                            id_persona = DGeneric.GetConsecutive("mpersona");
                        }
                        else
                        {
                            id_persona = dtoMaster.DtoAssigneed.DtoDataAssigneed.mpersona.id_persona;

                        }
                    }
                    DGeneric = new DataGenericExecute(App);
                    cod_rol = DGeneric.GetConsecutive("mcesionario");
                    cod_rolref = cod_rol;
                }

                id_personaref = id_persona;

                //Insertamos o Actualizamos id_persona y cod_rol
                if (dtoMaster.DtoAssigneed.DtoDataAssigneed != null)
                {
                    if (dtoMaster.DtoAssigneed.DtoDataAssigneed.mpersona != null)
                        dtoMaster.DtoAssigneed.DtoDataAssigneed.mpersona.Update("id_persona", id_persona);

                    //if (exist) dtoMaster.DtoAssigneed.DtoDataAssigneed.mpersona.State = 'U';

                    if (dtoMaster.DtoAssigneed.DtoDataAssigneed.mcesionario != null)
                        dtoMaster.DtoAssigneed.DtoDataAssigneed.mcesionario.Update("id_persona", id_persona);

                    if (dtoMaster.DtoAssigneed.DtoDataAssigneed.mcesionario != null)
                        dtoMaster.DtoAssigneed.DtoDataAssigneed.mcesionario.Update("cod_cesionario", cod_rol);

                    if (dtoMaster.DtoAssigneed.DtoDataAssigneed.mpersona_requiere_sarlaft != null)
                        dtoMaster.DtoAssigneed.DtoDataAssigneed.mpersona_requiere_sarlaft.Update("id_persona", id_persona);

                    if (dtoMaster.DtoAssigneed.DtoDataAssigneed.mpersona_ciiu != null)
                        dtoMaster.DtoAssigneed.DtoDataAssigneed.mpersona_ciiu.Update("id_persona", id_persona);
                }

                if (dtoMaster.DtoAssigneed.List_Mpersona_dir != null)
                    dtoMaster.DtoAssigneed.List_Mpersona_dir.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoAssigneed.List_Mpersona_telef != null)
                    dtoMaster.DtoAssigneed.List_Mpersona_telef.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoAssigneed.List_Mpersona_email != null)
                    dtoMaster.DtoAssigneed.List_Mpersona_email.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoAssigneed.List_Logbook != null)
                    dtoMaster.DtoAssigneed.List_Logbook.Update(c => { c.ID_PERSONA = id_persona; });

                if (dtoMaster.DtoAssigneed.List_Frm_sarlaft_accionistas_asoc != null)
                    dtoMaster.DtoAssigneed.List_Frm_sarlaft_accionistas_asoc.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoAssigneed.List_Mpersona_cuentas_bancarias != null)
                    dtoMaster.DtoAssigneed.List_Mpersona_cuentas_bancarias.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoAssigneed.dtoRep != null)
                {
                    if (dtoMaster.DtoAssigneed.dtoRep.mpersona_rep_legal != null)
                        dtoMaster.DtoAssigneed.dtoRep.mpersona_rep_legal.Update("id_persona", id_persona);
                    if (dtoMaster.DtoAssigneed.dtoRep.List_mpersona_rep_legal_dir != null)
                        dtoMaster.DtoAssigneed.dtoRep.List_mpersona_rep_legal_dir.Update(c => { c.id_persona = id_persona; });
                }

                if (dtoMaster.DtoAssigneed.dtoSarlaft != null)
                {
                    if (dtoMaster.DtoAssigneed.dtoSarlaft.List_detalle != null)
                        dtoMaster.DtoAssigneed.dtoSarlaft.List_detalle.Update(c => { c.id_persona = id_persona; });

                    if (dtoMaster.DtoAssigneed.dtoSarlaft.frm_sarlaft_vinculos != null)
                        dtoMaster.DtoAssigneed.dtoSarlaft.frm_sarlaft_vinculos.Update("id_persona", id_persona);

                    if (dtoMaster.DtoAssigneed.dtoSarlaft.frm_sarlaft_aut_incrementos != null)
                        dtoMaster.DtoAssigneed.dtoSarlaft.frm_sarlaft_aut_incrementos.Update("id_persona", id_persona);

                }

                if (dtoMaster.DtoAssigneed.List_Referencias != null)
                    dtoMaster.DtoAssigneed.List_Referencias.Update(c => { c.cod_rol = cod_rol; });

               

                if (dtoMaster.DtoAssigneed.List_Mcesio_trans_bancarias != null)
                    dtoMaster.DtoAssigneed.List_Mcesio_trans_bancarias.Update(c => { c.cod_cesionario = cod_rol; });

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

                        if (dtoMaster.DtoAssigneed.DtoDataAssigneed != null)
                        {
                            ListTableMessage = setDto.dtoDataAssigneed(dtoMaster.DtoAssigneed.DtoDataAssigneed, ListTableMessage);
                            if (ListTableMessage.Count(c => c.NameTable == "Mcesionario" && c.Message != "True") > 0)
                                if (dtoMaster.DtoAssigneed.DtoDataAssigneed.mpersona.State.Equals('V') || !dtoMaster.DtoAssigneed.DtoDataAssigneed.mcesionario.State.Equals('V'))
                                    cod_rolref = -1;
                        }

                        if (cod_rolref != -1)
                        {
                            if (dtoMaster.DtoAssigneed.List_Mpersona_dir != null)
                                ListTableMessage = setDto.List_Mpersona_dir(dtoMaster.DtoAssigneed.List_Mpersona_dir, ListTableMessage);

                            if (dtoMaster.DtoAssigneed.List_Mpersona_telef != null)
                                ListTableMessage = setDto.List_Mpersona_telef(dtoMaster.DtoAssigneed.List_Mpersona_telef, ListTableMessage);

                            if (dtoMaster.DtoAssigneed.List_Mpersona_email != null)
                                ListTableMessage = setDto.List_Mpersona_email(dtoMaster.DtoAssigneed.List_Mpersona_email, ListTableMessage);

                            if (dtoMaster.DtoAssigneed.List_Logbook != null)
                                ListTableMessage = setDto.List_Logbook(dtoMaster.DtoAssigneed.List_Logbook, ListTableMessage);

                            if (dtoMaster.DtoAssigneed.List_Frm_sarlaft_accionistas_asoc != null)
                                ListTableMessage = setDto.List_Frm_sarlaft_accionistas_asoc(dtoMaster.DtoAssigneed.List_Frm_sarlaft_accionistas_asoc, ListTableMessage);

                            if (dtoMaster.DtoAssigneed.List_Mpersona_cuentas_bancarias != null)
                                ListTableMessage = setDto.List_Mpersona_cuentas_bancarias(dtoMaster.DtoAssigneed.List_Mpersona_cuentas_bancarias, ListTableMessage);

                            if (dtoMaster.DtoAssigneed.dtoSarlaft != null)
                                ListTableMessage = setDto.dtoSarlaft(dtoMaster.DtoAssigneed.dtoSarlaft, ListTableMessage);

                            if (dtoMaster.DtoAssigneed.List_Referencias != null)
                                ListTableMessage = setDto.List_Referencias(dtoMaster.DtoAssigneed.List_Referencias, ListTableMessage);

                            if (dtoMaster.DtoAssigneed.dtoRep != null)
                                ListTableMessage = setDto.dtoRep(dtoMaster.DtoAssigneed.dtoRep, ListTableMessage);

                            if (dtoMaster.DtoAssigneed.List_Mcesio_trans_bancarias != null)
                                ListTableMessage = setDto.List_Mcesio_trans_bancarias(dtoMaster.DtoAssigneed.List_Mcesio_trans_bancarias, ListTableMessage);

                            if (
                                    (
                                        (dtoMaster.DtoAssigneed.List_Mpersona_dir != null && dtoMaster.DtoAssigneed.List_Mpersona_dir.Count > 0) ||
                                        (dtoMaster.DtoAssigneed.List_Mpersona_telef != null && dtoMaster.DtoAssigneed.List_Mpersona_telef.Count > 0) ||
                                        (dtoMaster.DtoAssigneed.List_Mpersona_email != null && dtoMaster.DtoAssigneed.List_Mpersona_email.Count > 0)
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
