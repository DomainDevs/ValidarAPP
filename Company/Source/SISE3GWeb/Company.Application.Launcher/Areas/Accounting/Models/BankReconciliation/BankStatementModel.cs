
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.BankReconciliation
{
    public class BankStatementModel
    {
        public int BankStatementId { get; set; }

        [Required]
        public int BankId { get; set; }

        [Required]
        public string BankDescription { get; set; }

        [Required]
        public int BankingMovementTypeId { get; set; }

        [Required]
        public string BankingMovementTypeDescription { get; set; }

        [Required]
        public int BranchId { get; set; }

        [Required]
        public string BranchDescription { get; set; }

        [Required]
        public string VoucherNumber { get; set; }

        [Required]
        public string MovementDate { get; set; }
        [DataType(DataType.Date)]

        [Required]
        public string MovementAmount { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "The field {0} should have minimum {2} and maximum {1} characters", MinimumLength = 10)]
        public string MovementDescription { get; set; }

        [Required]
        public string MovementThird { get; set; }
        public string MovementOrigin { get; set; }
    }

    public class BankStatemenListModel
    {
        public List<BankStatementModel> BankStatementList { get; set; }
    }
}