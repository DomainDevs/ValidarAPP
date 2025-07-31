using AutoMapper;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System.Collections.Generic;
using COMMEN = Sistran.Core.Application.Common.Entities;
using MODEL = Sistran.Core.Services.UtilitiesServices.Models;

namespace Sistran.Core.UtilitiesServicesEEProvider.Assemblers

{
    public class ModelAssembler
    {
        #region AsynchronousProcess

        public static AsynchronousProcess CreateAsynchronousProcess(COMMEN.AsynchronousProcess entityAsynchronousProcess)
        {
            return new AsynchronousProcess()
            {
                Id = entityAsynchronousProcess.ProcessId,
                Description = entityAsynchronousProcess.Description,
                BeginDate = entityAsynchronousProcess.BeginDate.Value,
                EndDate = entityAsynchronousProcess.EndDate,
                UserId = entityAsynchronousProcess.UserId.Value,
                HasError = entityAsynchronousProcess.HasError.GetValueOrDefault(),
                ErrorDescription = entityAsynchronousProcess.ErrorDescription,
                //Active = entityAsynchronousProcess.Active.GetValueOrDefault(),
                //StatusId = entityAsynchronousProcess.StatusId.GetValueOrDefault()
            };
        }

        public static List<AsynchronousProcess> CreateAsynchronousProcesses(BusinessCollection businessCollection)
        {
            List<AsynchronousProcess> asynchronousProcesses = new List<AsynchronousProcess>();

            foreach (COMMEN.AsynchronousProcess entity in businessCollection)
            {
                asynchronousProcesses.Add(ModelAssembler.CreateAsynchronousProcess(entity));
            }

            return asynchronousProcesses;
        }

        #endregion


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
        public static COMMEN.Template CreateTemplate(Template template)
        {
            return new COMMEN.Template(template.Id)
            {
                Description = template.Description,
                PropertyName = template.PropertyName
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

        #region PendingOperation

        public static PendingOperation CreatePendingOperation(COMMEN.PendingOperations entityPendingOperation)
        {
            return new PendingOperation
            {
                Id = entityPendingOperation.Id,
                ParentId = entityPendingOperation.ParentId.GetValueOrDefault(),
                UserId = entityPendingOperation.User.GetValueOrDefault(),
                UserName = entityPendingOperation.UserName,
                CreationDate = entityPendingOperation.CreationDate,
                Operation = entityPendingOperation.Operation,
                OperationName = entityPendingOperation.OperationName,
                AdditionalInformation = entityPendingOperation.AdditionalInformation,
                //IsMassive = entityPendingOperation.IsMassive
            };
        }

        public static List<PendingOperation> CreatePendingOperations(BusinessCollection businessCollection)
        {
            List<PendingOperation> pendingOperations = new List<PendingOperation>();

            foreach (COMMEN.PendingOperations entity in businessCollection)
            {
                pendingOperations.Add(ModelAssembler.CreatePendingOperation(entity));
            }

            return pendingOperations;
        }

        #endregion

        public static ValidationRegularExpression CreateValidationRegularExpression(COMMEN.ValidationRegularExpression validationRegularExpression)
        {
            return new ValidationRegularExpression
            {
                Id = validationRegularExpression.Id,
                FieldDescription = validationRegularExpression.Description,
                ParameterValue = validationRegularExpression.RegularExpression,
                ErrorMessage = validationRegularExpression.ErrorMessage
            };
        }

        public static List<ValidationRegularExpression> CreateValidationRegularExpressions(BusinessCollection businessCollection)
        {
            List<ValidationRegularExpression> validationRegularExpressions = new List<ValidationRegularExpression>();

            foreach (COMMEN.ValidationRegularExpression entity in businessCollection)
            {
                validationRegularExpressions.Add(ModelAssembler.CreateValidationRegularExpression(entity));
            }
            return validationRegularExpressions;
        }
        #region Mapper
        public IMapper CreateMapField()
        {
            var config = MapperCache.GetMapper<COMMEN.Field, Field>(cfg =>
            {
                cfg.CreateMap<COMMEN.Field, Field>()
                .ForMember(dest => dest.FieldType, opt => opt.MapFrom(src => (FieldType)src.FieldTypeId)
                );
            });
            return config;
       
        }
        #endregion


        #region EndorsementControl
        public static EndorsementControl CreateEndorsementControl(EndorsementControl endorsementControlEntity)
        {
            EndorsementControl endorsementControl = new EndorsementControl
            {
                Id = endorsementControlEntity.Id,
                UserId = endorsementControlEntity.UserId,
            };
            return endorsementControl;
        }
      
        #endregion
    }
}
