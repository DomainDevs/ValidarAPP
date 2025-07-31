using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Maseg_conducto
	{

		#region InnerClass
		public enum Maseg_conductoFields
		{
			cod_aseg,
			ind_conducto,
			cod_conducto,
			cod_red,
			nro_cta_tarj,
			aaaamm_vto_tarj,
			imp_limite_tarj,
			cod_banco_emisor_tarj,
			sn_habilitado,
			sn_autorizada,
			cnt_dias_rechazos,
			nro_autorizacion,
			nro_secuencia,
			id_negocio,
			sn_respeta_secuencia,
			fec_autorizacion
		}
		#endregion

		#region Data Members

			int _cod_aseg;
			int _ind_conducto;
			double _cod_conducto;
			string _cod_red;
			string _nro_cta_tarj;
			string _aaaamm_vto_tarj;
			double _imp_limite_tarj;
			string _cod_banco_emisor_tarj;
			string _sn_habilitado;
			string _sn_autorizada;
			string _cnt_dias_rechazos;
			string _nro_autorizacion;
			string _nro_secuencia;
			string _id_negocio;
			string _sn_respeta_secuencia;
			string _fec_autorizacion;
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
		public int  ind_conducto
		{
			 get { return _ind_conducto; }
			 set {_ind_conducto = value;}
		}

		[DataMember]
		public double  cod_conducto
		{
			 get { return _cod_conducto; }
			 set {_cod_conducto = value;}
		}

		[DataMember]
		public string  cod_red
		{
			 get { return _cod_red; }
			 set {_cod_red = value;}
		}

		[DataMember]
		public string  nro_cta_tarj
		{
			 get { return _nro_cta_tarj; }
			 set {_nro_cta_tarj = value;}
		}

		[DataMember]
		public string  aaaamm_vto_tarj
		{
			 get { return _aaaamm_vto_tarj; }
			 set {_aaaamm_vto_tarj = value;}
		}

		[DataMember]
		public double  imp_limite_tarj
		{
			 get { return _imp_limite_tarj; }
			 set {_imp_limite_tarj = value;}
		}

		[DataMember]
		public string  cod_banco_emisor_tarj
		{
			 get { return _cod_banco_emisor_tarj; }
			 set {_cod_banco_emisor_tarj = value;}
		}

		[DataMember]
		public string  sn_habilitado
		{
			 get { return _sn_habilitado; }
			 set {_sn_habilitado = value;}
		}

		[DataMember]
		public string  sn_autorizada
		{
			 get { return _sn_autorizada; }
			 set {_sn_autorizada = value;}
		}

		[DataMember]
		public string  cnt_dias_rechazos
		{
			 get { return _cnt_dias_rechazos; }
			 set {_cnt_dias_rechazos = value;}
		}

		[DataMember]
		public string  nro_autorizacion
		{
			 get { return _nro_autorizacion; }
			 set {_nro_autorizacion = value;}
		}

		[DataMember]
		public string  nro_secuencia
		{
			 get { return _nro_secuencia; }
			 set {_nro_secuencia = value;}
		}

		[DataMember]
		public string  id_negocio
		{
			 get { return _id_negocio; }
			 set {_id_negocio = value;}
		}

		[DataMember]
		public string  sn_respeta_secuencia
		{
			 get { return _sn_respeta_secuencia; }
			 set {_sn_respeta_secuencia = value;}
		}

		[DataMember]
		public string  fec_autorizacion
		{
			 get { return _fec_autorizacion; }
			 set {_fec_autorizacion = value;}
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
