using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class AGENT
	{

		#region InnerClass
		public enum AGENTFields
		{
			INDIVIDUAL_ID,
			AGENT_TYPE_CD,
			CHECK_PAYABLE_TO,
			ENTERED_DATE,
			DECLINED_DATE,
			AGENT_DECLINED_TYPE_CD,
			LICENSE_NUMBER,
			LICENSE_DATE,
			AGENT_GROUP_CD,
			ANNOTATIONS,
			REFERRED_BY,
			ACC_EXECUTIVE_IND_ID,
			SALES_CHANNEL_CD,
			LOCKER,
			TYPE_LICENSE_CD
		}
		#endregion

		#region Data Members

			int _individual_id;
			int _agent_type_cd;
			string _check_payable_to;
			string _entered_date;
			string _declined_date;
			string _agent_declined_type_cd;
			string _license_number;
			string _license_date;
			string _agent_group_cd;
			string _annotations;
			string _referred_by;
			string _acc_executive_ind_id;
			string _sales_channel_cd;
			string _locker;
			string _type_license_cd;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  INDIVIDUAL_ID
		{
			 get { return _individual_id; }
			 set {_individual_id = value;}
		}

		[DataMember]
		public int  AGENT_TYPE_CD
		{
			 get { return _agent_type_cd; }
			 set {_agent_type_cd = value;}
		}

		[DataMember]
		public string  CHECK_PAYABLE_TO
		{
			 get { return _check_payable_to; }
			 set {_check_payable_to = value;}
		}

		[DataMember]
		public string  ENTERED_DATE
		{
			 get { return _entered_date; }
			 set {_entered_date = value;}
		}

		[DataMember]
		public string  DECLINED_DATE
		{
			 get { return _declined_date; }
			 set {_declined_date = value;}
		}

		[DataMember]
		public string  AGENT_DECLINED_TYPE_CD
		{
			 get { return _agent_declined_type_cd; }
			 set {_agent_declined_type_cd = value;}
		}

		[DataMember]
		public string  LICENSE_NUMBER
		{
			 get { return _license_number; }
			 set {_license_number = value;}
		}

		[DataMember]
		public string  LICENSE_DATE
		{
			 get { return _license_date; }
			 set {_license_date = value;}
		}

		[DataMember]
		public string  AGENT_GROUP_CD
		{
			 get { return _agent_group_cd; }
			 set {_agent_group_cd = value;}
		}

		[DataMember]
		public string  ANNOTATIONS
		{
			 get { return _annotations; }
			 set {_annotations = value;}
		}

		[DataMember]
		public string  REFERRED_BY
		{
			 get { return _referred_by; }
			 set {_referred_by = value;}
		}

		[DataMember]
		public string  ACC_EXECUTIVE_IND_ID
		{
			 get { return _acc_executive_ind_id; }
			 set {_acc_executive_ind_id = value;}
		}

		[DataMember]
		public string  SALES_CHANNEL_CD
		{
			 get { return _sales_channel_cd; }
			 set {_sales_channel_cd = value;}
		}

		[DataMember]
		public string  LOCKER
		{
			 get { return _locker; }
			 set {_locker = value;}
		}

		[DataMember]
		public string  TYPE_LICENSE_CD
		{
			 get { return _type_license_cd; }
			 set {_type_license_cd = value;}
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
