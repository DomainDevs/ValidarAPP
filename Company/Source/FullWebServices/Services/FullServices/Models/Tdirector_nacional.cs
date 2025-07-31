using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tdirector_nacional
	{

		#region InnerClass
		public enum Tdirector_nacionalFields
		{
			cod_director_nacional,
			id_persona,
			id_correla_estado,
			sn_activo,
			fec_activ,
			cod_usuario,
			user_sise
		}
		#endregion

		#region Data Members

			int _cod_director_nacional;
			int _id_persona;
			double _id_correla_estado;
			string _sn_activo;
			string _fec_activ;
			string _cod_usuario;
			string _user_sise;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  cod_director_nacional
		{
			 get { return _cod_director_nacional; }
			 set {_cod_director_nacional = value;}
		}

		[DataMember]
		public int  id_persona
		{
			 get { return _id_persona; }
			 set {_id_persona = value;}
		}

		[DataMember]
		public double  id_correla_estado
		{
			 get { return _id_correla_estado; }
			 set {_id_correla_estado = value;}
		}

		[DataMember]
		public string  sn_activo
		{
			 get { return _sn_activo; }
			 set {_sn_activo = value;}
		}

		[DataMember]
		public string  fec_activ
		{
			 get { return _fec_activ; }
			 set {_fec_activ = value;}
		}

		[DataMember]
		public string  cod_usuario
		{
			 get { return _cod_usuario; }
			 set {_cod_usuario = value;}
		}

		[DataMember]
		public string  user_sise
		{
			 get { return _user_sise; }
			 set {_user_sise = value;}
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
