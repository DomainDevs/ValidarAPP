using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniquePersonService.V1.Entities.views
{
    [Serializable()]
    public class PersonAccountBankViewV1 : BusinessView
    {
        public BusinessCollection PersonAccountBank
        {
            get
            {
                return this["PersonAccountBank"];
            }
        }

        public BusinessCollection Bank
        {
            get
            {
                return this["Bank"];
            }
        }

        public BusinessCollection AccountType
        {
            get
            {
                return this["AccountType"];
            }
        }
    }
}