using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class UserIndividualRelation : BusinessView
    {
        public BusinessCollection UniqueUsers
        {
            get
            {
                return this["UniqueUser"];
            }
        }

        public BusinessCollection IndividualRelationsApp
        {
            get
            {
                return this["IndividualRelationApp"];
            }
        }
    }
}
