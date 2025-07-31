using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Texperiencia_aseg
	{

		#region InnerClass
		public enum Texperiencia_asegFields
		{
			cod_experiencia,
			txt_desc_experiencia
		}
		#endregion

		#region Data Members

			double _cod_experiencia;
			string _txt_desc_experiencia;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_experiencia
		{
			 get { return _cod_experiencia; }
			 set {_cod_experiencia = value;}
		}

		[DataMember]
		public string  txt_desc_experiencia
		{
			 get { return _txt_desc_experiencia; }
			 set {_txt_desc_experiencia = value;}
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
