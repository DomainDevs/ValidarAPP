using Sistran.Co.Previsora.Application.FullServices;
using Sistran.Co.Previsora.Application.FullServices.Models;
using Sistran.Co.Previsora.Application.FullServicesProvider.DataLayer;
using Sistran.Co.Previsora.Application.FullServicesProvider.Helpers;
using Sistran.Co.Previsora.Application.FullServicesProvider.Provider;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Sistran.Co.Previsora.Application.FullServicesProvider
{
    public class FullServicesProviderSUP : IFullServicesSUP
    {

        #region propiedades

        private string PrefixConn = ConfigurationManager.AppSettings["SUPAPP"];
        OperatingQuotaHelper operatingQuotaHelper = new OperatingQuotaHelper();

        #endregion

        #region Funciones y Procedimientos

        public bool GetStatusAplication(StatusAplication statusaplication)
        {
            DataGenericExecute DGE = new DataGenericExecute(PrefixConn);
            return DGE.GetStatusAplication(statusaplication);
        }

        public DtoMaster GetDto(int id_rol, string cod_rol, string entity)
        {
            DtoMaster ReturnData = new DtoMaster();
            if (entity == "SUP.GET_EXIST_PERSON")
            {
                FullServicesHelper FS = new FullServicesHelper();
                ReturnData = FS.GetExitsPerson(id_rol, cod_rol);
            }
            else
            {
                FacadeDto facadeDto = new FacadeDto("", 0, id_rol);
                ReturnData = facadeDto.GetDto(cod_rol, entity);
            }
            return ReturnData;
        }

        public SUPMessages SetDto(int id_rol, DtoMaster MasterDto)
        {

            FacadeDto facadeDto = new FacadeDto(MasterDto.User, MasterDto.IdApp, id_rol);
            SUPMessages _SUPMessages = new SUPMessages();
            _SUPMessages = facadeDto.SetDto(MasterDto);
            return _SUPMessages;
        }

        public List<RoleView> GetlistViews(int idRole)
        {
            DataGenericExecute DGE = new DataGenericExecute(PrefixConn);
            return DGE.GetlistViews(idRole);
        }

        public List<ResponseSearch> SearchPerson(ResquestSearch resquestSearch)
        {
            DataGenericExecute DGE = new DataGenericExecute(PrefixConn);
            List<ResponseSearch> ListResponseSearch = DGE.SearchPerson(resquestSearch);
            DataSet ds = new DataSet();

            if (resquestSearch.tipo_doc != 5 && resquestSearch.tipo_doc != null && resquestSearch.tipo_doc != 0)
            {
                if (ListResponseSearch.Count == 0)
                {
                    //tabla de documento/
                    ds = typedoc();
                    DatatableToList Dtl = new DatatableToList();
                    List<CIFIN_DOCUMENTTYPE> listtipodoc = new List<CIFIN_DOCUMENTTYPE>();
                    listtipodoc = Dtl.ConvertTo<CIFIN_DOCUMENTTYPE>(ds.Tables[0]);

                    //tabla estado del documento Cifin
                    ds = StateDoc();
                    List<CIFIN_STATEDOCUMENT> listStatedoc = new List<CIFIN_STATEDOCUMENT>();
                    listStatedoc = Dtl.ConvertTo<CIFIN_STATEDOCUMENT>(ds.Tables[0]);

                    //tabla de fechas maximas y contador
                    ds = dateMax();
                    List<CIFIN_DATEMAX> listdateMax = new List<CIFIN_DATEMAX>();
                    listdateMax = Dtl.ConvertTo<CIFIN_DATEMAX>(ds.Tables[0]);

                    //tabla usuario para obtener el codigo del usuario
                    DataSet codeuser = new DataSet();
                    codeuser = CodUser(resquestSearch.ID_user);

                    //Busco en tablas de Log y valido tiempo de ultima busqueda
                    List<Parameters> listParameter = new List<Parameters>();
                    Parameters parameter = new Parameters();
                    parameter.ParameterType = "AseDbType.VarChar";
                    parameter.Parameter = "tipo_doc";
                    parameter.Value = (resquestSearch.tipo_doc != null ? resquestSearch.tipo_doc.ToString() : null);
                    listParameter.Add(parameter);

                    parameter = new Parameters();
                    parameter.ParameterType = "AseDbType.VarChar";
                    parameter.Parameter = "number_doc";

                    if (resquestSearch.tipo_doc == 2)
                    {
                        if (resquestSearch.nro_documento != null)
                            parameter.Value = resquestSearch.nro_documento.Substring(0, resquestSearch.nro_documento.Length - 1);
                    }
                    else
                        parameter.Value = (resquestSearch.nro_documento != null ? resquestSearch.nro_documento.ToString() : null);

                    listParameter.Add(parameter);
                    ds = DGE.ExecuteStoreProcedure("SUP.GET_LOGCIFIN", listParameter);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DateTime old = (ds.Tables[0].Rows.Count != 0 ? Convert.ToDateTime(ds.Tables[0].Rows[0][3]) : Convert.ToDateTime(null));
                        int diasmax = listdateMax.Select(x => x.SubCon).First();
                        DateTime LeftOverdate = old.AddDays(diasmax);

                        if (LeftOverdate <= DateTime.Now)
                        {
                            SavePersonCifin(resquestSearch, listdateMax, listtipodoc, listStatedoc, ds, ref ListResponseSearch);
                        }
                        else
                        {
                            CIFIN_DOCUMENTTYPE resu = listtipodoc.Find(delegate(CIFIN_DOCUMENTTYPE bk) { return bk.cod_tipo_doc_sise == Convert.ToDouble(resquestSearch.tipo_doc); });
                            ResponseSearch rs = new ResponseSearch();
                            rs.Rol = "Externo";
                            rs.NombreRS = (ds.Tables[0].Rows[0][17] + " " + ds.Tables[0].Rows[0][18] + " " + ds.Tables[0].Rows[0][16]);
                            rs.CodTipoDoc = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                            rs.Documento = Convert.ToString(ds.Tables[0].Rows[0][2]);
                            rs.TipoDoc = resu.txt_desc_sise;
                            rs.Apellido1 = Convert.ToString(ds.Tables[0].Rows[0][17]);
                            rs.Apellido2 = Convert.ToString(ds.Tables[0].Rows[0][18]);
                            rs.Nombre = Convert.ToString(ds.Tables[0].Rows[0][16]);
                            string ciiu = Convert.ToString(ds.Tables[0].Rows[0][9]);
                            rs.CodigoCiiu = (ciiu.Trim() == string.Empty ? 0 : Convert.ToInt32(ciiu));
                            if (codeuser.Tables[0].Rows.Count > 0)
                            {
                                rs.CodSucursal = Convert.ToInt32(codeuser.Tables[0].Rows[0][0]);
                            }
                            else
                            {
                                rs.CodSucursal = 0;
                            }

                            //Edward Rubiano -- HD3555 -- 26/11/2015
                            //if (resquestSearch.tipo_doc != 2 && resquestSearch.tipo_doc != 4 && resquestSearch.tipo_doc != 6)
                            if (resquestSearch.tipo_doc != 2)
                            //Edward Rubiano -- 26/11/2015
                            {
                                int cod_doc = 0;
                                if (ds.Tables[0].Rows[0][19].ToString().Trim() != string.Empty)
                                    cod_doc = Convert.ToInt32(ds.Tables[0].Rows[0][19]);

                                int[] ArrayStateDocument = { 0, 12, 77, 88 };
                                if (!ArrayStateDocument.Contains(cod_doc))
                                {
                                    CIFIN_STATEDOCUMENT resulstate = listStatedoc.Find(delegate(CIFIN_STATEDOCUMENT bk) { return bk.cod_doc == Convert.ToInt32(cod_doc); });
                                    if (resulstate != null)
                                        rs.Message = "El estado del documento es" + " " + resulstate.txt_desc + " " + "según registraduría";

                                }
                            }
                            //Edward Rubiano -- HD3555 -- 26/11/2015
                            //else if (resquestSearch.tipo_doc == 4 || resquestSearch.tipo_doc == 2)
                            else if (resquestSearch.tipo_doc == 2)
                            //Edward Rubiano -- 26/11/2015
                            {
                                rs.NombreRS = Convert.ToString(ds.Tables[0].Rows[0][8]);
                                rs.Nombre = Convert.ToString(ds.Tables[0].Rows[0][8]);

                                if (Convert.ToString(ds.Tables[0].Rows[0][11]) != "")
                                {
                                    CIFIN_STATEDOCUMENT resulstate = listStatedoc.Find(delegate(CIFIN_STATEDOCUMENT bk) { return bk.txt_desc == Convert.ToString(ds.Tables[0].Rows[0][11]); });
                                    if (resulstate != null)
                                        rs.Message = "El estado del documento es" + " " + resulstate.txt_desc + " " + "según registraduría";
                                }
                            }
                            ListResponseSearch.Add(rs);
                        }
                    }
                    else if (ds.Tables[0].Rows.Count == 0)
                    {
                        SavePersonCifin(resquestSearch, listdateMax, listtipodoc, listStatedoc, ds, ref ListResponseSearch);
                    }
                }
            }

            if (ListResponseSearch.Count == 0)
            {
                //tabla usuario para obtener el codigo del usuario
                DataSet codeuser = new DataSet();
                codeuser = CodUser(resquestSearch.ID_user);
                ResponseSearch rs = new ResponseSearch();
                if (codeuser.Tables[0].Rows.Count > 0)
                {
                    rs.CodSucursal = Convert.ToInt32(codeuser.Tables[0].Rows[0][0]);
                }
                else
                {
                    rs.CodSucursal = 0;
                }
                ListResponseSearch.Add(rs);
            }

            return ListResponseSearch;
        }

        private void SavePersonCifin(ResquestSearch resquestSearch, List<CIFIN_DATEMAX> listdateMax, List<CIFIN_DOCUMENTTYPE> listtipodoc,
            List<CIFIN_STATEDOCUMENT> listStatedoc, DataSet ds, ref List<ResponseSearch> ListResponseSearch)
        {
            DataGenericExecute DGE = new DataGenericExecute(PrefixConn);
            int numeromax = listdateMax.Select(x => x.LeftOver).First();

            //tabla usuario para obtener el codigo del usuario
            DataSet codeuser = new DataSet();
            codeuser = CodUser(resquestSearch.ID_user);
           
            if (ds.Tables[0].Rows.Count == 0 && numeromax > 0 || ds.Tables[0].Rows.Count > 0)
            {
                //Si no registro o no cumple llama a funcion
                try
                {
                    if (resquestSearch.tipo_doc == 6)
                        resquestSearch.tipo_doc = 9;

                    // funcion de equivalencia del tipo doc
                    string tipodoc = resquestSearch.tipo_doc.ToString();
                    string numeroident;
                    if (resquestSearch.tipo_doc == 2)
                        numeroident = resquestSearch.nro_documento.Substring(0, resquestSearch.nro_documento.Length - 1);
                    else
                        numeroident = resquestSearch.nro_documento;

                    
                    return;
                    ///
                    /// TODO: Esta consulta (y el tratamiento de los datos) se omite, ya que actualmente no se tiene acceso a los servidores de CIFIN
                    /// Edward Bustos: 10/09/2018
                    string Result = DGE.consultarInformacionComercial(tipodoc, numeroident);
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(Result); // suppose that myXmlString contains "<Names>...</Names>"

                    if (xml.DocumentElement.SelectNodes("/CIFIN/Tercero").Count == 1)
                    {
                        XmlNodeList xnList = xml.SelectNodes("/CIFIN/Tercero");
                        if (xnList.Count > 0)
                        {
                            foreach (XmlNode xn in xnList)
                            {
                                if (xn["RespuestaConsulta"].InnerText != "01")
                                {
                                    CIFIN_DOCUMENTTYPE result = listtipodoc.Find(delegate(CIFIN_DOCUMENTTYPE bk) { return bk.txt_desc_redu == xn["TipoIdentificacion"].InnerText; });
                                    ResponseSearch rs = new ResponseSearch();
                                    // convertir al tipo sise            
                                    rs.Rol = "EXTERNO";
                                    rs.CodTipoDoc = Convert.ToInt32(result.cod_tipo_doc_sise);
                                    rs.TipoDoc = result.txt_desc_sise;
                                    rs.Documento = xn["NumeroIdentificacion"].InnerText;
                                    if (resquestSearch.tipo_doc != 2 && resquestSearch.tipo_doc != 4 && resquestSearch.tipo_doc != 6)
                                    {
                                        rs.NombreRS = xn["Nombre1"].InnerText + " " +
                                        xn["Nombre2"].InnerText + " " +
                                        xn["De"].InnerText + " " +
                                        xn["Apellido1"].InnerText + " " +
                                        xn["Apellido2"].InnerText;
                                        rs.Apellido1 = xn["Apellido1"].InnerText;
                                        rs.Apellido2 = xn["De"].InnerText + " " + xn["Apellido2"].InnerText;
                                        rs.Nombre = xn["Nombre1"].InnerText + " " + xn["Nombre2"].InnerText;

                                        if (xn["CodigoEstado"].InnerText != "")
                                        {
                                            int[] ArrayStateDocument = { 0, 12, 77, 88 };
                                            if (!ArrayStateDocument.Contains(Convert.ToInt32(xn["CodigoEstado"].InnerText)))
                                            {
                                                CIFIN_STATEDOCUMENT resulstate = listStatedoc.Find(delegate(CIFIN_STATEDOCUMENT bk) { return bk.cod_doc == Convert.ToInt32(xn["CodigoEstado"].InnerText); });
                                                if (resulstate != null)
                                                    rs.Message = "El estado del documento es" + " " + resulstate.txt_desc + " " + "según registraduría";

                                            }
                                        }
                                    }
                                    else if (resquestSearch.tipo_doc == 4 || resquestSearch.tipo_doc == 2)
                                    {
                                        rs.NombreRS = xn["NombreTitular"].InnerText;
                                        rs.Nombre = xn["NombreTitular"].InnerText;

                                        if (xn["Estado"].InnerText != "")
                                        {
                                            CIFIN_STATEDOCUMENT resulstate = listStatedoc.Find(delegate(CIFIN_STATEDOCUMENT bk) { return bk.txt_desc == xn["Estado"].InnerText; });
                                            if (resulstate != null)
                                                rs.Message = "El estado del documento es" + " " + resulstate.txt_desc + " " + "según registraduría";
                                        }
                                    }

                                    rs.CodigoCiiu = Convert.ToInt32(xn["CodigoCiiu"] == null || xn["CodigoCiiu"].InnerText == "" ? " 0" : xn["CodigoCiiu"].InnerText);

                                    if (codeuser.Tables[0].Rows.Count > 0)
                                    {
                                        rs.CodSucursal = Convert.ToInt32(codeuser.Tables[0].Rows[0][0]);
                                    }
                                    else
                                    {
                                        rs.CodSucursal = 0;
                                    }
                                    ListResponseSearch.Add(rs);
                                    //insertar en la tabla log el resultado de cifin
                                    CIFIN_LOG log = new CIFIN_LOG();
                                    log.cod_tipo_doc = Convert.ToString(result.cod_tipo_doc_sise);
                                    log.number_doc = xn["NumeroIdentificacion"].InnerText;
                                    log.request_date = Convert.ToString(DateTime.Now);
                                    log.user_log = resquestSearch.ID_user;
                                    log.IdentificadorLinea = xn["IdentificadorLinea"].InnerText;
                                    log.CodigoTipoIndentificacion = xn["CodigoTipoIndentificacion"] != null ? xn["CodigoTipoIndentificacion"].InnerText : null;
                                    log.NumeroIdentificacion = xn["NumeroIdentificacion"] != null ? xn["NumeroIdentificacion"].InnerText : null;
                                    log.NombreTitular = xn["NombreTitular"] != null ? xn["NombreTitular"].InnerText : null;
                                    log.CodigoCiiu = xn["CodigoCiiu"] != null ? xn["CodigoCiiu"].InnerText : null;
                                    log.NombreCiiu = xn["NombreCiiu"] != null ? xn["NombreCiiu"].InnerText : null;
                                    log.Estado = xn["Estado"] != null ? xn["Estado"].InnerText : null;
                                    log.Fecha = xn["Fecha"] != null ? xn["Fecha"].InnerText : null;
                                    log.Hora = xn["Hora"] != null ? xn["Hora"].InnerText : null;
                                    log.Entidad = xn["Entidad"] != null ? xn["Entidad"].InnerText : null;
                                    log.RespuestaConsulta = xn["RespuestaConsulta"] != null ? xn["RespuestaConsulta"].InnerText : null;
                                    //Edward Rubiano -- HD3555 -- 26/11/2015
                                    //if (resquestSearch.tipo_doc != 4 && resquestSearch.tipo_doc != 2)
                                    if (resquestSearch.tipo_doc != 2)
                                    //Edward Rubiano -- 26/11/2015
                                    {
                                        //Edward Rubiano - 26/09/2016 - PRE-410 - Se valida que los datos vengan nulos
                                        /*log.Nombre = xn["Nombre1"].InnerText + " " + xn["Nombre2"].InnerText;
                                        log.Apellido1 = xn["Apellido1"].InnerText;
                                        log.Apellido2 = xn["De"].InnerText + " " + xn["Apellido2"].InnerText;
                                        log.CodigoEstado = xn["CodigoEstado"].InnerText;*/
                                        log.Nombre = ((xn["Nombre1"] != null) ? xn["Nombre1"].InnerText : "") + " " + ((xn["Nombre2"] != null) ? xn["Nombre2"].InnerText : "");
                                        log.Apellido1 = (xn["Apellido1"] != null) ? xn["Apellido1"].InnerText : "";
                                        log.Apellido2 = ((xn["De"] != null) ? xn["De"].InnerText : "") + " " + ((xn["Apellido2"] != null) ? xn["Apellido2"].InnerText : "");
                                        log.CodigoEstado = (xn["CodigoEstado"] != null) ? xn["CodigoEstado"].InnerText : "";
                                        //Edward Rubiano - 26/09/2016 - PRE-410 - Se valida que los datos vengan nulos
                                    }
                                    DGE.InsertLOG(log);


                                    string oldDate = listdateMax.Select(x => x.DateMonthMax).First();
                                    DateTime oldate = Convert.ToDateTime(oldDate.ToDateTime().ToString("d"));
                                    DateTime newDate = Convert.ToDateTime(DateTime.Now.ToString("d"));
                                    int ContMax = listdateMax.Select(x => x.ContMax).First();
                                    int LeftOver = listdateMax.Select(x => x.LeftOver).First();
                                    int SubCon = listdateMax.Select(x => x.SubCon).First();

                                    if (newDate < oldate)
                                    {
                                        int incont = listdateMax.Select(x => x.ContMax).First();
                                        CIFIN_DATEMAX CifinDateMax = new CIFIN_DATEMAX();
                                        CifinDateMax.Id = 1;
                                        CifinDateMax.DateMonthMax = Convert.ToString(oldate);
                                        CifinDateMax.LeftOver = LeftOver - 1;
                                        CifinDateMax.ContMax = ContMax;
                                        CifinDateMax.SubCon = SubCon;
                                        DGE.UpdateCont(CifinDateMax);
                                    }
                                    else
                                    {
                                        DateTime lastDay = new DateTime(newDate.Year, newDate.Month, 1).AddMonths(1).AddDays(-1);
                                        CIFIN_DATEMAX CifinDateMax = new CIFIN_DATEMAX();
                                        CifinDateMax.Id = 1;
                                        CifinDateMax.DateMonthMax = Convert.ToString(lastDay);
                                        CifinDateMax.LeftOver = ContMax;
                                        CifinDateMax.ContMax = ContMax;
                                        CifinDateMax.SubCon = SubCon;
                                        DGE.UpdateCont(CifinDateMax);
                                    }
                                }
                                else
                                {
                                    ERROR_CIFIN ec = new ERROR_CIFIN();
                                    // convertir al tipo sise            
                                    ec.CodigoError = 101;
                                    ec.MensajeError = xml.InnerXml.ToString();
                                    ec.CodigoInformacion = "153";
                                    ec.MotivoConsulta = "22";
                                    ec.NumeroIdentificacion = xn["NumeroIdentificacion"].InnerText;
                                    ec.TipoIdentificacion = xn["TipoIdentificacion"].InnerText;
                                    DGE.InsertError(ec);
                                }
                            }
                        }
                    }

                    if (xml.DocumentElement.SelectNodes("/CifinError/Error").Count == 1)
                    {
                        XmlNodeList xnList = xml.SelectNodes("/CifinError/Error");
                        if (xnList.Count > 0)
                        {
                            foreach (XmlNode xn in xnList)
                            {
                                ERROR_CIFIN ec = new ERROR_CIFIN();
                                // convertir al tipo sise            
                                ec.CodigoError = Convert.ToInt32(xn["CodigoError"].InnerText);
                                ec.MensajeError = xn["MensajeError"].InnerText;
                                ec.CodigoInformacion = xn["CodigoInformacion"].InnerText;
                                ec.MotivoConsulta = xn["MotivoConsulta"].InnerText;
                                ec.NumeroIdentificacion = xn["NumeroIdentificacion"].InnerText;
                                ec.TipoIdentificacion = xn["TipoIdentificacion"].InnerText;
                                DGE.InsertError(ec);

                                ResponseSearch rs = new ResponseSearch();
                                rs.Message = "La fuente de consulta externa no está disponible por favor informe al administrador";
                                if (codeuser.Tables[0].Rows.Count > 0)
                                {
                                    rs.CodSucursal = Convert.ToInt32(codeuser.Tables[0].Rows[0][0]);
                                }
                                else
                                {
                                    rs.CodSucursal = 0;
                                }
                                ListResponseSearch.Add(rs);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ERROR_CIFIN ec = new ERROR_CIFIN();
                    //ingresar error cifin a la tabla de error         
                    ec.CodigoError = 100;
                    ec.MensajeError = ex.Message;
                    ec.CodigoInformacion = "153";
                    ec.MotivoConsulta = "22";
                    ec.NumeroIdentificacion = resquestSearch.nro_documento;
                    ec.TipoIdentificacion = resquestSearch.tipo_doc.ToString();
                    ec.Operacion = "";
                    DGE.InsertError(ec);

                    ResponseSearch rs = new ResponseSearch();
                    rs.Message = "La fuente de consulta externa no está disponible por favor informe al administrador";
                    if (codeuser.Tables[0].Rows.Count > 0)
                    {
                        rs.CodSucursal = Convert.ToInt32(codeuser.Tables[0].Rows[0][0]);
                    }
                    else
                    {
                        rs.CodSucursal = 0;
                    }
                    ListResponseSearch.Add(rs);
                }

            }

        }

        public DataSet typedoc()
        {
            DataGenericExecute DGE = new DataGenericExecute(PrefixConn);
            DataSet ds = new DataSet();
            List<Parameters> listParameter = new List<Parameters>();
            ds = DGE.ExecuteStoreProcedure("SUP.GET_TYPEDOC", listParameter);
            return ds;
        }

        public DataSet dateMax()
        {
            DataGenericExecute DGE = new DataGenericExecute(PrefixConn);
            DataSet ds = new DataSet();
            List<Parameters> listParameter = new List<Parameters>();
            ds = DGE.ExecuteStoreProcedure("SUP.GET_DATEMAX", listParameter);
            return ds;
        }

        public DataSet StateDoc()
        {
            DataGenericExecute DGE = new DataGenericExecute(PrefixConn);
            DataSet ds = new DataSet();
            List<Parameters> listParameter = new List<Parameters>();
            ds = DGE.ExecuteStoreProcedure("SUP.GET_STATEDOCUMENT", listParameter);
            return ds;
        }

        public DataSet CodUser(string user)
        {
            DataGenericExecute DGE = new DataGenericExecute(PrefixConn);
            DataSet ds = new DataSet();
            List<Parameters> listParameter = new List<Parameters>();
            Parameters parameter = new Parameters();
            parameter.ParameterType = "AseDbType.VarChar";
            parameter.Parameter = "USER";
            parameter.Value = (user);
            listParameter.Add(parameter);
            ds = DGE.ExecuteStoreProcedure("SUP.GET_CODEUSER", listParameter);
            return ds;
        }
        
        public Object CreateObject(string XMLString, Object YourClassObject)
        {
            XmlSerializer oXmlSerializer = new XmlSerializer(YourClassObject.GetType());

            YourClassObject = oXmlSerializer.Deserialize(new StringReader(XMLString));

            return YourClassObject;
        }

        public DataSet GenericQuery(string generic, List<Parameters> ListParameter)
        {
            DataSet ds = new DataSet();
            string[] data = generic.Split('|');
            if (data.Length == 2)
            {
                DataGenericExecute DGE = new DataGenericExecute(data[1]);
                return DGE.ExecuteStoreProcedure(data[0], ListParameter);
            }
            else
            {
                DataGenericExecute DGE = new DataGenericExecute(PrefixConn);
                return DGE.ExecuteStoreProcedure(generic, ListParameter);
            }
        }

        #endregion

        #region OperatingQuota 
        public double GetExchangeRateDate(DateTime operatingQuotaExchangeRateDate, int currencyCd)
        {

            return operatingQuotaHelper.GetExchangeRateDate(operatingQuotaExchangeRateDate, currencyCd);

        }

        public List<OperatingQuotaIndividual> GetIndividualOperatingQuota(int identificationType, string identificationId)
        {
            return operatingQuotaHelper.GetIndividualOperatingQuota(identificationType, identificationId);
        }

        public List<TableMessage> ModifyOperatingQuota(List<OPERATING_QUOTA> operatingQuota, List<TableMessage> listTableMessage)
        {
            return operatingQuotaHelper.ModifyOperatingQuotaProcess(operatingQuota, listTableMessage);
        }

        public OperatingQuotaResponse RegisterOperativeQuota(WSOperatingQuota operatingQuota)
        {
            return operatingQuotaHelper.RegisterOperativeQuota(operatingQuota);
        }



        #endregion
    }

}
