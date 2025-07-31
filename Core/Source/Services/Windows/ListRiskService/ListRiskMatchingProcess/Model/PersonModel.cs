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
        public bool IsPartialMatch { get; set; }
        public bool IsExcluded { get; set; }
        public bool OfacList { get; set; }
        public bool OwnList { get; set; }
        public bool OnuList { get; set; }
        public string ListDescription { get; set; }
    }
}
