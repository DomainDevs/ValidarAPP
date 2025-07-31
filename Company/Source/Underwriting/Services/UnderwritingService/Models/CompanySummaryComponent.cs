using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class CompanySummaryComponent
    {
        /// <summary>
        /// Gets or sets the Expense.
        /// </summary>
        /// <value>
        /// The Expense.
        /// </value>
        [DataMember]
        public List<CompanyExpenseComponent> Expense { get; set; }
        /// <summary>
        /// Gets or sets the Discount.
        /// </summary>
        /// <value>
        /// The Discount.
        /// </value>
        [DataMember]
        public List<CompanyDiscountComponent> Discount { get; set; }
        /// <summary>
        /// Gets or sets the Surcharge.
        /// </summary>
        /// <value>
        /// The Surcharge.
        /// </value>
        [DataMember]
        public List<CompanySurchargeComponent> Surcharge { get; set; }
        /// <summary>
        /// Gets or sets the TotalExpenses.
        /// </summary>
        /// <value>
        /// The TotalExpenses.
        /// </value>
        [DataMember]
        public decimal TotalExpenses { get; set; }
        /// <summary>
        /// Gets or sets the TotalDiscount.
        /// </summary>
        /// <value>
        /// The TotalDiscount.
        /// </value>
        [DataMember]
        public decimal TotalDiscount { get; set; }
        /// <summary>
        /// Gets or sets the TotalSurcharge.
        /// </summary>
        /// <value>
        /// The TotalSurcharge.
        /// </value>
        [DataMember]
        public decimal TotalSurcharge { get; set; }

    }
}
