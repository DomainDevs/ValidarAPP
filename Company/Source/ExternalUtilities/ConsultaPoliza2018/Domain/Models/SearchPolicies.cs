using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Domain.Models.Entities
{
    public class SearchPolicies
    {
        [Required]
        [DisplayName("Número de Póliza")]
        [Range(1, long.MaxValue, ErrorMessage = "Debe ser major a uno {1}")]
        public long PolizaNum { get; set; }

        [DisplayName("Número de Póliza a Comparar")]
        [Range(1, long.MaxValue, ErrorMessage = "Debe ser major a uno {1}")]
        public long? PolizaNum2 { get; set; } = new long?();

        [Required]
        [DisplayName("Código del Ramo")]
        [Range(1, short.MaxValue, ErrorMessage = "Debe ser major a uno {1}")]
        public short Ramo { get; set; }

        [Required]
        [DisplayName("Código de la Sucursal")]
        [Range(1, short.MaxValue, ErrorMessage = "Debe ser major a uno {1}")]
        public short Sucursal { get; set; }

        [Required]
        [DisplayName("Servidor")]
        public string servidor1 { get; set; }

        [Required]
        [DisplayName("Base de Datos")]
        public string basededatos1 { get; set; }

        [Required]
        [DisplayName("Usuario")]
        public string usuario1 { get; set; }

        [Required]
        [DisplayName("Contraseña")]
        [DataType(DataType.Password)]
        public string password1 { get; set; }

        [Required]
        [DisplayName("Servidor")]
        public string servidor2 { get; set; }

        [Required]
        [DisplayName("Base de Datos")]
        public string basededatos2 { get; set; }

        [Required]
        [DisplayName("Usuario")]
        public string usuario2 { get; set; }

        [Required]
        [DisplayName("Contraseña")]
        [DataType(DataType.Password)]
        public string password2 { get; set; }

        [Required]
        [DisplayName("Tipo de Conexión")]
        public int tipoConexion1 { get; set; }
        [Required]
        [DisplayName("Tipo de Conexión")]
        public int tipoConexion2 { get; set; }

    }

    public class Items {
        public int? Value { get; set; }
        public string Text { get; set; }
    }
}