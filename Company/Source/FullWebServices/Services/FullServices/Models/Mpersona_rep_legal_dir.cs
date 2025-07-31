using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Mpersona_rep_legal_dir
	{

		#region InnerClass
		public enum Mpersona_rep_legal_dirFields
		{
			id_persona,
			nro_doc_rep_legal,
			cod_tipo_doc,
			cod_tipo_dir,
			cod_tipo_calle_campo1,
			nro_campo1,
			txt_desc_campo1,
			cod_tipo_calle_campo2,
			nro_campo2,
			txt_desc_campo2,
			nro_campo3,
			txt_apto_ofic,
			txt_observaciones,
			txt_direccion,
			nro_cod_postal,
			cod_pais,
			cod_dpto,
			cod_municipio
		}
		#endregion

		#region Data Members

			int _id_persona;
			string _nro_doc_rep_legal;
			double _cod_tipo_doc;
			double _cod_tipo_dir;
			double _cod_tipo_calle_campo1;
			double _nro_campo1;
			string _txt_desc_campo1;
			double _cod_tipo_calle_campo2;
			double _nro_campo2;
			string _txt_desc_campo2;
			double _nro_campo3;
			double _txt_apto_ofic;
			string _txt_observaciones;
			string _txt_direccion;
			string _nro_cod_postal;
			double _cod_pais;
			double _cod_dpto;
			double _cod_municipio;
			int _identity; 
			char _state;
            char _state_3G;
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
		public string  nro_doc_rep_legal
		{
			 get { return _nro_doc_rep_legal; }
			 set {_nro_doc_rep_legal = value;}
		}

		[DataMember]
		public double  cod_tipo_doc
		{
			 get { return _cod_tipo_doc; }
			 set {_cod_tipo_doc = value;}
		}

		[DataMember]
		public double  cod_tipo_dir
		{
			 get { return _cod_tipo_dir; }
			 set {_cod_tipo_dir = value;}
		}

		[DataMember]
		public double  cod_tipo_calle_campo1
		{
			 get { return _cod_tipo_calle_campo1; }
			 set {_cod_tipo_calle_campo1 = value;}
		}

		[DataMember]
		public double  nro_campo1
		{
			 get { return _nro_campo1; }
			 set {_nro_campo1 = value;}
		}

		[DataMember]
		public string  txt_desc_campo1
		{
			 get { return _txt_desc_campo1; }
			 set {_txt_desc_campo1 = value;}
		}

		[DataMember]
		public double  cod_tipo_calle_campo2
		{
			 get { return _cod_tipo_calle_campo2; }
			 set {_cod_tipo_calle_campo2 = value;}
		}

		[DataMember]
		public double  nro_campo2
		{
			 get { return _nro_campo2; }
			 set {_nro_campo2 = value;}
		}

		[DataMember]
		public string  txt_desc_campo2
		{
			 get { return _txt_desc_campo2; }
			 set {_txt_desc_campo2 = value;}
		}

		[DataMember]
		public double  nro_campo3
		{
			 get { return _nro_campo3; }
			 set {_nro_campo3 = value;}
		}

		[DataMember]
		public double  txt_apto_ofic
		{
			 get { return _txt_apto_ofic; }
			 set {_txt_apto_ofic = value;}
		}

		[DataMember]
		public string  txt_observaciones
		{
			 get { return _txt_observaciones; }
			 set {_txt_observaciones = value;}
		}

		[DataMember]
		public string  txt_direccion
		{
			 get { return _txt_direccion; }
			 set {_txt_direccion = value;}
		}

		[DataMember]
		public string  nro_cod_postal
		{
			 get { return _nro_cod_postal; }
			 set {_nro_cod_postal = value;}
		}

		[DataMember]
		public double  cod_pais
		{
			 get { return _cod_pais; }
			 set {_cod_pais = value;}
		}

		[DataMember]
		public double  cod_dpto
		{
			 get { return _cod_dpto; }
			 set {_cod_dpto = value;}
		}

		[DataMember]
		public double  cod_municipio
		{
			 get { return _cod_municipio; }
			 set {_cod_municipio = value;}
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
