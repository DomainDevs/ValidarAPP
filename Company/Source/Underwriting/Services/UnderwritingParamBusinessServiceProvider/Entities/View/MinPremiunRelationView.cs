using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Company.Application.UnderwritingParamBusinessServiceProvider.Entities.Views
{
    [Serializable()]
    public class MinPremiunRelationView : BusinessView
    {
        public BusinessCollection MinPremiumRelation
        {
            get
            {
                return this["MinPremiumRelation"];
            }
        }
        public BusinessCollection Prefix
        {
            get
            {
                return this["Prefix"];
            }
        }
        public BusinessCollection Branch
        {
            get
            {
                return this["Branch"];
            }
        }
        public BusinessCollection EndorsementType
        {
            get
            {
                return this["EndorsementType"];
            }
        }
        public BusinessCollection Currency
        {
            get
            {
                return this["Currency"];
            }
        }
        public BusinessCollection Product
        {
            get
            {
                return this["Product"];
            }
        }
    }
}
