using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.PrintingService.JetForm.Clases
{
    public class PendingPrint
    {
        private int _asyncProcessId;
        private int _printProcessId;
        private string _fileName;
        private string _filePath;
        private decimal _fileSize;
        private int _rangeMinValue;
        private int _rangeMaxValue;
        private string _docTypeDescription;

        public int AsyncProcessId
        {
            get
            {
                return _asyncProcessId;
            }
            set
            {
                _asyncProcessId = value;
            }
        }

        public int PrintProcessId
        {
            get
            {
                return _printProcessId;
            }
            set
            {
                _printProcessId = value;
            }
        }

        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
            }
        }

        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
            }
        }

        public decimal FileSize
        {
            get
            {
                return _fileSize;
            }
            set
            {
                _fileSize = value;
            }
        }

        public int RangeMinValue
        {
            get
            {
                return _rangeMinValue;
            }
            set
            {
                _rangeMinValue = value;
            }
        }

        public int RangeMaxValue
        {
            get
            {
                return _rangeMaxValue;
            }
            set
            {
                _rangeMaxValue = value;
            }
        }

        public string DocTypeDescription
        {
            get
            {
                return _docTypeDescription;
            }
            set
            {
                _docTypeDescription = value;
            }
        }

        /// <summary>
        /// Constructor 1
        /// </summary>
        public PendingPrint()
        {
            AsyncProcessId = 0;
            PrintProcessId = 0;
            FileName = string.Empty;
            FilePath = string.Empty;
            FileSize = (decimal)0.0;
            RangeMinValue = 0;
            RangeMaxValue = 0;
            DocTypeDescription = string.Empty;
        }

        /// <summary>
        /// Constructor 2
        /// </summary>
        /// <param name="dr">Registro con los datos del reporte generado</param>
        public PendingPrint(DataRow dr)
        {
            AsyncProcessId = Convert.ToInt32(dr["AsyncProcessId"].ToString());
            PrintProcessId = Convert.ToInt32(dr["PrintProcessId"].ToString());
            FileName = dr["FileName"].ToString();
            FilePath = dr["FilePath"].ToString();
            FileSize = (dr["FileSize"].ToString().Length < 1) ? 0 : Convert.ToDecimal(dr["FileSize"].ToString());
            RangeMinValue = (dr["RangeMinValue"].ToString().Length < 1) ? 0 : Convert.ToInt32(dr["RangeMinValue"].ToString());
            RangeMaxValue = (dr["RangeMaxValue"].ToString().Length < 1) ? 0 : Convert.ToInt32(dr["RangeMaxValue"].ToString());
            DocTypeDescription = dr["DocTypeDescription"].ToString();
        }

        /// <summary>
        /// Constructor 3
        /// </summary>
        /// <param name="asyncProcessId">Id del proceso asincrono</param>
        /// <param name="printProcessId">Id del proceso de impresión</param>
        /// <param name="fileName">Nombre del archivo generado</param>
        /// <param name="filePath">Ruta del archivo generado</param>
        /// <param name="fileSize">tamaño del archivo generado</param>
        /// <param name="rangeMinValue">Valor menor del rango impreso</param>
        /// <param name="rangeMaxValue">Valor mayor del rango impreso</param>
        /// <param name="docTypeDescription">Descripción del reporte generado</param>
        public PendingPrint(int asyncProcessId, int printProcessId, string fileName, string filePath,
                            decimal fileSize, int rangeMinValue, int rangeMaxValue, string docTypeDescription)
        {
            AsyncProcessId = asyncProcessId;
            PrintProcessId = printProcessId;
            FileName = fileName;
            FilePath = filePath;
            FileSize = fileSize;
            RangeMinValue = rangeMinValue;
            RangeMaxValue = rangeMaxValue;
            DocTypeDescription = docTypeDescription;
        }
    }
}
