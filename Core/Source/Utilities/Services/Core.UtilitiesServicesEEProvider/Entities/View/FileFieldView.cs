using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.UtilitiesServicesEEProvider.Entities.Views
{
    [Serializable()]
    public class FileFieldView : BusinessView
    {
        public BusinessCollection Files
        {
            get
            {
                return this["File"];
            }
        }

        public BusinessCollection FileTemplates
        {
            get
            {
                return this["FileTemplate"];
            }
        }

        public BusinessCollection FileTemplateFields
        {
            get
            {
                return this["FileTemplateField"];
            }
        }

        public BusinessCollection Fields
        {
            get
            {
                return this["Field"];
            }
        }

        public BusinessCollection FieldTypes
        {
            get
            {
                return this["FieldType"];
            }
        }

        public BusinessCollection Template
        {
            get
            {
                return this["Template"];
            }
        }
    }
}