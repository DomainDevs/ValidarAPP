using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Maseg_ficha_tec_junta
	{

		#region InnerClass
		public enum Maseg_ficha_tec_juntaFields
		{
			cod_aseg,
			consec_miembro,
			txt_miembro,
			txt_cargo,
			fec_modif,
			cod_usuario_modif
		}
		#endregion

		#region Data Members

			int _cod_aseg;
			double _consec_miembro;
			string _txt_miembro;
			string _txt_cargo;
			string _fec_modif;
			string _cod_usuario_modif;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  cod_aseg
		{
			 get { return _cod_aseg; }
			 set {_cod_aseg = value;}
		}

		[DataMember]
		public double  consec_miembro
		{
			 get { return _consec_miembro; }
			 set {_consec_miembro = value;}
		}

		[DataMember]
		public string  txt_miembro
		{
			 get { return _txt_miembro; }
			 set {_txt_miembro = value;}
		}

		[DataMember]
		public string  txt_cargo
		{
			 get { return _txt_cargo; }
			 set {_txt_cargo = value;}
		}

		[DataMember]
		public string  fec_modif
		{
			 get { return _fec_modif; }
			 set {_fec_modif = value;}
		}

		[DataMember]
		public string  cod_usuario_modif
		{
			 get { return _cod_usuario_modif; }
			 set {_cod_usuario_modif = value;}
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
