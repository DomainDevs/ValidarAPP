using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Mpersona_requiere_sarlaft
	{

		#region InnerClass
		public enum Mpersona_requiere_sarlaftFields
		{
			id_persona,
			sn_exonerado,
			cod_motivo_exonera,
			txt_origen,
			cod_suc,
			fec_modifica
		}
		#endregion

		#region Data Members

			int _id_persona;
			int _sn_exonerado;
			string _cod_motivo_exonera;
			string _txt_origen;
			string _cod_suc;
			string _fec_modifica;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  id_persona
		{
			 get { return _id_persona; }
			 set {_id_persona = value;}
		}

		[DataMember]
		public int  sn_exonerado
		{
			 get { return _sn_exonerado; }
			 set {_sn_exonerado = value;}
		}

		[DataMember]
		public string  cod_motivo_exonera
		{
			 get { return _cod_motivo_exonera; }
			 set {_cod_motivo_exonera = value;}
		}

		[DataMember]
		public string  txt_origen
		{
			 get { return _txt_origen; }
			 set {_txt_origen = value;}
		}

		[DataMember]
		public string  cod_suc
		{
			 get { return _cod_suc; }
			 set {_cod_suc = value;}
		}

		[DataMember]
		public string  fec_modifica
		{
			 get { return _fec_modifica; }
			 set {_fec_modifica = value;}
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
