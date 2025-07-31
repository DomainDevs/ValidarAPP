using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ConsultaPoliza2018.Models.Entities
{
    public class SearchModel
    {
        public bool Itero { get; set; }

        [Required]
        [DisplayName("Numero Poliza")]
        [Range(1, 100000000000, ErrorMessage = "El Valor Minimo es {1} ")]
        public long DOCUMENT_NUM { get; set; } = 1;

        [Required]
        [DisplayName("Sucurzal")]
        [Range(1, 100000000000, ErrorMessage = "El Valor Minimo es {1}")]
        public int BRANCH_CD { get; set; } = 1;

        [Required]
        [DisplayName("Ramo")]
        [Range(1, 1000000, ErrorMessage = "El Valor Minimo es {1} ")]
        public int PREFIX_CD { get; set; } = 1;

        [Required]
        [DisplayName("Numero Poliza 2")]
        [Range(1, 100000000000000, ErrorMessage = "El Valor Minimo es {1}")]
        public long DOCUMENT_NUM_CompareTo { get; set; } = 1;
    }
}