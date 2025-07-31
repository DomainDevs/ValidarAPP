namespace Sistran.Core.Application.AccountingServices.EEProvider.Business
{
    class IntegrationBusiness
    {
        public decimal GetParticipationPercentageByEndorsementId(int endorsementId)
        {
            decimal percentage = 100;
            var coInsuranceAssignedDTOs = DelegateService.integrationUnderwritingService.GetCoInsuranceByEndorsementId(endorsementId);
            if (coInsuranceAssignedDTOs != null && coInsuranceAssignedDTOs.Count > 0)
            {
                coInsuranceAssignedDTOs.ForEach(x =>
                {
                    percentage -= x.PartCiaPercentage;
                });
            }
            return percentage;
        }
    }
}
