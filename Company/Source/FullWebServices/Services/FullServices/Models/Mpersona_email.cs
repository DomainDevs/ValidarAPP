using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Mpersona_email
	{

		#region InnerClass
		public enum Mpersona_emailFields
		{
			id_persona,
			cod_tipo_email,
			txt_nombre_email,
			txt_dominio_email,
			txt_direccion_email
		}
		#endregion

		#region Data Members

			int _id_persona;
			int _cod_tipo_email;
			string _txt_nombre_email;
			string _txt_dominio_email;
			string _txt_direccion_email;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  id_persona
		{
			 get { return _id_persona; }
			 set {_id_persona = value;}
		}

		[DataMember]
		public int  cod_tipo_email
		{
			 get { return _cod_tipo_email; }
			 set {_cod_tipo_email = value;}
		}

		[DataMember]
		public string  txt_nombre_email
		{
			 get { return _txt_nombre_email; }
			 set {_txt_nombre_email = value;}
		}

		[DataMember]
		public string  txt_dominio_email
		{
			 get { return _txt_dominio_email; }
			 set {_txt_dominio_email = value;}
		}

		[DataMember]
		public string  txt_direccion_email
		{
			 get { return _txt_direccion_email; }
			 set {_txt_direccion_email = value;}
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
        public bool IS_MAILING_ADDRESS
        {
            get;
            set;
        }

        [DataMember]
        public int DATA_ID
        {
            get;
            set;
        }

        [DataMember]
        public char State_3g
        {
            get;
            set;
        }

		#endregion

	}
}
