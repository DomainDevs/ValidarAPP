// -----------------------------------------------------------------------
// <copyright file="ModelServiceAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.PrintingParamServices.EEProvider.Assemblers
{
    using System.Collections.Generic;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Enums;    
    using Sistran.Core.Application.ModelServices.Models.Printing;
    using Sistran.Core.Application.ModelServices.Models.Underwriting;
    using Sistran.Core.Application.PrintingParamServices.Models;

    /// <summary>
    /// Clase ensambladora para mapear modelos de negocio a modelos de Servicio.
    /// </summary>
    public class ModelServiceAssembler
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ModelServiceAssembler"/>.
        /// </summary>
        protected ModelServiceAssembler()
        {
        }

        /// <summary>
        /// Mapear modelos de negocio ParamCptAlliancePrintFormat a modelos de Servicio CptAlliancePrintFormatsServiceModel.
        /// </summary>
        /// <param name="paramCptAlliancePrintFormats">Modelos de negocio ParamCptAlliancePrintFormat.</param>
        /// <returns>Modelos de Servicio CptAlliancePrintFormatsServiceModel</returns>
        public static CptAlliancePrintFormatsServiceModel MappCptAlliancePrintFormats(List<ParamCptAlliancePrintFormat> paramCptAlliancePrintFormats)
        {
            CptAlliancePrintFormatsServiceModel cptAlliancePrintFormatsServiceModel = new CptAlliancePrintFormatsServiceModel();            
            List<CptAlliancePrintFormatServiceModel> listCptAlliancePrintFormatServiceModel = new List<CptAlliancePrintFormatServiceModel>();            
            foreach (ParamCptAlliancePrintFormat cptAlliancePrintFormatBusinessModel in paramCptAlliancePrintFormats)
            {
                EndorsementTypeServiceQueryModel endorsementTypeServiceQueryModel = new EndorsementTypeServiceQueryModel();
                PrefixServiceQueryModel prefixServiceQueryModel = new PrefixServiceQueryModel();

                CptAlliancePrintFormatServiceModel itemCptAlliancePrintFormatServiceModel = new CptAlliancePrintFormatServiceModel();
                ParametricServiceModel parametricServiceModel = new ParametricServiceModel();
                ErrorServiceModel errorServiceModel = new ErrorServiceModel();
                itemCptAlliancePrintFormatServiceModel.Id = cptAlliancePrintFormatBusinessModel.Id;

                prefixServiceQueryModel.PrefixCode = cptAlliancePrintFormatBusinessModel.PrefixCd;
                itemCptAlliancePrintFormatServiceModel.PrefixServiceQueryModel = prefixServiceQueryModel;

                endorsementTypeServiceQueryModel.Id = cptAlliancePrintFormatBusinessModel.EndoTypeCd;
                itemCptAlliancePrintFormatServiceModel.EndorsementTypeServiceQueryModel = endorsementTypeServiceQueryModel;

                itemCptAlliancePrintFormatServiceModel.Format = cptAlliancePrintFormatBusinessModel.Format;
                itemCptAlliancePrintFormatServiceModel.Enable = cptAlliancePrintFormatBusinessModel.Enable;

                errorServiceModel.ErrorTypeService = ErrorTypeService.Ok;
                errorServiceModel.ErrorDescription = new List<string>();
                parametricServiceModel.ErrorServiceModel = errorServiceModel;
                parametricServiceModel.StatusTypeService = StatusTypeService.Original;
                itemCptAlliancePrintFormatServiceModel.ParametricServiceModel = parametricServiceModel;                

                listCptAlliancePrintFormatServiceModel.Add(itemCptAlliancePrintFormatServiceModel);
            }

            cptAlliancePrintFormatsServiceModel.ErrorDescription = new List<string>();
            cptAlliancePrintFormatsServiceModel.ErrorTypeService = ErrorTypeService.Ok;

            cptAlliancePrintFormatsServiceModel.CptAlliancePrintFormatServiceModel = listCptAlliancePrintFormatServiceModel;            

            return cptAlliancePrintFormatsServiceModel;
        }

        /// <summary>
        /// Mapear modelos de negocio ParamEndoresementType a modelos de Servicio PrefixsServiceQueryModel.
        /// </summary>
        /// <param name="paramPrefixs">Modelos de negocio ParamPrefix.</param>
        /// <returns>Modelos de Servicio PrefixsServiceQueryModel</returns>
        public static EndorsementTypesServiceQueryModel MappPrefixs(List<ParamEndoresementType> paramEndoresementTypes)
        {
            EndorsementTypesServiceQueryModel endorsementTypesServiceQueryModel = new EndorsementTypesServiceQueryModel();
            List<EndorsementTypeServiceQueryModel> listEndorsementTypeServiceQueryModel = new List<EndorsementTypeServiceQueryModel>();
            foreach (ParamEndoresementType paramEndoresementTypeBusinessModel in paramEndoresementTypes)
            {
                EndorsementTypeServiceQueryModel itemEndorsementTypeServiceQueryModel = new EndorsementTypeServiceQueryModel();
                ParametricServiceModel parametricServiceModel = new ParametricServiceModel();

                itemEndorsementTypeServiceQueryModel.Id = paramEndoresementTypeBusinessModel.Id;
                itemEndorsementTypeServiceQueryModel.Description = paramEndoresementTypeBusinessModel.Description;

                listEndorsementTypeServiceQueryModel.Add(itemEndorsementTypeServiceQueryModel);
            }

            endorsementTypesServiceQueryModel.ErrorDescription = new List<string>();
            endorsementTypesServiceQueryModel.ErrorTypeService = ErrorTypeService.Ok;

            endorsementTypesServiceQueryModel.EndorsementTypeServiceQueryModel = listEndorsementTypeServiceQueryModel;

            return endorsementTypesServiceQueryModel;
        }

        /// <summary>
        /// Mapear modelos de negocio ParamPrefix a modelos de Servicio PrefixsServiceQueryModel.
        /// </summary>
        /// <param name="paramPrefixs">Modelos de negocio ParamPrefix.</param>
        /// <returns>Modelos de Servicio PrefixsServiceQueryModel</returns>
        public static PrefixsServiceQueryModel MappPrefixs(List<ParamPrefix> paramPrefixs)
        {
            PrefixsServiceQueryModel prefixsServiceQueryModel = new PrefixsServiceQueryModel();
            List<PrefixServiceQueryModel> listPrefixServiceQueryModel = new List<PrefixServiceQueryModel>();
            foreach (ParamPrefix paramPrefixBusinessModel in paramPrefixs)
            {

                PrefixServiceQueryModel itemPrefixsServiceQueryModel = new PrefixServiceQueryModel();
                ParametricServiceModel parametricServiceModel = new ParametricServiceModel();

                itemPrefixsServiceQueryModel.PrefixCode = paramPrefixBusinessModel.PrefixCode;
                itemPrefixsServiceQueryModel.PrefixDescription = paramPrefixBusinessModel.Description;

                listPrefixServiceQueryModel.Add(itemPrefixsServiceQueryModel);
            }

            prefixsServiceQueryModel.ErrorDescription = new List<string>();
            prefixsServiceQueryModel.ErrorTypeService = ErrorTypeService.Ok;

            prefixsServiceQueryModel.PrefixServiceQueryModel = listPrefixServiceQueryModel;

            return prefixsServiceQueryModel;
        }

        /// <summary>
        /// Método para mapear una lista de objetos de tipo AlliancePrintFormatViewModel a InsuredProfile.
        /// </summary>
        /// <param name="cptAlliancePrintFormatServiceModels">lista de objetos de tipo cptAlliancePrintFormatServiceModel.</param>
        /// <returns>lista de objetos de tipo InsuredProfile.</returns>
        public static List<CptAlliancePrintFormatServiceModel> MappAlliancePrintFormatsByStatusType(List<CptAlliancePrintFormatServiceModel> cptAlliancePrintFormatServiceModels, StatusTypeService statusTypeService)
        {
            List<CptAlliancePrintFormatServiceModel> listCptAlliancePrintFormatServiceModel = new List<CptAlliancePrintFormatServiceModel>();
            if (cptAlliancePrintFormatServiceModels != null)
            {                
                foreach (CptAlliancePrintFormatServiceModel itemModel in cptAlliancePrintFormatServiceModels)
                {
                    if (itemModel.ParametricServiceModel.StatusTypeService == statusTypeService)
                    {
                        listCptAlliancePrintFormatServiceModel.Add(itemModel);
                    }                    
                }

            }           

            return listCptAlliancePrintFormatServiceModel;
        }
    }
}
