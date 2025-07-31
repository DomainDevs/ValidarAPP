using ENT = Sistran.Core.Application.Temporary.Entities;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using COMMEN = Sistran.Core.Application.Common.Entities;
using MODEL = Sistran.Core.Services.UtilitiesServices.Models;

namespace Sistran.Core.UtilitiesServicesEEProvider.Assemblers
{ 
    public class EntityAssembler
    {
        #region AsynchronousProcess

        /// <summary>
        /// Convierte un objeto de tipo Models.AsynchronousProcess a Entidad.AsynchronousProcess
        /// </summary>
        /// <param name="asynchronousProcess">Objeto del tipo Models.AsynchronousProcess</param>
        /// <returns>Retorna un objeto del tipo Entidad.AsynchronousProcess</returns>
        public static COMMEN.AsynchronousProcess AsynchronousProcess(AsynchronousProcess asynchronousProcess)
        {
            return new COMMEN.AsynchronousProcess()
            {
                Description = asynchronousProcess.Description,
                BeginDate = asynchronousProcess.BeginDate,
                EndDate = asynchronousProcess.EndDate,
                UserId = asynchronousProcess.UserId,
                HasError = asynchronousProcess.HasError,
                ErrorDescription = asynchronousProcess.ErrorDescription,
                //Active = asynchronousProcess.Active,
                //StatusId = asynchronousProcess.StatusId
            };
        }

        #endregion
        #region PendingOperation

        public static COMMEN.PendingOperations CreatePendingOperation(PendingOperation pendingOperation)
        {
            COMMEN.PendingOperations pendingOperations = new COMMEN.PendingOperations
            {
                User = pendingOperation.UserId,
                UserName = pendingOperation.UserName,
                Operation = pendingOperation.Operation,
                OperationName = pendingOperation.OperationName,
                CreationDate = DateTime.Now,
                IsMassive = pendingOperation.IsMassive,
            
            };

            if (pendingOperation.ParentId > 0)
            {
                pendingOperations.ParentId = pendingOperation.ParentId;
            }
            if (pendingOperation.AdditionalInformation != null)
            {
                pendingOperations.AdditionalInformation = pendingOperation.AdditionalInformation;
            }

            return pendingOperations;
        }

        #endregion
        public static COMMEN.File CreateFile(File file)
        {
            return new COMMEN.File(file.Id)
            {
                Description = file.Description,
                SmallDescription = file.SmallDescription,
                Observations = file.Observations,
                FileTypeId = (int)file.FileType,
                IsEnabled = file.IsEnabled,
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

        #region EndorsementControl        

        public static ENT.EndorsementControl CreateEndorsementControlEntity(ENT.EndorsementControl endorsementControlEntity)
        {
            ENT.EndorsementControl endorsementControl = new ENT.EndorsementControl
            {
                Id = endorsementControlEntity.Id,
                UserId = endorsementControlEntity.UserId
            };
            return endorsementControl;
        }
        #endregion
    }
}
