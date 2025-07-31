using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using Sistran.Co.Previsora.Application.FullServices.DTOs;
using Sistran.Co.Previsora.Application.FullServices.Models;
using Sybase.Data.AseClient;
using Sistran.Co.Previsora.Application.FullServicesProvider.BussinesLayer;
using Sistran.Co.Previsora.Application.FullServicesProvider.DataLayer;
using Sistran.Co.Previsora.Application.FullServicesProvider.Helpers;

namespace Sistran.Co.Previsora.Application.FullServicesProvider.Provider
{
    public class GetDto
    {

        #region Constructor

        public GetDto()
        {

        }

        public GetDto(int idRol)
        {
            PrefixConn = ConfigurationManager.AppSettings["SUPAPP"];
            PrefixConn2 = ConfigurationManager.AppSettings["SUPAPP2"];
            IdRol = idRol;
        }

        #endregion constructor

        #region Propiedades

        private string PrefixConn { get; set; }
        private string PrefixConn2 { get; set; }
        private int IdRol { get; set; }
        DatatableToList Dtl = new DatatableToList();
        DataGenericExecute DGeneric;

        DtoSarlaft dtoSarlaftVar = new DtoSarlaft();

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

        #endregion

        #region Tablas Roles Nuevo

        public DtoInsured DtoInsured()
        {
            DtoInsured dtoInsured = new DtoInsured();
            dtoInsured.List_Mcesionario = new List<Mcesionario>();
            dtoInsured.List_Logbook = new List<LOGBOOK>();
            dtoInsured.List_Mpersona_dir = new List<Mpersona_dir>();
            dtoInsured.List_Mpersona_telef = new List<Mpersona_telef>();
            dtoInsured.List_Frm_sarlaft_accionistas_asoc = new List<Frm_sarlaft_accionistas_asoc>();
            dtoInsured.List_Maseg_pmin_gastos_emi = new List<Maseg_pmin_gastos_emi>();
            dtoInsured.List_Maseg_tasa_tarifa = new List<Maseg_tasa_tarifa>();
            dtoInsured.List_Mpersona_email = new List<Mpersona_email>();
            dtoInsured.List_Referencias = new List<Referencias>();
            dtoInsured.List_Mpersona_cuentas_bancarias = new List<Mpersona_cuentas_bancarias>();
            dtoInsured.List_Maseg_conducto = new List<Maseg_conducto>();
            dtoInsured.List_Maseg_asociacion = new List<Maseg_asociacion>();
            dtoInsured.List_INDIVIDUAL_TAX_EXEMPTION = new List<INDIVIDUAL_TAX_EXEMPTION>();

            DtoDataPerson dtoDataPerson = new DtoDataPerson();
            dtoDataPerson.listMaseg_deporte = new List<Maseg_deporte>();
            dtoDataPerson.maseg_ficha_tec_financ = new Maseg_ficha_tec_financ();
            dtoDataPerson.tipo_persona_aseg = new Tipo_persona_aseg();
            dtoInsured.dtoDataPerson = dtoDataPerson;

            DtoRep dtoRep = new DtoRep();
            dtoRep.List_mpersona_rep_legal_dir = new List<Mpersona_rep_legal_dir>();
            dtoRep.mpersona_rep_legal = new Mpersona_rep_legal();
            dtoInsured.dtoRep = dtoRep;

            DtoSarlaft dtoSarlaft = new DtoSarlaft();
            dtoSarlaft.List_detalle = new List<Frm_sarlaft_detalle>();
            dtoSarlaft.List_oper_internacionales = new List<Frm_sarlaft_oper_internac>();
            dtoSarlaft.frm_sarlaft_detalle_entrevista = new Frm_sarlaft_detalle_entrevista();
            dtoSarlaft.frm_sarlaft_info_financiera = new Frm_sarlaft_info_financiera();
            dtoSarlaft.frm_sarlaft_vinculos = new Frm_sarlaft_vinculos();
            dtoSarlaft.frm_sarlaft_aut_incrementos = new Frm_sarlaft_aut_incrementos();
            dtoInsured.dtoSarlaft = dtoSarlaft;

            DtoDataInsured dtoDataInsured = new DtoDataInsured();
            dtoDataInsured.tcpto_aseg_adic = new Tcpto_aseg_adic();
            dtoDataInsured.maseg_header = new Maseg_header();
            dtoDataInsured.mpersona = new Mpersona();
            dtoDataInsured.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataInsured.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
            dtoDataInsured.magente = new Magente();
            dtoDataInsured.maseg_autoriza_consul = new Maseg_autoriza_consul();
            dtoDataInsured.tipo_persona_aseg = new Tipo_persona_aseg();
            dtoDataInsured.CO_EQUIVALENCE_INSURED_3G = new CO_EQUIVALENCE_INSURED_3G();
            dtoInsured.dtoDataInsured = dtoDataInsured;

            DtoTechnicalCard dtoTechnicalCard = new DtoTechnicalCard();
            dtoTechnicalCard.TECHNICAL_CARD = new TECHNICAL_CARD();
            dtoTechnicalCard.List_BOARD_DIRECTORS = new List<BOARD_DIRECTORS>();
            dtoTechnicalCard.List_FINANCIAL_STATEMENTS = new List<FINANCIAL_STATEMENTS>();
            dtoTechnicalCard.List_TECHNICAL_CARD_DESCRIPTION = new List<TECHNICAL_CARD_DESCRIPTION>();
            dtoInsured.dtoTechnicalCard = dtoTechnicalCard;

            return dtoInsured;
        }

        public DtoLawyer DtoLawyer()
        {
            DtoLawyer dtoLawyer = new DtoLawyer();

            DtoDataLawyer dtoDataLawyer = new DtoDataLawyer();
            dtoDataLawyer.mabogado = new Mabogado();
            dtoDataLawyer.mpersona = new Mpersona();
            dtoDataLawyer.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataLawyer.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
            dtoLawyer.dtoDataLawyer = dtoDataLawyer;

            dtoLawyer.List_Mpersona_dir = new List<Mpersona_dir>();
            dtoLawyer.List_Mpersona_telef = new List<Mpersona_telef>();
            dtoLawyer.List_Mpersona_email = new List<Mpersona_email>();
            dtoLawyer.List_Logbook = new List<LOGBOOK>();

            DtoSarlaft dtoSarlaft = new DtoSarlaft();
            dtoSarlaft.List_detalle = new List<Frm_sarlaft_detalle>();
            dtoSarlaft.List_oper_internacionales = new List<Frm_sarlaft_oper_internac>();
            dtoSarlaft.frm_sarlaft_detalle_entrevista = new Frm_sarlaft_detalle_entrevista();
            dtoSarlaft.frm_sarlaft_info_financiera = new Frm_sarlaft_info_financiera();
            dtoSarlaft.frm_sarlaft_vinculos = new Frm_sarlaft_vinculos();
            dtoLawyer.dtoSarlaft = dtoSarlaft;

            dtoLawyer.List_Mcesionario = new List<Mcesionario>();

            DtoRep dtoRep = new DtoRep();
            dtoRep.List_mpersona_rep_legal_dir = new List<Mpersona_rep_legal_dir>();
            dtoRep.mpersona_rep_legal = new Mpersona_rep_legal();
            dtoLawyer.dtoRep = dtoRep;

            dtoLawyer.List_Frm_sarlaft_accionistas_asoc = new List<Frm_sarlaft_accionistas_asoc>();

            return dtoLawyer;
        }

        public DtoAssigneed DtoAssigneed()
        {
            DtoAssigneed dtoAssigneed = new DtoAssigneed();

            DtoDataAssigneed dtoDataAssigneed = new DtoDataAssigneed();
            dtoDataAssigneed.mcesionario = new Mcesionario();
            dtoDataAssigneed.mpersona = new Mpersona();
            dtoDataAssigneed.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataAssigneed.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
            dtoAssigneed.DtoDataAssigneed = dtoDataAssigneed;

            dtoAssigneed.List_Mpersona_dir = new List<Mpersona_dir>();
            dtoAssigneed.List_Mpersona_telef = new List<Mpersona_telef>();
            dtoAssigneed.List_Mpersona_email = new List<Mpersona_email>();
            dtoAssigneed.List_Mcesionario = new List<Mcesionario>();
            dtoAssigneed.List_Logbook = new List<LOGBOOK>();
            dtoAssigneed.List_Frm_sarlaft_accionistas_asoc = new List<Frm_sarlaft_accionistas_asoc>();
            dtoAssigneed.List_Mpersona_cuentas_bancarias = new List<Mpersona_cuentas_bancarias>();
            dtoAssigneed.List_CesionarioDe = new List<CesionarioDe>();
            dtoAssigneed.List_Mcesio_trans_bancarias = new List<Mcesio_trans_bancarias>();
            dtoAssigneed.List_Referencias = new List<Referencias>();

            DtoSarlaft dtoSarlaft = new DtoSarlaft();
            dtoSarlaft.List_detalle = new List<Frm_sarlaft_detalle>();
            dtoSarlaft.frm_sarlaft_detalle_entrevista = new Frm_sarlaft_detalle_entrevista();
            dtoSarlaft.frm_sarlaft_info_financiera = new Frm_sarlaft_info_financiera();
            dtoSarlaft.frm_sarlaft_vinculos = new Frm_sarlaft_vinculos();
            dtoSarlaft.List_oper_internacionales = new List<Frm_sarlaft_oper_internac>();
            dtoAssigneed.dtoSarlaft = dtoSarlaft;

            DtoRep dtoRep = new DtoRep();
            dtoRep.List_mpersona_rep_legal_dir = new List<Mpersona_rep_legal_dir>();
            dtoRep.mpersona_rep_legal = new Mpersona_rep_legal();
            dtoAssigneed.dtoRep = dtoRep;


            return dtoAssigneed;
        }

        public DtoProvider DtoProvider()
        {
            DtoProvider dtoProvider = new DtoProvider();

            dtoProvider.dtoDataPerson = new DtoDataPerson();
            dtoProvider.List_Mpersona_dir = new List<Mpersona_dir>();
            dtoProvider.List_Mpersona_telef = new List<Mpersona_telef>();
            dtoProvider.List_Mpersona_email = new List<Mpersona_email>();
            dtoProvider.List_Logbook = new List<LOGBOOK>();
            dtoProvider.List_Mpersona_impuesto = new List<Mpersona_impuesto>();
            dtoProvider.List_Mpres_cpto = new List<Mpres_cpto>();
            dtoProvider.List_Mpersona_cuentas_bancarias = new List<Mpersona_cuentas_bancarias>();
            dtoProvider.List_Mcesionario = new List<Mcesionario>();
            dtoProvider.List_Referencias = new List<Referencias>();
            dtoProvider.List_Frm_sarlaft_accionistas_asoc = new List<Frm_sarlaft_accionistas_asoc>();

            DtoRep dtoRep = new DtoRep();
            dtoRep.List_mpersona_rep_legal_dir = new List<Mpersona_rep_legal_dir>();
            dtoRep.mpersona_rep_legal = new Mpersona_rep_legal();
            dtoProvider.dtoRep = dtoRep;

            DtoSarlaft dtoSarlaft = new DtoSarlaft();
            dtoSarlaft.List_detalle = new List<Frm_sarlaft_detalle>();
            dtoSarlaft.List_oper_internacionales = new List<Frm_sarlaft_oper_internac>();
            dtoSarlaft.frm_sarlaft_detalle_entrevista = new Frm_sarlaft_detalle_entrevista();
            dtoSarlaft.frm_sarlaft_info_financiera = new Frm_sarlaft_info_financiera();
            dtoSarlaft.frm_sarlaft_vinculos = new Frm_sarlaft_vinculos();
            dtoProvider.dtoSarlaft = dtoSarlaft;

            DtoDataProvider dtoDataProvider = new DtoDataProvider();
            dtoDataProvider.mpres = new Mpres();
            dtoDataProvider.mpersona = new Mpersona();
            dtoDataProvider.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataProvider.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
            dtoProvider.dtoDataProvider = dtoDataProvider;

            return dtoProvider;
        }

        public DtoPrincipalNational DtoPrincipalNational()
        {
            DtoPrincipalNational dtoPrincipalNational = new DtoPrincipalNational();

            DtoDataDN dtoDataDN = new DtoDataDN();
            dtoDataDN.tdirector_nacional = new Tdirector_nacional();
            dtoDataDN.mpersona = new Mpersona();
            dtoDataDN.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataDN.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
            dtoPrincipalNational.dtoDataDN = dtoDataDN;

            dtoPrincipalNational.List_Mpersona_dir = new List<Mpersona_dir>();
            dtoPrincipalNational.List_Mpersona_telef = new List<Mpersona_telef>();
            dtoPrincipalNational.List_Mpersona_email = new List<Mpersona_email>();
            dtoPrincipalNational.List_Logbook = new List<LOGBOOK>();

            DtoSarlaft dtoSarlaft = new DtoSarlaft();
            dtoSarlaft.List_detalle = new List<Frm_sarlaft_detalle>();
            dtoSarlaft.List_oper_internacionales = new List<Frm_sarlaft_oper_internac>();
            dtoSarlaft.frm_sarlaft_detalle_entrevista = new Frm_sarlaft_detalle_entrevista();
            dtoSarlaft.frm_sarlaft_info_financiera = new Frm_sarlaft_info_financiera();
            dtoSarlaft.frm_sarlaft_vinculos = new Frm_sarlaft_vinculos();
            dtoPrincipalNational.dtoSarlaft = dtoSarlaft;

            return dtoPrincipalNational;
        }

        public DtoBeneficiary DtoBeneficiary()
        {
            DtoBeneficiary dtoBeneficiary = new DtoBeneficiary();

            dtoBeneficiary.dtoDataPerson = new DtoDataPerson();
            dtoBeneficiary.List_Mpersona_dir = new List<Mpersona_dir>();
            dtoBeneficiary.List_Mpersona_telef = new List<Mpersona_telef>();
            dtoBeneficiary.List_Mpersona_email = new List<Mpersona_email>();
            dtoBeneficiary.List_Logbook = new List<LOGBOOK>();
            dtoBeneficiary.List_Mbenef_asoc_aseg = new List<Mbenef_asoc_aseg>();
            dtoBeneficiary.List_Referencias = new List<Referencias>();
            dtoBeneficiary.List_Frm_sarlaft_accionistas_asoc = new List<Frm_sarlaft_accionistas_asoc>();

            DtoRep dtoRep = new DtoRep();
            dtoRep.List_mpersona_rep_legal_dir = new List<Mpersona_rep_legal_dir>();
            dtoRep.mpersona_rep_legal = new Mpersona_rep_legal();
            dtoBeneficiary.dtoRep = dtoRep;

            DtoSarlaft dtoSarlaft = new DtoSarlaft();
            dtoSarlaft.List_detalle = new List<Frm_sarlaft_detalle>();
            dtoSarlaft.List_oper_internacionales = new List<Frm_sarlaft_oper_internac>();
            dtoSarlaft.frm_sarlaft_detalle_entrevista = new Frm_sarlaft_detalle_entrevista();
            dtoSarlaft.frm_sarlaft_info_financiera = new Frm_sarlaft_info_financiera();
            dtoSarlaft.frm_sarlaft_vinculos = new Frm_sarlaft_vinculos();
            dtoBeneficiary.dtoSarlaft = dtoSarlaft;

            DtoDataBeneficiary dtoDataBeneficiary = new DtoDataBeneficiary();
            dtoDataBeneficiary.mbeneficiario = new Mbeneficiario();
            dtoDataBeneficiary.mpersona = new Mpersona();
            dtoDataBeneficiary.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataBeneficiary.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
            dtoBeneficiary.dtoDataBeneficiary = dtoDataBeneficiary;

            return dtoBeneficiary;
        }

        public DtoPrincipalComertial DtoPrincipalComertial()
        {
            DtoPrincipalComertial dtoPrincipalComertial = new DtoPrincipalComertial();

            DtoDataDC dtoDataDC = new DtoDataDC();
            dtoDataDC.tdirector_comercial = new Tdirector_comercial();
            dtoDataDC.mpersona = new Mpersona();
            dtoDataDC.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataDC.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
            dtoDataDC.tdirector_comercial_hist = new Tdirector_comercial_hist();
            dtoPrincipalComertial.dtoDataDC = dtoDataDC;

            dtoPrincipalComertial.List_Mpersona_dir = new List<Mpersona_dir>();
            dtoPrincipalComertial.List_Mpersona_telef = new List<Mpersona_telef>();
            dtoPrincipalComertial.List_Mpersona_email = new List<Mpersona_email>();
            dtoPrincipalComertial.List_Logbook = new List<LOGBOOK>();

            DtoSarlaft dtoSarlaft = new DtoSarlaft();
            dtoSarlaft.List_detalle = new List<Frm_sarlaft_detalle>();
            dtoSarlaft.List_oper_internacionales = new List<Frm_sarlaft_oper_internac>();
            dtoSarlaft.frm_sarlaft_detalle_entrevista = new Frm_sarlaft_detalle_entrevista();
            dtoSarlaft.frm_sarlaft_info_financiera = new Frm_sarlaft_info_financiera();
            dtoSarlaft.frm_sarlaft_vinculos = new Frm_sarlaft_vinculos();
            dtoPrincipalComertial.dtoSarlaft = dtoSarlaft;

            return dtoPrincipalComertial;
        }

        public DtoThird DtoThird()
        {
            DtoThird dtoThird = new DtoThird();

            DtoDataThird dtoDataThird = new DtoDataThird();
            dtoDataThird.mtercero = new Mtercero();
            dtoDataThird.mpersona = new Mpersona();
            dtoDataThird.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataThird.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
            dtoThird.DtoDataThird = dtoDataThird;

            dtoThird.List_Mpersona_dir = new List<Mpersona_dir>();
            dtoThird.List_Mpersona_telef = new List<Mpersona_telef>();
            dtoThird.List_Mpersona_email = new List<Mpersona_email>();
            dtoThird.List_Logbook = new List<LOGBOOK>();
            dtoThird.List_Frm_sarlaft_accionistas_asoc = new List<Frm_sarlaft_accionistas_asoc>();
            dtoThird.List_Referencias = new List<Referencias>();

            DtoSarlaft dtoSarlaft = new DtoSarlaft();
            dtoSarlaft.List_detalle = new List<Frm_sarlaft_detalle>();
            dtoSarlaft.frm_sarlaft_detalle_entrevista = new Frm_sarlaft_detalle_entrevista();
            dtoSarlaft.frm_sarlaft_info_financiera = new Frm_sarlaft_info_financiera();
            dtoSarlaft.frm_sarlaft_vinculos = new Frm_sarlaft_vinculos();
            dtoSarlaft.List_oper_internacionales = new List<Frm_sarlaft_oper_internac>();
            dtoThird.dtoSarlaft = dtoSarlaft;

            DtoRep dtoRep = new DtoRep();
            dtoRep.List_mpersona_rep_legal_dir = new List<Mpersona_rep_legal_dir>();
            dtoRep.mpersona_rep_legal = new Mpersona_rep_legal();
            dtoThird.dtoRep = dtoRep;

            return dtoThird;
        }

        public DtoAgent DtoAgent()
        {
            DtoAgent dtoAgent = new DtoAgent();

            DtoDataAgent dtoDataAgent = new DtoDataAgent();
            dtoDataAgent.magente = new Magente();
            dtoDataAgent.mpersona = new Mpersona();
            dtoDataAgent.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataAgent.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
            dtoAgent.dtoDataAgent = dtoDataAgent;

            DtoDataPerson dtoDataPerson = new DtoDataPerson();
            dtoDataPerson.listMaseg_deporte = new List<Maseg_deporte>();
            dtoDataPerson.maseg_ficha_tec_financ = new Maseg_ficha_tec_financ();
            dtoDataPerson.tipo_persona_aseg = new Tipo_persona_aseg();
            dtoAgent.dtoDataPerson = dtoDataPerson;

            dtoAgent.List_Mpersona_dir = new List<Mpersona_dir>();
            dtoAgent.List_Mpersona_telef = new List<Mpersona_telef>();
            dtoAgent.List_Mpersona_email = new List<Mpersona_email>();
            dtoAgent.List_Logbook = new List<LOGBOOK>();
            dtoAgent.List_Frm_sarlaft_accionistas_asoc = new List<Frm_sarlaft_accionistas_asoc>();
            dtoAgent.List_Referencias = new List<Referencias>();
            dtoAgent.List_Magente_producto = new List<Magente_producto>();
            dtoAgent.List_Magente_organizador = new List<Magente_organizador>();
            dtoAgent.List_Mcesionario = new List<Mcesionario>();
            dtoAgent.List_Mpersona_cuentas_bancarias = new List<Mpersona_cuentas_bancarias>();
            dtoAgent.List_Magente_comision = new List<Magente_comision>();
            dtoAgent.List_Magente_ramo = new List<Magente_ramo>();

            DtoSarlaft dtoSarlaft = new DtoSarlaft();
            dtoSarlaft.List_detalle = new List<Frm_sarlaft_detalle>();
            dtoSarlaft.frm_sarlaft_detalle_entrevista = new Frm_sarlaft_detalle_entrevista();
            dtoSarlaft.frm_sarlaft_info_financiera = new Frm_sarlaft_info_financiera();
            dtoSarlaft.frm_sarlaft_vinculos = new Frm_sarlaft_vinculos();
            dtoSarlaft.List_oper_internacionales = new List<Frm_sarlaft_oper_internac>();
            dtoAgent.dtoSarlaft = dtoSarlaft;


            DtoRep dtoRep = new DtoRep();
            dtoRep.List_mpersona_rep_legal_dir = new List<Mpersona_rep_legal_dir>();
            dtoRep.mpersona_rep_legal = new Mpersona_rep_legal();
            dtoAgent.dtoRep = dtoRep;

            return dtoAgent;
        }

        public DtoEmployee DtoEmployee()
        {
            DtoEmployee dtoEmployee = new DtoEmployee();


            DtoDataEmployee dtoDataEmployee = new DtoDataEmployee();
            dtoDataEmployee.mempleado = new Mempleado();
            dtoDataEmployee.mpersona = new Mpersona();
            dtoDataEmployee.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataEmployee.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
            dtoEmployee.dtoDataEmployee = dtoDataEmployee;

            DtoDataPerson dtoDataPerson = new DtoDataPerson();
            dtoDataPerson.listMaseg_deporte = new List<Maseg_deporte>();
            dtoDataPerson.maseg_ficha_tec_financ = new Maseg_ficha_tec_financ();
            dtoDataPerson.tipo_persona_aseg = new Tipo_persona_aseg();
            dtoEmployee.dtoDataPerson = dtoDataPerson;

            dtoEmployee.List_Mpersona_dir = new List<Mpersona_dir>();
            dtoEmployee.List_Mpersona_telef = new List<Mpersona_telef>();
            dtoEmployee.List_Mpersona_email = new List<Mpersona_email>();
            dtoEmployee.List_Logbook = new List<LOGBOOK>();

            DtoSarlaft dtoSarlaft = new DtoSarlaft();
            dtoSarlaft.List_detalle = new List<Frm_sarlaft_detalle>();
            dtoSarlaft.List_oper_internacionales = new List<Frm_sarlaft_oper_internac>();
            dtoSarlaft.frm_sarlaft_detalle_entrevista = new Frm_sarlaft_detalle_entrevista();
            dtoSarlaft.frm_sarlaft_info_financiera = new Frm_sarlaft_info_financiera();
            dtoSarlaft.frm_sarlaft_vinculos = new Frm_sarlaft_vinculos();
            dtoEmployee.dtoSarlaft = dtoSarlaft;

            dtoEmployee.List_Mcesionario = new List<Mcesionario>();
            dtoEmployee.List_Mpersona_cuentas_bancarias = new List<Mpersona_cuentas_bancarias>();

            return dtoEmployee;
        }

        public DtoTechnicalAssistant DtoTechnicalAssistant()
        {
            DtoTechnicalAssistant dtoTechnicalAssistant = new DtoTechnicalAssistant();

            DtoDataTA dtoDataTA = new DtoDataTA();
            dtoDataTA.tasist_tecnico = new Tasist_tecnico();
            dtoDataTA.mpersona = new Mpersona();
            dtoDataTA.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataTA.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
            dtoTechnicalAssistant.dtoDataTA = dtoDataTA;

            dtoTechnicalAssistant.List_Mpersona_dir = new List<Mpersona_dir>();
            dtoTechnicalAssistant.List_Mpersona_telef = new List<Mpersona_telef>();
            dtoTechnicalAssistant.List_Mpersona_email = new List<Mpersona_email>();
            dtoTechnicalAssistant.List_Logbook = new List<LOGBOOK>();

            DtoSarlaft dtoSarlaft = new DtoSarlaft();
            dtoSarlaft.List_detalle = new List<Frm_sarlaft_detalle>();
            dtoSarlaft.List_oper_internacionales = new List<Frm_sarlaft_oper_internac>();
            dtoSarlaft.frm_sarlaft_detalle_entrevista = new Frm_sarlaft_detalle_entrevista();
            dtoSarlaft.frm_sarlaft_info_financiera = new Frm_sarlaft_info_financiera();
            dtoSarlaft.frm_sarlaft_vinculos = new Frm_sarlaft_vinculos();
            dtoTechnicalAssistant.dtoSarlaft = dtoSarlaft;

            return dtoTechnicalAssistant;
        }

        public DtoUser DtoUser()
        {

            DtoUser dtoUser = new DtoUser();
            dtoUser.dtoDataPerson = new DtoDataPerson();
            dtoUser.List_Mpersona_dir = new List<Mpersona_dir>();
            dtoUser.List_Mpersona_telef = new List<Mpersona_telef>();
            dtoUser.List_Mpersona_email = new List<Mpersona_email>();
            dtoUser.List_Logbook = new List<LOGBOOK>();
            dtoUser.List_Tusuario_limites = new List<Tusuario_limites>();
            dtoUser.List_Tusuario_modulo_imputacion = new List<Tusuario_modulo_imputacion>();
            dtoUser.List_Tusuario_concepto = new List<Tusuario_concepto>();
            dtoUser.List_Tusuario_centro_costo = new List<Tusuario_centro_costo>();
            dtoUser.List_Tpj_usuarios_email = new List<Tpj_usuarios_email>();

            DtoSarlaft dtoSarlaft = new DtoSarlaft();
            dtoSarlaft.List_detalle = new List<Frm_sarlaft_detalle>();
            dtoSarlaft.List_oper_internacionales = new List<Frm_sarlaft_oper_internac>();
            dtoSarlaft.frm_sarlaft_detalle_entrevista = new Frm_sarlaft_detalle_entrevista();
            dtoSarlaft.frm_sarlaft_info_financiera = new Frm_sarlaft_info_financiera();
            dtoSarlaft.frm_sarlaft_vinculos = new Frm_sarlaft_vinculos();
            dtoUser.dtoSarlaft = dtoSarlaft;

            DtoDataUser dtoDataUser = new DtoDataUser();
            dtoDataUser.tusuario = new Tusuario();
            dtoDataUser.mpersona = new Mpersona();
            dtoDataUser.mpersona_usuario = new Mpersona_usuario();
            dtoDataUser.log_usuario = new Log_usuario();
            dtoDataUser.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataUser.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
            dtoUser.dtoDataUser = dtoDataUser;

            return dtoUser;
        }

        #endregion

        #region Tablas Roles

        public DtoDataInsured dtoDataInsured(int id_persona)
        {
            DtoDataInsured dtoDataInsured = new DtoDataInsured();
            dtoDataInsured.tcpto_aseg_adic = new Tcpto_aseg_adic();
            dtoDataInsured.maseg_header = new Maseg_header();
            dtoDataInsured.mpersona = new Mpersona();
            dtoDataInsured.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataInsured.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
            dtoDataInsured.magente = new Magente();
            dtoDataInsured.tipo_persona_aseg = new Tipo_persona_aseg();
            dtoDataInsured.maseg_autoriza_consul = new Maseg_autoriza_consul();
            dtoDataInsured.CO_EQUIVALENCE_INSURED_3G = new CO_EQUIVALENCE_INSURED_3G();

            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_DETAILS_INSURED_FORM", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataInsured.tcpto_aseg_adic = Dtl.ConvertTo<Tcpto_aseg_adic>(DsReturn.Tables[0])[0]; }
                if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataInsured.maseg_header = Dtl.ConvertTo<Maseg_header>(DsReturn.Tables[1])[0]; }
                if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataInsured.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[2])[0]; }
                if (DsReturn.Tables[3].Rows.Count > 0) { dtoDataInsured.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[3])[0]; }
                if (DsReturn.Tables[4].Rows.Count > 0) { dtoDataInsured.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[4])[0]; }
                if (DsReturn.Tables[5].Rows.Count > 0) { dtoDataInsured.magente = Dtl.ConvertTo<Magente>(DsReturn.Tables[5])[0]; }
                if (DsReturn.Tables[6].Rows.Count > 0) { dtoDataInsured.tipo_persona_aseg = Dtl.ConvertTo<Tipo_persona_aseg>(DsReturn.Tables[6])[0]; }
                if (DsReturn.Tables[7].Rows.Count > 0) { dtoDataInsured.maseg_autoriza_consul = Dtl.ConvertTo<Maseg_autoriza_consul>(DsReturn.Tables[7])[0]; }
                if (DsReturn.Tables[8].Rows.Count > 0) { dtoDataInsured.CO_EQUIVALENCE_INSURED_3G = Dtl.ConvertTo<CO_EQUIVALENCE_INSURED_3G>(DsReturn.Tables[8])[0]; }
            }

            return dtoDataInsured;
        }

        public DtoDataLawyer dtoDataLawyer(int cod_rol)
        {
            DtoDataLawyer dtoDataLawyer = new DtoDataLawyer();
            dtoDataLawyer.mabogado = new Mabogado();
            dtoDataLawyer.mpersona = new Mpersona();
            dtoDataLawyer.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataLawyer.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();

            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_LAWYER", cod_rol, "cod_rol");

            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataLawyer.mabogado = Dtl.ConvertTo<Mabogado>(DsReturn.Tables[0])[0]; }
                    if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataLawyer.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[1])[0]; }
                    if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataLawyer.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[2])[0]; }
                    if (DsReturn.Tables[3].Rows.Count > 0) { dtoDataLawyer.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[3])[0]; }
                }

            }

            return dtoDataLawyer;
        }

        public DtoDataEmployee dtoDataEmployee(int cod_rol)
        {
            DtoDataEmployee dtoDataEmployee = new DtoDataEmployee();
            dtoDataEmployee.mempleado = new Mempleado();
            dtoDataEmployee.mpersona = new Mpersona();
            dtoDataEmployee.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataEmployee.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();

            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_EMPLOYEE", cod_rol, "cod_rol");

            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataEmployee.mempleado = Dtl.ConvertTo<Mempleado>(DsReturn.Tables[0])[0]; }
                    if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataEmployee.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[1])[0]; }
                    if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataEmployee.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[2])[0]; }
                    if (DsReturn.Tables[3].Rows.Count > 0) { dtoDataEmployee.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[3])[0]; }
                }

            }

            return dtoDataEmployee;
        }

        public DtoDataAssigneed dtoDataAssigneed(int cod_rol)
        {
            DtoDataAssigneed dtoDataAssigneed = new DtoDataAssigneed();

            dtoDataAssigneed.mcesionario = new Mcesionario();
            dtoDataAssigneed.mpersona = new Mpersona();
            dtoDataAssigneed.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataAssigneed.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();

            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_DETAILS_ASSIGNEED_FORM", cod_rol, "cod_rol");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataAssigneed.mcesionario = Dtl.ConvertTo<Mcesionario>(DsReturn.Tables[0])[0]; }
                    if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataAssigneed.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[1])[0]; }
                    if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataAssigneed.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[2])[0]; }
                    if (DsReturn.Tables[3].Rows.Count > 0) { dtoDataAssigneed.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[3])[0]; }
                }
            }

            return dtoDataAssigneed;
        }

        public DtoDataProvider dtoDataProvider(int cod_pres)
        {
            DtoDataProvider dtoDataProvider = new DtoDataProvider();
            dtoDataProvider.mpres = new Mpres();
            dtoDataProvider.mpersona = new Mpersona();
            dtoDataProvider.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataProvider.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();

            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_DETAILS_PROVIDER_FORM", cod_pres, "cod_pres");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataProvider.mpres = Dtl.ConvertTo<Mpres>(DsReturn.Tables[0])[0]; }
                    if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataProvider.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[1])[0]; }
                    if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataProvider.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[2])[0]; }
                    if (DsReturn.Tables[3].Rows.Count > 0) { dtoDataProvider.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[3])[0]; }
                }
            }

            return dtoDataProvider;
        }

        public DtoDataDN dtoDataDN(int cod_rol)
        {
            DtoDataDN dtoDataDN = new DtoDataDN();
            dtoDataDN.tdirector_nacional = new Tdirector_nacional();
            dtoDataDN.mpersona = new Mpersona();
            dtoDataDN.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataDN.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();

            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_DIRECTORN", cod_rol, "cod_rol");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataDN.tdirector_nacional = Dtl.ConvertTo<Tdirector_nacional>(DsReturn.Tables[0])[0]; }
                    if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataDN.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[1])[0]; }
                    if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataDN.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[2])[0]; }
                    else if (DsReturn.Tables[2].Rows.Count == 0) { dtoDataDN.mpersona_ciiu = new Mpersona_ciiu(); }
                    if (DsReturn.Tables[3].Rows.Count > 0) { dtoDataDN.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[3])[0]; }
                    else if (DsReturn.Tables[3].Rows.Count == 0) { dtoDataDN.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft(); }
                }
            }
            return dtoDataDN;
        }

        public DtoDataBeneficiary dtoDataBeneficiary(int cod_beneficiario)
        {
            DtoDataBeneficiary dtoDataBeneficiary = new DtoDataBeneficiary();
            dtoDataBeneficiary.mbeneficiario = new Mbeneficiario();
            dtoDataBeneficiary.mpersona = new Mpersona();
            dtoDataBeneficiary.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataBeneficiary.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();

            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_DETAILS_BENEF_FORM", cod_beneficiario, "cod_beneficiario");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataBeneficiary.mbeneficiario = Dtl.ConvertTo<Mbeneficiario>(DsReturn.Tables[0])[0]; }
                if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataBeneficiary.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[1])[0]; }
                if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataBeneficiary.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[2])[0]; }
                if (DsReturn.Tables[3].Rows.Count > 0) { dtoDataBeneficiary.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[3])[0]; }
            }

            return dtoDataBeneficiary;
        }

        public DtoDataDC dtoDataDC(int cod_rol)
        {
            DtoDataDC dtoDataDC = new DtoDataDC();
            dtoDataDC.tdirector_comercial = new Tdirector_comercial();
            dtoDataDC.mpersona = new Mpersona();
            dtoDataDC.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataDC.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
            dtoDataDC.tdirector_comercial_hist = new Tdirector_comercial_hist();

            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_DIRECTORC", cod_rol, "cod_rol");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataDC.tdirector_comercial = Dtl.ConvertTo<Tdirector_comercial>(DsReturn.Tables[0])[0]; }
                    if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataDC.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[1])[0]; }
                    if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataDC.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[2])[0]; }
                    else if (DsReturn.Tables[2].Rows.Count == 0) { dtoDataDC.mpersona_ciiu = new Mpersona_ciiu(); }
                    if (DsReturn.Tables[3].Rows.Count > 0) { dtoDataDC.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[3])[0]; }
                    else if (DsReturn.Tables[3].Rows.Count == 0) { dtoDataDC.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft(); }
                    if (DsReturn.Tables[4].Rows.Count > 0) { dtoDataDC.tdirector_comercial_hist = Dtl.ConvertTo<Tdirector_comercial_hist>(DsReturn.Tables[4])[0]; }
                }
            }
            return dtoDataDC;
        }

        public DtoDataThird dtoDataThird(int cod_rol)
        {
            DtoDataThird dtoDataThird = new DtoDataThird();

            dtoDataThird.mpersona = new Mpersona();
            dtoDataThird.mtercero = new Mtercero();
            dtoDataThird.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataThird.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();

            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_DETAILS_THIRD_FORM", cod_rol, "cod_rol");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataThird.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[0])[0]; }
                    if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataThird.mtercero = Dtl.ConvertTo<Mtercero>(DsReturn.Tables[1])[0]; }
                    if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataThird.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[2])[0]; }
                    if (DsReturn.Tables[3].Rows.Count > 0) { dtoDataThird.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[3])[0]; }
                }
            }

            return dtoDataThird;
        }

        public DtoDataAgent dtoDataAgent(int cod_rol)
        {
            DtoDataAgent dtoDataAgent = new DtoDataAgent();

            dtoDataAgent.magente = new Magente();
            dtoDataAgent.mpersona = new Mpersona();
            dtoDataAgent.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataAgent.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();

            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_DETAILS_AGENT_FORM", cod_rol, "cod_rol");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataAgent.magente = Dtl.ConvertTo<Magente>(DsReturn.Tables[0])[0]; }
                    if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataAgent.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[1])[0]; }
                    if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataAgent.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[2])[0]; }
                    else if (DsReturn.Tables[2].Rows.Count == 0) { dtoDataAgent.mpersona_ciiu = new Mpersona_ciiu(); }
                    if (DsReturn.Tables[3].Rows.Count > 0) { dtoDataAgent.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[3])[0]; }
                    else if (DsReturn.Tables[3].Rows.Count == 0) { dtoDataAgent.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft(); }
                }
            }

            return dtoDataAgent;
        }

        public DtoDataTA dtoDataTA(int cod_rol)
        {
            DtoDataTA dtoDataTA = new DtoDataTA();
            dtoDataTA.tasist_tecnico = new Tasist_tecnico();
            dtoDataTA.mpersona = new Mpersona();
            dtoDataTA.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataTA.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();

            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_ASISTECNICO", cod_rol, "cod_rol");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataTA.tasist_tecnico = Dtl.ConvertTo<Tasist_tecnico>(DsReturn.Tables[0])[0]; }
                    if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataTA.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[1])[0]; }
                    if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataTA.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[2])[0]; }
                    else if (DsReturn.Tables[2].Rows.Count == 0) { dtoDataTA.mpersona_ciiu = new Mpersona_ciiu(); }
                    if (DsReturn.Tables[3].Rows.Count > 0) { dtoDataTA.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[3])[0]; }
                    else if (DsReturn.Tables[3].Rows.Count == 0) { dtoDataTA.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft(); }
                }
            }
            return dtoDataTA;
        }

        public DtoDataUser dtoDataUser(string cod_usuario)
        {
            DtoDataUser dtoDataUser = new DtoDataUser();
            dtoDataUser.tusuario = new Tusuario();
            dtoDataUser.mpersona = new Mpersona();
            dtoDataUser.mpersona_usuario = new Mpersona_usuario();
            dtoDataUser.log_usuario = new Log_usuario();
            dtoDataUser.mpersona_ciiu = new Mpersona_ciiu();
            dtoDataUser.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();

            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_DETAILS_USER_FORM", cod_usuario, "cod_usuario");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataUser.tusuario = Dtl.ConvertTo<Tusuario>(DsReturn.Tables[0])[0]; }
                    if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataUser.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[1])[0]; }
                    if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataUser.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[2])[0]; }
                    if (DsReturn.Tables[3].Rows.Count > 0) { dtoDataUser.log_usuario = Dtl.ConvertTo<Log_usuario>(DsReturn.Tables[3])[0]; }
                    if (DsReturn.Tables[4].Rows.Count > 0) { dtoDataUser.mpersona_usuario = Dtl.ConvertTo<Mpersona_usuario>(DsReturn.Tables[4])[0]; }
                    if (DsReturn.Tables[5].Rows.Count > 0) { dtoDataUser.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[5])[0]; }
                }
            }

            return dtoDataUser;
        }

        #endregion

        #region Tablas Compartidas

        public List<Mbenef_asoc_aseg> List_Mbenef_asoc_aseg(int cod_rol)
        {
            int rowcount = 0;
            List<Mbenef_asoc_aseg> List_Mbenef_asoc_aseg = new List<Mbenef_asoc_aseg>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_BENEFICIARY_OF", cod_rol, "cod_beneficiario");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Mbenef_asoc_aseg = Dtl.ConvertTo<Mbenef_asoc_aseg>(DsReturn.Tables[0]);
                    List_Mbenef_asoc_aseg.ForEach(delegate(Mbenef_asoc_aseg element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }
            return List_Mbenef_asoc_aseg;
        }

        public List<Mpres_cpto> List_Mpres_cpto(int id_persona)
        {
            int rowcount = 0;
            List<Mpres_cpto> List_Mpres_cpto = new List<Mpres_cpto>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_CONCEPTS", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Mpres_cpto = Dtl.ConvertTo<Mpres_cpto>(DsReturn.Tables[0]);
                    List_Mpres_cpto.ForEach(delegate(Mpres_cpto element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }
            return List_Mpres_cpto;
        }

        public List<Mpersona_impuesto> List_Mpersona_impuesto(int id_persona)
        {
            int rowcount = 0;
            List<Mpersona_impuesto> List_Mpersona_impuesto = new List<Mpersona_impuesto>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_TAXES", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Mpersona_impuesto = Dtl.ConvertTo<Mpersona_impuesto>(DsReturn.Tables[0]);
                    List_Mpersona_impuesto.ForEach(delegate(Mpersona_impuesto element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }
            return List_Mpersona_impuesto;
        }

        public List<Mcesionario> List_Mcesionario(int id_persona)
        {
            List<Mcesionario> List_Mcesionario = new List<Mcesionario>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_ASSIGNS", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Mcesionario = Dtl.ConvertTo<Mcesionario>(DsReturn.Tables[0]);
                }
            }
            return List_Mcesionario;
        }

        public List<CesionarioDe> List_CesionarioDe(int cod_rol)
        {
            int rowcount = 0;
            List<CesionarioDe> List_CesionarioDe = new List<CesionarioDe>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_ASSIGNEEDOF", cod_rol, "cod_rol");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_CesionarioDe = Dtl.ConvertTo<CesionarioDe>(DsReturn.Tables[0]);
                    List_CesionarioDe.ForEach(delegate(CesionarioDe element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }
            return List_CesionarioDe;
        }

        public List<LOGBOOK> List_Logbook(int id_persona, int id_rol)
        {
            List<Parameters> ListParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.NameTable = "LOGBOOK";
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "id_persona";
            parameter.Value = id_persona.ToString();
            ListParameter.Add(parameter);
            parameter = new Parameters();
            parameter.NameTable = "LOGBOOK";
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "id_rol";
            parameter.Value = id_rol.ToString();
            ListParameter.Add(parameter);

            int rowcount = 0;
            List<LOGBOOK> List_Logbook = new List<LOGBOOK>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GETLOGBOOK", ListParameter);
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Logbook = Dtl.ConvertTo<LOGBOOK>(DsReturn.Tables[0]);
                    List_Logbook.ForEach(delegate(LOGBOOK element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }
            return List_Logbook;
        }

        public List<Mpersona_dir> List_Mpersona_dir(int id_persona)
        {
            int rowcount = 1;
            List<Mpersona_dir> List_Mpersona_dir = new List<Mpersona_dir>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_ADDRESS", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Mpersona_dir = Dtl.ConvertTo<Mpersona_dir>(DsReturn.Tables[0]);
                    List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.Identity = rowcount++; element.State = 'R'; });
                    //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                    List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.cod_tipo_dir_old = element.cod_tipo_dir; });
                    //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                }
            }

            return List_Mpersona_dir;
        }

        public List<Mpersona_telef> List_Mpersona_telef(int id_persona)
        {
            int rowcount = 1;
            List<Mpersona_telef> List_Mpersona_telef = new List<Mpersona_telef>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_PHONE", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Mpersona_telef = Dtl.ConvertTo<Mpersona_telef>(DsReturn.Tables[0]);
                    List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.Identity = rowcount++; element.State = 'R'; });
                    //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                    List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.cod_tipo_telef_old = element.cod_tipo_telef; });
                    //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                }
            }

            return List_Mpersona_telef;
        }

        public List<Frm_sarlaft_accionistas_asoc> List_Frm_sarlaft_accionistas_asoc(int id_persona)
        {
            int rowcount = 0;
            List<Frm_sarlaft_accionistas_asoc> List_Frm_sarlaft_accionistas_asoc = new List<Frm_sarlaft_accionistas_asoc>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GETSARLAFTSHAREHOLDERS", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Frm_sarlaft_accionistas_asoc = Dtl.ConvertTo<Frm_sarlaft_accionistas_asoc>(DsReturn.Tables[0]);
                    List_Frm_sarlaft_accionistas_asoc.ForEach(delegate(Frm_sarlaft_accionistas_asoc element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }

            return List_Frm_sarlaft_accionistas_asoc;
        }

        public List<Maseg_pmin_gastos_emi> List_Maseg_pmin_gastos_emi(int id_persona)
        {
            int rowcount = 0;
            List<Maseg_pmin_gastos_emi> List_Maseg_pmin_gastos_emi = new List<Maseg_pmin_gastos_emi>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GETMASEG_PMIN_GASTOS_EMI", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Maseg_pmin_gastos_emi = Dtl.ConvertTo<Maseg_pmin_gastos_emi>(DsReturn.Tables[0]);
                    List_Maseg_pmin_gastos_emi.ForEach(delegate(Maseg_pmin_gastos_emi element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }

            return List_Maseg_pmin_gastos_emi;
        }

        public List<Magente_producto> List_Magente_producto(int id_persona)
        {
            int rowcount = 0;
            List<Magente_producto> List_Magente_producto = new List<Magente_producto>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_MAGENTE_PRODUCTO", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Magente_producto = Dtl.ConvertTo<Magente_producto>(DsReturn.Tables[0]);
                    List_Magente_producto.ForEach(delegate(Magente_producto element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }

            return List_Magente_producto;
        }

        public List<Magente_organizador> List_Magente_organizador(int cod_rol)
        {
            int rowcount = 0;
            List<Magente_organizador> List_Magente_organizador = new List<Magente_organizador>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_MAGENTE_ORGANIZADOR", cod_rol, "cod_rol");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Magente_organizador = Dtl.ConvertTo<Magente_organizador>(DsReturn.Tables[0]);
                    List_Magente_organizador.ForEach(delegate(Magente_organizador element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }

            return List_Magente_organizador;
        }

        public List<Maseg_tasa_tarifa> List_Maseg_tasa_tarifa(int id_persona)
        {
            int rowcount = 0;
            List<Maseg_tasa_tarifa> List_Maseg_tasa_tarifa = new List<Maseg_tasa_tarifa>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GETMASEG_TASA_TARIFA", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Maseg_tasa_tarifa = Dtl.ConvertTo<Maseg_tasa_tarifa>(DsReturn.Tables[0]);
                    List_Maseg_tasa_tarifa.ForEach(delegate(Maseg_tasa_tarifa element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }

            return List_Maseg_tasa_tarifa;
        }

        public List<Maseg_asociacion> List_Maseg_asociacion(int id_persona)
        {
            int rowcount = 0;
            List<Maseg_asociacion> List_Maseg_asociacion = new List<Maseg_asociacion>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GETMASEG_ASOCIACION", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Maseg_asociacion = Dtl.ConvertTo<Maseg_asociacion>(DsReturn.Tables[0]);
                    List_Maseg_asociacion.ForEach(delegate(Maseg_asociacion element) { element.Identity = rowcount++; element.State = 'R'; });
                }

                if (ValidateExistInTable(id_persona, 5) > 0)
                {
                    List_Maseg_asociacion.ForEach(delegate(Maseg_asociacion element) { element.Identity = rowcount++; element.State_3G = 'R'; });
                }
                else
                {
                    List_Maseg_asociacion.ForEach(delegate(Maseg_asociacion element) { element.Identity = rowcount++; element.State_3G = 'C'; });
                }
            }
            return List_Maseg_asociacion;
        }

        public List<Referencias> List_Referencias(int cod_rol, int id_role)
        {
            List<Parameters> ListParameter = new List<Parameters>();

            Parameters parameter = new Parameters();
            parameter.NameTable = "Referencias";
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "cod_rol";
            parameter.Value = cod_rol.ToString();
            ListParameter.Add(parameter);

            parameter = new Parameters();
            parameter.NameTable = "Referencias";
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "id_role";
            parameter.Value = id_role.ToString();
            ListParameter.Add(parameter);

            int rowcount = 0;
            List<Referencias> List_Referencias = new List<Referencias>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_REFERENCES", ListParameter);
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Referencias = Dtl.ConvertTo<Referencias>(DsReturn.Tables[0]);
                    List_Referencias.ForEach(delegate(Referencias element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }
            return List_Referencias;
        }

        public DtoSarlaft dtoSarlaft(int id_persona)
        {
            int rowcount = 0;
            //DtoSarlaft dtoSarlaft = new DtoSarlaft();
            dtoSarlaftVar.List_detalle = new List<Frm_sarlaft_detalle>();
            dtoSarlaftVar.frm_sarlaft_vinculos = new Frm_sarlaft_vinculos();
            dtoSarlaftVar.frm_sarlaft_aut_incrementos = new Frm_sarlaft_aut_incrementos();

            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_SARLAFT_FORMS", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    dtoSarlaftVar.List_detalle = Dtl.ConvertTo<Frm_sarlaft_detalle>(DsReturn.Tables[0]);
                    dtoSarlaftVar.List_detalle.ForEach(delegate(Frm_sarlaft_detalle element) { element.Identity = rowcount++; element.State = 'R'; });
                    
                    dtoSarlaftForm(dtoSarlaftVar.List_detalle[0].id_formulario, id_persona);
                }
                if (DsReturn.Tables[1].Rows.Count > 0) { dtoSarlaftVar.frm_sarlaft_vinculos = Dtl.ConvertTo<Frm_sarlaft_vinculos>(DsReturn.Tables[1])[0]; }
                if (DsReturn.Tables[2].Rows.Count > 0) { dtoSarlaftVar.frm_sarlaft_aut_incrementos = Dtl.ConvertTo<Frm_sarlaft_aut_incrementos>(DsReturn.Tables[2])[0]; }

                if (ValidateExistInTable(id_persona, 2) > 0)
                {
                    dtoSarlaftVar.List_detalle.ForEach(delegate(Frm_sarlaft_detalle element) { element.Identity = rowcount++; element.State_3G = 'R'; });
                    dtoSarlaftVar.frm_sarlaft_vinculos.State_3G = 'R';
                    dtoSarlaftVar.frm_sarlaft_aut_incrementos.State_3G = 'R';
                }
                else
                {
                    dtoSarlaftVar.List_detalle.ForEach(delegate(Frm_sarlaft_detalle element) { element.Identity = rowcount++; element.State_3G = 'C'; });
                    dtoSarlaftVar.frm_sarlaft_vinculos.State_3G = 'C';
                    dtoSarlaftVar.frm_sarlaft_aut_incrementos.State_3G = 'C';
                }
            }
           
            return dtoSarlaftVar;
        }

        public DtoSarlaft dtoSarlaftForm(int ID_FORM)
        {
            int rowcount = 0;
            //DtoSarlaft dtoSarlaft = new DtoSarlaft();
            dtoSarlaftVar.frm_sarlaft_detalle_entrevista = new Frm_sarlaft_detalle_entrevista();
            dtoSarlaftVar.frm_sarlaft_info_financiera = new Frm_sarlaft_info_financiera();
            dtoSarlaftVar.List_oper_internacionales = new List<Frm_sarlaft_oper_internac>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_DETAILS_SARLAFT_FORMS", ID_FORM, "ID_FORM");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    if (DsReturn.Tables[0].Rows.Count > 0) { dtoSarlaftVar.frm_sarlaft_detalle_entrevista = Dtl.ConvertTo<Frm_sarlaft_detalle_entrevista>(DsReturn.Tables[0])[0]; }
                    if (DsReturn.Tables[1].Rows.Count > 0) { dtoSarlaftVar.frm_sarlaft_info_financiera = Dtl.ConvertTo<Frm_sarlaft_info_financiera>(DsReturn.Tables[1])[0]; }
                    if (DsReturn.Tables[2].Rows.Count > 0)
                    {
                        dtoSarlaftVar.List_oper_internacionales = Dtl.ConvertTo<Frm_sarlaft_oper_internac>(DsReturn.Tables[2]);
                        dtoSarlaftVar.List_oper_internacionales.ForEach(delegate(Frm_sarlaft_oper_internac element) { element.Identity = rowcount++; element.State = 'R'; });
                    }
                }
            }
            return dtoSarlaftVar;
        }

        public DtoSarlaft dtoSarlaftForm(int ID_FORM, int idPersona)
        {
            int rowcount = 0;
            //DtoSarlaft dtoSarlaft = new DtoSarlaft();
            dtoSarlaftVar.frm_sarlaft_detalle_entrevista = new Frm_sarlaft_detalle_entrevista();
            dtoSarlaftVar.frm_sarlaft_info_financiera = new Frm_sarlaft_info_financiera();
            dtoSarlaftVar.List_oper_internacionales = new List<Frm_sarlaft_oper_internac>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_DETAILS_SARLAFT_FORMS", ID_FORM, "ID_FORM");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    if (DsReturn.Tables[0].Rows.Count > 0) { dtoSarlaftVar.frm_sarlaft_detalle_entrevista = Dtl.ConvertTo<Frm_sarlaft_detalle_entrevista>(DsReturn.Tables[0])[0]; }
                    if (DsReturn.Tables[1].Rows.Count > 0) { dtoSarlaftVar.frm_sarlaft_info_financiera = Dtl.ConvertTo<Frm_sarlaft_info_financiera>(DsReturn.Tables[1])[0]; }
                    if (DsReturn.Tables[2].Rows.Count > 0)
                    {
                        dtoSarlaftVar.List_oper_internacionales = Dtl.ConvertTo<Frm_sarlaft_oper_internac>(DsReturn.Tables[2]);
                        dtoSarlaftVar.List_oper_internacionales.ForEach(delegate(Frm_sarlaft_oper_internac element) { element.Identity = rowcount++; element.State = 'R'; });
                    }
                }

                if (ValidateExistInTable(idPersona, 2) > 0)
                {
                    dtoSarlaftVar.List_oper_internacionales.ForEach(delegate(Frm_sarlaft_oper_internac element) { element.Identity = rowcount++; element.State_3G = 'R'; });
                    dtoSarlaftVar.frm_sarlaft_detalle_entrevista.State_3G = 'R';
                    dtoSarlaftVar.frm_sarlaft_info_financiera.State_3G = 'R';
                }
                else
                {
                    dtoSarlaftVar.List_oper_internacionales.ForEach(delegate(Frm_sarlaft_oper_internac element) { element.Identity = rowcount++; element.State_3G = 'C'; });
                    dtoSarlaftVar.frm_sarlaft_detalle_entrevista.State_3G = 'C';
                    dtoSarlaftVar.frm_sarlaft_info_financiera.State_3G = 'C';
                }
            }


            return dtoSarlaftVar;
        }

        public DtoDataPerson dtoDataPerson(int id_persona)
        {
            DtoDataPerson dtoDataPerson = new DtoDataPerson();
            dtoDataPerson.mpersona = new Mpersona();
            dtoDataPerson.maseg_header = new Maseg_header();
            dtoDataPerson.tipo_persona_aseg = new Tipo_persona_aseg();
            dtoDataPerson.maseg_ficha_tec_financ = new Maseg_ficha_tec_financ();
            dtoDataPerson.listMaseg_deporte = new List<Maseg_deporte>();

            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_DETAILS_PERSON_FORM", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataPerson.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[0])[0]; }
                    if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataPerson.maseg_header = Dtl.ConvertTo<Maseg_header>(DsReturn.Tables[1])[0]; }
                    if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataPerson.tipo_persona_aseg = Dtl.ConvertTo<Tipo_persona_aseg>(DsReturn.Tables[2])[0]; }
                    if (DsReturn.Tables[3].Rows.Count > 0) { dtoDataPerson.maseg_ficha_tec_financ = Dtl.ConvertTo<Maseg_ficha_tec_financ>(DsReturn.Tables[3])[0]; }
                    if (DsReturn.Tables[4].Rows.Count > 0) { dtoDataPerson.listMaseg_deporte = Dtl.ConvertTo<Maseg_deporte>(DsReturn.Tables[4]); }
                }
            }

            return dtoDataPerson;
        }

        public List<Mpersona_cuentas_bancarias> List_Mpersona_cuentas_bancarias(int id_persona)
        {
            int rowcount = 0;
            List<Mpersona_cuentas_bancarias> List_Mpersona_cuentas_bancarias = new List<Mpersona_cuentas_bancarias>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_BANKACCOUNT", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Mpersona_cuentas_bancarias = Dtl.ConvertTo<Mpersona_cuentas_bancarias>(DsReturn.Tables[0]);
                    List_Mpersona_cuentas_bancarias.ForEach(delegate(Mpersona_cuentas_bancarias element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }

            return List_Mpersona_cuentas_bancarias;
        }

        public List<Maseg_conducto> List_Maseg_conducto(int id_persona)
        {
            int rowcount = 0;
            List<Maseg_conducto> List_Maseg_conducto = new List<Maseg_conducto>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_DUCTS", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Maseg_conducto = Dtl.ConvertTo<Maseg_conducto>(DsReturn.Tables[0]);
                    List_Maseg_conducto.ForEach(delegate(Maseg_conducto element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }

            return List_Maseg_conducto;
        }

        bool reValidation = false;

        public int ValidateExistInTable(int individualId, int idTable)
        {
            DGeneric = new DataGenericExecute(PrefixConn2);
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "INDIVIDUAL_ID";
            parameter.Value = individualId.ToString();
            listParameter.Add(parameter);
            parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "ID_TABLE";
            parameter.Value = idTable.ToString();
            listParameter.Add(parameter);
            int retorno = DGeneric.ExecuteStoreProcedureScalar("SUP.GET_EXISTS_REGISTRY", listParameter);
            //Edward Rubiano -- 3504
            if (retorno == 0 && !reValidation)
            {
                reValidation = true;
                individualId = getIndividualID3g(individualId);
                retorno = ValidateExistInTable(individualId, idTable);
            }
            reValidation = false;
            //Edward Rubiano -- 3504
            return retorno;
        }

        //Autor: Edward Rubiano; Fecha: 13/04/2016; Asunto: C11226; Descripcion: Valida que si la persona existe o no en sise 3g para integrar los datos basicos
        public int ValidateExistPersonIn3G(int individualId)
        {
            DGeneric = new DataGenericExecute(PrefixConn);
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "INDIVIDUAL_ID";
            parameter.Value = individualId.ToString();
            listParameter.Add(parameter);
            return DGeneric.ExecuteStoreProcedureScalar("SUP.VALIDATE_EXISTS_PERSON_IN_3G", listParameter);
        }
        //Autor: Edward Rubiano; Fecha: 13/04/2016; Asunto: C11226; Descripcion: Valida que si la persona existe o no en sise 3g para integrar los datos basicos

        public DtoRep dtoRep(int id_persona)
        {
            int rowcount = 0;
            DtoRep dtoRep = new DtoRep();
            dtoRep.mpersona_rep_legal = new Mpersona_rep_legal();
            dtoRep.List_mpersona_rep_legal_dir = new List<Mpersona_rep_legal_dir>();
            List<Mpersona_rep_legal_dir> List_mpersona_rep_legal_dir = new List<Mpersona_rep_legal_dir>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_REPLEGAL", id_persona, "id_persona");

            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    dtoRep.mpersona_rep_legal = Dtl.ConvertTo<Mpersona_rep_legal>(DsReturn.Tables[0])[0];
                    DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_REPLEGALDIR", id_persona, "id_persona");
                    if (DsReturn.Tables.Count > 0)
                    {
                        if (DsReturn.Tables[0].Rows.Count > 0)
                        {
                            dtoRep.List_mpersona_rep_legal_dir = Dtl.ConvertTo<Mpersona_rep_legal_dir>(DsReturn.Tables[0]);
                            dtoRep.List_mpersona_rep_legal_dir.ForEach(delegate(Mpersona_rep_legal_dir element) { element.Identity = rowcount++; element.State = 'R'; });
                        }
                    }

                    if (ValidateExistInTable(id_persona, 3) > 0)
                    {
                        dtoRep.mpersona_rep_legal.State_3G = 'R';
                        dtoRep.List_mpersona_rep_legal_dir.ForEach(delegate(Mpersona_rep_legal_dir element) { element.Identity = rowcount++; element.State_3G = 'R'; });
                    }
                    else
                    {
                        dtoRep.mpersona_rep_legal.State_3G = 'C';
                        dtoRep.List_mpersona_rep_legal_dir.ForEach(delegate(Mpersona_rep_legal_dir element) { element.Identity = rowcount++; element.State_3G = 'C'; });
                    }
                }
            }

            return dtoRep;
        }

        public List<Mpersona_email> List_Mpersona_email(int id_persona)
        {
            int rowcount = 1;
            List<Mpersona_email> List_Mpersona_email = new List<Mpersona_email>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_EMAIL", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Mpersona_email = Dtl.ConvertTo<Mpersona_email>(DsReturn.Tables[0]);
                    List_Mpersona_email.ForEach(delegate(Mpersona_email element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }

            return List_Mpersona_email;
        }

        public List<Mcesio_trans_bancarias> List_Mcesio_trans_bancarias()
        {
            List<Mcesio_trans_bancarias> List_Mcesio_trans_bancarias = new List<Mcesio_trans_bancarias>();

            return List_Mcesio_trans_bancarias;
        }

        public List<Tusuario_limites> List_Tusuario_limites(string cod_rol)
        {
            int rowcount = 0;
            List<Tusuario_limites> List_Tusuario_limites = new List<Tusuario_limites>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_SUBSCRIPTIONLIMITS", cod_rol, "cod_usuario");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Tusuario_limites = Dtl.ConvertTo<Tusuario_limites>(DsReturn.Tables[0]);
                    List_Tusuario_limites.ForEach(delegate(Tusuario_limites element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }
            return List_Tusuario_limites;
        }

        public List<Tusuario_modulo_imputacion> List_Tusuario_modulo_imputacion(string cod_rol)
        {
            int rowcount = 0;
            List<Tusuario_modulo_imputacion> List_Tusuario_modulo_imputacion = new List<Tusuario_modulo_imputacion>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_IMPUTATION", cod_rol, "cod_usuario");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Tusuario_modulo_imputacion = Dtl.ConvertTo<Tusuario_modulo_imputacion>(DsReturn.Tables[0]);
                    List_Tusuario_modulo_imputacion.ForEach(delegate(Tusuario_modulo_imputacion element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }
            return List_Tusuario_modulo_imputacion;
        }

        public List<Tusuario_concepto> List_Tusuario_concepto(string cod_rol)
        {
            int rowcount = 0;
            List<Tusuario_concepto> List_Tusuario_concepto = new List<Tusuario_concepto>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_USERCONCEPTS", cod_rol, "cod_usuario");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Tusuario_concepto = Dtl.ConvertTo<Tusuario_concepto>(DsReturn.Tables[0]);
                    List_Tusuario_concepto.ForEach(delegate(Tusuario_concepto element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }
            return List_Tusuario_concepto;
        }

        public List<Tusuario_centro_costo> List_Tusuario_centro_costo(string cod_rol)
        {
            int rowcount = 0;
            List<Tusuario_centro_costo> List_Tusuario_centro_costo = new List<Tusuario_centro_costo>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_COSTCENTER", cod_rol, "cod_usuario");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Tusuario_centro_costo = Dtl.ConvertTo<Tusuario_centro_costo>(DsReturn.Tables[0]);
                    List_Tusuario_centro_costo.ForEach(delegate(Tusuario_centro_costo element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }
            return List_Tusuario_centro_costo;
        }

        public List<Tpj_usuarios_email> List_Tpj_usuarios_email(string cod_rol)
        {
            int rowcount = 0;
            List<Tpj_usuarios_email> List_Tpj_usuarios_email = new List<Tpj_usuarios_email>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_MAIL_LEGALPROC", cod_rol, "cod_usuario");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Tpj_usuarios_email = Dtl.ConvertTo<Tpj_usuarios_email>(DsReturn.Tables[0]);
                    List_Tpj_usuarios_email.ForEach(delegate(Tpj_usuarios_email element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }
            return List_Tpj_usuarios_email;
        }

        public List<Magente_comision> List_Magente_comision(int cod_rol)
        {
            int rowcount = 0;
            List<Magente_comision> List_Magente_comision = new List<Magente_comision>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_COMISSION", cod_rol, "cod_rol");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Magente_comision = Dtl.ConvertTo<Magente_comision>(DsReturn.Tables[0]);
                    List_Magente_comision.ForEach(delegate(Magente_comision element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }
            return List_Magente_comision;
        }

        public List<Magente_ramo> List_Magente_ramo(int cod_rol)
        {
            int rowcount = 0;
            List<Magente_ramo> List_Magente_ramo = new List<Magente_ramo>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_ENABLED_BRANCHS", cod_rol, "cod_rol");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Magente_ramo = Dtl.ConvertTo<Magente_ramo>(DsReturn.Tables[0]);
                    List_Magente_ramo.ForEach(delegate(Magente_ramo element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }
            return List_Magente_ramo;
        }

        public List<Mpersona_actividad> List_Mpersona_actividad(int id_persona)
        {
            int rowcount = 0;
            List<Mpersona_actividad> List_Mpersona_actividad = new List<Mpersona_actividad>();
            DGeneric = new DataGenericExecute(PrefixConn);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_PERSON_ACTIVITIES", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_Mpersona_actividad = Dtl.ConvertTo<Mpersona_actividad>(DsReturn.Tables[0]);
                    List_Mpersona_actividad.ForEach(delegate(Mpersona_actividad element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }
            return List_Mpersona_actividad;
        }

        public DtoTechnicalCard dtoTechnicalCard(int id_persona)
        {
            DtoTechnicalCard dtoTechnicalCard_2g = new DtoTechnicalCard();
            dtoTechnicalCard_2g.TECHNICAL_CARD = new TECHNICAL_CARD();
            dtoTechnicalCard_2g.List_BOARD_DIRECTORS = new List<BOARD_DIRECTORS>();
            dtoTechnicalCard_2g.List_FINANCIAL_STATEMENTS = new List<FINANCIAL_STATEMENTS>();
            dtoTechnicalCard_2g.List_TECHNICAL_CARD_DESCRIPTION = new List<TECHNICAL_CARD_DESCRIPTION>();

            DGeneric = new DataGenericExecute(PrefixConn);
        
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_DETAILS_TECHNICAL_CARD", id_persona, "id_persona");

            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    //Edward Rubiano -- HD3488 -- 30/10/2015
                    dtoTechnicalCard_2g.TECHNICAL_CARD = Dtl.ConvertTo<TECHNICAL_CARD>(DsReturn.Tables[0])[0];
                    //if (DsReturn.Tables[0].Rows.Count > 0) { dtoTechnicalCard_2g.TECHNICAL_CARD = Dtl.ConvertTo<TECHNICAL_CARD>(DsReturn.Tables[0])[0]; }
		        }
                if (DsReturn.Tables[1].Rows.Count > 0)
                {		
                    dtoTechnicalCard_2g.List_BOARD_DIRECTORS = Dtl.ConvertTo<BOARD_DIRECTORS>(DsReturn.Tables[1]);
                    //if (DsReturn.Tables[1].Rows.Count > 0) { dtoTechnicalCard_2g.List_BOARD_DIRECTORS = Dtl.ConvertTo<BOARD_DIRECTORS>(DsReturn.Tables[1]); }
                }
                if (DsReturn.Tables[2].Rows.Count > 0)
                {
                    dtoTechnicalCard_2g.List_FINANCIAL_STATEMENTS = Dtl.ConvertTo<FINANCIAL_STATEMENTS>(DsReturn.Tables[2]);
                    //if (DsReturn.Tables[2].Rows.Count > 0) { dtoTechnicalCard_2g.List_FINANCIAL_STATEMENTS = Dtl.ConvertTo<FINANCIAL_STATEMENTS>(DsReturn.Tables[2]); }
                }
                if (DsReturn.Tables[3].Rows.Count > 0)
                {
                    dtoTechnicalCard_2g.List_TECHNICAL_CARD_DESCRIPTION = Dtl.ConvertTo<TECHNICAL_CARD_DESCRIPTION>(DsReturn.Tables[3]);
                    //if (DsReturn.Tables[3].Rows.Count > 0) { dtoTechnicalCard_2g.List_TECHNICAL_CARD_DESCRIPTION = Dtl.ConvertTo<TECHNICAL_CARD_DESCRIPTION>(DsReturn.Tables[3]); }
                    //Edward Rubiano -- 30/10/2015
                }
            }


            //3g
            DtoTechnicalCard dtoTechnicalCard = new DtoTechnicalCard();
            dtoTechnicalCard.TECHNICAL_CARD = new TECHNICAL_CARD();
            dtoTechnicalCard.List_BOARD_DIRECTORS = new List<BOARD_DIRECTORS>();
            dtoTechnicalCard.List_FINANCIAL_STATEMENTS = new List<FINANCIAL_STATEMENTS>();
            dtoTechnicalCard.List_TECHNICAL_CARD_DESCRIPTION = new List<TECHNICAL_CARD_DESCRIPTION>();
            int rowcount = 0;

            DGeneric = new DataGenericExecute(PrefixConn2);

            DataSet DsReturn_3g = DGeneric.ExecuteStoreProcedure("SUP.GET_DETAILS_TECHNICAL_CARD", id_persona, "id_persona");


            if (DsReturn_3g.Tables.Count > 0)
            {
                if (DsReturn_3g.Tables[0].Rows.Count > 0)
                {
                    if (DsReturn_3g.Tables[0].Rows.Count > 0) { dtoTechnicalCard.TECHNICAL_CARD = Dtl.ConvertTo<TECHNICAL_CARD>(DsReturn_3g.Tables[0])[0]; }
                    if (DsReturn_3g.Tables[1].Rows.Count > 0) { dtoTechnicalCard.List_BOARD_DIRECTORS = Dtl.ConvertTo<BOARD_DIRECTORS>(DsReturn_3g.Tables[1]); }
                    if (DsReturn_3g.Tables[2].Rows.Count > 0) { dtoTechnicalCard.List_FINANCIAL_STATEMENTS = Dtl.ConvertTo<FINANCIAL_STATEMENTS>(DsReturn_3g.Tables[2]); }
                    if (DsReturn_3g.Tables[3].Rows.Count > 0) { dtoTechnicalCard.List_TECHNICAL_CARD_DESCRIPTION = Dtl.ConvertTo<TECHNICAL_CARD_DESCRIPTION>(DsReturn_3g.Tables[3]); }
                }
            }

            DtoTechnicalCard dtoTechnicalCardr = new DtoTechnicalCard();
            dtoTechnicalCardr.List_BOARD_DIRECTORS = new List<BOARD_DIRECTORS>();
            dtoTechnicalCardr.List_FINANCIAL_STATEMENTS = new List<FINANCIAL_STATEMENTS>();
            dtoTechnicalCardr.List_TECHNICAL_CARD_DESCRIPTION = new List<TECHNICAL_CARD_DESCRIPTION>();


            //Edward Rubiano -- HD3488 -- 30/10/2015
            //if (dtoTechnicalCard_2g.TECHNICAL_CARD.AUTHORIZED_CAPITAL_AMT != 0.0)
            if (DsReturn.Tables[0].Rows.Count > 0)
            //Edward Rubiano -- 30/10/2015
            {
              dtoTechnicalCardr.TECHNICAL_CARD = dtoTechnicalCard_2g.TECHNICAL_CARD;
            }else{
              dtoTechnicalCardr.TECHNICAL_CARD = dtoTechnicalCard.TECHNICAL_CARD;
            }

            var financial = dtoTechnicalCard_2g.List_FINANCIAL_STATEMENTS.OrderBy(a => a.BALANCE_DATE).ToList();
            var union = financial.Union(dtoTechnicalCard.List_FINANCIAL_STATEMENTS).GroupBy(a => a.BALANCE_DATE).Select(g => g.First(d => d.BALANCE_DATE == d.BALANCE_DATE)).ToList();

            if (union.Count() != 0)
            {
                foreach (FINANCIAL_STATEMENTS FINANCIAL in union)
                {
                    FINANCIAL.State = 'R';
                }
                dtoTechnicalCardr.List_FINANCIAL_STATEMENTS = union;
            }

            var orden = dtoTechnicalCard_2g.List_BOARD_DIRECTORS.OrderBy(a => a.BOARD_DIRECTORS_CD).ToList();
            var comparar = orden.Union(dtoTechnicalCard.List_BOARD_DIRECTORS).GroupBy(a => a.BOARD_DIRECTORS_CD).Select(g => g.First(d => d.BOARD_DIRECTORS_CD == d.BOARD_DIRECTORS_CD && d.BOARD_MEMBER_NAME == d.BOARD_MEMBER_NAME)).ToList();           
           
            //unificacion
            if (comparar.Count() != 0)
            {
                foreach (BOARD_DIRECTORS board in comparar)
                {
                    board.State = 'R';
                    board.State3G = 'R';

                }
                dtoTechnicalCardr.List_BOARD_DIRECTORS = comparar;
            }

            var result = dtoTechnicalCard_2g.List_TECHNICAL_CARD_DESCRIPTION.OrderBy(a => a.TECHNICAL_CARD_DESCRIPTION_CD).ToList();
            var observacion = result.Union(dtoTechnicalCard.List_TECHNICAL_CARD_DESCRIPTION).GroupBy(a => a.TECHNICAL_CARD_DESCRIPTION_CD).Select(g => g.First(d => d.TECHNICAL_CARD_DESCRIPTION_CD == d.TECHNICAL_CARD_DESCRIPTION_CD && d.DESCRIPTION == d.DESCRIPTION)).ToList();
            if (observacion.Count() != 0)
            {
                //TODO: Sergio Bautista, 27/04/2017, PRE-1223: Comento ya que el user id lo recupera de 3G con esta variable
                //var aumento = observacion.Count() - 1 ;
                var aumento = dtoTechnicalCard.List_TECHNICAL_CARD_DESCRIPTION.Count -1;  
                
                foreach (TECHNICAL_CARD_DESCRIPTION card in observacion)
                {
                    card.State = 'R';
                    card.State3G = 'R';
                    
                    //TODO: Sergio Bautista, 27/04/2017, PRE-1223
                    //if (dtoTechnicalCard.List_TECHNICAL_CARD_DESCRIPTION.Count != 0) ;
                    if (dtoTechnicalCard.List_TECHNICAL_CARD_DESCRIPTION.Count != 0 && aumento >= 0)
                    {
                        card.USER_ID = dtoTechnicalCard.List_TECHNICAL_CARD_DESCRIPTION[aumento].USER_ID;
                        aumento--;
                    }
                }

                dtoTechnicalCardr.List_TECHNICAL_CARD_DESCRIPTION = observacion;
            }

            if (ValidateExistInTable(id_persona, 1) > 0)
            {
                dtoTechnicalCardr.TECHNICAL_CARD.State_3G = 'R';
                dtoTechnicalCardr.List_BOARD_DIRECTORS.ForEach(delegate(BOARD_DIRECTORS element) { element.Identity = rowcount++; element.State3G = 'R'; });
                dtoTechnicalCardr.List_FINANCIAL_STATEMENTS.ForEach(delegate(FINANCIAL_STATEMENTS element) { element.Identity = rowcount++; element.State_3G = 'R'; });
                dtoTechnicalCardr.List_TECHNICAL_CARD_DESCRIPTION.ForEach(delegate(TECHNICAL_CARD_DESCRIPTION element) { element.Identity = rowcount++; element.State3G = 'R'; });

            }
            else
            {
                dtoTechnicalCardr.TECHNICAL_CARD.State_3G = 'C';
                dtoTechnicalCardr.List_BOARD_DIRECTORS.ForEach(delegate(BOARD_DIRECTORS element) { element.Identity = rowcount++; element.State3G = 'C'; });
                dtoTechnicalCardr.List_FINANCIAL_STATEMENTS.ForEach(delegate(FINANCIAL_STATEMENTS element) { element.Identity = rowcount++; element.State_3G = 'C'; });
                dtoTechnicalCardr.List_TECHNICAL_CARD_DESCRIPTION.ForEach(delegate(TECHNICAL_CARD_DESCRIPTION element) { element.Identity = rowcount++; element.State3G = 'C'; });
            }

            if (dtoTechnicalCardr != null)
            {
                return dtoTechnicalCardr;
            }
            else
            {
                return dtoTechnicalCard_2g;
            }
        }

        //Informacion 3g

        public int getIndividualID3g(int idPersona)
        {
            DGeneric = new DataGenericExecute(PrefixConn);

            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "INDIVIDUAL_2G";
            parameter.Value = idPersona.ToString();
            listParameter.Add(parameter);

            return DGeneric.ExecuteStoreProcedureScalar("SUP.GET_ID_PERSON_3G", listParameter);
        }


        //Edward Rubiano -- HD3519 -- 04/11/2015
        //Trae el INDIVIDUAL_ID de la base de datos 3g por numero de documento y tipo de documento
        public int getIndividualID3g(string idCardTypeCD, string idCardNoOrTributaryIdNo, string codTipoPersona)
        {
            DGeneric = new DataGenericExecute(PrefixConn2);
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "ID_CARD_TYPE_CD";
            parameter.Value = idCardTypeCD;
            listParameter.Add(parameter);
            parameter = new Parameters();
            parameter.ParameterType = "AseDbType.VarChar";
            parameter.Parameter = "ID_CARD_NO_OR_TRIBUTARY_ID_NO";
            parameter.Value = idCardNoOrTributaryIdNo;
            listParameter.Add(parameter);
            parameter = new Parameters();
            parameter.ParameterType = "AseDbType.VarChar";
            parameter.Parameter = "COD_TYPE_PERSON";
            parameter.Value = codTipoPersona;
            listParameter.Add(parameter);
            int retorno = DGeneric.ExecuteStoreProcedureScalar("SUP.GET_INDIVIDUAL_ID", listParameter);
            return retorno;
        }

        //Trae el INSURED_CD real de la base de datos 3g
        public int getInsuredCD3g(int individualId)
        {
            DGeneric = new DataGenericExecute(PrefixConn2);
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "INDIVIDUAL_ID";
            parameter.Value = individualId.ToString();
            listParameter.Add(parameter);
            int retorno = DGeneric.ExecuteStoreProcedureScalar("SUP.GET_INSURED_CD", listParameter);
            return retorno;
        }
        //Edward Rubiano -- 04/11/2015

        public DtoDataPerson dtoDataPerson3g(int id_persona, DtoInsured dtoInsured)
        {
            DGeneric = new DataGenericExecute(PrefixConn2);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_PERSON_DATA", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    dtoInsured.dtoDataPerson.mpersona.SPOUSE_NAME = DsReturn.Tables[0].Rows[0][0].ToString();
                    dtoInsured.dtoDataPerson.mpersona.EDUCATIVE_LEVEL_CD = DsReturn.Tables[0].Rows[0][1].ToString();
                    dtoInsured.dtoDataPerson.mpersona.CHILDREN = DsReturn.Tables[0].Rows[0][2].ToString();
                    dtoInsured.dtoDataPerson.mpersona.SOCIAL_LAYER_CD = DsReturn.Tables[0].Rows[0][3].ToString();
                    dtoInsured.dtoDataPerson.mpersona.HOUSE_TYPE_CD = DsReturn.Tables[0].Rows[0][4].ToString();
                    dtoInsured.dtoDataPerson.mpersona.INCOME_LEVEL_CD = DsReturn.Tables[0].Rows[0][5].ToString();
                    dtoInsured.dtoDataPerson.mpersona.SPECIALITY_CD = DsReturn.Tables[0].Rows[0][6].ToString();
                    dtoInsured.dtoDataPerson.mpersona.COMPANY_PHONE = DsReturn.Tables[0].Rows[0][7].ToString();
                }
            }

            return dtoInsured.dtoDataPerson;
        }

        public List<Mpersona_dir> dtoDataAddress3g(int id_persona, List<Mpersona_dir> List_ADDRESS_2G)
        {
            //List_ADDRESS_2G.ForEach(delegate(Mpersona_dir p) { p.State_3g = 'C'; p.State = 'U'; });
            List_ADDRESS_2G.ForEach(delegate(Mpersona_dir p) { p.State_3g = 'C'; });
            //Edward Rubiano -- HD3521 -- 04/11/2015            
            // << TODO:Sergio Bautista, 30/10/2017, PRE-1735: Modifico ya que vuelve a recuperar la equivalencia de 3g con el mismo id de 3g
              ////Es asegurado
                //if (IdRol == 1)
                //{
                //    if (getIndividualID3g(id_persona) != 0)
                //    {
                //        id_persona = getIndividualID3g(id_persona);
                //    }
                //}
              ////Edward Rubiano -- 04/11/2015
           //TODO: Fin, Sergio Bautista, 30/10/2017

            DGeneric = new DataGenericExecute(PrefixConn2);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_ADDRESS_DATA", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List<ADDRESS> List_ADDRESS_3G = Dtl.ConvertTo<ADDRESS>(DsReturn.Tables[0]);

                    foreach (ADDRESS dir_3g in List_ADDRESS_3G)
                    {
                        //Edward Rubiano -- HD3521 -- 04/11/2015
                        //Mpersona_dir dir_2g_Found = List_ADDRESS_2G.Find(delegate(Mpersona_dir p) { return p.id_persona == dir_3g.INDIVIDUAL_ID && p.cod_tipo_dir == dir_3g.ADDRESS_TYPE_CD; });
                        //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Funcion de guardado de datos direccion en sise 3g
                        //Mpersona_dir dir_2g_Found = List_ADDRESS_2G.Find(delegate(Mpersona_dir p) { return p.cod_tipo_dir == dir_3g.ADDRESS_TYPE_CD; });
                        Mpersona_dir dir_2g_Found = List_ADDRESS_2G.Find(delegate(Mpersona_dir p) { return p.cod_tipo_dir_old == dir_3g.ADDRESS_TYPE_CD; });
                        if (dir_2g_Found == null)
                        {
                            dir_2g_Found = List_ADDRESS_2G.Find(delegate(Mpersona_dir p) { return p.cod_tipo_dir == dir_3g.ADDRESS_TYPE_CD; });
                        }
                        //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Funcion de guardado de datos direccion en sise 3g
                        //Edward Rubiano -- 04/11/2015
                        if (dir_2g_Found != null)
                        {
                            dir_2g_Found.IS_MAILING_ADDRESS = dir_3g.IS_MAILING_ADDRESS;
                            dir_2g_Found.DATA_ID = dir_3g.DATA_ID;

                            dir_2g_Found.State = 'R';
                            //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: validar si fue modificado el tipo de direccion en 3g
                            if (dir_2g_Found.cod_tipo_dir != dir_2g_Found.cod_tipo_dir_old)
                            {
                                dir_2g_Found.State_3g = 'U';
                            }
                            else
                            {
                                dir_2g_Found.State_3g = 'R'; 
                            }
                            //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: validar si fue modificado el tipo de direccion en 3g

                            //dir_2g_Found.Identity = dir_3g.DATA_ID;
                        }

                        else
                        {
                            Mpersona_dir dirToAdd = new Mpersona_dir();

                            dirToAdd.id_persona = dir_3g.INDIVIDUAL_ID;
                            dirToAdd.cod_tipo_dir = dir_3g.ADDRESS_TYPE_CD;
                            dirToAdd.IS_MAILING_ADDRESS = dir_3g.IS_MAILING_ADDRESS;
                            dirToAdd.txt_direccion = dir_3g.STREET;
                            dirToAdd.cod_municipio = Convert.ToDouble(dir_3g.CITY_CD);
                            dirToAdd.nro_cod_postal = dir_3g.ZIP_CODE;
                            dirToAdd.cod_dpto = Convert.ToDouble(dir_3g.STATE_CD);
                            dirToAdd.cod_pais = Convert.ToDouble(dir_3g.COUNTRY_CD);
                            dirToAdd.IS_HOME = dir_3g.IS_HOME;

                            dirToAdd.DATA_ID = dir_3g.DATA_ID;
                            dirToAdd.Identity = dir_3g.DATA_ID;

                            dirToAdd.State = 'C';
                            dirToAdd.State_3g = 'R';

                            List_ADDRESS_2G.Add(dirToAdd);
                        }
                    }
                }
            }

            return (List<Mpersona_dir>)List_ADDRESS_2G.OrderBy(x => x.DATA_ID).ToList();
        }

        public List<Mpersona_telef> dtoDataPhone3g(int id_persona, List<Mpersona_telef> List_PHONE_2G)
        {
            //List_PHONE_2G.ForEach(delegate(Mpersona_telef t) { t.State_3g = 'C'; t.State = 'U'; });
            List_PHONE_2G.ForEach(delegate(Mpersona_telef t) { t.State_3g = 'C'; });
            //Edward Rubiano -- HD3521 -- 04/11/2015
            // << TODO:Sergio Bautista, 30/10/2017, PRE-1735: Modifico ya que vuelve a recuperar la equivalencia de 3g con el mismo id de 3g
                ////Es asegurado
                //if (IdRol == 1)
                //{ 
                //    if (getIndividualID3g(id_persona) != 0)
                //    {
                //        id_persona = getIndividualID3g(id_persona);
                //    }
                //}
                ////Edward Rubiano -- 04/11/2015
            //TODO: Fin, Sergio Bautista, 30/10/2017
            DGeneric = new DataGenericExecute(PrefixConn2);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_PHONE_DATA", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List<PHONE> List_PHONE_3G = Dtl.ConvertTo<PHONE>(DsReturn.Tables[0]);

                    foreach (PHONE tel_3g in List_PHONE_3G)
                    {
                        //Edward Rubiano -- HD3521 -- 04/11/2015
                        //Mpersona_telef tel_2g_Found = List_PHONE_2G.Find(delegate(Mpersona_telef p) { return p.id_persona == tel_3g.INDIVIDUAL_ID && p.cod_tipo_telef == tel_3g.PHONE_TYPE_CD; });
                        //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Funcion de guardado de datos telefono en sise 3g
                        //Mpersona_telef tel_2g_Found = List_PHONE_2G.Find(delegate(Mpersona_telef p) { return p.cod_tipo_telef == tel_3g.PHONE_TYPE_CD; });
                        Mpersona_telef tel_2g_Found = List_PHONE_2G.Find(delegate(Mpersona_telef p) { return p.cod_tipo_telef_old == tel_3g.PHONE_TYPE_CD; });
                        if (tel_2g_Found == null)
                        {
                            tel_2g_Found = List_PHONE_2G.Find(delegate(Mpersona_telef p) { return p.cod_tipo_telef == tel_3g.PHONE_TYPE_CD; });
                        }
                        //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Funcion de guardado de datos telefono en sise 3g
                        //Edward Rubiano -- 04/11/2015
                        if (tel_2g_Found != null)
                        {
                            tel_2g_Found.EXTENSION = tel_3g.EXTENSION;
                            tel_2g_Found.SCHEDULE_AVAILABILITY = tel_3g.SCHEDULE_AVAILABILITY;
                            tel_2g_Found.DATA_ID = tel_3g.DATA_ID;

                            tel_2g_Found.State = 'R';
                            //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: validar si fue modificado el tipo de telefono en 3g
                            if (tel_2g_Found.cod_tipo_telef != tel_2g_Found.cod_tipo_telef_old)
                            {
                                tel_2g_Found.State_3g = 'U';
                            }
                            else
                            {
                                tel_2g_Found.State_3g = 'R';
                            }
                            //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: validar si fue modificado el tipo de telefono en 3g

                            //tel_2g_Found.Identity = tel_3g.DATA_ID;
                        }

                        else
                        {
                            Mpersona_telef telToAdd = new Mpersona_telef();

                            telToAdd.id_persona = tel_3g.INDIVIDUAL_ID;
                            telToAdd.cod_tipo_telef = tel_3g.PHONE_TYPE_CD;
                            telToAdd.txt_telefono = tel_3g.PHONE_NUMBER.ToString();
                            telToAdd.EXTENSION = tel_3g.EXTENSION;
                            telToAdd.SCHEDULE_AVAILABILITY = tel_3g.SCHEDULE_AVAILABILITY;
                            telToAdd.cod_pais = Convert.ToString(tel_3g.COUNTRY_CD);
                            telToAdd.cod_dpto = Convert.ToString(tel_3g.STATE_CD);
                            telToAdd.cod_municipio = Convert.ToString(tel_3g.CITY_CD);
                            telToAdd.IS_HOME = tel_3g.IS_HOME;

                            telToAdd.Identity = tel_3g.DATA_ID;
                            telToAdd.DATA_ID = tel_3g.DATA_ID;

                            telToAdd.State = 'C';
                            telToAdd.State_3g = 'R';

                            List_PHONE_2G.Add(telToAdd);
                        }
                    }
                }
            }
            return (List<Mpersona_telef>)List_PHONE_2G.OrderBy(x => x.DATA_ID).ToList();
        }

        public List<Mpersona_email> dtoDataMail3g(int id_persona, List<Mpersona_email> List_EMAIL_2G)
        {
            //List_EMAIL_2G.ForEach(delegate(Mpersona_email e) { e.State_3g = 'C'; e.State = 'U'; });
            List_EMAIL_2G.ForEach(delegate(Mpersona_email e) { e.State_3g = 'C'; });
            //Edward Rubiano -- HD3521 -- 04/11/2015
            // << TODO:Sergio Bautista, 30/10/2017, PRE-1735: Modifico ya que vuelve a recuperar la equivalencia de 3g con el mismo id de 3g
                ////Es asegurado
                //if (IdRol == 1)
                //{
                //    if (getIndividualID3g(id_persona) != 0)
                //    {
                //        id_persona = getIndividualID3g(id_persona);
                //    }
                //}
                ////Edward Rubiano -- 04/11/2015
            //TODO: Fin, Sergio Bautista, 30/10/2017

            DGeneric = new DataGenericExecute(PrefixConn2);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_MAIL_DATA", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List<EMAIL> List_EMAIL_3G = Dtl.ConvertTo<EMAIL>(DsReturn.Tables[0]);

                    foreach (EMAIL email_3g in List_EMAIL_3G)
                    {
                        //Edward Rubiano -- HD3521 -- 04/11/2015
                        //Mpersona_email email_2g_Found = List_EMAIL_2G.Find(delegate(Mpersona_email p) { return p.id_persona == email_3g.INDIVIDUAL_ID && p.cod_tipo_email == email_3g.EMAIL_TYPE_CD; });
                        Mpersona_email email_2g_Found = List_EMAIL_2G.Find(delegate(Mpersona_email p) { return p.cod_tipo_email == email_3g.EMAIL_TYPE_CD; });
                        //Edward Rubiano -- 04/11/2015
                        if (email_2g_Found != null)
                        {
                            email_2g_Found.IS_MAILING_ADDRESS = email_3g.IS_MAILING_ADDRESS;
                            email_2g_Found.DATA_ID = email_3g.DATA_ID;

                            email_2g_Found.State = 'R';
                            email_2g_Found.State_3g = 'R';

                            //email_2g_Found.Identity = email_3g.DATA_ID;
                        }

                        else
                        {
                            Mpersona_email emailToAdd = new Mpersona_email();

                            emailToAdd.id_persona = email_3g.INDIVIDUAL_ID;
                            emailToAdd.txt_direccion_email = email_3g.ADDRESS;
                            emailToAdd.cod_tipo_email = email_3g.EMAIL_TYPE_CD;
                            emailToAdd.IS_MAILING_ADDRESS = email_3g.IS_MAILING_ADDRESS;

                            emailToAdd.DATA_ID = email_3g.DATA_ID;
                            emailToAdd.Identity = email_3g.DATA_ID;

                            emailToAdd.State = 'C';
                            emailToAdd.State_3g = 'R';

                            List_EMAIL_2G.Add(emailToAdd);
                        }
                    }
                }
            }

            return (List<Mpersona_email>)List_EMAIL_2G.OrderBy(x => x.DATA_ID).ToList();
        }

        public Mpersona dtoData3g(int id_persona, Mpersona mpersona)
        {
            DGeneric = new DataGenericExecute(PrefixConn2);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_COMPANY", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    mpersona.VERIFY_DIGIT = DsReturn.Tables[0].Rows[0][0].ToString();
                    mpersona.ASSOCIATION_TYPE_CD = DsReturn.Tables[0].Rows[0][1].ToString();
                    mpersona.COMPANY_TYPE_CD = DsReturn.Tables[0].Rows[0][2].ToString();
                    mpersona.CATEGORY_CD = DsReturn.Tables[0].Rows[0][3].ToString();
                }
            }

            return mpersona;
        }

        public DtoDataInsured dtoInsuredData3g(int id_persona, DtoDataInsured dtoDataInsured)
        {
            DGeneric = new DataGenericExecute(PrefixConn2);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_INSURED_DATA", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                    dtoDataInsured.maseg_header.BRANCH_ID = DsReturn.Tables[0].Rows[0][0].ToString();
            }

            return dtoDataInsured;
        }

        public List<AGENT_AGENCY> List_AGENT_AGENCY(int id_persona)
        {
            int rowcount = 0;
            List<AGENT_AGENCY> List_AGENT_AGENCY = new List<AGENT_AGENCY>();
            DGeneric = new DataGenericExecute(PrefixConn2);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_AGENCY", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_AGENT_AGENCY = Dtl.ConvertTo<AGENT_AGENCY>(DsReturn.Tables[0]);
                    List_AGENT_AGENCY.ForEach(delegate(AGENT_AGENCY element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }
            return List_AGENT_AGENCY;
        }

        public List<INDIVIDUAL_TAX_EXEMPTION> List_INDIVIDUAL_TAX_EXEMPTION(int id_persona)
        {
            int rowcount = 0;
            List<INDIVIDUAL_TAX_EXEMPTION> List_INDIVIDUAL_TAX_EXEMPTION = new List<INDIVIDUAL_TAX_EXEMPTION>();
            DGeneric = new DataGenericExecute(PrefixConn2);
            DataSet DsReturn = DGeneric.ExecuteStoreProcedure("SUP.GET_TAX_EXEMPTION", id_persona, "id_persona");
            if (DsReturn.Tables.Count > 0)
            {
                if (DsReturn.Tables[0].Rows.Count > 0)
                {
                    List_INDIVIDUAL_TAX_EXEMPTION = Dtl.ConvertTo<INDIVIDUAL_TAX_EXEMPTION>(DsReturn.Tables[0]);
                    List_INDIVIDUAL_TAX_EXEMPTION.ForEach(delegate(INDIVIDUAL_TAX_EXEMPTION element) { element.Identity = rowcount++; element.State = 'R'; });
                }
            }
            return List_INDIVIDUAL_TAX_EXEMPTION;
        }
        #endregion
    }
}
