using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;


namespace Sistran.Core.Application.TempCommonServices.Models
{
    [DataContract]
    public class Product
    {
              
        public Product()
        {
       
        }
       
        /// <summary>
        /// Id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Descripción del producto
        /// </summary>
        /// <param name="Description"></param>
        /// <returns></returns>
        [DataMember]
        public string Description { get; set; }

      


    }
}
