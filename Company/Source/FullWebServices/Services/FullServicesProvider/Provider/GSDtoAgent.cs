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
    public class GSDtoAgent
    {
        #region constructores

        public GSDtoAgent()
        {

        }

        public GSDtoAgent(string user, int idApp, int idRol)
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

        GetDto getDto = new GetDto(IdRol);
        DataGenericExecute DGeneric;

        #endregion

        #region Funciones y Procedimientos

        public DtoAgent GetDtoAgent(int id_rol, string cod_rol, string entity)
        {
            DtoAgent dtoAgent = new DtoAgent();

            //Carga el Dto solo con una entidad especifica
            if (entity != "")
            {
                dtoAgent = GetEntity(id_rol, cod_rol, entity);
            }
            else
            {
                //Carga el Dto Totalmente nuevo
                if (cod_rol == "new")
                    dtoAgent = getDto.DtoAgent();
                //Carga el Dto con toda la informacion del Rol buscado
                else
                {
                    dtoAgent = GetDto(id_rol, cod_rol);
                }
            }
            return dtoAgent;
        }

        private DtoAgent GetDto(int id_rol, string cod_rol)
        {
            DtoAgent dtoAgent = new DtoAgent();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataTable DtRolesAPP = DGeneric.GetListRolApp(IdRol);
            string App = string.Empty;

            foreach (DataRow dr in DtRolesAPP.Rows)
            {
                App = dr[0].ToString();
                switch (App)
                {
                    case "SISE2G":
                        GetDtoSISE2G(id_rol, cod_rol, ref dtoAgent);
                        break;

                    case "SISE3G":
                        GetDtoSISE3G(id_rol, cod_rol, ref dtoAgent);
                        break;

                }
            }

            return dtoAgent;
        }

        private void GetDtoSISE2G(int id_rol, string cod_rol, ref DtoAgent dtoAgent)
        {
            int id_persona = getDto.id_persona(id_rol, cod_rol);
            dtoAgent.dtoDataAgent = getDto.dtoDataAgent(Convert.ToInt32(cod_rol));
            dtoAgent.dtoDataPerson = getDto.dtoDataPerson(id_persona);
            dtoAgent.List_Mpersona_dir = getDto.List_Mpersona_dir(id_persona);
            dtoAgent.List_Mpersona_telef = getDto.List_Mpersona_telef(id_persona);
            dtoAgent.List_Mpersona_email = getDto.List_Mpersona_email(id_persona);
            dtoAgent.List_Logbook = getDto.List_Logbook(id_persona, id_rol);
            dtoAgent.dtoSarlaft = getDto.dtoSarlaft(id_persona);
            dtoAgent.List_Frm_sarlaft_accionistas_asoc = getDto.List_Frm_sarlaft_accionistas_asoc(id_persona);
            dtoAgent.List_Mpersona_cuentas_bancarias = getDto.List_Mpersona_cuentas_bancarias(id_persona);
            dtoAgent.List_Referencias = getDto.List_Referencias(Convert.ToInt32(cod_rol), id_rol);
            dtoAgent.List_Magente_producto = getDto.List_Magente_producto(id_persona);
            dtoAgent.List_Magente_organizador = getDto.List_Magente_organizador(Convert.ToInt32(cod_rol));
            dtoAgent.List_Magente_comision = getDto.List_Magente_comision(Convert.ToInt32(cod_rol));
            dtoAgent.dtoRep = getDto.dtoRep(id_persona);
            dtoAgent.List_Mpersona_impuesto = getDto.List_Mpersona_impuesto(id_persona);
            dtoAgent.List_Magente_ramo = getDto.List_Magente_ramo(Convert.ToInt32(cod_rol));
            dtoAgent.List_Mpersona_actividad = getDto.List_Mpersona_actividad(id_persona);

        }

        private void GetDtoSISE3G(int id_rol, string cod_rol, ref DtoAgent dtoAgent)
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
            dtoAgent.List_AGENT_AGENCY = getDto.List_AGENT_AGENCY(id_persona);
            dtoAgent.List_Mpersona_dir = getDto.dtoDataAddress3g(id_persona, dtoAgent.List_Mpersona_dir);
            dtoAgent.List_Mpersona_telef = getDto.dtoDataPhone3g(id_persona, dtoAgent.List_Mpersona_telef);
            dtoAgent.List_Mpersona_email = getDto.dtoDataMail3g(id_persona, dtoAgent.List_Mpersona_email);
            //dtoAgent.dtoDataAgent = getDto.dtoData3g(id_persona, dtoAgent);
            dtoAgent.dtoDataAgent.mpersona = getDto.dtoData3g(id_persona, dtoAgent.dtoDataAgent.mpersona);
        }

        private DtoAgent GetEntity(int id_rol, string cod_rol, string entity)
        {
            DtoAgent dtoAgent = new DtoAgent();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataTable DtRolesAPP = DGeneric.GetListRolApp(IdRol);
            string App = string.Empty;

            foreach (DataRow dr in DtRolesAPP.Rows)
            {
                App = dr[0].ToString();
                switch (App)
                {
                    case "SISE2G":
                        GetEntitySISE2G(id_rol, cod_rol, entity, ref dtoAgent);
                        break;

                    case "SISE3G":
                        GetEntitySISE3G(id_rol, cod_rol, entity, ref dtoAgent);
                        break;

                }
            }

            return dtoAgent;
        }

        private void GetEntitySISE2G(int id_rol, string cod_rol, string entity, ref DtoAgent dtoAgent)
        {
            int id_persona = getDto.id_persona(id_rol, cod_rol);
            switch (entity)
            {
                case "dtoDataAgent":
                    dtoAgent.dtoDataAgent = getDto.dtoDataAgent(Convert.ToInt32(cod_rol));
                    break;
                case "dtoDataPerson":
                    dtoAgent.dtoDataPerson = getDto.dtoDataPerson(id_persona);
                    break;
                case "List_Mpersona_dir":
                    dtoAgent.List_Mpersona_dir = getDto.List_Mpersona_dir(id_persona);
                    break;
                case "List_Mpersona_telef":
                    dtoAgent.List_Mpersona_telef = getDto.List_Mpersona_telef(id_persona);
                    break;
                case "List_Mpersona_email":
                    dtoAgent.List_Mpersona_email = getDto.List_Mpersona_email(id_persona);
                    break;
                case "List_Logbook":
                    dtoAgent.List_Logbook = getDto.List_Logbook(id_persona, id_rol);
                    break;
                case "dtoSarlaft":
                    dtoAgent.dtoSarlaft = getDto.dtoSarlaft(id_persona);
                    break;
                case "dtoSarlaftForm":
                    dtoAgent.dtoSarlaft = getDto.dtoSarlaftForm(Convert.ToInt32(cod_rol));
                    break;
                case "List_Frm_sarlaft_accionistas_asoc":
                    dtoAgent.List_Frm_sarlaft_accionistas_asoc = getDto.List_Frm_sarlaft_accionistas_asoc(id_persona);
                    break;
                case "List_Mpersona_cuentas_bancarias":
                    dtoAgent.List_Mpersona_cuentas_bancarias = getDto.List_Mpersona_cuentas_bancarias(id_persona);
                    break;
                case "List_Magente_producto":
                    dtoAgent.List_Magente_producto = getDto.List_Magente_producto(id_persona);
                    break;
                case "List_Magente_organizador":
                    dtoAgent.List_Magente_organizador = getDto.List_Magente_organizador(Convert.ToInt32(cod_rol));
                    break;
                case "List_Magente_comision":
                    dtoAgent.List_Magente_comision = getDto.List_Magente_comision(Convert.ToInt32(cod_rol));
                    break;
                case "List_Mpersona_impuesto":
                    dtoAgent.List_Mpersona_impuesto = getDto.List_Mpersona_impuesto(id_persona);
                    break;
                case "dtoRep":
                    dtoAgent.dtoRep = getDto.dtoRep(id_persona);
                    break;
                case "List_Magente_ramo":
                    dtoAgent.List_Magente_ramo = getDto.List_Magente_ramo(Convert.ToInt32(cod_rol));
                    break;
                case "List_Referencias":
                    dtoAgent.List_Referencias = getDto.List_Referencias(Convert.ToInt32(cod_rol), id_rol);
                    break;
                case "List_Mpersona_actividad":
                    dtoAgent.List_Mpersona_actividad = getDto.List_Mpersona_actividad(id_persona);
                    break;
            }
        }

        private void GetEntitySISE3G(int id_rol, string cod_rol, string entity, ref DtoAgent dtoAgent)
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
                case "dtoDataAgent":
                    //dtoAgent.dtoDataAgent = getDto.dtoData3g(id_persona, dtoAgent);
                    dtoAgent.dtoDataAgent.mpersona = getDto.dtoData3g(id_persona, dtoAgent.dtoDataAgent.mpersona);
                    break;
                case "List_AGENT_AGENCY":
                    dtoAgent.List_AGENT_AGENCY = getDto.List_AGENT_AGENCY(id_persona);
                    break;
                case "List_Mpersona_dir":
                    dtoAgent.List_Mpersona_dir = getDto.dtoDataAddress3g(id_persona, dtoAgent.List_Mpersona_dir);
                    break;
                case "List_Mpersona_telef":
                    dtoAgent.List_Mpersona_telef = getDto.dtoDataPhone3g(id_persona, dtoAgent.List_Mpersona_telef);
                    break;

                case "List_Mpersona_email":
                    dtoAgent.List_Mpersona_email = getDto.dtoDataMail3g(id_persona, dtoAgent.List_Mpersona_email);
                    break;
            }
        }

        public SUPMessages SetDtoAgent(DtoMaster dtoMaster)
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
                            ListTableMessage = setDto.UpdateBasicDataInSise2G(dtoMaster.DtoAgent.dtoDataAgent.mpersona, ListTableMessage); //UPDATE mpersona
                            //Se valida que el unico rol que posee es de SISE 2G, de ser asi se actualizan los datos basicos en la data de SISE 3G
                            if (DtRolesAPP.Rows.Count == 1)
                            {
                                individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoAgent.dtoDataAgent.mpersona.id_persona);
                                if (individualId3G > 0)
                                {
                                    ListTableMessage = setDto.UpdateBasicDataInSise3G(dtoMaster.DtoAgent.dtoDataAgent.mpersona, individualId3G, ListTableMessage); //UPDATE UP.PERSON || UP.COMPANY, UP.CO_COMPANY

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
                                SetSISE3G(dtoMaster, PrefixConn2, ListTableMessage, ref cod_rol, ref id_persona);
                        }
                        else
                        {
                            setDto = new SetDto(User, App, IdRol);
                            individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoAgent.dtoDataAgent.mpersona.id_persona);
                            if (individualId3G > 0)
                            {
                                ListTableMessage = setDto.UpdateBasicDataInSise3G(dtoMaster.DtoAgent.dtoDataAgent.mpersona, individualId3G, ListTableMessage);

                            }
                        }
                        //Autor: Edward Rubiano; Fecha: 10/05/2016; Asunto: C11226; Descripcion: Validaciones para guardado unico de datos basicos
                        _SUPMessages.INDIVIDUAL_ID = id_persona.ToString();
                        break;

                }
            }

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

                int cod_rol = cod_rolref;
                int id_persona = id_personaref;
                bool exist = false;
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
                    if (dtoMaster.DtoAgent.dtoDataAgent.mpersona != null)
                    {
                        if (dtoMaster.DtoAgent.dtoDataAgent.mpersona.id_persona == 0)
                        {
                            DGeneric = new DataGenericExecute(App);
                            id_persona = DGeneric.GetConsecutive("mpersona");
                        }
                        else
                        {
                            id_persona = dtoMaster.DtoAgent.dtoDataAgent.mpersona.id_persona;
                            exist = true;
                        }
                    }

                    DGeneric = new DataGenericExecute(App);
                    cod_rol = DGeneric.GetConsecutive("magente");
                    cod_rolref = cod_rol;
                }
                id_personaref = id_persona;
                //Insertamos o Actualizamos id_persona y cod_rol
                if (dtoMaster.DtoAgent.dtoDataAgent != null)
                {
                    if (dtoMaster.DtoAgent.dtoDataAgent.mpersona != null)
                        dtoMaster.DtoAgent.dtoDataAgent.mpersona.Update("id_persona", id_persona);

                    if (dtoMaster.DtoAgent.dtoDataAgent.mpersona_requiere_sarlaft != null)
                        dtoMaster.DtoAgent.dtoDataAgent.mpersona_requiere_sarlaft.Update("id_persona", id_persona);

                    if (dtoMaster.DtoAgent.dtoDataAgent.mpersona_ciiu != null)
                        dtoMaster.DtoAgent.dtoDataAgent.mpersona_ciiu.Update("id_persona", id_persona);

                    if (dtoMaster.DtoAgent.dtoDataAgent.magente != null)
                        dtoMaster.DtoAgent.dtoDataAgent.magente.Update("id_persona", id_persona);

                    if (dtoMaster.DtoAgent.dtoDataAgent.magente != null)
                        dtoMaster.DtoAgent.dtoDataAgent.magente.Update("cod_agente", cod_rol);
                }

                if (dtoMaster.DtoAgent.dtoDataPerson != null)
                {
                    if (dtoMaster.DtoAgent.dtoDataPerson.listMaseg_deporte != null)
                        dtoMaster.DtoAgent.dtoDataPerson.listMaseg_deporte.Update(c => c.cod_aseg = cod_rol);

                    if (dtoMaster.DtoAgent.dtoDataPerson.maseg_ficha_tec_financ != null)
                        dtoMaster.DtoAgent.dtoDataPerson.maseg_ficha_tec_financ.Update("cod_aseg", cod_rol);

                    if (dtoMaster.DtoAgent.dtoDataPerson.tipo_persona_aseg != null)
                        dtoMaster.DtoAgent.dtoDataPerson.tipo_persona_aseg.Update("cod_aseg", cod_rol);
                }

                if (dtoMaster.DtoAgent.List_Mpersona_dir != null)
                    dtoMaster.DtoAgent.List_Mpersona_dir.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoAgent.List_Mpersona_telef != null)
                    dtoMaster.DtoAgent.List_Mpersona_telef.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoAgent.List_Mpersona_email != null)
                    dtoMaster.DtoAgent.List_Mpersona_email.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoAgent.List_Magente_producto != null)
                    dtoMaster.DtoAgent.List_Magente_producto.Update(c => { c.cod_agente = id_persona; });

                if (dtoMaster.DtoAgent.List_Magente_comision != null)
                {
                    dtoMaster.DtoAgent.List_Magente_comision.Update(c => { c.cod_agente = cod_rol; });
                }

                if (dtoMaster.DtoAgent.List_Magente_organizador != null)
                {
                    dtoMaster.DtoAgent.List_Magente_organizador.Update(c => { c.cod_agente = cod_rol; });
                }

                if (dtoMaster.DtoAgent.List_Mpersona_impuesto != null)
                    dtoMaster.DtoAgent.List_Mpersona_impuesto.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoAgent.List_Logbook != null)
                    dtoMaster.DtoAgent.List_Logbook.Update(c => { c.ID_PERSONA = id_persona; });

                if (dtoMaster.DtoAgent.List_Frm_sarlaft_accionistas_asoc != null)
                    dtoMaster.DtoAgent.List_Frm_sarlaft_accionistas_asoc.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoAgent.List_Mpersona_cuentas_bancarias != null)
                    dtoMaster.DtoAgent.List_Mpersona_cuentas_bancarias.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoAgent.dtoRep != null)
                {
                    if (dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal != null)
                        dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.Update("id_persona", id_persona);
                    if (dtoMaster.DtoAgent.dtoRep.List_mpersona_rep_legal_dir != null)
                        dtoMaster.DtoAgent.dtoRep.List_mpersona_rep_legal_dir.Update(c => { c.id_persona = id_persona; });
                }

                if (dtoMaster.DtoAgent.dtoSarlaft != null)
                {
                    if (dtoMaster.DtoAgent.dtoSarlaft.List_detalle != null)
                        dtoMaster.DtoAgent.dtoSarlaft.List_detalle.Update(c => { c.id_persona = id_persona; });

                    if (dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_vinculos != null)
                        dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_vinculos.Update("id_persona", id_persona);

                    if (dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_aut_incrementos != null)
                        dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_aut_incrementos.Update("id_persona", id_persona);
                }

                if (dtoMaster.DtoAgent.List_Referencias != null)
                    dtoMaster.DtoAgent.List_Referencias.Update(c => { c.cod_rol = cod_rol; });

                if (dtoMaster.DtoAgent.List_Magente_ramo != null)
                    dtoMaster.DtoAgent.List_Magente_ramo.Update(c => { c.cod_agente = cod_rol; });

                if (dtoMaster.DtoAgent.List_Mpersona_actividad != null)
                    dtoMaster.DtoAgent.List_Mpersona_actividad.Update(c => { c.id_persona = id_persona; });

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

                        if (dtoMaster.DtoAgent.dtoDataAgent != null)
                        {
                            ListTableMessage = setDto.dtoDataAgent(dtoMaster.DtoAgent.dtoDataAgent, ListTableMessage);
                            if (ListTableMessage.Count(c => c.NameTable == "Magente" && c.Message != "True") > 0)
                            {
                                if (!dtoMaster.DtoAgent.dtoDataAgent.magente.State.Equals('V') && ListTableMessage.Count(c => c.NameTable == "Magente" && c.Message != "True") > 0)
                                {
                                    cod_rolref = -1;
                                }
                                if (dtoMaster.DtoAgent.dtoDataAgent.mpersona.State.Equals('V'))
                                {
                                    cod_rolref = -1;
                                    if (!dtoMaster.DtoAgent.dtoDataAgent.mpersona.State.Equals('V'))
                                    {
                                        cod_rolref = -1;
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
                        }

                        if (cod_rol != -1)
                        {
                            if (dtoMaster.DtoAgent.dtoDataPerson != null)
                                ListTableMessage = setDto.dtoDataPerson(dtoMaster.DtoAgent.dtoDataPerson, ListTableMessage);

                            if (dtoMaster.DtoAgent.List_Mpersona_dir != null)
                                ListTableMessage = setDto.List_Mpersona_dir(dtoMaster.DtoAgent.List_Mpersona_dir, ListTableMessage);

                            if (dtoMaster.DtoAgent.List_Mpersona_telef != null)
                                ListTableMessage = setDto.List_Mpersona_telef(dtoMaster.DtoAgent.List_Mpersona_telef, ListTableMessage);

                            if (dtoMaster.DtoAgent.List_Mpersona_email != null)
                                ListTableMessage = setDto.List_Mpersona_email(dtoMaster.DtoAgent.List_Mpersona_email, ListTableMessage);

                            if (dtoMaster.DtoAgent.List_Magente_producto != null)
                                ListTableMessage = setDto.List_Magente_producto(dtoMaster.DtoAgent.List_Magente_producto, ListTableMessage);

                            if (dtoMaster.DtoAgent.List_Magente_comision != null)
                                ListTableMessage = setDto.List_Magente_comision(dtoMaster.DtoAgent.List_Magente_comision, ListTableMessage);

                            if (dtoMaster.DtoAgent.List_Magente_organizador != null)
                                ListTableMessage = setDto.List_Magente_organizador(dtoMaster.DtoAgent.List_Magente_organizador, ListTableMessage);

                            if (dtoMaster.DtoAgent.List_Mpersona_impuesto != null)
                                ListTableMessage = setDto.List_Mpersona_impuesto(dtoMaster.DtoAgent.List_Mpersona_impuesto, ListTableMessage);

                            if (dtoMaster.DtoAgent.List_Logbook != null)
                                ListTableMessage = setDto.List_Logbook(dtoMaster.DtoAgent.List_Logbook, ListTableMessage);

                            if (dtoMaster.DtoAgent.List_Frm_sarlaft_accionistas_asoc != null)
                                ListTableMessage = setDto.List_Frm_sarlaft_accionistas_asoc(dtoMaster.DtoAgent.List_Frm_sarlaft_accionistas_asoc, ListTableMessage);

                            if (dtoMaster.DtoAgent.List_Mpersona_cuentas_bancarias != null)
                                ListTableMessage = setDto.List_Mpersona_cuentas_bancarias(dtoMaster.DtoAgent.List_Mpersona_cuentas_bancarias, ListTableMessage);

                            if (dtoMaster.DtoAgent.dtoSarlaft != null)
                                ListTableMessage = setDto.dtoSarlaft(dtoMaster.DtoAgent.dtoSarlaft, ListTableMessage);

                            if (dtoMaster.DtoAgent.List_Referencias != null)
                                ListTableMessage = setDto.List_Referencias(dtoMaster.DtoAgent.List_Referencias, ListTableMessage);

                            if (dtoMaster.DtoAgent.dtoRep != null)
                                ListTableMessage = setDto.dtoRep(dtoMaster.DtoAgent.dtoRep, ListTableMessage);

                            if (dtoMaster.DtoAgent.List_Magente_ramo != null)
                                ListTableMessage = setDto.List_Magente_ramo(dtoMaster.DtoAgent.List_Magente_ramo, ListTableMessage);

                            if (dtoMaster.DtoAgent.List_Mpersona_actividad != null)
                                ListTableMessage = setDto.List_Mpersona_actividad(dtoMaster.DtoAgent.List_Mpersona_actividad, ListTableMessage);

                            if (
                                    (
                                        (dtoMaster.DtoAgent.List_Mpersona_dir != null && dtoMaster.DtoAgent.List_Mpersona_dir.Count > 0) ||
                                        (dtoMaster.DtoAgent.List_Mpersona_telef != null && dtoMaster.DtoAgent.List_Mpersona_telef.Count > 0) ||
                                        (dtoMaster.DtoAgent.List_Mpersona_email != null && dtoMaster.DtoAgent.List_Mpersona_email.Count > 0)
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

        private List<TableMessage> SetSISE3G(DtoMaster dtoMaster, string App, List<TableMessage> ListTableMessage, ref int cod_rolref, ref int id_personaref)
        {
            if (ListTableMessage.Count(c => c.Message != "True") == 0)
            {
                TableMessage tableMessage = new TableMessage();
                TableMessage tableMessage2G = new TableMessage();
                tableMessage.NameTable = "Proceso 3G";
                tableMessage.Message = "True";
                try
                {

                    int cod_rol = cod_rolref;
                    int id_persona = id_personaref;
                    DGeneric = new DataGenericExecute(App);
                    int consecutiveA;
                    int rol;
                    int individual_id = 0;
                    int individual_id3g = 0;
                    int id = 0;
                    char state_person = 'C';
                    char state_agent = 'C';
                    char state_agency = 'C';
                    string IsSave = string.Empty;
                    DataSet ds = new DataSet();
                    char state_individual = 'C';
                    char state_requiere = 'C';

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

                            //Insertamos o Actualizamos id_persona y cod_rol
                            #region dtoMaster.DtoAgent.dtoDataAgent != null'
                            if (dtoMaster.DtoAgent.dtoDataAgent != null)
                            {
                                if (dtoMaster.DtoAgent.dtoDataAgent.mpersona != null)
                                {
                                    if (!dtoMaster.DtoAgent.dtoDataAgent.mpersona.State.Equals('V'))
                                    {
                                        ds = new DataSet();

                                        ds = statefor3g(App, dtoMaster.DtoAgent.dtoDataAgent.mpersona.cod_tipo_persona == "F" ? "1" : "2",
                                            dtoMaster.DtoAgent.dtoDataAgent.mpersona.cod_tipo_doc, dtoMaster.DtoAgent.dtoDataAgent.mpersona.nro_doc != null ? dtoMaster.DtoAgent.dtoDataAgent.mpersona.nro_doc :
                                            (dtoMaster.DtoAgent.dtoDataAgent.mpersona.nro_nit.Substring(0, dtoMaster.DtoAgent.dtoDataAgent.mpersona.nro_nit.Length - 1)), cod_rol, Convert.ToInt32(dtoMaster.DtoAgent.dtoDataAgent.magente.cod_suc));

                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            individual_id = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                                            if (individual_id != 0) { id_persona = individual_id; } //Edward Rubiano -- HD3500 -- 09/11/2015
                                            id = cod_rolref;
                                            individual_id3g = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                                            state_person = Convert.ToChar(ds.Tables[0].Rows[0][2]);
                                            state_agency = Convert.ToChar(ds.Tables[0].Rows[0][3]);
                                            state_agent = Convert.ToChar(ds.Tables[0].Rows[0][4]);
                                            state_requiere = Convert.ToChar(ds.Tables[0].Rows[0][5]);
                                            //state_individual = Convert.ToChar(ds.Tables[0].Rows[0][6]);
                                        }

                                        IsSave = "True";
                                        if (dtoMaster.DtoAgent.dtoDataAgent.mpersona.State.Equals('C') && state_person.Equals('C') ||
                                             dtoMaster.DtoAgent.dtoDataAgent.mpersona.State.Equals('U') && state_person.Equals('U') ||
                                             dtoMaster.DtoAgent.dtoDataAgent.mpersona.State.Equals('U') && state_person.Equals('C') ||
                                            dtoMaster.DtoAgent.dtoDataAgent.mpersona.State.Equals('U') && state_person.Equals('U'))
                                        {
                                            INDIVIDUAL _INDIVIDUAL = new INDIVIDUAL();
                                            _INDIVIDUAL.State = state_person;
                                            if (state_person.Equals('C'))
                                            {
                                                _INDIVIDUAL.INDIVIDUAL_ID = id_persona;
                                            }

                                            if (state_person.Equals('U'))
                                            {
                                                if (id_persona == individual_id)
                                                {
                                                    _INDIVIDUAL.INDIVIDUAL_ID = id_persona;
                                                }
                                                else
                                                {
                                                    _INDIVIDUAL.INDIVIDUAL_ID = individual_id;
                                                }
                                            }
                                            //_INDIVIDUAL.INDIVIDUAL_ID = dtoMaster.DtoAgent.dtoDataAgent.mpersona.id_persona;
                                            _INDIVIDUAL.INDIVIDUAL_TYPE_CD = (dtoMaster.DtoAgent.dtoDataAgent.mpersona.cod_tipo_persona == "F" ? 1 : 2);
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
                                                DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_DATAID3G", listParameter, Command);
                                                INDIVIDUAL INDIV = Dtl.ConvertTo<INDIVIDUAL>(DsReturn.Tables[0])[0];
                                                _INDIVIDUAL.AT_DATA_ID = INDIV.AT_DATA_ID;

                                            }

                                            _INDIVIDUAL.AT_PAYMENT_ID = 1;
                                            _INDIVIDUAL.AT_AGENT_AGENCY_ID = 1;
                                            //_INDIVIDUAL.OWNER_ROLE_CD = null;
                                            /*if (dtoMaster.DtoAgent.dtoDataAgent.mpersona_ciiu != null)
                                            {
                                                _INDIVIDUAL.ECONOMIC_ACTIVITY_CD = Convert.ToString(dtoMaster.DtoAgent.dtoDataAgent.mpersona_ciiu.cod_ciiu_ppal_nuevo);
                                            }*/
                                            _INDIVIDUAL.ECONOMIC_ACTIVITY_CD = null;

                                            List<INDIVIDUAL> List_INDIVIDUAL = new List<INDIVIDUAL>();
                                            List_INDIVIDUAL.Add(_INDIVIDUAL);

                                            ListTableMessage = setDto.List_INDIVIDUAL(List_INDIVIDUAL, ListTableMessage);

                                            TableMessage result = ListTableMessage.Find(delegate(TableMessage bk) { return bk.NameTable == "INDIVIDUAL"; });
                                            IsSave = result.Message;

                                            if (IsSave == "True")
                                            {
                                                if (dtoMaster.DtoAgent.dtoDataAgent.mpersona_ciiu != null)
                                                {
                                                    CO_PRV_INDIVIDUAL _CO_PRV_INDIVIDUAL = new CO_PRV_INDIVIDUAL();
                                                    _CO_PRV_INDIVIDUAL.State = state_person;
                                                    if (state_person.Equals('C'))
                                                    {
                                                        _CO_PRV_INDIVIDUAL.INDIVIDUAL_ID = id_persona;
                                                    }

                                                    if (state_person.Equals('U'))
                                                    {
                                                        if (id_persona == individual_id)
                                                        {
                                                            _CO_PRV_INDIVIDUAL.INDIVIDUAL_ID = id_persona;
                                                        }
                                                        else
                                                        {
                                                            _CO_PRV_INDIVIDUAL.INDIVIDUAL_ID = individual_id;
                                                        }
                                                    }
                                                    //_CO_PRV_INDIVIDUAL.INDIVIDUAL_ID = id_persona;
                                                    _CO_PRV_INDIVIDUAL.ECONOMIC_ACTIVITY_CD_NEW = dtoMaster.DtoAgent.dtoDataAgent.mpersona_ciiu.cod_ciiu_ppal_nuevo;
                                                    _CO_PRV_INDIVIDUAL.SECOND_ECONOMIC_ACTIVITY_CD_NEW = dtoMaster.DtoAgent.dtoDataAgent.mpersona_ciiu.cod_ciiu_scndria_nuevo;

                                                    List<CO_PRV_INDIVIDUAL> List_CO_PRV_INDIVIDUAL = new List<CO_PRV_INDIVIDUAL>();
                                                    List_CO_PRV_INDIVIDUAL.Add(_CO_PRV_INDIVIDUAL);
                                                    ListTableMessage = setDto.List_CO_PRV_INDIVIDUAL(List_CO_PRV_INDIVIDUAL, ListTableMessage);
                                                }


                                                if (dtoMaster.DtoAgent.dtoDataAgent.mpersona.cod_tipo_persona == "F")
                                                {
                                                    _INDIVIDUAL.INDIVIDUAL_TYPE_CD = 1;
                                                    PERSON _PERSON = new PERSON();
                                                    _PERSON.State = state_person;
                                                    if (state_person.Equals('C'))
                                                    {
                                                        _PERSON.INDIVIDUAL_ID = id_persona;
                                                    }

                                                    if (state_person.Equals('U'))
                                                    {
                                                        if (id_persona == individual_id)
                                                        {
                                                            _PERSON.INDIVIDUAL_ID = id_persona;
                                                        }
                                                        else
                                                        {
                                                            _PERSON.INDIVIDUAL_ID = individual_id;
                                                        }
                                                    }
                                                    //_PERSON.INDIVIDUAL_ID = id_persona;
                                                    _PERSON.SURNAME = dtoMaster.DtoAgent.dtoDataAgent.mpersona.txt_apellido1;
                                                    _PERSON.NAME = dtoMaster.DtoAgent.dtoDataAgent.mpersona.txt_nombre;
                                                    _PERSON.GENDER = dtoMaster.DtoAgent.dtoDataAgent.mpersona.txt_sexo;
                                                    _PERSON.ID_CARD_TYPE_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoAgent.dtoDataAgent.mpersona.cod_tipo_doc));
                                                    _PERSON.ID_CARD_NO = dtoMaster.DtoAgent.dtoDataAgent.mpersona.nro_doc;
                                                    _PERSON.MARITAL_STATUS_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoAgent.dtoDataAgent.mpersona.cod_est_civil));
                                                    _PERSON.BIRTH_DATE = dtoMaster.DtoAgent.dtoDataAgent.mpersona.fec_nac == null ? Convert.ToString(DateTime.Now) : dtoMaster.DtoAgent.dtoDataAgent.mpersona.fec_nac;
                                                    _PERSON.MOTHER_LAST_NAME = dtoMaster.DtoAgent.dtoDataAgent.mpersona.txt_apellido2;
                                                    _PERSON.BIRTH_PLACE = dtoMaster.DtoAgent.dtoDataAgent.mpersona.txt_lugar_nac;
                                                    _PERSON.CHILDREN = "0";

                                                    List<PERSON> List_PERSON = new List<PERSON>();
                                                    List_PERSON.Add(_PERSON);
                                                    ListTableMessage = setDto.List_PERSON(List_PERSON, ListTableMessage);

                                                    result = ListTableMessage.Find(delegate(TableMessage bk) { return bk.NameTable == "PERSON"; });
                                                    IsSave = result.Message;

                                                    if (IsSave == "True")
                                                    {
                                                        if (dtoMaster.DtoAgent.dtoDataAgent.mpersona.txt_cia_tra != null)
                                                        {
                                                            PERSON_JOB _PERSON_JOB = new PERSON_JOB();
                                                            _PERSON_JOB.State = state_person;
                                                            if (!state_person.Equals('U'))
                                                            {
                                                                _PERSON_JOB.INDIVIDUAL_ID = id_persona;
                                                            }

                                                            if (id_persona == individual_id)
                                                            {
                                                                _PERSON_JOB.INDIVIDUAL_ID = id_persona;
                                                            }
                                                            else
                                                            {
                                                                _PERSON_JOB.INDIVIDUAL_ID = individual_id;
                                                            }
                                                            //_PERSON_JOB.INDIVIDUAL_ID = id_persona;
                                                            _PERSON_JOB.OCCUPATION_CD = 99;
                                                            _PERSON_JOB.COMPANY_NAME = dtoMaster.DtoAgent.dtoDataAgent.mpersona.txt_cia_tra;
                                                            _PERSON_JOB.JOB_SECTOR = dtoMaster.DtoAgent.dtoDataAgent.mpersona.txt_dpto_tra;
                                                            _PERSON_JOB.POSITION = dtoMaster.DtoAgent.dtoDataAgent.mpersona.txt_puesto_tra;
                                                            _PERSON_JOB.CONTACT = dtoMaster.DtoAgent.dtoDataAgent.mpersona.txt_asistente_tra;

                                                            List<PERSON_JOB> List_PERSON_JOB = new List<PERSON_JOB>();
                                                            List_PERSON_JOB.Add(_PERSON_JOB);
                                                            ListTableMessage = setDto.List_PERSON_JOB(List_PERSON_JOB, ListTableMessage);
                                                        }
                                                    }
                                                }
                                                else if (dtoMaster.DtoAgent.dtoDataAgent.mpersona.cod_tipo_persona == "J")
                                                {
                                                    _INDIVIDUAL.INDIVIDUAL_TYPE_CD = 2;
                                                    COMPANY _COMPANY = new COMPANY();
                                                    _COMPANY.State = state_person;
                                                    if (state_person.Equals('C'))
                                                    {
                                                        _COMPANY.INDIVIDUAL_ID = id_persona;
                                                    }

                                                    if (state_person.Equals('U'))
                                                    {
                                                        if (id_persona == individual_id)
                                                        {
                                                            _COMPANY.INDIVIDUAL_ID = id_persona;
                                                        }
                                                        else
                                                        {
                                                            _COMPANY.INDIVIDUAL_ID = individual_id;
                                                        }
                                                    }
                                                    //_COMPANY.INDIVIDUAL_ID = id_persona;
                                                    _COMPANY.TRADE_NAME = dtoMaster.DtoAgent.dtoDataAgent.mpersona.txt_apellido1 + ' ' + dtoMaster.DtoAgent.dtoDataAgent.mpersona.txt_apellido2 + ' ' + dtoMaster.DtoAgent.dtoDataAgent.mpersona.txt_nombre;
                                                    _COMPANY.TRIBUTARY_ID_TYPE_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoAgent.dtoDataAgent.mpersona.cod_tipo_doc));
                                                    _COMPANY.TRIBUTARY_ID_NO = dtoMaster.DtoAgent.dtoDataAgent.mpersona.nro_nit.Substring(0, dtoMaster.DtoAgent.dtoDataAgent.mpersona.nro_nit.Length - 1);
                                                    _COMPANY.COUNTRY_CD = 1;
                                                    _COMPANY.COMPANY_TYPE_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoAgent.dtoDataAgent.mpersona.COMPANY_TYPE_CD == "-1" ? "1" : dtoMaster.DtoAgent.dtoDataAgent.mpersona.COMPANY_TYPE_CD));


                                                    List<COMPANY> List_COMPANY = new List<COMPANY>();
                                                    List_COMPANY.Add(_COMPANY);
                                                    ListTableMessage = setDto.List_COMPANY(List_COMPANY, ListTableMessage);

                                                    result = ListTableMessage.Find(delegate(TableMessage bk) { return bk.NameTable == "COMPANY"; });
                                                    IsSave = result.Message;

                                                    if (IsSave == "True")
                                                    {
                                                        CO_COMPANY _CO_COMPANY = new CO_COMPANY();
                                                        _CO_COMPANY.State = state_person;
                                                        if (state_person.Equals('C'))
                                                        {
                                                            _CO_COMPANY.INDIVIDUAL_ID = id_persona;
                                                        }

                                                        if (state_person.Equals('U'))
                                                        {
                                                            if (id_persona == individual_id)
                                                            {
                                                                _CO_COMPANY.INDIVIDUAL_ID = id_persona;
                                                            }
                                                            else
                                                            {
                                                                _CO_COMPANY.INDIVIDUAL_ID = individual_id;
                                                            }
                                                        }
                                                        //_CO_COMPANY.INDIVIDUAL_ID = id_persona;
                                                        _CO_COMPANY.VERIFY_DIGIT = dtoMaster.DtoAgent.dtoDataAgent.mpersona.VERIFY_DIGIT == "-1" ? "0" : dtoMaster.DtoAgent.dtoDataAgent.mpersona.VERIFY_DIGIT;
                                                        _CO_COMPANY.ASSOCIATION_TYPE_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoAgent.dtoDataAgent.mpersona.ASSOCIATION_TYPE_CD));
                                                        if (dtoMaster.DtoAgent.dtoDataAgent.mpersona.CATEGORY_CD != "0")
                                                        {
                                                            _CO_COMPANY.CATEGORY_CD = dtoMaster.DtoAgent.dtoDataAgent.mpersona.CATEGORY_CD == "2" ? "1" : "2";
                                                        }
                                                        else
                                                        {
                                                            _CO_COMPANY.CATEGORY_CD = dtoMaster.DtoAgent.dtoDataAgent.mpersona.CATEGORY_CD;
                                                        }

                                                        _CO_COMPANY.ENTITY_OFFICIAL_CD = "0";

                                                        List<CO_COMPANY> List_CO_COMPANY = new List<CO_COMPANY>();
                                                        List_CO_COMPANY.Add(_CO_COMPANY);
                                                        ListTableMessage = setDto.List_CO_COMPANY(List_CO_COMPANY, ListTableMessage);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if (IsSave == "True")
                                {
                                    if (dtoMaster.DtoAgent.dtoDataAgent.magente != null)
                                    {
                                        if (!dtoMaster.DtoAgent.dtoDataAgent.magente.State.Equals('V'))
                                        {

                                            AGENT _AGENT = new AGENT();
                                            _AGENT.State = state_agent;
                                            if (state_agent.Equals('C'))
                                            {
                                                _AGENT.INDIVIDUAL_ID = id_persona;
                                            }

                                            if (state_agent.Equals('U'))
                                            {
                                                if (id_persona == individual_id)
                                                {
                                                    _AGENT.INDIVIDUAL_ID = id_persona;
                                                }
                                                else
                                                {
                                                    _AGENT.INDIVIDUAL_ID = individual_id;
                                                }
                                            }
                                            //_AGENT.INDIVIDUAL_ID = id_persona;
                                            _AGENT.AGENT_TYPE_CD = Convert.ToInt32(dtoMaster.DtoAgent.dtoDataAgent.magente.cod_tipo_agente);
                                            _AGENT.CHECK_PAYABLE_TO = dtoMaster.DtoAgent.dtoDataAgent.magente.txt_cheque_a_nom;

                                            if (dtoMaster.DtoAgent.dtoDataAgent.magente.cod_estado.Equals("A"))
                                            {
                                                _AGENT.ENTERED_DATE = dtoMaster.DtoAgent.dtoDataAgent.magente.fec_alta;
                                                _AGENT.DECLINED_DATE = null;
                                            }
                                            else if (dtoMaster.DtoAgent.dtoDataAgent.magente.cod_estado.Equals("B"))
                                            {
                                                _AGENT.DECLINED_DATE = dtoMaster.DtoAgent.dtoDataAgent.magente.fec_baja;
                                                _AGENT.ENTERED_DATE = dtoMaster.DtoAgent.dtoDataAgent.magente.fec_alta;
                                            }
                                            else if (dtoMaster.DtoAgent.dtoDataAgent.magente.cod_estado.Equals("I"))
                                            {
                                                _AGENT.DECLINED_DATE = dtoMaster.DtoAgent.dtoDataAgent.magente.fec_inactivacion;
                                                _AGENT.ENTERED_DATE = dtoMaster.DtoAgent.dtoDataAgent.magente.fec_alta;
                                            }

                                            if (Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoAgent.dtoDataAgent.magente.cod_baja)) == 0 || Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoAgent.dtoDataAgent.magente.cod_baja)) == 3 ||
                                                Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoAgent.dtoDataAgent.magente.cod_baja)) == 4 || Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoAgent.dtoDataAgent.magente.cod_baja)) == 7 ||
                                                Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoAgent.dtoDataAgent.magente.cod_baja)) == 19)
                                            {
                                                _AGENT.AGENT_DECLINED_TYPE_CD = null;
                                                if (dtoMaster.DtoAgent.dtoDataAgent.magente.cod_estado.Equals("I"))
                                                    _AGENT.AGENT_DECLINED_TYPE_CD = "5";
                                            }
                                            else
                                            {
                                                _AGENT.AGENT_DECLINED_TYPE_CD = dtoMaster.DtoAgent.dtoDataAgent.magente.cod_baja;
                                                if (dtoMaster.DtoAgent.dtoDataAgent.magente.cod_estado.Equals("I"))
                                                    _AGENT.AGENT_DECLINED_TYPE_CD = "5";

                                            }
                                            _AGENT.LICENSE_NUMBER = dtoMaster.DtoAgent.dtoDataAgent.magente.nro_carnet;
                                            _AGENT.AGENT_GROUP_CD = Convert.ToString(1);

                                            List<AGENT> List_AGENT = new List<AGENT>();
                                            List_AGENT.Add(_AGENT);
                                            ListTableMessage = setDto.List_AGENT(List_AGENT, ListTableMessage);

                                            TableMessage resu = ListTableMessage.Find(delegate(TableMessage bk) { return bk.NameTable == "AGENT"; });
                                            IsSave = resu.Message;

                                            if (IsSave == "True")
                                            {
                                                consecutiveA = Consecutive3g(App, id_persona, id);
                                                AGENT_AGENCY _AGENT_AGENCY = new AGENT_AGENCY();
                                                _AGENT_AGENCY.State = state_agency;
                                                if (state_agency.Equals('C'))
                                                {
                                                    _AGENT_AGENCY.INDIVIDUAL_ID = id_persona;
                                                }

                                                if (state_agency.Equals('U'))
                                                {
                                                    if (id_persona == individual_id)
                                                    {
                                                        _AGENT_AGENCY.INDIVIDUAL_ID = id_persona;
                                                    }
                                                    else
                                                    {
                                                        _AGENT_AGENCY.INDIVIDUAL_ID = individual_id;
                                                    }
                                                }
                                                //_AGENT_AGENCY.INDIVIDUAL_ID = id_persona;
                                                _AGENT_AGENCY.AGENT_AGENCY_ID = consecutiveA;
                                                //Edward Rubiano - Intermediario con guion sin espacio entre id y descripcion -- HD3410
                                                //_AGENT_AGENCY.DESCRIPTION = id + "-" + dtoMaster.DtoAgent.dtoDataAgent.magente.txt_cheque_a_nom.Replace(",", " ");//dtoMaster.DtoAgent.dtoDataAgent.mpersona.txt_apellido1 + " " + dtoMaster.DtoAgent.dtoDataAgent.mpersona.txt_apellido2 + " " + dtoMaster.DtoAgent.dtoDataAgent.mpersona.txt_nombre;
                                                _AGENT_AGENCY.DESCRIPTION = id + " - " + dtoMaster.DtoAgent.dtoDataAgent.magente.txt_cheque_a_nom.Replace(",", " ");//dtoMaster.DtoAgent.dtoDataAgent.mpersona.txt_apellido1 + " " + dtoMaster.DtoAgent.dtoDataAgent.mpersona.txt_apellido2 + " " + dtoMaster.DtoAgent.dtoDataAgent.mpersona.txt_nombre;
                                                _AGENT_AGENCY.BRANCH_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoAgent.dtoDataAgent.magente.cod_suc));

                                                if (dtoMaster.DtoAgent.dtoDataAgent.mpersona != null)
                                                {
                                                    _AGENT_AGENCY.ANNOTATIONS = (dtoMaster.DtoAgent.dtoDataAgent.mpersona.txt_notas == null ? null : dtoMaster.DtoAgent.dtoDataAgent.mpersona.txt_notas);
                                                }
                                                _AGENT_AGENCY.AGENT_CD = id;
                                                _AGENT_AGENCY.AGENT_TYPE_CD = Convert.ToInt32(dtoMaster.DtoAgent.dtoDataAgent.magente.cod_tipo_agente);
                                                if (Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoAgent.dtoDataAgent.magente.cod_baja)) == 0 || Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoAgent.dtoDataAgent.magente.cod_baja)) == 3 ||
                                                    Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoAgent.dtoDataAgent.magente.cod_baja)) == 4 || Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoAgent.dtoDataAgent.magente.cod_baja)) == 7 ||
                                                    Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoAgent.dtoDataAgent.magente.cod_baja)) == 19)
                                                {
                                                    _AGENT_AGENCY.AGENT_DECLINED_TYPE_CD = null;
                                                }
                                                else
                                                {
                                                    _AGENT_AGENCY.AGENT_DECLINED_TYPE_CD = dtoMaster.DtoAgent.dtoDataAgent.magente.cod_baja;
                                                    _AGENT_AGENCY.DECLINED_DATE = dtoMaster.DtoAgent.dtoDataAgent.magente.fec_baja;
                                                }


                                                List<AGENT_AGENCY> List_AGENT_AGENCY = new List<AGENT_AGENCY>();
                                                List_AGENT_AGENCY.Add(_AGENT_AGENCY);
                                                ListTableMessage = setDto.List_AGENT_AGENCY(List_AGENT_AGENCY, ListTableMessage);
                                            }

                                        }
                                    }

                                    TableMessage resul = ListTableMessage.Find(delegate(TableMessage bk) { return bk.NameTable == "INDIVIDUAL"; });
                                    if (resul != null)
                                        IsSave = resul.Message;

                                    if (dtoMaster.DtoAgent.dtoDataAgent.mpersona_requiere_sarlaft != null && IsSave == "True")
                                    {
                                        if (!dtoMaster.DtoAgent.dtoDataAgent.mpersona_requiere_sarlaft.State.Equals('V'))
                                        {
                                            INDIVIDUAL_SARLAFT_EXONERATION _INDIVIDUAL_SARLAFT_EXONERATION = new INDIVIDUAL_SARLAFT_EXONERATION();
                                            _INDIVIDUAL_SARLAFT_EXONERATION.State = state_requiere;
                                            if (state_requiere.Equals('C'))
                                            {
                                                _INDIVIDUAL_SARLAFT_EXONERATION.INDIVIDUAL_ID = id_persona;
                                            }

                                            if (state_requiere.Equals('U'))
                                            {
                                                if (id_persona == individual_id)
                                                {
                                                    _INDIVIDUAL_SARLAFT_EXONERATION.INDIVIDUAL_ID = id_persona;
                                                }
                                                else
                                                {
                                                    _INDIVIDUAL_SARLAFT_EXONERATION.INDIVIDUAL_ID = individual_id;
                                                }
                                            }
                                            //_INDIVIDUAL_SARLAFT_EXONERATION.INDIVIDUAL_ID = id_persona;
                                            _INDIVIDUAL_SARLAFT_EXONERATION.USER_ID = 1;
                                            _INDIVIDUAL_SARLAFT_EXONERATION.EXONERATION_TYPE_CD = dtoMaster.DtoAgent.dtoDataAgent.mpersona_requiere_sarlaft.cod_motivo_exonera;
                                            if (dtoMaster.DtoAgent.dtoDataAgent.mpersona_requiere_sarlaft.sn_exonerado == -1)
                                                _INDIVIDUAL_SARLAFT_EXONERATION.IS_EXONERATED = true;
                                            else
                                                _INDIVIDUAL_SARLAFT_EXONERATION.IS_EXONERATED = false;

                                            _INDIVIDUAL_SARLAFT_EXONERATION.REGISTRATION_DATE = dtoMaster.DtoAgent.dtoDataAgent.mpersona_requiere_sarlaft.fec_modifica;

                                            string RoleCd = !string.IsNullOrEmpty(dtoMaster.DtoAgent.dtoDataAgent.mpersona_requiere_sarlaft.txt_origen) ? dtoMaster.DtoAgent.dtoDataAgent.mpersona_requiere_sarlaft.txt_origen.Substring(0, 1) : null;
                                            if (RoleCd == "C")
                                                _INDIVIDUAL_SARLAFT_EXONERATION.ROLE_CD = 9;
                                            else if (RoleCd == "C")
                                                _INDIVIDUAL_SARLAFT_EXONERATION.ROLE_CD = 2;
                                            else
                                                _INDIVIDUAL_SARLAFT_EXONERATION.ROLE_CD = 1;

                                            List<INDIVIDUAL_SARLAFT_EXONERATION> List_INDIVIDUAL_SARLAFT_EXONERATION = new List<INDIVIDUAL_SARLAFT_EXONERATION>();
                                            List_INDIVIDUAL_SARLAFT_EXONERATION.Add(_INDIVIDUAL_SARLAFT_EXONERATION);
                                            ListTableMessage = setDto.List_INDIVIDUAL_SARLAFT_EXONERATION(List_INDIVIDUAL_SARLAFT_EXONERATION, ListTableMessage);
                                        }
                                    }
                                }
                            }
                            #endregion//fin dtoMaster.DtoAgent.dtoDataAgent != null

                            #region Complemento

                            ds = new DataSet();
                            if (id_persona == individual_id)
                            {
                                ds = agentidperson(App, Convert.ToString(id_persona));
                            }
                            else
                            {
                                ds = agentidperson(App, Convert.ToString(individual_id));
                            }

                            //ds = agentidperson(App, Convert.ToString(id_persona));

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                state_individual = Convert.ToChar(ds.Tables[0].Rows[0][0]);
                                if (state_individual.Equals('U'))
                                    IsSave = "True";
                            }


                            if (dtoMaster.DtoAgent.List_Mpersona_dir != null && IsSave == "True")
                            {
                                List<ADDRESS> List_ADDRESS = new List<ADDRESS>();

                                foreach (Mpersona_dir dir in dtoMaster.DtoAgent.List_Mpersona_dir)
                                {
                                    if (Convert.ToInt32(dir.cod_tipo_dir) != 99)
                                    {
                                        ADDRESS _ADDRESS = new ADDRESS();
                                        _ADDRESS.State = dir.State_3g;

                                        if (dir.State_3g.Equals('U'))
                                        {
                                            if (id_persona == individual_id)
                                            {
                                                _ADDRESS.INDIVIDUAL_ID = id_persona;
                                            }
                                            else
                                            {
                                                _ADDRESS.INDIVIDUAL_ID = individual_id;
                                            }
                                        }
                                        else
                                        {
                                            _ADDRESS.INDIVIDUAL_ID = id_persona;
                                        }

                                        //_ADDRESS.INDIVIDUAL_ID = id_persona;
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

                            if (dtoMaster.DtoAgent.List_Mpersona_telef != null && IsSave == "True")
                            {
                                List<PHONE> List_PHONE = new List<PHONE>();

                                foreach (Mpersona_telef phone in dtoMaster.DtoAgent.List_Mpersona_telef)
                                {
                                    if (phone.txt_telefono != " " && phone.txt_telefono != ".")
                                    {
                                        PHONE _PHONE = new PHONE();
                                        _PHONE.State = phone.State_3g;

                                        if (phone.State_3g.Equals('U'))
                                        {
                                            if (id_persona == individual_id)
                                            {
                                                _PHONE.INDIVIDUAL_ID = id_persona;
                                            }
                                            else
                                            {
                                                _PHONE.INDIVIDUAL_ID = individual_id;
                                            }
                                        }
                                        else
                                        {
                                            _PHONE.INDIVIDUAL_ID = id_persona;
                                        }
                                        //_PHONE.INDIVIDUAL_ID = id_persona;
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

                            if (dtoMaster.DtoAgent.List_Mpersona_email != null && IsSave == "True")
                            {
                                List<EMAIL> List_EMAIL = new List<EMAIL>();

                                foreach (Mpersona_email mail in dtoMaster.DtoAgent.List_Mpersona_email)
                                {
                                    EMAIL _EMAIL = new EMAIL();

                                    _EMAIL.State = mail.State_3g;

                                    if (mail.State_3g.Equals('U'))
                                    {
                                        if (id_persona == individual_id)
                                        {
                                            _EMAIL.INDIVIDUAL_ID = id_persona;
                                        }
                                        else
                                        {
                                            _EMAIL.INDIVIDUAL_ID = individual_id;
                                        }
                                    }
                                    else
                                    {
                                        _EMAIL.INDIVIDUAL_ID = id_persona;
                                    }

                                    //_EMAIL.INDIVIDUAL_ID = id_persona;
                                    _EMAIL.DATA_ID = mail.DATA_ID;
                                    _EMAIL.ADDRESS = mail.txt_direccion_email;
                                    _EMAIL.EMAIL_TYPE_CD = mail.cod_tipo_email;
                                    _EMAIL.IS_MAILING_ADDRESS = mail.IS_MAILING_ADDRESS;

                                    List_EMAIL.Add(_EMAIL);
                                }

                                ListTableMessage = setDto.List_EMAIL(List_EMAIL, ListTableMessage);
                            }

                            if (dtoMaster.DtoAgent.List_Magente_ramo != null && IsSave == "True")
                            {
                                Dictionary<double, string> idsExcluidos = new Dictionary<double, string>();
                                idsExcluidos.Add(8, "");
                                idsExcluidos.Add(16, "");
                                idsExcluidos.Add(17, "");
                                idsExcluidos.Add(18, "");
                                idsExcluidos.Add(19, "");
                                idsExcluidos.Add(20, "");
                                idsExcluidos.Add(21, "");
                                idsExcluidos.Add(22, "");
                                idsExcluidos.Add(23, "");
                                idsExcluidos.Add(24, "");
                                idsExcluidos.Add(25, "");

                                List<Magente_ramo> listaTmp = dtoMaster.DtoAgent.List_Magente_ramo.FindAll(delegate(Magente_ramo r) { return !idsExcluidos.ContainsKey(r.cod_ramo); });
                                List<AGENT_PREFIX> List_AGENT_PREFIX = new List<AGENT_PREFIX>();
                                foreach (Magente_ramo _AGENT_PREFIX in listaTmp.ToList())
                                {
                                    AGENT_PREFIX _PREFIX = new AGENT_PREFIX();
                                    _PREFIX.State = _AGENT_PREFIX.State;
                                    if (_AGENT_PREFIX.State.Equals('U')
                                        || _AGENT_PREFIX.State.Equals('R') //Edward Rubiano -- HD3492 -- 13/11/2015
                                        )
                                    {
                                        //Edward Rubiano -- HD3492 -- 13/11/2015
                                        _PREFIX.State = 'U';
                                        //Edward Rubiano -- 13/11/2015
                                        if (id_persona == individual_id)
                                        {
                                            _PREFIX.INDIVIDUAL_ID = id_persona;
                                        }
                                        else
                                        {
                                            _PREFIX.INDIVIDUAL_ID = individual_id;
                                        }
                                    }
                                    else
                                    {
                                        _PREFIX.INDIVIDUAL_ID = id_persona;
                                    }

                                    //_PREFIX.INDIVIDUAL_ID = id_persona;
                                    _PREFIX.PREFIX_CD = Convert.ToInt32(_AGENT_PREFIX.cod_ramo);

                                    List_AGENT_PREFIX.Add(_PREFIX);

                                }
                                ListTableMessage = setDto.List_AGENT_PREFIX(List_AGENT_PREFIX, ListTableMessage);
                            }

                            if (dtoMaster.DtoAgent.dtoRep != null)
                            {
                                if (dtoMaster.DtoAgent.dtoRep.List_mpersona_rep_legal_dir != null)
                                {
                                    if (dtoMaster.DtoAgent.dtoRep.List_mpersona_rep_legal_dir.Count != 0 && dtoMaster.DtoAgent.dtoRep.List_mpersona_rep_legal_dir[0].cod_tipo_dir != 99 && IsSave == "True")
                                    {

                                        INDIVIDUAL_LEGAL_REPRESENT _INDIVIDUAL_LEGAL_REPRESENT = new INDIVIDUAL_LEGAL_REPRESENT();
                                        _INDIVIDUAL_LEGAL_REPRESENT.State = dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.State_3G;
                                        if (_INDIVIDUAL_LEGAL_REPRESENT.State.Equals('U'))
                                        {
                                            if (id_persona == individual_id)
                                            {
                                                _INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_ID = dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.id_persona;
                                            }
                                            else
                                            {
                                                _INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_ID = individual_id;
                                            }
                                        }
                                        else
                                        {
                                            _INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_ID = dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.id_persona;
                                        }

                                        _INDIVIDUAL_LEGAL_REPRESENT.LEGAL_REPRESENTATIVE_NAME = dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.txt_nombre == null ? "" : dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.txt_nombre;
                                        _INDIVIDUAL_LEGAL_REPRESENT.EXPEDITION_DATE = dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.fec_expedicion_doc == null ? DateTime.Now.ToString("dd/MM/yyyy") : dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.fec_expedicion_doc;
                                        _INDIVIDUAL_LEGAL_REPRESENT.EXPEDITION_PLACE = dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.txt_lugar_expedicion == null ? "" : dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.txt_lugar_expedicion;
                                        _INDIVIDUAL_LEGAL_REPRESENT.BIRTH_DATE = dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.fec_nacimiento == null ? DateTime.Now.ToString("dd/MM/yyyy") : dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.fec_nacimiento;
                                        _INDIVIDUAL_LEGAL_REPRESENT.BIRTH_PLACE = dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.txt_lugar_nacimi == null ? "" : dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.txt_lugar_nacimi;
                                        _INDIVIDUAL_LEGAL_REPRESENT.NATIONALITY = dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.txt_nacionalidad == null ? "" : dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.txt_nacionalidad;
                                        _INDIVIDUAL_LEGAL_REPRESENT.CITY = dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.txt_ciudad == null ? "" : dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.txt_ciudad;
                                        _INDIVIDUAL_LEGAL_REPRESENT.PHONE = dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.txt_telefono == null ? "" : dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.txt_telefono;
                                        _INDIVIDUAL_LEGAL_REPRESENT.JOB_TITLE = dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.txt_cargo == null ? "" : dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.txt_cargo;
                                        _INDIVIDUAL_LEGAL_REPRESENT.CELL_PHONE = dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.txt_celular == null ? "" : dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.txt_celular;
                                        _INDIVIDUAL_LEGAL_REPRESENT.EMAIL = dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.txt_email == null ? "" : dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.txt_email;
                                        _INDIVIDUAL_LEGAL_REPRESENT.ID_CARD_NO = dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.nro_doc_rep_legal == null ? "" : dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.nro_doc_rep_legal;
                                        _INDIVIDUAL_LEGAL_REPRESENT.AUTHORIZATION_AMT = dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.vr_facultades == null ? "" : dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.vr_facultades;
                                        _INDIVIDUAL_LEGAL_REPRESENT.DESCRIPTION = dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.txt_facultades == null ? "" : dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.txt_facultades;
                                        _INDIVIDUAL_LEGAL_REPRESENT.CURRENCY_CD = dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.cod_unidad == "-1" ? null : dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.cod_unidad;
                                        _INDIVIDUAL_LEGAL_REPRESENT.ID_CARD_TYPE_CD = Convert.ToInt32(dtoMaster.DtoAgent.dtoRep.mpersona_rep_legal.cod_tipo_doc);

                                        _INDIVIDUAL_LEGAL_REPRESENT.ADDRESS = dtoMaster.DtoAgent.dtoRep.List_mpersona_rep_legal_dir[0].txt_direccion;
                                        _INDIVIDUAL_LEGAL_REPRESENT.COUNTRY_CD = Convert.ToInt32(dtoMaster.DtoAgent.dtoRep.List_mpersona_rep_legal_dir[0].cod_pais);
                                        _INDIVIDUAL_LEGAL_REPRESENT.STATE_CD = Convert.ToInt32(dtoMaster.DtoAgent.dtoRep.List_mpersona_rep_legal_dir[0].cod_dpto);
                                        _INDIVIDUAL_LEGAL_REPRESENT.CITY_CD = Convert.ToInt32(dtoMaster.DtoAgent.dtoRep.List_mpersona_rep_legal_dir[0].cod_municipio);

                                        List<INDIVIDUAL_LEGAL_REPRESENT> List_INDIVIDUAL_LEGAL_REPRESENT = new List<INDIVIDUAL_LEGAL_REPRESENT>();

                                        List_INDIVIDUAL_LEGAL_REPRESENT.Add(_INDIVIDUAL_LEGAL_REPRESENT);

                                        ListTableMessage = setDto.List_INDIVIDUAL_LEGAL_REPRESENT(List_INDIVIDUAL_LEGAL_REPRESENT, ListTableMessage);
                                    }
                                }
                            }


                            //TableMessage resultAgent = ListTableMessage.Find(delegate(TableMessage bk) { return bk.NameTable == "_AGENT_AGENCY"; });
                            //IsSave = resultAgent.Message;

                            if (dtoMaster.DtoAgent.List_AGENT_AGENCY != null && IsSave == "True" && dtoMaster.DtoAgent.List_AGENT_AGENCY.Count != 0)
                            {
                                if (dtoMaster.DtoAgent.List_AGENT_AGENCY[0].State == 'U')
                                {
                                    foreach (AGENT_AGENCY _AGENT_AGENCY in dtoMaster.DtoAgent.List_AGENT_AGENCY)
                                    {
                                        if (id_persona == individual_id)
                                        {
                                            rol = rolConsecutive(App, id_persona);
                                        }
                                        else
                                        {
                                            rol = rolConsecutive(App, individual_id);
                                        }

                                        if (_AGENT_AGENCY.AGENT_CD == 0) _AGENT_AGENCY.AGENT_CD = rol;
                                        if (_AGENT_AGENCY.AGENT_DECLINED_TYPE_CD == "0") _AGENT_AGENCY.AGENT_DECLINED_TYPE_CD = null;
                                        _AGENT_AGENCY.DECLINED_DATE = dtoMaster.DtoAgent.dtoDataAgent.magente.fec_baja;

                                    }

                                    ListTableMessage = setDto.List_AGENT_AGENCY(dtoMaster.DtoAgent.List_AGENT_AGENCY, ListTableMessage);
                                }
                            }

                            if (dtoMaster.DtoAgent.dtoSarlaft != null)
                            {
                                if (dtoMaster.DtoAgent.dtoSarlaft.List_detalle != null && IsSave == "True")
                                {
                                    List<INDIVIDUAL_SARLAFT> List_INDIVIDUAL_SARLAFT = new List<INDIVIDUAL_SARLAFT>();

                                    foreach (Frm_sarlaft_detalle detalle in dtoMaster.DtoAgent.dtoSarlaft.List_detalle)
                                    {
                                        INDIVIDUAL_SARLAFT individual = new INDIVIDUAL_SARLAFT();

                                        individual.SARLAFT_ID = detalle.id_formulario;
                                        if (detalle.State_3G.Equals('U'))
                                        {
                                            if (id_persona == individual_id)
                                            {
                                                individual.INDIVIDUAL_ID = id_persona;
                                            }
                                            else
                                            {
                                                individual.INDIVIDUAL_ID = individual_id;
                                            }
                                        }
                                        else
                                        {
                                            individual.INDIVIDUAL_ID = id_persona;
                                        }
                                        //individual.INDIVIDUAL_ID = id_persona;
                                        individual.FORM_NUM = Convert.ToString(detalle.nro_formulario);
                                        individual.YEAR = detalle.aaaa_formulario;
                                        individual.REGISTRATION_DATE = detalle.fec_registro;
                                        individual.AUTHORIZED_BY = detalle.txt_usuario_auto;
                                        individual.FILLING_DATE = detalle.fec_diligenciamiento;
                                        individual.CHECK_DATE = detalle.fec_verifica;
                                        individual.VERIFYING_EMPLOYEE = detalle.txt_usuario_veri;
                                        individual.BRANCH_CD = Convert.ToString(detalle.cod_suc);

                                        if (dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_detalle_entrevista != null)
                                        {
                                            individual.INTERVIEW_DATE = dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_detalle_entrevista.fec_entrevista;
                                            individual.INTERVIEWER_NAME = dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_detalle_entrevista.txt_usua_entrev;
                                            individual.INTERVIEW_PLACE = dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_detalle_entrevista.txt_lugar_entrev;
                                            individual.INTERVIEW_RESULT_CD = (dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_detalle_entrevista.txt_resul_entrev == "RECHAZADO" ? 2 : 1);
                                        }

                                        if (dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_info_financiera != null)
                                        {
                                            individual.INTERNATIONAL_OPERATIONS = dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_info_financiera.id_formulario != 0 ? true : false;
                                        }

                                        if (detalle.cod_usuario_sise != null)
                                        {
                                            ds = new DataSet();
                                            string userid = detalle.cod_usuario_sise;
                                            ds = userid_3g(App, userid);

                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                individual.USER_ID = Convert.ToString(ds.Tables[0].Rows[0][0]);
                                            }
                                        }

                                        if (dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_info_financiera != null)
                                        {
                                            individual.ECONOMIC_ACTIVITY_CD = Convert.ToInt32(dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_info_financiera.cod_act_principal);
                                            individual.SECOND_ECONOMIC_ACTIVITY_CD = Convert.ToInt32(dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_info_financiera.cod_act_secundaria == -1 ? 0 : dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_info_financiera.cod_act_secundaria);

                                        }
                                        individual.PENDING_EVENT = dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_aut_incrementos != null ? true : false;
                                        individual.State = detalle.State_3G;

                                        List_INDIVIDUAL_SARLAFT.Add(individual);

                                    }

                                    ListTableMessage = setDto.List_INDIVIDUAL_SARLAFT(List_INDIVIDUAL_SARLAFT, ListTableMessage);

                                }

                                if (dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_vinculos != null && IsSave == "True")
                                {
                                    List<INDIVIDUAL_LINK> List_INDIVIDUAL_LINK = new List<INDIVIDUAL_LINK>();
                                    INDIVIDUAL_LINK _INDIVIDUAL_LINK = new INDIVIDUAL_LINK();
                                    if (dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_vinculos.tomador_asegurado != 0)
                                    {
                                        _INDIVIDUAL_LINK = new INDIVIDUAL_LINK();
                                        if (dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_vinculos.State_3G.Equals('U'))
                                        {
                                            if (id_persona == individual_id)
                                            {
                                                _INDIVIDUAL_LINK.INDIVIDUAL_ID = id_persona;
                                            }
                                            else
                                            {
                                                _INDIVIDUAL_LINK.INDIVIDUAL_ID = individual_id;
                                            }
                                        }
                                        else
                                        {
                                            _INDIVIDUAL_LINK.INDIVIDUAL_ID = id_persona;
                                        }
                                        //_INDIVIDUAL_LINK.INDIVIDUAL_ID = id_persona;
                                        _INDIVIDUAL_LINK.LINK_TYPE_CD = Convert.ToInt32(dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_vinculos.tomador_asegurado);
                                        _INDIVIDUAL_LINK.RELATIONSHIP_SARLAFT_CD = 1;
                                        _INDIVIDUAL_LINK.DESCRIPTION = dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_vinculos.txt_desc_TA;
                                        _INDIVIDUAL_LINK.State = dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_vinculos.State_3G;
                                        List_INDIVIDUAL_LINK.Add(_INDIVIDUAL_LINK);
                                    }
                                    if (dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_vinculos.tomador_benef != 0)
                                    {
                                        _INDIVIDUAL_LINK = new INDIVIDUAL_LINK();
                                        if (dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_vinculos.State_3G.Equals('U'))
                                        {
                                            if (id_persona == individual_id)
                                            {
                                                _INDIVIDUAL_LINK.INDIVIDUAL_ID = id_persona;
                                            }
                                            else
                                            {
                                                _INDIVIDUAL_LINK.INDIVIDUAL_ID = individual_id;
                                            }
                                        }
                                        else
                                        {
                                            _INDIVIDUAL_LINK.INDIVIDUAL_ID = id_persona;
                                        }
                                        //_INDIVIDUAL_LINK.INDIVIDUAL_ID = id_persona;
                                        _INDIVIDUAL_LINK.LINK_TYPE_CD = Convert.ToInt32(dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_vinculos.tomador_benef);
                                        _INDIVIDUAL_LINK.RELATIONSHIP_SARLAFT_CD = 2;
                                        _INDIVIDUAL_LINK.DESCRIPTION = dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_vinculos.txt_desc_TB;
                                        _INDIVIDUAL_LINK.State = dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_vinculos.State_3G;
                                        List_INDIVIDUAL_LINK.Add(_INDIVIDUAL_LINK);
                                    }
                                    if (dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_vinculos.asegurado_benef != 0)
                                    {
                                        _INDIVIDUAL_LINK = new INDIVIDUAL_LINK();
                                        if (dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_vinculos.State_3G.Equals('U'))
                                        {
                                            if (id_persona == individual_id)
                                            {
                                                _INDIVIDUAL_LINK.INDIVIDUAL_ID = id_persona;
                                            }
                                            else
                                            {
                                                _INDIVIDUAL_LINK.INDIVIDUAL_ID = individual_id;
                                            }
                                        }
                                        else
                                        {
                                            _INDIVIDUAL_LINK.INDIVIDUAL_ID = id_persona;
                                        }
                                        //_INDIVIDUAL_LINK.INDIVIDUAL_ID = id_persona;
                                        _INDIVIDUAL_LINK.LINK_TYPE_CD = Convert.ToInt32(dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_vinculos.asegurado_benef);
                                        _INDIVIDUAL_LINK.RELATIONSHIP_SARLAFT_CD = 3;
                                        _INDIVIDUAL_LINK.DESCRIPTION = dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_vinculos.txt_desc_AB;
                                        _INDIVIDUAL_LINK.State = dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_vinculos.State_3G;
                                        List_INDIVIDUAL_LINK.Add(_INDIVIDUAL_LINK);
                                    }

                                    ListTableMessage = setDto.List_INDIVIDUAL_LINK(List_INDIVIDUAL_LINK, ListTableMessage);
                                }
                                if (dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_info_financiera != null && IsSave == "True")
                                {
                                    FINANCIAL_SARLAFT _FINANCIAL_SARLAFT = new FINANCIAL_SARLAFT();

                                    _FINANCIAL_SARLAFT.SARLAFT_ID = dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_info_financiera.id_formulario;
                                    _FINANCIAL_SARLAFT.INCOME_AMT = dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_info_financiera.imp_ingresos;
                                    _FINANCIAL_SARLAFT.EXPENSE_AMT = dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_info_financiera.imp_egresos;
                                    _FINANCIAL_SARLAFT.EXTRA_INCOME_AMT = dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_info_financiera.imp_otros_ingr;
                                    _FINANCIAL_SARLAFT.ASSETS_AMT = dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_info_financiera.imp_activos;
                                    _FINANCIAL_SARLAFT.LIABILITIES_AMT = dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_info_financiera.imp_pasivos;
                                    _FINANCIAL_SARLAFT.DESCRIPTION = dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_detalle_entrevista.txt_obser_entrev == null ? "" : dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_detalle_entrevista.txt_obser_entrev;
                                    _FINANCIAL_SARLAFT.State = dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_detalle_entrevista.State_3G;

                                    List<FINANCIAL_SARLAFT> List_FINANCIAL_SARLAFT = new List<FINANCIAL_SARLAFT>();
                                    List_FINANCIAL_SARLAFT.Add(_FINANCIAL_SARLAFT);

                                    /*if (dtoMaster.DtoAgent.dtoSarlaft.frm_sarlaft_info_financiera.id_formulario != 0)
                                    {
                                        int typeindividual = (dtoMaster.DtoAgent.dtoDataAgent.mpersona.cod_tipo_persona == "F" ? 1 : 2);
                                        if (typeindividual == 1)
                                        {
                                            ListTableMessage = setDto.List_OPERATING_QUOTA_SARLAFT(List_FINANCIAL_SARLAFT, ListTableMessage);
                                        }
                                    }*/

                                    ListTableMessage = setDto.List_FINANCIAL_SARLAFT(List_FINANCIAL_SARLAFT, ListTableMessage);

                                }

                                if (dtoMaster.DtoAgent.dtoSarlaft.List_oper_internacionales != null && IsSave == "True")
                                {
                                    List<SARLAFT_OPERATION> List_SARLAFT_OPERATION = new List<SARLAFT_OPERATION>();

                                    foreach (Frm_sarlaft_oper_internac internat in dtoMaster.DtoAgent.dtoSarlaft.List_oper_internacionales)
                                    {
                                        SARLAFT_OPERATION operation = new SARLAFT_OPERATION();

                                        operation.SARLAFT_OPERATION_ID = Convert.ToInt32(internat.consecutivo_oper);
                                        operation.SARLAFT_ID = internat.id_formulario;
                                        operation.PRODUCT_NUM = internat.nro_producto;
                                        operation.PRODUCT_AMT = internat.imp_producto;
                                        operation.ENTITY = internat.txt_entidad;
                                        operation.OPERATION_TYPE_CD = Convert.ToInt32(internat.cod_tipo_operacion);
                                        operation.PRODUCT_TYPE_CD = Convert.ToInt32(internat.cod_tipo_producto);
                                        operation.CURRENCY_CD = Convert.ToInt32(internat.cod_moneda);
                                        operation.COUNTRY_CD = Convert.ToInt32(internat.cod_pais_origen);
                                        operation.STATE_CD = Convert.ToInt32(internat.cod_dpto);
                                        operation.CITY_CD = Convert.ToInt32(internat.cod_municipio);
                                        operation.State = internat.State_3G;

                                        List_SARLAFT_OPERATION.Add(operation);

                                    }

                                    ListTableMessage = setDto.List_SARLAFT_OPERATION(List_SARLAFT_OPERATION, ListTableMessage);
                                }
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

        private DataSet statefor3g(string App, string cod_tipo_persona, string cod_tipo_doc, string nro_doc, int cod_rol, int suc)
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
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "AGENT_CD";
            parameter.Value = Convert.ToString(cod_rol);
            listParameter.Add(parameter);
            parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "BRANCH_CD";
            parameter.Value = Convert.ToString(suc);
            listParameter.Add(parameter);
            ds = DGeneric.ExecuteStoreProcedure("SUP.GET_CONSEC_AGENT", listParameter);

            return ds;

        }

        private int Consecutive3g(string App, int id_persona, int agent_cd)
        {
            int consecutiveA = 0;
            DGeneric = new DataGenericExecute(App);
            DataSet ds = new DataSet();
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "id_persona";
            parameter.Value = Convert.ToString(id_persona);
            listParameter.Add(parameter);
            parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "agent_cd";
            parameter.Value = Convert.ToString(agent_cd);
            listParameter.Add(parameter);
            ds = DGeneric.ExecuteStoreProcedure("SUP.GET_CONSECUTIVEA", listParameter);

            if (ds.Tables[0].Rows.Count > 0)
                if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) == 0)
                    return consecutiveA = 1;

            return consecutiveA = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        }

        private int rolConsecutive(string App, int id_persona)
        {
            int rol = 0;
            DGeneric = new DataGenericExecute(App);
            DataSet ds = new DataSet();
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "id_persona";
            parameter.Value = Convert.ToString(id_persona);
            listParameter.Add(parameter);
            ds = DGeneric.ExecuteStoreProcedure("SUP.GET_CONSECUTIVEROl", listParameter);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return rol = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }
            return rol;
        }

        private DataSet userid_3g(string App, string cod_usuario)
        {
            DGeneric = new DataGenericExecute(App);
            DataSet ds = new DataSet();
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.VarChar";
            parameter.Parameter = "COD_USUARIO";
            parameter.Value = (cod_usuario);
            listParameter.Add(parameter);
            ds = DGeneric.ExecuteStoreProcedure("SUP.GET_USER_ID", listParameter);
            return ds;
        }

        private DataSet agentidperson(string App, string idpersona)
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

        #endregion

    }
}
