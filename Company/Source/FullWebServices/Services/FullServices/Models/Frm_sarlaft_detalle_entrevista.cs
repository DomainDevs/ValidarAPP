using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Frm_sarlaft_detalle_entrevista
	{

		#region InnerClass
		public enum Frm_sarlaft_detalle_entrevistaFields
		{
			id_formulario,
			txt_lugar_entrev,
			txt_obser_entrev,
			txt_resul_entrev,
			fec_entrevista,
			txt_usua_entrev
		}
		#endregion

		#region Data Members

			int _id_formulario;
			string _txt_lugar_entrev;
			string _txt_obser_entrev;
			string _txt_resul_entrev;
			string _fec_entrevista;
			string _txt_usua_entrev;
			int _identity; 
			char _state;
            char _state_3G;
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  id_formulario
		{
			 get { return _id_formulario; }
			 set {_id_formulario = value;}
		}

		[DataMember]
		public string  txt_lugar_entrev
		{
			 get { return _txt_lugar_entrev; }
			 set {_txt_lugar_entrev = value;}
		}

		[DataMember]
		public string  txt_obser_entrev
		{
			 get { return _txt_obser_entrev; }
			 set {_txt_obser_entrev = value;}
		}

		[DataMember]
		public string  txt_resul_entrev
		{
			 get { return _txt_resul_entrev; }
			 set {_txt_resul_entrev = value;}
		}

		[DataMember]
		public string  fec_entrevista
		{
			 get { return _fec_entrevista; }
			 set {_fec_entrevista = value;}
		}

		[DataMember]
		public string  txt_usua_entrev
		{
			 get { return _txt_usua_entrev; }
			 set {_txt_usua_entrev = value;}
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
