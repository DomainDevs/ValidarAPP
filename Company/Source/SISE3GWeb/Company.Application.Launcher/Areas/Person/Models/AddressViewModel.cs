using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Person.Models
{
    public class AddressViewModel
    {

        /// <summary>
        /// Obtiene o setea el Id de la Direccion
        /// </summary>
        /// <value>
        /// Identificador
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// obtiene o setea el consecutivo de la Direccion
        /// </summary>
        /// <value>
        /// consecutivo de la Direccion
        /// </value>
        public int DataId { get; set; }

        /// <summary>
        /// obtiene o setea la descripcion direccion
        /// </summary>
        /// <value>
        /// descripcion direccion
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// obtiene o setea tipo direccion
        /// </summary>
        /// <value>
        /// tipo direccion
        /// </value>
        public int AddressTypeId { get; set; }

        /// <summary>
        /// obtiene o setea descripcion tipo direccion
        /// </summary>
        /// <value>
        /// The address type description.
        /// </value>
        public string AddressTypeDescription { get; set; }

        /// <summary>
        /// obtiene o setea si es la direccion principal
        /// </summary>
        /// <value>
        /// <c>true</c> si es la direccion principal; otro, <c>false</c>.
        /// </value>
        public bool IsMailAddress { get; set; }

        /// <summary>
        /// obtiene o setea el id de la ciudad
        /// </summary>
        /// <value>
        ///   <c>true</c> si [CityId]; otro, <c>false</c>.
        /// </value>
        public int CityId { get; set; }

        /// <summary>
        /// Obtiene o setea el nombre de la Ciudad
        /// </summary>
        /// <value>
        /// nombre de la Ciudad
        /// </value>
        public string CityDescription { get; set; }

        /// <summary>
        /// Obtiene o setea el id del departamento
        /// </summary>
        /// <value>
        ///   <c>true</c> si [StateId]; otro, <c>false</c>.
        /// </value>
        public int StateId { get; set; }

        /// <summary>
        /// Obtiene o setea el nombre del departamento
        /// </summary>
        /// <value>
        ///  nombre del departamento
        /// </value>
        public string StateDescription { get; set; }

        /// <summary>
        /// Obtiene o setea el id del pais
        /// </summary>
        /// <value>
        ///   <c>true</c> si [CountryId]; otro, <c>false</c>.
        /// </value>
        public int CountryId { get; set; }

        /// <summary>
        /// Obtiene o setea el nombre del pais
        /// </summary>
        /// <value>
        ///  nombre del pais
        /// </value>
        public string CountryDescription { get; set; }

    }
}