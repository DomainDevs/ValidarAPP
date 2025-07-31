using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Maseg_ficha_tec_obs
	{

		#region InnerClass
		public enum Maseg_ficha_tec_obsFields
		{
			cod_aseg,
			consec_obs,
			txt_obs,
			fec_creacion,
			fec_modif,
			cod_usuario_crea,
			cod_usuario_modif
		}
		#endregion

		#region Data Members

			int _cod_aseg;
			double _consec_obs;
			string _txt_obs;
			string _fec_creacion;
			string _fec_modif;
			string _cod_usuario_crea;
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
		public double  consec_obs
		{
			 get { return _consec_obs; }
			 set {_consec_obs = value;}
		}

		[DataMember]
		public string  txt_obs
		{
			 get { return _txt_obs; }
			 set {_txt_obs = value;}
		}

		[DataMember]
		public string  fec_creacion
		{
			 get { return _fec_creacion; }
			 set {_fec_creacion = value;}
		}

		[DataMember]
		public string  fec_modif
		{
			 get { return _fec_modif; }
			 set {_fec_modif = value;}
		}

		[DataMember]
		public string  cod_usuario_crea
		{
			 get { return _cod_usuario_crea; }
			 set {_cod_usuario_crea = value;}
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
