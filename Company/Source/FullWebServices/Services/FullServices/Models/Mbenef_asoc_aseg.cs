using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Mbenef_asoc_aseg
	{

		#region InnerClass
		public enum Mbenef_asoc_asegFields
		{
			cod_beneficiario,
			cod_aseg,
			cod_parentesco,
			sn_activo
		}
		#endregion

		#region Data Members

			int _cod_beneficiario;
			int _cod_aseg;
			double _cod_parentesco;
			int _sn_activo;
			int _identity; 
			char _state; 
			string _connection;
            string _cod_tipo_doc;
            string _num_doc;
            string _txt_nombre;
            string _cod_tipo_persona;

		#endregion

		#region Properties

		[DataMember]
		public int  cod_beneficiario
		{
			 get { return _cod_beneficiario; }
			 set {_cod_beneficiario = value;}
		}

		[DataMember]
		public int  cod_aseg
		{
			 get { return _cod_aseg; }
			 set {_cod_aseg = value;}
		}

		[DataMember]
		public double  cod_parentesco
		{
			 get { return _cod_parentesco; }
			 set {_cod_parentesco = value;}
		}

		[DataMember]
		public int  sn_activo
		{
			 get { return _sn_activo; }
			 set {_sn_activo = value;}
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
        public string num_doc
        {
            get { return _num_doc; }
            set { _num_doc = value; }
        }

        [DataMember]
        public string txt_nombre
        {
            get { return _txt_nombre; }
            set { _txt_nombre = value; }
        }

        [DataMember]
        public string cod_tipo_persona
        {
            get { return _cod_tipo_persona; }
            set { _cod_tipo_persona = value; }
        }

        [DataMember]
        public string cod_tipo_doc
        {
            get { return _cod_tipo_doc; }
            set { _cod_tipo_doc = value; }
        }
		#endregion

	}
}
