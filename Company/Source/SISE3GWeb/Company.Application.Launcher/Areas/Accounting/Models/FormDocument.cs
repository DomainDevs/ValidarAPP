
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models
{
    [KnownType("FormDocument")]
    public class FormDocument
    {
        public int TemporalFileId { get; set; }
        public string TemporalFilePath { get; set; }
        public string TemporalFileName { get; set; }
        public string TemporalFileAlias { get; set; }
        public string TemporalFileType { get; set; }
        public string TemporalFileStatus { get; set; }
        public int TemporalFileTypeId { get; set; }
    }

    [KnownType("FormAttachedDocuments")]
    public class FormAttachedDocuments
    {
        public int Id { get; set; }

        public List<FormDocument> ListFormDocuments { get; set; }

    }
}