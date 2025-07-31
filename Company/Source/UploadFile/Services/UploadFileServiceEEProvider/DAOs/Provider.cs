using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.BAF.Application;
using Sistran.Core.Framework.Queries;
using System;

namespace Sistran.Company.Services.UploadFileServices.EEProvider.DAOs
{
    public class Provider
    {
        /// <summary>
        /// Solicitud del procesador
        /// </summary>
        IRequestProcessor _RequestProcessor;
        IApplicationHome _ApplicationHome;

        public IRequestProcessor RequestProcessor { get { return this._RequestProcessor; } }
        public IApplicationHome ApplicationHome { get { return this._ApplicationHome; } }

        public Provider(IRequestProcessor requestProcessor)
        {
            this._RequestProcessor = requestProcessor;
        }

        public Provider(IRequestProcessor requestProcessor, IApplicationHome applicationHome)
        {
            this._RequestProcessor = requestProcessor;
            this._ApplicationHome = applicationHome;
        }



        //todo: Asunto: se agregó como MERGE con SISEService 
        //autor:Diego González
        //fecha:11/05/2012   
        public static void AddCriteriaParameter(ObjectCriteriaBuilder critBuilder, string propertyName, object propertyValue)
        {
            switch (propertyValue.GetType().ToString())
            {
                case "System.Int16":
                    if (Convert.ToInt16(propertyValue) != -1 || propertyValue != null)
                    {
                        critBuilder.Property(propertyName);
                        critBuilder.Equal();
                        critBuilder.Constant(propertyValue);
                    }
                    break;
                case "System.Int32":
                    if (Convert.ToInt32(propertyValue) != -1 || propertyValue != null)
                    {
                        critBuilder.Property(propertyName);
                        critBuilder.Equal();
                        critBuilder.Constant(propertyValue);
                    }
                    break;
                case "System.Int64":
                    if (Convert.ToInt64(propertyValue) != -1 || propertyValue != null)
                    {
                        critBuilder.Property(propertyName);
                        critBuilder.Equal();
                        critBuilder.Constant(propertyValue);
                    }
                    break;
                case "System.String":
                    if (Convert.ToString(propertyValue) != string.Empty || propertyValue != null)
                    {
                        critBuilder.Property(propertyName);
                        critBuilder.Equal();
                        critBuilder.Constant(propertyValue);
                    }
                    break;
                case "System.DateTime":
                    if (propertyValue != null)
                    {
                        critBuilder.Property(propertyName);
                        critBuilder.Equal();
                        critBuilder.Constant(propertyValue);
                    }
                    break;
            }
        }
    }
}
