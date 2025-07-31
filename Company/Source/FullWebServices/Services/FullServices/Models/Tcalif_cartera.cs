using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tcalif_cartera
	{

		#region InnerClass
		public enum Tcalif_carteraFields
		{
			cod_calif_cart,
			txt_desc,
			sn_maseg,
			sn_magente
		}
		#endregion

		#region Data Members

			string _cod_calif_cart;
			string _txt_desc;
			string _sn_maseg;
			string _sn_magente;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public string  cod_calif_cart
		{
			 get { return _cod_calif_cart; }
			 set {_cod_calif_cart = value;}
		}

		[DataMember]
		public string  txt_desc
		{
			 get { return _txt_desc; }
			 set {_txt_desc = value;}
		}

		[DataMember]
		public string  sn_maseg
		{
			 get { return _sn_maseg; }
			 set {_sn_maseg = value;}
		}

		[DataMember]
		public string  sn_magente
		{
			 get { return _sn_magente; }
			 set {_sn_magente = value;}
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
