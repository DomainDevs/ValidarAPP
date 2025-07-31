using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tmotivo_baja
	{

		#region InnerClass
		public enum Tmotivo_bajaFields
		{
			cod_baja,
			txt_desc,
			sn_maseg,
			sn_magente,
			sn_mpres,
			sn_tercero,
			sn_cesionario,
			sn_beneficiario
		}
		#endregion

		#region Data Members

			double _cod_baja;
			string _txt_desc;
			string _sn_maseg;
			string _sn_magente;
			string _sn_mpres;
			string _sn_tercero;
			int _sn_cesionario;
			string _sn_beneficiario;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_baja
		{
			 get { return _cod_baja; }
			 set {_cod_baja = value;}
		}

		[DataMember]
		public string  txt_desc
		{
			 get { return _txt_desc; }
			 set {_txt_desc = value;}
		}

		[DataMember]
		public string  sn_maseg
		{
			 get { return _sn_maseg; }
			 set {_sn_maseg = value;}
		}

		[DataMember]
		public string  sn_magente
		{
			 get { return _sn_magente; }
			 set {_sn_magente = value;}
		}

		[DataMember]
		public string  sn_mpres
		{
			 get { return _sn_mpres; }
			 set {_sn_mpres = value;}
		}

		[DataMember]
		public string  sn_tercero
		{
			 get { return _sn_tercero; }
			 set {_sn_tercero = value;}
		}

		[DataMember]
		public int  sn_cesionario
		{
			 get { return _sn_cesionario; }
			 set {_sn_cesionario = value;}
		}

		[DataMember]
		public string  sn_beneficiario
		{
			 get { return _sn_beneficiario; }
			 set {_sn_beneficiario = value;}
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
