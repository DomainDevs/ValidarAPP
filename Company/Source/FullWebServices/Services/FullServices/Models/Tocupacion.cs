using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tocupacion
	{

		#region InnerClass
		public enum TocupacionFields
		{
			cod_ocupacion,
			txt_desc,
			cod_actividad_au
		}
		#endregion

		#region Data Members

			double _cod_ocupacion;
			string _txt_desc;
			string _cod_actividad_au;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_ocupacion
		{
			 get { return _cod_ocupacion; }
			 set {_cod_ocupacion = value;}
		}

		[DataMember]
		public string  txt_desc
		{
			 get { return _txt_desc; }
			 set {_txt_desc = value;}
		}

		[DataMember]
		public string  cod_actividad_au
		{
			 get { return _cod_actividad_au; }
			 set {_cod_actividad_au = value;}
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
