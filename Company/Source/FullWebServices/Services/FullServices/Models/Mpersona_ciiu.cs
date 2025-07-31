using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Mpersona_ciiu
	{

		#region InnerClass
		public enum Mpersona_ciiuFields
		{
			id_persona,
			cod_ciiu_principal,
			cod_ciiu_secundaria,
			cod_ciiu_ppal_nuevo,
			cod_ciiu_scndria_nuevo
		}
		#endregion

		#region Data Members

			int _id_persona;
			double _cod_ciiu_principal;
			string _cod_ciiu_secundaria;
			string _cod_ciiu_ppal_nuevo;
			string _cod_ciiu_scndria_nuevo;
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
		public double  cod_ciiu_principal
		{
			 get { return _cod_ciiu_principal; }
			 set {_cod_ciiu_principal = value;}
		}

		[DataMember]
		public string  cod_ciiu_secundaria
		{
			 get { return _cod_ciiu_secundaria; }
			 set {_cod_ciiu_secundaria = value;}
		}

		[DataMember]
		public string  cod_ciiu_ppal_nuevo
		{
			 get { return _cod_ciiu_ppal_nuevo; }
			 set {_cod_ciiu_ppal_nuevo = value;}
		}

		[DataMember]
		public string  cod_ciiu_scndria_nuevo
		{
			 get { return _cod_ciiu_scndria_nuevo; }
			 set {_cod_ciiu_scndria_nuevo = value;}
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
