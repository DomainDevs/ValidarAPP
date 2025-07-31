using System.Collections.Generic;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Framework.DAF;

namespace Utilities.Excel.Assemblers
{
    class ModelAssembler
    {
        public static File CreateFile(COMMEN.File entityFile)
        {
            return new File()
            {
                Id = entityFile.Id,
                Description = entityFile.Description,
                SmallDescription = entityFile.SmallDescription,
                Observations = entityFile.Observations,
                IsEnabled = entityFile.IsEnabled,
                FileType = (FileType)entityFile.FileTypeId,
                Templates = new List<Template>()
            };
        }

        public static COMMEN.FileTemplate CreateFileTemplate(Template Filetemplate, int fileId)
        {
            return new COMMEN.FileTemplate(Filetemplate.Id)
            {
                FileId = fileId,
                IsMandatory = Filetemplate.IsMandatory,
                IsEnabled = Filetemplate.IsEnabled,
                Order = Filetemplate.Order,
                IsPrincipal = Filetemplate.IsPrincipal,
                TemplateId = Filetemplate.TemplateId,
                Description = Filetemplate.Description

            };
        }

        public static COMMEN.FileTemplateField CreateFileTemplateField(Field FiletemplateField, int fileTemplateId)
        {
            return new COMMEN.FileTemplateField(FiletemplateField.TemplateFieldId)
            {
                FileTemplateId = fileTemplateId,
                FieldId = (int)FiletemplateField.Id,
                Order = FiletemplateField.Order,
                ColumnSpan = FiletemplateField.ColumnSpan,
                RowPosition = FiletemplateField.RowPosition,
                IsMandatory = FiletemplateField.IsMandatory,
                IsEnabled = FiletemplateField.IsEnabled,
                Description = FiletemplateField.Description
            };
        }



        #region Field       

        public static List<Field> CreateFields(BusinessCollection businessCollection)
        {
            List<Field> fields = new List<Field>();

            foreach (COMMEN.Field entity in businessCollection)
            {
                fields.Add(ModelAssembler.CreateField(entity));
            }

            return fields;
        }

        public static Field CreateField(COMMEN.Field entityFile)
        {
            return new Field()
            {
                Id = entityFile.Id,
                Description = entityFile.Description,
                SmallDescription = entityFile.SmallDescription,
                FieldType = (FieldType)entityFile.FieldTypeId,
                PropertyName = entityFile.PropertyName,
                PropertyLength = entityFile.PropertyLength
            };
        }


        public static Field CreateField(COMMEN.FileTemplateField entityFileTemplateField, COMMEN.Field entityField)
        {
            return new Field
            {
                Id = entityField.Id,
                TemplateFieldId = entityFileTemplateField.Id,
                Description = entityFileTemplateField.Description,
                SmallDescription = entityField.SmallDescription,
                FieldType = (FieldType)entityField.FieldTypeId,
                IsEnabled = entityFileTemplateField.IsEnabled,
                IsMandatory = entityFileTemplateField.IsMandatory,
                Order = entityFileTemplateField.Order,
                ColumnSpan = entityFileTemplateField.ColumnSpan,
                RowPosition = entityFileTemplateField.RowPosition,
                PropertyName = entityField.PropertyName,
                PropertyLength = entityField.PropertyLength
            };
        }
        #endregion

        #region File



        public static FileProcessValue CreateFileProcessValue(BusinessCollection businessCollection)
        {
            FileProcessValue fileProcess = null;

            if (businessCollection.Count > 0)
            {
                foreach (COMMEN.FileProcessValue item in businessCollection)
                {
                    fileProcess = new FileProcessValue()
                    {
                        Id = item.Id,
                        FileId = item.FileId,
                        Key1 = item.Key1,
                        Key2 = item.Key2.HasValue ? item.Key2.Value : 0,
                        Key3 = item.Key3.HasValue ? item.Key3.Value : 0,
                        Key4 = item.Key4.HasValue ? item.Key4.Value : 0,
                        Key5 = item.Key5.HasValue ? item.Key5.Value : 0
                    };
                }
            }

            return fileProcess;
        }

        #endregion

        #region Template

        public static List<Template> CreateTemplates(BusinessCollection businessCollection)
        {
            List<Template> templates = new List<Template>();

            foreach (COMMEN.Template entity in businessCollection)
            {
                templates.Add(ModelAssembler.CreateTemplate(entity));
            }

            return templates;
        }

        public static Template CreateTemplate(COMMEN.Template entityFile)
        {
            return new Template()
            {
                Id = entityFile.Id,
                Description = entityFile.Description,
                PropertyName = entityFile.PropertyName,
            };
        }

        #endregion

    }
}
