using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Timpuesto
	{

		#region InnerClass
		public enum TimpuestoFields
		{
			cod_impuesto,
			txt_desc,
			txt_desc_reducida,
			pje_impuesto,
			base_minima,
			impuesto_minimo,
			retencion_impuesto,
			sn_vigente,
			sn_devengado,
			sn_retencion,
			cod_impuesto_base_ret
		}
		#endregion

		#region Data Members

			double _cod_impuesto;
			string _txt_desc;
			string _txt_desc_reducida;
			int _pje_impuesto;
			double _base_minima;
			double _impuesto_minimo;
			int _retencion_impuesto;
			int _sn_vigente;
			int _sn_devengado;
			int _sn_retencion;
			double _cod_impuesto_base_ret;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_impuesto
		{
			 get { return _cod_impuesto; }
			 set {_cod_impuesto = value;}
		}

		[DataMember]
		public string  txt_desc
		{
			 get { return _txt_desc; }
			 set {_txt_desc = value;}
		}

		[DataMember]
		public string  txt_desc_reducida
		{
			 get { return _txt_desc_reducida; }
			 set {_txt_desc_reducida = value;}
		}

		[DataMember]
		public int  pje_impuesto
		{
			 get { return _pje_impuesto; }
			 set {_pje_impuesto = value;}
		}

		[DataMember]
		public double  base_minima
		{
			 get { return _base_minima; }
			 set {_base_minima = value;}
		}

		[DataMember]
		public double  impuesto_minimo
		{
			 get { return _impuesto_minimo; }
			 set {_impuesto_minimo = value;}
		}

		[DataMember]
		public int  retencion_impuesto
		{
			 get { return _retencion_impuesto; }
			 set {_retencion_impuesto = value;}
		}

		[DataMember]
		public int  sn_vigente
		{
			 get { return _sn_vigente; }
			 set {_sn_vigente = value;}
		}

		[DataMember]
		public int  sn_devengado
		{
			 get { return _sn_devengado; }
			 set {_sn_devengado = value;}
		}

		[DataMember]
		public int  sn_retencion
		{
			 get { return _sn_retencion; }
			 set {_sn_retencion = value;}
		}

		[DataMember]
		public double  cod_impuesto_base_ret
		{
			 get { return _cod_impuesto_base_ret; }
			 set {_cod_impuesto_base_ret = value;}
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
