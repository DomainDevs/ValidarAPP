using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tmoneda
	{

		#region InnerClass
		public enum TmonedaFields
		{
			cod_moneda,
			txt_desc_redu,
			txt_desc,
			imp_dif_max,
			cnt_decimales,
			cnt_decimales_cambio,
			pje_desvio_cambio_ingreso,
			pje_desvio_cambio_aplicacion
		}
		#endregion

		#region Data Members

			double _cod_moneda;
			string _txt_desc_redu;
			string _txt_desc;
			double _imp_dif_max;
			double _cnt_decimales;
			double _cnt_decimales_cambio;
			string _pje_desvio_cambio_ingreso;
			string _pje_desvio_cambio_aplicacion;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_moneda
		{
			 get { return _cod_moneda; }
			 set {_cod_moneda = value;}
		}

		[DataMember]
		public string  txt_desc_redu
		{
			 get { return _txt_desc_redu; }
			 set {_txt_desc_redu = value;}
		}

		[DataMember]
		public string  txt_desc
		{
			 get { return _txt_desc; }
			 set {_txt_desc = value;}
		}

		[DataMember]
		public double  imp_dif_max
		{
			 get { return _imp_dif_max; }
			 set {_imp_dif_max = value;}
		}

		[DataMember]
		public double  cnt_decimales
		{
			 get { return _cnt_decimales; }
			 set {_cnt_decimales = value;}
		}

		[DataMember]
		public double  cnt_decimales_cambio
		{
			 get { return _cnt_decimales_cambio; }
			 set {_cnt_decimales_cambio = value;}
		}

		[DataMember]
		public string  pje_desvio_cambio_ingreso
		{
			 get { return _pje_desvio_cambio_ingreso; }
			 set {_pje_desvio_cambio_ingreso = value;}
		}

		[DataMember]
		public string  pje_desvio_cambio_aplicacion
		{
			 get { return _pje_desvio_cambio_aplicacion; }
			 set {_pje_desvio_cambio_aplicacion = value;}
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
