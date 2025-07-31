using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tipo_persona_aseg
	{

		#region InnerClass
		public enum Tipo_persona_asegFields
		{
			cod_aseg,
			sn_asalariado,
			sn_independiente,
			sn_estudiante,
			sn_rentista,
			sn_socio,
			sn_pensionado,
			sn_jubilado
		}
		#endregion

		#region Data Members

			int _cod_aseg;
			int _sn_asalariado;
			int _sn_independiente;
			int _sn_estudiante;
			int _sn_rentista;
			int _sn_socio;
			int _sn_pensionado;
			int _sn_jubilado;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  cod_aseg
		{
			 get { return _cod_aseg; }
			 set {_cod_aseg = value;}
		}

		[DataMember]
		public int  sn_asalariado
		{
			 get { return _sn_asalariado; }
			 set {_sn_asalariado = value;}
		}

		[DataMember]
		public int  sn_independiente
		{
			 get { return _sn_independiente; }
			 set {_sn_independiente = value;}
		}

		[DataMember]
		public int  sn_estudiante
		{
			 get { return _sn_estudiante; }
			 set {_sn_estudiante = value;}
		}

		[DataMember]
		public int  sn_rentista
		{
			 get { return _sn_rentista; }
			 set {_sn_rentista = value;}
		}

		[DataMember]
		public int  sn_socio
		{
			 get { return _sn_socio; }
			 set {_sn_socio = value;}
		}

		[DataMember]
		public int  sn_pensionado
		{
			 get { return _sn_pensionado; }
			 set {_sn_pensionado = value;}
		}

		[DataMember]
		public int  sn_jubilado
		{
			 get { return _sn_jubilado; }
			 set {_sn_jubilado = value;}
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
