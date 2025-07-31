using System.Runtime.Serialization;

namespace Sistran.Company.Application.Declaration.DTO
{
    /// <summary>
    /// modelo para textos 
    /// </summary>
    [DataContract]
	public class TextDTO
    {
        /// <summary>
        /// identificacion  
        /// </summary>
		[DataMember]
		public int Id { get; set; }

        /// <summary>
        /// texto 
        /// </summary>
		[DataMember]
		public string TextTitle { get; set; }

        /// <summary>
        /// comentarios 
        /// </summary>
		[DataMember]
		public string TextBody { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Observations { get; set; }

        /// <summary>
        /// constructor de la clase 
        /// </summary>
		public TextDTO()
        {

        }
    }
}