using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tusuario_concepto
	{

		#region InnerClass
		public enum Tusuario_conceptoFields
		{
			cod_usuario,
			cod_concepto,
			cod_suc
		}
		#endregion

		#region Data Members

			string _cod_usuario;
			double _cod_concepto;
			double _cod_suc;
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
		public double  cod_concepto
		{
			 get { return _cod_concepto; }
			 set {_cod_concepto = value;}
		}

		[DataMember]
		public double  cod_suc
		{
			 get { return _cod_suc; }
			 set {_cod_suc = value;}
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
