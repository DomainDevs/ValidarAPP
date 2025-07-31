using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tvarias_ult_nro
	{

		#region InnerClass
		public enum Tvarias_ult_nroFields
		{
			txt_nom_tabla,
			nro_ultimo
		}
		#endregion

		#region Data Members

			string _txt_nom_tabla;
			int _nro_ultimo;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public string  txt_nom_tabla
		{
			 get { return _txt_nom_tabla; }
			 set {_txt_nom_tabla = value;}
		}

		[DataMember]
		public int  nro_ultimo
		{
			 get { return _nro_ultimo; }
			 set {_nro_ultimo = value;}
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
