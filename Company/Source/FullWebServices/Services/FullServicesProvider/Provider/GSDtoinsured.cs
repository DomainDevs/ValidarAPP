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
using Sybase.Data.AseClient;
using System.Transactions;
using System.Data.Common;

namespace Sistran.Co.Previsora.Application.FullServicesProvider.Provider
{
    public class GSDtoInsured
    {
        #region constructores

        public GSDtoInsured()
        {

        }

        public GSDtoInsured(string user, int idApp, int idRol)
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

        public DtoInsured GetDtoInsured(int id_rol, string cod_rol, string entity)
        {
            DtoInsured dtoInsured = new DtoInsured();

            //Carga el Dto solo con una entidad especifica
            if (entity != "")
            {
                dtoInsured = GetEntity(id_rol, cod_rol, entity);
            }
            else
            {
                //Carga el Dto Totalmente nuevo
                if (cod_rol == "new")
                    dtoInsured = getDto.DtoInsured();
                //Carga el Dto con toda la informacion del Rol buscado
                else
                {
                    dtoInsured = GetDto(id_rol, cod_rol);
                }
            }
            return dtoInsured;
        }

        private DtoInsured GetDto(int id_rol, string cod_rol)
        {
            DtoInsured dtoInsured = new DtoInsured();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataTable DtRolesAPP = DGeneric.GetListRolApp(IdRol);
            string App = string.Empty;

            foreach (DataRow dr in DtRolesAPP.Rows)
            {
                App = dr[0].ToString();
                switch (App)
                {
                    case "SISE2G":
                        GetDtoSISE2G(id_rol, cod_rol, ref dtoInsured);
                        break;

                    case "SISE3G":
                        GetDtoSISE3G(id_rol, cod_rol, ref dtoInsured);
                        break;

                }
            }

            return dtoInsured;
        }

        private void GetDtoSISE2G(int id_rol, string cod_rol, ref DtoInsured dtoInsured)
        {
            int id_persona = getDto.id_persona(id_rol, cod_rol);
            dtoInsured.dtoDataPerson = getDto.dtoDataPerson(id_persona);
            dtoInsured.dtoDataInsured = getDto.dtoDataInsured(id_persona);
            dtoInsured.List_Mpersona_dir = getDto.List_Mpersona_dir(id_persona);
            dtoInsured.List_Mpersona_telef = getDto.List_Mpersona_telef(id_persona);
            dtoInsured.List_Mpersona_email = getDto.List_Mpersona_email(id_persona);
            dtoInsured.List_Logbook = getDto.List_Logbook(id_persona, id_rol);
            dtoInsured.dtoSarlaft = getDto.dtoSarlaft(id_persona);
            //dtoInsured.dtoSarlaft = getDto.dtoSarlaftForm(Convert.ToInt32(cod_rol), id_persona);
            dtoInsured.List_Frm_sarlaft_accionistas_asoc = getDto.List_Frm_sarlaft_accionistas_asoc(id_persona);
            dtoInsured.List_Mcesionario = getDto.List_Mcesionario(id_persona);
            dtoInsured.List_Maseg_pmin_gastos_emi = getDto.List_Maseg_pmin_gastos_emi(id_persona);
            dtoInsured.List_Maseg_tasa_tarifa = getDto.List_Maseg_tasa_tarifa(id_persona);
            dtoInsured.List_Maseg_asociacion = getDto.List_Maseg_asociacion(id_persona);
            dtoInsured.List_Mpersona_cuentas_bancarias = getDto.List_Mpersona_cuentas_bancarias(id_persona);
            dtoInsured.List_Referencias = getDto.List_Referencias(Convert.ToInt32(cod_rol), id_rol);
            dtoInsured.List_Maseg_conducto = getDto.List_Maseg_conducto(id_persona);
            dtoInsured.dtoRep = getDto.dtoRep(id_persona);
            dtoInsured.dtoTechnicalCard = getDto.dtoTechnicalCard(id_persona);
        }

        private void GetDtoSISE3G(int id_rol, string cod_rol, ref DtoInsured dtoInsured)
        {
            int id_persona = getDto.id_persona(id_rol, cod_rol);
            //Edward Rubiano -- HD3500 -- 02/11/2015
            if (dtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INDIVIDUAL_3G_ID != 0 && dtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INDIVIDUAL_3G_ID != null)
            {
                id_persona = dtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INDIVIDUAL_3G_ID;
            }
            //Edward Rubiano -- 02/11/2015
            //logica de consulta para anexar campos de 3G
            dtoInsured.dtoDataPerson = getDto.dtoDataPerson3g(id_persona, dtoInsured);
            dtoInsured.List_Mpersona_dir = getDto.dtoDataAddress3g(id_persona, dtoInsured.List_Mpersona_dir);
            dtoInsured.List_Mpersona_telef = getDto.dtoDataPhone3g(id_persona, dtoInsured.List_Mpersona_telef);
            dtoInsured.List_Mpersona_email = getDto.dtoDataMail3g(id_persona, dtoInsured.List_Mpersona_email);
            dtoInsured.dtoDataInsured.mpersona = getDto.dtoData3g(id_persona, dtoInsured.dtoDataInsured.mpersona);
            dtoInsured.dtoDataInsured = getDto.dtoInsuredData3g(id_persona, dtoInsured.dtoDataInsured);
            dtoInsured.List_INDIVIDUAL_TAX_EXEMPTION = getDto.List_INDIVIDUAL_TAX_EXEMPTION(id_persona);
            //dtoInsured.dtoTechnicalCard = getDto.dtoTechnicalCard(id_persona);
        }

        private DtoInsured GetEntity(int id_rol, string cod_rol, string entity)
        {
            DtoInsured dtoInsured = new DtoInsured();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataTable DtRolesAPP = DGeneric.GetListRolApp(IdRol);
            string App = string.Empty;

            foreach (DataRow dr in DtRolesAPP.Rows)
            {
                App = dr[0].ToString();
                switch (App)
                {
                    case "SISE2G":
                        GetEntitySISE2G(id_rol, cod_rol, entity, ref dtoInsured);
                        break;

                    case "SISE3G":
                        GetEntitySISE3G(id_rol, cod_rol, entity, ref dtoInsured);
                        break;

                }
            }

            return dtoInsured;
        }

        private void GetEntitySISE2G(int id_rol, string cod_rol, string entity, ref DtoInsured dtoInsured)
        {
            int id_persona = getDto.id_persona(id_rol, cod_rol);
            switch (entity)
            {
                case "dtoDataPerson":
                    dtoInsured.dtoDataPerson = getDto.dtoDataPerson(id_persona);
                    break;
                case "dtoDataInsured":
                    dtoInsured.dtoDataInsured = getDto.dtoDataInsured(id_persona);
                    break;
                case "List_Mpersona_dir":
                    dtoInsured.List_Mpersona_dir = getDto.List_Mpersona_dir(id_persona);
                    break;
                case "List_Mpersona_telef":
                    dtoInsured.List_Mpersona_telef = getDto.List_Mpersona_telef(id_persona);
                    break;
                case "List_Mpersona_email":
                    dtoInsured.List_Mpersona_email = getDto.List_Mpersona_email(id_persona);
                    break;
                case "List_Logbook":
                    dtoInsured.List_Logbook = getDto.List_Logbook(id_persona, id_rol);
                    break;
                case "dtoSarlaft":
                    dtoInsured.dtoSarlaft = getDto.dtoSarlaft(id_persona);
                    break;
                case "dtoSarlaftForm":
                    dtoInsured.dtoSarlaft = getDto.dtoSarlaftForm(Convert.ToInt32(cod_rol));
                    break;
                case "List_Frm_sarlaft_accionistas_asoc":
                    dtoInsured.List_Frm_sarlaft_accionistas_asoc = getDto.List_Frm_sarlaft_accionistas_asoc(id_persona);
                    break;
                case "List_Mcesionario":
                    dtoInsured.List_Mcesionario = getDto.List_Mcesionario(id_persona);
                    break;
                case "List_Maseg_pmin_gastos_emi":
                    dtoInsured.List_Maseg_pmin_gastos_emi = getDto.List_Maseg_pmin_gastos_emi(id_persona);
                    break;
                case "List_Maseg_tasa_tarifa":
                    dtoInsured.List_Maseg_tasa_tarifa = getDto.List_Maseg_tasa_tarifa(id_persona);
                    break;
                case "List_Mpersona_cuentas_bancarias":
                    dtoInsured.List_Mpersona_cuentas_bancarias = getDto.List_Mpersona_cuentas_bancarias(id_persona);
                    break;
                case "List_Maseg_conducto":
                    dtoInsured.List_Maseg_conducto = getDto.List_Maseg_conducto(id_persona);
                    break;
                case "dtoRep":
                    dtoInsured.dtoRep = getDto.dtoRep(id_persona);
                    break;
                case "List_Referencias":
                    dtoInsured.List_Referencias = getDto.List_Referencias(Convert.ToInt32(cod_rol), id_rol);
                    break;
                case "List_Maseg_asociacion":
                    dtoInsured.List_Maseg_asociacion = getDto.List_Maseg_asociacion(id_persona);
                    break;
                case "dtoTechnicalCard":
                    dtoInsured.dtoTechnicalCard = getDto.dtoTechnicalCard(id_persona);
                    break;
            }
        }

        private void GetEntitySISE3G(int id_rol, string cod_rol, string entity, ref DtoInsured dtoInsured)
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
                case "dtoDataInsured":
                    dtoInsured.dtoDataInsured.mpersona = getDto.dtoData3g(id_persona, dtoInsured.dtoDataInsured.mpersona);
                    dtoInsured.dtoDataInsured = getDto.dtoInsuredData3g(id_persona, dtoInsured.dtoDataInsured);
                    break;

                case "dtoDataPerson":
                    dtoInsured.dtoDataPerson = getDto.dtoDataPerson3g(id_persona, dtoInsured);
                    break;

                case "List_Mpersona_dir":
                    dtoInsured.List_Mpersona_dir = getDto.dtoDataAddress3g(id_persona, dtoInsured.List_Mpersona_dir);
                    break;

                case "List_Mpersona_telef":
                    dtoInsured.List_Mpersona_telef = getDto.dtoDataPhone3g(id_persona, dtoInsured.List_Mpersona_telef);
                    break;

                case "List_Mpersona_email":
                    dtoInsured.List_Mpersona_email = getDto.dtoDataMail3g(id_persona, dtoInsured.List_Mpersona_email);
                    break;

                case "DtoTechnicalCard":
                    dtoInsured.dtoTechnicalCard = getDto.dtoTechnicalCard(id_persona);
                    break;

                case "List_INDIVIDUAL_TAX_EXEMPTION":
                    dtoInsured.List_INDIVIDUAL_TAX_EXEMPTION = getDto.List_INDIVIDUAL_TAX_EXEMPTION(id_persona);
                    break;
            }
        }

        public SUPMessages SetDtoInsured(DtoMaster dtoMaster)
        {
            DGeneric = new DataGenericExecute(PrefixConn);
            DataTable DtRolesAPP = DGeneric.GetListRolApp(IdRol);
            string App = string.Empty;
            int cod_rol = Convert.ToInt32(dtoMaster.cod_Rol);
            int id_persona = 0;
            SUPMessages _SUPMessages = new SUPMessages();
            List<TableMessage> ListTableMessage = new List<TableMessage>();

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
                            SetDto setDto = new SetDto(User, App, IdRol);
                            ListTableMessage = setDto.UpdateBasicDataInSise2G(dtoMaster.DtoInsured.dtoDataInsured.mpersona, ListTableMessage); //UPDATE mpersona
                            //Se valida que el unico rol que posee es de SISE 2G, de ser asi se actualizan los datos basicos en la data de SISE 3G
                            if (DtRolesAPP.Rows.Count == 1)
                            {
                                int individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoInsured.dtoDataInsured.mpersona.id_persona);
                                if (individualId3G > 0)
                                {
                                    ListTableMessage = setDto.UpdateBasicDataInSise3G(dtoMaster.DtoInsured.dtoDataInsured.mpersona, individualId3G, ListTableMessage); //UPDATE UP.PERSON || UP.COMPANY, UP.CO_COMPANY

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
                            SetDto setDto = new SetDto(User, App, IdRol);
                            int individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoInsured.dtoDataInsured.mpersona.id_persona);
                            if (individualId3G > 0)
                            {
                                ListTableMessage = setDto.UpdateBasicDataInSise3G(dtoMaster.DtoInsured.dtoDataInsured.mpersona, individualId3G, ListTableMessage);

                            }
                        }
                        //Autor: Edward Rubiano; Fecha: 10/05/2016; Asunto: C11226; Descripcion: Validaciones para guardado unico de datos basicos
                        _SUPMessages.INDIVIDUAL_ID = id_persona.ToString();
                        break;

                }
            }

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
                    if (dtoMaster.DtoInsured.dtoDataInsured.mpersona != null)
                    {
                        if (dtoMaster.DtoInsured.dtoDataInsured.mpersona.id_persona == 0)
                        {
                            DGeneric = new DataGenericExecute(App);
                            id_persona = DGeneric.GetConsecutive("mpersona");
                        }
                        else
                        {
                            id_persona = dtoMaster.DtoInsured.dtoDataInsured.mpersona.id_persona;
                            exist = true;
                        }
                    }

                    DGeneric = new DataGenericExecute(App);
                    cod_rol = DGeneric.GetConsecutive("maseg_header");
                    cod_rolref = cod_rol;
                }
                id_personaref = id_persona;
                //Insertamos o Actualizamos id_persona y cod_rol
                if (dtoMaster.DtoInsured.dtoDataInsured != null)
                {
                    if (dtoMaster.DtoInsured.dtoDataInsured.mpersona != null)
                        dtoMaster.DtoInsured.dtoDataInsured.mpersona.Update("id_persona", id_persona);

                    if (dtoMaster.DtoInsured.dtoDataInsured.mpersona_ciiu != null)
                        dtoMaster.DtoInsured.dtoDataInsured.mpersona_ciiu.Update("id_persona", id_persona);


                    if (dtoMaster.DtoInsured.dtoDataInsured.mpersona_requiere_sarlaft != null)
                        dtoMaster.DtoInsured.dtoDataInsured.mpersona_requiere_sarlaft.Update("id_persona", id_persona);


                    if (dtoMaster.DtoInsured.dtoDataInsured.maseg_header != null)
                        dtoMaster.DtoInsured.dtoDataInsured.maseg_header.Update("id_persona", id_persona);

                    if (dtoMaster.DtoInsured.dtoDataInsured.maseg_header != null)
                    {
                        dtoMaster.DtoInsured.dtoDataInsured.maseg_header.Update("cod_aseg", cod_rol);

                        if (dtoMaster.DtoInsured.dtoDataInsured.maseg_header.cod_aseg_vinc == null)
                            dtoMaster.DtoInsured.dtoDataInsured.maseg_header.Update("cod_aseg_vinc", cod_rol.ToString());
                    }

                    if (dtoMaster.DtoInsured.dtoDataInsured.tipo_persona_aseg != null)
                        dtoMaster.DtoInsured.dtoDataInsured.tipo_persona_aseg.Update("cod_aseg", cod_rol);


                    if (dtoMaster.DtoInsured.dtoDataInsured.tcpto_aseg_adic != null)
                        dtoMaster.DtoInsured.dtoDataInsured.tcpto_aseg_adic.Update("cod_aseg", cod_rol);


                    if (dtoMaster.DtoInsured.dtoDataInsured.maseg_autoriza_consul != null)
                        dtoMaster.DtoInsured.dtoDataInsured.maseg_autoriza_consul.Update("cod_aseg", cod_rol);

                    if (dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G != null)
                    {
                        //Edward Rubiano -- MANTENIMIENTO -- 30/11/2015
                        //Edward Rubiano -- HD3573 -- 22/12/2015
                        if ((dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INDIVIDUAL_2G_ID +
                            dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INDIVIDUAL_3G_ID +
                            dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INSURED_2G_CD +
                            dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INSURED_3G_CD) == 0)
                        {
                            dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.State = 'C';
                        }
                        else
                        { 
                            if (dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INSURED_2G_CD == 0 ||
                                dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INSURED_3G_CD == 0)
                                dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.State = 'U';
                        }
                        //Edward Rubiano -- HD3573 -- 22/12/2015
                        //Edward Rubiano -- MANTENIMIENTO -- 30/11/2015
                        dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.Update("INDIVIDUAL_2G_ID", id_persona);
                        //Edward Rubiano -- 3504 -- 22/10/2015
                        if (dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INDIVIDUAL_3G_ID == 0)
                            dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.Update("INDIVIDUAL_3G_ID", id_persona);
                        //Edward Rubiano -- 3504 -- 22/10/2015                        
                        dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.Update("INSURED_2G_CD", cod_rol);
                        //Edward Rubiano -- HD3501 -- 22/10/2015
                        if (dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INSURED_3G_CD == 0)
                            dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.Update("INSURED_3G_CD", cod_rol);
                        //Edward Rubiano -- HD3501 -- 22/10/2015
                        dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.Update("NAME_NUM", 1);
                    }
                }

                if (dtoMaster.DtoInsured.dtoDataPerson != null)
                {
                    if (dtoMaster.DtoInsured.dtoDataPerson.listMaseg_deporte != null)
                        dtoMaster.DtoInsured.dtoDataPerson.listMaseg_deporte.Update(c => c.cod_aseg = cod_rol);

                    if (dtoMaster.DtoInsured.dtoDataPerson.maseg_ficha_tec_financ != null)
                        dtoMaster.DtoInsured.dtoDataPerson.maseg_ficha_tec_financ.Update("cod_aseg", cod_rol);


                    if (dtoMaster.DtoInsured.dtoDataPerson.tipo_persona_aseg != null)
                        dtoMaster.DtoInsured.dtoDataPerson.tipo_persona_aseg.Update("cod_aseg", cod_rol);
                }

                if (dtoMaster.DtoInsured.List_Mpersona_dir != null)
                    dtoMaster.DtoInsured.List_Mpersona_dir.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoInsured.List_Mpersona_telef != null)
                    dtoMaster.DtoInsured.List_Mpersona_telef.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoInsured.List_Mpersona_email != null)
                    dtoMaster.DtoInsured.List_Mpersona_email.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoInsured.List_Logbook != null)
                    dtoMaster.DtoInsured.List_Logbook.Update(c => { c.ID_PERSONA = id_persona; });

                if (dtoMaster.DtoInsured.List_Frm_sarlaft_accionistas_asoc != null)
                    dtoMaster.DtoInsured.List_Frm_sarlaft_accionistas_asoc.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoInsured.List_Maseg_pmin_gastos_emi != null)
                    dtoMaster.DtoInsured.List_Maseg_pmin_gastos_emi.Update(c => { c.cod_aseg = cod_rol; });

                if (dtoMaster.DtoInsured.List_Maseg_tasa_tarifa != null)
                    dtoMaster.DtoInsured.List_Maseg_tasa_tarifa.Update(c => { c.cod_aseg = cod_rol; });

                if (dtoMaster.DtoInsured.List_Maseg_asociacion != null)
                    dtoMaster.DtoInsured.List_Maseg_asociacion.Update(c => { c.cod_aseg_asociacion = cod_rol; });

                if (dtoMaster.DtoInsured.List_Mpersona_cuentas_bancarias != null)
                    dtoMaster.DtoInsured.List_Mpersona_cuentas_bancarias.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoInsured.List_Maseg_conducto != null)
                    dtoMaster.DtoInsured.List_Maseg_conducto.Update(c => { c.cod_aseg = cod_rol; });

                if (dtoMaster.DtoInsured.dtoRep != null)
                {
                    if (dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal != null)
                        dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.Update("id_persona", id_persona);
                    if (dtoMaster.DtoInsured.dtoRep.List_mpersona_rep_legal_dir != null)
                        dtoMaster.DtoInsured.dtoRep.List_mpersona_rep_legal_dir.Update(c => { c.id_persona = id_persona; });
                }

                if (dtoMaster.DtoInsured.dtoSarlaft != null)
                {
                    if (dtoMaster.DtoInsured.dtoSarlaft.List_detalle != null)
                        dtoMaster.DtoInsured.dtoSarlaft.List_detalle.Update(c => { c.id_persona = id_persona; });

                    if (dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_vinculos != null)
                        dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_vinculos.Update("id_persona", id_persona);

                    if (dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_aut_incrementos != null)
                        dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_aut_incrementos.Update("id_persona", id_persona);

                }

                if (dtoMaster.DtoInsured.List_Referencias != null)
                    dtoMaster.DtoInsured.List_Referencias.Update(c => { c.cod_rol = cod_rol; });

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
                        #region Insert-Update
                        SetDto setDto = new SetDto(User, App, IdRol, Command);
                        if (dtoMaster.DtoInsured.dtoDataInsured != null)
                        {
                            List<AseCommand> ListBaseCommand = new List<AseCommand>();
                            ListTableMessage = setDto.dtoDataInsured(dtoMaster.DtoInsured.dtoDataInsured, ListTableMessage);

                            if (ListTableMessage.Count(c => c.NameTable == "Maseg_header" && c.Message != "True") > 0)
                            {
                                if (!dtoMaster.DtoInsured.dtoDataInsured.maseg_header.State.Equals('V') && ListTableMessage.Count(c => c.NameTable == "Maseg_header" && c.Message != "True") > 0)
                                {
                                    cod_rolref = -1;
                                }
                                if (dtoMaster.DtoInsured.dtoDataInsured.mpersona.State.Equals('V'))
                                {
                                    cod_rolref = -1;
                                    if (!dtoMaster.DtoInsured.dtoDataInsured.mpersona.State.Equals('V'))
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

                        if (cod_rolref != -1)
                        {
                            if (dtoMaster.DtoInsured.dtoDataPerson != null)
                                ListTableMessage = setDto.dtoDataPerson(dtoMaster.DtoInsured.dtoDataPerson, ListTableMessage);

                            if (dtoMaster.DtoInsured.List_Mpersona_dir != null)
                                ListTableMessage = setDto.List_Mpersona_dir(dtoMaster.DtoInsured.List_Mpersona_dir, ListTableMessage);

                            if (dtoMaster.DtoInsured.List_Mpersona_telef != null)
                                ListTableMessage = setDto.List_Mpersona_telef(dtoMaster.DtoInsured.List_Mpersona_telef, ListTableMessage);

                            if (dtoMaster.DtoInsured.List_Mpersona_email != null)
                                ListTableMessage = setDto.List_Mpersona_email(dtoMaster.DtoInsured.List_Mpersona_email, ListTableMessage);

                            if (dtoMaster.DtoInsured.List_Logbook != null)
                                ListTableMessage = setDto.List_Logbook(dtoMaster.DtoInsured.List_Logbook, ListTableMessage);

                            if (dtoMaster.DtoInsured.List_Frm_sarlaft_accionistas_asoc != null)
                                ListTableMessage = setDto.List_Frm_sarlaft_accionistas_asoc(dtoMaster.DtoInsured.List_Frm_sarlaft_accionistas_asoc, ListTableMessage);

                            if (dtoMaster.DtoInsured.List_Maseg_pmin_gastos_emi != null)
                                ListTableMessage = setDto.List_Maseg_pmin_gastos_emi(dtoMaster.DtoInsured.List_Maseg_pmin_gastos_emi, ListTableMessage);

                            if (dtoMaster.DtoInsured.List_Maseg_tasa_tarifa != null)
                                ListTableMessage = setDto.List_Maseg_tasa_tarifa(dtoMaster.DtoInsured.List_Maseg_tasa_tarifa, ListTableMessage);

                            if (dtoMaster.DtoInsured.List_Maseg_asociacion != null)
                                ListTableMessage = setDto.List_Maseg_asociacion(dtoMaster.DtoInsured.List_Maseg_asociacion, ListTableMessage);

                            if (dtoMaster.DtoInsured.List_Mpersona_cuentas_bancarias != null)
                                ListTableMessage = setDto.List_Mpersona_cuentas_bancarias(dtoMaster.DtoInsured.List_Mpersona_cuentas_bancarias, ListTableMessage);

                            if (dtoMaster.DtoInsured.List_Maseg_conducto != null)
                                ListTableMessage = setDto.List_Maseg_conducto(dtoMaster.DtoInsured.List_Maseg_conducto, ListTableMessage);

                            if (dtoMaster.DtoInsured.dtoSarlaft != null)
                                ListTableMessage = setDto.dtoSarlaft(dtoMaster.DtoInsured.dtoSarlaft, ListTableMessage);

                            if (dtoMaster.DtoInsured.List_Referencias != null)
                                ListTableMessage = setDto.List_Referencias(dtoMaster.DtoInsured.List_Referencias, ListTableMessage);

                            if (dtoMaster.DtoInsured.dtoRep != null)
                                ListTableMessage = setDto.dtoRep(dtoMaster.DtoInsured.dtoRep, ListTableMessage);

                            if (
                                    (
                                        (dtoMaster.DtoInsured.List_Mpersona_dir != null && dtoMaster.DtoInsured.List_Mpersona_dir.Count > 0) ||
                                        (dtoMaster.DtoInsured.List_Mpersona_telef != null && dtoMaster.DtoInsured.List_Mpersona_telef.Count > 0) ||
                                        (dtoMaster.DtoInsured.List_Mpersona_email != null && dtoMaster.DtoInsured.List_Mpersona_email.Count > 0)
                                    )
                                )
                            {
                                ListTableMessage = setDto.set_Juridical_Person_Register(id_persona, ListTableMessage);
                            }

                            if (dtoMaster.DtoInsured.dtoTechnicalCard != null)
                            {
                                if (dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD != null)
                                {
                                    Maseg_ficha_tec maseg_ficha_tec = new Maseg_ficha_tec();

                                    maseg_ficha_tec.cod_aseg = cod_rol;
                                    maseg_ficha_tec.State = dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.State;

                                    maseg_ficha_tec.cod_experiencia = dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.EXPERIENCE_TYPE_CD;
                                    maseg_ficha_tec.imp_k_autorizado = dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.AUTHORIZED_CAPITAL_AMT;
                                    maseg_ficha_tec.nro_matricula = dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.ENROLLMENT_NUM;
                                    maseg_ficha_tec.fec_desde_matric = dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.ENROLLMENT_FROM;
                                    maseg_ficha_tec.fec_hasta_matric = dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.ENROLLMENT_TO;
                                    maseg_ficha_tec.txt_ubicacion_ficha = dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.TECHNICAL_CARD_LOCATION;
                                    maseg_ficha_tec.txt_revisor_fiscal = dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.TAX_INSPECTOR;
                                    maseg_ficha_tec.txt_objeto_soc = dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.CORPORATE_PURPOSE;
                                    maseg_ficha_tec.txt_referencias = dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.REFERENCES;
                                    maseg_ficha_tec.txt_cpto_financiero = dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.FINANCIAL_CONCEPT;
                                    maseg_ficha_tec.txt_obs_cumulo = dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.PILE_DESCRIPTION;
                                    maseg_ficha_tec.txt_experiencia_en = dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.EXPERIENCE;

                                    if (maseg_ficha_tec.State == 'C')
                                    {
                                        maseg_ficha_tec.fec_creacion = DateTime.Now.ToString("dd/MM/yyyy");
                                        maseg_ficha_tec.cod_usuario_crea = User;
                                    }
                                    else
                                    {
                                        maseg_ficha_tec.fec_creacion = dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.REGISTRATION_DATE;
                                        maseg_ficha_tec.cod_usuario_crea = dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.REGISTERED_USER_ID;
                                    }

                                    ListTableMessage = setDto.Maseg_ficha_tec(maseg_ficha_tec, ListTableMessage);
                                }

                                if (dtoMaster.DtoInsured.dtoTechnicalCard.List_BOARD_DIRECTORS != null)
                                {
                                    List<Maseg_ficha_tec_junta> List_Maseg_ficha_tec_junta = new List<Maseg_ficha_tec_junta>();

                                    foreach (BOARD_DIRECTORS BOARD_DIRECTOR in dtoMaster.DtoInsured.dtoTechnicalCard.List_BOARD_DIRECTORS)
                                    {
                                        Maseg_ficha_tec_junta maseg_ficha_tec_junta = new Maseg_ficha_tec_junta();

                                        maseg_ficha_tec_junta.cod_aseg = cod_rol;
                                        maseg_ficha_tec_junta.State = BOARD_DIRECTOR.State;
                                        maseg_ficha_tec_junta.consec_miembro = BOARD_DIRECTOR.BOARD_DIRECTORS_CD;
                                        maseg_ficha_tec_junta.txt_miembro = BOARD_DIRECTOR.BOARD_MEMBER_NAME;
                                        maseg_ficha_tec_junta.txt_cargo = BOARD_DIRECTOR.BOARD_MEMBER_JOB_TITLE;
                                        maseg_ficha_tec_junta.fec_modif = DateTime.Now.ToString("dd/MM/yyyy");
                                        maseg_ficha_tec_junta.cod_usuario_modif = User;

                                        List_Maseg_ficha_tec_junta.Add(maseg_ficha_tec_junta);
                                    }

                                    ListTableMessage = setDto.Maseg_ficha_tec_junta(List_Maseg_ficha_tec_junta, ListTableMessage);
                                }

                                if (dtoMaster.DtoInsured.dtoTechnicalCard.List_FINANCIAL_STATEMENTS != null)
                                {
                                    List<Maseg_ficha_tec_financ> List_Maseg_ficha_tec_financ = new List<Maseg_ficha_tec_financ>();

                                    foreach (FINANCIAL_STATEMENTS FINANCIAL_STATEMENTS in dtoMaster.DtoInsured.dtoTechnicalCard.List_FINANCIAL_STATEMENTS)
                                    {
                                        Maseg_ficha_tec_financ maseg_ficha_tec_financ = new Maseg_ficha_tec_financ();

                                        maseg_ficha_tec_financ.cod_aseg = cod_rol;
                                        maseg_ficha_tec_financ.State = FINANCIAL_STATEMENTS.State;

                                        maseg_ficha_tec_financ.fec_informacion = FINANCIAL_STATEMENTS.BALANCE_DATE;
                                        maseg_ficha_tec_financ.imp_inventarios = FINANCIAL_STATEMENTS.INVENTORY_AMT;
                                        maseg_ficha_tec_financ.imp_cuentas_cobrar = FINANCIAL_STATEMENTS.ACCOUNTS_RECEIVABLE_AMT;
                                        maseg_ficha_tec_financ.imp_invers_temp = FINANCIAL_STATEMENTS.CASH_INVESTMENT_TEMPORARY_AMT;
                                        maseg_ficha_tec_financ.imp_activo_cte = FINANCIAL_STATEMENTS.CURRENT_ASSETS_AMT;
                                        maseg_ficha_tec_financ.imp_equipos = 0;
                                        maseg_ficha_tec_financ.imp_activ_fijos_br = FINANCIAL_STATEMENTS.FIXED_GROSS_ASSETS_AMT;
                                        maseg_ficha_tec_financ.imp_activo_fijo = FINANCIAL_STATEMENTS.FIXED_NET_ASSETS_AMT;
                                        maseg_ficha_tec_financ.imp_valorizaciones = FINANCIAL_STATEMENTS.VALUATION_AMT;
                                        maseg_ficha_tec_financ.imp_activo_total = FINANCIAL_STATEMENTS.ASSETS_AMT;
                                        maseg_ficha_tec_financ.imp_pasivo_cte = FINANCIAL_STATEMENTS.CURRENT_LIABILITIES_AMT;
                                        maseg_ficha_tec_financ.imp_pasivo_lplazo = FINANCIAL_STATEMENTS.LONG_TERM_LIABILITIES_AMT;
                                        maseg_ficha_tec_financ.imp_pasivo_total = FINANCIAL_STATEMENTS.LIABILITIES_AMT;
                                        maseg_ficha_tec_financ.imp_cap_social = FINANCIAL_STATEMENTS.CAPITAL_AMT;
                                        maseg_ficha_tec_financ.imp_reval_patrim = FINANCIAL_STATEMENTS.REVALUATION_AMT;
                                        maseg_ficha_tec_financ.imp_superavit_valoriz = FINANCIAL_STATEMENTS.SURPLUS_VALUE_AMT;
                                        maseg_ficha_tec_financ.imp_otros_superavit = FINANCIAL_STATEMENTS.OTHERS_SURPLUS_AMT;
                                        maseg_ficha_tec_financ.imp_reservas = FINANCIAL_STATEMENTS.RESERVES_AMT;
                                        maseg_ficha_tec_financ.imp_primas = FINANCIAL_STATEMENTS.PREMIUM_AMT;
                                        maseg_ficha_tec_financ.imp_util_acum = FINANCIAL_STATEMENTS.ACCUMULATED_PROFIT_AMT;
                                        maseg_ficha_tec_financ.imp_patrimonio = FINANCIAL_STATEMENTS.PATRIMONY_AMT;
                                        maseg_ficha_tec_financ.imp_ventas = FINANCIAL_STATEMENTS.NET_SALES_AMT;
                                        maseg_ficha_tec_financ.imp_costo_vtas = FINANCIAL_STATEMENTS.SALES_COST_AMT;
                                        maseg_ficha_tec_financ.imp_util_bruta = FINANCIAL_STATEMENTS.GROSS_PROFIT_AMT;
                                        maseg_ficha_tec_financ.imp_util_oper = FINANCIAL_STATEMENTS.OPERATING_PROFIT_AMT;
                                        maseg_ficha_tec_financ.imp_util_neta = FINANCIAL_STATEMENTS.NET_PROFIT_AMT;
                                        maseg_ficha_tec_financ.imp_otros_ingr_nopera = FINANCIAL_STATEMENTS.OTHERS_INCOME_AMT;
                                        maseg_ficha_tec_financ.imp_ajustes_infl = FINANCIAL_STATEMENTS.INFLATION_ADJUSTMENTS_AMT;
                                        maseg_ficha_tec_financ.imp_intereses = FINANCIAL_STATEMENTS.INTERESTS_AMT;
                                        maseg_ficha_tec_financ.imp_otros_gastos_nopera = FINANCIAL_STATEMENTS.OTHERS_EXPENSE_AMT;

                                        if (maseg_ficha_tec_financ.State == 'C')
                                            maseg_ficha_tec_financ.fec_creacion = DateTime.Now.ToString("dd/MM/yyyy");
                                        else
                                            maseg_ficha_tec_financ.fec_creacion = FINANCIAL_STATEMENTS.REGISTRATION_BALANCE_DATE;

                                        maseg_ficha_tec_financ.fec_modif = DateTime.Now.ToString("dd/MM/yyyy");
                                        maseg_ficha_tec_financ.cod_usuario_crea = FINANCIAL_STATEMENTS.BALANCE_USER_ID.ToString();
                                        maseg_ficha_tec_financ.cod_usuario_modif = User;
                                        maseg_ficha_tec_financ.imp_ctas_pagar = 0;
                                        maseg_ficha_tec_financ.imp_edificios = 0;
                                        maseg_ficha_tec_financ.imp_obl_bancos = 0;
                                        maseg_ficha_tec_financ.imp_obl_lplazo = 0;
                                        maseg_ficha_tec_financ.imp_terrenos = 0;

                                        List_Maseg_ficha_tec_financ.Add(maseg_ficha_tec_financ);
                                    }

                                    ListTableMessage = setDto.Maseg_ficha_tec_financ(List_Maseg_ficha_tec_financ, ListTableMessage);
                                }

                                if (dtoMaster.DtoInsured.dtoTechnicalCard.List_TECHNICAL_CARD_DESCRIPTION != null)
                                {
                                    List<Maseg_ficha_tec_obs> List_Maseg_ficha_tec_obs = new List<Maseg_ficha_tec_obs>();

                                    foreach (TECHNICAL_CARD_DESCRIPTION TECHNICAL_CARD_DESCRIPTION in dtoMaster.DtoInsured.dtoTechnicalCard.List_TECHNICAL_CARD_DESCRIPTION)
                                    {
                                        Maseg_ficha_tec_obs maseg_ficha_tec_obs = new Maseg_ficha_tec_obs();

                                        maseg_ficha_tec_obs.cod_aseg = cod_rol;
                                        maseg_ficha_tec_obs.State = TECHNICAL_CARD_DESCRIPTION.State;

                                        maseg_ficha_tec_obs.consec_obs = TECHNICAL_CARD_DESCRIPTION.TECHNICAL_CARD_DESCRIPTION_CD;
                                        maseg_ficha_tec_obs.fec_creacion = TECHNICAL_CARD_DESCRIPTION.DESCRIPTION_DATE;
                                        maseg_ficha_tec_obs.fec_modif = DateTime.Now.ToString("dd/MM/yyyy");
                                        maseg_ficha_tec_obs.txt_obs = TECHNICAL_CARD_DESCRIPTION.DESCRIPTION;
                                        maseg_ficha_tec_obs.cod_usuario_crea = User;
                                        maseg_ficha_tec_obs.cod_usuario_modif = User;

                                        List_Maseg_ficha_tec_obs.Add(maseg_ficha_tec_obs);
                                    }

                                    ListTableMessage = setDto.Maseg_ficha_tec_obs(List_Maseg_ficha_tec_obs, ListTableMessage);
                                }
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
                    DataSet ds = new DataSet();
                    int cod_rol = cod_rolref;
                    int id_persona = id_personaref;
                    string IsSave = string.Empty;

                    //Individual ID 3G Equivalent
                    int id3GEquivalent = getDto.getIndividualID3g(id_persona);
                    if (id3GEquivalent != 0)
                        id_persona = id3GEquivalent;

                    string cnnString = ConfigurationManager.ConnectionStrings[PrefixConn2].ToString();

                    char state;
                    state = statefor3g(App, dtoMaster.DtoInsured.dtoDataInsured.mpersona.cod_tipo_persona == "F" ? "1" : "2",
                        dtoMaster.DtoInsured.dtoDataInsured.mpersona.cod_tipo_doc, dtoMaster.DtoInsured.dtoDataInsured.mpersona.nro_doc != null ? dtoMaster.DtoInsured.dtoDataInsured.mpersona.nro_doc :
                        dtoMaster.DtoInsured.dtoDataInsured.mpersona.nro_nit.Substring(0, dtoMaster.DtoInsured.dtoDataInsured.mpersona.nro_nit.Length - 1),id_persona);

                    DataSet States3g = new DataSet();
                    States3g = stateinsured(App, Convert.ToString(id_persona));

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
                            DGeneric = new DataGenericExecute(App);

                            char state_insured = 'C';
                            char state_coinsured = 'C';
                            char state_agent = 'C';
                            char state_individual = 'C';
                            char state_requiere = 'C';
                            char state_contragarantias = 'C';
                            //Insertamos o Actualizamos id_persona y cod_rol
                            #region PERSONA-COMPAÑIA

                            if (dtoMaster.DtoInsured.dtoDataInsured != null)
                            {
                                if (dtoMaster.DtoInsured.dtoDataInsured.mpersona != null)
                                {
                                    if (!dtoMaster.DtoInsured.dtoDataInsured.mpersona.State.Equals('V'))
                                    {
                                        INDIVIDUAL _INDIVIDUAL = new INDIVIDUAL();
                                        _INDIVIDUAL.State = state;
                                        _INDIVIDUAL.INDIVIDUAL_ID = id_persona;
                                        _INDIVIDUAL.INDIVIDUAL_TYPE_CD = (dtoMaster.DtoInsured.dtoDataInsured.mpersona.cod_tipo_persona == "F" ? 1 : 2);
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
                                            parameter.Value = id_persona.ToString();
                                            listParameter.Add(parameter);

                                            DGeneric = new DataGenericExecute(PrefixConn);
                                            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_DATAID3G", listParameter, Command);
                                            INDIVIDUAL INDIV = Dtl.ConvertTo<INDIVIDUAL>(DsReturn.Tables[0])[0];
                                            _INDIVIDUAL.AT_DATA_ID = INDIV.AT_DATA_ID;

                                        }

                                        _INDIVIDUAL.AT_PAYMENT_ID = 1;
                                        _INDIVIDUAL.AT_AGENT_AGENCY_ID = 0;
                                        //_INDIVIDUAL.OWNER_ROLE_CD = null;
                                        /*if (dtoMaster.DtoInsured.dtoDataInsured.mpersona_ciiu != null)
                                           {
                                               _INDIVIDUAL.ECONOMIC_ACTIVITY_CD = Convert.ToString(dtoMaster.DtoInsured.dtoDataInsured.mpersona_ciiu.cod_ciiu_ppal_nuevo);
                                           }*/
                                        _INDIVIDUAL.ECONOMIC_ACTIVITY_CD = null;
                                        List<INDIVIDUAL> List_INDIVIDUAL = new List<INDIVIDUAL>();
                                        List_INDIVIDUAL.Add(_INDIVIDUAL);
                                        ListTableMessage = setDto.List_INDIVIDUAL(List_INDIVIDUAL, ListTableMessage);

                                        TableMessage result = ListTableMessage.Find(delegate(TableMessage bk) { return bk.NameTable == "INDIVIDUAL"; });
                                        IsSave = result.Message;

                                        if (IsSave == "True")
                                        {
                                            if (state == 'C'
                                                //Edward Rubiano -- HD3554 -- 10/12/2015
                                                || state == 'U'
                                                || state == 'D'
                                                //Edward Rubiano -- HD3554 -- 10/12/2015
                                                )
                                            {
                                                INDIVIDUAL_PAYMENT_METHOD _INDIVIDUAL_PAYMENT_METHOD = new INDIVIDUAL_PAYMENT_METHOD();
                                                //Edward Rubiano -- HD3554 -- 10/12/2015
                                                if (state != 'D')
                                                {
                                                    if (getDto.ValidateExistInTable(id_persona, 7) > 0)
                                                    {
                                                        _INDIVIDUAL_PAYMENT_METHOD.State = 'U';
                                                    }
                                                    else
                                                    {
                                                        _INDIVIDUAL_PAYMENT_METHOD.State = 'C';
                                                    }
                                                }
                                                else
                                                {
                                                    _INDIVIDUAL_PAYMENT_METHOD.State = state; 
                                                }
                                                //Edward Rubiano -- HD3554 -- 10/12/2015
                                                _INDIVIDUAL_PAYMENT_METHOD.INDIVIDUAL_ID = id_persona;
                                                _INDIVIDUAL_PAYMENT_METHOD.PAYMENT_ID = 1;
                                                _INDIVIDUAL_PAYMENT_METHOD.ROLE_CD = 1;
                                                _INDIVIDUAL_PAYMENT_METHOD.PAYMENT_METHOD_CD = 1;
                                                _INDIVIDUAL_PAYMENT_METHOD.ENABLED = true;

                                                List<INDIVIDUAL_PAYMENT_METHOD> List_INDIVIDUAL_PAYMENT_METHOD = new List<INDIVIDUAL_PAYMENT_METHOD>();
                                                List_INDIVIDUAL_PAYMENT_METHOD.Add(_INDIVIDUAL_PAYMENT_METHOD);
                                                ListTableMessage = setDto.List_INDIVIDUAL_PAYMENT_METHOD(List_INDIVIDUAL_PAYMENT_METHOD, ListTableMessage);
                                            }

                                            if (dtoMaster.DtoInsured.dtoDataInsured.mpersona_ciiu != null)
                                            {
                                                CO_PRV_INDIVIDUAL _CO_PRV_INDIVIDUAL = new CO_PRV_INDIVIDUAL();
                                                _CO_PRV_INDIVIDUAL.State = state;
                                                _CO_PRV_INDIVIDUAL.INDIVIDUAL_ID = id_persona;
                                                _CO_PRV_INDIVIDUAL.ECONOMIC_ACTIVITY_CD_NEW = dtoMaster.DtoInsured.dtoDataInsured.mpersona_ciiu.cod_ciiu_ppal_nuevo;
                                                _CO_PRV_INDIVIDUAL.SECOND_ECONOMIC_ACTIVITY_CD_NEW = dtoMaster.DtoInsured.dtoDataInsured.mpersona_ciiu.cod_ciiu_scndria_nuevo;

                                                List<CO_PRV_INDIVIDUAL> List_CO_PRV_INDIVIDUAL = new List<CO_PRV_INDIVIDUAL>();
                                                List_CO_PRV_INDIVIDUAL.Add(_CO_PRV_INDIVIDUAL);
                                                ListTableMessage = setDto.List_CO_PRV_INDIVIDUAL(List_CO_PRV_INDIVIDUAL, ListTableMessage);
                                            }

                                            if (dtoMaster.DtoInsured.dtoDataInsured.mpersona.cod_tipo_persona == "F")
                                            {
                                                _INDIVIDUAL.INDIVIDUAL_TYPE_CD = 1;
                                                PERSON _PERSON = new PERSON();
                                                _PERSON.State = state;
                                                _PERSON.INDIVIDUAL_ID = id_persona;
                                                _PERSON.SURNAME = dtoMaster.DtoInsured.dtoDataInsured.mpersona.txt_apellido1;
                                                _PERSON.NAME = dtoMaster.DtoInsured.dtoDataInsured.mpersona.txt_nombre;
                                                _PERSON.GENDER = dtoMaster.DtoInsured.dtoDataInsured.mpersona.txt_sexo;
                                                _PERSON.ID_CARD_TYPE_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoInsured.dtoDataInsured.mpersona.cod_tipo_doc));
                                                _PERSON.ID_CARD_NO = dtoMaster.DtoInsured.dtoDataInsured.mpersona.nro_doc;
                                                _PERSON.MARITAL_STATUS_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoInsured.dtoDataInsured.mpersona.cod_est_civil));
                                                _PERSON.BIRTH_DATE = dtoMaster.DtoInsured.dtoDataInsured.mpersona.fec_nac == null ? Convert.ToString(DateTime.Now) : dtoMaster.DtoInsured.dtoDataInsured.mpersona.fec_nac;
                                                _PERSON.MOTHER_LAST_NAME = dtoMaster.DtoInsured.dtoDataInsured.mpersona.txt_apellido2;
                                                //_PERSON.TRIBUTARY_ID_TYPE_CD = dtoMaster.DtoInsured.dtoDataInsured.mpersona.txt_apellido1;
                                                //_PERSON.TRIBUTARY_ID_NO = dtoMaster.DtoInsured.dtoDataInsured.mpersona.txt_apellido1;
                                                _PERSON.BIRTH_COUNTRY_CD = null;
                                                _PERSON.BIRTH_PLACE = dtoMaster.DtoInsured.dtoDataInsured.mpersona.txt_lugar_nac;

                                                _PERSON.SPOUSE_NAME = dtoMaster.DtoInsured.dtoDataInsured.mpersona.SPOUSE_NAME;
                                                _PERSON.EDUCATIVE_LEVEL_CD = dtoMaster.DtoInsured.dtoDataInsured.mpersona.EDUCATIVE_LEVEL_CD;
                                                _PERSON.CHILDREN = string.IsNullOrEmpty(dtoMaster.DtoInsured.dtoDataInsured.mpersona.CHILDREN) ? "0" : dtoMaster.DtoInsured.dtoDataInsured.mpersona.CHILDREN;
                                                _PERSON.SOCIAL_LAYER_CD = dtoMaster.DtoInsured.dtoDataInsured.mpersona.SOCIAL_LAYER_CD;
                                                _PERSON.HOUSE_TYPE_CD = dtoMaster.DtoInsured.dtoDataInsured.mpersona.HOUSE_TYPE_CD;


                                                List<PERSON> List_PERSON = new List<PERSON>();
                                                List_PERSON.Add(_PERSON);
                                                ListTableMessage = setDto.List_PERSON(List_PERSON, ListTableMessage);

                                                result = ListTableMessage.Find(delegate(TableMessage bk) { return bk.NameTable == "PERSON"; });
                                                IsSave = result.Message;

                                                if (IsSave == "True")
                                                {
                                                    PERSON_JOB _PERSON_JOB = new PERSON_JOB();

                                                    _PERSON_JOB.State = state;

                                                    _PERSON_JOB.INDIVIDUAL_ID = id_persona;
                                                    _PERSON_JOB.SPECIALITY_CD = dtoMaster.DtoInsured.dtoDataInsured.mpersona.SPECIALITY_CD;

                                                    _PERSON_JOB.COMPANY_NAME = dtoMaster.DtoInsured.dtoDataInsured.mpersona.txt_cia_tra;
                                                    _PERSON_JOB.JOB_SECTOR = dtoMaster.DtoInsured.dtoDataInsured.mpersona.txt_dpto_tra;
                                                    _PERSON_JOB.POSITION = dtoMaster.DtoInsured.dtoDataInsured.mpersona.txt_puesto_tra;
                                                    _PERSON_JOB.CONTACT = dtoMaster.DtoInsured.dtoDataInsured.mpersona.txt_asistente_tra;

                                                    _PERSON_JOB.INCOME_LEVEL_CD = dtoMaster.DtoInsured.dtoDataInsured.mpersona.INCOME_LEVEL_CD;
                                                    _PERSON_JOB.COMPANY_PHONE = dtoMaster.DtoInsured.dtoDataInsured.mpersona.COMPANY_PHONE;

                                                    if (dtoMaster.DtoInsured.dtoDataInsured.maseg_header != null)
                                                    {
                                                        _PERSON_JOB.OCCUPATION_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoInsured.dtoDataInsured.maseg_header.cod_ocupacion)) > 0 ? Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoInsured.dtoDataInsured.maseg_header.cod_ocupacion)) : 99;
                                                        _PERSON_JOB.OTHER_OCCUPATION_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoInsured.dtoDataInsured.maseg_header.cod_seg_ocupacion)) > 0 ? dtoMaster.DtoInsured.dtoDataInsured.maseg_header.cod_seg_ocupacion : null;
                                                    }

                                                    List<PERSON_JOB> List_PERSON_JOB = new List<PERSON_JOB>();
                                                    List_PERSON_JOB.Add(_PERSON_JOB);
                                                    ListTableMessage = setDto.List_PERSON_JOB(List_PERSON_JOB, ListTableMessage);
                                                }

                                            }
                                            else if (dtoMaster.DtoInsured.dtoDataInsured.mpersona.cod_tipo_persona == "J")
                                            {
                                                _INDIVIDUAL.INDIVIDUAL_TYPE_CD = 2;

                                                COMPANY _COMPANY = new COMPANY();
                                                _COMPANY.State = state;
                                                _COMPANY.INDIVIDUAL_ID = id_persona;
                                                _COMPANY.TRADE_NAME = dtoMaster.DtoInsured.dtoDataInsured.mpersona.txt_apellido1 + ' ' + dtoMaster.DtoInsured.dtoDataInsured.mpersona.txt_apellido2 + ' ' + dtoMaster.DtoInsured.dtoDataInsured.mpersona.txt_nombre;
                                                _COMPANY.TRIBUTARY_ID_TYPE_CD = 2; //NIT es el unico tipo tributario.
                                                _COMPANY.TRIBUTARY_ID_NO = dtoMaster.DtoInsured.dtoDataInsured.mpersona.nro_nit.Substring(0, dtoMaster.DtoInsured.dtoDataInsured.mpersona.nro_nit.Length - 1);
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
                                                    _CO_COMPANY.State = state;
                                                    _CO_COMPANY.INDIVIDUAL_ID = id_persona;
                                                    _CO_COMPANY.VERIFY_DIGIT = dtoMaster.DtoInsured.dtoDataInsured.mpersona.VERIFY_DIGIT;
                                                    _CO_COMPANY.ASSOCIATION_TYPE_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoInsured.dtoDataInsured.mpersona.ASSOCIATION_TYPE_CD));

                                                    if (dtoMaster.DtoInsured.dtoDataInsured.maseg_header != null)
                                                        _CO_COMPANY.CATEGORY_CD = (dtoMaster.DtoInsured.dtoDataInsured.maseg_header.cod_imp_aseg == 2 ? 1 : 2).ToString();

                                                    List<CO_COMPANY> List_CO_COMPANY = new List<CO_COMPANY>();
                                                    List_CO_COMPANY.Add(_CO_COMPANY);
                                                    ListTableMessage = setDto.List_CO_COMPANY(List_CO_COMPANY, ListTableMessage);
                                                }
                                            }
                                        }
                                    }
                                }
                            #endregion

                                #region ASEGURADO
                                if (IsSave == "True")
                                {
                                    if (dtoMaster.DtoInsured.dtoDataInsured.maseg_header != null)
                                    {
                                        //Edward Rubiano -- C11224 -- 09/02/2016
                                        if (States3g.Tables[0].Rows.Count > 0)
                                        {
                                            state_insured = Convert.ToChar(States3g.Tables[0].Rows[0][0]);
                                            state_coinsured = Convert.ToChar(States3g.Tables[0].Rows[0][1]);
                                            state_agent = Convert.ToChar(States3g.Tables[0].Rows[0][2]);
                                            state_requiere = Convert.ToChar(States3g.Tables[0].Rows[0][3]);
                                            state_contragarantias = Convert.ToChar(States3g.Tables[0].Rows[0][4]);
                                        }
                                        //Edward Rubiano -- C11224 -- 09/02/2016

                                        if (!dtoMaster.DtoInsured.dtoDataInsured.maseg_header.State.Equals('V'))
                                        {

                                            //Edward Rubiano -- C11224 -- 09/02/2016
                                            //if (States3g.Tables[0].Rows.Count > 0)
                                            //{
                                            //    state_insured = Convert.ToChar(States3g.Tables[0].Rows[0][0]);
                                            //    state_coinsured = Convert.ToChar(States3g.Tables[0].Rows[0][1]);
                                            //    state_agent = Convert.ToChar(States3g.Tables[0].Rows[0][2]);
                                            //    state_requiere = Convert.ToChar(States3g.Tables[0].Rows[0][3]);
                                            //    state_contragarantias = Convert.ToChar(States3g.Tables[0].Rows[0][4]);
                                            //}
                                            //Edward Rubiano -- C11224 -- 09/02/2016

                                            INSURED _INSURED = new INSURED();
                                            _INSURED.State = state_insured;
                                            //Edward Rubiano -- HD3501 -- 22/10/2015
                                            //_INSURED.INDIVIDUAL_ID = id_persona;
                                            _INSURED.INDIVIDUAL_ID = dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INDIVIDUAL_3G_ID;
                                            //_INSURED.INSURED_CD = cod_rol;
                                            _INSURED.INSURED_CD = dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INSURED_3G_CD;
                                            //Edward Rubiano -- HD3501 -- 22/10/2015

                                            // se debe buscar el branch_cd de 3G
                                            //if (dtoMaster.DtoInsured.dtoDataInsured.maseg_header.State.Equals('C'))
                                            _INSURED.BRANCH_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoInsured.dtoDataInsured.maseg_header.BRANCH_ID));
                                            int cod_figura_aseg = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoInsured.dtoDataInsured.maseg_header.cod_figura_aseg));
                                            _INSURED.INS_PROFILE_CD = cod_figura_aseg == 0 ? 1 : cod_figura_aseg;
                                            _INSURED.INS_SEGMENT_CD = Convert.ToInt32(dtoMaster.DtoInsured.dtoDataInsured.maseg_header.cod_tipo_aseg);
                                            _INSURED.ENTERED_DATE = dtoMaster.DtoInsured.dtoDataInsured.maseg_header.fec_alta;
                                            _INSURED.DECLINED_DATE = dtoMaster.DtoInsured.dtoDataInsured.maseg_header.fec_baja;
                                            if (Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoInsured.dtoDataInsured.maseg_header.cod_baja)) == 3 || Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoInsured.dtoDataInsured.maseg_header.cod_baja)) == 4 ||
                                               Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoInsured.dtoDataInsured.maseg_header.cod_baja)) == 5 || Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoInsured.dtoDataInsured.maseg_header.cod_baja)) == 6 ||
                                               Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoInsured.dtoDataInsured.maseg_header.cod_baja)) == 8 || Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoInsured.dtoDataInsured.maseg_header.cod_baja)) == 19)
                                            {
                                                _INSURED.INS_DECLINED_TYPE_CD = dtoMaster.DtoInsured.dtoDataInsured.maseg_header.cod_baja;
                                            }
                                            else
                                            {
                                                _INSURED.INS_DECLINED_TYPE_CD = null;
                                            }
                                            _INSURED.MAIN_INSURED_IND_ID = Convert.ToString(id_persona);
                                            _INSURED.CHECK_PAYABLE_TO = dtoMaster.DtoInsured.dtoDataInsured.maseg_header.txt_nom_factura;
                                            _INSURED.REFERRED_BY = dtoMaster.DtoInsured.dtoDataInsured.maseg_header.txt_vincula;
                                            _INSURED.ANNOTATIONS = dtoMaster.DtoInsured.dtoDataInsured.maseg_header.ANNOTATIONS;

                                            List<INSURED> List_INSURED = new List<INSURED>();
                                            List_INSURED.Add(_INSURED);
                                            ListTableMessage = setDto.List_INSURED(List_INSURED, ListTableMessage);
                                            //Modificacion Ingreso COMM. PARAMETER
                                            //ListTableMessage = setDto.PARAMETER_Update(List_INSURED, ListTableMessage);

                                            TableMessage result = ListTableMessage.Find(delegate(TableMessage bk) { return bk.NameTable == "INSURED"; });
                                            IsSave = result.Message;

                                            if (IsSave == "True")
                                            {
                                                if (dtoMaster.DtoInsured.dtoDataInsured.mpersona_requiere_sarlaft != null && dtoMaster.DtoInsured.dtoDataInsured.maseg_autoriza_consul != null)
                                                {
                                                    if (dtoMaster.DtoInsured.dtoDataInsured.mpersona_requiere_sarlaft.sn_exonerado == 0)
                                                    {
                                                        CO_INSURED _CO_INSURED = new CO_INSURED();
                                                        _CO_INSURED.State = state_coinsured;
                                                        //Edward Rubiano -- 3504
                                                        //_CO_INSURED.INDIVIDUAL_ID = id_personaref;
                                                        _CO_INSURED.INDIVIDUAL_ID = dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INDIVIDUAL_3G_ID;
                                                        _CO_INSURED.SIGNING_DATE = dtoMaster.DtoInsured.dtoDataInsured.maseg_autoriza_consul.fec_sing;
                                                        _CO_INSURED.AUTHORIZED_BY = dtoMaster.DtoInsured.dtoDataInsured.maseg_autoriza_consul.txt_nombre_autoriza;
                                                        _CO_INSURED.ID_CARD_NO = dtoMaster.DtoInsured.dtoDataInsured.maseg_autoriza_consul.nro_doc;
                                                        _CO_INSURED.ID_CARD_TYPE_CD = dtoMaster.DtoInsured.dtoDataInsured.maseg_autoriza_consul.cod_tipo_doc.ToString();
                                                        _CO_INSURED.USER_CREATE = dtoMaster.DtoInsured.dtoDataInsured.maseg_autoriza_consul.fec_sing;
                                                        _CO_INSURED.CREATE_DATE = dtoMaster.DtoInsured.dtoDataInsured.maseg_autoriza_consul.fec_sing;
                                                        _CO_INSURED.LAST_UPDATE_USER = dtoMaster.DtoInsured.dtoDataInsured.maseg_autoriza_consul.fec_sing;
                                                        _CO_INSURED.LAST_UPDATE_DATE = dtoMaster.DtoInsured.dtoDataInsured.maseg_autoriza_consul.fec_sing;

                                                        List<CO_INSURED> List_CO_INSURED = new List<CO_INSURED>();
                                                        List_CO_INSURED.Add(_CO_INSURED);
                                                        ListTableMessage = setDto.List_CO_INSURED(List_CO_INSURED, ListTableMessage);
                                                    }
                                                }

                                                INSURED_AGENT _INSURED_AGENT = new INSURED_AGENT();
                                                _INSURED_AGENT.State = state_agent;
                                                //Edward Rubiano -- 3504
                                                //_INSURED_AGENT.INSURED_IND_ID = id_personaref;
                                                _INSURED_AGENT.INSURED_IND_ID = dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INDIVIDUAL_3G_ID;
                                                _INSURED_AGENT.AGENT_IND_ID = dtoMaster.DtoInsured.dtoDataInsured.maseg_header.AGENT_INDIVIDUAL_ID;
                                                _INSURED_AGENT.AGENT_AGENCY_ID = dtoMaster.DtoInsured.dtoDataInsured.maseg_header.AGENT_AGENCY_ID;
                                                _INSURED_AGENT.IS_MAIN = true;

                                                List<INSURED_AGENT> List_INSURED_AGENT = new List<INSURED_AGENT>();
                                                List_INSURED_AGENT.Add(_INSURED_AGENT);
                                                ListTableMessage = setDto.List_INSURED_AGENT(List_INSURED_AGENT, ListTableMessage);

                                                INSURED_CONTRACTOR _INSURED_CONTRACTOR = new INSURED_CONTRACTOR();
                                                _INSURED_CONTRACTOR.State = state_contragarantias;

                                                //Edward Rubiano -- HD3519 -- 04/11/2015
                                                if (dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INDIVIDUAL_3G_ID != 0 &&
                                                    dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INDIVIDUAL_3G_ID != null)
                                                {
                                                    _INSURED_CONTRACTOR.INDIVIDUAL_ID = dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INDIVIDUAL_3G_ID;
                                                }
                                                else
                                                {
                                                    _INSURED_CONTRACTOR.INDIVIDUAL_ID = id_personaref;
                                                }
                                                //Edward Rubiano -- HD3519 -- 04/11/2015
                                                _INSURED_CONTRACTOR.IS_MANDATORY_GUARANTEE = (dtoMaster.DtoInsured.dtoDataInsured.maseg_header.sn_valida_cgarantia == -1 ? true : false);

                                                List<INSURED_CONTRACTOR> List_INSURED_CONTRACTOR = new List<INSURED_CONTRACTOR>();
                                                List_INSURED_CONTRACTOR.Add(_INSURED_CONTRACTOR);
                                                ListTableMessage = setDto.List_INSURED_CONTRACTOR(List_INSURED_CONTRACTOR, ListTableMessage);
                                            }
                                        }

                                    }

                                    TableMessage resul = ListTableMessage.Find(delegate(TableMessage bk) { return bk.NameTable == "INDIVIDUAL"; });
                                    IsSave = resul.Message;

                                    if (dtoMaster.DtoInsured.dtoDataInsured.mpersona_requiere_sarlaft != null && IsSave == "True")
                                    {
                                        if (!dtoMaster.DtoInsured.dtoDataInsured.mpersona_requiere_sarlaft.State.Equals('V'))
                                        {
                                            INDIVIDUAL_SARLAFT_EXONERATION _INDIVIDUAL_SARLAFT_EXONERATION = new INDIVIDUAL_SARLAFT_EXONERATION();
                                            _INDIVIDUAL_SARLAFT_EXONERATION.State = state_requiere;
                                            _INDIVIDUAL_SARLAFT_EXONERATION.INDIVIDUAL_ID = id_persona;
                                            _INDIVIDUAL_SARLAFT_EXONERATION.USER_ID = 1;
                                            _INDIVIDUAL_SARLAFT_EXONERATION.EXONERATION_TYPE_CD = dtoMaster.DtoInsured.dtoDataInsured.mpersona_requiere_sarlaft.cod_motivo_exonera;
                                            if (dtoMaster.DtoInsured.dtoDataInsured.mpersona_requiere_sarlaft.sn_exonerado == -1)
                                                _INDIVIDUAL_SARLAFT_EXONERATION.IS_EXONERATED = true;
                                            else
                                                _INDIVIDUAL_SARLAFT_EXONERATION.IS_EXONERATED = false;

                                            _INDIVIDUAL_SARLAFT_EXONERATION.REGISTRATION_DATE = dtoMaster.DtoInsured.dtoDataInsured.mpersona_requiere_sarlaft.fec_modifica;

                                            string RoleCd = !string.IsNullOrEmpty(dtoMaster.DtoInsured.dtoDataInsured.mpersona_requiere_sarlaft.txt_origen) ? dtoMaster.DtoInsured.dtoDataInsured.mpersona_requiere_sarlaft.txt_origen.Substring(0, 1) : null;
                                            if (RoleCd == "A")
                                                _INDIVIDUAL_SARLAFT_EXONERATION.ROLE_CD = 1;
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
                                #endregion //FIN ASEGURADO

                            #region Complemento

                            ds = new DataSet();
                            ds = stateinsured(App, Convert.ToString(id_persona));

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                state_individual = Convert.ToChar(ds.Tables[0].Rows[0][3]);
                                if (state_individual.Equals('U'))
                                    IsSave = "True";
                            }
                            IsSave = "True";

                            if (dtoMaster.DtoInsured.List_Mpersona_dir != null && IsSave == "True")
                            {
                                List<ADDRESS> List_ADDRESS = new List<ADDRESS>();

                                foreach (Mpersona_dir dir in dtoMaster.DtoInsured.List_Mpersona_dir)
                                {
                                    if (Convert.ToInt32(dir.cod_tipo_dir) != 99)
                                    {
                                        ADDRESS _ADDRESS = new ADDRESS();
                                        _ADDRESS.State = dir.State_3g;

                                    _ADDRESS.INDIVIDUAL_ID = id_persona;
                                    _ADDRESS.DATA_ID = dir.DATA_ID;
                                    _ADDRESS.ADDRESS_TYPE_CD = Convert.ToInt32(dir.cod_tipo_dir);
                                    _ADDRESS.STREET_TYPE_CD = 1;
                                    _ADDRESS.STREET = dir.txt_direccion;
                                    _ADDRESS.ZIP_CODE = dir.nro_cod_postal;
                                    _ADDRESS.COUNTRY_CD = dir.cod_pais.ToString();
                                    _ADDRESS.STATE_CD = dir.cod_dpto.ToString();
                                    _ADDRESS.CITY_CD = dir.cod_municipio.ToString();
                                    _ADDRESS.IS_MAILING_ADDRESS = dir.IS_MAILING_ADDRESS;
                                    _ADDRESS.IS_HOME = dir.IS_HOME; // David vanegas
                                    List_ADDRESS.Add(_ADDRESS);
                                    }

                                }

                                ListTableMessage = setDto.List_ADDRESS(List_ADDRESS, ListTableMessage);
                            }

                            if (dtoMaster.DtoInsured.List_Mpersona_telef != null && IsSave == "True")
                            {
                                List<PHONE> List_PHONE = new List<PHONE>();

                                foreach (Mpersona_telef phone in dtoMaster.DtoInsured.List_Mpersona_telef)
                                {
                                    if (phone.txt_telefono != " " && phone.txt_telefono != ".")
                                    {
                                        PHONE _PHONE = new PHONE();

                                        _PHONE.State = phone.State_3g;

                                        _PHONE.INDIVIDUAL_ID = id_persona;
                                        _PHONE.DATA_ID = phone.DATA_ID;
                                        _PHONE.PHONE_TYPE_CD = Convert.ToInt32(phone.cod_tipo_telef);
                                        _PHONE.IS_HOME = phone.sn_domicilio == 0 ? false : true; // David vanegas
                                        //Autor: Edward Rubiano; Fecha: 08/04/2016; Asunto: C11226; Descripcion: Se agregan el codigo de pais, departamento y ciudad luego de agregar foraneas
                                        _PHONE.COUNTRY_CD = Convert.ToInt32(phone.cod_pais);
                                        _PHONE.STATE_CD = Convert.ToInt32(phone.cod_dpto);
                                        _PHONE.CITY_CD = Convert.ToInt32(phone.cod_municipio);
                                        //Autor: Edward Rubiano; Fecha: 08/04/2016
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
                                        //Edward Rubiano -- HD3519 -- 10/11/2015
                                        long i = 0;
                                        bool correctFormat = long.TryParse((telefono == "") ? phone.txt_telefono : telefono, out i);
                                        if (correctFormat)
                                        {
                                        //Edward Rubiano -- 10/11/2015
                                            _PHONE.PHONE_NUMBER = Convert.ToInt64(FullServicesProviderHelper.ToNullInt(telefono == "" ? phone.txt_telefono : telefono));
                                            _PHONE.EXTENSION = phone.EXTENSION;
                                            _PHONE.SCHEDULE_AVAILABILITY = phone.SCHEDULE_AVAILABILITY;
                                            
                                            List_PHONE.Add(_PHONE);
                                        //Edward Rubiano -- HD3519 -- 10/11/2015
                                        }
                                        //Edward Rubiano -- 10/11/2015
                                    }
                                }

                                ListTableMessage = setDto.List_PHONE(List_PHONE, ListTableMessage);
                            }

                            if (dtoMaster.DtoInsured.List_Mpersona_email != null && IsSave == "True")
                            {
                                List<EMAIL> List_EMAIL = new List<EMAIL>();

                                foreach (Mpersona_email mail in dtoMaster.DtoInsured.List_Mpersona_email)
                                {
                                    EMAIL _EMAIL = new EMAIL();

                                    _EMAIL.State = mail.State_3g;

                                    _EMAIL.INDIVIDUAL_ID = id_persona;
                                    _EMAIL.DATA_ID = mail.DATA_ID;
                                    _EMAIL.ADDRESS = mail.txt_direccion_email;
                                    _EMAIL.EMAIL_TYPE_CD = mail.cod_tipo_email;
                                    _EMAIL.IS_MAILING_ADDRESS = mail.IS_MAILING_ADDRESS;

                                    List_EMAIL.Add(_EMAIL);
                                }

                                ListTableMessage = setDto.List_EMAIL(List_EMAIL, ListTableMessage);
                            }

                            if (dtoMaster.DtoInsured.List_Maseg_asociacion != null && IsSave == "True")
                            {
                                List<CO_CONSORTIUM> List_CO_CONSORTIUM = new List<CO_CONSORTIUM>();

                                foreach (Maseg_asociacion asociacion in dtoMaster.DtoInsured.List_Maseg_asociacion)
                                {
                                    CO_CONSORTIUM asoc = new CO_CONSORTIUM();

                                    asoc.State = asociacion.State_3G;

                                    //Edward Rubiano -- HD3529 -- 09/11/2015
                                    //asoc.INSURED_CD = cod_rol;
                                    asoc.INSURED_CD = dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INSURED_3G_CD;
                                    //Edward Rubiano -- 09/11/2015
                                    //asoc.INDIVIDUAL_ID = asociacion.cod_aseg_asociacion;

                                    ds = new DataSet();
                                    ds = person_2g(asociacion.cod_aseg.ToString());
                                    asoc.INDIVIDUAL_ID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                                    //Edward Rubiano -- HD3517 -- 02/11/2015
                                    asoc.INDIVIDUAL_ID = getDto.getIndividualID3g(asoc.INDIVIDUAL_ID);
                                    //Edward Rubiano -- 02/11/2015

                                    asoc.CONSORTIUM_ID = asociacion.nro_correla_asoc;
                                    asoc.IS_MAIN = asociacion.sn_ppal == -1 ? true : false;
                                    //Edward Rubiano -- HD3554 -- 10/12/2015
                                    //asoc.PARTICIPATION_RATE = asociacion.pje_part;
                                    asoc.PARTICIPATION_RATE = Convert.ToDouble(asociacion.pje_part);
                                    //Edward Rubiano -- HD3554 -- 10/12/2015
                                    asoc.START_DATE = asociacion.fec_asociacion;
                                    asoc.ENABLED = true;

                                    List_CO_CONSORTIUM.Add(asoc);
                                }

                                ListTableMessage = setDto.List_CO_CONSORTIUM(List_CO_CONSORTIUM, ListTableMessage);
                            }

                            if (dtoMaster.DtoInsured.dtoRep != null)
                            {
                                if (dtoMaster.DtoInsured.dtoRep.List_mpersona_rep_legal_dir != null)
                                {
                                    if (dtoMaster.DtoInsured.dtoRep.List_mpersona_rep_legal_dir.Count != 0 && dtoMaster.DtoInsured.dtoRep.List_mpersona_rep_legal_dir[0].cod_tipo_dir != 99 && IsSave == "True")
                                    {
                                        INDIVIDUAL_LEGAL_REPRESENT _INDIVIDUAL_LEGAL_REPRESENT = new INDIVIDUAL_LEGAL_REPRESENT();
                                        _INDIVIDUAL_LEGAL_REPRESENT.State = dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.State_3G;
                                        //Edward Rubiano -- 3504
                                        //_INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_ID = dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.id_persona;
                                        _INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_ID = dtoMaster.DtoInsured.dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INDIVIDUAL_3G_ID;
                                        _INDIVIDUAL_LEGAL_REPRESENT.LEGAL_REPRESENTATIVE_NAME = dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.txt_nombre == null ? "" : dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.txt_nombre;
                                        _INDIVIDUAL_LEGAL_REPRESENT.EXPEDITION_DATE = dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.fec_expedicion_doc == null ? DateTime.Now.ToString("dd/MM/yyyy") : dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.fec_expedicion_doc;
                                        _INDIVIDUAL_LEGAL_REPRESENT.EXPEDITION_PLACE = dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.txt_lugar_expedicion == null ? "" : dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.txt_lugar_expedicion;
                                        _INDIVIDUAL_LEGAL_REPRESENT.BIRTH_DATE = dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.fec_nacimiento == null ? DateTime.Now.ToString("dd/MM/yyyy") : dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.fec_nacimiento;
                                        _INDIVIDUAL_LEGAL_REPRESENT.BIRTH_PLACE = dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.txt_lugar_nacimi == null ? "" : dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.txt_lugar_nacimi;
                                        _INDIVIDUAL_LEGAL_REPRESENT.NATIONALITY = dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.txt_nacionalidad == null ? "" : dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.txt_nacionalidad;
                                        _INDIVIDUAL_LEGAL_REPRESENT.CITY = dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.txt_ciudad == null ? "" : dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.txt_ciudad;
                                        _INDIVIDUAL_LEGAL_REPRESENT.PHONE = dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.txt_telefono == null ? "" : dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.txt_telefono;
                                        _INDIVIDUAL_LEGAL_REPRESENT.JOB_TITLE = dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.txt_cargo == null ? "" : dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.txt_cargo;
                                        _INDIVIDUAL_LEGAL_REPRESENT.CELL_PHONE = dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.txt_celular == null ? "" : dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.txt_celular;
                                        _INDIVIDUAL_LEGAL_REPRESENT.EMAIL = dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.txt_email == null ? "" : dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.txt_email;
                                        _INDIVIDUAL_LEGAL_REPRESENT.ID_CARD_NO = dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.nro_doc_rep_legal == null ? "" : dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.nro_doc_rep_legal;
                                        _INDIVIDUAL_LEGAL_REPRESENT.AUTHORIZATION_AMT = dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.vr_facultades == null ? "" : dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.vr_facultades;
                                        _INDIVIDUAL_LEGAL_REPRESENT.DESCRIPTION = dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.txt_facultades == null ? "" : dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.txt_facultades;
                                        _INDIVIDUAL_LEGAL_REPRESENT.CURRENCY_CD = dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.cod_unidad == "-1" ? null : dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.cod_unidad;
                                        _INDIVIDUAL_LEGAL_REPRESENT.ID_CARD_TYPE_CD = Convert.ToInt32(dtoMaster.DtoInsured.dtoRep.mpersona_rep_legal.cod_tipo_doc);

                                        _INDIVIDUAL_LEGAL_REPRESENT.ADDRESS = dtoMaster.DtoInsured.dtoRep.List_mpersona_rep_legal_dir[0].txt_direccion;
                                        _INDIVIDUAL_LEGAL_REPRESENT.COUNTRY_CD = Convert.ToInt32(dtoMaster.DtoInsured.dtoRep.List_mpersona_rep_legal_dir[0].cod_pais);
                                        _INDIVIDUAL_LEGAL_REPRESENT.STATE_CD = Convert.ToInt32(dtoMaster.DtoInsured.dtoRep.List_mpersona_rep_legal_dir[0].cod_dpto);
                                        _INDIVIDUAL_LEGAL_REPRESENT.CITY_CD = Convert.ToInt32(dtoMaster.DtoInsured.dtoRep.List_mpersona_rep_legal_dir[0].cod_municipio);

                                        List<INDIVIDUAL_LEGAL_REPRESENT> List_INDIVIDUAL_LEGAL_REPRESENT = new List<INDIVIDUAL_LEGAL_REPRESENT>();

                                        List_INDIVIDUAL_LEGAL_REPRESENT.Add(_INDIVIDUAL_LEGAL_REPRESENT);

                                        ListTableMessage = setDto.List_INDIVIDUAL_LEGAL_REPRESENT(List_INDIVIDUAL_LEGAL_REPRESENT, ListTableMessage);
                                    }
                                }
                            }

                            if (dtoMaster.DtoInsured.dtoTechnicalCard != null)
                            {
                                ds = new DataSet();
                                ds = userid_3g(App, User);

                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    /*ds = new DataSet();
                                    char statet = 'C';
                                    ds = statetechnicalc(App, Convert.ToString(id_persona));
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        statet = Convert.ToChar(ds.Tables[0].Rows[0][0]);
                                    }*/

                                    if (dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.ENROLLMENT_FROM != null && IsSave == "True")
                                    {
                                        dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.Update("TECHNICAL_CARD_ID", id_persona);
                                        dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.Update("INDIVIDUAL_ID", id_persona);
                                        dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.State = dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.State_3G;

                                        if (dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.State == 'C' || dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.State_3G == 'C')
                                            dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.REGISTRATION_DATE = DateTime.Now.ToString("dd/MM/yyyy");

                                        if (dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.REGISTERED_USER_ID != null)
                                        {
                                            ds = new DataSet();
                                            string userid = dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.REGISTERED_USER_ID;
                                            ds = userid_3g(App, userid);

                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.REGISTERED_USER_ID = Convert.ToString(ds.Tables[0].Rows[0][0]);
                                            }
                                            else
                                            {
                                                dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.REGISTERED_USER_ID = null;
                                            }
                                        }

                                        dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.ENROLLMENT_FROM = dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.ENROLLMENT_FROM;
                                        ListTableMessage = setDto.TECHNICAL_CARD(dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD, ListTableMessage);
                                    }

                                    if (dtoMaster.DtoInsured.dtoTechnicalCard.List_BOARD_DIRECTORS != null && IsSave == "True")
                                    {
                                        dtoMaster.DtoInsured.dtoTechnicalCard.List_BOARD_DIRECTORS.Update(c => { c.TECHNICAL_CARD_ID = id_persona; });
                                        int count = 0;
                                        foreach (BOARD_DIRECTORS _BOARD_DIRECTORS in dtoMaster.DtoInsured.dtoTechnicalCard.List_BOARD_DIRECTORS)
                                        {
                                            _BOARD_DIRECTORS.State = dtoMaster.DtoInsured.dtoTechnicalCard.List_BOARD_DIRECTORS[count].State3G;
                                            count++;

                                        }
                                        ListTableMessage = setDto.List_BOARD_DIRECTORS(dtoMaster.DtoInsured.dtoTechnicalCard.List_BOARD_DIRECTORS, ListTableMessage);
                                    }

                                    if (dtoMaster.DtoInsured.dtoTechnicalCard.List_FINANCIAL_STATEMENTS != null && IsSave == "True")
                                    {
                                        dtoMaster.DtoInsured.dtoTechnicalCard.List_FINANCIAL_STATEMENTS.Update(c => { c.TECHNICAL_CARD_ID = id_persona; });
                                        dtoMaster.DtoInsured.dtoTechnicalCard.List_FINANCIAL_STATEMENTS.Update(c => { c.REGISTRATION_BALANCE_DATE = DateTime.Now.ToString("dd/MM/yyyy"); });

                                        ds = new DataSet();
                                        int user_3G = 0;
                                        ds = userid_3g(App, User);
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            user_3G = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                                        }

                                        dtoMaster.DtoInsured.dtoTechnicalCard.List_FINANCIAL_STATEMENTS.Update(c => { c.BALANCE_USER_ID = user_3G; });

                                        int count = 0;
                                        foreach (FINANCIAL_STATEMENTS _FINANCIAL_STATEMENTS in dtoMaster.DtoInsured.dtoTechnicalCard.List_FINANCIAL_STATEMENTS)
                                        {
                                            _FINANCIAL_STATEMENTS.State = dtoMaster.DtoInsured.dtoTechnicalCard.List_FINANCIAL_STATEMENTS[count].State_3G;
                                            count++;
                                        }

                                        int result = Convert.ToInt32(dtoMaster.DtoInsured.dtoDataInsured.mpersona.cod_tipo_persona == "F" ? "1" : "2");

                                        if (result == 2)
                                        {
                                            double MaxOperatingQuotaAmt = 0;
                                            ds = new DataSet();
                                            double MAX_OPERATING = 0;
                                            int ParameterOQ = 2228;
                                            ds = parameter_operatingq_3g(App, ParameterOQ);
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                MAX_OPERATING = Convert.ToDouble(ds.Tables[0].Rows[0][0]);
                                                MaxOperatingQuotaAmt = MAX_OPERATING;
                                            }

                                            if (dtoMaster.DtoInsured.dtoTechnicalCard.List_FINANCIAL_STATEMENTS != null)
                                            {
                                                dtoMaster.DtoInsured.dtoTechnicalCard.List_FINANCIAL_STATEMENTS = dtoMaster.DtoInsured.dtoTechnicalCard.List_FINANCIAL_STATEMENTS.Where(p => p.State != 'R').ToList();
                                            }

                                            if (dtoMaster.DtoInsured.dtoTechnicalCard.List_FINANCIAL_STATEMENTS.Count != 0)
                                            {
                                                double patrimony = dtoMaster.DtoInsured.dtoTechnicalCard.List_FINANCIAL_STATEMENTS[0].PATRIMONY_AMT;
                                                double operatingQuotaValue = (((MaxOperatingQuotaAmt > 0) && ((patrimony * 4) > MaxOperatingQuotaAmt)) ? MaxOperatingQuotaAmt : (patrimony * 4));

                                                ds = new DataSet();
                                                ds = statequota(App, Convert.ToString(id_persona));
                                                List<OPERATING_QUOTA> List_OPERATING_QUOTA = new List<OPERATING_QUOTA>();
                                                DatatableToList Dtl = new DatatableToList();
                                                List<OPERATING_QUOTA> listquota = new List<OPERATING_QUOTA>();
                                                listquota = Dtl.ConvertTo<OPERATING_QUOTA>(ds.Tables[0]);

                                                //Edward Rubiano - 04/04/2016 - C11236 - CUPO OPERATIVO
                                                //if (listquota.ToList().Count.Equals(0))
                                                if (listquota.ToList().Count < 2)
                                                //Edward Rubiano - 04/04/2016 - C11236 - CUPO OPERATIVO
                                                {
                                                    //Edward Rubiano - 04/04/2016 - C11236 - CUPO OPERATIVO
                                                    foreach (OPERATING_QUOTA oc in listquota)
                                                    {
                                                        oc.State = 'D';
                                                        List_OPERATING_QUOTA.Add(oc);

                                                    } 
                                                    ListTableMessage = setDto.List_OPERATING_QUOTA(List_OPERATING_QUOTA, ListTableMessage);
                                                    List_OPERATING_QUOTA.Clear();
                                                    //Edward Rubiano - 04/04/2016 - C11236 - CUPO OPERATIVO

                                                    OPERATING_QUOTA _OPERATING_QUOTA = new OPERATING_QUOTA();
                                                    OPERATING_QUOTA _OPERATING_QUOTA_1 = new OPERATING_QUOTA();
                                                    /*Se extiende la fecha de balance cuatro meses mas del año*/
                                                    //Grabo Pesos            
                                                    //Se cambia PatrimonyAmount * 4 por operatingQuotaValue (valor validado)*/

                                                    ds = new DataSet();
                                                    int lineq = 2;
                                                    ds = linequota(App, lineq);
                                                    if (ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        _OPERATING_QUOTA.INDIVIDUAL_ID = id_persona;
                                                        _OPERATING_QUOTA.LINE_BUSINESS_CD = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                                                        _OPERATING_QUOTA.CURRENT_TO = DateTime.Now.AddYears(1).AddMonths(4).ToString("dd/MM/yyyy");
                                                        _OPERATING_QUOTA.OPERATING_QUOTA_AMT = operatingQuotaValue;
                                                        _OPERATING_QUOTA.CURRENCY_CD = 0;
                                                        _OPERATING_QUOTA.State = 'C';
                                                        List_OPERATING_QUOTA.Add(_OPERATING_QUOTA);

                                                        // Grabo Dolares            
                                                        //Se cambia PatrimonyAmount * 4 por operatingQuotaValue (valor validado)
                                                        _OPERATING_QUOTA_1.INDIVIDUAL_ID = id_persona;
                                                        _OPERATING_QUOTA_1.LINE_BUSINESS_CD = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                                                        _OPERATING_QUOTA_1.CURRENT_TO = DateTime.Now.AddYears(1).AddMonths(4).ToString("dd/MM/yyyy");
                                                        double exchangeRat = 1;
                                                        int CURRENCY_CD = 1;
                                                        ds = new DataSet();
                                                        ds = exchangeRate(App, CURRENCY_CD);
                                                        if (ds.Tables[0].Rows.Count > 0)
                                                        {
                                                            if (Convert.ToDouble(ds.Tables[0].Rows[0][0]) != 0)
                                                            {
                                                                exchangeRat = (exchangeRat / Convert.ToDouble(ds.Tables[0].Rows[0][0]));
                                                            }
                                                            else
                                                            {
                                                                exchangeRat = 0;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            exchangeRat = 0;
                                                        }
                                                        _OPERATING_QUOTA_1.OPERATING_QUOTA_AMT = (operatingQuotaValue * exchangeRat);
                                                        _OPERATING_QUOTA_1.CURRENCY_CD = 1;
                                                        _OPERATING_QUOTA_1.State = 'C';
                                                        List_OPERATING_QUOTA.Add(_OPERATING_QUOTA_1);
                                                    }
                                                }
                                                else
                                                {
                                                    foreach (OPERATING_QUOTA _OPERATING_QUOTA in listquota)
                                                    {
                                                        // Solo si la fecha del balance actual es mayor a la fecha                      
                                                        if (Convert.ToDateTime(_OPERATING_QUOTA.CURRENT_TO) <= DateTime.Now.AddYears(1).AddMonths(4))
                                                        {
                                                            switch (_OPERATING_QUOTA.CURRENCY_CD)
                                                            {
                                                                case 0:
                                                                    /*Se extiende la fecha de balance cuatro meses mas del año*/
                                                                    _OPERATING_QUOTA.CURRENT_TO = DateTime.Now.AddYears(1).AddMonths(4).ToString("dd/MM/yyyy");
                                                                    //Se cambia PatrimonyAmount * 4 por operatingQuotaValue (valor validado)
                                                                    _OPERATING_QUOTA.OPERATING_QUOTA_AMT = operatingQuotaValue;
                                                                    _OPERATING_QUOTA.State = 'U';
                                                                    List_OPERATING_QUOTA.Add(_OPERATING_QUOTA);
                                                                    break;
                                                                case 1:
                                                                    /*Se extiende la fecha de balance cuatro meses mas del año*/
                                                                    _OPERATING_QUOTA.CURRENT_TO = DateTime.Now.AddYears(1).AddMonths(4).ToString("dd/MM/yyyy");
                                                                    //Se cambia PatrimonyAmount * 4 por operatingQuotaValue (valor validado)
                                                                    double exchangeRat = 1;
                                                                    int CURRENCY_CD = 1;
                                                                    ds = new DataSet();
                                                                    ds = exchangeRate(App, CURRENCY_CD);
                                                                    if (ds.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        if (Convert.ToDouble(ds.Tables[0].Rows[0][0]) != 0)
                                                                        {
                                                                            exchangeRat = (exchangeRat / Convert.ToDouble(ds.Tables[0].Rows[0][0]));
                                                                        }
                                                                        else
                                                                        {
                                                                            exchangeRat = 0;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        exchangeRat = 0;
                                                                    }
                                                                    _OPERATING_QUOTA.OPERATING_QUOTA_AMT = (operatingQuotaValue * exchangeRat);
                                                                    _OPERATING_QUOTA.State = 'U';
                                                                    List_OPERATING_QUOTA.Add(_OPERATING_QUOTA);
                                                                    break;
                                                            }
                                                        }
                                                    }
                                                }
                                                ListTableMessage = setDto.List_OPERATING_QUOTA(List_OPERATING_QUOTA, ListTableMessage);
                                            }
                                        }
                                        ListTableMessage = setDto.List_FINANCIAL_STATEMENTS(dtoMaster.DtoInsured.dtoTechnicalCard.List_FINANCIAL_STATEMENTS, ListTableMessage);
                                    }

                                    if (dtoMaster.DtoInsured.dtoTechnicalCard.List_TECHNICAL_CARD_DESCRIPTION != null && IsSave == "True")
                                    {
                                        dtoMaster.DtoInsured.dtoTechnicalCard.List_TECHNICAL_CARD_DESCRIPTION.Update(c => { c.TECHNICAL_CARD_ID = id_persona; });
                                        int count = 0;
                                        foreach (TECHNICAL_CARD_DESCRIPTION _TECHNICAL_CARD_DESCRIPTION in dtoMaster.DtoInsured.dtoTechnicalCard.List_TECHNICAL_CARD_DESCRIPTION)
                                        {
                                            _TECHNICAL_CARD_DESCRIPTION.State = dtoMaster.DtoInsured.dtoTechnicalCard.List_TECHNICAL_CARD_DESCRIPTION[count].State3G;

                                            ds = new DataSet();
                                            string userid = User;
                                            ds = userid_3g(App, userid);
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                _TECHNICAL_CARD_DESCRIPTION.USER_ID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                                            }
                                            count++;
                                        }
                                        ListTableMessage = setDto.List_TECHNICAL_CARD_DESCRIPTION(dtoMaster.DtoInsured.dtoTechnicalCard.List_TECHNICAL_CARD_DESCRIPTION, ListTableMessage);
                                    }
                                }
                                else
                                {
                                    if (dtoMaster.DtoInsured.dtoTechnicalCard.TECHNICAL_CARD.ENROLLMENT_FROM != null)
                                    {
                                        TableMessage tMessage = new TableMessage();
                                        tMessage.Message = "No se puede crear ficha tecnica porque el usuario no existe en 3G";
                                        tMessage.NameTable = "TECHNICAL_CARD";
                                        ListTableMessage.Add(tMessage);
                                    }
                                    else
                                    {
                                        TableMessage tMessage = new TableMessage();
                                        tMessage.Message = "True";
                                        tMessage.NameTable = "TECHNICAL_CARD";
                                        ListTableMessage.Add(tMessage);
                                    }
                                }
                            }


                            if (dtoMaster.DtoInsured.dtoSarlaft != null)
                            {
                                if (dtoMaster.DtoInsured.dtoSarlaft.List_detalle != null && IsSave == "True")
                                {
                                    List<INDIVIDUAL_SARLAFT> List_INDIVIDUAL_SARLAFT = new List<INDIVIDUAL_SARLAFT>();

                                    foreach (Frm_sarlaft_detalle detalle in dtoMaster.DtoInsured.dtoSarlaft.List_detalle)
                                    {
                                        INDIVIDUAL_SARLAFT individual = new INDIVIDUAL_SARLAFT();

                                        individual.SARLAFT_ID = detalle.id_formulario;
                                        individual.INDIVIDUAL_ID = id_persona;
                                        individual.FORM_NUM = Convert.ToString(detalle.nro_formulario);
                                        individual.YEAR = detalle.aaaa_formulario;
                                        individual.REGISTRATION_DATE = detalle.fec_registro;
                                        individual.AUTHORIZED_BY = detalle.txt_usuario_auto;
                                        individual.FILLING_DATE = detalle.fec_diligenciamiento;
                                        individual.CHECK_DATE = detalle.fec_verifica;
                                        individual.VERIFYING_EMPLOYEE = detalle.txt_usuario_veri;
                                        individual.BRANCH_CD = Convert.ToString(detalle.cod_suc);

                                        if (dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_detalle_entrevista != null)
                                        {
                                            individual.INTERVIEW_DATE = dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_detalle_entrevista.fec_entrevista;
                                            individual.INTERVIEWER_NAME = dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_detalle_entrevista.txt_usua_entrev;
                                            individual.INTERVIEW_PLACE = dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_detalle_entrevista.txt_lugar_entrev;
                                            individual.INTERVIEW_RESULT_CD = (dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_detalle_entrevista.txt_resul_entrev == "RECHAZADO" ? 2 : 1);
                                        }
                                        if (dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_info_financiera != null)
                                        {
                                            individual.INTERNATIONAL_OPERATIONS = dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_info_financiera.id_formulario != 0 ? true : false;
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

                                        if (dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_info_financiera != null)
                                        {
                                            individual.ECONOMIC_ACTIVITY_CD = Convert.ToInt32(dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_info_financiera.cod_act_principal);
                                            individual.SECOND_ECONOMIC_ACTIVITY_CD = Convert.ToInt32(dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_info_financiera.cod_act_secundaria == -1 ? 0 : dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_info_financiera.cod_act_secundaria);

                                        }

                                        individual.PENDING_EVENT = dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_aut_incrementos != null ? true : false;
                                        individual.State = detalle.State_3G;

                                        List_INDIVIDUAL_SARLAFT.Add(individual);
                                    }

                                    ListTableMessage = setDto.List_INDIVIDUAL_SARLAFT(List_INDIVIDUAL_SARLAFT, ListTableMessage);

                                }

                                if (dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_vinculos != null && IsSave == "True")
                                {
                                    List<INDIVIDUAL_LINK> List_INDIVIDUAL_LINK = new List<INDIVIDUAL_LINK>();
                                    INDIVIDUAL_LINK _INDIVIDUAL_LINK = new INDIVIDUAL_LINK();
                                    if (dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_vinculos.tomador_asegurado != 0)
                                    {
                                        _INDIVIDUAL_LINK = new INDIVIDUAL_LINK();
                                        _INDIVIDUAL_LINK.INDIVIDUAL_ID = id_persona;
                                        _INDIVIDUAL_LINK.LINK_TYPE_CD = Convert.ToInt32(dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_vinculos.tomador_asegurado);
                                        _INDIVIDUAL_LINK.RELATIONSHIP_SARLAFT_CD = 1;
                                        _INDIVIDUAL_LINK.DESCRIPTION = dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_vinculos.txt_desc_TA;
                                        _INDIVIDUAL_LINK.State = dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_vinculos.State_3G;
                                        List_INDIVIDUAL_LINK.Add(_INDIVIDUAL_LINK);
                                    }
                                    if (dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_vinculos.tomador_benef != 0)
                                    {
                                        _INDIVIDUAL_LINK = new INDIVIDUAL_LINK();
                                        _INDIVIDUAL_LINK.INDIVIDUAL_ID = id_persona;
                                        _INDIVIDUAL_LINK.LINK_TYPE_CD = Convert.ToInt32(dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_vinculos.tomador_benef);
                                        _INDIVIDUAL_LINK.RELATIONSHIP_SARLAFT_CD = 2;
                                        _INDIVIDUAL_LINK.DESCRIPTION = dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_vinculos.txt_desc_TB;
                                        _INDIVIDUAL_LINK.State = dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_vinculos.State_3G;
                                        List_INDIVIDUAL_LINK.Add(_INDIVIDUAL_LINK);
                                    }
                                    if (dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_vinculos.asegurado_benef != 0)
                                    {
                                        _INDIVIDUAL_LINK = new INDIVIDUAL_LINK();
                                        _INDIVIDUAL_LINK.INDIVIDUAL_ID = id_persona;
                                        _INDIVIDUAL_LINK.LINK_TYPE_CD = Convert.ToInt32(dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_vinculos.asegurado_benef);
                                        _INDIVIDUAL_LINK.RELATIONSHIP_SARLAFT_CD = 3;
                                        _INDIVIDUAL_LINK.DESCRIPTION = dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_vinculos.txt_desc_AB;
                                        _INDIVIDUAL_LINK.State = dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_vinculos.State_3G;
                                        List_INDIVIDUAL_LINK.Add(_INDIVIDUAL_LINK);
                                    }

                                    ListTableMessage = setDto.List_INDIVIDUAL_LINK(List_INDIVIDUAL_LINK, ListTableMessage);
                                }
                                if (dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_info_financiera != null && IsSave == "True")
                                {
                                    FINANCIAL_SARLAFT _FINANCIAL_SARLAFT = new FINANCIAL_SARLAFT();

                                    _FINANCIAL_SARLAFT.SARLAFT_ID = dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_info_financiera.id_formulario;
                                    _FINANCIAL_SARLAFT.INCOME_AMT = dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_info_financiera.imp_ingresos;
                                    _FINANCIAL_SARLAFT.EXPENSE_AMT = dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_info_financiera.imp_egresos;
                                    _FINANCIAL_SARLAFT.EXTRA_INCOME_AMT = dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_info_financiera.imp_otros_ingr;
                                    _FINANCIAL_SARLAFT.ASSETS_AMT = dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_info_financiera.imp_activos;
                                    _FINANCIAL_SARLAFT.LIABILITIES_AMT = dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_info_financiera.imp_pasivos;
                                    _FINANCIAL_SARLAFT.DESCRIPTION = dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_detalle_entrevista.txt_obser_entrev == null ? "" : dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_detalle_entrevista.txt_obser_entrev;
                                    _FINANCIAL_SARLAFT.State = dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_detalle_entrevista.State_3G;
                                    _FINANCIAL_SARLAFT.INDIVIDUAL_ID = id_persona;

                                    List<FINANCIAL_SARLAFT> List_FINANCIAL_SARLAFT = new List<FINANCIAL_SARLAFT>();
                                    List_FINANCIAL_SARLAFT.Add(_FINANCIAL_SARLAFT);

                                    if (dtoMaster.DtoInsured.dtoSarlaft.frm_sarlaft_info_financiera.id_formulario != 0)
                                    {
                                        int typeindividual = (dtoMaster.DtoInsured.dtoDataInsured.mpersona.cod_tipo_persona == "F" ? 1 : 2);
                                        if (typeindividual == 1)
                                        {
                                            ListTableMessage = setDto.List_OPERATING_QUOTA_SARLAFT(List_FINANCIAL_SARLAFT, ListTableMessage);
                                        }
                                    }
                                    ListTableMessage = setDto.List_FINANCIAL_SARLAFT(List_FINANCIAL_SARLAFT, ListTableMessage);
                                }

                                if (dtoMaster.DtoInsured.dtoSarlaft.List_oper_internacionales != null && IsSave == "True")
                                {
                                    List<SARLAFT_OPERATION> List_SARLAFT_OPERATION = new List<SARLAFT_OPERATION>();

                                    foreach (Frm_sarlaft_oper_internac internat in dtoMaster.DtoInsured.dtoSarlaft.List_oper_internacionales)
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

                                if (dtoMaster.DtoInsured.List_INDIVIDUAL_TAX_EXEMPTION != null && IsSave == "True")
                                {
                                    List<INDIVIDUAL_TAX_EXEMPTION> List_INDIVIDUAL_TAX_EXEMPTION = new List<INDIVIDUAL_TAX_EXEMPTION>();
                                    foreach (INDIVIDUAL_TAX_EXEMPTION tax in dtoMaster.DtoInsured.List_INDIVIDUAL_TAX_EXEMPTION)
                                    {
                                        if (tax.State == 'C')
                                        {
                                            ds = new DataSet();
                                            string Name_Company = "Previsora";
                                            ds = NumberCountry_3g(App, Name_Company);
                                            string Country = string.Empty;

                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                Country = Convert.ToString(ds.Tables[0].Rows[0][0]);
                                            }

                                            tax.COUNTRY_CD = Country;

                                            ds = TaxId_3g(App, id_persona);
                                            int IdTax = 0;

                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                IdTax = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                                            }

                                            tax.IND_TAX_EXEMPTION_ID = IdTax;

                                        }

                                        tax.INDIVIDUAL_ID = id_persona;
                                        tax.STATE_CD = (tax.STATE_CD == "0" ? null : tax.STATE_CD);
                                        tax.TAX_CATEGORY_CD = (tax.TAX_CATEGORY_CD == "0" ? null : tax.TAX_CATEGORY_CD);
                                        List_INDIVIDUAL_TAX_EXEMPTION.Add(tax);
                                    }

                                    ListTableMessage = setDto.List_INDIVIDUAL_TAX_EXEMPTION(List_INDIVIDUAL_TAX_EXEMPTION, ListTableMessage);
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

        private char statefor3g(string App, string cod_tipo_persona, string cod_tipo_doc, string nro_doc, int id_Persona)
        {
            char state = 'C';
            DGeneric = new DataGenericExecute(App);
            DataSet ds = new DataSet();

            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.VarChar";
            parameter.Parameter = "TABLE_NAME";
            parameter.Value = "UP.INSURED";
            listParameter.Add(parameter);

            parameter = new Parameters();
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
            parameter.Parameter = "INDIVIDUAL_ID2G";
            parameter.Value = id_Persona.ToString();
            listParameter.Add(parameter);


            ds = DGeneric.ExecuteStoreProcedure("SUP.GET_PERSON", listParameter);
            //ds = DGeneric.ExecuteStoreProcedure("SUP.GET_PERSON", listParameter, Command);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) == 0)
                    state = 'C';
                else
                    state = 'U';
            }

            return state;

        }

        private DataSet stateinsured(string App, string idpersona)
        {
            DataSet ds = new DataSet();
            DGeneric = new DataGenericExecute(App);
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "id_persona";
            parameter.Value = (idpersona);
            listParameter.Add(parameter);
            ds = DGeneric.ExecuteStoreProcedure("SUP.GET_INSURED", listParameter);
            return ds;
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

        private DataSet person_2g(string cod_rol)
        {
            DGeneric = new DataGenericExecute("SISE2G");
            DataSet ds = new DataSet();
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "id_rol";
            parameter.Value = "1";
            listParameter.Add(parameter);

            parameter = new Parameters();
            parameter.ParameterType = "AseDbType.VarChar";
            parameter.Parameter = "cod_rol";
            parameter.Value = cod_rol;
            listParameter.Add(parameter);


            ds = DGeneric.ExecuteStoreProcedure("SUP.GET_IDPERSON", listParameter);
            return ds;
        }

        private DataSet NumberCountry_3g(string App, string Name_Company)
        {
            DGeneric = new DataGenericExecute(App);
            DataSet ds = new DataSet();
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.VarChar";
            parameter.Parameter = "Name_Company";
            parameter.Value = (Name_Company);
            listParameter.Add(parameter);
            ds = DGeneric.ExecuteStoreProcedure("SUP.GET_NUMBER_COUNTRY", listParameter);
            return ds;
        }

        private DataSet TaxId_3g(string App, int idpersona)
        {
            DGeneric = new DataGenericExecute(App);
            DataSet ds = new DataSet();
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "idpersona";
            parameter.Value = Convert.ToString(idpersona);
            listParameter.Add(parameter);
            ds = DGeneric.ExecuteStoreProcedure("SUP.GET_TAXID", listParameter);
            return ds;
        }

        private DataSet parameter_operatingq_3g(string App, int ParameterOQ)
        {
            DGeneric = new DataGenericExecute(App);
            DataSet ds = new DataSet();
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "ParameterOQ";
            parameter.Value = ParameterOQ.ToString();
            listParameter.Add(parameter);
            ds = DGeneric.ExecuteStoreProcedure("SUP.GET_PARAMETERO", listParameter);
            return ds;
        }

        private DataSet statequota(string App, string individual_id)
        {
            DGeneric = new DataGenericExecute(App);
            DataSet ds = new DataSet();
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "individual_id";
            parameter.Value = individual_id;
            listParameter.Add(parameter);
            ds = DGeneric.ExecuteStoreProcedure("SUP.GET_OPERATING_QUOTA", listParameter);
            return ds;
        }

        private DataSet linequota(string App, int HARD_RISK_TYPE_CD)
        {
            DGeneric = new DataGenericExecute(App);
            DataSet ds = new DataSet();
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "HARD_RISK_TYPE_CD";
            parameter.Value = HARD_RISK_TYPE_CD.ToString();
            listParameter.Add(parameter);
            ds = DGeneric.ExecuteStoreProcedure("SUP.GET_HARD_RISK_TYPE", listParameter);
            return ds;
        }

        private DataSet exchangeRate(string App, int CURRENCY_CD)
        {
            DGeneric = new DataGenericExecute(App);
            DataSet ds = new DataSet();
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "CURRENCY_CD";
            parameter.Value = CURRENCY_CD.ToString();
            listParameter.Add(parameter);
            ds = DGeneric.ExecuteStoreProcedure("SUP.GET_EXCHANGE_RATE", listParameter);
            return ds;
        }

        private DataSet statetechnicalc(string App, string idpersona)
        {
            DataSet ds = new DataSet();
            DGeneric = new DataGenericExecute(App);
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "id_persona";
            parameter.Value = (idpersona);
            listParameter.Add(parameter);
            ds = DGeneric.ExecuteStoreProcedure("SUP.GET_TECHNICAL_CARD", listParameter);
            return ds;
        }
        #endregion

    }
}
