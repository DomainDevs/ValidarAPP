using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tusuario_limites
	{

		#region InnerClass
		public enum Tusuario_limitesFields
		{
			cod_usuario,
			cod_ramo,
			cod_moneda,
			imp_limite_me
		}
		#endregion

		#region Data Members

			string _cod_usuario;
			double _cod_ramo;
			double _cod_moneda;
			double _imp_limite_me;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public string  cod_usuario
		{
			 get { return _cod_usuario; }
			 set {_cod_usuario = value;}
		}

		[DataMember]
		public double  cod_ramo
		{
			 get { return _cod_ramo; }
			 set {_cod_ramo = value;}
		}

		[DataMember]
		public double  cod_moneda
		{
			 get { return _cod_moneda; }
			 set {_cod_moneda = value;}
		}

		[DataMember]
		public double  imp_limite_me
		{
			 get { return _imp_limite_me; }
			 set {_imp_limite_me = value;}
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
