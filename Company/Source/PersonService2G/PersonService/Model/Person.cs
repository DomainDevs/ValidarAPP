using System;
using System.Collections.Generic;

namespace PersonService.Model
{

    public class Person
    {
        public int Id { get; set; }

        public string FullName { get; set; }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 

        public string DocumentNumber { get; set; }

        public string Role { get; set; }

        public IdentificationDocument IdentificationDocument { get; set; }       
        
        public string Surname { get; set; }
        
        public string SecondSurname { get; set; }

        public string Names { get; set; }

        public int DocumentTypeId { get; set; }
        
        public string Document { get; set; }
        
        public DateTime BirthDate { get; set; }
        
        public string BirthPlace { get; set; }
        
        public string Gender { get; set; }
        
        public int MaritalStatusId { get; set; }
        
        public int? ExonerationTypeCode { get; set; }
        
        public List<Address> Addresses { get; set; }
        
        public List<Email> Emails { get; set; }
        
        public List<Phone> Phones { get; set; }
        
        public int EconomicActivityId { get; set; }
        
        public string EconomicActivityDescription { get; set; }
        
        public string CheckPayable { get; set; }
        
        public int OperationId { get; set; }
    }
}