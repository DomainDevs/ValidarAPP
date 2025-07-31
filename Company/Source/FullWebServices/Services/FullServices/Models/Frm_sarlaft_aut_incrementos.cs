using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Frm_sarlaft_aut_incrementos
	{

		#region InnerClass
		public enum Frm_sarlaft_aut_incrementosFields
		{
			id_formulario,
			id_persona,
			aaaa_formulario,
			nro_formulario,
			cod_suc,
			fec_solicita,
			cod_tipo_persona,
			fec_vig_desde,
			fec_vig_hasta,
			fec_autoriza,
			imp_anterior,
			imp_actual,
			sn_autoriza,
			cod_usuario_solic,
			cod_usuario_autoriza,
			cod_motivo_autoriza,
			sn_procesado
		}
		#endregion

		#region Data Members

			int _id_formulario;
			int _id_persona;
			int _aaaa_formulario;
			double _nro_formulario;
			double _cod_suc;
			string _fec_solicita;
			string _cod_tipo_persona;
			string _fec_vig_desde;
			string _fec_vig_hasta;
			string _fec_autoriza;
			string _imp_anterior;
			string _imp_actual;
			int _sn_autoriza;
			string _cod_usuario_solic;
			string _cod_usuario_autoriza;
			string _cod_motivo_autoriza;
			string _sn_procesado;
			int _identity; 
			char _state; 
			string _connection;
            char _state_3G;

		#endregion

		#region Properties

		[DataMember]
		public int  id_formulario
		{
			 get { return _id_formulario; }
			 set {_id_formulario = value;}
		}

		[DataMember]
		public int  id_persona
		{
			 get { return _id_persona; }
			 set {_id_persona = value;}
		}

		[DataMember]
		public int  aaaa_formulario
		{
			 get { return _aaaa_formulario; }
			 set {_aaaa_formulario = value;}
		}

		[DataMember]
		public double  nro_formulario
		{
			 get { return _nro_formulario; }
			 set {_nro_formulario = value;}
		}

		[DataMember]
		public double  cod_suc
		{
			 get { return _cod_suc; }
			 set {_cod_suc = value;}
		}

		[DataMember]
		public string  fec_solicita
		{
			 get { return _fec_solicita; }
			 set {_fec_solicita = value;}
		}

		[DataMember]
		public string  cod_tipo_persona
		{
			 get { return _cod_tipo_persona; }
			 set {_cod_tipo_persona = value;}
		}

		[DataMember]
		public string  fec_vig_desde
		{
			 get { return _fec_vig_desde; }
			 set {_fec_vig_desde = value;}
		}

		[DataMember]
		public string  fec_vig_hasta
		{
			 get { return _fec_vig_hasta; }
			 set {_fec_vig_hasta = value;}
		}

		[DataMember]
		public string  fec_autoriza
		{
			 get { return _fec_autoriza; }
			 set {_fec_autoriza = value;}
		}

		[DataMember]
		public string  imp_anterior
		{
			 get { return _imp_anterior; }
			 set {_imp_anterior = value;}
		}

		[DataMember]
		public string  imp_actual
		{
			 get { return _imp_actual; }
			 set {_imp_actual = value;}
		}

		[DataMember]
		public int  sn_autoriza
		{
			 get { return _sn_autoriza; }
			 set {_sn_autoriza = value;}
		}

		[DataMember]
		public string  cod_usuario_solic
		{
			 get { return _cod_usuario_solic; }
			 set {_cod_usuario_solic = value;}
		}

		[DataMember]
		public string  cod_usuario_autoriza
		{
			 get { return _cod_usuario_autoriza; }
			 set {_cod_usuario_autoriza = value;}
		}

		[DataMember]
		public string  cod_motivo_autoriza
		{
			 get { return _cod_motivo_autoriza; }
			 set {_cod_motivo_autoriza = value;}
		}

		[DataMember]
		public string  sn_procesado
		{
			 get { return _sn_procesado; }
			 set {_sn_procesado = value;}
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
        public char State_3G
        {
            get { return _state_3G; }
            set { _state_3G = value; }
        }
		#endregion

	}
}
