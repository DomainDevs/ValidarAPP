using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tcondicion_impuesto
	{

		#region InnerClass
		public enum Tcondicion_impuestoFields
		{
			cod_impuesto,
			cod_condicion,
			txt_desc
		}
		#endregion

		#region Data Members

			double _cod_impuesto;
			double _cod_condicion;
			string _txt_desc;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_impuesto
		{
			 get { return _cod_impuesto; }
			 set {_cod_impuesto = value;}
		}

		[DataMember]
		public double  cod_condicion
		{
			 get { return _cod_condicion; }
			 set {_cod_condicion = value;}
		}

		[DataMember]
		public string  txt_desc
		{
			 get { return _txt_desc; }
			 set {_txt_desc = value;}
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
