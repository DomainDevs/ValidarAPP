using Sistran.Core.Application.CommonService.Models;
using COMMEN = Sistran.Core.Application.Common.Entities;
using COMMML = Sistran.Core.Application.CommonService.Models;
using EtLineBusiness = Sistran.Core.Application.Common.Entities;
using Model = Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Services.UtilitiesServices.Models;
namespace Sistran.Core.Application.CommonServices.EEProvider.Assemblers
{
    public static class EntityAssembler
    {

        #region Parameter

        public static COMMEN.Parameter CreateParameter(COMMML.Parameter parameter)
        {
            COMMEN.Parameter parameters = new COMMEN.Parameter(0)
            {
                AmountParameter = parameter.AmountParameter,
                NumberParameter = parameter.NumberParameter,
                PercentageParameter = parameter.PercentageParameter
            };
            return parameters;
        }

        #endregion

        public static COMMEN.SubLineBusiness CreateSubLineBusiness(SubLineBusiness subLineBusiness)
        {
            return new COMMEN.SubLineBusiness(subLineBusiness.Id, subLineBusiness.LineBusinessId)
            {
                SubLineBusinessCode = subLineBusiness.Id,
                Description = subLineBusiness.Description,
                SmallDescription = subLineBusiness.SmallDescription,
                LineBusinessCode = subLineBusiness.LineBusinessId
            };
        }
        #region LineBussinessOperation

        /// <summary>
        /// Asembler para creacion de Line Business
        /// </summary>
        /// <param name="linebusiness"></param>
        /// <returns></returns>
        public static EtLineBusiness.LineBusiness CreateLineBusiness(Model.LineBusiness linebusiness)
        {
            EtLineBusiness.LineBusiness lineBusinessEntities = new EtLineBusiness.LineBusiness(linebusiness.Id)
            {
                Description = linebusiness.Description,
                LineBusinessCode = linebusiness.Id,
                ReportLineBusinessCode = linebusiness.ReportLineBusiness.ToString(),
                TinyDescription = linebusiness.TyniDescription,
                SmallDescription = linebusiness.ShortDescription
            };
            return lineBusinessEntities;
        }

        /// <summary>
        /// Asembler para tipos de riesgo por lineBusiness
        /// </summary>
        /// <param name="linebusinessCoveredRiskType"></param>
        /// <returns></returns>
        //public static EtLineBusiness.LineBusinessCoveredRiskType CreateLineBusinessCoveredRiskType(ModelUnderwriting.LineBusinessCoveredRiskType linebusinessCoveredRiskType)
        //{
        //    EtLineBusiness.LineBusinessCoveredRiskType lineBusinessCoveredRiskTypeEntities = new EtLineBusiness.LineBusinessCoveredRiskType(linebusinessCoveredRiskType.IdLineBusiness, linebusinessCoveredRiskType.IdRiskType)
        //    {
        //        CoveredRiskTypeCode = linebusinessCoveredRiskType.IdRiskType,
        //        LineBusinessCode = linebusinessCoveredRiskType.IdLineBusiness
        //    };
        //    return lineBusinessCoveredRiskTypeEntities;
        //}


        /// <summary>
        /// Asembler para AMparos del Line Business
        /// </summary>
        /// <param name="linebusinessPeril"></param>
        /// <returns></returns>
        //public static QUOEN.PerilLineBusiness CreateLineBusinessProtection(ModelUnderwriting.PerilLineBusiness linebusinessPeril)
        //{
        //    QUOEN.PerilLineBusiness lineBusinessPerilEntities = new QUOEN.PerilLineBusiness(linebusinessPeril.IdPeril, linebusinessPeril.IdLineBusiness)
        //    {
        //        PerilCode= linebusinessPeril.IdPeril,
        //        LineBusinessCode= linebusinessPeril.IdLineBusiness
        //    };
        //    return lineBusinessPerilEntities;
        //}

        #endregion

        public static COMMEN.Prefix CreatePrefix(COMMML.Prefix Prefix)
        {
            return new COMMEN.Prefix(0)
            {
                PrefixCode = Prefix.Id,
                Description = Prefix.Description,
                SmallDescription = Prefix.SmallDescription,
                TinyDescription = Prefix.TinyDescription,
                PrefixTypeCode = Prefix.PrefixTypeCode
            };
        }

        public static COMMEN.Branch CreateBranch(COMMML.Branch branch)
        {
            return new COMMEN.Branch(branch.Id)
            {
                Description = branch.Description,
                SmallDescription = branch.SmallDescription
            };
        }


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
        
    }
}
