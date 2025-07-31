using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Mpres
	{

		#region InnerClass
		public enum MpresFields
		{
			cod_pres,
			id_persona,
			cod_ciiu,
			cod_tipo_pres,
			sn_convenio,
			cod_especialidad,
			nro_porc,
			fec_alta,
			fec_baja,
			txt_cheque_a_nom,
			cod_dependencia,
			cod_cpto_default,
			fec_habilitacion,
			cod_suc_operacion,
			sn_intersuc,
			cod_baja,
			sn_afp,
			sn_eps,
			codigo_afp,
			codigo_eps,
			sn_nacional,
			sn_ips,
			codigo_ipss,
			codigo_minsalud
		}
		#endregion

		#region Data Members

			double _cod_pres;
			int _id_persona;
			double _cod_ciiu;
			double _cod_tipo_pres;
			string _sn_convenio;
			double _cod_especialidad;
			string _nro_porc;
			string _fec_alta;
			string _fec_baja;
			string _txt_cheque_a_nom;
			string _cod_dependencia;
			string _cod_cpto_default;
			string _fec_habilitacion;
			string _cod_suc_operacion;
			string _sn_intersuc;
			string _cod_baja;
			string _sn_afp;
			string _sn_eps;
			string _codigo_afp;
			string _codigo_eps;
			string _sn_nacional;
			string _sn_ips;
			string _codigo_ipss;
			string _codigo_minsalud;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_pres
		{
			 get { return _cod_pres; }
			 set {_cod_pres = value;}
		}

		[DataMember]
		public int  id_persona
		{
			 get { return _id_persona; }
			 set {_id_persona = value;}
		}

		[DataMember]
		public double  cod_ciiu
		{
			 get { return _cod_ciiu; }
			 set {_cod_ciiu = value;}
		}

		[DataMember]
		public double  cod_tipo_pres
		{
			 get { return _cod_tipo_pres; }
			 set {_cod_tipo_pres = value;}
		}

		[DataMember]
		public string  sn_convenio
		{
			 get { return _sn_convenio; }
			 set {_sn_convenio = value;}
		}

		[DataMember]
		public double  cod_especialidad
		{
			 get { return _cod_especialidad; }
			 set {_cod_especialidad = value;}
		}

		[DataMember]
		public string  nro_porc
		{
			 get { return _nro_porc; }
			 set {_nro_porc = value;}
		}

		[DataMember]
		public string  fec_alta
		{
			 get { return _fec_alta; }
			 set {_fec_alta = value;}
		}

		[DataMember]
		public string  fec_baja
		{
			 get { return _fec_baja; }
			 set {_fec_baja = value;}
		}

		[DataMember]
		public string  txt_cheque_a_nom
		{
			 get { return _txt_cheque_a_nom; }
			 set {_txt_cheque_a_nom = value;}
		}

		[DataMember]
		public string  cod_dependencia
		{
			 get { return _cod_dependencia; }
			 set {_cod_dependencia = value;}
		}

		[DataMember]
		public string  cod_cpto_default
		{
			 get { return _cod_cpto_default; }
			 set {_cod_cpto_default = value;}
		}

		[DataMember]
		public string  fec_habilitacion
		{
			 get { return _fec_habilitacion; }
			 set {_fec_habilitacion = value;}
		}

		[DataMember]
		public string  cod_suc_operacion
		{
			 get { return _cod_suc_operacion; }
			 set {_cod_suc_operacion = value;}
		}

		[DataMember]
		public string  sn_intersuc
		{
			 get { return _sn_intersuc; }
			 set {_sn_intersuc = value;}
		}

		[DataMember]
		public string  cod_baja
		{
			 get { return _cod_baja; }
			 set {_cod_baja = value;}
		}

		[DataMember]
		public string  sn_afp
		{
			 get { return _sn_afp; }
			 set {_sn_afp = value;}
		}

		[DataMember]
		public string  sn_eps
		{
			 get { return _sn_eps; }
			 set {_sn_eps = value;}
		}

		[DataMember]
		public string  codigo_afp
		{
			 get { return _codigo_afp; }
			 set {_codigo_afp = value;}
		}

		[DataMember]
		public string  codigo_eps
		{
			 get { return _codigo_eps; }
			 set {_codigo_eps = value;}
		}

		[DataMember]
		public string  sn_nacional
		{
			 get { return _sn_nacional; }
			 set {_sn_nacional = value;}
		}

		[DataMember]
		public string  sn_ips
		{
			 get { return _sn_ips; }
			 set {_sn_ips = value;}
		}

		[DataMember]
		public string  codigo_ipss
		{
			 get { return _codigo_ipss; }
			 set {_codigo_ipss = value;}
		}

		[DataMember]
		public string  codigo_minsalud
		{
			 get { return _codigo_minsalud; }
			 set {_codigo_minsalud = value;}
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

		#endregion

	}
}
