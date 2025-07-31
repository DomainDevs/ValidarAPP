using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sistran.Co.Previsora.Application.FullServices.Models;
using Sybase.Data.AseClient;

namespace Sistran.Co.Previsora.Application.FullServicesProvider.Provider
{
    public class FacadeDto
    {
        #region constructor

        public FacadeDto(string user, int idApp, int idRol)
        {
            User = user;
            IdApp = idApp;
            IdRol = idRol;

            _GSDtoinsured = new GSDtoInsured(user, idApp, idRol);
            _GSDtoLawyer = new GSDtoLawyer(user, idApp, idRol);
            _GSDtoBeneficiary = new GSDtoBeneficiary(user, idApp, idRol);
            _GSDtoAssigneed = new GSDtoAssigneed(user, idApp, idRol);
            _GSDtoPrincipalNational = new GSDtoPrincipalNational(user, idApp, idRol);
            _GSDtoPrincipalComertial = new GSDtoPrincipalComertial(user, idApp, idRol);
            _GSDtoTechnicalAssistant = new GSDtoTechnicalAssistant(user, idApp, idRol);   
            _GSDtoProvider = new GSDtoProvider(user, idApp, idRol);
            _GSDtoEmployee = new GSDtoEmployee(user, idApp, idRol);
            _GSDtoThird = new GSDtoThird(user, idApp, idRol);
            _GSDtoUser = new GSDtoUser(user, idApp, idRol);
            _GSDtoAgent = new GSDtoAgent(user, idApp, idRol); 

        }

        #endregion

        #region Propiedades

        private static string User { get; set; }
        private static int IdApp { get; set; }
        private static int IdRol { get; set; }

        private GSDtoInsured _GSDtoinsured;
        private GSDtoLawyer _GSDtoLawyer;
        private GSDtoBeneficiary _GSDtoBeneficiary;
        private GSDtoAssigneed _GSDtoAssigneed;
        private GSDtoPrincipalNational _GSDtoPrincipalNational;
        private GSDtoPrincipalComertial _GSDtoPrincipalComertial;
        private GSDtoTechnicalAssistant _GSDtoTechnicalAssistant; 
        private GSDtoProvider _GSDtoProvider;
        private GSDtoEmployee _GSDtoEmployee;
        private GSDtoThird _GSDtoThird;
        private GSDtoUser _GSDtoUser;
        private GSDtoAgent _GSDtoAgent;


        #endregion

        #region Funciones y Procedimientos

        public DtoMaster GetDto(string cod_rol, string entity)
        {
            DtoMaster ReturnData = new DtoMaster();
            ReturnData.cod_Rol = cod_rol;
            switch (IdRol)
            {
                case 1:
                    ReturnData.DtoInsured = _GSDtoinsured.GetDtoInsured(IdRol, cod_rol, entity);
                    break;
                case 2:
                    ReturnData.DtoLawyer = _GSDtoLawyer.GetDtoLawyer(IdRol, cod_rol, entity);
                    break;
                case 3:
                    ReturnData.DtoBeneficiary = _GSDtoBeneficiary.GetDtoBeneficiary(IdRol, cod_rol, entity);
                    break;
                case 4:
                    ReturnData.DtoAssigneed = _GSDtoAssigneed.GetDtoAssigneed(IdRol, cod_rol, entity);
                    break;
                case 5:
                    ReturnData.DtoPrincipalNational = _GSDtoPrincipalNational.GetDtoPrincipalNational(IdRol, cod_rol, entity);
                    break;
                case 6:
                    ReturnData.DtoPrincipalComertial = _GSDtoPrincipalComertial.GetDtoPrincipalComertial(IdRol, cod_rol, entity);
                    break;
                case 7:
                    ReturnData.DtoTechnicalAssistant = _GSDtoTechnicalAssistant.GetDtoTechnicalAssistant(IdRol, cod_rol, entity);
                    break;
                case 8:
                    ReturnData.DtoEmployee = _GSDtoEmployee.GetDtoEmployee(IdRol, cod_rol, entity);
                    break;
                case 9:
                    ReturnData.DtoAgent = _GSDtoAgent.GetDtoAgent(IdRol, cod_rol, entity);
                    break;
                case 10:
                    ReturnData.DtoProvider = _GSDtoProvider.GetDtoProvider(IdRol, cod_rol, entity);
                    break;
                case 11:
                    ReturnData.DtoThird = _GSDtoThird.GetDtoThird(IdRol, cod_rol, entity);
                    break;
                case 12:
                    ReturnData.DtoUser = _GSDtoUser.GetDtoUser(IdRol, cod_rol, entity);
                    break;
            }
            return ReturnData;
        }

        public SUPMessages SetDto(DtoMaster dtoMaster)
        {
            SUPMessages _SUPMessages = new SUPMessages();
            switch (IdRol)
            {
                case 1:
                    _SUPMessages = _GSDtoinsured.SetDtoInsured(dtoMaster);
                    break;
                case 2:
                    _SUPMessages = _GSDtoLawyer.SetDtoLawyer(dtoMaster);
                    break;
                case 3:
                    _SUPMessages = _GSDtoBeneficiary.SetDtoBeneficiary(dtoMaster);
                    break;
                case 4:
                    _SUPMessages = _GSDtoAssigneed.SetDtoAssigneed(dtoMaster);
                    break;
                case 5:
                    _SUPMessages = _GSDtoPrincipalNational.SetDtoPrincipalNational(dtoMaster);
                    break;
                case 6:
                    _SUPMessages = _GSDtoPrincipalComertial.SetDtoPrincipalComertial(dtoMaster);
                    break;
                case 7:
                    _SUPMessages = _GSDtoTechnicalAssistant.SetDtoTechnicalAssistant(dtoMaster);
                    break;
                case 8:
                    _SUPMessages = _GSDtoEmployee.SetDtoEmployee(dtoMaster);
                    break;
                case 9:
                    _SUPMessages = _GSDtoAgent.SetDtoAgent(dtoMaster);
                    break;
                case 10:
                    _SUPMessages = _GSDtoProvider.SetDtoProvider(dtoMaster);
                    break;
                case 11:
                    _SUPMessages = _GSDtoThird.SetDtoThird(dtoMaster);
                    break;
                case 12:
                    _SUPMessages = _GSDtoUser.SetDtoUser(dtoMaster);
                    break;
            }
            return _SUPMessages;
        }

        #endregion

    }
}
