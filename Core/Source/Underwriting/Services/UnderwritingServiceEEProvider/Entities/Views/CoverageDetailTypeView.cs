using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class CoverageDetailTypeView : BusinessView
    {
        public BusinessCollection CoverDetailTypes
        {
            get
            {
                return this["CoverDetailType"];
            }
        }

        public BusinessCollection DetailTypes
        {
            get
            {
                return this["DetailType"];
            }
        }

        public BusinessCollection Descriptions
        {
            get
            {
                return this["Descriptions"];
            }
        }

        public BusinessCollection SmallDescriptions
        {
            get
            {
                return this["SmallDescription"];
            }
        }

        public BusinessCollection DetailClassCodes
        {
            get
            {
                return this["DetailClassCode"];
            }
        }
    }
}
