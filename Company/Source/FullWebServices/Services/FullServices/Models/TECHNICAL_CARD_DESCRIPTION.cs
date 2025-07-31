using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class TECHNICAL_CARD_DESCRIPTION
	{

		#region InnerClass
		public enum TECHNICAL_CARD_DESCRIPTIONFields
		{
			TECHNICAL_CARD_DESCRIPTION_CD,
			TECHNICAL_CARD_ID,
			DESCRIPTION_DATE,
			DESCRIPTION,
			USER_ID
		}
		#endregion

		#region Data Members

			int _technical_card_description_cd;
			int _technical_card_id;
			string _description_date;
			string _description;
			int _user_id;
			int _identity; 
			char _state;
            char _state3g; 
			string _connection;
            int _cod_aseg;

		#endregion

		#region Properties

		[DataMember]
		public int  TECHNICAL_CARD_DESCRIPTION_CD
		{
			 get { return _technical_card_description_cd; }
			 set {_technical_card_description_cd = value;}
		}

		[DataMember]
		public int  TECHNICAL_CARD_ID
		{
			 get { return _technical_card_id; }
			 set {_technical_card_id = value;}
		}

		[DataMember]
		public string  DESCRIPTION_DATE
		{
			 get { return _description_date; }
			 set {_description_date = value;}
		}

		[DataMember]
		public string DESCRIPTION
		{
			 get { return _description; }
			 set {_description = value;}
		}

		[DataMember]
		public int  USER_ID
		{
			 get { return _user_id; }
			 set {_user_id = value;}
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
        public char State3G
        {
            get { return _state3g; }
            set { _state3g = value; }
        }

		[DataMember]
		public string  Connection
		{
		  get { return _connection; }
		  set	{ _connection = value;}
		}

        [DataMember]
        public int cod_aseg
        {
            get { return _cod_aseg; }
            set { _cod_aseg = value; }
        }
		#endregion

	}
}
