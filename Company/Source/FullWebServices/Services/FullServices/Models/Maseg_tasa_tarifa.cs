using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Maseg_tasa_tarifa
	{

		#region InnerClass
		public enum Maseg_tasa_tarifaFields
		{
			cod_aseg,
			cod_ramo,
			cod_subramo,
			cod_tarifa,
			pje_tasa,
			fec_ingreso,
			cod_usuario
		}
		#endregion

		#region Data Members

			int _cod_aseg;
			double _cod_ramo;
			double _cod_subramo;
			double _cod_tarifa;
			double _pje_tasa;
			string _fec_ingreso;
			string _cod_usuario;
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
		public double  cod_ramo
		{
			 get { return _cod_ramo; }
			 set {_cod_ramo = value;}
		}

		[DataMember]
		public double  cod_subramo
		{
			 get { return _cod_subramo; }
			 set {_cod_subramo = value;}
		}

		[DataMember]
		public double  cod_tarifa
		{
			 get { return _cod_tarifa; }
			 set {_cod_tarifa = value;}
		}

		[DataMember]
		public double  pje_tasa
		{
			 get { return _pje_tasa; }
			 set {_pje_tasa = value;}
		}

		[DataMember]
		public string  fec_ingreso
		{
			 get { return _fec_ingreso; }
			 set {_fec_ingreso = value;}
		}

		[DataMember]
		public string  cod_usuario
		{
			 get { return _cod_usuario; }
			 set {_cod_usuario = value;}
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
