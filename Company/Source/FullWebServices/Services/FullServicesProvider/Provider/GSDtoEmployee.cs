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
    public class GSDtoEmployee
    {
        #region constructores

        public GSDtoEmployee()
        {

        }

        public GSDtoEmployee(string user, int idApp, int idRol)
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

        public DtoEmployee GetDtoEmployee(int id_rol, string cod_rol, string entity)
        {
            DtoEmployee DtoEmployee = new DtoEmployee();

            //Carga el Dto solo con una entidad especifica
            if (entity != "")
            {
                DtoEmployee = GetEntity(id_rol, cod_rol, entity);
            }
            else
            {
                //Carga el Dto Totalmente nuevo
                if (cod_rol == "new")
                    DtoEmployee = getDto.DtoEmployee();
                //Carga el Dto con toda la informacion del Rol buscado
                else
                {
                    DtoEmployee = GetDto(id_rol, cod_rol);
                }
            }
            return DtoEmployee;
        }

        private DtoEmployee GetDto(int id_rol, string cod_rol)
        {
            DtoEmployee DtoEmployee = new DtoEmployee();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataTable DtRolesAPP = DGeneric.GetListRolApp(IdRol);
            string App = string.Empty;

            foreach (DataRow dr in DtRolesAPP.Rows)
            {
                App = dr[0].ToString();
                switch (App)
                {
                    case "SISE2G":
                        GetDtoSISE2G(id_rol, cod_rol, ref DtoEmployee);
                        break;

                    case "SISE3G":
                        GetDtoSISE3G(id_rol, cod_rol, ref DtoEmployee);
                        break;

                }
            }

            return DtoEmployee;
        }

        private void GetDtoSISE2G(int id_rol, string cod_rol, ref DtoEmployee DtoEmployee)
        {
            int id_persona = getDto.id_persona(id_rol, cod_rol);
            DtoEmployee.dtoDataEmployee = getDto.dtoDataEmployee(Convert.ToInt32(cod_rol));
            DtoEmployee.dtoDataPerson = getDto.dtoDataPerson(id_persona);
            DtoEmployee.List_Mpersona_dir = getDto.List_Mpersona_dir(id_persona);
            DtoEmployee.List_Mpersona_telef = getDto.List_Mpersona_telef(id_persona);
            DtoEmployee.List_Mpersona_email = getDto.List_Mpersona_email(id_persona);
            DtoEmployee.List_Logbook = getDto.List_Logbook(id_persona, id_rol);
            DtoEmployee.dtoSarlaft = getDto.dtoSarlaft(id_persona);
            DtoEmployee.List_Mcesionario = getDto.List_Mcesionario(id_persona);
            DtoEmployee.List_Mpersona_cuentas_bancarias = getDto.List_Mpersona_cuentas_bancarias(id_persona);
        }

        private void GetDtoSISE3G(int id_rol, string cod_rol, ref DtoEmployee DtoEmployee)
        {
            //logica de consulta para anexar campos de 3G
        }

        private DtoEmployee GetEntity(int id_rol, string cod_rol, string entity)
        {
            DtoEmployee DtoEmployee = new DtoEmployee();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataTable DtRolesAPP = DGeneric.GetListRolApp(IdRol);
            string App = string.Empty;

            foreach (DataRow dr in DtRolesAPP.Rows)
            {
                App = dr[0].ToString();
                switch (App)
                {
                    case "SISE2G":
                        GetEntitySISE2G(id_rol, cod_rol, entity, ref DtoEmployee);
                        break;

                    case "SISE3G":
                        GetEntitySISE3G(id_rol, cod_rol, entity, ref DtoEmployee);
                        break;

                }
            }

            return DtoEmployee;
        }

        private void GetEntitySISE2G(int id_rol, string cod_rol, string entity, ref DtoEmployee DtoEmployee)
        {
            int id_persona = getDto.id_persona(id_rol, cod_rol);
            switch (entity)
            {
                case "dtoDataEmployee":
                    DtoEmployee.dtoDataEmployee = getDto.dtoDataEmployee(Convert.ToInt32(cod_rol));
                    break;
                case "List_Mpersona_dir":
                    DtoEmployee.List_Mpersona_dir = getDto.List_Mpersona_dir(id_persona);
                    break;
                case "List_Mpersona_telef":
                    DtoEmployee.List_Mpersona_telef = getDto.List_Mpersona_telef(id_persona);
                    break;
                case "List_Mpersona_email":
                    DtoEmployee.List_Mpersona_email = getDto.List_Mpersona_email(id_persona);
                    break;
                case "List_Logbook":
                    DtoEmployee.List_Logbook = getDto.List_Logbook(id_persona, id_rol);
                    break;
                case "List_Mcesionario":
                    DtoEmployee.List_Mcesionario = getDto.List_Mcesionario(id_persona);
                    break;
                case "List_Mpersona_cuentas_bancarias":
                    DtoEmployee.List_Mpersona_cuentas_bancarias = getDto.List_Mpersona_cuentas_bancarias(id_persona);
                    break;
                case "dtoDataPerson":
                    DtoEmployee.dtoDataPerson = getDto.dtoDataPerson(id_persona);
                    break;
                case "dtoSarlaft":
                    DtoEmployee.dtoSarlaft = getDto.dtoSarlaft(id_persona);
                    break;
                case "dtoSarlaftForm":
                    DtoEmployee.dtoSarlaft = getDto.dtoSarlaftForm(Convert.ToInt32(cod_rol));
                    break;
            }
        }

        private void GetEntitySISE3G(int id_rol, string cod_rol, string entity, ref DtoEmployee DtoEmployee)
        {
            //aqui codigo de otra base de datos llenando campos extras

        }

        public SUPMessages SetDtoEmployee(DtoMaster dtoMaster)
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
                            ListTableMessage = setDto.UpdateBasicDataInSise2G(dtoMaster.DtoEmployee.dtoDataEmployee.mpersona, ListTableMessage); //UPDATE mpersona
                            //Se valida que el unico rol que posee es de SISE 2G, de ser asi se actualizan los datos basicos en la data de SISE 3G
                            if (DtRolesAPP.Rows.Count == 1)
                            {
                                individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoEmployee.dtoDataEmployee.mpersona.id_persona);
                                if (individualId3G > 0)
                                {
                                    ListTableMessage = setDto.UpdateBasicDataInSise3G(dtoMaster.DtoEmployee.dtoDataEmployee.mpersona, individualId3G, ListTableMessage); //UPDATE UP.PERSON || UP.COMPANY, UP.CO_COMPANY

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
                            individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoEmployee.dtoDataEmployee.mpersona.id_persona);
                            if (individualId3G > 0)
                            {
                                ListTableMessage = setDto.UpdateBasicDataInSise3G(dtoMaster.DtoEmployee.dtoDataEmployee.mpersona, individualId3G, ListTableMessage);

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
                individualId3G = getDto.ValidateExistPersonIn3G(dtoMaster.DtoEmployee.dtoDataEmployee.mpersona.id_persona);
                if (individualId3G > 0 && DtRolesAPP.Rows.Count < 2)
                {
                    ListTableMessage = setDto.UpdateAddressAndPhoneSise3G(dtoMaster.DtoEmployee.List_Mpersona_dir, dtoMaster.DtoEmployee.List_Mpersona_telef, dtoMaster.DtoEmployee.List_Mpersona_email, individualId3G, ListTableMessage);
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

                if (dtoMaster.DtoEmployee.dtoDataEmployee != null)
                {
                    if (dtoMaster.DtoEmployee.dtoDataEmployee.mpersona != null)
                    {
                        if (dtoMaster.DtoEmployee.dtoDataEmployee.mpersona.State.Equals('C'))
                        {
                            DGeneric = new DataGenericExecute(App);
                            id_persona = DGeneric.GetConsecutive("mpersona");
                            cod_rol = Convert.ToInt32(dtoMaster.DtoEmployee.dtoDataEmployee.mempleado.cod_empleado);
                        }
                        else
                        {
                            id_persona = dtoMaster.DtoEmployee.dtoDataEmployee.mpersona.id_persona;
                            cod_rol = Convert.ToInt32(dtoMaster.cod_Rol);
                            //cod_rol = Convert.ToInt32(dtoMaster.DtoEmployee.dtoDataEmployee.mempleado.cod_empleado);
                        }
                    }
                    else
                    {
                        cod_rol = Convert.ToInt32(dtoMaster.cod_Rol);
                        cod_rolref = cod_rol;
                        id_persona = getDto.id_persona(IdRol, cod_rol.ToString());
                    }
                }
                else
                {
                    cod_rol = Convert.ToInt32(dtoMaster.cod_Rol);
                    cod_rolref = cod_rol;
                    id_persona = getDto.id_persona(IdRol, cod_rol.ToString());
                }


                cod_rolref = cod_rol;
                id_personaref = id_persona;

                //Insertamos o Actualizamos id_persona y cod_rol
                if (dtoMaster.DtoEmployee.dtoDataEmployee != null)
                {
                    if (dtoMaster.DtoEmployee.dtoDataEmployee.mpersona != null)
                        dtoMaster.DtoEmployee.dtoDataEmployee.mpersona.Update("id_persona", id_persona);

                    if (dtoMaster.DtoEmployee.dtoDataEmployee.mpersona_ciiu != null)
                        dtoMaster.DtoEmployee.dtoDataEmployee.mpersona_ciiu.Update("id_persona", id_persona);

                    if (dtoMaster.DtoEmployee.dtoDataEmployee.mpersona_requiere_sarlaft != null)
                        dtoMaster.DtoEmployee.dtoDataEmployee.mpersona_requiere_sarlaft.Update("id_persona", id_persona);

                    if (dtoMaster.DtoEmployee.dtoDataEmployee.mempleado != null)
                        dtoMaster.DtoEmployee.dtoDataEmployee.mempleado.Update("id_persona", id_persona);

                    if (dtoMaster.DtoEmployee.dtoDataEmployee.mempleado != null)
                        dtoMaster.DtoEmployee.dtoDataEmployee.mempleado.Update("cod_empleado", cod_rol);
                }

                if (dtoMaster.DtoEmployee.dtoDataPerson != null)
                {
                    if (dtoMaster.DtoEmployee.dtoDataPerson.listMaseg_deporte != null)
                        dtoMaster.DtoEmployee.dtoDataPerson.listMaseg_deporte.Update(c => c.cod_aseg = cod_rol);

                    if (dtoMaster.DtoEmployee.dtoDataPerson.maseg_ficha_tec_financ != null)
                        dtoMaster.DtoEmployee.dtoDataPerson.maseg_ficha_tec_financ.Update("cod_aseg", cod_rol);

                    if (dtoMaster.DtoEmployee.dtoDataPerson.tipo_persona_aseg != null)
                        dtoMaster.DtoEmployee.dtoDataPerson.tipo_persona_aseg.Update("cod_aseg", cod_rol);
                }

                if (dtoMaster.DtoEmployee.dtoSarlaft != null)
                {
                    if (dtoMaster.DtoEmployee.dtoSarlaft.List_detalle != null)
                        dtoMaster.DtoEmployee.dtoSarlaft.List_detalle.Update(c => { c.id_persona = id_persona; });

                    if (dtoMaster.DtoEmployee.dtoSarlaft.frm_sarlaft_vinculos != null)
                        dtoMaster.DtoEmployee.dtoSarlaft.frm_sarlaft_vinculos.Update("id_persona", id_persona);

                    if (dtoMaster.DtoEmployee.dtoSarlaft.frm_sarlaft_aut_incrementos != null)
                        dtoMaster.DtoEmployee.dtoSarlaft.frm_sarlaft_aut_incrementos.Update("id_persona", id_persona);

                }


                if (dtoMaster.DtoEmployee.List_Mpersona_dir != null)
                    dtoMaster.DtoEmployee.List_Mpersona_dir.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoEmployee.List_Mpersona_telef != null)
                    dtoMaster.DtoEmployee.List_Mpersona_telef.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoEmployee.List_Mpersona_email != null)
                    dtoMaster.DtoEmployee.List_Mpersona_email.Update(c => { c.id_persona = id_persona; });

                if (dtoMaster.DtoEmployee.List_Logbook != null)
                    dtoMaster.DtoEmployee.List_Logbook.Update(c => { c.ID_PERSONA = id_persona; });

                if (dtoMaster.DtoEmployee.List_Mpersona_cuentas_bancarias != null)
                    dtoMaster.DtoEmployee.List_Mpersona_cuentas_bancarias.Update(c => { c.id_persona = id_persona; });



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

                        if (dtoMaster.DtoEmployee.dtoDataEmployee != null)
                        {
                            ListTableMessage = setDto.dtoDataEmployee(dtoMaster.DtoEmployee.dtoDataEmployee, ListTableMessage);
                            if (ListTableMessage.Count(c => c.NameTable == "Mempleado" && c.Message != "True") > 0)
                                if (dtoMaster.DtoEmployee.dtoDataEmployee.mpersona.State.Equals('V') || !dtoMaster.DtoEmployee.dtoDataEmployee.mempleado.State.Equals('V'))
                                    cod_rolref = -1;
                        }

                        if (cod_rolref != -1)
                        {
                            if (dtoMaster.DtoEmployee.dtoDataPerson != null)
                                ListTableMessage = setDto.dtoDataPerson(dtoMaster.DtoEmployee.dtoDataPerson, ListTableMessage);

                            if (dtoMaster.DtoEmployee.List_Mpersona_dir != null)
                                ListTableMessage = setDto.List_Mpersona_dir(dtoMaster.DtoEmployee.List_Mpersona_dir, ListTableMessage);

                            if (dtoMaster.DtoEmployee.List_Mpersona_telef != null)
                                ListTableMessage = setDto.List_Mpersona_telef(dtoMaster.DtoEmployee.List_Mpersona_telef, ListTableMessage);

                            if (dtoMaster.DtoEmployee.List_Mpersona_email != null)
                                ListTableMessage = setDto.List_Mpersona_email(dtoMaster.DtoEmployee.List_Mpersona_email, ListTableMessage);

                            if (dtoMaster.DtoEmployee.List_Logbook != null)
                                ListTableMessage = setDto.List_Logbook(dtoMaster.DtoEmployee.List_Logbook, ListTableMessage);

                            if (dtoMaster.DtoEmployee.dtoSarlaft != null)
                                ListTableMessage = setDto.dtoSarlaft(dtoMaster.DtoEmployee.dtoSarlaft, ListTableMessage);


                            if (dtoMaster.DtoEmployee.List_Mpersona_cuentas_bancarias != null)
                                ListTableMessage = setDto.List_Mpersona_cuentas_bancarias(dtoMaster.DtoEmployee.List_Mpersona_cuentas_bancarias, ListTableMessage);

                            if (
                                    (
                                        (dtoMaster.DtoEmployee.List_Mpersona_dir != null && dtoMaster.DtoEmployee.List_Mpersona_dir.Count > 0) ||
                                        (dtoMaster.DtoEmployee.List_Mpersona_telef != null && dtoMaster.DtoEmployee.List_Mpersona_telef.Count > 0) ||
                                        (dtoMaster.DtoEmployee.List_Mpersona_email != null && dtoMaster.DtoEmployee.List_Mpersona_email.Count > 0)
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
