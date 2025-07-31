using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Mpersona_rep_legal_dirKeys
	{

		#region Data Members

		int _id_persona;
		string _nro_doc_rep_legal;
		decimal _cod_tipo_doc;
		decimal _cod_tipo_dir;

		#endregion

		#region Constructor

		public Mpersona_rep_legal_dirKeys()
		{
				
		}

		public Mpersona_rep_legal_dirKeys(int id_persona, string nro_doc_rep_legal, decimal cod_tipo_doc, decimal cod_tipo_dir)
		{
			 _id_persona = id_persona; 
			 _nro_doc_rep_legal = nro_doc_rep_legal; 
			 _cod_tipo_doc = cod_tipo_doc; 
			 _cod_tipo_dir = cod_tipo_dir; 
		}

		#endregion

		#region Properties

		public int  id_persona
		{
			 get { return _id_persona; }
		}
		public string  nro_doc_rep_legal
		{
			 get { return _nro_doc_rep_legal; }
		}
		public decimal  cod_tipo_doc
		{
			 get { return _cod_tipo_doc; }
		}
		public decimal  cod_tipo_dir
		{
			 get { return _cod_tipo_dir; }
		}

		#endregion

	}
}
