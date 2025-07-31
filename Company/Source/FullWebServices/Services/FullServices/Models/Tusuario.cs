using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tusuario
	{

		#region InnerClass
		public enum TusuarioFields
		{
			cod_usuario,
			cod_grupo_usuario,
			txt_nombre,
			cod_suc,
			cod_sector,
			sn_activo,
			fec_vto_password,
			cnt_dias_validez_pwd,
			txt_password,
			fec_alta,
			circuito_op,
			iniciales,
			sn_cpto_cble,
			sn_todos_cpto_cb,
			sn_cta_cble,
			sn_todos_cta_cb,
			nro_caja,
			cod_pto_vta,
			cod_ramo,
			sn_perfil,
			usuario_perfil,
			limite_op,
			sn_pedir_password,
			sn_habilitado,
			perfil_director,
			sn_activo_web,
			sn_usuario_externo,
			sn_bloqueo_x_clave,
			fec_hora_activacion,
			txt_email,
			sn_activo_3g,
			sn_perfil_delegado,
			fec_baja,
			fec_activacion,
			fec_desactivacion
		}
		#endregion

		#region Data Members

			string _cod_usuario;
			string _cod_grupo_usuario;
			string _txt_nombre;
			double _cod_suc;
			double _cod_sector;
			int _sn_activo;
			string _fec_vto_password;
			string _cnt_dias_validez_pwd;
			string _txt_password;
			string _fec_alta;
			string _circuito_op;
			string _iniciales;
			int _sn_cpto_cble;
			int _sn_todos_cpto_cb;
			int _sn_cta_cble;
			int _sn_todos_cta_cb;
			double _nro_caja;
			string _cod_pto_vta;
			string _cod_ramo;
			string _sn_perfil;
			string _usuario_perfil;
			string _limite_op;
			string _sn_pedir_password;
			string _sn_habilitado;
			int _perfil_director;
			int _sn_activo_web;
			int _sn_usuario_externo;
			int _sn_bloqueo_x_clave;
			string _fec_hora_activacion;
			string _txt_email;
			int _sn_activo_3g;
			string _sn_perfil_delegado;
			string _fec_baja;
			string _fec_activacion;
			string _fec_desactivacion;
			int _identity; 
			char _state; 
			string _connection;
            bool _restart_Password;

		#endregion

		#region Properties

		[DataMember]
		public string  cod_usuario
		{
			 get { return _cod_usuario; }
			 set {_cod_usuario = value;}
		}

		[DataMember]
		public string  cod_grupo_usuario
		{
			 get { return _cod_grupo_usuario; }
			 set {_cod_grupo_usuario = value;}
		}

		[DataMember]
		public string  txt_nombre
		{
			 get { return _txt_nombre; }
			 set {_txt_nombre = value;}
		}

		[DataMember]
		public double  cod_suc
		{
			 get { return _cod_suc; }
			 set {_cod_suc = value;}
		}

		[DataMember]
		public double  cod_sector
		{
			 get { return _cod_sector; }
			 set {_cod_sector = value;}
		}

		[DataMember]
		public int  sn_activo
		{
			 get { return _sn_activo; }
			 set {_sn_activo = value;}
		}

		[DataMember]
		public string  fec_vto_password
		{
			 get { return _fec_vto_password; }
			 set {_fec_vto_password = value;}
		}

		[DataMember]
		public string  cnt_dias_validez_pwd
		{
			 get { return _cnt_dias_validez_pwd; }
			 set {_cnt_dias_validez_pwd = value;}
		}

		[DataMember]
		public string  txt_password
		{
			 get { return _txt_password; }
			 set {_txt_password = value;}
		}

		[DataMember]
		public string  fec_alta
		{
			 get { return _fec_alta; }
			 set {_fec_alta = value;}
		}

		[DataMember]
		public string  circuito_op
		{
			 get { return _circuito_op; }
			 set {_circuito_op = value;}
		}

		[DataMember]
		public string  iniciales
		{
			 get { return _iniciales; }
			 set {_iniciales = value;}
		}

		[DataMember]
		public int  sn_cpto_cble
		{
			 get { return _sn_cpto_cble; }
			 set {_sn_cpto_cble = value;}
		}

		[DataMember]
		public int  sn_todos_cpto_cb
		{
			 get { return _sn_todos_cpto_cb; }
			 set {_sn_todos_cpto_cb = value;}
		}

		[DataMember]
		public int  sn_cta_cble
		{
			 get { return _sn_cta_cble; }
			 set {_sn_cta_cble = value;}
		}

		[DataMember]
		public int  sn_todos_cta_cb
		{
			 get { return _sn_todos_cta_cb; }
			 set {_sn_todos_cta_cb = value;}
		}

		[DataMember]
		public double  nro_caja
		{
			 get { return _nro_caja; }
			 set {_nro_caja = value;}
		}

		[DataMember]
		public string  cod_pto_vta
		{
			 get { return _cod_pto_vta; }
			 set {_cod_pto_vta = value;}
		}

		[DataMember]
		public string  cod_ramo
		{
			 get { return _cod_ramo; }
			 set {_cod_ramo = value;}
		}

		[DataMember]
		public string  sn_perfil
		{
			 get { return _sn_perfil; }
			 set {_sn_perfil = value;}
		}

		[DataMember]
		public string  usuario_perfil
		{
			 get { return _usuario_perfil; }
			 set {_usuario_perfil = value;}
		}

		[DataMember]
		public string  limite_op
		{
			 get { return _limite_op; }
			 set {_limite_op = value;}
		}

		[DataMember]
		public string  sn_pedir_password
		{
			 get { return _sn_pedir_password; }
			 set {_sn_pedir_password = value;}
		}

		[DataMember]
		public string  sn_habilitado
		{
			 get { return _sn_habilitado; }
			 set {_sn_habilitado = value;}
		}

		[DataMember]
		public int  perfil_director
		{
			 get { return _perfil_director; }
			 set {_perfil_director = value;}
		}

		[DataMember]
		public int  sn_activo_web
		{
			 get { return _sn_activo_web; }
			 set {_sn_activo_web = value;}
		}

		[DataMember]
		public int  sn_usuario_externo
		{
			 get { return _sn_usuario_externo; }
			 set {_sn_usuario_externo = value;}
		}

		[DataMember]
		public int  sn_bloqueo_x_clave
		{
			 get { return _sn_bloqueo_x_clave; }
			 set {_sn_bloqueo_x_clave = value;}
		}

		[DataMember]
		public string  fec_hora_activacion
		{
			 get { return _fec_hora_activacion; }
			 set {_fec_hora_activacion = value;}
		}

		[DataMember]
		public string  txt_email
		{
			 get { return _txt_email; }
			 set {_txt_email = value;}
		}

		[DataMember]
		public int  sn_activo_3g
		{
			 get { return _sn_activo_3g; }
			 set {_sn_activo_3g = value;}
		}

		[DataMember]
		public string  sn_perfil_delegado
		{
			 get { return _sn_perfil_delegado; }
			 set {_sn_perfil_delegado = value;}
		}

		[DataMember]
		public string  fec_baja
		{
			 get { return _fec_baja; }
			 set {_fec_baja = value;}
		}

		[DataMember]
		public string  fec_activacion
		{
			 get { return _fec_activacion; }
			 set {_fec_activacion = value;}
		}

		[DataMember]
		public string  fec_desactivacion
		{
			 get { return _fec_desactivacion; }
			 set {_fec_desactivacion = value;}
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
        public bool Restart_Password
        {
            get;
            set;
        }


		#endregion

	}
}
