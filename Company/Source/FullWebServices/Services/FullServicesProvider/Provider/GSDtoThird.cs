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
    public class GSDtoThird
    {
        #region constructores

        public GSDtoThird()
        {

        }

        public GSDtoThird(string user, int idApp, int idRol)
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

        public DtoThird GetDtoThird(int id_rol, string cod_rol, string entity)
        {
            DtoThird dtoThird = new DtoThird();

            //Carga el Dto solo con una entidad especifica
            if (entity != "")
            {
                dtoThird = GetEntity(id_rol, cod_rol, entity);
            }
            else
            {
                //Carga el Dto Totalmente nuevo
                if (cod_rol == "new")
                    dtoThird = getDto.DtoThird();
                //Carga el Dto con toda la informacion del Rol buscado
                else
                {
                    dtoThird = GetDto(id_rol, cod_rol);
                }
            }
            return dtoThird;
        }

        private DtoThird GetDto(int id_rol, string cod_rol)
        {
            DtoThird dtoThird = new DtoThird();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataTable DtRolesAPP = DGeneric.GetListRolApp(IdRol);
            string App = string.Empty;

            foreach (DataRow dr in DtRolesAPP.Rows)
            {
                App = dr[0].ToString();
                switch (App)
                {
                    case "SISE2G":
                        GetDtoSISE2G(id_rol, cod_rol, ref dtoThird);
                        break;

                    case "SISE3G":
                        GetDtoSISE3G(id_rol, cod_rol, ref dtoThird);
                        break;

                }
            }

            return dtoThird;
        }

        private void GetDtoSISE2G(int id_rol, string cod_rol, ref DtoThird dtoThird)
        {
            int id_persona = getDto.id_persona(id_rol, cod_rol);
            dtoThird.DtoDataThird = getDto.dtoDataThird(Convert.ToInt32(cod_rol));
            dtoThird.List_Mpersona_dir = getDto.List_Mpersona_dir(id_persona);
            dtoThird.List_Mpersona_telef = getDto.List_Mpersona_telef(id_persona);
            dtoThird.List_Mpersona_email = getDto.List_Mpersona_email(id_persona);
            dtoThird.List_Logbook = getDto.List_Logbook(id_persona, id_rol);
            dtoThird.dtoSarlaft = getDto.dtoSarlaft(id_persona);
            dtoThird.List_Frm_sarlaft_accionistas_asoc = getDto.List_Frm_sarlaft_accionistas_asoc(id_persona);
            dtoThird.List_Referencias = getDto.List_Referencias(Convert.ToInt32(cod_rol), id_rol);
            dtoThird.dtoRep = getDto.dtoRep(id_persona);
            dtoThird.List_Mpersona_impuesto = getDto.List_Mpersona_impuesto(id_persona);
            dtoThird.List_Mpersona_actividad = getDto.List_Mpersona_actividad(id_persona);
        }

        private void GetDtoSISE3G(int id_rol, string cod_rol, ref DtoThird dtoThird)
        {
            //logica de consulta para anexar campos de 3G
        }

        private DtoThird GetEntity(int id_rol, string cod_rol, string entity)
        {
            DtoThird dtoThird = new DtoThird();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataTable DtRolesAPP = DGeneric.GetListRolApp(IdRol);
            string App = string.Empty;

            foreach (DataRow dr in DtRolesAPP.Rows)
            {
                App = dr[0].ToString();
                switch (App)
                {
                    case "SISE2G":
                        GetEntitySISE2G(id_rol, cod_rol, entity, ref dtoThird);
                        break;

                    case "SISE3G":
                        GetEntitySISE3G(id_rol, cod_rol, entity, ref dtoThird);
                        break;

                }
            }

            return dtoThird;
        }

        private void GetEntitySISE2G(int id_rol, string cod_rol, string entity, ref DtoThird dtoThird)
        {
            int id_persona = getDto.id_persona(id_rol, cod_rol);
            switch (entity)
            {
                case "DtoDataThird":
                    dtoThird.DtoDataThird = getDto.dtoDataThird(Convert.ToInt32(cod_rol));
                    break;
                case "List_Mpersona_dir":
                    dtoThird.List_Mpersona_dir = getDto.List_Mpersona_dir(id_persona);
                    break;
                case "List_Mpersona_telef":
                    dtoThird.List_Mpersona_telef = getDto.List_Mpersona_telef(id_persona);
                    break;
                case "List_Mpersona_email":
                    dtoThird.List_Mpersona_email = getDto.List_Mpersona_email(id_persona);
                    break;
                case "List_Logbook":
                    dtoThird.List_Logbook = getDto.List_Logbook(id_persona, id_rol);
                    break;
                case "dtoSarlaft":
                    dtoThird.dtoSarlaft = getDto.dtoSarlaft(id_persona);
                    break;
                case "dtoSarlaftForm":
                    dtoThird.dtoSarlaft = getDto.dtoSarlaftForm(Convert.ToInt32(cod_rol));
                    break;
                case "List_Frm_sarlaft_accionistas_asoc":
                    dtoThird.List_Frm_sarlaft_accionistas_asoc = getDto.List_Frm_sarlaft_accionistas_asoc(id_persona);
                    break;
                case "List_Referencias":
                    dtoThird.List_Referencias = getDto.List_Referencias(id_rol, Convert.ToInt32(cod_rol));
                    break;
                case "dtoRep":
                    dtoThird.dtoRep = getDto.dtoRep(id_persona);
                    break;
                case "List_Mpersona_actividad":
                    dtoThird.List_Mpersona_actividad = getDto.List_Mpersona_actividad(id_persona);
                    break;
                case "List_Mpersona_impuesto":
                    dtoThird.List_Mpersona_impuesto = getDto.List_Mpersona_impuesto(id_persona);
                    break;
            }
        }

        private void GetEntitySISE3G(int id_rol, string cod_rol, string entity, ref DtoThird dtoThird)
        {
            //aqui codigo de otra base de datos llenando campos extras

        }

        public SUPMessages SetDtoThird(DtoMaster dtoMaster)
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
                            ListTableMessage = setDto.UpdateBasicDataInSise2G(dtoMaster.DtoThird.DtoDataThird.mpersona, ListTableMessage); //UPDATE mpersona
                            //Se valida que el unico rol que posee es de SISE 2G, de ser asi se actualizan los datos basicos en la data de SISE 3G
                            if (DtRolesAPP.Rows.Count == 1)
                            {
                                individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoThird.DtoDataThird.mpersona.id_persona);
                                if (individualId3G > 0)
                                {
                                    ListTableMessage = setDto.UpdateBasicDataInSise3G(dtoMaster.DtoThird.DtoDataThird.mpersona, individualId3G, ListTableMessage); //UPDATE UP.PERSON || UP.COMPANY, UP.CO_COMPANY

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
                            individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoThird.DtoDataThird.mpersona.id_persona);
                            if (individualId3G > 0)
                            {
                                ListTableMessage = setDto.UpdateBasicDataInSise3G(dtoMaster.DtoThird.DtoDataThird.mpersona, individualId3G, ListTableMessage);

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
                individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoThird.DtoDataThird.mpersona.id_persona);
                if (individualId3G > 0)
                {
                    ListTableMessage = setDto.UpdateAddressAndPhoneSise3G(dtoMaster.DtoThird.List_Mpersona_dir, dtoMaster.DtoThird.List_Mpersona_telef, dtoMaster.DtoThird.List_Mpersona_email, individualId3G, ListTableMessage);
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
                    if (dtoMaster.DtoThird.DtoDataThird.mpersona != null)
                    {
                        if (dtoMaster.DtoThird.DtoDataThird.mpersona.id_persona == 0)
                        {
                            DGeneric = new DataGenericExecute(App);
                            id_persona = DGeneric.GetConsecutive("mpersona");
                        }
                        else
                        {
                            id_persona = dtoMaster.DtoThird.DtoDataThird.mpersona.id_persona;

                        }
                    }

                    DGeneric = new DataGenericExecute(App);
                    cod_rol = DGeneric.GetConsecutive("mtercero");
                    cod_rolref = cod_rol;
                }

                id_personaref = id_persona;

                //Insertamos o Actualizamos id_persona y cod_rol
                if (dtoMaster.DtoThird.DtoDataThird != null)
                {
                    if (dtoMaster.DtoThird.DtoDataThird.mpersona != null)
                        dtoMaster.DtoThird.DtoDataThird.mpersona.Update("id_persona", id_persona);

                    if (dtoMaster.DtoThird.DtoDataThird.mtercero != null)
                        dtoMaster.DtoThird.DtoDataThird.mtercero.Update("id_persona", id_persona);

                    if (dtoMaster.DtoThird.DtoDataThird.mtercero != null)
                        dtoMaster.DtoThird.DtoDataThird.mtercero.Update("cod_tercero", cod_rol);

                    if (dtoMaster.DtoThird.DtoDataThird.mpersona_requiere_sarlaft != null)
                        dtoMaster.DtoThird.DtoDataThird.mpersona_requiere_sarlaft.Update("id_persona", id_persona);

                    if (dtoMaster.DtoThird.DtoDataThird.mpersona_ciiu != null)
                        dtoMaster.DtoThird.DtoDataThird.mpersona_ciiu.Update("id_persona", id_persona);
                }

                if (dtoMaster.DtoThird.List_Mpersona_dir != null)
                    dtoMaster.DtoThird.List_Mpersona_dir.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoThird.List_Mpersona_telef != null)
                    dtoMaster.DtoThird.List_Mpersona_telef.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoThird.List_Mpersona_email != null)
                    dtoMaster.DtoThird.List_Mpersona_email.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoThird.List_Logbook != null)
                    dtoMaster.DtoThird.List_Logbook.Update(c => { c.ID_PERSONA = id_persona; });

                if (dtoMaster.DtoThird.List_Frm_sarlaft_accionistas_asoc != null)
                    dtoMaster.DtoThird.List_Frm_sarlaft_accionistas_asoc.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoThird.dtoRep != null)
                {
                    if (dtoMaster.DtoThird.dtoRep.mpersona_rep_legal != null)
                        dtoMaster.DtoThird.dtoRep.mpersona_rep_legal.Update("id_persona", id_persona);
                    if (dtoMaster.DtoThird.dtoRep.List_mpersona_rep_legal_dir != null)
                        dtoMaster.DtoThird.dtoRep.List_mpersona_rep_legal_dir.Update(c => { c.id_persona = id_persona; });
                }

                if (dtoMaster.DtoThird.dtoSarlaft != null)
                {
                    if (dtoMaster.DtoThird.dtoSarlaft.List_detalle != null)
                        dtoMaster.DtoThird.dtoSarlaft.List_detalle.Update(c => { c.id_persona = id_persona; });

                    if (dtoMaster.DtoThird.dtoSarlaft.frm_sarlaft_vinculos != null)
                        dtoMaster.DtoThird.dtoSarlaft.frm_sarlaft_vinculos.Update("id_persona", id_persona);

                    if (dtoMaster.DtoThird.dtoSarlaft.frm_sarlaft_aut_incrementos != null)
                        dtoMaster.DtoThird.dtoSarlaft.frm_sarlaft_aut_incrementos.Update("id_persona", id_persona);

                }

                if (dtoMaster.DtoThird.List_Referencias != null)
                    dtoMaster.DtoThird.List_Referencias.Update(c => { c.cod_rol = cod_rol; });

                if (dtoMaster.DtoThird.List_Mpersona_impuesto != null)
                    dtoMaster.DtoThird.List_Mpersona_impuesto.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoThird.List_Mpersona_actividad != null)
                    dtoMaster.DtoThird.List_Mpersona_actividad.Update(c => { c.id_persona = id_persona; });

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
                        if (dtoMaster.DtoThird.DtoDataThird != null)
                        {
                            ListTableMessage = setDto.dtoDataThird(dtoMaster.DtoThird.DtoDataThird, ListTableMessage);
                            if (ListTableMessage.Count(c => c.NameTable == "Mtercero" && c.Message != "True") > 0)
                                if (dtoMaster.DtoThird.DtoDataThird.mpersona.State.Equals('V') || !dtoMaster.DtoThird.DtoDataThird.mtercero.State.Equals('V'))
                                    cod_rolref = -1;
                        }

                        if (cod_rolref != -1)
                        {
                            if (dtoMaster.DtoThird.List_Mpersona_dir != null)
                                ListTableMessage = setDto.List_Mpersona_dir(dtoMaster.DtoThird.List_Mpersona_dir, ListTableMessage);

                            if (dtoMaster.DtoThird.List_Mpersona_telef != null)
                                ListTableMessage = setDto.List_Mpersona_telef(dtoMaster.DtoThird.List_Mpersona_telef, ListTableMessage);

                            if (dtoMaster.DtoThird.List_Mpersona_email != null)
                                ListTableMessage = setDto.List_Mpersona_email(dtoMaster.DtoThird.List_Mpersona_email, ListTableMessage);

                            if (dtoMaster.DtoThird.List_Logbook != null)
                                ListTableMessage = setDto.List_Logbook(dtoMaster.DtoThird.List_Logbook, ListTableMessage);

                            if (dtoMaster.DtoThird.List_Frm_sarlaft_accionistas_asoc != null)
                                ListTableMessage = setDto.List_Frm_sarlaft_accionistas_asoc(dtoMaster.DtoThird.List_Frm_sarlaft_accionistas_asoc, ListTableMessage);

                            if (dtoMaster.DtoThird.dtoSarlaft != null)
                                ListTableMessage = setDto.dtoSarlaft(dtoMaster.DtoThird.dtoSarlaft, ListTableMessage);

                            if (dtoMaster.DtoThird.List_Referencias != null)
                                ListTableMessage = setDto.List_Referencias(dtoMaster.DtoThird.List_Referencias, ListTableMessage);

                            if (dtoMaster.DtoThird.dtoRep != null)
                                ListTableMessage = setDto.dtoRep(dtoMaster.DtoThird.dtoRep, ListTableMessage);

                            if (dtoMaster.DtoThird.List_Mpersona_impuesto != null)
                                ListTableMessage = setDto.List_Mpersona_impuesto(dtoMaster.DtoThird.List_Mpersona_impuesto, ListTableMessage);

                            if (dtoMaster.DtoThird.List_Mpersona_actividad != null)
                                ListTableMessage = setDto.List_Mpersona_actividad(dtoMaster.DtoThird.List_Mpersona_actividad, ListTableMessage);

                            if (
                                    (
                                        (dtoMaster.DtoThird.List_Mpersona_dir != null && dtoMaster.DtoThird.List_Mpersona_dir.Count > 0) ||
                                        (dtoMaster.DtoThird.List_Mpersona_telef != null && dtoMaster.DtoThird.List_Mpersona_telef.Count > 0) ||
                                        (dtoMaster.DtoThird.List_Mpersona_email != null && dtoMaster.DtoThird.List_Mpersona_email.Count > 0)
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
