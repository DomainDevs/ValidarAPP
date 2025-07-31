using Sistran.Company.Application.PrintingServices.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.PrintingServices.Models
{
    public class Vehicle
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

        #endregion

        public Vehicle()
        {
            MinRow = 0;
            RowCount = 0;
            DeductCount = 0;
            TextLength = 0;
            CoveragesCount = 0;
            AccesoriesCount = 0;
            ClausesCount = 0;
            RegisterCount = 0;
        }

        public Vehicle(DataTable dt1, DataTable dt2)
        {
            MinRow = Convert.ToInt32(dt1.Rows[0]["MIN_ROW"].ToString());
            RowCount = Convert.ToInt32(dt1.Rows[0]["ROW_COUNT"].ToString());
            DeductCount = Convert.ToInt32(dt1.Rows[0]["DEDUCT_COUNT"].ToString());
            TextLength = Convert.ToInt32(dt1.Rows[0]["CONDITION_TEXT"].ToString());
            CoveragesCount = Convert.ToInt32(dt1.Rows[0]["COVERAGES"].ToString());
            AccesoriesCount = Convert.ToInt32(dt1.Rows[0]["ACCESORIES"].ToString());
            ClausesCount = Convert.ToInt32(dt1.Rows[0]["CLAUSES"].ToString());
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
                case (int)FileReportVersion.VERSION_1:
                    if (TextLength > (int)TextLines.MAX_CHAR_PER_LINE) secondPage = true;
                    break;
                case (int)FileReportVersion.VERSION_2:
                    if ((ClausesCount > 0) || (AccesoriesCount > 30) || (TextLength > (int)TextLines.MAX_CHAR_PER_LINE)) secondPage = true;
                    break;
            }
            return secondPage;
        }
    }
}
