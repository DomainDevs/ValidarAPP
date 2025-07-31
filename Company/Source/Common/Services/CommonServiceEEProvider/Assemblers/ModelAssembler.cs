
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System.Collections.Generic;
using COMCOMP = Sistran.Company.Application.CommonServices.Models;
using COMENT = Sistran.Core.Application.Common.Entities;
using COMENTCOM = Sistran.Company.Application.Common.Entities;
using LineBusiness = Sistran.Core.Application.CommonService.Models.LineBusiness;
using ParametersEntities = Sistran.Core.Application.Parameters.Entities;
using QUOENT = Sistran.Core.Application.Quotation.Entities;

namespace Sistran.Company.Application.CommonService.Assemblers
{
    public static class ModelAssembler  
    {
        public static SubLineBusiness CreateSubLineBusiness(COMENT.SubLineBusiness subLineBusinessEntity)
        {
            return new SubLineBusiness
            {
                Id = subLineBusinessEntity.SubLineBusinessCode,
                Description = subLineBusinessEntity.Description,
                // Se agrega el ramo técnico para poder hacer filtros en memoria en el cliente sin volver a consultar el servidor.
                LineBusiness = new LineBusiness { Id = subLineBusinessEntity.LineBusinessCode }
            };
        }
        
        #region  DetailTypes
        public static List<COMCOMP.DetailType> CreateDetailTypes(BusinessCollection businessCollection)
        {
            List<COMCOMP.DetailType> DetailTypes = new List<COMCOMP.DetailType>();

            foreach (QUOENT.DetailType field in businessCollection)
            {
                DetailTypes.Add(CreateDetailType(field));
            }

            return DetailTypes;
        }
        public static COMCOMP.DetailType CreateDetailType(QUOENT.DetailType detailType)
        {
            return new COMCOMP.DetailType
            {
                DetailTypeCode = detailType.DetailTypeCode,
                Description = detailType.Description,
                SmallDescription = detailType.SmallDescription,
                DetailClassCode = detailType.DetailClassCode
            };
        }
        #endregion

        public static CompositionType CreateCompositionType(ParametersEntities.CompositionType compositionTypeEntity)
        {
            return new CompositionType
            {
                Id = compositionTypeEntity.CompositionTypeCode,
                Description = compositionTypeEntity.Description,
                SmallDescription = compositionTypeEntity.SmallDescription
            };
        }

        #region Nomenclature 

        public static List<Nomenclature> CreateNomenclatures(BusinessCollection businessCollection)
        {
            List<Nomenclature> nomenlcatures = new List<Nomenclature>();
            foreach (COMENTCOM.CoNomenclatures item in businessCollection)
            {
                nomenlcatures.Add(ModelAssembler.CreateNomenclature(item));
            }
            return nomenlcatures;
        }

        public static Nomenclature CreateNomenclature(COMENTCOM.CoNomenclatures coNomenclatures)
        {
            return new Nomenclature()
            {
                Nomenclatura = coNomenclatures.Nomenclatura,
                Abreviatura = coNomenclatures.Abreviatura
            };
        }

        #endregion
               
                
        public static File CreateFile(COMENT.File entityFile)
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
        #region Parameter
        public static COMCOMP.CompanyParameter CreateParameter(COMENT.CoParameter coParameter)
        {
            if (coParameter != null)
            {
                return new COMCOMP.CompanyParameter
                {
                    Id = coParameter.ParameterId,
                    Description = coParameter.Description,
                    BoolParameter = coParameter.BoolParameter,
                    NumberParameter = coParameter.NumberParameter,
                    DateParameter = coParameter.DateParameter,
                    TextParameter = coParameter.TextParameter,
                    PercentageParameter = coParameter.PercentageParameter,
                    AmountParameter = coParameter.AmountParameter
                };
            }

            return null;
        }
        #endregion Parameter

    }
}
