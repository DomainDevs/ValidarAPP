using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary;
using System.Data.Odbc;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.SqlClient;
using Sistran.Co.Application.Data;
using Sistran.Co.Previsora.Application.FullServices.Models;
using Sybase.Data.AseClient;
using System.Configuration;
using Sistran.Co.Previsora.Application.FullServicesProvider.DataLayer;
using Sistran.Co.Previsora.Application.FullServices.DTOs;
using Sistran.Co.Previsora.Application.FullServicesProvider.Helpers;
using Sistran.Co.Previsora.Application.FullServicesProvider.Provider;
/// <summary>
/// Summary description for DataHelper
/// </summary>

public class FullServicesHelper
{
    private string PrefixConn = ConfigurationManager.AppSettings["SUPAPP"];

    public FullServicesHelper()
    {

    }

    public DataSet ExecuteETLSistranFunction1(EntityExampleRequest entityExampleRequest, DynamicDataAccess pdb)
    {
        try
        {
            NameValue[] paramsp = new NameValue[3];
            paramsp[0] = new NameValue("contador", entityExampleRequest.Campo1);
            paramsp[1] = new NameValue("cod_tipo_agente", entityExampleRequest.Campo2);
            paramsp[2] = new NameValue("cod_agente", entityExampleRequest.Campo3);
            return pdb.ExecuteSPSerDataSetNoTransaction("sp_buscar_agentes", paramsp);
        }
        catch
        {
            throw;
        }
    }

    public DataSet ExecuteETLSistranFunction2(EntityExampleRequest entityExampleRequest, DynamicDataAccess pdb)
    {
        try
        {
            NameValue[] paramsp = new NameValue[3];
            paramsp[0] = new NameValue("contador", entityExampleRequest.Campo1);
            paramsp[1] = new NameValue("cod_tipo_agente", entityExampleRequest.Campo2);
            paramsp[2] = new NameValue("cod_agente", entityExampleRequest.Campo3);
            return pdb.ExecuteSPSerDataSetNoTransaction("sp_buscar_agentes", paramsp);
        }
        catch
        {
            throw;
        }
    }
    public DtoMaster GetExitsPerson(int id_rol, String id_persona)
    {
        try
        {
            DatatableToList Dtl = new DatatableToList();
            DtoMaster ReturnData = new DtoMaster();
            DataGenericExecute dge = new DataGenericExecute(PrefixConn);
            List<Parameters> ListParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "id_persona";
            parameter.Value = id_persona;
            ListParameter.Add(parameter);
            parameter = new Parameters();
            parameter.ParameterType = "AseDbType.Integer";
            parameter.Parameter = "id_rol";
            parameter.Value = id_rol.ToString();
            ListParameter.Add(parameter);
            DataSet DsReturn = dge.ExecuteStoreProcedure("SUP.GET_EXIST_PERSON", ListParameter);

            DtoDataPerson dtoDataPerson = new DtoDataPerson();
            dtoDataPerson.listMaseg_deporte = new List<Maseg_deporte>();
            dtoDataPerson.maseg_ficha_tec_financ = new Maseg_ficha_tec_financ();
            dtoDataPerson.tipo_persona_aseg = new Tipo_persona_aseg();

            DtoSarlaft dtoSarlaft = new DtoSarlaft();
            dtoSarlaft.List_detalle = new List<Frm_sarlaft_detalle>();
            dtoSarlaft.frm_sarlaft_vinculos = new Frm_sarlaft_vinculos();
            dtoSarlaft.frm_sarlaft_aut_incrementos = new Frm_sarlaft_aut_incrementos();

            DtoRep dtorep = new DtoRep();
            dtorep.mpersona_rep_legal = new Mpersona_rep_legal();
            dtorep.List_mpersona_rep_legal_dir = new List<Mpersona_rep_legal_dir>();

            GetDto datos = new GetDto(id_rol);
            switch (id_rol)
            {
                case 1:
                    DtoInsured dtoInsured = new DtoInsured();
                    dtoInsured.List_Mpersona_dir = new List<Mpersona_dir>();
                    dtoInsured.List_Mpersona_telef = new List<Mpersona_telef>();
                    dtoInsured.List_Mpersona_email = new List<Mpersona_email>();
                    dtoInsured.List_Logbook = new List<LOGBOOK>();
                    dtoInsured.List_Frm_sarlaft_accionistas_asoc = new List<Frm_sarlaft_accionistas_asoc>();
                    dtoInsured.List_Maseg_asociacion = new List<Maseg_asociacion>();
                    dtoInsured.List_INDIVIDUAL_TAX_EXEMPTION = new List<INDIVIDUAL_TAX_EXEMPTION>();


                    DtoDataInsured dtoDataInsured = new DtoDataInsured();
                    dtoDataInsured.mpersona = new Mpersona();
                    dtoDataInsured.mpersona_ciiu = new Mpersona_ciiu();
                    dtoDataInsured.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
                    dtoDataInsured.maseg_header = new Maseg_header();
                    dtoDataInsured.tipo_persona_aseg = new Tipo_persona_aseg();
                    dtoDataInsured.tcpto_aseg_adic = new Tcpto_aseg_adic();
                    dtoDataInsured.maseg_autoriza_consul = new Maseg_autoriza_consul();
                    dtoDataInsured.CO_EQUIVALENCE_INSURED_3G = new CO_EQUIVALENCE_INSURED_3G();


                    DtoTechnicalCard dtoTechnicalCard = new DtoTechnicalCard();
                    dtoTechnicalCard.TECHNICAL_CARD = new TECHNICAL_CARD();
                    dtoTechnicalCard.List_BOARD_DIRECTORS = new List<BOARD_DIRECTORS>();
                    dtoTechnicalCard.List_FINANCIAL_STATEMENTS = new List<FINANCIAL_STATEMENTS>();
                    dtoTechnicalCard.List_TECHNICAL_CARD_DESCRIPTION = new List<TECHNICAL_CARD_DESCRIPTION>();

                    if (DsReturn.Tables.Count > 0)
                    {
                        if (DsReturn.Tables[0].Rows.Count > 0)
                        {
                            if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataInsured.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[0])[0]; }
                            if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataInsured.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[1])[0]; }
                            if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataInsured.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[2])[0]; }
                            int rowcount = 1;
                            if (DsReturn.Tables[3].Rows.Count > 0)
                            {
                                dtoInsured.List_Mpersona_dir = Dtl.ConvertTo<Mpersona_dir>(DsReturn.Tables[3]);
                                dtoInsured.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                                dtoInsured.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.cod_tipo_dir_old = element.cod_tipo_dir; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                                dtoInsured.List_Mpersona_dir = datos.dtoDataAddress3g(Convert.ToInt32(id_persona), dtoInsured.List_Mpersona_dir);
                            }

                            rowcount = 1;
                            if (DsReturn.Tables[4].Rows.Count > 0)
                            {
                                dtoInsured.List_Mpersona_telef = Dtl.ConvertTo<Mpersona_telef>(DsReturn.Tables[4]);
                                dtoInsured.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                                dtoInsured.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.cod_tipo_telef_old = element.cod_tipo_telef; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                                dtoInsured.List_Mpersona_telef = datos.dtoDataPhone3g(Convert.ToInt32(id_persona), dtoInsured.List_Mpersona_telef);

                            }

                            rowcount = 1;
                            if (DsReturn.Tables[5].Rows.Count > 0)
                            {
                                dtoInsured.List_Mpersona_email = Dtl.ConvertTo<Mpersona_email>(DsReturn.Tables[5]);
                                dtoInsured.List_Mpersona_email.ForEach(delegate(Mpersona_email element) { element.Identity = rowcount++; element.State = 'R'; });
                                dtoInsured.List_Mpersona_email = datos.dtoDataMail3g(Convert.ToInt32(id_persona), dtoInsured.List_Mpersona_email);

                            }

                            rowcount = 0;
                            if (DsReturn.Tables[6].Rows.Count > 0)
                            {
                                dtoInsured.List_Logbook = Dtl.ConvertTo<LOGBOOK>(DsReturn.Tables[6]);
                                dtoInsured.List_Logbook.ForEach(delegate(LOGBOOK element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[7].Rows.Count > 0)
                            {
                                dtoSarlaft.List_detalle = Dtl.ConvertTo<Frm_sarlaft_detalle>(DsReturn.Tables[7]);
                                dtoSarlaft.List_detalle.ForEach(delegate(Frm_sarlaft_detalle element) { element.Identity = rowcount++; element.State = 'R'; });
                            }
                            if (DsReturn.Tables[8].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_vinculos = Dtl.ConvertTo<Frm_sarlaft_vinculos>(DsReturn.Tables[8])[0]; }
                            if (DsReturn.Tables[9].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_aut_incrementos = Dtl.ConvertTo<Frm_sarlaft_aut_incrementos>(DsReturn.Tables[9])[0]; }

                            rowcount = 0;
                            if (DsReturn.Tables[10].Rows.Count > 0)
                            {
                                if (DsReturn.Tables[10].Rows.Count > 0) { dtorep.mpersona_rep_legal = Dtl.ConvertTo<Mpersona_rep_legal>(DsReturn.Tables[10])[0]; }
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[11].Rows.Count > 0)
                            {
                                dtorep.List_mpersona_rep_legal_dir = Dtl.ConvertTo<Mpersona_rep_legal_dir>(DsReturn.Tables[11]);
                                dtorep.List_mpersona_rep_legal_dir.ForEach(delegate(Mpersona_rep_legal_dir element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[12].Rows.Count > 0)
                            {
                                dtoInsured.List_Frm_sarlaft_accionistas_asoc = Dtl.ConvertTo<Frm_sarlaft_accionistas_asoc>(DsReturn.Tables[12]);
                                dtoInsured.List_Frm_sarlaft_accionistas_asoc.ForEach(delegate(Frm_sarlaft_accionistas_asoc element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                        }
                    }

                    dtoInsured.dtoDataInsured = dtoDataInsured;
                    dtoInsured.dtoSarlaft = dtoSarlaft;
                    dtoInsured.dtoRep = dtorep;
                    dtoInsured.dtoDataPerson = dtoDataPerson;
                    dtoInsured.dtoTechnicalCard = dtoTechnicalCard;
                    ReturnData.DtoInsured = dtoInsured;
                    break;
                case 2:
                    DtoLawyer dtoLawyer = new DtoLawyer();
                    dtoLawyer.List_Mpersona_dir = new List<Mpersona_dir>();
                    dtoLawyer.List_Mpersona_telef = new List<Mpersona_telef>();
                    dtoLawyer.List_Mpersona_email = new List<Mpersona_email>();
                    dtoLawyer.List_Logbook = new List<LOGBOOK>();
                    dtoLawyer.List_Frm_sarlaft_accionistas_asoc = new List<Frm_sarlaft_accionistas_asoc>();

                    DtoDataLawyer dtoDataLawyer = new DtoDataLawyer();
                    dtoDataLawyer.mpersona = new Mpersona();
                    dtoDataLawyer.mpersona_ciiu = new Mpersona_ciiu();
                    dtoDataLawyer.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
                    dtoDataLawyer.mabogado = new Mabogado();


                    if (DsReturn.Tables.Count > 0)
                    {
                        if (DsReturn.Tables[0].Rows.Count > 0)
                        {
                            if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataLawyer.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[0])[0]; }
                            if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataLawyer.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[1])[0]; }
                            if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataLawyer.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[2])[0]; }
                            int rowcount = 0;
                            if (DsReturn.Tables[3].Rows.Count > 0)
                            {
                                dtoLawyer.List_Mpersona_dir = Dtl.ConvertTo<Mpersona_dir>(DsReturn.Tables[3]);
                                dtoLawyer.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                                dtoLawyer.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.cod_tipo_dir_old = element.cod_tipo_dir; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[4].Rows.Count > 0)
                            {
                                dtoLawyer.List_Mpersona_telef = Dtl.ConvertTo<Mpersona_telef>(DsReturn.Tables[4]);
                                dtoLawyer.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                                dtoLawyer.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.cod_tipo_telef_old = element.cod_tipo_telef; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[5].Rows.Count > 0)
                            {
                                dtoLawyer.List_Mpersona_email = Dtl.ConvertTo<Mpersona_email>(DsReturn.Tables[5]);
                                dtoLawyer.List_Mpersona_email.ForEach(delegate(Mpersona_email element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[6].Rows.Count > 0)
                            {
                                dtoLawyer.List_Logbook = Dtl.ConvertTo<LOGBOOK>(DsReturn.Tables[6]);
                                dtoLawyer.List_Logbook.ForEach(delegate(LOGBOOK element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[7].Rows.Count > 0)
                            {
                                dtoSarlaft.List_detalle = Dtl.ConvertTo<Frm_sarlaft_detalle>(DsReturn.Tables[7]);
                                dtoSarlaft.List_detalle.ForEach(delegate(Frm_sarlaft_detalle element) { element.Identity = rowcount++; element.State = 'R'; });
                            }
                            if (DsReturn.Tables[8].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_vinculos = Dtl.ConvertTo<Frm_sarlaft_vinculos>(DsReturn.Tables[8])[0]; }
                            if (DsReturn.Tables[9].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_aut_incrementos = Dtl.ConvertTo<Frm_sarlaft_aut_incrementos>(DsReturn.Tables[9])[0]; }

                            rowcount = 0;
                            if (DsReturn.Tables[10].Rows.Count > 0)
                            {
                                if (DsReturn.Tables[10].Rows.Count > 0) { dtorep.mpersona_rep_legal = Dtl.ConvertTo<Mpersona_rep_legal>(DsReturn.Tables[10])[0]; }
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[11].Rows.Count > 0)
                            {
                                dtorep.List_mpersona_rep_legal_dir = Dtl.ConvertTo<Mpersona_rep_legal_dir>(DsReturn.Tables[11]);
                                dtorep.List_mpersona_rep_legal_dir.ForEach(delegate(Mpersona_rep_legal_dir element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[12].Rows.Count > 0)
                            {
                                dtoLawyer.List_Frm_sarlaft_accionistas_asoc = Dtl.ConvertTo<Frm_sarlaft_accionistas_asoc>(DsReturn.Tables[12]);
                                dtoLawyer.List_Frm_sarlaft_accionistas_asoc.ForEach(delegate(Frm_sarlaft_accionistas_asoc element) { element.Identity = rowcount++; element.State = 'R'; });
                            }
                        }
                    }

                    dtoLawyer.dtoDataLawyer = dtoDataLawyer;
                    dtoLawyer.dtoSarlaft = dtoSarlaft;
                    dtoLawyer.dtoRep = dtorep;
                    ReturnData.DtoLawyer = dtoLawyer;
                    break;
                case 3:
                    DtoBeneficiary dtoBeneficiary = new DtoBeneficiary();
                    dtoBeneficiary.List_Mpersona_dir = new List<Mpersona_dir>();
                    dtoBeneficiary.List_Mpersona_telef = new List<Mpersona_telef>();
                    dtoBeneficiary.List_Mpersona_email = new List<Mpersona_email>();
                    dtoBeneficiary.List_Logbook = new List<LOGBOOK>();
                    dtoBeneficiary.List_Frm_sarlaft_accionistas_asoc = new List<Frm_sarlaft_accionistas_asoc>();

                    DtoDataBeneficiary dtoDataBeneficiary = new DtoDataBeneficiary();
                    dtoDataBeneficiary.mpersona = new Mpersona();
                    dtoDataBeneficiary.mpersona_ciiu = new Mpersona_ciiu();
                    dtoDataBeneficiary.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
                    dtoDataBeneficiary.mbeneficiario = new Mbeneficiario();

                    if (DsReturn.Tables.Count > 0)
                    {
                        if (DsReturn.Tables[0].Rows.Count > 0)
                        {
                            if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataBeneficiary.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[0])[0]; }
                            if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataBeneficiary.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[1])[0]; }
                            if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataBeneficiary.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[2])[0]; }
                            int rowcount = 1;
                            if (DsReturn.Tables[3].Rows.Count > 0)
                            {
                                dtoBeneficiary.List_Mpersona_dir = Dtl.ConvertTo<Mpersona_dir>(DsReturn.Tables[3]);
                                dtoBeneficiary.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                                dtoBeneficiary.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.cod_tipo_dir_old = element.cod_tipo_dir; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                                dtoBeneficiary.List_Mpersona_dir = datos.dtoDataAddress3g(Convert.ToInt32(id_persona), dtoBeneficiary.List_Mpersona_dir);
                            }

                            rowcount = 1;
                            if (DsReturn.Tables[4].Rows.Count > 0)
                            {
                                dtoBeneficiary.List_Mpersona_telef = Dtl.ConvertTo<Mpersona_telef>(DsReturn.Tables[4]);
                                dtoBeneficiary.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                                dtoBeneficiary.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.cod_tipo_telef_old = element.cod_tipo_telef; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                                dtoBeneficiary.List_Mpersona_telef = datos.dtoDataPhone3g(Convert.ToInt32(id_persona), dtoBeneficiary.List_Mpersona_telef);

                            }

                            rowcount = 1;
                            if (DsReturn.Tables[5].Rows.Count > 0)
                            {
                                dtoBeneficiary.List_Mpersona_email = Dtl.ConvertTo<Mpersona_email>(DsReturn.Tables[5]);
                                dtoBeneficiary.List_Mpersona_email.ForEach(delegate(Mpersona_email element) { element.Identity = rowcount++; element.State = 'R'; });
                                dtoBeneficiary.List_Mpersona_email = datos.dtoDataMail3g(Convert.ToInt32(id_persona), dtoBeneficiary.List_Mpersona_email);
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[6].Rows.Count > 0)
                            {
                                dtoBeneficiary.List_Logbook = Dtl.ConvertTo<LOGBOOK>(DsReturn.Tables[6]);
                                dtoBeneficiary.List_Logbook.ForEach(delegate(LOGBOOK element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[7].Rows.Count > 0)
                            {
                                dtoSarlaft.List_detalle = Dtl.ConvertTo<Frm_sarlaft_detalle>(DsReturn.Tables[7]);
                                dtoSarlaft.List_detalle.ForEach(delegate(Frm_sarlaft_detalle element) { element.Identity = rowcount++; element.State = 'R'; });
                            }
                            if (DsReturn.Tables[8].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_vinculos = Dtl.ConvertTo<Frm_sarlaft_vinculos>(DsReturn.Tables[8])[0]; }
                            if (DsReturn.Tables[9].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_aut_incrementos = Dtl.ConvertTo<Frm_sarlaft_aut_incrementos>(DsReturn.Tables[9])[0]; }

                            rowcount = 0;
                            if (DsReturn.Tables[10].Rows.Count > 0)
                            {
                                if (DsReturn.Tables[10].Rows.Count > 0) { dtorep.mpersona_rep_legal = Dtl.ConvertTo<Mpersona_rep_legal>(DsReturn.Tables[10])[0]; }
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[11].Rows.Count > 0)
                            {
                                dtorep.List_mpersona_rep_legal_dir = Dtl.ConvertTo<Mpersona_rep_legal_dir>(DsReturn.Tables[11]);
                                dtorep.List_mpersona_rep_legal_dir.ForEach(delegate(Mpersona_rep_legal_dir element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[12].Rows.Count > 0)
                            {
                                dtoBeneficiary.List_Frm_sarlaft_accionistas_asoc = Dtl.ConvertTo<Frm_sarlaft_accionistas_asoc>(DsReturn.Tables[12]);
                                dtoBeneficiary.List_Frm_sarlaft_accionistas_asoc.ForEach(delegate(Frm_sarlaft_accionistas_asoc element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                        }
                    }

                    dtoBeneficiary.dtoDataBeneficiary = dtoDataBeneficiary;
                    dtoBeneficiary.List_Mbenef_asoc_aseg = new List<Mbenef_asoc_aseg>();
                    dtoBeneficiary.dtoSarlaft = dtoSarlaft;
                    dtoBeneficiary.dtoRep = dtorep;
                    ReturnData.DtoBeneficiary = dtoBeneficiary;
                    break;
                case 4:
                    DtoAssigneed dtoAssigneed = new DtoAssigneed();
                    dtoAssigneed.List_Mpersona_dir = new List<Mpersona_dir>();
                    dtoAssigneed.List_Mpersona_telef = new List<Mpersona_telef>();
                    dtoAssigneed.List_Mpersona_email = new List<Mpersona_email>();
                    dtoAssigneed.List_Logbook = new List<LOGBOOK>();
                    dtoAssigneed.List_Frm_sarlaft_accionistas_asoc = new List<Frm_sarlaft_accionistas_asoc>();

                    DtoDataAssigneed dtoDataAssigneed = new DtoDataAssigneed();
                    dtoDataAssigneed.mpersona = new Mpersona();
                    dtoDataAssigneed.mpersona_ciiu = new Mpersona_ciiu();
                    dtoDataAssigneed.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
                    dtoDataAssigneed.mcesionario = new Mcesionario();

                    if (DsReturn.Tables.Count > 0)
                    {
                        if (DsReturn.Tables[0].Rows.Count > 0)
                        {
                            if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataAssigneed.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[0])[0]; }
                            if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataAssigneed.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[1])[0]; }
                            if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataAssigneed.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[2])[0]; }
                            int rowcount = 0;
                            if (DsReturn.Tables[3].Rows.Count > 0)
                            {
                                dtoAssigneed.List_Mpersona_dir = Dtl.ConvertTo<Mpersona_dir>(DsReturn.Tables[3]);
                                dtoAssigneed.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                                dtoAssigneed.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.cod_tipo_dir_old = element.cod_tipo_dir; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[4].Rows.Count > 0)
                            {
                                dtoAssigneed.List_Mpersona_telef = Dtl.ConvertTo<Mpersona_telef>(DsReturn.Tables[4]);
                                dtoAssigneed.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                                dtoAssigneed.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.cod_tipo_telef_old = element.cod_tipo_telef; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[5].Rows.Count > 0)
                            {
                                dtoAssigneed.List_Mpersona_email = Dtl.ConvertTo<Mpersona_email>(DsReturn.Tables[5]);
                                dtoAssigneed.List_Mpersona_email.ForEach(delegate(Mpersona_email element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[6].Rows.Count > 0)
                            {
                                dtoAssigneed.List_Logbook = Dtl.ConvertTo<LOGBOOK>(DsReturn.Tables[6]);
                                dtoAssigneed.List_Logbook.ForEach(delegate(LOGBOOK element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[7].Rows.Count > 0)
                            {
                                dtoSarlaft.List_detalle = Dtl.ConvertTo<Frm_sarlaft_detalle>(DsReturn.Tables[7]);
                                dtoSarlaft.List_detalle.ForEach(delegate(Frm_sarlaft_detalle element) { element.Identity = rowcount++; element.State = 'R'; });
                            }
                            if (DsReturn.Tables[8].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_vinculos = Dtl.ConvertTo<Frm_sarlaft_vinculos>(DsReturn.Tables[8])[0]; }
                            if (DsReturn.Tables[9].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_aut_incrementos = Dtl.ConvertTo<Frm_sarlaft_aut_incrementos>(DsReturn.Tables[9])[0]; }

                            rowcount = 0;
                            if (DsReturn.Tables[10].Rows.Count > 0)
                            {
                                if (DsReturn.Tables[10].Rows.Count > 0) { dtorep.mpersona_rep_legal = Dtl.ConvertTo<Mpersona_rep_legal>(DsReturn.Tables[10])[0]; }
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[11].Rows.Count > 0)
                            {
                                dtorep.List_mpersona_rep_legal_dir = Dtl.ConvertTo<Mpersona_rep_legal_dir>(DsReturn.Tables[11]);
                                dtorep.List_mpersona_rep_legal_dir.ForEach(delegate(Mpersona_rep_legal_dir element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[12].Rows.Count > 0)
                            {
                                dtoAssigneed.List_Frm_sarlaft_accionistas_asoc = Dtl.ConvertTo<Frm_sarlaft_accionistas_asoc>(DsReturn.Tables[12]);
                                dtoAssigneed.List_Frm_sarlaft_accionistas_asoc.ForEach(delegate(Frm_sarlaft_accionistas_asoc element) { element.Identity = rowcount++; element.State = 'R'; });
                            }
                        }
                    }

                    dtoAssigneed.DtoDataAssigneed = dtoDataAssigneed;
                    dtoAssigneed.dtoSarlaft = dtoSarlaft;
                    dtoAssigneed.dtoRep = dtorep;
                    ReturnData.DtoAssigneed = dtoAssigneed;
                    break;
                case 5:
                    DtoPrincipalNational dtoPrincipalNational = new DtoPrincipalNational();
                    dtoPrincipalNational.List_Mpersona_dir = new List<Mpersona_dir>();
                    dtoPrincipalNational.List_Mpersona_telef = new List<Mpersona_telef>();
                    dtoPrincipalNational.List_Mpersona_email = new List<Mpersona_email>();
                    dtoPrincipalNational.List_Logbook = new List<LOGBOOK>();

                    DtoDataDN dtoDataDN = new DtoDataDN();
                    dtoDataDN.mpersona = new Mpersona();
                    dtoDataDN.mpersona_ciiu = new Mpersona_ciiu();
                    dtoDataDN.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
                    dtoDataDN.tdirector_nacional = new Tdirector_nacional();

                    if (DsReturn.Tables.Count > 0)
                    {
                        if (DsReturn.Tables[0].Rows.Count > 0)
                        {
                            if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataDN.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[0])[0]; }
                            if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataDN.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[1])[0]; }
                            if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataDN.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[2])[0]; }
                            int rowcount = 0;
                            if (DsReturn.Tables[3].Rows.Count > 0)
                            {
                                dtoPrincipalNational.List_Mpersona_dir = Dtl.ConvertTo<Mpersona_dir>(DsReturn.Tables[3]);
                                dtoPrincipalNational.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                                dtoPrincipalNational.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.cod_tipo_dir_old = element.cod_tipo_dir; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[4].Rows.Count > 0)
                            {
                                dtoPrincipalNational.List_Mpersona_telef = Dtl.ConvertTo<Mpersona_telef>(DsReturn.Tables[4]);
                                dtoPrincipalNational.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                                dtoPrincipalNational.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.cod_tipo_telef_old = element.cod_tipo_telef; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[5].Rows.Count > 0)
                            {
                                dtoPrincipalNational.List_Mpersona_email = Dtl.ConvertTo<Mpersona_email>(DsReturn.Tables[5]);
                                dtoPrincipalNational.List_Mpersona_email.ForEach(delegate(Mpersona_email element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[6].Rows.Count > 0)
                            {
                                dtoPrincipalNational.List_Logbook = Dtl.ConvertTo<LOGBOOK>(DsReturn.Tables[6]);
                                dtoPrincipalNational.List_Logbook.ForEach(delegate(LOGBOOK element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[7].Rows.Count > 0)
                            {
                                dtoSarlaft.List_detalle = Dtl.ConvertTo<Frm_sarlaft_detalle>(DsReturn.Tables[7]);
                                dtoSarlaft.List_detalle.ForEach(delegate(Frm_sarlaft_detalle element) { element.Identity = rowcount++; element.State = 'R'; });
                            }
                            if (DsReturn.Tables[8].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_vinculos = Dtl.ConvertTo<Frm_sarlaft_vinculos>(DsReturn.Tables[8])[0]; }
                            if (DsReturn.Tables[9].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_aut_incrementos = Dtl.ConvertTo<Frm_sarlaft_aut_incrementos>(DsReturn.Tables[9])[0]; }

                        }
                    }

                    dtoPrincipalNational.dtoDataDN = dtoDataDN;
                    dtoPrincipalNational.dtoSarlaft = dtoSarlaft;
                    ReturnData.DtoPrincipalNational = dtoPrincipalNational;
                    break;
                case 6:
                    DtoPrincipalComertial dtoPrincipalComertial = new DtoPrincipalComertial();
                    dtoPrincipalComertial.List_Mpersona_dir = new List<Mpersona_dir>();
                    dtoPrincipalComertial.List_Mpersona_telef = new List<Mpersona_telef>();
                    dtoPrincipalComertial.List_Mpersona_email = new List<Mpersona_email>();
                    dtoPrincipalComertial.List_Logbook = new List<LOGBOOK>();

                    DtoDataDC dtoDataDC = new DtoDataDC();
                    dtoDataDC.mpersona = new Mpersona();
                    dtoDataDC.mpersona_ciiu = new Mpersona_ciiu();
                    dtoDataDC.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
                    dtoDataDC.tdirector_comercial = new Tdirector_comercial();
                    dtoDataDC.tdirector_comercial_hist = new Tdirector_comercial_hist();

                    if (DsReturn.Tables.Count > 0)
                    {
                        if (DsReturn.Tables[0].Rows.Count > 0)
                        {
                            if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataDC.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[0])[0]; }
                            if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataDC.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[1])[0]; }
                            if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataDC.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[2])[0]; }
                            int rowcount = 0;
                            if (DsReturn.Tables[3].Rows.Count > 0)
                            {
                                dtoPrincipalComertial.List_Mpersona_dir = Dtl.ConvertTo<Mpersona_dir>(DsReturn.Tables[3]);
                                dtoPrincipalComertial.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                                dtoPrincipalComertial.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.cod_tipo_dir_old = element.cod_tipo_dir; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[4].Rows.Count > 0)
                            {
                                dtoPrincipalComertial.List_Mpersona_telef = Dtl.ConvertTo<Mpersona_telef>(DsReturn.Tables[4]);
                                dtoPrincipalComertial.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                                dtoPrincipalComertial.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.cod_tipo_telef_old = element.cod_tipo_telef; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[5].Rows.Count > 0)
                            {
                                dtoPrincipalComertial.List_Mpersona_email = Dtl.ConvertTo<Mpersona_email>(DsReturn.Tables[5]);
                                dtoPrincipalComertial.List_Mpersona_email.ForEach(delegate(Mpersona_email element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[6].Rows.Count > 0)
                            {
                                dtoPrincipalComertial.List_Logbook = Dtl.ConvertTo<LOGBOOK>(DsReturn.Tables[6]);
                                dtoPrincipalComertial.List_Logbook.ForEach(delegate(LOGBOOK element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[7].Rows.Count > 0)
                            {
                                dtoSarlaft.List_detalle = Dtl.ConvertTo<Frm_sarlaft_detalle>(DsReturn.Tables[7]);
                                dtoSarlaft.List_detalle.ForEach(delegate(Frm_sarlaft_detalle element) { element.Identity = rowcount++; element.State = 'R'; });
                            }
                            if (DsReturn.Tables[8].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_vinculos = Dtl.ConvertTo<Frm_sarlaft_vinculos>(DsReturn.Tables[8])[0]; }
                            if (DsReturn.Tables[9].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_aut_incrementos = Dtl.ConvertTo<Frm_sarlaft_aut_incrementos>(DsReturn.Tables[9])[0]; }

                        }
                    }

                    dtoPrincipalComertial.dtoDataDC = dtoDataDC;
                    dtoPrincipalComertial.dtoSarlaft = dtoSarlaft;
                    ReturnData.DtoPrincipalComertial = dtoPrincipalComertial;
                    break;
                case 7:
                    DtoTechnicalAssistant dtoTechnicalAssistant = new DtoTechnicalAssistant();
                    dtoTechnicalAssistant.List_Mpersona_dir = new List<Mpersona_dir>();
                    dtoTechnicalAssistant.List_Mpersona_telef = new List<Mpersona_telef>();
                    dtoTechnicalAssistant.List_Mpersona_email = new List<Mpersona_email>();
                    dtoTechnicalAssistant.List_Logbook = new List<LOGBOOK>();

                    DtoDataTA dtoDataTA = new DtoDataTA();
                    dtoDataTA.mpersona = new Mpersona();
                    dtoDataTA.mpersona_ciiu = new Mpersona_ciiu();
                    dtoDataTA.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
                    dtoDataTA.tasist_tecnico = new Tasist_tecnico();

                    if (DsReturn.Tables.Count > 0)
                    {
                        if (DsReturn.Tables[0].Rows.Count > 0)
                        {
                            if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataTA.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[0])[0]; }
                            if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataTA.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[1])[0]; }
                            if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataTA.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[2])[0]; }
                            int rowcount = 0;
                            if (DsReturn.Tables[3].Rows.Count > 0)
                            {
                                dtoTechnicalAssistant.List_Mpersona_dir = Dtl.ConvertTo<Mpersona_dir>(DsReturn.Tables[3]);
                                dtoTechnicalAssistant.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                                dtoTechnicalAssistant.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.cod_tipo_dir_old = element.cod_tipo_dir; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[4].Rows.Count > 0)
                            {
                                dtoTechnicalAssistant.List_Mpersona_telef = Dtl.ConvertTo<Mpersona_telef>(DsReturn.Tables[4]);
                                dtoTechnicalAssistant.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                                dtoTechnicalAssistant.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.cod_tipo_telef_old = element.cod_tipo_telef; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[5].Rows.Count > 0)
                            {
                                dtoTechnicalAssistant.List_Mpersona_email = Dtl.ConvertTo<Mpersona_email>(DsReturn.Tables[5]);
                                dtoTechnicalAssistant.List_Mpersona_email.ForEach(delegate(Mpersona_email element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[6].Rows.Count > 0)
                            {
                                dtoTechnicalAssistant.List_Logbook = Dtl.ConvertTo<LOGBOOK>(DsReturn.Tables[6]);
                                dtoTechnicalAssistant.List_Logbook.ForEach(delegate(LOGBOOK element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[7].Rows.Count > 0)
                            {
                                dtoSarlaft.List_detalle = Dtl.ConvertTo<Frm_sarlaft_detalle>(DsReturn.Tables[7]);
                                dtoSarlaft.List_detalle.ForEach(delegate(Frm_sarlaft_detalle element) { element.Identity = rowcount++; element.State = 'R'; });
                            }
                            if (DsReturn.Tables[8].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_vinculos = Dtl.ConvertTo<Frm_sarlaft_vinculos>(DsReturn.Tables[8])[0]; }
                            if (DsReturn.Tables[9].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_aut_incrementos = Dtl.ConvertTo<Frm_sarlaft_aut_incrementos>(DsReturn.Tables[9])[0]; }

                        }
                    }

                    dtoTechnicalAssistant.dtoDataTA = dtoDataTA;
                    dtoTechnicalAssistant.dtoSarlaft = dtoSarlaft;
                    ReturnData.DtoTechnicalAssistant = dtoTechnicalAssistant;
                    break;
                case 8:
                    DtoEmployee dtoEmployee = new DtoEmployee();
                    dtoEmployee.List_Mpersona_dir = new List<Mpersona_dir>();
                    dtoEmployee.List_Mpersona_telef = new List<Mpersona_telef>();
                    dtoEmployee.List_Mpersona_email = new List<Mpersona_email>();
                    dtoEmployee.List_Logbook = new List<LOGBOOK>();

                    DtoDataEmployee dtoDataEmployee = new DtoDataEmployee();
                    dtoDataEmployee.mpersona = new Mpersona();
                    dtoDataEmployee.mpersona_ciiu = new Mpersona_ciiu();
                    dtoDataEmployee.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
                    dtoDataEmployee.mempleado = new Mempleado();

                    if (DsReturn.Tables.Count > 0)
                    {
                        if (DsReturn.Tables[0].Rows.Count > 0)
                        {
                            if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataEmployee.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[0])[0]; }
                            if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataEmployee.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[1])[0]; }
                            if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataEmployee.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[2])[0]; }
                            int rowcount = 0;
                            if (DsReturn.Tables[3].Rows.Count > 0)
                            {
                                dtoEmployee.List_Mpersona_dir = Dtl.ConvertTo<Mpersona_dir>(DsReturn.Tables[3]);
                                dtoEmployee.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                                dtoEmployee.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.cod_tipo_dir_old = element.cod_tipo_dir; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[4].Rows.Count > 0)
                            {
                                dtoEmployee.List_Mpersona_telef = Dtl.ConvertTo<Mpersona_telef>(DsReturn.Tables[4]);
                                dtoEmployee.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                                dtoEmployee.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.cod_tipo_telef_old = element.cod_tipo_telef; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[5].Rows.Count > 0)
                            {
                                dtoEmployee.List_Mpersona_email = Dtl.ConvertTo<Mpersona_email>(DsReturn.Tables[5]);
                                dtoEmployee.List_Mpersona_email.ForEach(delegate(Mpersona_email element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[6].Rows.Count > 0)
                            {
                                dtoEmployee.List_Logbook = Dtl.ConvertTo<LOGBOOK>(DsReturn.Tables[6]);
                                dtoEmployee.List_Logbook.ForEach(delegate(LOGBOOK element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[7].Rows.Count > 0)
                            {
                                dtoSarlaft.List_detalle = Dtl.ConvertTo<Frm_sarlaft_detalle>(DsReturn.Tables[7]);
                                dtoSarlaft.List_detalle.ForEach(delegate(Frm_sarlaft_detalle element) { element.Identity = rowcount++; element.State = 'R'; });
                            }
                            if (DsReturn.Tables[8].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_vinculos = Dtl.ConvertTo<Frm_sarlaft_vinculos>(DsReturn.Tables[8])[0]; }
                            if (DsReturn.Tables[9].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_aut_incrementos = Dtl.ConvertTo<Frm_sarlaft_aut_incrementos>(DsReturn.Tables[9])[0]; }

                        }
                    }

                    dtoEmployee.dtoDataEmployee = dtoDataEmployee;
                    dtoEmployee.dtoSarlaft = dtoSarlaft;
                    ReturnData.DtoEmployee = dtoEmployee;
                    break;
                case 9:
                    DtoAgent dtoAgent = new DtoAgent();
                    dtoAgent.List_Mpersona_dir = new List<Mpersona_dir>();
                    dtoAgent.List_Mpersona_telef = new List<Mpersona_telef>();
                    dtoAgent.List_Mpersona_email = new List<Mpersona_email>();
                    dtoAgent.List_Logbook = new List<LOGBOOK>();
                    dtoAgent.List_Frm_sarlaft_accionistas_asoc = new List<Frm_sarlaft_accionistas_asoc>();
                    dtoAgent.List_Magente_ramo = new List<Magente_ramo>();

                    DtoDataAgent dtoDataAgent = new DtoDataAgent();
                    dtoDataAgent.mpersona = new Mpersona();
                    dtoDataAgent.mpersona_ciiu = new Mpersona_ciiu();
                    dtoDataAgent.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
                    dtoDataAgent.magente = new Magente();

                    if (DsReturn.Tables.Count > 0)
                    {
                        if (DsReturn.Tables[0].Rows.Count > 0)
                        {
                            if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataAgent.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[0])[0]; }
                            if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataAgent.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[1])[0]; }
                            if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataAgent.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[2])[0]; }
                            int rowcount = 1;
                            if (DsReturn.Tables[3].Rows.Count > 0)
                            {
                                dtoAgent.List_Mpersona_dir = Dtl.ConvertTo<Mpersona_dir>(DsReturn.Tables[3]);
                                dtoAgent.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                                dtoAgent.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.cod_tipo_dir_old = element.cod_tipo_dir; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                                dtoAgent.List_Mpersona_dir = datos.dtoDataAddress3g(Convert.ToInt32(id_persona), dtoAgent.List_Mpersona_dir);
                            }

                            rowcount = 1;
                            if (DsReturn.Tables[4].Rows.Count > 0)
                            {
                                dtoAgent.List_Mpersona_telef = Dtl.ConvertTo<Mpersona_telef>(DsReturn.Tables[4]);
                                dtoAgent.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                                dtoAgent.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.cod_tipo_telef_old = element.cod_tipo_telef; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                                dtoAgent.List_Mpersona_telef = datos.dtoDataPhone3g(Convert.ToInt32(id_persona), dtoAgent.List_Mpersona_telef);
                            }

                            rowcount = 1;
                            if (DsReturn.Tables[5].Rows.Count > 0)
                            {
                                dtoAgent.List_Mpersona_email = Dtl.ConvertTo<Mpersona_email>(DsReturn.Tables[5]);
                                dtoAgent.List_Mpersona_email.ForEach(delegate(Mpersona_email element) { element.Identity = rowcount++; element.State = 'R'; });
                                dtoAgent.List_Mpersona_email = datos.dtoDataMail3g(Convert.ToInt32(id_persona), dtoAgent.List_Mpersona_email);
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[6].Rows.Count > 0)
                            {
                                dtoAgent.List_Logbook = Dtl.ConvertTo<LOGBOOK>(DsReturn.Tables[6]);
                                dtoAgent.List_Logbook.ForEach(delegate(LOGBOOK element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[7].Rows.Count > 0)
                            {
                                dtoSarlaft.List_detalle = Dtl.ConvertTo<Frm_sarlaft_detalle>(DsReturn.Tables[7]);
                                dtoSarlaft.List_detalle.ForEach(delegate(Frm_sarlaft_detalle element) { element.Identity = rowcount++; element.State = 'R'; });
                            }
                            if (DsReturn.Tables[8].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_vinculos = Dtl.ConvertTo<Frm_sarlaft_vinculos>(DsReturn.Tables[8])[0]; }
                            if (DsReturn.Tables[9].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_aut_incrementos = Dtl.ConvertTo<Frm_sarlaft_aut_incrementos>(DsReturn.Tables[9])[0]; }

                            rowcount = 0;
                            if (DsReturn.Tables[10].Rows.Count > 0)
                            {
                                if (DsReturn.Tables[10].Rows.Count > 0) { dtorep.mpersona_rep_legal = Dtl.ConvertTo<Mpersona_rep_legal>(DsReturn.Tables[10])[0]; }
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[11].Rows.Count > 0)
                            {
                                dtorep.List_mpersona_rep_legal_dir = Dtl.ConvertTo<Mpersona_rep_legal_dir>(DsReturn.Tables[11]);
                                dtorep.List_mpersona_rep_legal_dir.ForEach(delegate(Mpersona_rep_legal_dir element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[12].Rows.Count > 0)
                            {
                                dtoAgent.List_Frm_sarlaft_accionistas_asoc = Dtl.ConvertTo<Frm_sarlaft_accionistas_asoc>(DsReturn.Tables[12]);
                                dtoAgent.List_Frm_sarlaft_accionistas_asoc.ForEach(delegate(Frm_sarlaft_accionistas_asoc element) { element.Identity = rowcount++; element.State = 'R'; });
                            }
                        }
                    }

                    dtoAgent.dtoDataAgent = dtoDataAgent;
                    dtoAgent.dtoSarlaft = dtoSarlaft;
                    dtoAgent.dtoRep = dtorep;
                    ReturnData.DtoAgent = dtoAgent;
                    break;
                case 10:
                    DtoProvider dtoProvider = new DtoProvider();
                    dtoProvider.List_Mpersona_dir = new List<Mpersona_dir>();
                    dtoProvider.List_Mpersona_telef = new List<Mpersona_telef>();
                    dtoProvider.List_Mpersona_email = new List<Mpersona_email>();
                    dtoProvider.List_Logbook = new List<LOGBOOK>();
                    dtoProvider.List_Frm_sarlaft_accionistas_asoc = new List<Frm_sarlaft_accionistas_asoc>();

                    DtoDataProvider dtoDataProvider = new DtoDataProvider();
                    dtoDataProvider.mpersona = new Mpersona();
                    dtoDataProvider.mpersona_ciiu = new Mpersona_ciiu();
                    dtoDataProvider.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
                    dtoDataProvider.mpres = new Mpres();

                    if (DsReturn.Tables.Count > 0)
                    {
                        if (DsReturn.Tables[0].Rows.Count > 0)
                        {
                            if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataProvider.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[0])[0]; }
                            if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataProvider.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[1])[0]; }
                            if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataProvider.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[2])[0]; }
                            int rowcount = 0;
                            if (DsReturn.Tables[3].Rows.Count > 0)
                            {
                                dtoProvider.List_Mpersona_dir = Dtl.ConvertTo<Mpersona_dir>(DsReturn.Tables[3]);
                                dtoProvider.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                                dtoProvider.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.cod_tipo_dir_old = element.cod_tipo_dir; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[4].Rows.Count > 0)
                            {
                                dtoProvider.List_Mpersona_telef = Dtl.ConvertTo<Mpersona_telef>(DsReturn.Tables[4]);
                                dtoProvider.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                                dtoProvider.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.cod_tipo_telef_old = element.cod_tipo_telef; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[5].Rows.Count > 0)
                            {
                                dtoProvider.List_Mpersona_email = Dtl.ConvertTo<Mpersona_email>(DsReturn.Tables[5]);
                                dtoProvider.List_Mpersona_email.ForEach(delegate(Mpersona_email element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[6].Rows.Count > 0)
                            {
                                dtoProvider.List_Logbook = Dtl.ConvertTo<LOGBOOK>(DsReturn.Tables[6]);
                                dtoProvider.List_Logbook.ForEach(delegate(LOGBOOK element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[7].Rows.Count > 0)
                            {
                                dtoSarlaft.List_detalle = Dtl.ConvertTo<Frm_sarlaft_detalle>(DsReturn.Tables[7]);
                                dtoSarlaft.List_detalle.ForEach(delegate(Frm_sarlaft_detalle element) { element.Identity = rowcount++; element.State = 'R'; });
                            }
                            if (DsReturn.Tables[8].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_vinculos = Dtl.ConvertTo<Frm_sarlaft_vinculos>(DsReturn.Tables[8])[0]; }
                            if (DsReturn.Tables[9].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_aut_incrementos = Dtl.ConvertTo<Frm_sarlaft_aut_incrementos>(DsReturn.Tables[9])[0]; }

                            rowcount = 0;
                            if (DsReturn.Tables[10].Rows.Count > 0)
                            {
                                if (DsReturn.Tables[10].Rows.Count > 0) { dtorep.mpersona_rep_legal = Dtl.ConvertTo<Mpersona_rep_legal>(DsReturn.Tables[10])[0]; }
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[11].Rows.Count > 0)
                            {
                                dtorep.List_mpersona_rep_legal_dir = Dtl.ConvertTo<Mpersona_rep_legal_dir>(DsReturn.Tables[11]);
                                dtorep.List_mpersona_rep_legal_dir.ForEach(delegate(Mpersona_rep_legal_dir element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[12].Rows.Count > 0)
                            {
                                dtoProvider.List_Frm_sarlaft_accionistas_asoc = Dtl.ConvertTo<Frm_sarlaft_accionistas_asoc>(DsReturn.Tables[12]);
                                dtoProvider.List_Frm_sarlaft_accionistas_asoc.ForEach(delegate(Frm_sarlaft_accionistas_asoc element) { element.Identity = rowcount++; element.State = 'R'; });
                            }
                        }
                    }

                    dtoProvider.dtoDataProvider = dtoDataProvider;
                    dtoProvider.dtoSarlaft = dtoSarlaft;
                    dtoProvider.dtoRep = dtorep;
                    ReturnData.DtoProvider = dtoProvider;
                    break;
                case 11:
                    DtoThird dtoThird = new DtoThird();
                    dtoThird.List_Mpersona_dir = new List<Mpersona_dir>();
                    dtoThird.List_Mpersona_telef = new List<Mpersona_telef>();
                    dtoThird.List_Mpersona_email = new List<Mpersona_email>();
                    dtoThird.List_Logbook = new List<LOGBOOK>();
                    dtoThird.List_Frm_sarlaft_accionistas_asoc = new List<Frm_sarlaft_accionistas_asoc>();

                    DtoDataThird DtoDataThird = new DtoDataThird();
                    DtoDataThird.mpersona = new Mpersona();
                    DtoDataThird.mpersona_ciiu = new Mpersona_ciiu();
                    DtoDataThird.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
                    DtoDataThird.mtercero = new Mtercero();

                    if (DsReturn.Tables.Count > 0)
                    {
                        if (DsReturn.Tables[0].Rows.Count > 0)
                        {
                            if (DsReturn.Tables[0].Rows.Count > 0) { DtoDataThird.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[0])[0]; }
                            if (DsReturn.Tables[1].Rows.Count > 0) { DtoDataThird.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[1])[0]; }
                            if (DsReturn.Tables[2].Rows.Count > 0) { DtoDataThird.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[2])[0]; }
                            int rowcount = 0;
                            if (DsReturn.Tables[3].Rows.Count > 0)
                            {
                                dtoThird.List_Mpersona_dir = Dtl.ConvertTo<Mpersona_dir>(DsReturn.Tables[3]);
                                dtoThird.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                                dtoThird.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.cod_tipo_dir_old = element.cod_tipo_dir; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[4].Rows.Count > 0)
                            {
                                dtoThird.List_Mpersona_telef = Dtl.ConvertTo<Mpersona_telef>(DsReturn.Tables[4]);
                                dtoThird.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                                dtoThird.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.cod_tipo_telef_old = element.cod_tipo_telef; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[5].Rows.Count > 0)
                            {
                                dtoThird.List_Mpersona_email = Dtl.ConvertTo<Mpersona_email>(DsReturn.Tables[5]);
                                dtoThird.List_Mpersona_email.ForEach(delegate(Mpersona_email element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[6].Rows.Count > 0)
                            {
                                dtoThird.List_Logbook = Dtl.ConvertTo<LOGBOOK>(DsReturn.Tables[6]);
                                dtoThird.List_Logbook.ForEach(delegate(LOGBOOK element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[7].Rows.Count > 0)
                            {
                                dtoSarlaft.List_detalle = Dtl.ConvertTo<Frm_sarlaft_detalle>(DsReturn.Tables[7]);
                                dtoSarlaft.List_detalle.ForEach(delegate(Frm_sarlaft_detalle element) { element.Identity = rowcount++; element.State = 'R'; });
                            }
                            if (DsReturn.Tables[8].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_vinculos = Dtl.ConvertTo<Frm_sarlaft_vinculos>(DsReturn.Tables[8])[0]; }
                            if (DsReturn.Tables[9].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_aut_incrementos = Dtl.ConvertTo<Frm_sarlaft_aut_incrementos>(DsReturn.Tables[9])[0]; }

                            rowcount = 0;
                            if (DsReturn.Tables[10].Rows.Count > 0)
                            {
                                if (DsReturn.Tables[10].Rows.Count > 0) { dtorep.mpersona_rep_legal = Dtl.ConvertTo<Mpersona_rep_legal>(DsReturn.Tables[10])[0]; }
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[11].Rows.Count > 0)
                            {
                                dtorep.List_mpersona_rep_legal_dir = Dtl.ConvertTo<Mpersona_rep_legal_dir>(DsReturn.Tables[11]);
                                dtorep.List_mpersona_rep_legal_dir.ForEach(delegate(Mpersona_rep_legal_dir element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[12].Rows.Count > 0)
                            {
                                dtoThird.List_Frm_sarlaft_accionistas_asoc = Dtl.ConvertTo<Frm_sarlaft_accionistas_asoc>(DsReturn.Tables[12]);
                                dtoThird.List_Frm_sarlaft_accionistas_asoc.ForEach(delegate(Frm_sarlaft_accionistas_asoc element) { element.Identity = rowcount++; element.State = 'R'; });
                            }
                        }
                    }

                    dtoThird.DtoDataThird = DtoDataThird;
                    dtoThird.dtoSarlaft = dtoSarlaft;
                    dtoThird.dtoRep = dtorep;
                    ReturnData.DtoThird = dtoThird;
                    break;
                case 12:
                    DtoUser dtoUser = new DtoUser();
                    dtoUser.List_Mpersona_dir = new List<Mpersona_dir>();
                    dtoUser.List_Mpersona_telef = new List<Mpersona_telef>();
                    dtoUser.List_Mpersona_email = new List<Mpersona_email>();
                    dtoUser.List_Logbook = new List<LOGBOOK>();
                    dtoUser.List_Tusuario_centro_costo = new List<Tusuario_centro_costo>();
                    dtoUser.List_Tusuario_limites = new List<Tusuario_limites>();
                    dtoUser.List_Tusuario_modulo_imputacion = new List<Tusuario_modulo_imputacion>();
                    dtoUser.List_Tpj_usuarios_email = new List<Tpj_usuarios_email>();

                    DtoDataUser dtoDataUser = new DtoDataUser();
                    dtoDataUser.mpersona = new Mpersona();
                    dtoDataUser.mpersona_ciiu = new Mpersona_ciiu();
                    dtoDataUser.mpersona_requiere_sarlaft = new Mpersona_requiere_sarlaft();
                    dtoDataUser.log_usuario = new Log_usuario();
                    dtoDataUser.mpersona_usuario = new Mpersona_usuario();
                    dtoDataUser.tusuario = new Tusuario();

                    if (DsReturn.Tables.Count > 0)
                    {
                        if (DsReturn.Tables[0].Rows.Count > 0)
                        {
                            if (DsReturn.Tables[0].Rows.Count > 0) { dtoDataUser.mpersona = Dtl.ConvertTo<Mpersona>(DsReturn.Tables[0])[0]; }
                            if (DsReturn.Tables[1].Rows.Count > 0) { dtoDataUser.mpersona_ciiu = Dtl.ConvertTo<Mpersona_ciiu>(DsReturn.Tables[1])[0]; }
                            if (DsReturn.Tables[2].Rows.Count > 0) { dtoDataUser.mpersona_requiere_sarlaft = Dtl.ConvertTo<Mpersona_requiere_sarlaft>(DsReturn.Tables[2])[0]; }

                            int rowcount = 1;
                            if (DsReturn.Tables[3].Rows.Count > 0)
                            {
                                dtoUser.List_Mpersona_dir = Dtl.ConvertTo<Mpersona_dir>(DsReturn.Tables[3]);
                                dtoUser.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                                dtoUser.List_Mpersona_dir.ForEach(delegate(Mpersona_dir element) { element.cod_tipo_dir_old = element.cod_tipo_dir; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
                                dtoUser.List_Mpersona_dir = datos.dtoDataAddress3g(Convert.ToInt32(id_persona), dtoUser.List_Mpersona_dir);

                            }

                            rowcount = 1;
                            if (DsReturn.Tables[4].Rows.Count > 0)
                            {
                                dtoUser.List_Mpersona_telef = Dtl.ConvertTo<Mpersona_telef>(DsReturn.Tables[4]);
                                dtoUser.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.Identity = rowcount++; element.State = 'R'; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                                dtoUser.List_Mpersona_telef.ForEach(delegate(Mpersona_telef element) { element.cod_tipo_telef_old = element.cod_tipo_telef; });
                                //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
                                dtoUser.List_Mpersona_telef = datos.dtoDataPhone3g(Convert.ToInt32(id_persona), dtoUser.List_Mpersona_telef);
                            }

                            rowcount = 1;
                            if (DsReturn.Tables[5].Rows.Count > 0)
                            {
                                dtoUser.List_Mpersona_email = Dtl.ConvertTo<Mpersona_email>(DsReturn.Tables[5]);
                                dtoUser.List_Mpersona_email.ForEach(delegate(Mpersona_email element) { element.Identity = rowcount++; element.State = 'R'; });
                                dtoUser.List_Mpersona_email = datos.dtoDataMail3g(Convert.ToInt32(id_persona), dtoUser.List_Mpersona_email);
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[6].Rows.Count > 0)
                            {
                                dtoUser.List_Logbook = Dtl.ConvertTo<LOGBOOK>(DsReturn.Tables[6]);
                                dtoUser.List_Logbook.ForEach(delegate(LOGBOOK element) { element.Identity = rowcount++; element.State = 'R'; });
                            }

                            rowcount = 0;
                            if (DsReturn.Tables[7].Rows.Count > 0)
                            {
                                dtoSarlaft.List_detalle = Dtl.ConvertTo<Frm_sarlaft_detalle>(DsReturn.Tables[7]);
                                dtoSarlaft.List_detalle.ForEach(delegate(Frm_sarlaft_detalle element) { element.Identity = rowcount++; element.State = 'R'; });
                            }
                            if (DsReturn.Tables[8].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_vinculos = Dtl.ConvertTo<Frm_sarlaft_vinculos>(DsReturn.Tables[8])[0]; }
                            if (DsReturn.Tables[9].Rows.Count > 0) { dtoSarlaft.frm_sarlaft_aut_incrementos = Dtl.ConvertTo<Frm_sarlaft_aut_incrementos>(DsReturn.Tables[9])[0]; }

                        }
                    }

                    dtoUser.dtoDataUser = dtoDataUser;
                    dtoUser.dtoSarlaft = dtoSarlaft;
                    ReturnData.DtoUser = dtoUser;
                    break;
            }
            return ReturnData;

        }
        catch
        {
            throw;
        }
    }
}

