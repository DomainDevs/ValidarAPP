using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tsubramo
	{

		#region InnerClass
		public enum TsubramoFields
		{
			cod_ramo,
			cod_subramo,
			txt_desc,
			cod_ramo_cta_cte_cias_reas,
			cod_subramo_cta_cte_cias_reas,
			sn_amit
		}
		#endregion

		#region Data Members

			double _cod_ramo;
			double _cod_subramo;
			string _txt_desc;
			string _cod_ramo_cta_cte_cias_reas;
			string _cod_subramo_cta_cte_cias_reas;
			string _sn_amit;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

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
		public string  txt_desc
		{
			 get { return _txt_desc; }
			 set {_txt_desc = value;}
		}

		[DataMember]
		public string  cod_ramo_cta_cte_cias_reas
		{
			 get { return _cod_ramo_cta_cte_cias_reas; }
			 set {_cod_ramo_cta_cte_cias_reas = value;}
		}

		[DataMember]
		public string  cod_subramo_cta_cte_cias_reas
		{
			 get { return _cod_subramo_cta_cte_cias_reas; }
			 set {_cod_subramo_cta_cte_cias_reas = value;}
		}

		[DataMember]
		public string  sn_amit
		{
			 get { return _sn_amit; }
			 set {_sn_amit = value;}
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
