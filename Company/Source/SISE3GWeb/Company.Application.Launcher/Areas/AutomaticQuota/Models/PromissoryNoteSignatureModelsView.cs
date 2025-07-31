using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Framework.UIF.Web.Areas.AutomaticQuota.Models
{
    
    public class PromissoryNoteSignatureModelsView
    {

        public int Id { get; set; }

        public string SmallDescrption { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }
    }
}
