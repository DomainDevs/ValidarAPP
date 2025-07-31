using System;
using Sistran.Core.Framework.Views;
using Sistran.Core.Framework.DAF;
using Sistran.Company.Application.Request.Entities;
using Sistran.Core.Application.Common.Entities;

namespace Sistran.Company.Application.MassiveServices.EEProvider.Entities.View
{
    [Serializable()]
    public class CoRequestCoinsuranceAssignedView : BusinessView
    {
        public BusinessCollection CoRequestCoinsuranceAssigned
        {
            get
            {
                return this["CoRequestCoinsuranceAssigned"];
            }
        }

        public BusinessCollection CoInsuranceCompany
        {
            get
            {
                return this["CoInsuranceCompany"];
            }
        }
        //public CoInsuranceCompany GetCoInsuranceCompanyByCoRequestCoinsuranceAssigned(
        //    CoRequestCoinsuranceAssigned coRequestCoinsuranceAssigned)
        //{
        //    return (CoInsuranceCompany)this.GetObjectByRelation(
        //        "CoInsuranceCompany", coRequestCoinsuranceAssigned, true);
        //}
    }
}
