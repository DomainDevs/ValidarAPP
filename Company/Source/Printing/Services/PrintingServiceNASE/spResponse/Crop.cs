using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.PrintingService.NASE.spResponse
{
    public class Crop
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

        public Crop()
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

        public Crop(DataTable dt1, DataTable dt2)
        {
            MinRow = Convert.ToInt32(dt1.Rows[0]["MIN_ROW"].ToString() != "" ? dt1.Rows[0]["MIN_ROW"].ToString() : "0");
            RowCount = Convert.ToInt32(dt1.Rows[0]["ROW_COUNT"].ToString() != "" ? dt1.Rows[0]["ROW_COUNT"].ToString() : "0");
            SecondPage = Convert.ToInt32(dt1.Rows[0]["SECOND_PAGE"].ToString() != "" ? dt1.Rows[0]["SECOND_PAGE"].ToString() : "0");
            CoveragesCount = Convert.ToInt32(dt1.Rows[0]["COVERAGES_QUANTITY"].ToString() != "" ? dt1.Rows[0]["COVERAGES_QUANTITY"].ToString() : "0");
            BeneficiariesCount = Convert.ToInt32(dt1.Rows[0]["COUNT_BENEFICIARIES"].ToString() != "" ? dt1.Rows[0]["COUNT_BENEFICIARIES"].ToString() : "0");
            TextLinesCount = Convert.ToInt32(dt1.Rows[0]["TOTAL_LINES_COUNT"].ToString() != "" ? dt1.Rows[0]["TOTAL_LINES_COUNT"].ToString() : "0");
            /// HD-2333 07/05/2010 
            /// Autor: Edgar O. Piraneque E.
            /// Descripción: Propiedad que almacena el estado que indica si se debe imprimir o no el formato de recaudo
            /// teniendo en cuenta el valor recibido de procedimiento almacenado
            /// </summary>
            PrintFormat = Convert.ToInt32(dt1.Rows[0]["PRINTFORMAT"].ToString() != "" ? dt1.Rows[0]["PRINTFORMAT"].ToString() : "0");
            ///**************************************************
            /// HD-2181 21/05/2010 
            /// Autor: Edgar O. Piraneque E.
            /// Descripción: Propiedad que almacena el estado que indica si se debe imprimir o no el plan de pago
            /// teniendo en cuenta el valor recibido de procedimiento almacenado
            /// </summary>
            PrintConvection = Convert.ToInt32(dt1.Rows[0]["PRINTCONVECTION"].ToString() != "" ? dt1.Rows[0]["PRINTCONVECTION"].ToString() : "0");
            ///**************************************************
            RegisterCount = Convert.ToInt32(dt2.Rows[0]["TOTAL_REG"].ToString() != "" ? dt2.Rows[0]["TOTAL_REG"].ToString() : "0");
        }

        /*// <summary>
        /// Evalua que versión de reportes se está imprimiendo e indica si se debe anexar 
        /// la segunda hoja del reporte según sea el caso
        /// </summary>
        /// <param name="version">Versión de reportes que se está usando</param>
        /// <returns> Verdadero si alguno de los item de la segunda hoja tiene un valor mayo a cero
        ///           False si no se existen items que imprimir en la segunda hoja
        /// </returns>
        public bool printSecondPage(int version)
        {
            switch (version)
            {
                case (int)ReportEnum.FileReportVersion.VERSION_1: if (TextLength > 0) return true;
                    break;
                case (int)ReportEnum.FileReportVersion.VERSION_2: if (ClausesCount > 0) return true;
                    break;
            }
            return false;
        }*/
    }
}
