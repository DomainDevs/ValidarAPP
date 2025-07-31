using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Mpersona_requiere_sarlaft_hist
	{

		#region InnerClass
		public enum Mpersona_requiere_sarlaft_histFields
		{
			id_persona,
			cod_usuario,
			sn_exonera,
			cod_motivo_exonera,
			fec_modifica,
			txt_origen
		}
		#endregion

		#region Data Members

			int _id_persona;
			string _cod_usuario;
			int _sn_exonera;
			string _cod_motivo_exonera;
			string _fec_modifica;
			string _txt_origen;
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
		public string  cod_usuario
		{
			 get { return _cod_usuario; }
			 set {_cod_usuario = value;}
		}

		[DataMember]
		public int  sn_exonera
		{
			 get { return _sn_exonera; }
			 set {_sn_exonera = value;}
		}

		[DataMember]
		public string cod_motivo_exonera
		{
			 get { return _cod_motivo_exonera; }
			 set {_cod_motivo_exonera = value;}
		}

		[DataMember]
		public string  fec_modifica
		{
			 get { return _fec_modifica; }
			 set {_fec_modifica = value;}
		}

		[DataMember]
		public string  txt_origen
		{
			 get { return _txt_origen; }
			 set {_txt_origen = value;}
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
