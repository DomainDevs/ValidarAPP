using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Mpersona_dir
	{

		#region InnerClass
		public enum Mpersona_dirFields
		{
            id_persona,
            cod_tipo_dir,
            cod_tipo_dir_old, //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
			cod_calle1,
			nro_nro1,
			txt_desc1,
			cod_calle2,
			nro_nro2,
			txt_desc2,
			nro_nro3,
			nro_apto,
			txt_observaciones,
			txt_direccion,
			nro_cod_postal,
			cod_pais,
			cod_dpto,
			cod_municipio,
			cod_zona_dir,
            sn_domicilio,
            sn_principal
		}
		#endregion

		#region Data Members

			int _id_persona;
			double _cod_tipo_dir;
            double _cod_tipo_dir_old; //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
			double _cod_calle1;
			double _nro_nro1;
			string _txt_desc1;
			double _cod_calle2;
			double _nro_nro2;
			string _txt_desc2;
			double _nro_nro3;
			double _nro_apto;
			string _txt_observaciones;
			string _txt_direccion;
			string _nro_cod_postal;
			double _cod_pais;
			double _cod_dpto;
			double _cod_municipio;
			string _cod_zona_dir;
            int _sn_domicilio;
            int _sn_principal;
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
		public double  cod_tipo_dir
		{
			 get { return _cod_tipo_dir; }
			 set {_cod_tipo_dir = value;}
		}

        //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g
        [DataMember]
        public double cod_tipo_dir_old
        {
            get { return _cod_tipo_dir_old; }
            set { _cod_tipo_dir_old = value; }
        }
        //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de direccion antiguo para modificacion de este en 2g

		[DataMember]
		public double  cod_calle1
		{
			 get { return _cod_calle1; }
			 set {_cod_calle1 = value;}
		}

		[DataMember]
		public double  nro_nro1
		{
			 get { return _nro_nro1; }
			 set {_nro_nro1 = value;}
		}

		[DataMember]
		public string  txt_desc1
		{
			 get { return _txt_desc1; }
			 set {_txt_desc1 = value;}
		}

		[DataMember]
		public double  cod_calle2
		{
			 get { return _cod_calle2; }
			 set {_cod_calle2 = value;}
		}

		[DataMember]
		public double  nro_nro2
		{
			 get { return _nro_nro2; }
			 set {_nro_nro2 = value;}
		}

		[DataMember]
		public string  txt_desc2
		{
			 get { return _txt_desc2; }
			 set {_txt_desc2 = value;}
		}

		[DataMember]
		public double  nro_nro3
		{
			 get { return _nro_nro3; }
			 set {_nro_nro3 = value;}
		}

		[DataMember]
		public double  nro_apto
		{
			 get { return _nro_apto; }
			 set {_nro_apto = value;}
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
		public string  cod_zona_dir
		{
			 get { return _cod_zona_dir; }
			 set {_cod_zona_dir = value;}
		}

        [DataMember]
        public int sn_domicilio
        {
            get { return _sn_domicilio; }
            set { _sn_domicilio = value; }
        }

        [DataMember]
        public int sn_principal
        {
            get { return _sn_principal; }
            set { _sn_principal = value; }
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

        [DataMember]
        public bool IS_HOME
        {
            get;
            set;
        }
		#endregion

	}
}
