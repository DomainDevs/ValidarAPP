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
    public class GSDtoBeneficiary
    {
        #region constructores

        public GSDtoBeneficiary()
        {

        }

        public GSDtoBeneficiary(string user, int idApp, int idRol)
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

        public DtoBeneficiary GetDtoBeneficiary(int id_rol, string cod_rol, string entity)
        {
            DtoBeneficiary dtoBeneficiary = new DtoBeneficiary();

            //Carga el Dto solo con una entidad especifica
            if (entity != "")
            {
                dtoBeneficiary = GetEntity(id_rol, cod_rol, entity);
            }
            else
            {
                //Carga el Dto Totalmente nuevo
                if (cod_rol == "new")
                    dtoBeneficiary = getDto.DtoBeneficiary();
                //Carga el Dto con toda la informacion del Rol buscado
                else
                {
                    dtoBeneficiary = GetDto(id_rol, cod_rol);
                }
            }
            return dtoBeneficiary;
        }

        private DtoBeneficiary GetDto(int id_rol, string cod_rol)
        {
            DtoBeneficiary dtoBeneficiary = new DtoBeneficiary();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataTable DtRolesAPP = DGeneric.GetListRolApp(IdRol);
            string App = string.Empty;

            foreach (DataRow dr in DtRolesAPP.Rows)
            {
                App = dr[0].ToString();
                switch (App)
                {
                    case "SISE2G":
                        GetDtoSISE2G(id_rol, cod_rol, ref dtoBeneficiary);
                        break;

                    case "SISE3G":
                        GetDtoSISE3G(id_rol, cod_rol, ref dtoBeneficiary);
                        break;

                }
            }

            return dtoBeneficiary;
        }

        private void GetDtoSISE2G(int id_rol, string cod_rol, ref DtoBeneficiary dtoBeneficiary)
        {
            int id_persona = getDto.id_persona(id_rol, cod_rol);
            dtoBeneficiary.dtoDataBeneficiary = getDto.dtoDataBeneficiary(Convert.ToInt32(cod_rol));
            dtoBeneficiary.dtoDataPerson = getDto.dtoDataPerson(id_persona);
            dtoBeneficiary.List_Mpersona_dir = getDto.List_Mpersona_dir(id_persona);
            dtoBeneficiary.List_Mpersona_telef = getDto.List_Mpersona_telef(id_persona);
            dtoBeneficiary.List_Mpersona_email = getDto.List_Mpersona_email(id_persona);
            dtoBeneficiary.List_Logbook = getDto.List_Logbook(id_persona, id_rol);
            dtoBeneficiary.dtoSarlaft = getDto.dtoSarlaft(id_persona);
            dtoBeneficiary.List_Referencias = getDto.List_Referencias(Convert.ToInt32(cod_rol), id_rol);
            dtoBeneficiary.List_Mbenef_asoc_aseg = getDto.List_Mbenef_asoc_aseg(Convert.ToInt32(cod_rol));
            dtoBeneficiary.List_Frm_sarlaft_accionistas_asoc = getDto.List_Frm_sarlaft_accionistas_asoc(id_persona);
            dtoBeneficiary.dtoRep = getDto.dtoRep(id_persona);
        }

        private void GetDtoSISE3G(int id_rol, string cod_rol, ref DtoBeneficiary dtoBeneficiary)
        {
            //logica de consulta para anexar campos de 3G
            /*int id_persona = getDto.id_persona(id_rol, cod_rol);
            dtoBeneficiary.List_Mpersona_dir = getDto.dtoDataAddress3g(id_persona, dtoBeneficiary.List_Mpersona_dir);
            dtoBeneficiary.List_Mpersona_telef = getDto.dtoDataPhone3g(id_persona, dtoBeneficiary.List_Mpersona_telef);
            dtoBeneficiary.List_Mpersona_email = getDto.dtoDataMail3g(id_persona, dtoBeneficiary.List_Mpersona_email);
            dtoBeneficiary.dtoDataBeneficiary.mpersona = getDto.dtoData3g(id_persona, dtoBeneficiary.dtoDataBeneficiary.mpersona);*/

        }

        private DtoBeneficiary GetEntity(int id_rol, string cod_rol, string entity)
        {
            DtoBeneficiary dtoBeneficiary = new DtoBeneficiary();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataTable DtRolesAPP = DGeneric.GetListRolApp(IdRol);
            string App = string.Empty;

            foreach (DataRow dr in DtRolesAPP.Rows)
            {
                App = dr[0].ToString();
                switch (App)
                {
                    case "SISE2G":
                        GetEntitySISE2G(id_rol, cod_rol, entity, ref dtoBeneficiary);
                        break;

                    case "SISE3G":
                        GetEntitySISE3G(id_rol, cod_rol, entity, ref dtoBeneficiary);
                        break;

                }
            }

            return dtoBeneficiary;
        }

        private void GetEntitySISE2G(int id_rol, string cod_rol, string entity, ref DtoBeneficiary dtoBeneficiary)
        {
            int id_persona = getDto.id_persona(id_rol, cod_rol);
            switch (entity)
            {
                case "dtoDataPerson":
                    dtoBeneficiary.dtoDataPerson = getDto.dtoDataPerson(id_persona);
                    break;
                case "dtoDataBeneficiary":
                    dtoBeneficiary.dtoDataBeneficiary = getDto.dtoDataBeneficiary(Convert.ToInt32(cod_rol));
                    break;
                case "List_Mpersona_dir":
                    dtoBeneficiary.List_Mpersona_dir = getDto.List_Mpersona_dir(id_persona);
                    break;
                case "List_Mpersona_telef":
                    dtoBeneficiary.List_Mpersona_telef = getDto.List_Mpersona_telef(id_persona);
                    break;
                case "List_Mpersona_email":
                    dtoBeneficiary.List_Mpersona_email = getDto.List_Mpersona_email(id_persona);
                    break;
                case "List_Logbook":
                    dtoBeneficiary.List_Logbook = getDto.List_Logbook(id_persona, id_rol);
                    break;
                case "dtoSarlaft":
                    dtoBeneficiary.dtoSarlaft = getDto.dtoSarlaft(id_persona);
                    break;
                case "dtoSarlaftForm":
                    dtoBeneficiary.dtoSarlaft = getDto.dtoSarlaftForm(Convert.ToInt32(cod_rol));
                    break;
                case "List_Referencias":
                    dtoBeneficiary.List_Referencias = getDto.List_Referencias(Convert.ToInt32(cod_rol), id_rol);
                    break;
                case "List_Mbenef_asoc_aseg":
                    dtoBeneficiary.List_Mbenef_asoc_aseg = getDto.List_Mbenef_asoc_aseg(Convert.ToInt32(cod_rol));
                    break;
                case "List_Frm_sarlaft_accionistas_asoc":
                    dtoBeneficiary.List_Frm_sarlaft_accionistas_asoc = getDto.List_Frm_sarlaft_accionistas_asoc(id_persona);
                    break;
                case "dtoRep":
                    dtoBeneficiary.dtoRep = getDto.dtoRep(id_persona);
                    break;
            }
        }

        private void GetEntitySISE3G(int id_rol, string cod_rol, string entity, ref DtoBeneficiary dtoBeneficiary)
        {
            //aqui codigo de otra base de datos llenando campos extras
            /*int id_persona = getDto.id_persona(id_rol, cod_rol);
            switch (entity)
            {
                case "dtoDataBeneficiary":
                    dtoBeneficiary.dtoDataBeneficiary.mpersona = getDto.dtoData3g(id_persona, dtoBeneficiary.dtoDataBeneficiary.mpersona);
                    break;
                case "List_Mpersona_dir":
                    dtoBeneficiary.List_Mpersona_dir = getDto.dtoDataAddress3g(id_persona, dtoBeneficiary.List_Mpersona_dir);
                    break;
                case "List_Mpersona_telef":
                    dtoBeneficiary.List_Mpersona_telef = getDto.dtoDataPhone3g(id_persona, dtoBeneficiary.List_Mpersona_telef);
                    break;

                case "List_Mpersona_email":
                    dtoBeneficiary.List_Mpersona_email = getDto.dtoDataMail3g(id_persona, dtoBeneficiary.List_Mpersona_email);
                    break;
            }*/
        }

        public SUPMessages SetDtoBeneficiary(DtoMaster dtoMaster)
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
                            ListTableMessage = setDto.UpdateBasicDataInSise2G(dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona, ListTableMessage); //UPDATE mpersona
                            //Se valida que el unico rol que posee es de SISE 2G, de ser asi se actualizan los datos basicos en la data de SISE 3G
                            if (DtRolesAPP.Rows.Count == 1)
                            {
                                individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.id_persona);
                                if (individualId3G > 0)
                                {
                                    ListTableMessage = setDto.UpdateBasicDataInSise3G(dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona, individualId3G, ListTableMessage); //UPDATE UP.PERSON || UP.COMPANY, UP.CO_COMPANY

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
                            individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.id_persona);
                            if (individualId3G > 0)
                            {
                                ListTableMessage = setDto.UpdateBasicDataInSise3G(dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona, individualId3G, ListTableMessage);

                            }
                        }
                        //Autor: Edward Rubiano; Fecha: 10/05/2016; Asunto: C11226; Descripcion: Validaciones para guardado unico de datos basicos
                        //_SUPMessages.INDIVIDUAL_ID = id_persona.ToString();
                        break;
                }
            }

            //Autor: Edward Rubiano; Fecha: 13/04/2016; Asunto: C11226; Descripcion: Se valida si la persona existe en 3g y de ser asi se ejecuta el proceso de guardado
            if (dtoMaster.GlobalOperation != "UpdateD")
            {
                setDto = new SetDto(User, App, IdRol);
                individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.id_persona);
                if (individualId3G > 0)
                {
                    ListTableMessage = setDto.UpdateAddressAndPhoneSise3G(dtoMaster.DtoBeneficiary.List_Mpersona_dir, dtoMaster.DtoBeneficiary.List_Mpersona_telef, dtoMaster.DtoBeneficiary.List_Mpersona_email, individualId3G, ListTableMessage);
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
                    if (dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona != null)
                    {
                        if (dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.id_persona == 0)
                        {
                            DGeneric = new DataGenericExecute(App);
                            id_persona = DGeneric.GetConsecutive("mpersona");
                        }
                        else
                        {
                            id_persona = dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.id_persona;
                        }
                    }

                    DGeneric = new DataGenericExecute(App);
                    cod_rol = DGeneric.GetConsecutive("mbeneficiario");
                    cod_rolref = cod_rol;
                }
                id_personaref = id_persona;
                //Insertamos o Actualizamos id_persona y cod_rol
                if (dtoMaster.DtoBeneficiary.dtoDataBeneficiary != null)
                {
                    if (dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona != null)
                        dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.Update("id_persona", id_persona);

                    if (dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mbeneficiario != null)
                        dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mbeneficiario.Update("id_persona", id_persona);

                    if (dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mbeneficiario != null)
                        dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mbeneficiario.Update("cod_beneficiario", cod_rol);

                    if (dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona_ciiu != null)
                        dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona_ciiu.Update("id_persona", id_persona);

                    if (dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona_requiere_sarlaft != null)
                        dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona_requiere_sarlaft.Update("id_persona", id_persona);
                }

                if (dtoMaster.DtoBeneficiary.dtoDataPerson != null)
                {
                    if (dtoMaster.DtoBeneficiary.dtoDataPerson.listMaseg_deporte != null)
                        dtoMaster.DtoBeneficiary.dtoDataPerson.listMaseg_deporte.Update(c => c.cod_aseg = cod_rol);

                    if (dtoMaster.DtoBeneficiary.dtoDataPerson.maseg_ficha_tec_financ != null)
                        dtoMaster.DtoBeneficiary.dtoDataPerson.maseg_ficha_tec_financ.Update("cod_aseg", cod_rol);

                    if (dtoMaster.DtoBeneficiary.dtoDataPerson.tipo_persona_aseg != null)
                        dtoMaster.DtoBeneficiary.dtoDataPerson.tipo_persona_aseg.Update("cod_aseg", cod_rol);
                }

                if (dtoMaster.DtoBeneficiary.List_Mpersona_dir != null)
                    dtoMaster.DtoBeneficiary.List_Mpersona_dir.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoBeneficiary.List_Mpersona_telef != null)
                    dtoMaster.DtoBeneficiary.List_Mpersona_telef.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoBeneficiary.List_Mpersona_email != null)
                    dtoMaster.DtoBeneficiary.List_Mpersona_email.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoBeneficiary.List_Logbook != null)
                    dtoMaster.DtoBeneficiary.List_Logbook.Update(c => { c.ID_PERSONA = id_persona; });

                if (dtoMaster.DtoBeneficiary.List_Mbenef_asoc_aseg != null)
                    dtoMaster.DtoBeneficiary.List_Mbenef_asoc_aseg.Update(c => { c.cod_beneficiario = cod_rol; });

                if (dtoMaster.DtoBeneficiary.List_Frm_sarlaft_accionistas_asoc != null)
                    dtoMaster.DtoBeneficiary.List_Frm_sarlaft_accionistas_asoc.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoBeneficiary.dtoRep != null)
                {
                    if (dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal != null)
                        dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal.Update("id_persona", id_persona);
                    if (dtoMaster.DtoBeneficiary.dtoRep.List_mpersona_rep_legal_dir != null)
                        dtoMaster.DtoBeneficiary.dtoRep.List_mpersona_rep_legal_dir.Update(c => { c.id_persona = id_persona; });
                }

                if (dtoMaster.DtoBeneficiary.dtoSarlaft != null)
                {
                    if (dtoMaster.DtoBeneficiary.dtoSarlaft.List_detalle != null)
                        dtoMaster.DtoBeneficiary.dtoSarlaft.List_detalle.Update(c => { c.id_persona = id_persona; });

                    if (dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_vinculos != null)
                        dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_vinculos.Update("id_persona", id_persona);

                    if (dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_aut_incrementos != null)
                        dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_aut_incrementos.Update("id_persona", id_persona);

                }

                if (dtoMaster.DtoBeneficiary.List_Referencias != null)
                    dtoMaster.DtoBeneficiary.List_Referencias.Update(c => { c.cod_rol = cod_rol; });

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

                        if (dtoMaster.DtoBeneficiary.dtoDataBeneficiary != null)
                        {
                            ListTableMessage = setDto.dtoDataBeneficiary(dtoMaster.DtoBeneficiary.dtoDataBeneficiary, ListTableMessage);
                            if (ListTableMessage.Count(c => c.NameTable == "Mbeneficiario" && c.Message != "True") > 0)
                            {
                                if (!dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mbeneficiario.State.Equals('V') && ListTableMessage.Count(c => c.NameTable == "Mbeneficiario" && c.Message != "True") > 0)
                                {
                                    cod_rolref = -1;
                                }
                                if (dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.State.Equals('V'))
                                {
                                    cod_rolref = -1;
                                    if (!dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.State.Equals('V'))
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
                            if (dtoMaster.DtoBeneficiary.dtoDataPerson != null)
                                ListTableMessage = setDto.dtoDataPerson(dtoMaster.DtoBeneficiary.dtoDataPerson, ListTableMessage);

                            if (dtoMaster.DtoBeneficiary.List_Mpersona_dir != null)
                                ListTableMessage = setDto.List_Mpersona_dir(dtoMaster.DtoBeneficiary.List_Mpersona_dir, ListTableMessage);

                            if (dtoMaster.DtoBeneficiary.List_Mpersona_telef != null)
                                ListTableMessage = setDto.List_Mpersona_telef(dtoMaster.DtoBeneficiary.List_Mpersona_telef, ListTableMessage);

                            if (dtoMaster.DtoBeneficiary.List_Mpersona_email != null)
                                ListTableMessage = setDto.List_Mpersona_email(dtoMaster.DtoBeneficiary.List_Mpersona_email, ListTableMessage);

                            if (dtoMaster.DtoBeneficiary.List_Logbook != null)
                                ListTableMessage = setDto.List_Logbook(dtoMaster.DtoBeneficiary.List_Logbook, ListTableMessage);

                            if (dtoMaster.DtoBeneficiary.List_Mbenef_asoc_aseg != null)
                                ListTableMessage = setDto.List_Mbenef_asoc_aseg(dtoMaster.DtoBeneficiary.List_Mbenef_asoc_aseg, ListTableMessage);

                            if (dtoMaster.DtoBeneficiary.List_Frm_sarlaft_accionistas_asoc != null)
                                ListTableMessage = setDto.List_Frm_sarlaft_accionistas_asoc(dtoMaster.DtoBeneficiary.List_Frm_sarlaft_accionistas_asoc, ListTableMessage);

                            if (dtoMaster.DtoBeneficiary.dtoSarlaft != null)
                                ListTableMessage = setDto.dtoSarlaft(dtoMaster.DtoBeneficiary.dtoSarlaft, ListTableMessage);

                            if (dtoMaster.DtoBeneficiary.dtoRep != null)
                                ListTableMessage = setDto.dtoRep(dtoMaster.DtoBeneficiary.dtoRep, ListTableMessage);

                            if (dtoMaster.DtoBeneficiary.List_Referencias != null)
                                ListTableMessage = setDto.List_Referencias(dtoMaster.DtoBeneficiary.List_Referencias, ListTableMessage);

                            if (
                                    (
                                        (dtoMaster.DtoBeneficiary.List_Mpersona_dir != null && dtoMaster.DtoBeneficiary.List_Mpersona_dir.Count > 0) ||
                                        (dtoMaster.DtoBeneficiary.List_Mpersona_telef != null && dtoMaster.DtoBeneficiary.List_Mpersona_telef.Count > 0) ||
                                        (dtoMaster.DtoBeneficiary.List_Mpersona_email != null && dtoMaster.DtoBeneficiary.List_Mpersona_email.Count > 0)
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
            SetDto setDto = new SetDto(User, App, IdRol);
            return ListTableMessage;

            /*if (ListTableMessage.Count(c => c.Message != "True") == 0)
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
                    char state;
                    DataSet ds = new DataSet();
                    string IsSave = string.Empty;
                    char state_individual = 'C';


                    //Insertamos o Actualizamos id_persona y cod_rol

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

                            #region Insert - Update
                            if (dtoMaster.DtoBeneficiary.dtoDataBeneficiary != null)
                            {
                                if (dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona != null)
                                {
                                    if (!dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.State.Equals('V'))
                                    {
                                        ds = new DataSet();

                                        state = statefor3g(App, dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.cod_tipo_persona == "F" ? "1" : "2",
                                            dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.cod_tipo_doc, dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.nro_doc != null ? dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.nro_doc : dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.nro_nit.Substring(0, 9),
                                                            id_persona);

                                        INDIVIDUAL _INDIVIDUAL = new INDIVIDUAL();
                                        _INDIVIDUAL.State = state;
                                        _INDIVIDUAL.INDIVIDUAL_ID = id_persona;
                                        _INDIVIDUAL.INDIVIDUAL_TYPE_CD = (dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.cod_tipo_persona == "F" ? 1 : 2);
                                        //campos a validar de donde se sacan
                                        if (_INDIVIDUAL.State == 'C')
                                        {
                                            _INDIVIDUAL.AT_DATA_ID = 1;
                                        }
                                        _INDIVIDUAL.AT_PAYMENT_ID = 1;
                                        _INDIVIDUAL.AT_AGENT_AGENCY_ID = 0;

                                        _INDIVIDUAL.ECONOMIC_ACTIVITY_CD = null;

                                        List<INDIVIDUAL> List_INDIVIDUAL = new List<INDIVIDUAL>();
                                        List_INDIVIDUAL.Add(_INDIVIDUAL);
                                        ListTableMessage = setDto.List_INDIVIDUAL(List_INDIVIDUAL, ListTableMessage);

                                        TableMessage result = ListTableMessage.Find(delegate(TableMessage bk) { return bk.NameTable == "INDIVIDUAL"; });
                                        IsSave = result.Message;

                                        if (IsSave == "True")
                                        {
                                            if (dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona_ciiu != null)
                                            {
                                                CO_PRV_INDIVIDUAL _CO_PRV_INDIVIDUAL = new CO_PRV_INDIVIDUAL();
                                                _CO_PRV_INDIVIDUAL.State = state;
                                                _CO_PRV_INDIVIDUAL.INDIVIDUAL_ID = id_persona;
                                                _CO_PRV_INDIVIDUAL.ECONOMIC_ACTIVITY_CD_NEW = dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona_ciiu.cod_ciiu_ppal_nuevo;
                                                _CO_PRV_INDIVIDUAL.SECOND_ECONOMIC_ACTIVITY_CD_NEW = dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona_ciiu.cod_ciiu_scndria_nuevo;

                                                List<CO_PRV_INDIVIDUAL> List_CO_PRV_INDIVIDUAL = new List<CO_PRV_INDIVIDUAL>();
                                                List_CO_PRV_INDIVIDUAL.Add(_CO_PRV_INDIVIDUAL);
                                                ListTableMessage = setDto.List_CO_PRV_INDIVIDUAL(List_CO_PRV_INDIVIDUAL, ListTableMessage);
                                            }

                                            if (dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.cod_tipo_persona == "F")
                                            {
                                                _INDIVIDUAL.INDIVIDUAL_TYPE_CD = 1;
                                                PERSON _PERSON = new PERSON();
                                                _PERSON.State = state;
                                                _PERSON.INDIVIDUAL_ID = id_persona;
                                                _PERSON.SURNAME = dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.txt_apellido1;
                                                _PERSON.NAME = dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.txt_nombre;
                                                _PERSON.GENDER = dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.txt_sexo;
                                                _PERSON.ID_CARD_TYPE_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.cod_tipo_doc));
                                                _PERSON.ID_CARD_NO = dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.nro_doc;
                                                _PERSON.MARITAL_STATUS_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.cod_est_civil));
                                                _PERSON.BIRTH_DATE = dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.fec_nac;
                                                _PERSON.CHILDREN = "0";
                                                _PERSON.MOTHER_LAST_NAME = dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.txt_apellido2;

                                                List<PERSON> List_PERSON = new List<PERSON>();
                                                List_PERSON.Add(_PERSON);
                                                ListTableMessage = setDto.List_PERSON(List_PERSON, ListTableMessage);

                                            }
                                            else if (dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.cod_tipo_persona == "J")
                                            {
                                                _INDIVIDUAL.INDIVIDUAL_TYPE_CD = 2;
                                                COMPANY _COMPANY = new COMPANY();
                                                _COMPANY.State = state;
                                                _COMPANY.INDIVIDUAL_ID = dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.id_persona;
                                                _COMPANY.TRADE_NAME = dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.txt_apellido1 + " " + dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.txt_apellido2 + " " + dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.txt_nombre;
                                                _COMPANY.TRIBUTARY_ID_TYPE_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.cod_tipo_doc));
                                                _COMPANY.TRIBUTARY_ID_NO = dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.nro_nit.Substring(0, 9);
                                                _COMPANY.COUNTRY_CD = 1;
                                                _COMPANY.COMPANY_TYPE_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.COMPANY_TYPE_CD));

                                                List<COMPANY> List_COMPANY = new List<COMPANY>();
                                                List_COMPANY.Add(_COMPANY);
                                                ListTableMessage = setDto.List_COMPANY(List_COMPANY, ListTableMessage);

                                                result = ListTableMessage.Find(delegate(TableMessage bk) { return bk.NameTable == "COMPANY"; });
                                                IsSave = result.Message;

                                                if (IsSave == "True")
                                                {
                                                    CO_COMPANY _CO_COMPANY = new CO_COMPANY();
                                                    _CO_COMPANY.State = state;
                                                    _CO_COMPANY.INDIVIDUAL_ID = dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.id_persona;
                                                    _CO_COMPANY.VERIFY_DIGIT = dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.VERIFY_DIGIT;
                                                    _CO_COMPANY.ASSOCIATION_TYPE_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona.ASSOCIATION_TYPE_CD));
                                                    _CO_COMPANY.ENTITY_OFFICIAL_CD = "0";

                                                    List<CO_COMPANY> List_CO_COMPANY = new List<CO_COMPANY>();
                                                    List_CO_COMPANY.Add(_CO_COMPANY);
                                                    ListTableMessage = setDto.List_CO_COMPANY(List_CO_COMPANY, ListTableMessage);
                                                }
                                            }
                                        }
                                    }
                                }

                                TableMessage resul = ListTableMessage.Find(delegate(TableMessage bk) { return bk.NameTable == "INDIVIDUAL"; });
                                if (resul != null)
                                {
                                    IsSave = resul.Message;

                                    if (dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona_requiere_sarlaft != null && IsSave == "True")
                                    {
                                        if (!dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona_requiere_sarlaft.State.Equals('V'))
                                        {
                                            INDIVIDUAL_SARLAFT_EXONERATION _INDIVIDUAL_SARLAFT_EXONERATION = new INDIVIDUAL_SARLAFT_EXONERATION();
                                            _INDIVIDUAL_SARLAFT_EXONERATION.State = dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona_requiere_sarlaft.State;
                                            _INDIVIDUAL_SARLAFT_EXONERATION.INDIVIDUAL_ID = id_persona;
                                            _INDIVIDUAL_SARLAFT_EXONERATION.USER_ID = 1;
                                            _INDIVIDUAL_SARLAFT_EXONERATION.EXONERATION_TYPE_CD = dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona_requiere_sarlaft.cod_motivo_exonera;
                                            if (dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona_requiere_sarlaft.sn_exonerado == -1)
                                                _INDIVIDUAL_SARLAFT_EXONERATION.IS_EXONERATED = true;
                                            else
                                                _INDIVIDUAL_SARLAFT_EXONERATION.IS_EXONERATED = false;

                                            _INDIVIDUAL_SARLAFT_EXONERATION.REGISTRATION_DATE = dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona_requiere_sarlaft.fec_modifica;

                                            string RoleCd = !string.IsNullOrEmpty(dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona_requiere_sarlaft.txt_origen) ? dtoMaster.DtoBeneficiary.dtoDataBeneficiary.mpersona_requiere_sarlaft.txt_origen.Substring(0, 1) : null;
                                            if (RoleCd == "B")
                                                _INDIVIDUAL_SARLAFT_EXONERATION.ROLE_CD = 3;
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

                            #endregion//fin dtoMaster.DtoBeneficiary.dtoDataBeneficiary != null

                            #region Complemento

                            ds = new DataSet();
                            ds = beneficiaryidperson(App, Convert.ToString(id_persona));

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                state_individual = Convert.ToChar(ds.Tables[0].Rows[0][0]);
                                if (state_individual.Equals('U'))
                                    IsSave = "True";
                            }

                            //Validacion Almacenamiento Direccion
                            if (dtoMaster.DtoBeneficiary.List_Mpersona_dir != null && IsSave == "True")
                            {
                                List<ADDRESS> List_ADDRESS = new List<ADDRESS>();

                                foreach (Mpersona_dir dir in dtoMaster.DtoBeneficiary.List_Mpersona_dir)
                                {
                                    ADDRESS _ADDRESS = new ADDRESS();
                                    if (dir.txt_direccion != "")
                                    {
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

                                        if (_ADDRESS.ADDRESS_TYPE_CD != 99)
                                        {
                                            List_ADDRESS.Add(_ADDRESS);
                                        }
                                    }
                                }

                                ListTableMessage = setDto.List_ADDRESS(List_ADDRESS, ListTableMessage);
                            }

                            if (dtoMaster.DtoBeneficiary.List_Mpersona_telef != null && IsSave == "True")
                            {
                                List<PHONE> List_PHONE = new List<PHONE>();

                                foreach (Mpersona_telef phone in dtoMaster.DtoBeneficiary.List_Mpersona_telef)
                                {
                                    PHONE _PHONE = new PHONE();
                                    if (phone.txt_telefono != "")
                                    {
                                        _PHONE.State = phone.State_3g;

                                        _PHONE.INDIVIDUAL_ID = id_persona;
                                        _PHONE.DATA_ID = phone.DATA_ID;
                                        _PHONE.PHONE_TYPE_CD = Convert.ToInt32(phone.cod_tipo_telef);
                                        _PHONE.PHONE_NUMBER = Convert.ToInt64(FullServicesProviderHelper.ToNullInt(phone.txt_telefono));
                                        _PHONE.EXTENSION = phone.EXTENSION;
                                        _PHONE.SCHEDULE_AVAILABILITY = phone.SCHEDULE_AVAILABILITY;

                                        List_PHONE.Add(_PHONE);
                                    }
                                }

                                ListTableMessage = setDto.List_PHONE(List_PHONE, ListTableMessage);
                            }

                            if (dtoMaster.DtoBeneficiary.List_Mpersona_email != null && IsSave == "True")
                            {
                                List<EMAIL> List_EMAIL = new List<EMAIL>();

                                foreach (Mpersona_email mail in dtoMaster.DtoBeneficiary.List_Mpersona_email)
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

                            if (dtoMaster.DtoBeneficiary.dtoSarlaft != null)
                            {
                                if (dtoMaster.DtoBeneficiary.dtoSarlaft.List_detalle != null && IsSave == "True")
                                {
                                    List<INDIVIDUAL_SARLAFT> List_INDIVIDUAL_SARLAFT = new List<INDIVIDUAL_SARLAFT>();

                                    foreach (Frm_sarlaft_detalle detalle in dtoMaster.DtoBeneficiary.dtoSarlaft.List_detalle)
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

                                        if (dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_detalle_entrevista != null)
                                        {
                                            individual.INTERVIEW_DATE = dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_detalle_entrevista.fec_entrevista;
                                            individual.INTERVIEWER_NAME = dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_detalle_entrevista.txt_usua_entrev;
                                            individual.INTERVIEW_PLACE = dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_detalle_entrevista.txt_lugar_entrev;
                                            individual.INTERVIEW_RESULT_CD = (dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_detalle_entrevista.txt_resul_entrev == "RECHAZADO" ? 2 : 1);
                                        }

                                        if (dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_info_financiera != null)
                                        {
                                            individual.INTERNATIONAL_OPERATIONS = dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_info_financiera.id_formulario != 0 ? true : false;
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

                                        if (dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_info_financiera != null)
                                        {
                                            individual.ECONOMIC_ACTIVITY_CD = Convert.ToInt32(dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_info_financiera.cod_act_principal);
                                            individual.SECOND_ECONOMIC_ACTIVITY_CD = Convert.ToInt32(dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_info_financiera.cod_act_secundaria == -1 ? 0 : dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_info_financiera.cod_act_secundaria);

                                        }
                                        individual.PENDING_EVENT = dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_aut_incrementos != null ? true : false;
                                        individual.State = detalle.State;

                                        List_INDIVIDUAL_SARLAFT.Add(individual);

                                    }

                                    ListTableMessage = setDto.List_INDIVIDUAL_SARLAFT(List_INDIVIDUAL_SARLAFT, ListTableMessage);

                                }

                                if (dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_vinculos != null && IsSave == "True")
                                {
                                    List<INDIVIDUAL_LINK> List_INDIVIDUAL_LINK = new List<INDIVIDUAL_LINK>();
                                    INDIVIDUAL_LINK _INDIVIDUAL_LINK = new INDIVIDUAL_LINK();
                                    if (dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_vinculos.tomador_asegurado != 0)
                                    {
                                        _INDIVIDUAL_LINK = new INDIVIDUAL_LINK();
                                        _INDIVIDUAL_LINK.INDIVIDUAL_ID = id_persona;
                                        _INDIVIDUAL_LINK.LINK_TYPE_CD = Convert.ToInt32(dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_vinculos.tomador_asegurado);
                                        _INDIVIDUAL_LINK.RELATIONSHIP_SARLAFT_CD = 1;
                                        _INDIVIDUAL_LINK.DESCRIPTION = dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_vinculos.txt_desc_TA;
                                        _INDIVIDUAL_LINK.State = dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_vinculos.State;
                                        List_INDIVIDUAL_LINK.Add(_INDIVIDUAL_LINK);
                                    }
                                    if (dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_vinculos.tomador_benef != 0)
                                    {
                                        _INDIVIDUAL_LINK = new INDIVIDUAL_LINK();
                                        _INDIVIDUAL_LINK.INDIVIDUAL_ID = id_persona;
                                        _INDIVIDUAL_LINK.LINK_TYPE_CD = Convert.ToInt32(dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_vinculos.tomador_benef);
                                        _INDIVIDUAL_LINK.RELATIONSHIP_SARLAFT_CD = 2;
                                        _INDIVIDUAL_LINK.DESCRIPTION = dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_vinculos.txt_desc_TB;
                                        _INDIVIDUAL_LINK.State = dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_vinculos.State;
                                        List_INDIVIDUAL_LINK.Add(_INDIVIDUAL_LINK);
                                    }
                                    if (dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_vinculos.asegurado_benef != 0)
                                    {
                                        _INDIVIDUAL_LINK = new INDIVIDUAL_LINK();
                                        _INDIVIDUAL_LINK.INDIVIDUAL_ID = id_persona;
                                        _INDIVIDUAL_LINK.LINK_TYPE_CD = Convert.ToInt32(dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_vinculos.asegurado_benef);
                                        _INDIVIDUAL_LINK.RELATIONSHIP_SARLAFT_CD = 3;
                                        _INDIVIDUAL_LINK.DESCRIPTION = dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_vinculos.txt_desc_AB;
                                        _INDIVIDUAL_LINK.State = dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_vinculos.State;
                                        List_INDIVIDUAL_LINK.Add(_INDIVIDUAL_LINK);
                                    }

                                    ListTableMessage = setDto.List_INDIVIDUAL_LINK(List_INDIVIDUAL_LINK, ListTableMessage);
                                }
                                if (dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_info_financiera != null && IsSave == "True")
                                {
                                    FINANCIAL_SARLAFT _FINANCIAL_SARLAFT = new FINANCIAL_SARLAFT();

                                    _FINANCIAL_SARLAFT.SARLAFT_ID = dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_info_financiera.id_formulario;
                                    _FINANCIAL_SARLAFT.INCOME_AMT = dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_info_financiera.imp_ingresos;
                                    _FINANCIAL_SARLAFT.EXPENSE_AMT = dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_info_financiera.imp_egresos;
                                    _FINANCIAL_SARLAFT.EXTRA_INCOME_AMT = dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_info_financiera.imp_otros_ingr;
                                    _FINANCIAL_SARLAFT.ASSETS_AMT = dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_info_financiera.imp_activos;
                                    _FINANCIAL_SARLAFT.LIABILITIES_AMT = dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_info_financiera.imp_pasivos;
                                    if (dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_detalle_entrevista != null)
                                    {
                                        _FINANCIAL_SARLAFT.DESCRIPTION = dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_detalle_entrevista.txt_obser_entrev;
                                    }
                                    _FINANCIAL_SARLAFT.State = dtoMaster.DtoBeneficiary.dtoSarlaft.frm_sarlaft_detalle_entrevista.State;

                                    List<FINANCIAL_SARLAFT> List_FINANCIAL_SARLAFT = new List<FINANCIAL_SARLAFT>();
                                    List_FINANCIAL_SARLAFT.Add(_FINANCIAL_SARLAFT);
                                    ListTableMessage = setDto.List_FINANCIAL_SARLAFT(List_FINANCIAL_SARLAFT, ListTableMessage);

                                }

                                if (dtoMaster.DtoBeneficiary.dtoSarlaft.List_oper_internacionales != null && IsSave == "True")
                                {
                                    List<SARLAFT_OPERATION> List_SARLAFT_OPERATION = new List<SARLAFT_OPERATION>();

                                    foreach (Frm_sarlaft_oper_internac internat in dtoMaster.DtoBeneficiary.dtoSarlaft.List_oper_internacionales)
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
                                        operation.State = internat.State;

                                        List_SARLAFT_OPERATION.Add(operation);

                                    }

                                    ListTableMessage = setDto.List_SARLAFT_OPERATION(List_SARLAFT_OPERATION, ListTableMessage);
                                }

                                if (dtoMaster.DtoBeneficiary.dtoRep != null)
                                {
                                    if (dtoMaster.DtoBeneficiary.dtoRep.List_mpersona_rep_legal_dir != null)
                                    {
                                        if (dtoMaster.DtoBeneficiary.dtoRep.List_mpersona_rep_legal_dir.Count != 0 && dtoMaster.DtoBeneficiary.dtoRep.List_mpersona_rep_legal_dir[0].cod_tipo_dir != 99 && IsSave == "True")
                                        {
                                            INDIVIDUAL_LEGAL_REPRESENT _INDIVIDUAL_LEGAL_REPRESENT = new INDIVIDUAL_LEGAL_REPRESENT();
                                            _INDIVIDUAL_LEGAL_REPRESENT.State = dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal.State;
                                            _INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_ID = dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal.id_persona;
                                            _INDIVIDUAL_LEGAL_REPRESENT.LEGAL_REPRESENTATIVE_NAME = dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal.txt_nombre;
                                            _INDIVIDUAL_LEGAL_REPRESENT.EXPEDITION_DATE = dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal.fec_expedicion_doc;
                                            _INDIVIDUAL_LEGAL_REPRESENT.EXPEDITION_PLACE = dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal.txt_lugar_expedicion == null ? "" : dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal.txt_lugar_expedicion;
                                            _INDIVIDUAL_LEGAL_REPRESENT.BIRTH_DATE = dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal.fec_nacimiento;
                                            _INDIVIDUAL_LEGAL_REPRESENT.BIRTH_PLACE = dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal.txt_lugar_nacimi;
                                            _INDIVIDUAL_LEGAL_REPRESENT.NATIONALITY = dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal.txt_nacionalidad;
                                            _INDIVIDUAL_LEGAL_REPRESENT.CITY = dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal.txt_ciudad;
                                            _INDIVIDUAL_LEGAL_REPRESENT.PHONE = dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal.txt_telefono;
                                            _INDIVIDUAL_LEGAL_REPRESENT.JOB_TITLE = dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal.txt_cargo;
                                            _INDIVIDUAL_LEGAL_REPRESENT.CELL_PHONE = dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal.txt_celular;
                                            _INDIVIDUAL_LEGAL_REPRESENT.EMAIL = dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal.txt_email == null ? "" : dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal.txt_email;
                                            _INDIVIDUAL_LEGAL_REPRESENT.ID_CARD_NO = dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal.nro_doc_rep_legal;
                                            _INDIVIDUAL_LEGAL_REPRESENT.AUTHORIZATION_AMT = dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal.vr_facultades;
                                            _INDIVIDUAL_LEGAL_REPRESENT.DESCRIPTION = dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal.txt_facultades;
                                            _INDIVIDUAL_LEGAL_REPRESENT.CURRENCY_CD = dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal.cod_unidad == "-1" ? null : dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal.cod_unidad;
                                            _INDIVIDUAL_LEGAL_REPRESENT.ID_CARD_TYPE_CD = Convert.ToInt32(dtoMaster.DtoBeneficiary.dtoRep.mpersona_rep_legal.cod_tipo_doc);


                                            _INDIVIDUAL_LEGAL_REPRESENT.ADDRESS = dtoMaster.DtoBeneficiary.dtoRep.List_mpersona_rep_legal_dir[0].txt_direccion;
                                            _INDIVIDUAL_LEGAL_REPRESENT.COUNTRY_CD = Convert.ToInt32(dtoMaster.DtoBeneficiary.dtoRep.List_mpersona_rep_legal_dir[0].cod_pais);
                                            _INDIVIDUAL_LEGAL_REPRESENT.STATE_CD = Convert.ToInt32(dtoMaster.DtoBeneficiary.dtoRep.List_mpersona_rep_legal_dir[0].cod_dpto);
                                            _INDIVIDUAL_LEGAL_REPRESENT.CITY_CD = Convert.ToInt32(dtoMaster.DtoBeneficiary.dtoRep.List_mpersona_rep_legal_dir[0].cod_municipio);

                                            List<INDIVIDUAL_LEGAL_REPRESENT> List_INDIVIDUAL_LEGAL_REPRESENT = new List<INDIVIDUAL_LEGAL_REPRESENT>();

                                            List_INDIVIDUAL_LEGAL_REPRESENT.Add(_INDIVIDUAL_LEGAL_REPRESENT);

                                            ListTableMessage = setDto.List_INDIVIDUAL_LEGAL_REPRESENT(List_INDIVIDUAL_LEGAL_REPRESENT, ListTableMessage);
                                        }
                                    }
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
            parameter.Value = "UU.UNIQUE_USERS";
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
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) == 0)
                    state = 'C';
                else
                    state = 'U';
            }

            return state;

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

        private DataSet beneficiaryidperson(string App, string idpersona)
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
        }*/
        }
        #endregion
    }
}
