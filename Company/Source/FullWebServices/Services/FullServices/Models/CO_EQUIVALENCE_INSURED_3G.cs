using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class CO_EQUIVALENCE_INSURED_3G
	{

		#region InnerClass
		public enum CO_EQUIVALENCE_INSURED_3GFields
		{
			INDIVIDUAL_2G_ID,
			INDIVIDUAL_3G_ID,
			INSURED_2G_CD,
			INSURED_3G_CD,
			NAME_NUM
		}
		#endregion

		#region Data Members

			int _individual_2g_id;
			int _individual_3g_id;
			int _insured_2g_cd;
			int _insured_3g_cd;
			int _name_num;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int INDIVIDUAL_2G_ID
		{
			 get { return _individual_2g_id; }
			 set {_individual_2g_id = value;}
		}

		[DataMember]
		public int INDIVIDUAL_3G_ID
		{
			 get { return _individual_3g_id; }
			 set {_individual_3g_id = value;}
		}

		[DataMember]
		public int INSURED_2G_CD
		{
			 get { return _insured_2g_cd; }
			 set {_insured_2g_cd = value;}
		}

		[DataMember]
		public int INSURED_3G_CD
		{
			 get { return _insured_3g_cd; }
			 set {_insured_3g_cd = value;}
		}

		[DataMember]
		public int  NAME_NUM
		{
			 get { return _name_num; }
			 set {_name_num = value;}
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