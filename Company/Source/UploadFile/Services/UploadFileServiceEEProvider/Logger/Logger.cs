using System;
using System.Configuration;
using System.Diagnostics;

namespace Sistran.Core.Application.LoggerUp
{
    public class Logger
    {
        /// <summary>
        /// objeto que permite bloquear.
        /// </summary>
        private object locker = new object();

        /// <summary>
        /// Nombre del proyecto, origen.
        /// </summary>
        private string strSource;

        /// <summary>
        /// Texto del log.
        /// </summary>
        private string strLog;

        /// <summary>
        /// Obtiene o establece un valor que indica si debe escribir eventos.
        /// </summary>
        /// <value>
        ///   <c>Verdadero</c> si escribe log de eventos; de lo contrario, <c>Falso</c>.
        /// </value>
        public bool writeLogEvents { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase Logger.
        /// </summary>
        public Logger()
        {
            strSource = "SistranUploadFileService";
            strLog = "SistranUploadFileService";

            if (!EventLog.SourceExists(strSource))
                EventLog.CreateEventSource(strSource, strLog);

            bool writeLogEvents = false;
            Boolean.TryParse(ConfigurationManager.AppSettings.Get("WriteLogEvents"), out writeLogEvents);
            this.writeLogEvents = writeLogEvents;
        }

        /// <summary>
        /// Escribe la entrada.
        /// </summary>
        /// <param name="message">Texto del mensaje.</param>
        /// <param name="booWrite">Valor que si debe escribir el mensaje (verdadero o falso).</param>
        public void WriteEntry(string message, bool booWrite)
        {
            lock (locker)
            {
                if (booWrite)
                {
                    EventLog.WriteEntry(strSource, message);
                }
            }
        }

        /// <summary>
        /// Escribe el log.
        /// </summary>
        /// <param name="message">Texto del mensaje.</param>
        public void WriteEntry(string message)
        {
            lock (locker)
            {
                if (this.writeLogEvents)
                {
                    EventLog.WriteEntry(strSource, message);
                }
            }
        }
    }
}
