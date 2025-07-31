using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tciiu
	{

		#region InnerClass
		public enum TciiuFields
		{
			cod_ciiu,
			txt_descr,
			sn_titulo,
			sn_natural,
			sn_juridica
		}
		#endregion

		#region Data Members

			double _cod_ciiu;
			string _txt_descr;
			string _sn_titulo;
			int _sn_natural;
			int _sn_juridica;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_ciiu
		{
			 get { return _cod_ciiu; }
			 set {_cod_ciiu = value;}
		}

		[DataMember]
		public string  txt_descr
		{
			 get { return _txt_descr; }
			 set {_txt_descr = value;}
		}

		[DataMember]
		public string  sn_titulo
		{
			 get { return _sn_titulo; }
			 set {_sn_titulo = value;}
		}

		[DataMember]
		public int  sn_natural
		{
			 get { return _sn_natural; }
			 set {_sn_natural = value;}
		}

		[DataMember]
		public int  sn_juridica
		{
			 get { return _sn_juridica; }
			 set {_sn_juridica = value;}
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
