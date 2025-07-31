using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Maseg_asociacion
	{

		#region InnerClass
		public enum Maseg_asociacionFields
		{
			cod_aseg_asociacion,
			cod_aseg,
			nro_correla_asoc,
			fec_asociacion,
			sn_ppal,
			pje_part,
            InsuredName
		}
		#endregion

		#region Data Members

			int _cod_aseg_asociacion;
			int _cod_aseg;
			int _nro_correla_asoc;
			string _fec_asociacion;
			int _sn_ppal;
            //Edward Rubiano -- HD3554 -- 10/12/2015
            //int _pje_part;
            decimal _pje_part;
            //Edward Rubiano -- HD3554 -- 10/12/2015
			int _identity;
            string _InsuredName; 
			char _state; 
			string _connection;
            char _state_3G;

		#endregion

		#region Properties

		[DataMember]
		public int  cod_aseg_asociacion
		{
			 get { return _cod_aseg_asociacion; }
			 set {_cod_aseg_asociacion = value;}
		}

		[DataMember]
		public int  cod_aseg
		{
			 get { return _cod_aseg; }
			 set {_cod_aseg = value;}
		}

		[DataMember]
		public int  nro_correla_asoc
		{
			 get { return _nro_correla_asoc; }
			 set {_nro_correla_asoc = value;}
		}

		[DataMember]
		public string  fec_asociacion
		{
			 get { return _fec_asociacion; }
			 set {_fec_asociacion = value;}
		}

		[DataMember]
		public int  sn_ppal
		{
			 get { return _sn_ppal; }
			 set {_sn_ppal = value;}
		}

        [DataMember]
        //Edward Rubiano -- HD3554 -- 10/12/2015
        //public int pje_part
        public decimal pje_part
        //Edward Rubiano -- HD3554 -- 10/12/2015
		{
			 get { return _pje_part; }
			 set {_pje_part = value;}
		}

        [DataMember]
        public string InsuredName
        {
            get { return _InsuredName; }
            set { _InsuredName = value; }
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

        [DataMember]
        public char State_3G
        {
            get { return _state_3G; }
            set { _state_3G = value; }
        }
		#endregion

	}
}
