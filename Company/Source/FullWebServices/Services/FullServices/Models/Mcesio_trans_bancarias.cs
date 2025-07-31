using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Mcesio_trans_bancarias
	{

		#region InnerClass
		public enum Mcesio_trans_bancariasFields
		{
			cod_cesionario,
			cod_abona,
			id_persona,
			sn_activo
		}
		#endregion

		#region Data Members

			int _cod_cesionario;
			double _cod_abona;
			int _id_persona;
			string _sn_activo;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  cod_cesionario
		{
			 get { return _cod_cesionario; }
			 set {_cod_cesionario = value;}
		}

		[DataMember]
		public double  cod_abona
		{
			 get { return _cod_abona; }
			 set {_cod_abona = value;}
		}

		[DataMember]
		public int  id_persona
		{
			 get { return _id_persona; }
			 set {_id_persona = value;}
		}

		[DataMember]
		public string  sn_activo
		{
			 get { return _sn_activo; }
			 set {_sn_activo = value;}
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
