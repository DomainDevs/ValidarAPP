using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    [DataContract]
    public class ResponseSearch
    {
        #region Data Members

        int? _identity;
        int? _idRol;
        string _Rol;
        string _codigo;
        int? _idPersona;
        string _tipodoc;
        string _documento;
        string _nombre_rs;
        string _tipopersona;
        int _codAbona;
        int _codtipodoc;
        string _activo;
        string _message;
        string _apellido1;
        string _apellido2;
        string _nombre;
        int _codigoCiiu;
        int _codSucursal;

        #endregion

        #region Properties

        [DataMember(EmitDefaultValue = false)]
        public int? Identity
        {
            get { return _identity; }
            set { _identity = value; }
        }


        [DataMember(EmitDefaultValue = false)]
        public int? IdRol
        {
            get { return _idRol; }
            set { _idRol = value; }
        }

        [DataMember(EmitDefaultValue = false)]
        public string Rol
        {
            get { return _Rol; }
            set { _Rol = value; }
        }

        [DataMember(EmitDefaultValue = false)]
        public string Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        [DataMember(EmitDefaultValue = false)]
        public int? IdPersona
        {
            get { return _idPersona; }
            set { _idPersona = value; }
        }

        [DataMember(EmitDefaultValue = false)]
        public string TipoDoc
        {
            get { return _tipodoc; }
            set { _tipodoc = value; }
        }

        [DataMember(EmitDefaultValue = false)]
        public string Documento
        {
            get { return _documento; }
            set { _documento = value; }
        }

        [DataMember(EmitDefaultValue = false)]
        public string NombreRS
        {
            get { return _nombre_rs; }
            set { _nombre_rs = value; }
        }

        [DataMember(EmitDefaultValue = false)]
        public string TipoPersona
        {
            get { return _tipopersona; }
            set { _tipopersona = value; }
        }

        [DataMember(EmitDefaultValue = false)]
        public int CodAbona
        {
            get { return _codAbona; }
            set { _codAbona = value; }
        }

        [DataMember(EmitDefaultValue = false)]
        public int CodTipoDoc
        {
            get { return _codtipodoc; }
            set { _codtipodoc = value; }
        }

        [DataMember(EmitDefaultValue = false)]
        public string Activo
        {
            get { return _activo; }
            set { _activo = value; }
        }

        [DataMember(EmitDefaultValue = false)]
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        [DataMember(EmitDefaultValue = false)]
        public string Apellido1
        {
            get { return _apellido1; }
            set { _apellido1 = value; }
        }

        [DataMember(EmitDefaultValue = false)]
        public string Apellido2
        {
            get { return _apellido2; }
            set { _apellido2 = value; }
        }

        [DataMember(EmitDefaultValue = false)]
        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        [DataMember(EmitDefaultValue = false)]
        public int CodigoCiiu
        {
            get { return _codigoCiiu; }
            set { _codigoCiiu = value; }
        }

        [DataMember(EmitDefaultValue = false)]
        public int CodSucursal
        {
            get { return _codSucursal; }
            set { _codSucursal = value; }
        }

        #endregion

    }
}
