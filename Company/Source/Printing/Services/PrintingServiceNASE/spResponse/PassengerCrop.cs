using Sistran.Company.PrintingService.NASE.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.PrintingService.NASE.spResponse
{
    public class PassengerCrop
    {
        #region Propierties
        private int _minRow;
        private int _rowCount;
        private int _deductCount;
        private int _textLength;
        private int _coveragesCount;
        private int _accesoriesCount;
        private int _clausesCount;
        private int _registerCount;
        private int _secondPage;
        /// HD-2317 04/05/2010 
        /// Autor: Edgar O. Piraneque E.
        /// Descripción: Variable que almacena el estado que indica si se debe imprimir o no el formato de recaudo
        /// teniendo en cuenta el valor recibido de procedimiento almacenado
        /// </summary>
        private int _printFormat;
        ///**************************************************
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

        public int DeductCount
        {
            get { return _deductCount; }
            set { _deductCount = value; }
        }

        public int TextLength
        {
            get { return _textLength; }
            set { _textLength = value; }
        }

        public int CoveragesCount
        {
            get { return _coveragesCount; }
            set { _coveragesCount = value; }
        }

        public int AccesoriesCount
        {
            get { return _accesoriesCount; }
            set { _accesoriesCount = value; }
        }

        public int ClausesCount
        {
            get { return _clausesCount; }
            set { _clausesCount = value; }
        }

        public int RegisterCount
        {
            get { return _registerCount; }
            set { _registerCount = value; }
        }

        public int SecondPage
        {
            get { return _secondPage; }
            set { _secondPage = value; }
        }
        /// HD-2317 04/05/2010 
        /// Autor: Edgar O. Piraneque E.
        /// Descripción: Propiedad que almacena el estado que indica si se debe imprimir o no el formato de recaudo
        /// teniendo en cuenta el valor recibido de procedimiento almacenado
        /// </summary>
        public int PrintFormat
        {
            get { return _printFormat; }
            set { _printFormat = value; }
        }
        /// *********************************************
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

        public PassengerCrop()
        {
            MinRow = 0;
            RowCount = 0;
            DeductCount = 0;
            TextLength = 0;
            CoveragesCount = 0;
            AccesoriesCount = 0;
            ClausesCount = 0;
            RegisterCount = 0;
            SecondPage = 0;
            /// HD-2317 04/05/2010 
            /// Autor: Edgar O. Piraneque E.
            /// Descripción: Propiedad que almacena el estado que indica si se debe imprimir o no el formato de recaudo
            /// teniendo en cuenta el valor recibido de procedimiento almacenado
            /// </summary>
            PrintFormat = 0;
            ///******************************************
            /// HD-2181 21/05/2010 
            /// Autor: Edgar O. Piraneque E.
            /// Descripción: Propiedad que almacena el estado que indica si se debe imprimir o no el plan de pago
            /// teniendo en cuenta el valor recibido de procedimiento almacenado
            /// </summary>
            PrintConvection = 0;
            ///**************************************************
        }

        public PassengerCrop(DataTable dt1, DataTable dt2)
        {
            MinRow = Convert.ToInt32(dt1.Rows[0]["MIN_ROW"].ToString());
            RowCount = Convert.ToInt32(dt1.Rows[0]["ROW_COUNT"].ToString());
            DeductCount = Convert.ToInt32(dt1.Rows[0]["DEDUCT_COUNT"].ToString());
            TextLength = Convert.ToInt32(dt1.Rows[0]["CONDITION_TEXT"].ToString());
            CoveragesCount = Convert.ToInt32(dt1.Rows[0]["COVERAGES"].ToString());
            AccesoriesCount = Convert.ToInt32(dt1.Rows[0]["ACCESORIES"].ToString());
            ClausesCount = Convert.ToInt32(dt1.Rows[0]["CLAUSES"].ToString());
            SecondPage = (dt1.Rows[0]["SECOND_PAGE"].ToString()).Length;
            /// HD-2317 04/05/2010 
            /// Autor: Edgar O. Piraneque E.
            /// Descripción: Propiedad que almacena el estado que indica si se debe imprimir o no el formato de recaudo
            /// teniendo en cuenta el valor recibido de procedimiento almacenado
            /// </summary>
            PrintFormat = Convert.ToInt32(dt1.Rows[0]["PRINTFORMAT"].ToString());
            ///******************************************
            /// HD-2181 21/05/2010 
            /// Autor: Edgar O. Piraneque E.
            /// Descripción: Propiedad que almacena el estado que indica si se debe imprimir o no el plan de pago
            /// teniendo en cuenta el valor recibido de procedimiento almacenado
            /// </summary>
            PrintConvection = Convert.ToInt32(dt1.Rows[0]["PRINTCONVECTION"].ToString());
            ///**************************************************
            RegisterCount = Convert.ToInt32(dt2.Rows[0]["TOTAL_REG"].ToString());
        }

        /// <summary>
        /// Evalua que versión de reportes se está imprimiendo e indica si se debe anexar 
        /// la segunda hoja del reporte según sea el caso
        /// </summary>
        /// <param name="version">Versión de reportes que se está usando</param>
        /// <returns> Verdadero si alguno de los item de la segunda hoja tiene un valor mayo a cero
        ///           False si no se existen items que imprimir en la segunda hoja
        /// </returns>
        public bool printSecondPage(int version)
        {
            Boolean secondPage = false;
            switch (version)
            {
                case (int)ReportEnum.FileReportVersion.VERSION_1:
                    if ((TextLength > (int)ReportEnum.TextLines.MAX_CHAR_PER_LINE) || (SecondPage > 2)) secondPage = true;
                    break;
                case (int)ReportEnum.FileReportVersion.VERSION_2:
                    if ((ClausesCount > 0) || (TextLength > (int)ReportEnum.TextLines.MAX_CHAR_PER_LINE) || (SecondPage > 2)) secondPage = true;
                    break;
            }
            return secondPage;
        }
    }
}
