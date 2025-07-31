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
using System.Configuration;
//using Sybase.Data.AseClient;

namespace Sistran.Co.Previsora.Application.FullServicesProvider.Provider
{
    public class SetDto
    {

        #region Constructor

        public SetDto()
        {

        }

        public SetDto(string user, string App, int idRol)
        {
            DataGenericExecute dge = new DataGenericExecute(App);
            PrefixConn = App;
            User = user;
            IdRol = idRol;
        }

        public SetDto(string user, string App, int idRol, AseCommand command)
        {
            DataGenericExecute dge = new DataGenericExecute(App);
            PrefixConn = App;
            User = user;
            IdRol = idRol;
            Command = command;
        }

        #endregion

        #region Propiedades

        private string PrefixConn { get; set; }
        private string User { get; set; }
        private int IdApp { get; set; }
        private int IdRol { get; set; }
        private AseCommand Command { get; set; }
        DatatableToList Dtl = new DatatableToList();
        DataGenericExecute DGeneric;
        bool validateDataId = false;

        #endregion

        #region Procedimientos Independientes

        public int id_persona(int id_rol, string cod_rol)
        {
            List<Parameters> ListParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.NameTable = "";
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "id_rol";
            parameter.Value = id_rol.ToString();
            ListParameter.Add(parameter);
            parameter = new Parameters();
            parameter.NameTable = "";
            parameter.ParameterType = "AseDbType.VarChar";
            parameter.Parameter = "cod_rol";
            parameter.Value = cod_rol.ToString();
            ListParameter.Add(parameter);
            int Return = 0;
            DGeneric = new DataGenericExecute(PrefixConn);
            return Return = DGeneric.ExecuteStoreProcedureScalar("SUP.GET_IDPERSON", ListParameter);
        }

        public int GetConsecutive(string nom_tabla)
        {
            List<Parameters> ListParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Double";
            parameter.Parameter = "sn_muestra";
            parameter.Value = "-1";
            ListParameter.Add(parameter);
            parameter = new Parameters();
            parameter.ParameterType = "AseDbType.VarChar";
            parameter.Parameter = "nom_tabla";
            parameter.Value = nom_tabla;
            ListParameter.Add(parameter);
            parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "ult_nro";
            parameter.Value = "0";
            ListParameter.Add(parameter);
            int Return = 0;
            DGeneric = new DataGenericExecute(PrefixConn);
            return Return = DGeneric.ExecuteStoreProcedureScalar("spiu_tvarias_ult_nro", ListParameter);
        }

        #endregion

        #region Tablas Roles

        public List<TableMessage> dtoDataInsured(DtoDataInsured dtoDataInsured, List<TableMessage> ListTableMessage)
        {
            if (dtoDataInsured.mpersona != null)
            {
                MpersonaFactory mpersona = new MpersonaFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona> listMpersona = new List<Mpersona>();
                listMpersona.Add(dtoDataInsured.mpersona);
                ListTableMessage = mpersona.Process(listMpersona, ListTableMessage);
            }

            if (dtoDataInsured.mpersona_ciiu != null)
            {
                Mpersona_ciiuFactory mpersona_ciiu = new Mpersona_ciiuFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_ciiu> listMpersona_ciiu = new List<Mpersona_ciiu>();
                listMpersona_ciiu.Add(dtoDataInsured.mpersona_ciiu);
                ListTableMessage = mpersona_ciiu.Process(listMpersona_ciiu, ListTableMessage);
            }

            if (dtoDataInsured.mpersona_requiere_sarlaft != null)
            {
                Mpersona_requiere_sarlaftFactory mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaftFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_requiere_sarlaft> listMpersona_requiere_sarlaft = new List<Mpersona_requiere_sarlaft>();
                listMpersona_requiere_sarlaft.Add(dtoDataInsured.mpersona_requiere_sarlaft);
                ListTableMessage = mpersona_requiere_sarlaft.Process(listMpersona_requiere_sarlaft, ListTableMessage);

                ListTableMessage = List_Mpersona_requiere_sarlaft_hist(listMpersona_requiere_sarlaft, ListTableMessage);
            }

            if (dtoDataInsured.maseg_header != null)
            {
                Maseg_headerFactory maseg_header = new Maseg_headerFactory(PrefixConn, User, IdApp, Command);
                List<Maseg_header> listMaseg_header = new List<Maseg_header>();
                listMaseg_header.Add(dtoDataInsured.maseg_header);
                ListTableMessage = maseg_header.Process(listMaseg_header, ListTableMessage);
            }

            if (dtoDataInsured.tcpto_aseg_adic != null)
            {
                Tcpto_aseg_adicFactory tcpto_aseg_adicFactory = new Tcpto_aseg_adicFactory(PrefixConn, User, IdApp, Command);
                List<Tcpto_aseg_adic> listTcpto_aseg_adic = new List<Tcpto_aseg_adic>();
                listTcpto_aseg_adic.Add(dtoDataInsured.tcpto_aseg_adic);
                ListTableMessage = tcpto_aseg_adicFactory.Process(listTcpto_aseg_adic, ListTableMessage);
            }

            if (dtoDataInsured.tipo_persona_aseg != null)
            {
                Tipo_persona_asegFactory tipo_persona_asegFactory = new Tipo_persona_asegFactory(PrefixConn, User, IdApp, Command);
                List<Tipo_persona_aseg> ListTipo_persona_aseg = new List<Tipo_persona_aseg>();
                ListTipo_persona_aseg.Add(dtoDataInsured.tipo_persona_aseg);
                ListTableMessage = tipo_persona_asegFactory.Process(ListTipo_persona_aseg, ListTableMessage);
            }

            if (dtoDataInsured.maseg_autoriza_consul != null)
            {
                if (!string.IsNullOrEmpty(dtoDataInsured.maseg_autoriza_consul.fec_sing))
                {
                    Maseg_autoriza_consulFactory maseg_autoriza_consulFactory = new Maseg_autoriza_consulFactory(PrefixConn, User, IdApp, Command);
                    List<Maseg_autoriza_consul> listmaseg_autoriza_consul = new List<Maseg_autoriza_consul>();
                    listmaseg_autoriza_consul.Add(dtoDataInsured.maseg_autoriza_consul);
                    ListTableMessage = maseg_autoriza_consulFactory.Process(listmaseg_autoriza_consul, ListTableMessage);
                }
            }

            if (dtoDataInsured.CO_EQUIVALENCE_INSURED_3G != null)
            {
                CO_EQUIVALENCE_INSURED_3GFactory CO_EQUIVALENCE_INSURED_3GFactory = new CO_EQUIVALENCE_INSURED_3GFactory(PrefixConn, User, IdApp, Command);
                List<CO_EQUIVALENCE_INSURED_3G> list_CO_EQUIVALENCE_INSURED_3G = new List<CO_EQUIVALENCE_INSURED_3G>();
                //list_CO_EQUIVALENCE_INSURED_3G.Add(dtoDataInsured.CO_EQUIVALENCE_INSURED_3G);

                CO_EQUIVALENCE_INSURED_3G EQUIVALENCE_INSURED = new CO_EQUIVALENCE_INSURED_3G();
                if (dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.State == 'C')
                {
                    char state = 'C';
                    TableMessage tMessage = new TableMessage();
                    tMessage.Message = "True";
                    tMessage.NameTable = "CO_EQUIVALENCE_INSURED_3G";

                    List<Parameters> ListParameter = new List<Parameters>();
                    Parameters parameter = new Parameters();
                    parameter.ParameterType = "AseDbType.Integer";
                    parameter.Parameter = "ID_PERSON";
                    parameter.Value = dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INDIVIDUAL_2G_ID.ToString();
                    ListParameter.Add(parameter);

                    DGeneric = new DataGenericExecute(PrefixConn);
                    DataSet Return = DGeneric.ExecuteStoreProcedure("SUP.GETEQUIVALENCE", ListParameter);
                    if (Return.Tables[0].Rows.Count > 0)
                    {
                        state = Convert.ToChar(Return.Tables[0].Rows[0][0]);
                        /*EQUIVALENCE_INSURED.State = state;
                        list_CO_EQUIVALENCE_INSURED_3G.Add(EQUIVALENCE_INSURED);*/
                        if (state != 'U')
                        {
                            dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.Update("State", state);
                            list_CO_EQUIVALENCE_INSURED_3G.Add(dtoDataInsured.CO_EQUIVALENCE_INSURED_3G);
                        }
                    }
                }
                else
                {
                    //Edward Rubiano -- HD3519 -- 04/11/2015
                    GetDto getDto = new GetDto(IdRol);
                    int INDIVIDUAL_3G_ID = 0;
                    if (dtoDataInsured.mpersona.cod_tipo_persona == "J")
                    {
                        INDIVIDUAL_3G_ID = getDto.getIndividualID3g(dtoDataInsured.mpersona.cod_tipo_doc, dtoDataInsured.mpersona.nro_nit, dtoDataInsured.mpersona.cod_tipo_persona); //Se obtiene el INDIVIDUAL_ID real de 3g juridico
                    }
                    else
                    {
                        INDIVIDUAL_3G_ID = getDto.getIndividualID3g(dtoDataInsured.mpersona.cod_tipo_doc, dtoDataInsured.mpersona.nro_doc, dtoDataInsured.mpersona.cod_tipo_persona); //Se obtiene el INDIVIDUAL_ID real de 3g natural 
                    }
                    int INSURED_3G_CD = getDto.getInsuredCD3g(INDIVIDUAL_3G_ID); //Se obtiene el INSURED_CD real de 3g
                    //Si no existe el asegurado
                    if (INSURED_3G_CD == 0)
                    {
                        DGeneric = new DataGenericExecute(PrefixConn);
                        INSURED_3G_CD = DGeneric.GetConsecutive("maseg_header");
                    }
                    //Si el INDIVIDUAL_ID de 3G que hay actualmente en la tabla de equivalencia es 0 se modifica por defecto con el INDIVIDUAL_ID de 2G
                    if (dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INDIVIDUAL_3G_ID == 0)
                    {
                        dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INDIVIDUAL_3G_ID = dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INDIVIDUAL_2G_ID;
                        dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.State = 'U';
                    }
                    //Se valida si el INDIVIDUAL_ID traido de 3g es diferente al almacenado en la equivalencia y existe en 3g
                    if (INDIVIDUAL_3G_ID != dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INDIVIDUAL_3G_ID && INDIVIDUAL_3G_ID != 0)
                    {
                        dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INDIVIDUAL_3G_ID = INDIVIDUAL_3G_ID;
                        dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.State = 'U';
                    }
                    //Se valida si el INSURED_CD traido de 3g es diferente al almacenado en la equivalencia
                    if (INSURED_3G_CD != dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INSURED_3G_CD)
                    {
                        dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.INSURED_3G_CD = INSURED_3G_CD;
                        dtoDataInsured.CO_EQUIVALENCE_INSURED_3G.State = 'U';
                    }
                    //Edward Rubiano -- 04/11/2015
                    list_CO_EQUIVALENCE_INSURED_3G.Add(dtoDataInsured.CO_EQUIVALENCE_INSURED_3G);
                }

                ListTableMessage = CO_EQUIVALENCE_INSURED_3GFactory.Process(list_CO_EQUIVALENCE_INSURED_3G, ListTableMessage);
            }

            return ListTableMessage;
        }

        public List<TableMessage> dtoDataPerson(DtoDataPerson dtoDataPerson, List<TableMessage> ListTableMessage)
        {
            if (dtoDataPerson.listMaseg_deporte != null)
            {
                Maseg_deporteFactory maseg_desporte = new Maseg_deporteFactory(PrefixConn, User, IdApp, Command);
                ListTableMessage = maseg_desporte.Process(dtoDataPerson.listMaseg_deporte, ListTableMessage);
            }

            if (dtoDataPerson.tipo_persona_aseg != null)
            {
                Tipo_persona_asegFactory tipo_persona_aseg = new Tipo_persona_asegFactory(PrefixConn, User, IdApp, Command);
                List<Tipo_persona_aseg> listaTipo_persona_aseg = new List<Tipo_persona_aseg>();
                listaTipo_persona_aseg.Add(dtoDataPerson.tipo_persona_aseg);
                ListTableMessage = tipo_persona_aseg.Process(listaTipo_persona_aseg, ListTableMessage);
            }

            if (dtoDataPerson.mpersona != null)
            {
                MpersonaFactory mpersona = new MpersonaFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona> listaMpersona = new List<Mpersona>();
                listaMpersona.Add(dtoDataPerson.mpersona);
                ListTableMessage = mpersona.Process(listaMpersona, ListTableMessage);
            }


            return ListTableMessage;
        }

        public List<TableMessage> dtoDataLawyer(DtoDataLawyer dtoDataLawyer, List<TableMessage> ListTableMessage)
        {
            if (dtoDataLawyer.mpersona != null)
            {
                MpersonaFactory mpersona = new MpersonaFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona> listMpersona = new List<Mpersona>();
                listMpersona.Add(dtoDataLawyer.mpersona);
                ListTableMessage = mpersona.Process(listMpersona, ListTableMessage);
            }

            if (dtoDataLawyer.mabogado != null)
            {
                MabogadoFactory mabogado = new MabogadoFactory(PrefixConn, User, IdApp, Command);
                List<Mabogado> listMabogado = new List<Mabogado>();
                listMabogado.Add(dtoDataLawyer.mabogado);
                ListTableMessage = mabogado.Process(listMabogado, ListTableMessage);
            }

            if (dtoDataLawyer.mpersona_ciiu != null)
            {
                Mpersona_ciiuFactory mpersona_ciiu = new Mpersona_ciiuFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_ciiu> listMpersona_ciiu = new List<Mpersona_ciiu>();
                listMpersona_ciiu.Add(dtoDataLawyer.mpersona_ciiu);
                ListTableMessage = mpersona_ciiu.Process(listMpersona_ciiu, ListTableMessage);
            }

            if (dtoDataLawyer.mpersona_requiere_sarlaft != null)
            {
                Mpersona_requiere_sarlaftFactory mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaftFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_requiere_sarlaft> listMpersona_requiere_sarlaft = new List<Mpersona_requiere_sarlaft>();
                listMpersona_requiere_sarlaft.Add(dtoDataLawyer.mpersona_requiere_sarlaft);
                ListTableMessage = mpersona_requiere_sarlaft.Process(listMpersona_requiere_sarlaft, ListTableMessage);

                ListTableMessage = List_Mpersona_requiere_sarlaft_hist(listMpersona_requiere_sarlaft, ListTableMessage);
            }
            return ListTableMessage;
        }

        public List<TableMessage> dtoDataAssigneed(DtoDataAssigneed dtoDataAssigneed, List<TableMessage> ListTableMessage)
        {
            if (dtoDataAssigneed.mpersona != null)
            {
                MpersonaFactory mpersona = new MpersonaFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona> listMpersona = new List<Mpersona>();
                listMpersona.Add(dtoDataAssigneed.mpersona);
                ListTableMessage = mpersona.Process(listMpersona, ListTableMessage);
            }

            if (dtoDataAssigneed.mcesionario != null)
            {
                McesionarioFactory mcesionario = new McesionarioFactory(PrefixConn, User, IdApp, Command);
                List<Mcesionario> listMcesionario = new List<Mcesionario>();
                listMcesionario.Add(dtoDataAssigneed.mcesionario);
                ListTableMessage = mcesionario.Process(listMcesionario, ListTableMessage);
            }

            if (dtoDataAssigneed.mpersona_ciiu != null)
            {
                Mpersona_ciiuFactory mpersona_ciiu = new Mpersona_ciiuFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_ciiu> listMpersona_ciiu = new List<Mpersona_ciiu>();
                listMpersona_ciiu.Add(dtoDataAssigneed.mpersona_ciiu);
                ListTableMessage = mpersona_ciiu.Process(listMpersona_ciiu, ListTableMessage);
            }

            if (dtoDataAssigneed.mpersona_requiere_sarlaft != null)
            {
                Mpersona_requiere_sarlaftFactory mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaftFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_requiere_sarlaft> listMpersona_requiere_sarlaft = new List<Mpersona_requiere_sarlaft>();
                listMpersona_requiere_sarlaft.Add(dtoDataAssigneed.mpersona_requiere_sarlaft);
                ListTableMessage = mpersona_requiere_sarlaft.Process(listMpersona_requiere_sarlaft, ListTableMessage);

                ListTableMessage = List_Mpersona_requiere_sarlaft_hist(listMpersona_requiere_sarlaft, ListTableMessage);
            }

            return ListTableMessage;
        }

        public List<TableMessage> dtoDataDN(DtoDataDN dtoDataDN, List<TableMessage> ListTableMessage)
        {
            if (dtoDataDN.mpersona != null)
            {
                MpersonaFactory mpersona = new MpersonaFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona> listMpersona = new List<Mpersona>();
                listMpersona.Add(dtoDataDN.mpersona);
                ListTableMessage = mpersona.Process(listMpersona, ListTableMessage);
            }

            if (dtoDataDN.tdirector_nacional != null)
            {
                Tdirector_nacionalFactory tdirector_nacional = new Tdirector_nacionalFactory(PrefixConn, User, IdApp, Command);
                List<Tdirector_nacional> listTdirector_nacional = new List<Tdirector_nacional>();
                listTdirector_nacional.Add(dtoDataDN.tdirector_nacional);
                ListTableMessage = tdirector_nacional.Process(listTdirector_nacional, ListTableMessage);
            }

            if (dtoDataDN.mpersona_ciiu != null)
            {
                Mpersona_ciiuFactory mpersona_ciiu = new Mpersona_ciiuFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_ciiu> listMpersona_ciiu = new List<Mpersona_ciiu>();
                listMpersona_ciiu.Add(dtoDataDN.mpersona_ciiu);
                ListTableMessage = mpersona_ciiu.Process(listMpersona_ciiu, ListTableMessage);
            }

            if (dtoDataDN.mpersona_requiere_sarlaft != null)
            {
                Mpersona_requiere_sarlaftFactory mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaftFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_requiere_sarlaft> listMpersona_requiere_sarlaft = new List<Mpersona_requiere_sarlaft>();
                listMpersona_requiere_sarlaft.Add(dtoDataDN.mpersona_requiere_sarlaft);
                ListTableMessage = mpersona_requiere_sarlaft.Process(listMpersona_requiere_sarlaft, ListTableMessage);

                ListTableMessage = List_Mpersona_requiere_sarlaft_hist(listMpersona_requiere_sarlaft, ListTableMessage);
            }
            return ListTableMessage;
        }

        public List<TableMessage> dtoDataProvider(DtoDataProvider dtoDataProvider, List<TableMessage> ListTableMessage)
        {
            if (dtoDataProvider.mpersona != null)
            {
                MpersonaFactory mpersona = new MpersonaFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona> listMpersona = new List<Mpersona>();
                listMpersona.Add(dtoDataProvider.mpersona);
                ListTableMessage = mpersona.Process(listMpersona, ListTableMessage);
            }

            if (dtoDataProvider.mpersona_ciiu != null)
            {
                Mpersona_ciiuFactory mpersona_ciiu = new Mpersona_ciiuFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_ciiu> listMpersona_ciiu = new List<Mpersona_ciiu>();
                listMpersona_ciiu.Add(dtoDataProvider.mpersona_ciiu);
                ListTableMessage = mpersona_ciiu.Process(listMpersona_ciiu, ListTableMessage);
            }

            if (dtoDataProvider.mpersona_requiere_sarlaft != null)
            {
                Mpersona_requiere_sarlaftFactory mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaftFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_requiere_sarlaft> listMpersona_requiere_sarlaft = new List<Mpersona_requiere_sarlaft>();
                listMpersona_requiere_sarlaft.Add(dtoDataProvider.mpersona_requiere_sarlaft);
                ListTableMessage = mpersona_requiere_sarlaft.Process(listMpersona_requiere_sarlaft, ListTableMessage);

                ListTableMessage = List_Mpersona_requiere_sarlaft_hist(listMpersona_requiere_sarlaft, ListTableMessage);
            }

            if (dtoDataProvider.mpres != null)
            {
                MpresFactory mpres = new MpresFactory(PrefixConn, User, IdApp, Command);
                List<Mpres> listMpres = new List<Mpres>();
                listMpres.Add(dtoDataProvider.mpres);
                ListTableMessage = mpres.Process(listMpres, ListTableMessage);
            }
            return ListTableMessage;
        }

        public List<TableMessage> dtoDataBeneficiary(DtoDataBeneficiary dtoDataBeneficiary, List<TableMessage> ListTableMessage)
        {
            if (dtoDataBeneficiary.mpersona != null)
            {
                MpersonaFactory mpersona = new MpersonaFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona> listMpersona = new List<Mpersona>();
                listMpersona.Add(dtoDataBeneficiary.mpersona);
                ListTableMessage = mpersona.Process(listMpersona, ListTableMessage);
            }

            if (dtoDataBeneficiary.mpersona_ciiu != null)
            {
                Mpersona_ciiuFactory mpersona_ciiu = new Mpersona_ciiuFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_ciiu> listMpersona_ciiu = new List<Mpersona_ciiu>();
                listMpersona_ciiu.Add(dtoDataBeneficiary.mpersona_ciiu);
                ListTableMessage = mpersona_ciiu.Process(listMpersona_ciiu, ListTableMessage);
            }

            if (dtoDataBeneficiary.mpersona_requiere_sarlaft != null)
            {
                Mpersona_requiere_sarlaftFactory mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaftFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_requiere_sarlaft> listMpersona_requiere_sarlaft = new List<Mpersona_requiere_sarlaft>();
                listMpersona_requiere_sarlaft.Add(dtoDataBeneficiary.mpersona_requiere_sarlaft);
                ListTableMessage = mpersona_requiere_sarlaft.Process(listMpersona_requiere_sarlaft, ListTableMessage);

                ListTableMessage = List_Mpersona_requiere_sarlaft_hist(listMpersona_requiere_sarlaft, ListTableMessage);
            }

            if (dtoDataBeneficiary.mbeneficiario != null)
            {
                MbeneficiarioFactory mbeneficiario = new MbeneficiarioFactory(PrefixConn, User, IdApp, Command);
                List<Mbeneficiario> listMbeneficiario = new List<Mbeneficiario>();
                listMbeneficiario.Add(dtoDataBeneficiary.mbeneficiario);
                ListTableMessage = mbeneficiario.Process(listMbeneficiario, ListTableMessage);
            }
            return ListTableMessage;
        }

        public List<TableMessage> dtoDataDC(DtoDataDC dtoDataDC, List<TableMessage> ListTableMessage)
        {
            if (dtoDataDC.mpersona != null)
            {
                MpersonaFactory mpersona = new MpersonaFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona> listMpersona = new List<Mpersona>();
                listMpersona.Add(dtoDataDC.mpersona);
                ListTableMessage = mpersona.Process(listMpersona, ListTableMessage);
            }

            if (dtoDataDC.tdirector_comercial != null)
            {
                Tdirector_comercialFactory tdirector_comercial = new Tdirector_comercialFactory(PrefixConn, User, IdApp, Command);
                List<Tdirector_comercial> listTdirector_comercial = new List<Tdirector_comercial>();
                listTdirector_comercial.Add(dtoDataDC.tdirector_comercial);
                ListTableMessage = tdirector_comercial.Process(listTdirector_comercial, ListTableMessage);
            }

            if (dtoDataDC.mpersona_ciiu != null)
            {
                Mpersona_ciiuFactory mpersona_ciiu = new Mpersona_ciiuFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_ciiu> listMpersona_ciiu = new List<Mpersona_ciiu>();
                listMpersona_ciiu.Add(dtoDataDC.mpersona_ciiu);
                ListTableMessage = mpersona_ciiu.Process(listMpersona_ciiu, ListTableMessage);
            }

            if (dtoDataDC.mpersona_requiere_sarlaft != null)
            {
                Mpersona_requiere_sarlaftFactory mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaftFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_requiere_sarlaft> listMpersona_requiere_sarlaft = new List<Mpersona_requiere_sarlaft>();
                listMpersona_requiere_sarlaft.Add(dtoDataDC.mpersona_requiere_sarlaft);
                ListTableMessage = mpersona_requiere_sarlaft.Process(listMpersona_requiere_sarlaft, ListTableMessage);

                ListTableMessage = List_Mpersona_requiere_sarlaft_hist(listMpersona_requiere_sarlaft, ListTableMessage);
            }

            if (dtoDataDC.tdirector_comercial_hist != null)
            {
                Tdirector_comercial_histFactory tdirector_comercial_hist = new Tdirector_comercial_histFactory(PrefixConn, User, IdApp, Command);
                List<Tdirector_comercial_hist> listTdirector_comercial_hist = new List<Tdirector_comercial_hist>();
                listTdirector_comercial_hist.Add(dtoDataDC.tdirector_comercial_hist);
                ListTableMessage = tdirector_comercial_hist.Process(listTdirector_comercial_hist, ListTableMessage);
            }

            return ListTableMessage;
        }

        public List<TableMessage> dtoDataThird(DtoDataThird dtoDataThird, List<TableMessage> ListTableMessage)
        {
            if (dtoDataThird.mpersona != null)
            {
                MpersonaFactory mpersonaThird = new MpersonaFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona> listMpersonaThird = new List<Mpersona>();
                listMpersonaThird.Add(dtoDataThird.mpersona);
                ListTableMessage = mpersonaThird.Process(listMpersonaThird, ListTableMessage);
            }

            if (dtoDataThird.mtercero != null)
            {
                MterceroFactory mterceroThird = new MterceroFactory(PrefixConn, User, IdApp, Command);
                List<Mtercero> listMterceroThird = new List<Mtercero>();
                listMterceroThird.Add(dtoDataThird.mtercero);
                ListTableMessage = mterceroThird.Process(listMterceroThird, ListTableMessage);
            }

            if (dtoDataThird.mpersona_ciiu != null)
            {
                Mpersona_ciiuFactory mpersona_ciiuThird = new Mpersona_ciiuFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_ciiu> listMpersona_ciiuThird = new List<Mpersona_ciiu>();
                listMpersona_ciiuThird.Add(dtoDataThird.mpersona_ciiu);
                ListTableMessage = mpersona_ciiuThird.Process(listMpersona_ciiuThird, ListTableMessage);
            }

            if (dtoDataThird.mpersona_requiere_sarlaft != null)
            {
                Mpersona_requiere_sarlaftFactory mpersona_requiere_sarlaftThird = new Mpersona_requiere_sarlaftFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_requiere_sarlaft> listMpersona_requiere_sarlaftThird = new List<Mpersona_requiere_sarlaft>();
                listMpersona_requiere_sarlaftThird.Add(dtoDataThird.mpersona_requiere_sarlaft);
                ListTableMessage = mpersona_requiere_sarlaftThird.Process(listMpersona_requiere_sarlaftThird, ListTableMessage);

                ListTableMessage = List_Mpersona_requiere_sarlaft_hist(listMpersona_requiere_sarlaftThird, ListTableMessage);
            }

            return ListTableMessage;
        }

        public List<TableMessage> dtoDataEmployee(DtoDataEmployee dtoDataEmployee, List<TableMessage> ListTableMessage)
        {
            if (dtoDataEmployee.mpersona != null)
            {
                MpersonaFactory mpersona = new MpersonaFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona> listMpersona = new List<Mpersona>();
                listMpersona.Add(dtoDataEmployee.mpersona);
                ListTableMessage = mpersona.Process(listMpersona, ListTableMessage);
            }

            if (dtoDataEmployee.mempleado != null)
            {
                MempleadoFactory mempleadoFactory = new MempleadoFactory(PrefixConn, User, IdApp, Command);
                List<Mempleado> listMempleado = new List<Mempleado>();
                listMempleado.Add(dtoDataEmployee.mempleado);
                ListTableMessage = mempleadoFactory.Process(listMempleado, ListTableMessage);
            }

            if (dtoDataEmployee.mpersona_ciiu != null)
            {
                Mpersona_ciiuFactory mpersona_ciiu = new Mpersona_ciiuFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_ciiu> listMpersona_ciiu = new List<Mpersona_ciiu>();
                listMpersona_ciiu.Add(dtoDataEmployee.mpersona_ciiu);
                ListTableMessage = mpersona_ciiu.Process(listMpersona_ciiu, ListTableMessage);
            }

            if (dtoDataEmployee.mpersona_requiere_sarlaft != null)
            {
                Mpersona_requiere_sarlaftFactory mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaftFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_requiere_sarlaft> listMpersona_requiere_sarlaft = new List<Mpersona_requiere_sarlaft>();
                listMpersona_requiere_sarlaft.Add(dtoDataEmployee.mpersona_requiere_sarlaft);
                ListTableMessage = mpersona_requiere_sarlaft.Process(listMpersona_requiere_sarlaft, ListTableMessage);

                ListTableMessage = List_Mpersona_requiere_sarlaft_hist(listMpersona_requiere_sarlaft, ListTableMessage);
            }
            return ListTableMessage;
        }

        public List<TableMessage> dtoDataUser(DtoDataUser dtoDataUser, List<TableMessage> ListTableMessage)
        {
            if (dtoDataUser.mpersona != null)
            {
                MpersonaFactory mpersona = new MpersonaFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona> listMpersona = new List<Mpersona>();
                listMpersona.Add(dtoDataUser.mpersona);
                ListTableMessage = mpersona.Process(listMpersona, ListTableMessage);
            }

            if (dtoDataUser.mpersona_ciiu != null)
            {
                Mpersona_ciiuFactory mpersona_ciiu = new Mpersona_ciiuFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_ciiu> listMpersona_ciiu = new List<Mpersona_ciiu>();
                listMpersona_ciiu.Add(dtoDataUser.mpersona_ciiu);
                ListTableMessage = mpersona_ciiu.Process(listMpersona_ciiu, ListTableMessage);
            }

            if (dtoDataUser.tusuario != null)
            {
                TusuarioFactory tusuario = new TusuarioFactory(PrefixConn, User, IdApp, Command);
                List<Tusuario> listTusuario = new List<Tusuario>();
                listTusuario.Add(dtoDataUser.tusuario);
                ListTableMessage = tusuario.Process(listTusuario, ListTableMessage);
            }

            if (dtoDataUser.mpersona_usuario != null)
            {
                Mpersona_usuarioFactory mpersona_usuario = new Mpersona_usuarioFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_usuario> listMpersona_usuario = new List<Mpersona_usuario>();
                listMpersona_usuario.Add(dtoDataUser.mpersona_usuario);
                ListTableMessage = mpersona_usuario.Process(listMpersona_usuario, ListTableMessage);
            }

            if (dtoDataUser.log_usuario != null)
            {
                Log_usuarioFactory log_usuario = new Log_usuarioFactory(PrefixConn, User, IdApp, Command);
                List<Log_usuario> listLog_usuario = new List<Log_usuario>();
                listLog_usuario.Add(dtoDataUser.log_usuario);
                ListTableMessage = log_usuario.Process(listLog_usuario, ListTableMessage);
            }

            return ListTableMessage;
        }

        public List<TableMessage> dtoDataAgent(DtoDataAgent dtoDataAgent, List<TableMessage> ListTableMessage)
        {
            if (dtoDataAgent.mpersona != null)
            {
                MpersonaFactory mpersona = new MpersonaFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona> listMpersona = new List<Mpersona>();
                listMpersona.Add(dtoDataAgent.mpersona);
                ListTableMessage = mpersona.Process(listMpersona, ListTableMessage);
            }

            if (dtoDataAgent.magente != null)
            {
                MagenteFactory magenteFactory = new MagenteFactory(PrefixConn, User, IdApp, Command);
                List<Magente> listMagente = new List<Magente>();
                listMagente.Add(dtoDataAgent.magente);
                ListTableMessage = magenteFactory.Process(listMagente, ListTableMessage);
            }

            if (dtoDataAgent.mpersona_ciiu != null)
            {
                Mpersona_ciiuFactory mpersona_ciiu = new Mpersona_ciiuFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_ciiu> listMpersona_ciiu = new List<Mpersona_ciiu>();
                listMpersona_ciiu.Add(dtoDataAgent.mpersona_ciiu);
                ListTableMessage = mpersona_ciiu.Process(listMpersona_ciiu, ListTableMessage);
            }

            if (dtoDataAgent.mpersona_requiere_sarlaft != null)
            {
                Mpersona_requiere_sarlaftFactory mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaftFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_requiere_sarlaft> listMpersona_requiere_sarlaft = new List<Mpersona_requiere_sarlaft>();
                listMpersona_requiere_sarlaft.Add(dtoDataAgent.mpersona_requiere_sarlaft);
                ListTableMessage = mpersona_requiere_sarlaft.Process(listMpersona_requiere_sarlaft, ListTableMessage);

                ListTableMessage = List_Mpersona_requiere_sarlaft_hist(listMpersona_requiere_sarlaft, ListTableMessage);
            }

            return ListTableMessage;
        }

        public List<TableMessage> dtoDataTA(DtoDataTA dtoDataTA, List<TableMessage> ListTableMessage)
        {
            if (dtoDataTA.mpersona != null)
            {
                MpersonaFactory mpersona = new MpersonaFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona> listMpersona = new List<Mpersona>();
                listMpersona.Add(dtoDataTA.mpersona);
                ListTableMessage = mpersona.Process(listMpersona, ListTableMessage);
            }

            if (dtoDataTA.tasist_tecnico != null)
            {
                Tasist_tecnicoFactory tasist_tecnico = new Tasist_tecnicoFactory(PrefixConn, User, IdApp, Command);
                List<Tasist_tecnico> listTasist_tecnico = new List<Tasist_tecnico>();
                listTasist_tecnico.Add(dtoDataTA.tasist_tecnico);
                ListTableMessage = tasist_tecnico.Process(listTasist_tecnico, ListTableMessage);
            }

            if (dtoDataTA.mpersona_ciiu != null)
            {
                Mpersona_ciiuFactory mpersona_ciiu = new Mpersona_ciiuFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_ciiu> listMpersona_ciiu = new List<Mpersona_ciiu>();
                listMpersona_ciiu.Add(dtoDataTA.mpersona_ciiu);
                ListTableMessage = mpersona_ciiu.Process(listMpersona_ciiu, ListTableMessage);
            }

            if (dtoDataTA.mpersona_requiere_sarlaft != null)
            {
                Mpersona_requiere_sarlaftFactory mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaftFactory(PrefixConn, User, IdApp, Command);
                List<Mpersona_requiere_sarlaft> listMpersona_requiere_sarlaft = new List<Mpersona_requiere_sarlaft>();
                listMpersona_requiere_sarlaft.Add(dtoDataTA.mpersona_requiere_sarlaft);
                ListTableMessage = mpersona_requiere_sarlaft.Process(listMpersona_requiere_sarlaft, ListTableMessage);

                ListTableMessage = List_Mpersona_requiere_sarlaft_hist(listMpersona_requiere_sarlaft, ListTableMessage);
            }
            return ListTableMessage;
        }

        #endregion

        #region Tablas Compartidas

        public List<TableMessage> List_Tusuario_limites(List<Tusuario_limites> List_Tusuario_limites, List<TableMessage> ListTableMessage)
        {
            Tusuario_limitesFactory tusuario_limites = new Tusuario_limitesFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = tusuario_limites.Process(List_Tusuario_limites, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_Tusuario_modulo_imputacion(List<Tusuario_modulo_imputacion> List_Tusuario_modulo_imputacion, List<TableMessage> ListTableMessage)
        {
            string cod = List_Tusuario_modulo_imputacion[0].cod_usuario;
            string codigo_imputacionC = string.Empty;
            string codigo_imputacionD = string.Empty;
            int contC = 0;
            int contD = 0;
            foreach (Tusuario_modulo_imputacion mi in List_Tusuario_modulo_imputacion)
            {
                if (mi.State == 'C')
                {
                    if (contC == 0)
                        codigo_imputacionC = mi.cod_imputacion.ToString();
                    else
                        codigo_imputacionC = codigo_imputacionC + '|' + mi.cod_imputacion.ToString();
                    contC++;
                }

                if (mi.State == 'D')
                {
                    if (contD == 0)
                        codigo_imputacionD = mi.cod_imputacion.ToString();
                    else
                        codigo_imputacionD = codigo_imputacionD + '|' + mi.cod_imputacion.ToString();
                    contD++;
                }
            }

            int Return = 0;

            TableMessage tMessage = new TableMessage();
            tMessage.Message = "True";
            tMessage.NameTable = "tusuario_modulo_imputacion";
            try
            {
                List<Parameters> ListParameter = new List<Parameters>();
                Parameters parameter = new Parameters();
                parameter.ParameterType = "AseDbType.VarChar";
                parameter.Parameter = "cod_usuario";
                parameter.Value = cod;
                ListParameter.Add(parameter);
                parameter = new Parameters();
                parameter.ParameterType = "AseDbType.VarChar";
                parameter.Parameter = "ListAdd";
                parameter.Value = codigo_imputacionC;
                ListParameter.Add(parameter);
                parameter = new Parameters();
                parameter.ParameterType = "AseDbType.VarChar";
                parameter.Parameter = "ListRem";
                parameter.Value = codigo_imputacionD;
                ListParameter.Add(parameter);

                DGeneric = new DataGenericExecute(PrefixConn);
                Return = DGeneric.ExecuteStoreProcedureScalar("SUP.tusua_mod_imputa", ListParameter, Command);
                if (Return == 0)
                    tMessage.Message = "True";

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

        public List<TableMessage> List_Tusuario_concepto(List<Tusuario_concepto> List_Tusuario_concepto, List<TableMessage> ListTableMessage)
        {
            string wcod_usu = List_Tusuario_concepto[0].cod_usuario;
            string wcod_suc = List_Tusuario_concepto[0].cod_suc.ToString();
            string codigo_conceptoC = string.Empty;
            string codigo_conceptoD = string.Empty;
            int contC = 0;
            int contD = 0;
            foreach (Tusuario_concepto mi in List_Tusuario_concepto)
            {
                if (mi.State == 'C')
                {
                    if (contC == 0)
                        codigo_conceptoC = mi.cod_concepto.ToString();
                    else
                        codigo_conceptoC = codigo_conceptoC + '|' + mi.cod_concepto.ToString();
                    contC++;
                }

                if (mi.State == 'D')
                {
                    if (contD == 0)
                        codigo_conceptoD = mi.cod_concepto.ToString();
                    else
                        codigo_conceptoD = codigo_conceptoD + '|' + mi.cod_concepto.ToString();
                    contD++;
                }
            }

            int Return = 0;

            TableMessage tMessage = new TableMessage();
            tMessage.Message = "True";
            tMessage.NameTable = "tusuario_concepto";
            try
            {
                List<Parameters> ListParameter = new List<Parameters>();
                Parameters parameter = new Parameters();
                parameter.ParameterType = "AseDbType.VarChar";
                parameter.Parameter = "cod_usuario";
                parameter.Value = wcod_usu;
                ListParameter.Add(parameter);
                parameter = new Parameters();
                parameter.ParameterType = "AseDbType.Double";
                parameter.Parameter = "cod_suc";
                parameter.Value = wcod_suc;
                ListParameter.Add(parameter);
                parameter = new Parameters();
                parameter.ParameterType = "AseDbType.VarChar";
                parameter.Parameter = "ListAdd";
                parameter.Value = codigo_conceptoC;
                ListParameter.Add(parameter);
                parameter = new Parameters();
                parameter.ParameterType = "AseDbType.VarChar";
                parameter.Parameter = "ListRem";
                parameter.Value = codigo_conceptoD;
                ListParameter.Add(parameter);

                DGeneric = new DataGenericExecute(PrefixConn);
                Return = DGeneric.ExecuteStoreProcedureScalar("SUP.tusuario_concepto", ListParameter, Command);
                if (Return == 0)
                    tMessage.Message = "True";

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

        public List<TableMessage> List_Tusuario_centro_costo(List<Tusuario_centro_costo> List_Tusuario_centro_costo, List<TableMessage> ListTableMessage)
        {
            string cod = List_Tusuario_centro_costo[0].cod_usuario;
            string codigo_ctroctoC = string.Empty;
            string codigo_ctroctoD = string.Empty;
            int contC = 0;
            int contD = 0;
            foreach (Tusuario_centro_costo mi in List_Tusuario_centro_costo)
            {
                if (mi.State == 'C')
                {
                    if (contC == 0)
                        codigo_ctroctoC = mi.cod_cencosto.ToString();
                    else
                        codigo_ctroctoC = codigo_ctroctoC + '|' + mi.cod_cencosto.ToString();
                    contC++;
                }

                if (mi.State == 'D')
                {
                    if (contD == 0)
                        codigo_ctroctoD = mi.cod_cencosto.ToString();
                    else
                        codigo_ctroctoD = codigo_ctroctoD + '|' + mi.cod_cencosto.ToString();
                    contD++;
                }
            }

            int Return = 0;

            TableMessage tMessage = new TableMessage();
            tMessage.Message = "True";
            tMessage.NameTable = "tusuario_centro_costo";
            try
            {
                List<Parameters> ListParameter = new List<Parameters>();
                Parameters parameter = new Parameters();
                parameter.ParameterType = "AseDbType.VarChar";
                parameter.Parameter = "cod_usuario";
                parameter.Value = cod;
                ListParameter.Add(parameter);
                parameter = new Parameters();
                parameter.ParameterType = "AseDbType.VarChar";
                parameter.Parameter = "ListAdd";
                parameter.Value = codigo_ctroctoC;
                ListParameter.Add(parameter);
                parameter = new Parameters();
                parameter.ParameterType = "AseDbType.VarChar";
                parameter.Parameter = "ListRem";
                parameter.Value = codigo_ctroctoD;
                ListParameter.Add(parameter);

                DGeneric = new DataGenericExecute(PrefixConn);
                Return = DGeneric.ExecuteStoreProcedureScalar("SUP.tusua_centro_cost", ListParameter, Command);
                if (Return == 0)
                    tMessage.Message = "True";

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

        public List<TableMessage> List_Tpj_usuarios_email(List<Tpj_usuarios_email> List_Tpj_usuarios_email, List<TableMessage> ListTableMessage)
        {
            Tpj_usuarios_emailFactory tpj_usuarios_email = new Tpj_usuarios_emailFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = tpj_usuarios_email.Process(List_Tpj_usuarios_email, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_Mbenef_asoc_aseg(List<Mbenef_asoc_aseg> List_Mbenef_asoc_aseg, List<TableMessage> ListTableMessage)
        {
            Mbenef_asoc_asegFactory mbenef_asoc_aseg = new Mbenef_asoc_asegFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = mbenef_asoc_aseg.Process(List_Mbenef_asoc_aseg, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_Mpres_cpto(List<Mpres_cpto> List_Mpres_cpto, List<TableMessage> ListTableMessage)
        {
            Mpres_cptoFactory mpres_cpto = new Mpres_cptoFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = mpres_cpto.Process(List_Mpres_cpto, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_Referencias(List<Referencias> List_Referencias, List<TableMessage> ListTableMessage)
        {
            switch (IdRol)
            {
                case 1:
                    Referencias_asegFactory Referencias_asegFactory = new Referencias_asegFactory(PrefixConn, User, IdApp, Command);
                    ListTableMessage = Referencias_asegFactory.Process(List_Referencias, ListTableMessage);
                    break;
                case 3:
                    Referencias_beneficiariosFactory Referencias_beneficiariosFactory = new Referencias_beneficiariosFactory(PrefixConn, User, IdApp, Command);
                    ListTableMessage = Referencias_beneficiariosFactory.Process(List_Referencias, ListTableMessage);
                    break;
                case 4:
                    Referencias_cesionarioFactory Referencias_cesionarioFactory = new Referencias_cesionarioFactory(PrefixConn, User, IdApp, Command);
                    ListTableMessage = Referencias_cesionarioFactory.Process(List_Referencias, ListTableMessage);
                    break;
                case 9:
                    Referencias_agenteFactory Referencias_agenteFactory = new Referencias_agenteFactory(PrefixConn, User, IdApp, Command);
                    ListTableMessage = Referencias_agenteFactory.Process(List_Referencias, ListTableMessage);
                    break;
                case 10:
                    Referencias_presFactory Referencias_presFactory = new Referencias_presFactory(PrefixConn, User, IdApp, Command);
                    ListTableMessage = Referencias_presFactory.Process(List_Referencias, ListTableMessage);
                    break;
                case 11:
                    Referencias_terceroFactory Referencias_terceroFactory = new Referencias_terceroFactory(PrefixConn, User, IdApp, Command);
                    ListTableMessage = Referencias_terceroFactory.Process(List_Referencias, ListTableMessage);
                    break;
            }

            return ListTableMessage;
        }

        public List<TableMessage> List_Mpersona_impuesto(List<Mpersona_impuesto> List_Mpersona_impuesto, List<TableMessage> ListTableMessage)
        {
            Mpersona_impuestoFactory mpersona_impuesto = new Mpersona_impuestoFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = mpersona_impuesto.Process(List_Mpersona_impuesto, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_Mcesionario(List<Mcesionario> List_Mcesionario, List<TableMessage> ListTableMessage)
        {
            McesionarioFactory McesionarioFactory = new McesionarioFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = McesionarioFactory.Process(List_Mcesionario, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_Logbook(List<LOGBOOK> List_Logbook, List<TableMessage> ListTableMessage)
        {
            LogBookFactory logBookFactory = new LogBookFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = logBookFactory.Process(List_Logbook, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_Frm_sarlaft_accionistas_asoc(List<Frm_sarlaft_accionistas_asoc> List_Frm_sarlaft_accionistas_asoc, List<TableMessage> ListTableMessage)
        {
            Frm_sarlaft_accionistas_asocFactory frm_sarlaft_accionistas_asocFactory = new Frm_sarlaft_accionistas_asocFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = frm_sarlaft_accionistas_asocFactory.Process(List_Frm_sarlaft_accionistas_asoc, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_Maseg_pmin_gastos_emi(List<Maseg_pmin_gastos_emi> List_Maseg_pmin_gastos_emi, List<TableMessage> ListTableMessage)
        {
            Maseg_pmin_gastos_emiFactory Maseg_pmin_gastos_emiFactory = new Maseg_pmin_gastos_emiFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = Maseg_pmin_gastos_emiFactory.Process(List_Maseg_pmin_gastos_emi, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_Maseg_tasa_tarifa(List<Maseg_tasa_tarifa> List_Maseg_tasa_tarifa, List<TableMessage> ListTableMessage)
        {
            Maseg_tasa_tarifaFactory maseg_tasa_tarifaFactory = new Maseg_tasa_tarifaFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = maseg_tasa_tarifaFactory.Process(List_Maseg_tasa_tarifa, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_Maseg_asociacion(List<Maseg_asociacion> List_Maseg_asociacion, List<TableMessage> ListTableMessage)
        {
            Maseg_asociacionFactory Maseg_asociacionFactory = new Maseg_asociacionFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = Maseg_asociacionFactory.Process(List_Maseg_asociacion, ListTableMessage);

            ListTableMessage = List_Maseg_asoc(List_Maseg_asociacion, ListTableMessage);

            return ListTableMessage;
        }

        /*
        public List<TableMessage> List_Maseg_asoc(List<Maseg_asoc> List_Maseg_asoc, List<TableMessage> ListTableMessage)
        {
            Maseg_asocFactory Maseg_asocFactory = new Maseg_asocFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = Maseg_asocFactory.Process(List_Maseg_asoc, ListTableMessage);
            return ListTableMessage;
        }
         * */

        public List<TableMessage> List_Mpersona_cuentas_bancarias(List<Mpersona_cuentas_bancarias> List_Mpersona_cuentas_bancarias, List<TableMessage> ListTableMessage)
        {
            Mpersona_cuentas_bancariasFactory mpersona_cuentas_bancarias = new Mpersona_cuentas_bancariasFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = mpersona_cuentas_bancarias.Process(List_Mpersona_cuentas_bancarias, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_Maseg_conducto(List<Maseg_conducto> List_Maseg_conducto, List<TableMessage> ListTableMessage)
        {
            Maseg_conductoFactory maseg_conducto = new Maseg_conductoFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = maseg_conducto.Process(List_Maseg_conducto, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_Mpersona_dir(List<Mpersona_dir> List_Mpersona_dir, List<TableMessage> ListTableMessage)
        {
            Mpersona_dirFactory mpersona_dir = new Mpersona_dirFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = mpersona_dir.Process(List_Mpersona_dir, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_Mpersona_telef(List<Mpersona_telef> List_Mpersona_telef, List<TableMessage> ListTableMessage)
        {
            Mpersona_telefFactory mpersona_telef = new Mpersona_telefFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = mpersona_telef.Process(List_Mpersona_telef, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> dtoSarlaft(DtoSarlaft dtoSarlaft, List<TableMessage> ListTableMessage)
        {
            if (dtoSarlaft.frm_sarlaft_aut_incrementos != null)
            {
                Frm_sarlaft_aut_incrementosFactory frm_sarlaft_aut_incrementosFactory = new Frm_sarlaft_aut_incrementosFactory(PrefixConn, User, IdApp, Command);
                List<Frm_sarlaft_aut_incrementos> listTempauto = new List<Frm_sarlaft_aut_incrementos>();
                listTempauto.Add(dtoSarlaft.frm_sarlaft_aut_incrementos);
                ListTableMessage = frm_sarlaft_aut_incrementosFactory.Process(listTempauto, ListTableMessage);
            }
            else
            {
                if (dtoSarlaft.List_detalle != null)
                {
                    foreach (Frm_sarlaft_detalle sarlaft_detalle in dtoSarlaft.List_detalle)
                    {
                        if (sarlaft_detalle.State == 'C')
                        {
                            int Return = 0;
                            TableMessage tMessage = new TableMessage();
                            tMessage.Message = "True";
                            tMessage.NameTable = "INTEGRATION_SARLAFT";
                            try
                            {
                                List<Parameters> ListParameter = new List<Parameters>();
                                Parameters parameter = new Parameters();
                                parameter.ParameterType = "AseDbType.Integer";
                                parameter.Parameter = "ID_PERSON";
                                parameter.Value = sarlaft_detalle.id_persona.ToString();
                                ListParameter.Add(parameter);

                                parameter = new Parameters();
                                parameter.ParameterType = "AseDbType.Integer";
                                parameter.Parameter = "ID_FORM";
                                parameter.Value = sarlaft_detalle.id_formulario.ToString();
                                ListParameter.Add(parameter);

                                parameter = new Parameters();
                                parameter.ParameterType = "AseDbType.Integer";
                                parameter.Parameter = "NUM_FORM";
                                parameter.Value = sarlaft_detalle.nro_formulario.ToString();
                                ListParameter.Add(parameter);

                                parameter = new Parameters();
                                parameter.ParameterType = "AseDbType.Integer";
                                parameter.Parameter = "BRANCH";
                                parameter.Value = sarlaft_detalle.cod_suc.ToString();
                                ListParameter.Add(parameter);

                                DGeneric = new DataGenericExecute(PrefixConn);
                                Return = DGeneric.ExecuteStoreProcedureScalar("SUP.INTEGRATION_SARLAFT_Insert", ListParameter, Command);
                                if (Return == 0)
                                    tMessage.Message = "True";
                            }
                            catch (SupException ex)
                            {
                                tMessage.Message = ex.Source;
                            }
                            finally
                            {
                                ListTableMessage.Add(tMessage);
                            }
                        }
                    }
                }

                if (dtoSarlaft.List_detalle != null)
                {
                    Frm_sarlaft_detalleFactory frm_sarlaft_detalle = new Frm_sarlaft_detalleFactory(PrefixConn, User, IdApp, Command);
                    ListTableMessage = frm_sarlaft_detalle.Process(dtoSarlaft.List_detalle, ListTableMessage);
                }

                if (dtoSarlaft.frm_sarlaft_vinculos != null)
                {
                    Frm_sarlaft_vinculosFactory frm_sarlaft_vinculosFactory = new Frm_sarlaft_vinculosFactory(PrefixConn, User, IdApp, Command);
                    List<Frm_sarlaft_vinculos> listTempfsv = new List<Frm_sarlaft_vinculos>();
                    listTempfsv.Add(dtoSarlaft.frm_sarlaft_vinculos);
                    ListTableMessage = frm_sarlaft_vinculosFactory.Process(listTempfsv, ListTableMessage);
                }

                if (dtoSarlaft.frm_sarlaft_info_financiera != null)
                {
                    Frm_sarlaft_info_financieraFactory frm_sarlaft_info_financieraFactory = new Frm_sarlaft_info_financieraFactory(PrefixConn, User, IdApp, Command);
                    List<Frm_sarlaft_info_financiera> listTempfsif = new List<Frm_sarlaft_info_financiera>();
                    listTempfsif.Add(dtoSarlaft.frm_sarlaft_info_financiera);
                    ListTableMessage = frm_sarlaft_info_financieraFactory.Process(listTempfsif, ListTableMessage);
                }

                if (dtoSarlaft.List_oper_internacionales != null)
                {
                    Frm_sarlaft_oper_internacFactory frm_sarlaft_oper_internacFactory = new Frm_sarlaft_oper_internacFactory(PrefixConn, User, IdApp, Command);
                    ListTableMessage = frm_sarlaft_oper_internacFactory.Process(dtoSarlaft.List_oper_internacionales, ListTableMessage);
                }

                if (dtoSarlaft.frm_sarlaft_detalle_entrevista != null)
                {
                    Frm_sarlaft_detalle_entrevistaFactory frm_sarlaft_detalle_entrevista = new Frm_sarlaft_detalle_entrevistaFactory(PrefixConn, User, IdApp, Command);
                    List<Frm_sarlaft_detalle_entrevista> listTempfsde = new List<Frm_sarlaft_detalle_entrevista>();
                    dtoSarlaft.frm_sarlaft_detalle_entrevista.txt_obser_entrev = (string.IsNullOrEmpty(dtoSarlaft.frm_sarlaft_detalle_entrevista.txt_obser_entrev) ? "" : dtoSarlaft.frm_sarlaft_detalle_entrevista.txt_obser_entrev);
                    listTempfsde.Add(dtoSarlaft.frm_sarlaft_detalle_entrevista);
                    ListTableMessage = frm_sarlaft_detalle_entrevista.Process(listTempfsde, ListTableMessage);
                }
            }
            return ListTableMessage;
        }

        public List<TableMessage> dtoRep(DtoRep dtoRep, List<TableMessage> ListTableMessage)
        {
            Mpersona_rep_legalFactory mpersona_rep_legal = new Mpersona_rep_legalFactory(PrefixConn, User, IdApp, Command);
            List<Mpersona_rep_legal> listTemp = new List<Mpersona_rep_legal>();
            listTemp.Add(dtoRep.mpersona_rep_legal);
            ListTableMessage = mpersona_rep_legal.Process(listTemp, ListTableMessage);

            if (dtoRep.List_mpersona_rep_legal_dir != null)
            {
                Mpersona_rep_legal_dirFactory List_mpersona_rep_legal_dir = new Mpersona_rep_legal_dirFactory(PrefixConn, User, IdApp, Command);
                ListTableMessage = List_mpersona_rep_legal_dir.Process(dtoRep.List_mpersona_rep_legal_dir, ListTableMessage);
            }
            return ListTableMessage;
        }

        public List<TableMessage> List_Mpersona_email(List<Mpersona_email> List_Mpersona_email, List<TableMessage> ListTableMessage)
        {
            Mpersona_emailFactory mpersona_email = new Mpersona_emailFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = mpersona_email.Process(List_Mpersona_email, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_Mcesio_trans_bancarias(List<Mcesio_trans_bancarias> List_Mcesio_trans_bancarias, List<TableMessage> ListTableMessage)
        {
            Mcesio_trans_bancariasFactory mcesio_trans_bancarias = new Mcesio_trans_bancariasFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = mcesio_trans_bancarias.Process(List_Mcesio_trans_bancarias, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_Magente_producto(List<Magente_producto> List_Magente_producto, List<TableMessage> ListTableMessage)
        {
            Magente_productoFactory magente_producto = new Magente_productoFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = magente_producto.Process(List_Magente_producto, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_Magente_organizador(List<Magente_organizador> List_Magente_organizador, List<TableMessage> ListTableMessage)
        {
            Magente_organizadorFactory magente_organizador = new Magente_organizadorFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = magente_organizador.Process(List_Magente_organizador, ListTableMessage);
            return ListTableMessage;
        }
        public List<TableMessage> List_Magente_comision(List<Magente_comision> List_Magente_comision, List<TableMessage> ListTableMessage)
        {
            Magente_comisionFactory magente_comision = new Magente_comisionFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = magente_comision.Process(List_Magente_comision, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_Magente_ramo(List<Magente_ramo> List_Magente_ramo, List<TableMessage> ListTableMessage)
        {
            Magente_ramoFactory magente_ramo = new Magente_ramoFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = magente_ramo.Process(List_Magente_ramo, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_Mpersona_actividad(List<Mpersona_actividad> List_Mpersona_actividad, List<TableMessage> ListTableMessage)
        {
            Mpersona_actividadFactory Mpersona_actividad = new Mpersona_actividadFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = Mpersona_actividad.Process(List_Mpersona_actividad, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_Mpersona_requiere_sarlaft_hist(List<Mpersona_requiere_sarlaft> List_Mpersona_requiere_sarlaft, List<TableMessage> ListTableMessage)
        {
            List<Mpersona_requiere_sarlaft_hist> List_Mpersona_requiere_sarlaft_hist = new List<Mpersona_requiere_sarlaft_hist>();
            foreach (Mpersona_requiere_sarlaft sarlaft in List_Mpersona_requiere_sarlaft)
            {
                /*if (sarlaft.State == 'U')
                {
                    List_Mpersona_requiere_sarlaft_hist[0].State = 'C';
                }
                if (sarlaft.State == 'C' || List_Mpersona_requiere_sarlaft_hist[0].State == 'C' )
                {*/
                Mpersona_requiere_sarlaft_hist Mpersona_requiere_sarlaft_hist_Tmp = new Mpersona_requiere_sarlaft_hist();

                Mpersona_requiere_sarlaft_hist_Tmp.id_persona = sarlaft.id_persona;
                Mpersona_requiere_sarlaft_hist_Tmp.cod_usuario = User;
                Mpersona_requiere_sarlaft_hist_Tmp.sn_exonera = sarlaft.sn_exonerado;
                Mpersona_requiere_sarlaft_hist_Tmp.cod_motivo_exonera = sarlaft.cod_motivo_exonera;
                Mpersona_requiere_sarlaft_hist_Tmp.fec_modifica = sarlaft.fec_modifica;
                Mpersona_requiere_sarlaft_hist_Tmp.txt_origen = sarlaft.txt_origen;
                Mpersona_requiere_sarlaft_hist_Tmp.State = (sarlaft.State == 'U' ? 'C' : sarlaft.State);

                List_Mpersona_requiere_sarlaft_hist.Add(Mpersona_requiere_sarlaft_hist_Tmp);
                //}
            }

            Mpersona_requiere_sarlaft_histFactory Mpersona_requiere_sarlaft_hist = new Mpersona_requiere_sarlaft_histFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = Mpersona_requiere_sarlaft_hist.Process(List_Mpersona_requiere_sarlaft_hist, ListTableMessage);

            return ListTableMessage;
        }

        public List<TableMessage> List_Maseg_asoc(List<Maseg_asociacion> List_Maseg_asociacion, List<TableMessage> ListTableMessage)
        {
            List<Maseg_asoc> List_Maseg_asoc = new List<Maseg_asoc>();
            foreach (Maseg_asociacion asociacion in List_Maseg_asociacion)
            {
                Maseg_asoc asoc = new Maseg_asoc();
                asoc.cod_aseg_asociacion = asociacion.cod_aseg_asociacion;
                asoc.cod_aseg = asociacion.cod_aseg;
                asoc.nro_correla_asoc = asociacion.nro_correla_asoc;
                asoc.fec_asociacion = asociacion.fec_asociacion;
                asoc.sn_ppal = asociacion.sn_ppal;
                asoc.pje_part = asociacion.pje_part;

                asoc.State = asociacion.State;

                List_Maseg_asoc.Add(asoc);
            }

            Maseg_asocFactory Maseg_asocFactory = new Maseg_asocFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = Maseg_asocFactory.Process(List_Maseg_asoc, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> set_Juridical_Person_Register(int id_persona, List<TableMessage> ListTableMessage)
        {
            int Return = 0;
            TableMessage tMessage = new TableMessage();
            tMessage.Message = "True";
            tMessage.NameTable = "SUP.PERSON_REGISTER";

            try
            {
                List<Parameters> listParameter = new List<Parameters>();
                Parameters parameter = new Parameters();
                parameter.ParameterType = "AseDbType.Integer";
                parameter.Parameter = "PERSON_ID";
                parameter.Value = id_persona.ToString();
                listParameter.Add(parameter);

                DGeneric = new DataGenericExecute(PrefixConn);
                Return = DGeneric.ExecuteStoreProcedureScalar("SUP.SET_PERSON_REGISTER", listParameter, Command);
                if (Return == 0)
                    tMessage.Message = "True";
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

        public List<TableMessage> Maseg_ficha_tec(Maseg_ficha_tec maseg_ficha_tec, List<TableMessage> ListTableMessage)
        {
            Maseg_ficha_tecFactory maseg_ficha_tecFactory = new Maseg_ficha_tecFactory(PrefixConn, User, IdApp, Command);
            List<Maseg_ficha_tec> List_Maseg_ficha_tec = new List<Maseg_ficha_tec>();
            List_Maseg_ficha_tec.Add(maseg_ficha_tec);
            ListTableMessage = maseg_ficha_tecFactory.Process(List_Maseg_ficha_tec, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> Maseg_ficha_tec_junta(List<Maseg_ficha_tec_junta> List_maseg_ficha_tec_junta, List<TableMessage> ListTableMessage)
        {
            Maseg_ficha_tec_juntaFactory maseg_ficha_tec_juntaFactory = new Maseg_ficha_tec_juntaFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = maseg_ficha_tec_juntaFactory.Process(List_maseg_ficha_tec_junta, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> Maseg_ficha_tec_financ(List<Maseg_ficha_tec_financ> List_Maseg_ficha_tec_financ, List<TableMessage> ListTableMessage)
        {
            Maseg_ficha_tec_financFactory maseg_ficha_tec_financFactory = new Maseg_ficha_tec_financFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = maseg_ficha_tec_financFactory.Process(List_Maseg_ficha_tec_financ, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> Maseg_ficha_tec_obs(List<Maseg_ficha_tec_obs> List_Maseg_ficha_tec_obs, List<TableMessage> ListTableMessage)
        {
            Maseg_ficha_tec_obsFactory maseg_ficha_tec_financFactory = new Maseg_ficha_tec_obsFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = maseg_ficha_tec_financFactory.Process(List_Maseg_ficha_tec_obs, ListTableMessage);
            return ListTableMessage;
        }

        #endregion Tablas Compartidas

        #region Tablas 3G

        public List<TableMessage> List_INDIVIDUAL(List<INDIVIDUAL> _INDIVIDUAL, List<TableMessage> ListTableMessage)
        {
            INDIVIDUALFactory _INDIVIDUALFactory = new INDIVIDUALFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _INDIVIDUALFactory.Process(_INDIVIDUAL, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_PERSON(List<PERSON> _PERSON, List<TableMessage> ListTableMessage)
        {
            PERSONFactory _PERSONFactory = new PERSONFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _PERSONFactory.Process(_PERSON, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_PERSON_JOB(List<PERSON_JOB> _PERSON_JOB, List<TableMessage> ListTableMessage)
        {
            PERSON_JOBFactory _PERSON_JOBFactory = new PERSON_JOBFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _PERSON_JOBFactory.Process(_PERSON_JOB, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_ADDRESS(List<ADDRESS> List_ADDRESS, List<TableMessage> ListTableMessage)
        {
            int DATA_ID = 0;
            foreach (ADDRESS ad in List_ADDRESS)
            {
                if (ad.State == 'C')
                {
                    List<Parameters> listParameter = new List<Parameters>();
                    Parameters parameter = new Parameters();
                    parameter.ParameterType = "AseDbType.Integer";
                    parameter.Parameter = "id_persona";
                    parameter.Value = ad.INDIVIDUAL_ID.ToString();
                    listParameter.Add(parameter);

                    List<Parameters> listParameterValidate = new List<Parameters>();
                    Parameters parameterValidate = new Parameters();
                    parameterValidate.ParameterType = "AseDbType.Integer";
                    parameterValidate.Parameter = "INDIVIDUAL_ID";
                    parameterValidate.Value = ad.INDIVIDUAL_ID.ToString();
                    listParameterValidate.Add(parameterValidate);

                    DGeneric = new DataGenericExecute(PrefixConn);

                    if (!validateDataId)
                    {
                        DGeneric.ExecuteStoreProcedure("SUP.VALIDATE_DATA_ID_INDIVIDUAL", listParameterValidate, Command);
                        validateDataId = true;
                    }

                    DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_DATAID3G", listParameter, Command);
                    INDIVIDUAL INDIV = Dtl.ConvertTo<INDIVIDUAL>(DsReturn.Tables[0])[0];

                    DATA_ID = (INDIV.AT_DATA_ID == 0 ? 1 : INDIV.AT_DATA_ID + 1);
                    //Se asigna el atributo
                    ad.DATA_ID = DATA_ID;
                    //Se actualiza la tabla Individual
                    List<INDIVIDUAL> lista = new List<FullServices.Models.INDIVIDUAL>();
                    INDIV.AT_DATA_ID = DATA_ID;
                    INDIV.State = 'U';
                    lista.Add(INDIV);

                    List_INDIVIDUAL(lista, ListTableMessage);
                }
            }

            ADDRESSFactory _ADDRESSFactory = new ADDRESSFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _ADDRESSFactory.Process(List_ADDRESS, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_PHONE(List<PHONE> List_PHONE, List<TableMessage> ListTableMessage)
        {
            int DATA_ID = 0;
            foreach (PHONE tel in List_PHONE)
            {
                if (tel.State == 'C')
                {
                    List<Parameters> listParameter = new List<Parameters>();
                    Parameters parameter = new Parameters();
                    parameter.ParameterType = "AseDbType.Integer";
                    parameter.Parameter = "id_persona";
                    parameter.Value = tel.INDIVIDUAL_ID.ToString();
                    listParameter.Add(parameter);

                    List<Parameters> listParameterValidate = new List<Parameters>();
                    Parameters parameterValidate = new Parameters();
                    parameterValidate.ParameterType = "AseDbType.Integer";
                    parameterValidate.Parameter = "INDIVIDUAL_ID";
                    parameterValidate.Value = tel.INDIVIDUAL_ID.ToString();
                    listParameterValidate.Add(parameterValidate);

                    DGeneric = new DataGenericExecute(PrefixConn);

                    if (!validateDataId)
                    {
                        DGeneric.ExecuteStoreProcedure("SUP.VALIDATE_DATA_ID_INDIVIDUAL", listParameterValidate, Command);
                        validateDataId = true;
                    }


                    DGeneric = new DataGenericExecute(PrefixConn);
                    DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_DATAID3G", listParameter, Command);
                    INDIVIDUAL INDIV = Dtl.ConvertTo<INDIVIDUAL>(DsReturn.Tables[0])[0];

                    DATA_ID = (INDIV.AT_DATA_ID == 0 ? 1 : INDIV.AT_DATA_ID + 1);
                    //Se asigna el atributo
                    tel.DATA_ID = DATA_ID;
                    //Se actualiza la tabla Individual
                    List<INDIVIDUAL> lista = new List<FullServices.Models.INDIVIDUAL>();
                    INDIV.AT_DATA_ID = DATA_ID;
                    INDIV.State = 'U';
                    lista.Add(INDIV);

                    List_INDIVIDUAL(lista, ListTableMessage);
                }
            }
            PHONEFactory _PHONEFactory = new PHONEFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _PHONEFactory.Process(List_PHONE, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_EMAIL(List<EMAIL> List_EMAIL, List<TableMessage> ListTableMessage)
        {
            int DATA_ID = 0;
            foreach (EMAIL mail in List_EMAIL)
            {
                if (mail.State == 'C')
                {
                    List<Parameters> listParameter = new List<Parameters>();
                    Parameters parameter = new Parameters();
                    parameter.ParameterType = "AseDbType.Integer";
                    parameter.Parameter = "id_persona";
                    parameter.Value = mail.INDIVIDUAL_ID.ToString();
                    listParameter.Add(parameter);

                    List<Parameters> listParameterValidate = new List<Parameters>();
                    Parameters parameterValidate = new Parameters();
                    parameterValidate.ParameterType = "AseDbType.Integer";
                    parameterValidate.Parameter = "INDIVIDUAL_ID";
                    parameterValidate.Value = mail.INDIVIDUAL_ID.ToString();
                    listParameterValidate.Add(parameterValidate);

                    DGeneric = new DataGenericExecute(PrefixConn);

                    if (!validateDataId)
                    {
                        DGeneric.ExecuteStoreProcedure("SUP.VALIDATE_DATA_ID_INDIVIDUAL", listParameterValidate, Command);
                        validateDataId = true;
                    }

                    DGeneric = new DataGenericExecute(PrefixConn);
                    DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_DATAID3G", listParameter, Command);
                    INDIVIDUAL INDIV = Dtl.ConvertTo<INDIVIDUAL>(DsReturn.Tables[0])[0];

                    DATA_ID = (INDIV.AT_DATA_ID == 0 ? 1 : INDIV.AT_DATA_ID + 1);
                    //Se asigna el atributo
                    mail.DATA_ID = DATA_ID;
                    //Se actualiza la tabla Individual
                    List<INDIVIDUAL> lista = new List<FullServices.Models.INDIVIDUAL>();
                    INDIV.AT_DATA_ID = DATA_ID;
                    INDIV.State = 'U';
                    lista.Add(INDIV);

                    List_INDIVIDUAL(lista, ListTableMessage);
                }
            }
            EMAILFactory _EMAILFactory = new EMAILFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _EMAILFactory.Process(List_EMAIL, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_INSURED(List<INSURED> _INSURED, List<TableMessage> ListTableMessage)
        {
            INSUREDFactory _INSUREDFactory = new INSUREDFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _INSUREDFactory.Process(_INSURED, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_CO_INSURED(List<CO_INSURED> _CO_INSURED, List<TableMessage> ListTableMessage)
        {
            CO_INSUREDFactory _CO_INSUREDFactory = new CO_INSUREDFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _CO_INSUREDFactory.Process(_CO_INSURED, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_INSURED_AGENT(List<INSURED_AGENT> _INSURED_AGENT, List<TableMessage> ListTableMessage)
        {
            INSURED_AGENTFactory _INSURED_AGENTFactory = new INSURED_AGENTFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _INSURED_AGENTFactory.Process(_INSURED_AGENT, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_INDIVIDUAL_SARLAFT_EXONERATION(List<INDIVIDUAL_SARLAFT_EXONERATION> _INDIVIDUAL_SARLAFT_EXONERATION, List<TableMessage> ListTableMessage)
        {
            INDIVIDUAL_SARLAFT_EXONERATIONFactory _INDIVIDUAL_SARLAFT_EXONERATIONFactory = new INDIVIDUAL_SARLAFT_EXONERATIONFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _INDIVIDUAL_SARLAFT_EXONERATIONFactory.Process(_INDIVIDUAL_SARLAFT_EXONERATION, ListTableMessage);

            ListTableMessage = List_INDIVIDUAL_SARLAFT_LOG(_INDIVIDUAL_SARLAFT_EXONERATION, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_INDIVIDUAL_SARLAFT(List<INDIVIDUAL_SARLAFT> _INDIVIDUAL_SARLAFT, List<TableMessage> ListTableMessage)
        {
            foreach (INDIVIDUAL_SARLAFT sarlaft_detalle in _INDIVIDUAL_SARLAFT)
            {
                if (sarlaft_detalle.State == 'C')
                {
                    int Return = 0;
                    TableMessage tMessage = new TableMessage();
                    tMessage.Message = "True";
                    tMessage.NameTable = "INTEGRATION_SARLAFT";
                    try
                    {
                        List<Parameters> ListParameter = new List<Parameters>();
                        Parameters parameter = new Parameters();
                        parameter.ParameterType = "AseDbType.Integer";
                        parameter.Parameter = "ID_PERSON";
                        parameter.Value = sarlaft_detalle.INDIVIDUAL_ID.ToString();
                        ListParameter.Add(parameter);

                        parameter = new Parameters();
                        parameter.ParameterType = "AseDbType.Integer";
                        parameter.Parameter = "ID_FORM";
                        parameter.Value = sarlaft_detalle.SARLAFT_ID.ToString();
                        ListParameter.Add(parameter);

                        parameter = new Parameters();
                        parameter.ParameterType = "AseDbType.Integer";
                        parameter.Parameter = "NUM_FORM";
                        parameter.Value = sarlaft_detalle.FORM_NUM.ToString();
                        ListParameter.Add(parameter);


                        parameter = new Parameters();
                        parameter.ParameterType = "AseDbType.Integer";
                        parameter.Parameter = "BRANCH";
                        parameter.Value = sarlaft_detalle.BRANCH_CD.ToString();
                        ListParameter.Add(parameter);

                        DGeneric = new DataGenericExecute(PrefixConn);
                        Return = DGeneric.ExecuteStoreProcedureScalar("SUP.INTEGRATION_SARLAFT_Insert", ListParameter, Command);
                        if (Return == 0)
                            tMessage.Message = "True";
                    }
                    catch (SupException ex)
                    {
                        tMessage.Message = ex.Source;
                    }
                    finally
                    {
                        ListTableMessage.Add(tMessage);
                    }
                }
            }

            INDIVIDUAL_SARLAFTFactory _INDIVIDUAL_SARLAFTFactory = new INDIVIDUAL_SARLAFTFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _INDIVIDUAL_SARLAFTFactory.Process(_INDIVIDUAL_SARLAFT, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_INDIVIDUAL_LINK(List<INDIVIDUAL_LINK> _INDIVIDUAL_LINK, List<TableMessage> ListTableMessage)
        {
            INDIVIDUAL_LINKFactory _INDIVIDUAL_LINKFactory = new INDIVIDUAL_LINKFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _INDIVIDUAL_LINKFactory.Process(_INDIVIDUAL_LINK, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_INDIVIDUAL_PAYMENT_METHOD(List<INDIVIDUAL_PAYMENT_METHOD> _INDIVIDUAL_PAYMENT_METHOD, List<TableMessage> ListTableMessage)
        {
            INDIVIDUAL_PAYMENT_METHODFactory INDIVIDUAL_PAYMENT_METHODFactory = new INDIVIDUAL_PAYMENT_METHODFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = INDIVIDUAL_PAYMENT_METHODFactory.Process(_INDIVIDUAL_PAYMENT_METHOD, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_FINANCIAL_SARLAFT(List<FINANCIAL_SARLAFT> _FINANCIAL_SARLAFT, List<TableMessage> ListTableMessage)
        {
            FINANCIAL_SARLAFTFactory _FINANCIAL_SARLAFTFactory = new FINANCIAL_SARLAFTFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _FINANCIAL_SARLAFTFactory.Process(_FINANCIAL_SARLAFT, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_SARLAFT_OPERATION(List<SARLAFT_OPERATION> _SARLAFT_OPERATION, List<TableMessage> ListTableMessage)
        {
            SARLAFT_OPERATIONFactory _SARLAFT_OPERATIONFactory = new SARLAFT_OPERATIONFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _SARLAFT_OPERATIONFactory.Process(_SARLAFT_OPERATION, ListTableMessage);
            return ListTableMessage;
        }


        public List<TableMessage> List_COMPANY(List<COMPANY> _COMPANY, List<TableMessage> ListTableMessage)
        {
            COMPANYFactory _COMPANYFactory = new COMPANYFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _COMPANYFactory.Process(_COMPANY, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_UNIQUE_USERS(List<UNIQUE_USERS> _UNIQUE_USERS, List<TableMessage> ListTableMessage)
        {
            UNIQUE_USERSFactory _UNIQUE_USERSFactory = new UNIQUE_USERSFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _UNIQUE_USERSFactory.Process(_UNIQUE_USERS, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_UNIQUE_USER_LOGIN(List<UNIQUE_USER_LOGIN> _UNIQUE_USER_LOGIN, List<TableMessage> ListTableMessage)
        {
            UNIQUE_USER_LOGINFactory _UNIQUE_USER_LOGINFactory = new UNIQUE_USER_LOGINFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _UNIQUE_USER_LOGINFactory.Process(_UNIQUE_USER_LOGIN, ListTableMessage);
            return ListTableMessage;
        }
        public List<TableMessage> List_AGENT(List<AGENT> _AGENT, List<TableMessage> ListTableMessage)
        {
            AGENTFactory _AGENTFactory = new AGENTFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _AGENTFactory.Process(_AGENT, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_AGENT_AGENCY(List<AGENT_AGENCY> _AGENT_AGENCY, List<TableMessage> ListTableMessage)
        {
            AGENT_AGENCYFactory _AGENT_AGENCYFactory = new AGENT_AGENCYFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _AGENT_AGENCYFactory.Process(_AGENT_AGENCY, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_CO_COMPANY(List<CO_COMPANY> _CO_COMPANY, List<TableMessage> ListTableMessage)
        {
            CO_COMPANYFactory _CO_COMPANYFactory = new CO_COMPANYFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _CO_COMPANYFactory.Process(_CO_COMPANY, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_CO_CONSORTIUM(List<CO_CONSORTIUM> List_CO_CONSORTIUM, List<TableMessage> ListTableMessage)
        {
            CO_CONSORTIUMFactory _CO_CONSORTIUMFactory = new CO_CONSORTIUMFactory(PrefixConn, User, IdApp, Command);
            int Return = 0;
            TableMessage tMessage = new TableMessage();
            tMessage.Message = "True";
            tMessage.NameTable = "COMPANY";
            try
            {
                List<CO_CONSORTIUM> List_CO_CONSORTIUM1 = new List<CO_CONSORTIUM>();
                int count = 0;
                foreach (CO_CONSORTIUM CONSORTIUM in List_CO_CONSORTIUM)
                {
                    List<Parameters> ListParameter = new List<Parameters>();
                    Parameters parameter = new Parameters();
                    parameter.ParameterType = "AseDbType.Integer";
                    parameter.Parameter = "INDIVIDUAL_ID";
                    parameter.Value = CONSORTIUM.INDIVIDUAL_ID.ToString();
                    ListParameter.Add(parameter);

                    parameter = new Parameters();
                    parameter.ParameterType = "AseDbType.Integer";
                    parameter.Parameter = "ID_TABLE";
                    parameter.Value = "6";
                    ListParameter.Add(parameter);

                    DGeneric = new DataGenericExecute(PrefixConn);
                    Return = DGeneric.ExecuteStoreProcedureScalar("SUP.GET_EXISTS_REGISTRY", ListParameter, Command);

                    List_CO_CONSORTIUM1.Add(List_CO_CONSORTIUM[count]);
                    count++;
                    if (Return == 0)
                    {
                        
                        tMessage.Message = "El Asegurado (Consorciado) con el INDIVIDUAL_ID: " + CONSORTIUM.INDIVIDUAL_ID + " no existe en SISE 3G, se recomienda actualizar este asegurado antes de continuar";
                        throw new ApplicationException(tMessage.Message);
                    }
                    else
                    {
                        ListTableMessage = _CO_CONSORTIUMFactory.Process(List_CO_CONSORTIUM1, ListTableMessage);
                    }
                    List_CO_CONSORTIUM1.Clear();
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

        public List<TableMessage> TECHNICAL_CARD(TECHNICAL_CARD TECHNICAL_CARD, List<TableMessage> ListTableMessage)
        {
            TECHNICAL_CARDFactory _TECHNICAL_CARDFactory = new TECHNICAL_CARDFactory(PrefixConn, User, IdApp, Command);
            List<TECHNICAL_CARD> List_TECHNICAL_CARD = new List<TECHNICAL_CARD>();
            List_TECHNICAL_CARD.Add(TECHNICAL_CARD);
            ListTableMessage = _TECHNICAL_CARDFactory.Process(List_TECHNICAL_CARD, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_BOARD_DIRECTORS(List<BOARD_DIRECTORS> List_BOARD_DIRECTORS, List<TableMessage> ListTableMessage)
        {
            BOARD_DIRECTORSFactory _BOARD_DIRECTORSFactory = new BOARD_DIRECTORSFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _BOARD_DIRECTORSFactory.Process(List_BOARD_DIRECTORS, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_FINANCIAL_STATEMENTS(List<FINANCIAL_STATEMENTS> List_FINANCIAL_STATEMENTS, List<TableMessage> ListTableMessage)
        {
            FINANCIAL_STATEMENTSFactory _FINANCIAL_STATEMENTSFactory = new FINANCIAL_STATEMENTSFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _FINANCIAL_STATEMENTSFactory.Process(List_FINANCIAL_STATEMENTS, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_TECHNICAL_CARD_DESCRIPTION(List<TECHNICAL_CARD_DESCRIPTION> List_TECHNICAL_CARD_DESCRIPTION, List<TableMessage> ListTableMessage)
        {
            TECHNICAL_CARD_DESCRIPTIONFactory _TECHNICAL_CARD_DESCRIPTIONFactory = new TECHNICAL_CARD_DESCRIPTIONFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _TECHNICAL_CARD_DESCRIPTIONFactory.Process(List_TECHNICAL_CARD_DESCRIPTION, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_AGENT_PREFIX(List<AGENT_PREFIX> List_AGENT_PREFIX, List<TableMessage> ListTableMessage)
        {
            AGENT_PREFIXFactory AGENT_PREFIX = new AGENT_PREFIXFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = AGENT_PREFIX.Process(List_AGENT_PREFIX, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_INDIVIDUAL_LEGAL_REPRESENT(List<INDIVIDUAL_LEGAL_REPRESENT> _INDIVIDUAL_LEGAL_REPRESENT, List<TableMessage> ListTableMessage)
        {
            INDIVIDUAL_LEGAL_REPRESENTFactory _INDIVIDUAL_LEGAL_REPRESENTFactory = new INDIVIDUAL_LEGAL_REPRESENTFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _INDIVIDUAL_LEGAL_REPRESENTFactory.Process(_INDIVIDUAL_LEGAL_REPRESENT, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_CO_PRV_INDIVIDUAL(List<CO_PRV_INDIVIDUAL> _CO_PRV_INDIVIDUAL, List<TableMessage> ListTableMessage)
        {
            CO_PRV_INDIVIDUALFactory _CO_PRV_INDIVIDUALFactory = new CO_PRV_INDIVIDUALFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _CO_PRV_INDIVIDUALFactory.Process(_CO_PRV_INDIVIDUAL, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> PARAMETER_Update(List<INSURED> List_Insured, List<TableMessage> ListTableMessage)
        {
            TableMessage tMessage = new TableMessage();
            tMessage.Message = "True";
            tMessage.NameTable = "COMM.PARAMETER";
            try
            {
                foreach (INSURED insured in List_Insured)
                {
                    if (insured.State == 'C')
                    {
                        List<Parameters> listParameter = new List<Parameters>();
                        Parameters parameter = new Parameters();
                        parameter.ParameterType = "AseDbType.Integer";
                        parameter.Parameter = "NUM_PARAMETER";
                        parameter.Value = insured.INSURED_CD.ToString();
                        listParameter.Add(parameter);

                        DGeneric = new DataGenericExecute(PrefixConn);
                        DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.INSURED_PARAMETER_Update", listParameter, Command);

                        tMessage.Message = "True";
                    }
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

        public List<TableMessage> List_INDIVIDUAL_TAX_EXEMPTION(List<INDIVIDUAL_TAX_EXEMPTION> _INDIVIDUAL_TAX_EXEMPTION, List<TableMessage> ListTableMessage)
        {
            INDIVIDUAL_TAX_EXEMPTIONFactory _INDIVIDUAL_TAX_EXEMPTIONFactory = new INDIVIDUAL_TAX_EXEMPTIONFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _INDIVIDUAL_TAX_EXEMPTIONFactory.Process(_INDIVIDUAL_TAX_EXEMPTION, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_OPERATING_QUOTA(List<OPERATING_QUOTA> List_OPERATING_QUOTA, List<TableMessage> ListTableMessage)
        {
            OPERATING_QUOTAFactory _OPERATING_QUOTAFactory = new OPERATING_QUOTAFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _OPERATING_QUOTAFactory.Process(List_OPERATING_QUOTA, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_OPERATING_QUOTA_SARLAFT(List<FINANCIAL_SARLAFT> List_FINANCIAL, List<TableMessage> ListTableMessage)
        {
            List<OPERATING_QUOTA> List_OPERATING_QUOTA = new List<OPERATING_QUOTA>();
            foreach (FINANCIAL_SARLAFT FINANCIAL in List_FINANCIAL)
            {
                double MaxOperatingQuotaAmt = 0;
                double MAX_OPERATING = 0;
                int ParameterOQ = 2228;
                DGeneric = new DataGenericExecute(PrefixConn);
                List<Parameters> listParameter = new List<Parameters>();
                Parameters parameter = new Parameters();
                parameter.ParameterType = "AseDbType.Integer";
                parameter.Parameter = "ParameterOQ";
                parameter.Value = ParameterOQ.ToString();
                listParameter.Add(parameter);
                DataSet ds = DGeneric.ExecuteStoreProcedure("SUP.GET_PARAMETERO", listParameter);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    MAX_OPERATING = Convert.ToDouble(ds.Tables[0].Rows[0][0]);
                    MaxOperatingQuotaAmt = MAX_OPERATING;
                }

                double patrimony = (FINANCIAL.ASSETS_AMT - FINANCIAL.LIABILITIES_AMT);
                double operatingQuotaValue = (((MaxOperatingQuotaAmt > 0) && ((patrimony * 4) > MaxOperatingQuotaAmt)) ? MaxOperatingQuotaAmt : (patrimony * 4));

                DGeneric = new DataGenericExecute(PrefixConn);
                List<Parameters> listParameterO = new List<Parameters>();
                Parameters parameterO = new Parameters();
                parameterO.ParameterType = "AseDbType.Integer";
                parameterO.Parameter = "individual_id";
                parameterO.Value = FINANCIAL.INDIVIDUAL_ID.ToString();
                listParameterO.Add(parameterO);
                ds = DGeneric.ExecuteStoreProcedure("SUP.GET_OPERATING_QUOTA", listParameterO);
                DatatableToList Dtl = new DatatableToList();
                List<OPERATING_QUOTA> listquota = new List<OPERATING_QUOTA>();
                listquota = Dtl.ConvertTo<OPERATING_QUOTA>(ds.Tables[0]);

                if (listquota.ToList().Count.Equals(0))
                {
                    OPERATING_QUOTA _OPERATING_QUOTA = new OPERATING_QUOTA();
                    OPERATING_QUOTA _OPERATING_QUOTA_1 = new OPERATING_QUOTA();
                    /*Se extiende la fecha de balance cuatro meses mas del año*/
                    //Grabo Pesos            
                    //Se cambia PatrimonyAmount * 4 por operatingQuotaValue (valor validado)
                    int lineq = 2;
                    List<Parameters> listParameterHR = new List<Parameters>();
                    Parameters parameterHR = new Parameters();
                    parameterHR.ParameterType = "AseDbType.Integer";
                    parameterHR.Parameter = "HARD_RISK_TYPE_CD";
                    parameterHR.Value = lineq.ToString();
                    listParameterHR.Add(parameterHR);
                    DGeneric = new DataGenericExecute(PrefixConn);
                    ds = DGeneric.ExecuteStoreProcedure("SUP.GET_HARD_RISK_TYPE", listParameterHR);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _OPERATING_QUOTA.INDIVIDUAL_ID = FINANCIAL.INDIVIDUAL_ID;
                        _OPERATING_QUOTA.LINE_BUSINESS_CD = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                        _OPERATING_QUOTA.CURRENT_TO = DateTime.Now.AddYears(1).AddMonths(4).ToString("dd/MM/yyyy");
                        _OPERATING_QUOTA.OPERATING_QUOTA_AMT = operatingQuotaValue;
                        _OPERATING_QUOTA.CURRENCY_CD = 0;
                        _OPERATING_QUOTA.State = 'C';
                        List_OPERATING_QUOTA.Add(_OPERATING_QUOTA);

                        // Grabo Dolares            
                        //Se cambia PatrimonyAmount * 4 por operatingQuotaValue (valor validado)
                        _OPERATING_QUOTA_1.INDIVIDUAL_ID = FINANCIAL.INDIVIDUAL_ID;
                        _OPERATING_QUOTA_1.LINE_BUSINESS_CD = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                        _OPERATING_QUOTA_1.CURRENT_TO = DateTime.Now.AddYears(1).AddMonths(4).ToString("dd/MM/yyyy");
                        double exchangeRat = 1;
                        int CURRENCY_CD = 1;
                        DGeneric = new DataGenericExecute(PrefixConn);
                        List<Parameters> listParameterER = new List<Parameters>();
                        Parameters parameterER = new Parameters();
                        parameterER.ParameterType = "AseDbType.Integer";
                        parameterER.Parameter = "CURRENCY_CD";
                        parameterER.Value = CURRENCY_CD.ToString();
                        listParameterER.Add(parameterER);
                        ds = DGeneric.ExecuteStoreProcedure("SUP.GET_EXCHANGE_RATE", listParameterER);

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
                                    DGeneric = new DataGenericExecute(PrefixConn);
                                    List<Parameters> listParameterER = new List<Parameters>();
                                    Parameters parameterER = new Parameters();
                                    parameterER.ParameterType = "AseDbType.Integer";
                                    parameterER.Parameter = "CURRENCY_CD";
                                    parameterER.Value = CURRENCY_CD.ToString();
                                    listParameterER.Add(parameterER);
                                    ds = DGeneric.ExecuteStoreProcedure("SUP.GET_EXCHANGE_RATE", listParameterER);

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
            }

            OPERATING_QUOTAFactory _OPERATING_QUOTAFactory = new OPERATING_QUOTAFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _OPERATING_QUOTAFactory.Process(List_OPERATING_QUOTA, ListTableMessage);
            return ListTableMessage;
        }

        public List<TableMessage> List_INDIVIDUAL_SARLAFT_LOG(List<INDIVIDUAL_SARLAFT_EXONERATION> List_INDIVIDUAL_SARLAFT_EXONERATION, List<TableMessage> ListTableMessage)
        {
            List<INDIVIDUAL_SARLAFT_LOG> List_INDIVIDUAL_SARLAFT_LOG = new List<INDIVIDUAL_SARLAFT_LOG>();
            foreach (INDIVIDUAL_SARLAFT_EXONERATION sarlaft in List_INDIVIDUAL_SARLAFT_EXONERATION)
            {
                INDIVIDUAL_SARLAFT_LOG INDIVIDUAL_SARLAFT_LOG_Tmp = new INDIVIDUAL_SARLAFT_LOG();

                INDIVIDUAL_SARLAFT_LOG_Tmp.INDIVIDUAL_ID = sarlaft.INDIVIDUAL_ID;
                INDIVIDUAL_SARLAFT_LOG_Tmp.USER_ID = 1;
                INDIVIDUAL_SARLAFT_LOG_Tmp.EXONERATION_TYPE_CD = sarlaft.EXONERATION_TYPE_CD;
                INDIVIDUAL_SARLAFT_LOG_Tmp.IS_EXONERATED = sarlaft.IS_EXONERATED;
                INDIVIDUAL_SARLAFT_LOG_Tmp.CHANGE_DATE = sarlaft.REGISTRATION_DATE;
                INDIVIDUAL_SARLAFT_LOG_Tmp.ROLE_CD = sarlaft.ROLE_CD;
                INDIVIDUAL_SARLAFT_LOG_Tmp.State = (sarlaft.State == 'U' ? 'C' : sarlaft.State);

                List_INDIVIDUAL_SARLAFT_LOG.Add(INDIVIDUAL_SARLAFT_LOG_Tmp);
            }

            INDIVIDUAL_SARLAFT_LOGFactory _INDIVIDUAL_SARLAFT_LOG = new INDIVIDUAL_SARLAFT_LOGFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _INDIVIDUAL_SARLAFT_LOG.Process(List_INDIVIDUAL_SARLAFT_LOG, ListTableMessage);

            return ListTableMessage;
        }

        public List<TableMessage> List_INSURED_CONTRACTOR(List<INSURED_CONTRACTOR> _INSURED_CONTRACTOR, List<TableMessage> ListTableMessage)
        {
            INSURED_CONTRACTORFactory _INSURED_CONTRACTORFactory = new INSURED_CONTRACTORFactory(PrefixConn, User, IdApp, Command);
            ListTableMessage = _INSURED_CONTRACTORFactory.Process(_INSURED_CONTRACTOR, ListTableMessage);
            return ListTableMessage;
        }

        //Autor: Edward Rubiano; Fecha: 10/05/2016; Asunto: C11226; Descripcion: Funcion de guardado de datos personales basicos en sise 2g
        public List<TableMessage> UpdateBasicDataInSise2G(Mpersona mPersona, List<TableMessage> ListTableMessage)
        {
            string cnnString = ConfigurationManager.ConnectionStrings[PrefixConn].ToString();
            if (ListTableMessage.Count(c => c.Message != "True") <= 1)
            {
                using (AseConnection cnn = new AseConnection(cnnString))
                {
                    TableMessage tableMessage = new TableMessage();
                    AseCommand Command = new AseCommand();
                    Command.Connection = cnn;
                    cnn.Open();
                    SetDto setDto = new SetDto(User, "SISE2G", IdRol, Command);
                    DGeneric = new DataGenericExecute("SISE2G");

                    if (mPersona != null)
                    {
                        MpersonaFactory mpersona = new MpersonaFactory(PrefixConn, User, IdApp, Command);
                        List<Mpersona> listMpersona = new List<Mpersona>();
                        mPersona.State = 'B'; //Actualizacion de datos basicos
                        listMpersona.Add(mPersona);
                        ListTableMessage = mpersona.Process(listMpersona, ListTableMessage);
                    }
                }
            }
            return ListTableMessage;
        }
        //Autor: Edward Rubiano; Fecha: 10/05/2016; Asunto: C11226; Descripcion: Funcion de guardado de datos personales basicos en sise 2g

        //Autor: Edward Rubiano; Fecha: 13/04/2016; Asunto: C11226; Descripcion: Funcion de guardado de datos personales basicos en sise 3g
        public List<TableMessage> UpdateBasicDataInSise3G(Mpersona mPersona, int individualId, List<TableMessage> ListTableMessage)
        {
            string PrefixConn2 = ConfigurationManager.AppSettings["SUPAPP2"];
            string cnnString = ConfigurationManager.ConnectionStrings[PrefixConn2].ToString();
            if (ListTableMessage.Count(c => c.Message != "True") <= 1)
            {
                using (AseConnection cnn = new AseConnection(cnnString))
                {
                    TableMessage tableMessage = new TableMessage();
                    AseCommand Command = new AseCommand();
                    Command.Connection = cnn;
                    cnn.Open();
                    SetDto setDto = new SetDto(User, "SISE3G", IdRol, Command);
                    //DGeneric = new DataGenericExecute("SISE3G");
                    if (mPersona.cod_tipo_persona == "F")
                    {
                        PERSON _PERSON = new PERSON();
                        _PERSON.State = 'B';
                        _PERSON.INDIVIDUAL_ID = individualId;
                        _PERSON.SURNAME = mPersona.txt_apellido1;
                        _PERSON.MOTHER_LAST_NAME = mPersona.txt_apellido2;
                        _PERSON.NAME = mPersona.txt_nombre;
                        _PERSON.ID_CARD_TYPE_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(mPersona.cod_tipo_doc));
                        _PERSON.ID_CARD_NO = mPersona.nro_doc;

                        //OT PENDIENTE POR PASAR -- Si se pasa hay que comentar este fragmento de codigo
                        _PERSON.GENDER = mPersona.txt_sexo;
                        _PERSON.MARITAL_STATUS_CD = Convert.ToInt32((FullServicesProviderHelper.ToNullInt(mPersona.cod_est_civil) == null || mPersona.cod_est_civil == "99") ? 7 : FullServicesProviderHelper.ToNullInt(mPersona.cod_est_civil));
                        _PERSON.BIRTH_DATE = mPersona.fec_nac == null ? Convert.ToString(DateTime.Now) : mPersona.fec_nac;
                        _PERSON.BIRTH_PLACE = mPersona.txt_lugar_nac;
                        _PERSON.SPOUSE_NAME = mPersona.SPOUSE_NAME;
                        _PERSON.EDUCATIVE_LEVEL_CD = mPersona.EDUCATIVE_LEVEL_CD;
                        _PERSON.CHILDREN = string.IsNullOrEmpty(mPersona.CHILDREN) ? "0" : mPersona.CHILDREN;
                        _PERSON.SOCIAL_LAYER_CD = mPersona.SOCIAL_LAYER_CD;
                        _PERSON.HOUSE_TYPE_CD = mPersona.HOUSE_TYPE_CD;
                        //OT PENDIENTE POR PASAR -- Si se pasa hay que comentar este fragmento de codigo


                        List<PERSON> List_PERSON = new List<PERSON>();
                        List_PERSON.Add(_PERSON);
                        ListTableMessage = setDto.List_PERSON(List_PERSON, ListTableMessage);

                    }
                    else if (mPersona.cod_tipo_persona == "J")
                    {

                        COMPANY _COMPANY = new COMPANY();
                        _COMPANY.State = 'B';
                        _COMPANY.INDIVIDUAL_ID = individualId;
                        _COMPANY.TRADE_NAME = mPersona.txt_apellido1 + ' ' + mPersona.txt_apellido2 + ' ' + mPersona.txt_nombre;
                        _COMPANY.TRIBUTARY_ID_NO = mPersona.nro_nit.Substring(0, mPersona.nro_nit.Length - 1);

                        //OT PENDIENTE POR PASAR -- Si se pasa hay que comentar este fragmento de codigo
                        _COMPANY.TRIBUTARY_ID_TYPE_CD = 2; //NIT es el unico tipo tributario.
                        _COMPANY.COUNTRY_CD = 1;
                        _COMPANY.COMPANY_TYPE_CD = Convert.ToInt32(FullServicesProviderHelper.ToNullInt(mPersona.COMPANY_TYPE_CD == "-1" ? "1" : mPersona.COMPANY_TYPE_CD));
                        //OT PENDIENTE POR PASAR -- Si se pasa hay que comentar este fragmento de codigo

                        List<COMPANY> List_COMPANY = new List<COMPANY>();
                        List_COMPANY.Add(_COMPANY);
                        ListTableMessage = setDto.List_COMPANY(List_COMPANY, ListTableMessage);

                        //Update a CO_COMPANY <<Digito de Verificacion>>
                        CO_COMPANY _CO_COMPANY = new CO_COMPANY();
                        _CO_COMPANY.State = 'U';
                        _CO_COMPANY.INDIVIDUAL_ID = individualId;
                        _CO_COMPANY.VERIFY_DIGIT = mPersona.nro_nit.Substring(mPersona.nro_nit.Length - 1); ;
                        _CO_COMPANY.ASSOCIATION_TYPE_CD = 0;

                        List<CO_COMPANY> List_CO_COMPANY = new List<CO_COMPANY>();
                        List_CO_COMPANY.Add(_CO_COMPANY);
                        ListTableMessage = setDto.List_CO_COMPANY(List_CO_COMPANY, ListTableMessage);

                    }
                }
            }
            return ListTableMessage;
        }
        //Autor: Edward Rubiano; Fecha: 13/04/2016; Asunto: C11226; Descripcion: Funcion de guardado de datos personales basicos en sise 3g

        //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Funcion de guardado de datos direccion y telefono en sise 3g
        public List<TableMessage> UpdateAddressAndPhoneSise3G(List<Mpersona_dir> mPersonaDir, List<Mpersona_telef> mPersonaTelef, List<Mpersona_email> mPersonaEmail, int individualId, List<TableMessage> ListTableMessage)
        {
            string PrefixConn2 = ConfigurationManager.AppSettings["SUPAPP2"];
            string cnnString = ConfigurationManager.ConnectionStrings[PrefixConn2].ToString();
            if (ListTableMessage.Count(c => c.Message != "True") <= 1)
            {
                using (AseConnection cnn = new AseConnection(cnnString))
                {
                    TableMessage tableMessage = new TableMessage();
                    AseCommand Command = new AseCommand();
                    Command.Connection = cnn;
                    cnn.Open();
                    SetDto setDto = new SetDto(User, "SISE3G", IdRol, Command);
                    GetDto getDto = new GetDto(IdRol);

                    //DGeneric = new DataGenericExecute("SISE3G");


                    List<ADDRESS> List_ADDRESS = new List<ADDRESS>();

                    if (mPersonaDir == null)
                        mPersonaDir = new List<Mpersona_dir>();
                    if (mPersonaTelef == null)
                        mPersonaTelef = new List<Mpersona_telef>();
                    if (mPersonaEmail == null)
                        mPersonaEmail = new List<Mpersona_email>();

                    mPersonaDir = getDto.dtoDataAddress3g(individualId, mPersonaDir);
                    mPersonaTelef = getDto.dtoDataPhone3g(individualId, mPersonaTelef);
                    mPersonaEmail = getDto.dtoDataMail3g(individualId, mPersonaEmail);

                    foreach (Mpersona_dir dir in mPersonaDir)
                    {
                        if (Convert.ToInt32(dir.cod_tipo_dir) != 99)
                        {
                            ADDRESS _ADDRESS = new ADDRESS();
                            _ADDRESS.State = dir.State_3g;

                            _ADDRESS.INDIVIDUAL_ID = individualId;
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

                    List<PHONE> List_PHONE = new List<PHONE>();

                    foreach (Mpersona_telef phone in mPersonaTelef)
                    {
                        if (phone.txt_telefono != " " && phone.txt_telefono != ".")
                        {
                            PHONE _PHONE = new PHONE();

                            _PHONE.State = phone.State_3g;

                            _PHONE.INDIVIDUAL_ID = individualId;
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
                            //Edward Rubiano -- HD3519 -- 10/11/2015
                            long i = 0;
                            bool correctFormat = long.TryParse((telefono == "") ? phone.txt_telefono : telefono, out i);
                            if (correctFormat)
                            {
                                //Edward Rubiano -- 10/11/2015
                                _PHONE.PHONE_NUMBER = Convert.ToInt64(FullServicesProviderHelper.ToNullInt(telefono == "" ? phone.txt_telefono : telefono));
                                _PHONE.EXTENSION = phone.EXTENSION;
                                _PHONE.SCHEDULE_AVAILABILITY = phone.SCHEDULE_AVAILABILITY;
                                _PHONE.COUNTRY_CD = Convert.ToInt32(phone.cod_pais);
                                _PHONE.STATE_CD = Convert.ToInt32(phone.cod_dpto);
                                _PHONE.CITY_CD = Convert.ToInt32(phone.cod_municipio);
                                _PHONE.IS_HOME = phone.IS_HOME;

                                List_PHONE.Add(_PHONE);
                                //Edward Rubiano -- HD3519 -- 10/11/2015
                            }
                            //Edward Rubiano -- 10/11/2015
                        }
                    }

                    List<EMAIL> List_EMAIL = new List<EMAIL>();

                    foreach (Mpersona_email mail in mPersonaEmail)
                    {
                        EMAIL _EMAIL = new EMAIL();

                        _EMAIL.State = mail.State_3g;

                        _EMAIL.INDIVIDUAL_ID = individualId;
                        _EMAIL.DATA_ID = mail.DATA_ID;
                        _EMAIL.ADDRESS = mail.txt_direccion_email;
                        _EMAIL.EMAIL_TYPE_CD = mail.cod_tipo_email;
                        _EMAIL.IS_MAILING_ADDRESS = mail.IS_MAILING_ADDRESS;

                        List_EMAIL.Add(_EMAIL);
                    }

                    ListTableMessage = setDto.List_EMAIL(List_EMAIL, ListTableMessage);
                    ListTableMessage = setDto.List_ADDRESS(List_ADDRESS, ListTableMessage);
                    ListTableMessage = setDto.List_PHONE(List_PHONE, ListTableMessage);

                }
            }
            return ListTableMessage;
        }
        //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Funcion de guardado de datos direccion y telefono en sise 3g

        #endregion
    }
}
