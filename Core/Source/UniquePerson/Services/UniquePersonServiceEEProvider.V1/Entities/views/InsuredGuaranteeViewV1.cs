using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniquePersonService.V1.Entities.views
{
    [Serializable()]
    public class InsuredGuaranteeViewV1 : BusinessView
    {
        public BusinessCollection GuaranteeClasses
        {
            get
            {
                return this["GuaranteeClass"];
            }
        }

        public BusinessCollection GuaranteeTypes
        {
            get
            {
                return this["GuaranteeType"];
            }
        }

        public BusinessCollection Guarantees
        {
            get
            {
                return this["Guarantee"];
            }
        }

        public BusinessCollection InsuredGuarantees
        {
            get
            {
                return this["InsuredGuarantee"];
            }
        }

        public BusinessCollection Currencyes
        {
            get
            {
                return this["Currency"];
            }
        }

        public BusinessCollection PromissoryNoteTypes
        {
            get
            {
                return this["PromissoryNoteType"];
            }
        }

        public BusinessCollection Insureds
        {
            get
            {
                return this["Insured"];
            }
        }

        public BusinessCollection GuaranteeStates
        {
            get
            {
                return this["GuaranteeStatus"];
            }
        }
    }
}
