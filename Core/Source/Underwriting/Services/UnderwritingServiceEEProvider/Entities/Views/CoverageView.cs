using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class CoverageView : BusinessView
    {
        public BusinessCollection Coverages
        {
            get
            {
                return this["Coverage"];
            }
        }
        public BusinessCollection CoCoverages
        {
            get
            {
                return this["CoCoverage"];
            }
        }
        public BusinessCollection GroupCoverages
        {
            get
            {
                return this["GroupCoverage"];
            }
        }

        public BusinessCollection CoCoverageValues
        {
            get
            {
                return this["CoCoverageValue"];
            }
        }

        public BusinessCollection InsuredObjects
        {
            get
            {
                return this["InsuredObject"];
            }
        }

        public BusinessCollection GroupInsuredObject
        {
            get
            {
                return this["GroupInsuredObject"];
            }
        }

        public BusinessCollection CoverDetailTypes
        {
            get
            {
                return this["CoverDetailType"];
            }
        }

        public BusinessCollection LinesBusiness
        {
            get
            {
                return this["LineBusiness"];
            }
        }

        public BusinessCollection SubLinesBusiness
        {
            get
            {
                return this["SubLineBusiness"];
            }
        }
        public BusinessCollection CoverageAllied
        {
            get
            {
                return this["AllyCoverage"];
            }
        }
    }
}
