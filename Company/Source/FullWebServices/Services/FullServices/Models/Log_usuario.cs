using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Log_usuario
	{

		#region InnerClass
		public enum Log_usuarioFields
		{
			cod_usuario,
			txt_apellido1_ant,
			txt_apellido1_nue,
			txt_apellido2_ant,
			txt_apellido2_nue,
			txt_nombre_ant,
			txt_nombre_nue,
			cod_tipo_doc_ant,
			cod_tipo_doc_nue,
			nro_doc_ant,
			nro_doc_nue,
			fec_nac_ant,
			fec_nac_nue,
			cod_ciiu_ant,
			cod_ciiu_nue,
			cod_usuario_mod,
			fec_modificacion,
			fec_vto_password_ant,
			fec_vto_password_act,
			cod_sector_ant,
			cod_sector_act,
			sn_activo_ant,
			sn_activo_act,
			usuario_perfil_ant,
			usuario_perfil_act,
			nro_tiquete,
			menu
		}
		#endregion

		#region Data Members

			string _cod_usuario;
			string _txt_apellido1_ant;
			string _txt_apellido1_nue;
			string _txt_apellido2_ant;
			string _txt_apellido2_nue;
			string _txt_nombre_ant;
			string _txt_nombre_nue;
			double _cod_tipo_doc_ant;
			double _cod_tipo_doc_nue;
			string _nro_doc_ant;
			string _nro_doc_nue;
			string _fec_nac_ant;
			string _fec_nac_nue;
			double _cod_ciiu_ant;
			double _cod_ciiu_nue;
			string _cod_usuario_mod;
			string _fec_modificacion;
			string _fec_vto_password_ant;
			string _fec_vto_password_act;
			double _cod_sector_ant;
			double _cod_sector_act;
			int _sn_activo_ant;
			int _sn_activo_act;
			string _usuario_perfil_ant;
			string _usuario_perfil_act;
			string _nro_tiquete;
			string _menu;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public string  cod_usuario
		{
			 get { return _cod_usuario; }
			 set {_cod_usuario = value;}
		}

		[DataMember]
		public string  txt_apellido1_ant
		{
			 get { return _txt_apellido1_ant; }
			 set {_txt_apellido1_ant = value;}
		}

		[DataMember]
		public string  txt_apellido1_nue
		{
			 get { return _txt_apellido1_nue; }
			 set {_txt_apellido1_nue = value;}
		}

		[DataMember]
		public string  txt_apellido2_ant
		{
			 get { return _txt_apellido2_ant; }
			 set {_txt_apellido2_ant = value;}
		}

		[DataMember]
		public string  txt_apellido2_nue
		{
			 get { return _txt_apellido2_nue; }
			 set {_txt_apellido2_nue = value;}
		}

		[DataMember]
		public string  txt_nombre_ant
		{
			 get { return _txt_nombre_ant; }
			 set {_txt_nombre_ant = value;}
		}

		[DataMember]
		public string  txt_nombre_nue
		{
			 get { return _txt_nombre_nue; }
			 set {_txt_nombre_nue = value;}
		}

		[DataMember]
		public double  cod_tipo_doc_ant
		{
			 get { return _cod_tipo_doc_ant; }
			 set {_cod_tipo_doc_ant = value;}
		}

		[DataMember]
		public double  cod_tipo_doc_nue
		{
			 get { return _cod_tipo_doc_nue; }
			 set {_cod_tipo_doc_nue = value;}
		}

		[DataMember]
		public string  nro_doc_ant
		{
			 get { return _nro_doc_ant; }
			 set {_nro_doc_ant = value;}
		}

		[DataMember]
		public string  nro_doc_nue
		{
			 get { return _nro_doc_nue; }
			 set {_nro_doc_nue = value;}
		}

		[DataMember]
		public string  fec_nac_ant
		{
			 get { return _fec_nac_ant; }
			 set {_fec_nac_ant = value;}
		}

		[DataMember]
		public string  fec_nac_nue
		{
			 get { return _fec_nac_nue; }
			 set {_fec_nac_nue = value;}
		}

		[DataMember]
		public double  cod_ciiu_ant
		{
			 get { return _cod_ciiu_ant; }
			 set {_cod_ciiu_ant = value;}
		}

		[DataMember]
		public double  cod_ciiu_nue
		{
			 get { return _cod_ciiu_nue; }
			 set {_cod_ciiu_nue = value;}
		}

		[DataMember]
		public string  cod_usuario_mod
		{
			 get { return _cod_usuario_mod; }
			 set {_cod_usuario_mod = value;}
		}

		[DataMember]
		public string  fec_modificacion
		{
			 get { return _fec_modificacion; }
			 set {_fec_modificacion = value;}
		}

		[DataMember]
		public string  fec_vto_password_ant
		{
			 get { return _fec_vto_password_ant; }
			 set {_fec_vto_password_ant = value;}
		}

		[DataMember]
		public string  fec_vto_password_act
		{
			 get { return _fec_vto_password_act; }
			 set {_fec_vto_password_act = value;}
		}

		[DataMember]
		public double  cod_sector_ant
		{
			 get { return _cod_sector_ant; }
			 set {_cod_sector_ant = value;}
		}

		[DataMember]
		public double  cod_sector_act
		{
			 get { return _cod_sector_act; }
			 set {_cod_sector_act = value;}
		}

		[DataMember]
		public int  sn_activo_ant
		{
			 get { return _sn_activo_ant; }
			 set {_sn_activo_ant = value;}
		}

		[DataMember]
		public int  sn_activo_act
		{
			 get { return _sn_activo_act; }
			 set {_sn_activo_act = value;}
		}

		[DataMember]
		public string  usuario_perfil_ant
		{
			 get { return _usuario_perfil_ant; }
			 set {_usuario_perfil_ant = value;}
		}

		[DataMember]
		public string  usuario_perfil_act
		{
			 get { return _usuario_perfil_act; }
			 set {_usuario_perfil_act = value;}
		}

		[DataMember]
		public string  nro_tiquete
		{
			 get { return _nro_tiquete; }
			 set {_nro_tiquete = value;}
		}

		[DataMember]
		public string  menu
		{
			 get { return _menu; }
			 set {_menu = value;}
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
