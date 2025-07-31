namespace Sistran.Core.Application.CollectiveServices.EEProvider.Entities.Views
{
    using System;
    using Framework.DAF;
    using Framework.Views;

    [Serializable()]
    public class CollectiveGetAuthorizations : BusinessView
    {
        public BusinessCollection AutorizarionRequest => this["AutorizarionRequest"];

        public BusinessCollection AutorizarionAnswer => this["AutorizarionAnswer"];

        public BusinessCollection Policies => this["Policies"];

        public BusinessCollection Users => this["Users"];
    }
}

