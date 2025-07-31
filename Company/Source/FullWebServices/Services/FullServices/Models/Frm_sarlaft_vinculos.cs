using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Frm_sarlaft_vinculos
	{

		#region InnerClass
		public enum Frm_sarlaft_vinculosFields
		{
			id_persona,
			tomador_asegurado,
			txt_desc_TA,
			tomador_benef,
			txt_desc_TB,
			asegurado_benef,
			txt_desc_AB
		}
		#endregion

		#region Data Members

			int _id_persona;
			double _tomador_asegurado;
			string _txt_desc_ta;
			double _tomador_benef;
			string _txt_desc_tb;
			double _asegurado_benef;
			string _txt_desc_ab;
			int _identity; 
			char _state; 
			string _connection;
            char _state_3G;

		#endregion

		#region Properties

		[DataMember]
		public int  id_persona
		{
			 get { return _id_persona; }
			 set {_id_persona = value;}
		}

		[DataMember]
		public double  tomador_asegurado
		{
			 get { return _tomador_asegurado; }
			 set {_tomador_asegurado = value;}
		}

		[DataMember]
		public string  txt_desc_TA
		{
			 get { return _txt_desc_ta; }
			 set {_txt_desc_ta = value;}
		}

		[DataMember]
		public double  tomador_benef
		{
			 get { return _tomador_benef; }
			 set {_tomador_benef = value;}
		}

		[DataMember]
		public string  txt_desc_TB
		{
			 get { return _txt_desc_tb; }
			 set {_txt_desc_tb = value;}
		}

		[DataMember]
		public double  asegurado_benef
		{
			 get { return _asegurado_benef; }
			 set {_asegurado_benef = value;}
		}

		[DataMember]
		public string  txt_desc_AB
		{
			 get { return _txt_desc_ab; }
			 set {_txt_desc_ab = value;}
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

        [DataMember]
        public char State_3G
        {
            get { return _state_3G; }
            set { _state_3G = value; }
        }
		#endregion

	}
}
