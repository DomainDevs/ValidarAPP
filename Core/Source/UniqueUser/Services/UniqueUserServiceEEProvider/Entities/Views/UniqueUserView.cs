using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class UniqueUserView : BusinessView
    {
        public BusinessCollection UniqueUsers
        {
            get
            {
                return this["UniqueUsers"];
            }
        }

        public BusinessCollection ProfilesUniqueUser
        {
            get
            {
                return this["ProfileUniqueUser"];
            }
        }

        public BusinessCollection Profiles
        {
            get
            {
                return this["Profile"];
            }
        }

        public BusinessCollection Persons
        {
            get
            {
                return this["Person"];
            }
        }

        public BusinessCollection Hierarchies
        {
            get
            {
                return this["CoHierarchyAccess"];
            }
        }

        public BusinessCollection CoHierarchiesAssociation
        {
            get
            {
                return this["CoHierarchyAssociation"];
            }
        }

        public BusinessCollection Modules
        {
            get
            {
                return this["Module"];
            }
        }

        public BusinessCollection SubModules
        {
            get
            {
                return this["SubModule"];
            }
        }

        public BusinessCollection UserSalesPoint
        {
            get
            {
                return this["UserSalePoint"];
            }
        }

        public BusinessCollection Branches
        {
            get
            {
                return this["Branch"];
            }
        }

        public BusinessCollection SalesPoint
        {
            get
            {
                return this["SalePoint"];
            }
        }

        public BusinessCollection UserBranches
        {
            get
            {
                return this["UserBranch"];
            }
        }

        public BusinessCollection IndividualsRelationApp
        {
            get
            {
                return this["IndividualRelationApp"];
            }
        }

        public BusinessCollection AgentAgencies
        {
            get
            {
                return this["AgentAgency"];
            }
        }
    }
}
