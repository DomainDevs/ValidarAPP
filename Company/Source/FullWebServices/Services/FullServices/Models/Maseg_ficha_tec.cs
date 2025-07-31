using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Maseg_ficha_tec
	{

		#region InnerClass
		public enum Maseg_ficha_tecFields
		{
			cod_aseg,
			nro_matricula,
			fec_desde_matric,
			fec_hasta_matric,
			imp_k_autorizado,
			txt_revisor_fiscal,
			txt_ubicacion_ficha,
			cod_experiencia,
			txt_cpto_financiero,
			txt_obs_cumulo,
			txt_otras_obs,
			txt_objeto_soc,
			txt_experiencia_en,
			txt_referencias,
			fec_creacion,
			fec_modif,
			cod_usuario_crea,
			cod_usuario_modif,
			txt_contragarantias,
			imp_cumulo,
			fec_cifin
		}
		#endregion

		#region Data Members

			int _cod_aseg;
			string _nro_matricula;
			string _fec_desde_matric;
			string _fec_hasta_matric;
			double _imp_k_autorizado;
			string _txt_revisor_fiscal;
			string _txt_ubicacion_ficha;
			double _cod_experiencia;
			string _txt_cpto_financiero;
			string _txt_obs_cumulo;
			string _txt_otras_obs;
			string _txt_objeto_soc;
			string _txt_experiencia_en;
			string _txt_referencias;
			string _fec_creacion;
			string _fec_modif;
			string _cod_usuario_crea;
			string _cod_usuario_modif;
			string _txt_contragarantias;
			string _imp_cumulo;
			string _fec_cifin;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  cod_aseg
		{
			 get { return _cod_aseg; }
			 set {_cod_aseg = value;}
		}

		[DataMember]
		public string  nro_matricula
		{
			 get { return _nro_matricula; }
			 set {_nro_matricula = value;}
		}

		[DataMember]
		public string  fec_desde_matric
		{
			 get { return _fec_desde_matric; }
			 set {_fec_desde_matric = value;}
		}

		[DataMember]
		public string  fec_hasta_matric
		{
			 get { return _fec_hasta_matric; }
			 set {_fec_hasta_matric = value;}
		}

		[DataMember]
		public double  imp_k_autorizado
		{
			 get { return _imp_k_autorizado; }
			 set {_imp_k_autorizado = value;}
		}

		[DataMember]
		public string  txt_revisor_fiscal
		{
			 get { return _txt_revisor_fiscal; }
			 set {_txt_revisor_fiscal = value;}
		}

		[DataMember]
		public string  txt_ubicacion_ficha
		{
			 get { return _txt_ubicacion_ficha; }
			 set {_txt_ubicacion_ficha = value;}
		}

		[DataMember]
		public double  cod_experiencia
		{
			 get { return _cod_experiencia; }
			 set {_cod_experiencia = value;}
		}

		[DataMember]
		public string  txt_cpto_financiero
		{
			 get { return _txt_cpto_financiero; }
			 set {_txt_cpto_financiero = value;}
		}

		[DataMember]
		public string  txt_obs_cumulo
		{
			 get { return _txt_obs_cumulo; }
			 set {_txt_obs_cumulo = value;}
		}

		[DataMember]
		public string  txt_otras_obs
		{
			 get { return _txt_otras_obs; }
			 set {_txt_otras_obs = value;}
		}

		[DataMember]
		public string  txt_objeto_soc
		{
			 get { return _txt_objeto_soc; }
			 set {_txt_objeto_soc = value;}
		}

		[DataMember]
		public string  txt_experiencia_en
		{
			 get { return _txt_experiencia_en; }
			 set {_txt_experiencia_en = value;}
		}

		[DataMember]
		public string  txt_referencias
		{
			 get { return _txt_referencias; }
			 set {_txt_referencias = value;}
		}

		[DataMember]
		public string  fec_creacion
		{
			 get { return _fec_creacion; }
			 set {_fec_creacion = value;}
		}

		[DataMember]
		public string  fec_modif
		{
			 get { return _fec_modif; }
			 set {_fec_modif = value;}
		}

		[DataMember]
		public string  cod_usuario_crea
		{
			 get { return _cod_usuario_crea; }
			 set {_cod_usuario_crea = value;}
		}

		[DataMember]
		public string  cod_usuario_modif
		{
			 get { return _cod_usuario_modif; }
			 set {_cod_usuario_modif = value;}
		}

		[DataMember]
		public string  txt_contragarantias
		{
			 get { return _txt_contragarantias; }
			 set {_txt_contragarantias = value;}
		}

		[DataMember]
		public string  imp_cumulo
		{
			 get { return _imp_cumulo; }
			 set {_imp_cumulo = value;}
		}

		[DataMember]
		public string  fec_cifin
		{
			 get { return _fec_cifin; }
			 set {_fec_cifin = value;}
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
