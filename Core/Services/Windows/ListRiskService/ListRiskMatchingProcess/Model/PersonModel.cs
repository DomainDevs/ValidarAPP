using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListRiskMatchingProcess.Model
{
    public class PersonModel
    {
        public int IndividualId { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string SecondSurName { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
    }
}
