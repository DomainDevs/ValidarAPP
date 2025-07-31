using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.PrintingServices.Clases
{
    public class DatRecord
    {
        #region Constructor

        public DatRecord()
        {
            this.FieldValue = string.Empty;
            this.FieldName = string.Empty;
        }

        public DatRecord(string fieldName, string fieldValue)
        {
            this.FieldValue = fieldValue;
            this.FieldName = fieldName;
        }

        #endregion

        #region Attributes
        private string fieldName;

        private object fieldValue;
        #endregion

        #region Properties
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        public object FieldValue
        {
            get { return fieldValue; }
            set { fieldValue = value; }
        }
        #endregion
    }
}
