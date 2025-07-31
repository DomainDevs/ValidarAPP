using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Mempleado
	{

		#region InnerClass
		public enum MempleadoFields
		{
			cod_empleado,
			id_persona,
			sn_RLegal,
			cod_suc
		}
		#endregion

		#region Data Members

			double _cod_empleado;
			int _id_persona;
			int? _sn_RLegal;
			double _cod_suc;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_empleado
		{
			 get { return _cod_empleado; }
			 set {_cod_empleado = value;}
		}

		[DataMember]
		public int  id_persona
		{
			 get { return _id_persona; }
			 set {_id_persona = value;}
		}

		[DataMember]
		public int?  sn_RLegal
		{
			 get { return _sn_RLegal; }
			 set {_sn_RLegal = value;}
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
