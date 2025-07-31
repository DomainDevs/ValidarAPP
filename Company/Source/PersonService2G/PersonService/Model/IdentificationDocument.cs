using System;

namespace PersonService.Model
{
    public class IdentificationDocument
    {
        public string Number { get; set; }
        
        public DateTime ExpeditionDate { get; set; }
        
        public DocumentType DocumentType { get; set; }
    }
}
