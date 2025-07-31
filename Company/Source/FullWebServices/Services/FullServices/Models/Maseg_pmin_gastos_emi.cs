using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Maseg_pmin_gastos_emi
	{

		#region InnerClass
		public enum Maseg_pmin_gastos_emiFields
		{
			cod_aseg,
			cod_ramo,
			cod_moneda,
			imp_prima_min,
			imp_gastos_emi,
			suma_aseg_max
		}
		#endregion

		#region Data Members

			int _cod_aseg;
			double _cod_ramo;
			double _cod_moneda;
			double _imp_prima_min;
			double _imp_gastos_emi;
			double _suma_aseg_max;
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
		public double  cod_moneda
		{
			 get { return _cod_moneda; }
			 set {_cod_moneda = value;}
		}

		[DataMember]
		public double  imp_prima_min
		{
			 get { return _imp_prima_min; }
			 set {_imp_prima_min = value;}
		}

		[DataMember]
		public double  imp_gastos_emi
		{
			 get { return _imp_gastos_emi; }
			 set {_imp_gastos_emi = value;}
		}

		[DataMember]
		public double  suma_aseg_max
		{
			 get { return _suma_aseg_max; }
			 set {_suma_aseg_max = value;}
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
