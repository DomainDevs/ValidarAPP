using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Magente_comision
	{

		#region InnerClass
		public enum Magente_comisionFields
		{
			cod_tipo_agente,
			cod_agente,
			cod_ramo,
			cod_subramo,
			cod_moneda,
			cod_calc_comis_cob,
			pje_comis_cob,
			cod_calc_comis_normal,
			pje_comis_normal,
			cod_calc_comis_extra,
			pje_comis_extra,
			pje_recargo_adm
		}
		#endregion

		#region Data Members

			double _cod_tipo_agente;
			int _cod_agente;
			double _cod_ramo;
			double _cod_subramo;
			double _cod_moneda;
			double _cod_calc_comis_cob;
			int _pje_comis_cob;
			double _cod_calc_comis_normal;
			int _pje_comis_normal;
			double _cod_calc_comis_extra;
			int _pje_comis_extra;
			int _pje_recargo_adm;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_tipo_agente
		{
			 get { return _cod_tipo_agente; }
			 set {_cod_tipo_agente = value;}
		}

		[DataMember]
		public int  cod_agente
		{
			 get { return _cod_agente; }
			 set {_cod_agente = value;}
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
		public double  cod_moneda
		{
			 get { return _cod_moneda; }
			 set {_cod_moneda = value;}
		}

		[DataMember]
		public double  cod_calc_comis_cob
		{
			 get { return _cod_calc_comis_cob; }
			 set {_cod_calc_comis_cob = value;}
		}

		[DataMember]
		public int  pje_comis_cob
		{
			 get { return _pje_comis_cob; }
			 set {_pje_comis_cob = value;}
		}

		[DataMember]
		public double  cod_calc_comis_normal
		{
			 get { return _cod_calc_comis_normal; }
			 set {_cod_calc_comis_normal = value;}
		}

		[DataMember]
		public int  pje_comis_normal
		{
			 get { return _pje_comis_normal; }
			 set {_pje_comis_normal = value;}
		}

		[DataMember]
		public double  cod_calc_comis_extra
		{
			 get { return _cod_calc_comis_extra; }
			 set {_cod_calc_comis_extra = value;}
		}

		[DataMember]
		public int  pje_comis_extra
		{
			 get { return _pje_comis_extra; }
			 set {_pje_comis_extra = value;}
		}

		[DataMember]
		public int  pje_recargo_adm
		{
			 get { return _pje_recargo_adm; }
			 set {_pje_recargo_adm = value;}
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
