using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Ttipo_asociacion
	{

		#region InnerClass
		public enum Ttipo_asociacionFields
		{
			cod_tasociacion,
			txt_desc_tasociacion,
			sn_individual,
			sn_consorcio,
			sn_union
		}
		#endregion

		#region Data Members

			double _cod_tasociacion;
			string _txt_desc_tasociacion;
			int _sn_individual;
			int _sn_consorcio;
			int _sn_union;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_tasociacion
		{
			 get { return _cod_tasociacion; }
			 set {_cod_tasociacion = value;}
		}

		[DataMember]
		public string  txt_desc_tasociacion
		{
			 get { return _txt_desc_tasociacion; }
			 set {_txt_desc_tasociacion = value;}
		}

		[DataMember]
		public int  sn_individual
		{
			 get { return _sn_individual; }
			 set {_sn_individual = value;}
		}

		[DataMember]
		public int  sn_consorcio
		{
			 get { return _sn_consorcio; }
			 set {_sn_consorcio = value;}
		}

		[DataMember]
		public int  sn_union
		{
			 get { return _sn_union; }
			 set {_sn_union = value;}
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
