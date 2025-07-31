using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ListRiskMatchingProcess.Model
{
    public class OnuPerson
    {
        public int DataId { get; set; }
        public int VersionNum { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string FourthName { get; set; }
        public Nationality Nationality { get; set; }
        public ListType ListType { get; set; }
        public DateTime ListedOn { get; set; }
        public string Comments1 { get; set; }
        public List<LastDateUpdated> LastDateUpdated { get; set; }
        public List<OnuDocument> Document { get; set; }
        public List<OnuAlias> Alias { get; set; }
        public List<OnuAdrress> Adress { get; set; }
        public DateTime SiseReistrationDate { get; set; }
        public int Event { get; set; }
        public DateTime RegistrationDate { get; set; }
    }

}
