using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tunidad
	{

		#region InnerClass
		public enum TunidadFields
		{
			cod_unidad,
			txt_desc_unidad,
			txt_simbolo,
			sn_salario
		}
		#endregion

		#region Data Members

			double _cod_unidad;
			string _txt_desc_unidad;
			string _txt_simbolo;
			string _sn_salario;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_unidad
		{
			 get { return _cod_unidad; }
			 set {_cod_unidad = value;}
		}

		[DataMember]
		public string  txt_desc_unidad
		{
			 get { return _txt_desc_unidad; }
			 set {_txt_desc_unidad = value;}
		}

		[DataMember]
		public string  txt_simbolo
		{
			 get { return _txt_simbolo; }
			 set {_txt_simbolo = value;}
		}

		[DataMember]
		public string  sn_salario
		{
			 get { return _sn_salario; }
			 set {_sn_salario = value;}
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
