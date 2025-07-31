using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServicesProvider.Helpers
{   

    [Serializable]
    public class SupException : Exception
    {
        public SupException()
            : base() { }

        public SupException(string message)
            : base(message) { }

        public SupException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public SupException(string message, Exception innerException)
            : base(message, innerException) 
        { 
            string LowMessage = innerException.Message;
            if (innerException.Message.Contains("insert duplicate key"))
                this.Source = message + " Llave Duplicada";
            else
            {
                this.Source = truncateMessage(innerException.ToString());
            }
        }

        private string truncateMessage(string Message)
        {
            string Tmessge=String.Empty;
            int ibr = Message.IndexOf("\n");
            if (ibr > 0)
                Message = Message.Substring(0, ibr);

            Message = Message.Replace("Sybase.Data.AseClient.AseException:", "");
            return Tmessge = Message;

        }

        public SupException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected SupException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
