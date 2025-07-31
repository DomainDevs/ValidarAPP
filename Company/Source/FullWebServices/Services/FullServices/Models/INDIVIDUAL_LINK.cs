using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class INDIVIDUAL_LINK
	{

		#region InnerClass
		public enum INDIVIDUAL_LINKFields
		{
			INDIVIDUAL_ID,
			LINK_TYPE_CD,
			RELATIONSHIP_SARLAFT_CD,
			DESCRIPTION
		}
		#endregion

		#region Data Members

			int _individual_id;
			int _link_type_cd;
			int _relationship_sarlaft_cd;
			string _description;
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
		public int  LINK_TYPE_CD
		{
			 get { return _link_type_cd; }
			 set {_link_type_cd = value;}
		}

		[DataMember]
		public int  RELATIONSHIP_SARLAFT_CD
		{
			 get { return _relationship_sarlaft_cd; }
			 set {_relationship_sarlaft_cd = value;}
		}

		[DataMember]
		public string  DESCRIPTION
		{
			 get { return _description; }
			 set {_description = value;}
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
