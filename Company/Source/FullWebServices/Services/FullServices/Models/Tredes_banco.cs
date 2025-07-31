using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tredes_banco
	{

		#region InnerClass
		public enum Tredes_bancoFields
		{
			cod_red,
			txt_nombre,
			sn_requiere_autoriz,
			nro_correlativo_negocio,
			sn_tiene_lote,
			cod_red_asociado,
			txt_nombre_archivo_autoriz,
			txt_nombre_archivo_recaudo,
			sn_tiene_control,
			sn_tiene_cabecera
		}
		#endregion

		#region Data Members

			double _cod_red;
			string _txt_nombre;
			string _sn_requiere_autoriz;
			int _nro_correlativo_negocio;
			string _sn_tiene_lote;
			string _cod_red_asociado;
			string _txt_nombre_archivo_autoriz;
			string _txt_nombre_archivo_recaudo;
			string _sn_tiene_control;
			string _sn_tiene_cabecera;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_red
		{
			 get { return _cod_red; }
			 set {_cod_red = value;}
		}

		[DataMember]
		public string  txt_nombre
		{
			 get { return _txt_nombre; }
			 set {_txt_nombre = value;}
		}

		[DataMember]
		public string  sn_requiere_autoriz
		{
			 get { return _sn_requiere_autoriz; }
			 set {_sn_requiere_autoriz = value;}
		}

		[DataMember]
		public int  nro_correlativo_negocio
		{
			 get { return _nro_correlativo_negocio; }
			 set {_nro_correlativo_negocio = value;}
		}

		[DataMember]
		public string  sn_tiene_lote
		{
			 get { return _sn_tiene_lote; }
			 set {_sn_tiene_lote = value;}
		}

		[DataMember]
		public string  cod_red_asociado
		{
			 get { return _cod_red_asociado; }
			 set {_cod_red_asociado = value;}
		}

		[DataMember]
		public string  txt_nombre_archivo_autoriz
		{
			 get { return _txt_nombre_archivo_autoriz; }
			 set {_txt_nombre_archivo_autoriz = value;}
		}

		[DataMember]
		public string  txt_nombre_archivo_recaudo
		{
			 get { return _txt_nombre_archivo_recaudo; }
			 set {_txt_nombre_archivo_recaudo = value;}
		}

		[DataMember]
		public string  sn_tiene_control
		{
			 get { return _sn_tiene_control; }
			 set {_sn_tiene_control = value;}
		}

		[DataMember]
		public string  sn_tiene_cabecera
		{
			 get { return _sn_tiene_cabecera; }
			 set {_sn_tiene_cabecera = value;}
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
