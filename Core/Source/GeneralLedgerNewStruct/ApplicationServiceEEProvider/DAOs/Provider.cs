using Sistran.Core.Framework.BAF;

namespace Sistran.Core.Application.GeneralLedgerServices.Provider
{
   public  class Provider
    {

        readonly IRequestProcessor _RequestProcessor;
        public IRequestProcessor RequestProcessor { get { return this._RequestProcessor; } }

        public Provider(IRequestProcessor requestProcessor)
        {
            this._RequestProcessor = requestProcessor;
        }
    }
}
