using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Company.Application.UniqueUserServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class CompanyUserBranchView : BusinessView
    {
        public BusinessCollection Branches
        {
            get
            {
                return this["Branch"];
            }
        }

        public BusinessCollection CoBranches
        {
            get
            {
                return this["CoBranch"];
            }
        }

        public BusinessCollection UserBranches
        {
            get
            {
                return this["UserBranch"];
            }
        }
    }
}
