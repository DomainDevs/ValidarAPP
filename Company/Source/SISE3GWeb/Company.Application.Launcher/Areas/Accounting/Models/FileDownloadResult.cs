using System;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models
{
    public class FileDownloadResult : ContentResult
    {
        readonly string fileName;
        readonly byte[] fileData;

        /// <summary>
        /// FileDownloadResult
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileData"></param>
        public FileDownloadResult(string fileName, byte[] fileData)
        {
            this.fileName = fileName;
            this.fileData = fileData;
        }

        /// <summary>
        /// ExecuteResult
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {

            if (string.IsNullOrEmpty(this.fileName))
            {
                throw new Exception("A file name is required.");
            }

            if (this.fileData == null)
            {
                throw new Exception("File data is required.");
            }

            var contentDisposition = string.Format("attachment; filename={0}", this.fileName);

            context.HttpContext.Response.AddHeader("Content-Disposition", contentDisposition);
            ContentType = "application/force-download";
            context.HttpContext.Response.BinaryWrite(this.fileData);
        }
    }
}