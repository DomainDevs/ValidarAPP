using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class SARLAFT_OPERATION
	{

		#region InnerClass
		public enum SARLAFT_OPERATIONFields
		{
			SARLAFT_OPERATION_ID,
			SARLAFT_ID,
			PRODUCT_NUM,
			PRODUCT_AMT,
			ENTITY,
			OPERATION_TYPE_CD,
			PRODUCT_TYPE_CD,
			CURRENCY_CD,
			COUNTRY_CD,
			STATE_CD,
			CITY_CD
		}
		#endregion

		#region Data Members

			int _sarlaft_operation_id;
			int _sarlaft_id;
			string _product_num;
			double _product_amt;
			string _entity;
			int _operation_type_cd;
			int _product_type_cd;
			int _currency_cd;
			int _country_cd;
			int _state_cd;
			int _city_cd;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  SARLAFT_OPERATION_ID
		{
			 get { return _sarlaft_operation_id; }
			 set {_sarlaft_operation_id = value;}
		}

		[DataMember]
		public int  SARLAFT_ID
		{
			 get { return _sarlaft_id; }
			 set {_sarlaft_id = value;}
		}

		[DataMember]
		public string  PRODUCT_NUM
		{
			 get { return _product_num; }
			 set {_product_num = value;}
		}

		[DataMember]
		public double  PRODUCT_AMT
		{
			 get { return _product_amt; }
			 set {_product_amt = value;}
		}

		[DataMember]
		public string  ENTITY
		{
			 get { return _entity; }
			 set {_entity = value;}
		}

		[DataMember]
		public int  OPERATION_TYPE_CD
		{
			 get { return _operation_type_cd; }
			 set {_operation_type_cd = value;}
		}

		[DataMember]
		public int  PRODUCT_TYPE_CD
		{
			 get { return _product_type_cd; }
			 set {_product_type_cd = value;}
		}

		[DataMember]
		public int  CURRENCY_CD
		{
			 get { return _currency_cd; }
			 set {_currency_cd = value;}
		}

		[DataMember]
		public int  COUNTRY_CD
		{
			 get { return _country_cd; }
			 set {_country_cd = value;}
		}

		[DataMember]
		public int  STATE_CD
		{
			 get { return _state_cd; }
			 set {_state_cd = value;}
		}

		[DataMember]
		public int  CITY_CD
		{
			 get { return _city_cd; }
			 set {_city_cd = value;}
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
