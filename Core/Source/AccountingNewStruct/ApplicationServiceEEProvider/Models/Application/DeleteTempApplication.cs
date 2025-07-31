using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Application
{
    public class DeleteTempApplication
    {
        public int TempApplicationId { get; set; }

        public int ApplicationId { get; set; }

        public ApplicationTypes ApplicationType { get; set; }

        public int UserId { get; set; }

        public int SourceId { get; set; }
    }
}
