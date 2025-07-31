using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tredes_secuencia
	{

		#region InnerClass
		public enum Tredes_secuenciaFields
		{
			cod_red,
			nro_secuencia,
			txt_descripcion
		}
		#endregion

		#region Data Members

			double _cod_red;
			double _nro_secuencia;
			string _txt_descripcion;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_red
		{
			 get { return _cod_red; }
			 set {_cod_red = value;}
		}

		[DataMember]
		public double  nro_secuencia
		{
			 get { return _nro_secuencia; }
			 set {_nro_secuencia = value;}
		}

		[DataMember]
		public string  txt_descripcion
		{
			 get { return _txt_descripcion; }
			 set {_txt_descripcion = value;}
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
