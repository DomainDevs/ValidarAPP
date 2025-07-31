using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class BOARD_DIRECTORS
	{

		#region InnerClass
		public enum BOARD_DIRECTORSFields
		{
			BOARD_DIRECTORS_CD,
			TECHNICAL_CARD_ID,
			BOARD_MEMBER_NAME,
			BOARD_MEMBER_JOB_TITLE
		}
		#endregion

		#region Data Members

			int _board_directors_cd;
			int _technical_card_id;
			string _board_member_name;
			string _board_member_job_title;
			int _identity; 
			char _state;
            char _state3g;
			string _connection;
            int _cod_aseg;
		#endregion

		#region Properties

		[DataMember]
		public int  BOARD_DIRECTORS_CD
		{
			 get { return _board_directors_cd; }
			 set {_board_directors_cd = value;}
		}

		[DataMember]
		public int  TECHNICAL_CARD_ID
		{
			 get { return _technical_card_id; }
			 set {_technical_card_id = value;}
		}

		[DataMember]
		public string  BOARD_MEMBER_NAME
		{
			 get { return _board_member_name; }
			 set {_board_member_name = value;}
		}

		[DataMember]
		public string  BOARD_MEMBER_JOB_TITLE
		{
			 get { return _board_member_job_title; }
			 set {_board_member_job_title = value;}
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
