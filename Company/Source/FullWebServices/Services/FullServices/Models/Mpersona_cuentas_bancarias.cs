using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Mpersona_cuentas_bancarias
	{

		#region InnerClass
		public enum Mpersona_cuentas_bancariasFields
		{
			cod_tipo_red,
			cod_plaza,
			cod_banco,
			id_persona,
			cod_tipo_cta_bco,
			cod_moneda,
			txt_nro_cta,
			txt_nombre_titular,
			sn_activa_cuenta,
			sn_default_cuenta,
			txt_nombre_email,
			txt_dominio_email,
			txt_direccion_email,
			cod_usuario,
			fec_registro
		}
		#endregion

		#region Data Members

			int _cod_tipo_red;
			string _cod_plaza;
			double _cod_banco;
			int _id_persona;
			double _cod_tipo_cta_bco;
			double _cod_moneda;
			string _txt_nro_cta;
			string _txt_nombre_titular;
			int _sn_activa_cuenta;
			int _sn_default_cuenta;
			string _txt_nombre_email;
			string _txt_dominio_email;
			string _txt_direccion_email;
			string _cod_usuario;
			string _fec_registro;
			int _identity; 
			char _state; 
			string _connection;
            string _red;
            string _banco;
            string _txt_plaza;
            string _tipo;
            string _moneda;

		#endregion

		#region Properties

		[DataMember]
		public int  cod_tipo_red
		{
			 get { return _cod_tipo_red; }
			 set {_cod_tipo_red = value;}
		}

		[DataMember]
		public string  cod_plaza
		{
			 get { return _cod_plaza; }
			 set {_cod_plaza = value;}
		}

		[DataMember]
		public double  cod_banco
		{
			 get { return _cod_banco; }
			 set {_cod_banco = value;}
		}

		[DataMember]
		public int  id_persona
		{
			 get { return _id_persona; }
			 set {_id_persona = value;}
		}

		[DataMember]
		public double  cod_tipo_cta_bco
		{
			 get { return _cod_tipo_cta_bco; }
			 set {_cod_tipo_cta_bco = value;}
		}

		[DataMember]
		public double  cod_moneda
		{
			 get { return _cod_moneda; }
			 set {_cod_moneda = value;}
		}

		[DataMember]
		public string  txt_nro_cta
		{
			 get { return _txt_nro_cta; }
			 set {_txt_nro_cta = value;}
		}

		[DataMember]
		public string  txt_nombre_titular
		{
			 get { return _txt_nombre_titular; }
			 set {_txt_nombre_titular = value;}
		}

		[DataMember]
		public int  sn_activa_cuenta
		{
			 get { return _sn_activa_cuenta; }
			 set {_sn_activa_cuenta = value;}
		}

		[DataMember]
		public int  sn_default_cuenta
		{
			 get { return _sn_default_cuenta; }
			 set {_sn_default_cuenta = value;}
		}

		[DataMember]
		public string  txt_nombre_email
		{
			 get { return _txt_nombre_email; }
			 set {_txt_nombre_email = value;}
		}

		[DataMember]
		public string  txt_dominio_email
		{
			 get { return _txt_dominio_email; }
			 set {_txt_dominio_email = value;}
		}

		[DataMember]
		public string  txt_direccion_email
		{
			 get { return _txt_direccion_email; }
			 set {_txt_direccion_email = value;}
		}

		[DataMember]
		public string  cod_usuario
		{
			 get { return _cod_usuario; }
			 set {_cod_usuario = value;}
		}

		[DataMember]
		public string  fec_registro
		{
			 get { return _fec_registro; }
			 set {_fec_registro = value;}
		}


		[DataMember]
		public int  Identity
		{
		  get { return _identity; }
		  set	{ _identity = value;}
		}

		[DataMember]
		public char  State
		{
		  get { return _state; }
		  set	{ _state = value;}
		}

		[DataMember]
		public string  Connection
		{
		  get { return _connection; }
		  set	{ _connection = value;}
		}

        [DataMember]
        public string red
        {
            get { return _red; }
            set { _red = value; }
        }

        [DataMember]
        public string banco
        {
            get { return _banco; }
            set { _banco = value; }
        }

        [DataMember]
        public string txt_plaza
        {
            get { return _txt_plaza; }
            set { _txt_plaza = value; }
        }

        [DataMember]
        public string tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }

        [DataMember]
        public string moneda
        {
            get { return _moneda; }
            set { _moneda = value; }
        }

		#endregion

	}
}
