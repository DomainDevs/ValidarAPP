using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tdirector_comercial
	{

		#region InnerClass
		public enum Tdirector_comercialFields
		{
			cod_director_comercial,
			id_persona,
			cod_director_nacional,
			cod_suc_asoc,
			fec_ult_mov,
			sn_activo,
			user_sise
		}
		#endregion

		#region Data Members

			int _cod_director_comercial;
			int _id_persona;
			int _cod_director_nacional;
			double _cod_suc_asoc;
			string _fec_ult_mov;
			string _sn_activo;
			string _user_sise;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  cod_director_comercial
		{
			 get { return _cod_director_comercial; }
			 set {_cod_director_comercial = value;}
		}

		[DataMember]
		public int  id_persona
		{
			 get { return _id_persona; }
			 set {_id_persona = value;}
		}

		[DataMember]
		public int  cod_director_nacional
		{
			 get { return _cod_director_nacional; }
			 set {_cod_director_nacional = value;}
		}

		[DataMember]
		public double  cod_suc_asoc
		{
			 get { return _cod_suc_asoc; }
			 set {_cod_suc_asoc = value;}
		}

		[DataMember]
		public string  fec_ult_mov
		{
			 get { return _fec_ult_mov; }
			 set {_fec_ult_mov = value;}
		}

		[DataMember]
		public string  sn_activo
		{
			 get { return _sn_activo; }
			 set {_sn_activo = value;}
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
