using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.SarlaftBusinessServicesProvider.Entities.Views
{
    [Serializable()]
    public class IndividualLinkView : BusinessView
    {
        public BusinessCollection IndividualLinks
        {
            get
            {
                return this["IndividualLink"];
            }
        }

        public BusinessCollection LinkTypes
        {
            get
            {
                return this["LinkType"];
            }
        }

        public BusinessCollection RelationShipSarlafts
        {
            get
            {
                return this["RelationshipSarlaft"];
            }
        }
    }
}
