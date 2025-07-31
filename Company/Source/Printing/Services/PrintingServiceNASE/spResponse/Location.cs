using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.PrintingService.NASE.spResponse
{
    public class Location
    {
        #region Propierties
        private int _minRow;
        private int _rowCount;
        private int _secondPage;
        private int _coveragesCount;
        private int _beneficiariesCount;
        private int _textLinesCount;
        /// <summary>
        /// HD-2333 07/05/2010 
        /// Autor: Edgar O. Piraneque E.
        /// Descripción: Variable que almacena el estado que indica si se debe imprimir o no el formato de recaudo
        /// teniendo en cuenta el valor recibido de procedimiento almacenado
        /// </summary>
        private int _printFormat;
        ///**************************************************
        private int _registerCount;
        /// <summary>
        /// HD-2181 21/05/2010 
        /// Autor: Edgar O. Piraneque E.
        /// Descripción: Variable que almacena el estado que indica si se debe imprimir o no el plan de pago
        /// teniendo en cuenta el valor recibido de procedimiento almacenado
        /// </summary>
        private int _printConvection;
        ///**************************************************

        public int MinRow
        {
            get { return _minRow; }
            set { _minRow = value; }
        }

        public int RowCount
        {
            get { return _rowCount; }
            set { _rowCount = value; }
        }

        public int SecondPage
        {
            get { return _secondPage; }
            set { _secondPage = value; }
        }

        public int CoveragesCount
        {
            get { return _coveragesCount; }
            set { _coveragesCount = value; }
        }

        public int BeneficiariesCount
        {
            get { return _beneficiariesCount; }
            set { _beneficiariesCount = value; }
        }

        public int TextLinesCount
        {
            get { return _textLinesCount; }
            set { _textLinesCount = value; }
        }

        public int RegisterCount
        {
            get { return _registerCount; }
            set { _registerCount = value; }
        }

        /// <summary>
        /// HD-2333 07/05/2010 
        /// Autor: Edgar O. Piraneque E.
        /// Descripción: Propiedad que almacena el estado que indica si se debe imprimir o no el formato de recaudo
        /// teniendo en cuenta el valor recibido de procedimiento almacenado
        /// </summary>
        public int PrintFormat
        {
            get { return _printFormat; }
            set { _printFormat = value; }
        }
        /// ***************************************************
        /// <summary>
        /// HD-2181 21/05/2010 
        /// Autor: Edgar O. Piraneque E.
        /// Descripción: Propiedad que almacena el estado que indica si se debe imprimir o no el plan de pago
        /// teniendo en cuenta el valor recibido de procedimiento almacenado
        /// </summary>
        public int PrintConvection
        {
            get { return _printConvection; }
            set { _printConvection = value; }
        }
        /// ***************************************************       
        #endregion

        public Location()
        {
            MinRow = 0;
            RowCount = 0;
            SecondPage = 0;
            CoveragesCount = 0;
            BeneficiariesCount = 0;
            TextLinesCount = 0;
            /// HD-2333 07/05/2010 
            /// Autor: Edgar O. Piraneque E.
            /// Descripción: Propiedad que almacena el estado que indica si se debe imprimir o no el formato de recaudo
            /// teniendo en cuenta el valor recibido de procedimiento almacenado
            /// </summary>
            PrintFormat = 0;
            ///**************************************************
            /// HD-2181 21/05/2010 
            /// Autor: Edgar O. Piraneque E.
            /// Descripción: Propiedad que almacena el estado que indica si se debe imprimir o no el plan de pago
            /// teniendo en cuenta el valor recibido de procedimiento almacenado
            /// </summary>
            PrintConvection = 0;
            ///**************************************************
            RegisterCount = 0;
        }

        public Location(DataTable dt1, DataTable dt2)
        {
            MinRow = Convert.ToInt32(dt1.Rows[0]["MIN_ROW"].ToString());
            RowCount = Convert.ToInt32(dt1.Rows[0]["ROW_COUNT"].ToString());
            SecondPage = Convert.ToInt32(dt1.Rows[0]["SECOND_PAGE"]);
            CoveragesCount = Convert.ToInt32(dt1.Rows[0]["COVERAGES_QUANTITY"].ToString());
            BeneficiariesCount = Convert.ToInt32(dt1.Rows[0]["COUNT_BENEFICIARIES"]);
            TextLinesCount = Convert.ToInt32(dt1.Rows[0]["TOTAL_LINES_COUNT"]);
            /// HD-2333 07/05/2010 
            /// Autor: Edgar O. Piraneque E.
            /// Descripción: Propiedad que almacena el estado que indica si se debe imprimir o no el formato de recaudo
            /// teniendo en cuenta el valor recibido de procedimiento almacenado
            /// </summary>
            PrintFormat = Convert.ToInt32(dt1.Rows[0]["PRINTFORMAT"].ToString());
            ///**************************************************
            /// HD-2181 21/05/2010 
            /// Autor: Edgar O. Piraneque E.
            /// Descripción: Propiedad que almacena el estado que indica si se debe imprimir o no el plan de pago
            /// teniendo en cuenta el valor recibido de procedimiento almacenado
            /// </summary>
            PrintConvection = Convert.ToInt32(dt1.Rows[0]["PRINTCONVECTION"].ToString());
            ///**************************************************
            RegisterCount = Convert.ToInt32(dt2.Rows[0]["TOTAL_REG"]);
        }
    }
}
