using Sistran.Company.ExternalRenewalServices;
using Sistran.Company.ExternalRenewalServices.Models;
using System;
using System.ServiceModel;

namespace Sistran.Company.ExternalRenewalServicesEEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ExternalRenewalServicesEEProvider : IExternalRenewalServices
    {
        public ResponseFirmRenewalPolicy FirmRenewalPolicy(int tempId)
        {
            return new ResponseFirmRenewalPolicy
            {
                AssistanceAmount = 1,
                BranchCode = 1,
                CurrentFrom = new DateTime(),
                CurrentTo = new DateTime(),
                EndorsementNumber = 1,
                ExpensesAmount = 333,
                LicensePlate = "",
                LimitRcSum = 223,
                PolicyNumber = 1,
                PrefixCode = 30,
                PremiumAmount = 323232,
                ProcessMessage = "",
                ProductId = 130,
                Status = true,
                TaxAmount = 323232,
                TotalPremiumAmount = 323232,
                VehiclePrice = 4324242,
                VehiclePriceDetail = 3343
            };
        }

        public ResponsePolicyRenewal PolicyRenewal(PolicyRenewalRequest policyRenewalRequest) => new ResponsePolicyRenewal
        {
            ExpensesAmount = 332323,
            TaxAmount = 32323,
            BranchCd = 30,
            DocumentNumber = 1,
            EndorsementNumber = 1,
            HasEvents = true,
            InsuredAmount = 3232,
            Message = "",
            PrefixCd = 2,
            PremiumAmount = 32323,
            State = true,
            TempId = 2,
            Events = new System.Collections.Generic.List<Event>
            {
                new Event
                {
                    EventDescription = "",
                    EventId = 1
                }
            }
        };

        public ResponsePolicyRenewal PolicyRenewalByAlliance(PolicyRenewalByAllianceRequest policyRenewalByAllianceRequest) => new ResponsePolicyRenewal
        {
            ExpensesAmount = 332323,
            TaxAmount = 32323,
            BranchCd = 30,
            DocumentNumber = 1,
            EndorsementNumber = 1,
            HasEvents = true,
            InsuredAmount = 3232,
            Message = "",
            PrefixCd = 2,
            PremiumAmount = 32323,
            State = true,
            TempId = 2,
            Events = new System.Collections.Generic.List<Event>
            {
                new Event
                {
                    EventDescription = "",
                    EventId = 1
                }
            }
        };
    }
}
