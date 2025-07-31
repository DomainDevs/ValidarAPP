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
    public class GSDtoProvider
    {
        #region constructores

        public GSDtoProvider()
        {

        }

        public GSDtoProvider(string user, int idApp, int idRol)
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

        public DtoProvider GetDtoProvider(int id_rol, string cod_rol, string entity)
        {
            DtoProvider dtoProvider = new DtoProvider();

            //Carga el Dto solo con una entidad especifica
            if (entity != "")
            {
                dtoProvider = GetEntity(id_rol, cod_rol, entity);
            }
            else
            {
                //Carga el Dto Totalmente nuevo
                if (cod_rol == "new")
                    dtoProvider = getDto.DtoProvider();
                //Carga el Dto con toda la informacion del Rol buscado
                else
                {
                    dtoProvider = GetDto(id_rol, cod_rol);
                }
            }
            return dtoProvider;
        }

        private DtoProvider GetDto(int id_rol, string cod_rol)
        {
            DtoProvider dtoProvider = new DtoProvider();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataTable DtRolesAPP = DGeneric.GetListRolApp(IdRol);
            string App = string.Empty;

            foreach (DataRow dr in DtRolesAPP.Rows)
            {
                App = dr[0].ToString();
                switch (App)
                {
                    case "SISE2G":
                        GetDtoSISE2G(id_rol, cod_rol, ref dtoProvider);
                        break;

                    case "SISE3G":
                        GetDtoSISE3G(id_rol, cod_rol, ref dtoProvider);
                        break;

                }
            }

            return dtoProvider;
        }

        private void GetDtoSISE2G(int id_rol, string cod_rol, ref DtoProvider dtoProvider)
        {
            int id_persona = getDto.id_persona(id_rol, cod_rol);
            dtoProvider.dtoDataProvider = getDto.dtoDataProvider(Convert.ToInt32(cod_rol));
            dtoProvider.dtoDataPerson = getDto.dtoDataPerson(id_persona);
            dtoProvider.List_Mpersona_dir = getDto.List_Mpersona_dir(id_persona);
            dtoProvider.List_Mpersona_telef = getDto.List_Mpersona_telef(id_persona);
            dtoProvider.List_Mpersona_email = getDto.List_Mpersona_email(id_persona);
            dtoProvider.List_Logbook = getDto.List_Logbook(id_persona, id_rol);
            dtoProvider.dtoSarlaft = getDto.dtoSarlaft(id_persona);
            dtoProvider.List_Mpersona_impuesto = getDto.List_Mpersona_impuesto(id_persona);
            dtoProvider.List_Mpres_cpto = getDto.List_Mpres_cpto(id_persona);
            dtoProvider.dtoRep = getDto.dtoRep(id_persona);
            dtoProvider.List_Frm_sarlaft_accionistas_asoc = getDto.List_Frm_sarlaft_accionistas_asoc(id_persona);
            dtoProvider.List_Referencias = getDto.List_Referencias(Convert.ToInt32(cod_rol), id_rol);
            dtoProvider.List_Mcesionario = getDto.List_Mcesionario(id_persona);
            dtoProvider.List_Mpersona_cuentas_bancarias = getDto.List_Mpersona_cuentas_bancarias(id_persona);
            dtoProvider.List_Mpersona_actividad = getDto.List_Mpersona_actividad(id_persona);
        }

        private void GetDtoSISE3G(int id_rol, string cod_rol, ref DtoProvider dtoProvider)
        {
            //logica de consulta para anexar campos de 3G
        }

        private DtoProvider GetEntity(int id_rol, string cod_rol, string entity)
        {
            DtoProvider dtoProvider = new DtoProvider();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataTable DtRolesAPP = DGeneric.GetListRolApp(IdRol);
            string App = string.Empty;

            foreach (DataRow dr in DtRolesAPP.Rows)
            {
                App = dr[0].ToString();
                switch (App)
                {
                    case "SISE2G":
                        GetEntitySISE2G(id_rol, cod_rol, entity, ref dtoProvider);
                        break;

                    case "SISE3G":
                        GetEntitySISE3G(id_rol, cod_rol, entity, ref dtoProvider);
                        break;

                }
            }

            return dtoProvider;
        }

        private void GetEntitySISE2G(int id_rol, string cod_rol, string entity, ref DtoProvider dtoProvider)
        {
            int id_persona = getDto.id_persona(id_rol, cod_rol);
            switch (entity)
            {
                case "dtoDataPerson":
                    dtoProvider.dtoDataPerson = getDto.dtoDataPerson(id_persona);
                    break;
                case "dtoDataProvider":
                    dtoProvider.dtoDataProvider = getDto.dtoDataProvider(Convert.ToInt32(cod_rol));
                    break;
                case "List_Mpersona_dir":
                    dtoProvider.List_Mpersona_dir = getDto.List_Mpersona_dir(id_persona);
                    break;
                case "List_Mpersona_telef":
                    dtoProvider.List_Mpersona_telef = getDto.List_Mpersona_telef(id_persona);
                    break;
                case "List_Mpersona_email":
                    dtoProvider.List_Mpersona_email = getDto.List_Mpersona_email(id_persona);
                    break;
                case "List_Logbook":
                    dtoProvider.List_Logbook = getDto.List_Logbook(id_persona, id_rol);
                    break;
                case "dtoSarlaft":
                    dtoProvider.dtoSarlaft = getDto.dtoSarlaft(id_persona);
                    break;
                case "dtoSarlaftForm":
                    dtoProvider.dtoSarlaft = getDto.dtoSarlaftForm(Convert.ToInt32(cod_rol));
                    break;
                case "List_Mpersona_impuesto":
                    dtoProvider.List_Mpersona_impuesto = getDto.List_Mpersona_impuesto(id_persona);
                    break;
                case "List_Mpres_cpto":
                    dtoProvider.List_Mpres_cpto = getDto.List_Mpres_cpto(id_persona);
                    break;
                case "dtoRep":
                    dtoProvider.dtoRep = getDto.dtoRep(id_persona);
                    break;
                case "List_Frm_sarlaft_accionistas_asoc":
                    dtoProvider.List_Frm_sarlaft_accionistas_asoc = getDto.List_Frm_sarlaft_accionistas_asoc(id_persona);
                    break;
                case "List_Referencias":
                    dtoProvider.List_Referencias = getDto.List_Referencias(Convert.ToInt32(cod_rol), id_rol);
                    break;
                case "List_Mcesionario":
                    dtoProvider.List_Mcesionario = getDto.List_Mcesionario(id_persona);
                    break;
                case "List_Mpersona_cuentas_bancarias":
                    dtoProvider.List_Mpersona_cuentas_bancarias = getDto.List_Mpersona_cuentas_bancarias(id_persona);
                    break;
                case "List_Mpersona_actividad":
                    dtoProvider.List_Mpersona_actividad = getDto.List_Mpersona_actividad(id_persona);
                    break;
            }
        }

        private void GetEntitySISE3G(int id_rol, string cod_rol, string entity, ref DtoProvider dtoProvider)
        {
            //aqui codigo de otra base de datos llenando campos extras

        }

        public SUPMessages SetDtoProvider(DtoMaster dtoMaster)
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
                            ListTableMessage = setDto.UpdateBasicDataInSise2G(dtoMaster.DtoProvider.dtoDataProvider.mpersona, ListTableMessage); //UPDATE mpersona
                            //Se valida que el unico rol que posee es de SISE 2G, de ser asi se actualizan los datos basicos en la data de SISE 3G
                            if (DtRolesAPP.Rows.Count == 1)
                            {
                                individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoProvider.dtoDataProvider.mpersona.id_persona);
                                if (individualId3G > 0)
                                {
                                    ListTableMessage = setDto.UpdateBasicDataInSise3G(dtoMaster.DtoProvider.dtoDataProvider.mpersona, individualId3G, ListTableMessage); //UPDATE UP.PERSON || UP.COMPANY, UP.CO_COMPANY

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
                            individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoProvider.dtoDataProvider.mpersona.id_persona);
                            if (individualId3G > 0)
                            {
                                ListTableMessage = setDto.UpdateBasicDataInSise3G(dtoMaster.DtoProvider.dtoDataProvider.mpersona, individualId3G, ListTableMessage);

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
                individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoProvider.dtoDataProvider.mpersona.id_persona);
                if (individualId3G > 0)
                {
                    ListTableMessage = setDto.UpdateAddressAndPhoneSise3G(dtoMaster.DtoProvider.List_Mpersona_dir, dtoMaster.DtoProvider.List_Mpersona_telef, dtoMaster.DtoProvider.List_Mpersona_email, individualId3G, ListTableMessage);
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

                int cod_rol = cod_rolref;
                int id_persona = id_personaref;
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
                    if (dtoMaster.DtoProvider.dtoDataProvider.mpersona != null)
                    {
                        if (dtoMaster.DtoProvider.dtoDataProvider.mpersona.id_persona == 0)
                        {
                            DGeneric = new DataGenericExecute(App);
                            id_persona = DGeneric.GetConsecutive("mpersona");
                        }
                        else
                        {
                            id_persona = dtoMaster.DtoProvider.dtoDataProvider.mpersona.id_persona;
                        }
                    }

                    DGeneric = new DataGenericExecute(App);
                    cod_rol = DGeneric.GetConsecutive("mpres");
                    cod_rolref = cod_rol;
                }
                id_personaref = id_persona;

                //Insertamos o Actualizamos id_persona y cod_rol
                if (dtoMaster.DtoProvider.dtoDataProvider != null)
                {
                    if (dtoMaster.DtoProvider.dtoDataProvider.mpersona != null)
                        dtoMaster.DtoProvider.dtoDataProvider.mpersona.Update("id_persona", id_persona);

                    if (dtoMaster.DtoProvider.dtoDataProvider.mpres != null)
                        dtoMaster.DtoProvider.dtoDataProvider.mpres.Update("id_persona", id_persona);

                    if (dtoMaster.DtoProvider.dtoDataProvider.mpres != null)
                        dtoMaster.DtoProvider.dtoDataProvider.mpres.Update("cod_pres", cod_rol);

                    if (dtoMaster.DtoProvider.dtoDataProvider.mpersona_ciiu != null)
                        dtoMaster.DtoProvider.dtoDataProvider.mpersona_ciiu.Update("id_persona", id_persona);

                    if (dtoMaster.DtoProvider.dtoDataProvider.mpersona_requiere_sarlaft != null)
                        dtoMaster.DtoProvider.dtoDataProvider.mpersona_requiere_sarlaft.Update("id_persona", id_persona);

                }

                if (dtoMaster.DtoProvider.dtoDataPerson != null)
                {
                    if (dtoMaster.DtoProvider.dtoDataPerson.listMaseg_deporte != null)
                        dtoMaster.DtoProvider.dtoDataPerson.listMaseg_deporte.Update(c => c.cod_aseg = cod_rol);

                    if (dtoMaster.DtoProvider.dtoDataPerson.maseg_ficha_tec_financ != null)
                        dtoMaster.DtoProvider.dtoDataPerson.maseg_ficha_tec_financ.Update("cod_aseg", cod_rol);

                    if (dtoMaster.DtoProvider.dtoDataPerson.tipo_persona_aseg != null)
                        dtoMaster.DtoProvider.dtoDataPerson.tipo_persona_aseg.Update("cod_aseg", cod_rol);
                }

                if (dtoMaster.DtoProvider.List_Mpersona_dir != null)
                    dtoMaster.DtoProvider.List_Mpersona_dir.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoProvider.List_Mpersona_telef != null)
                    dtoMaster.DtoProvider.List_Mpersona_telef.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoProvider.List_Mpersona_email != null)
                    dtoMaster.DtoProvider.List_Mpersona_email.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoProvider.List_Logbook != null)
                    dtoMaster.DtoProvider.List_Logbook.Update(c => { c.ID_PERSONA = id_persona; });

                if (dtoMaster.DtoProvider.List_Frm_sarlaft_accionistas_asoc != null)
                    dtoMaster.DtoProvider.List_Frm_sarlaft_accionistas_asoc.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoProvider.List_Mpersona_impuesto != null)
                    dtoMaster.DtoProvider.List_Mpersona_impuesto.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoProvider.List_Mpres_cpto != null)
                    dtoMaster.DtoProvider.List_Mpres_cpto.Update(c => { c.cod_pres = cod_rol; });

                if (dtoMaster.DtoProvider.List_Mpersona_cuentas_bancarias != null)
                    dtoMaster.DtoProvider.List_Mpersona_cuentas_bancarias.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoProvider.dtoRep != null)
                {
                    if (dtoMaster.DtoProvider.dtoRep.mpersona_rep_legal != null)
                        dtoMaster.DtoProvider.dtoRep.mpersona_rep_legal.Update("id_persona", id_persona);
                    if (dtoMaster.DtoProvider.dtoRep.List_mpersona_rep_legal_dir != null)
                        dtoMaster.DtoProvider.dtoRep.List_mpersona_rep_legal_dir.Update(c => { c.id_persona = id_persona; });
                }

                if (dtoMaster.DtoProvider.dtoSarlaft != null)
                {
                    if (dtoMaster.DtoProvider.dtoSarlaft.List_detalle != null)
                        dtoMaster.DtoProvider.dtoSarlaft.List_detalle.Update(c => { c.id_persona = id_persona; });

                    if (dtoMaster.DtoProvider.dtoSarlaft.frm_sarlaft_vinculos != null)
                        dtoMaster.DtoProvider.dtoSarlaft.frm_sarlaft_vinculos.Update("id_persona", id_persona);

                    if (dtoMaster.DtoProvider.dtoSarlaft.frm_sarlaft_aut_incrementos != null)
                        dtoMaster.DtoProvider.dtoSarlaft.frm_sarlaft_aut_incrementos.Update("id_persona", id_persona);

                }

                if (dtoMaster.DtoProvider.List_Referencias != null)
                    dtoMaster.DtoProvider.List_Referencias.Update(c => { c.cod_rol = cod_rol; });

                
                if (dtoMaster.DtoProvider.List_Mpersona_actividad != null)
                    dtoMaster.DtoProvider.List_Mpersona_actividad.Update(c => { c.id_persona = id_persona; });

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
                        if (dtoMaster.DtoProvider.dtoDataProvider != null)
                        {
                            ListTableMessage = setDto.dtoDataProvider(dtoMaster.DtoProvider.dtoDataProvider, ListTableMessage);
                            if (ListTableMessage.Count(c => c.NameTable == "Mpres" && c.Message != "True") > 0)
                                if (dtoMaster.DtoProvider.dtoDataProvider.mpersona.State.Equals('V') || !dtoMaster.DtoProvider.dtoDataProvider.mpres.State.Equals('V'))
                                    cod_rolref = -1;
                        }

                        if (cod_rolref != -1)
                        {
                            if (dtoMaster.DtoProvider.dtoDataPerson != null)
                                ListTableMessage = setDto.dtoDataPerson(dtoMaster.DtoProvider.dtoDataPerson, ListTableMessage);

                            if (dtoMaster.DtoProvider.List_Mpersona_dir != null)
                                ListTableMessage = setDto.List_Mpersona_dir(dtoMaster.DtoProvider.List_Mpersona_dir, ListTableMessage);

                            if (dtoMaster.DtoProvider.List_Mpersona_telef != null)
                                ListTableMessage = setDto.List_Mpersona_telef(dtoMaster.DtoProvider.List_Mpersona_telef, ListTableMessage);

                            if (dtoMaster.DtoProvider.List_Mpersona_email != null)
                                ListTableMessage = setDto.List_Mpersona_email(dtoMaster.DtoProvider.List_Mpersona_email, ListTableMessage);

                            if (dtoMaster.DtoProvider.List_Logbook != null)
                                ListTableMessage = setDto.List_Logbook(dtoMaster.DtoProvider.List_Logbook, ListTableMessage);

                            if (dtoMaster.DtoProvider.List_Frm_sarlaft_accionistas_asoc != null)
                                ListTableMessage = setDto.List_Frm_sarlaft_accionistas_asoc(dtoMaster.DtoProvider.List_Frm_sarlaft_accionistas_asoc, ListTableMessage);

                            if (dtoMaster.DtoProvider.List_Mpersona_impuesto != null)
                                ListTableMessage = setDto.List_Mpersona_impuesto(dtoMaster.DtoProvider.List_Mpersona_impuesto, ListTableMessage);

                            if (dtoMaster.DtoProvider.List_Mpres_cpto != null)
                                ListTableMessage = setDto.List_Mpres_cpto(dtoMaster.DtoProvider.List_Mpres_cpto, ListTableMessage);

                            if (dtoMaster.DtoProvider.List_Mpersona_cuentas_bancarias != null)
                                ListTableMessage = setDto.List_Mpersona_cuentas_bancarias(dtoMaster.DtoProvider.List_Mpersona_cuentas_bancarias, ListTableMessage);

                            if (dtoMaster.DtoProvider.dtoSarlaft != null)
                                ListTableMessage = setDto.dtoSarlaft(dtoMaster.DtoProvider.dtoSarlaft, ListTableMessage);

                            if (dtoMaster.DtoProvider.dtoRep != null)
                                ListTableMessage = setDto.dtoRep(dtoMaster.DtoProvider.dtoRep, ListTableMessage);

                            if (dtoMaster.DtoProvider.List_Referencias != null)
                                ListTableMessage = setDto.List_Referencias(dtoMaster.DtoProvider.List_Referencias, ListTableMessage);

                            if (dtoMaster.DtoProvider.List_Mpersona_actividad != null)
                                ListTableMessage = setDto.List_Mpersona_actividad(dtoMaster.DtoProvider.List_Mpersona_actividad, ListTableMessage);

                            if (
                                    (
                                        (dtoMaster.DtoProvider.List_Mpersona_dir != null && dtoMaster.DtoProvider.List_Mpersona_dir.Count > 0) ||
                                        (dtoMaster.DtoProvider.List_Mpersona_telef != null && dtoMaster.DtoProvider.List_Mpersona_telef.Count > 0) ||
                                        (dtoMaster.DtoProvider.List_Mpersona_email != null && dtoMaster.DtoProvider.List_Mpersona_email.Count > 0)
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
