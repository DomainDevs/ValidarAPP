namespace Sistran.Company.Application.MassiveUnderwritingServices.EEProvider.Entities.Views
{
    using System;
    using Core.Framework.DAF;
    using Core.Framework.Views;

    [Serializable()]
    public class CompanyMassiveRenewalGetAuthorizations : BusinessView
    {
        public BusinessCollection AutorizarionRequest => this["AutorizarionRequest"];

        public BusinessCollection AutorizarionAnswer => this["AutorizarionAnswer"];

        public BusinessCollection Policies => this["Policies"];

        public BusinessCollection Users => this["Users"];
    }
}

