using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tactividad
	{

		#region InnerClass
		public enum TactividadFields
		{
			cod_actividad,
			cod_dpto,
			cod_munic,
			txt_desc
		}
		#endregion

		#region Data Members

			double _cod_actividad;
			double _cod_dpto;
			double _cod_munic;
			string _txt_desc;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_actividad
		{
			 get { return _cod_actividad; }
			 set {_cod_actividad = value;}
		}

		[DataMember]
		public double  cod_dpto
		{
			 get { return _cod_dpto; }
			 set {_cod_dpto = value;}
		}

		[DataMember]
		public double  cod_munic
		{
			 get { return _cod_munic; }
			 set {_cod_munic = value;}
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
