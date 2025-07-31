using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tdirector_comercial_hist
	{

		#region InnerClass
		public enum Tdirector_comercial_histFields
		{
			cod_director_comercial,
			cod_director_nacional,
			cod_suc_asoc,
			id_correla_estado,
			sn_activo,
			fec_activ,
			cod_usuario
		}
		#endregion

		#region Data Members

			int _cod_director_comercial;
			int _cod_director_nacional;
			double _cod_suc_asoc;
			double _id_correla_estado;
			string _sn_activo;
			string _fec_activ;
			string _cod_usuario;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  cod_director_comercial
		{
			 get { return _cod_director_comercial; }
			 set {_cod_director_comercial = value;}
		}

		[DataMember]
		public int  cod_director_nacional
		{
			 get { return _cod_director_nacional; }
			 set {_cod_director_nacional = value;}
		}

		[DataMember]
		public double  cod_suc_asoc
		{
			 get { return _cod_suc_asoc; }
			 set {_cod_suc_asoc = value;}
		}

		[DataMember]
		public double  id_correla_estado
		{
			 get { return _id_correla_estado; }
			 set {_id_correla_estado = value;}
		}

		[DataMember]
		public string  sn_activo
		{
			 get { return _sn_activo; }
			 set {_sn_activo = value;}
		}

		[DataMember]
		public string  fec_activ
		{
			 get { return _fec_activ; }
			 set {_fec_activ = value;}
		}

		[DataMember]
		public string  cod_usuario
		{
			 get { return _cod_usuario; }
			 set {_cod_usuario = value;}
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
