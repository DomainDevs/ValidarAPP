using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class CO_PRV_INDIVIDUAL
	{

		#region InnerClass
		public enum CO_PRV_INDIVIDUALFields
		{
			INDIVIDUAL_ID,
			ECONOMIC_ACTIVITY_CD_NEW,
			SECOND_ECONOMIC_ACTIVITY_CD_NEW
		}
		#endregion

		#region Data Members

			int _individual_id;
			string _economic_activity_cd_new;
			string _second_economic_activity_cd_new;
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
		public string  ECONOMIC_ACTIVITY_CD_NEW
		{
			 get { return _economic_activity_cd_new; }
			 set {_economic_activity_cd_new = value;}
		}

		[DataMember]
		public string  SECOND_ECONOMIC_ACTIVITY_CD_NEW
		{
			 get { return _second_economic_activity_cd_new; }
			 set {_second_economic_activity_cd_new = value;}
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
