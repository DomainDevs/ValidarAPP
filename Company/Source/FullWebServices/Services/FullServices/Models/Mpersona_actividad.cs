using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Mpersona_actividad
	{

		#region InnerClass
		public enum Mpersona_actividadFields
		{
			id_persona,
			cod_abona,
			cod_dpto,
			cod_munic,
			cod_actividad,
			cod_usuario,
			fec_registro
		}
		#endregion

		#region Data Members

			int _id_persona;
			double _cod_abona;
			double _cod_dpto;
			double _cod_munic;
			double _cod_actividad;
			string _cod_usuario;
			string _fec_registro;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  id_persona
		{
			 get { return _id_persona; }
			 set {_id_persona = value;}
		}

		[DataMember]
		public double  cod_abona
		{
			 get { return _cod_abona; }
			 set {_cod_abona = value;}
		}

		[DataMember]
		public double  cod_dpto
		{
			 get { return _cod_dpto; }
			 set {_cod_dpto = value;}
		}

		[DataMember]
		public double  cod_munic
		{
			 get { return _cod_munic; }
			 set {_cod_munic = value;}
		}

		[DataMember]
		public double  cod_actividad
		{
			 get { return _cod_actividad; }
			 set {_cod_actividad = value;}
		}

		[DataMember]
		public string  cod_usuario
		{
			 get { return _cod_usuario; }
			 set {_cod_usuario = value;}
		}

		[DataMember]
		public string  fec_registro
		{
			 get { return _fec_registro; }
			 set {_fec_registro = value;}
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
