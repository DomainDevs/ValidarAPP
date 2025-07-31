using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Mabogado
	{

		#region InnerClass
		public enum MabogadoFields
		{
			cod_abogado,
			id_persona,
			cod_vinc_abogado,
			sn_habilitado,
			fec_inhabilitacion,
			txt_motivo_inhab,
			fec_alta,
			fec_baja,
			nro_tarj_profesional,
			cod_pto_vta,
			txt_observaciones,
			fec_ult_modif,
			cod_usuario
		}
		#endregion

		#region Data Members

			int _cod_abogado;
			int _id_persona;
			int _cod_vinc_abogado;
			string _sn_habilitado;
			string _fec_inhabilitacion;
			string _txt_motivo_inhab;
			string _fec_alta;
			string _fec_baja;
			string _nro_tarj_profesional;
			double _cod_pto_vta;
			string _txt_observaciones;
			string _fec_ult_modif;
			string _cod_usuario;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  cod_abogado
		{
			 get { return _cod_abogado; }
			 set {_cod_abogado = value;}
		}

		[DataMember]
		public int  id_persona
		{
			 get { return _id_persona; }
			 set {_id_persona = value;}
		}

		[DataMember]
		public int  cod_vinc_abogado
		{
			 get { return _cod_vinc_abogado; }
			 set {_cod_vinc_abogado = value;}
		}

		[DataMember]
		public string  sn_habilitado
		{
			 get { return _sn_habilitado; }
			 set {_sn_habilitado = value;}
		}

		[DataMember]
		public string  fec_inhabilitacion
		{
			 get { return _fec_inhabilitacion; }
			 set {_fec_inhabilitacion = value;}
		}

		[DataMember]
		public string  txt_motivo_inhab
		{
			 get { return _txt_motivo_inhab; }
			 set {_txt_motivo_inhab = value;}
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
		public string  nro_tarj_profesional
		{
			 get { return _nro_tarj_profesional; }
			 set {_nro_tarj_profesional = value;}
		}

		[DataMember]
		public double  cod_pto_vta
		{
			 get { return _cod_pto_vta; }
			 set {_cod_pto_vta = value;}
		}

		[DataMember]
		public string  txt_observaciones
		{
			 get { return _txt_observaciones; }
			 set {_txt_observaciones = value;}
		}

		[DataMember]
		public string  fec_ult_modif
		{
			 get { return _fec_ult_modif; }
			 set {_fec_ult_modif = value;}
		}

		[DataMember]
		public string  cod_usuario
		{
			 get { return _cod_usuario; }
			 set {_cod_usuario = value;}
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
