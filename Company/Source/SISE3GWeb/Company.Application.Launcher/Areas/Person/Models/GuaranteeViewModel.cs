using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Person.Models
{
    public class GuaranteeViewModel
    {
        /// <summary>
        /// Id del afianzado
        /// </summary>
        public int ContractorId { get; set; }

        /// <summary>
        /// Id del afianzado
        /// </summary>
        public int ContractorNumber { get; set; }


        /// <summary>
        /// Nombre afianzado
        /// </summary>
        public string ContractorName { get; set; }

        /// <summary>
        /// tipo de documento
        /// </summary>
        public string ContractorDocumentType{ get; set; }

        /// <summary>
        /// Sucursal
        /// </summary>
        [Required]
        public int Branch { get; set; }

        /// <summary>
        /// Cerrada
        /// </summary>
        public bool IsClosed { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        [Required]
        public int Status { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        public int searchType { get; set; }

        public string Address { get; set; }
        
        public string PhoneNumber { get; set; }

        public string CityText { get; set; }

        public int ParamGuarantee { get; set; }

        public DocumentationReceivedViewModel DocumentationReceived { get; set; }
        public PrefixAssociatedViewModel PrefixAssociated { get; set; }
        public BindPolicyViewModel BindPolicy { get; set; }
        public BinnacleViewModel Binnacle { get; set; }

        public PromissoryNoteViewModel PromissoryNote { get; set; }
        public PledgeViewModel Pledge { get; set; }
        public MortageViewModel Mortage { get; set; }
        public OthersViewModel Others { get; set; }
        public FixedTermDepositViewModel FixedTermDeposit { get; set; }

        public ActionsViewModel Actions { get; set; }

    }
}