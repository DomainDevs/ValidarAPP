using Domain.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Domain.Models.Entities
{
    public class DBInfo
    {
        [Required]
        [DisplayName("Servidor")]        
        public string servidor { get; set; }

        [Required]
        [DisplayName("Base de Datos")]        
        public string basededatos { get; set; }

        [Required]
        [DisplayName("Usuario")]
        public string usuario { get; set; }

        [Required]
        [DisplayName("Contraseña")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        public BaseDeDatos baseDeDatos { get; set; }

    }
}