using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    public class Mpersona_dirKeys
    {

        #region Data Members

        int _id_persona;
        decimal _cod_tipo_dir;

        #endregion

        #region Constructor

        public Mpersona_dirKeys()
        {

        }

        public Mpersona_dirKeys(int id_persona, decimal cod_tipo_dir)
        {
            _id_persona = id_persona;
            _cod_tipo_dir = cod_tipo_dir;
        }

        #endregion

        #region Properties

        public int id_persona
        {
            get { return _id_persona; }
        }
        public decimal cod_tipo_dir
        {
            get { return _cod_tipo_dir; }
        }

        #endregion

    }
}
