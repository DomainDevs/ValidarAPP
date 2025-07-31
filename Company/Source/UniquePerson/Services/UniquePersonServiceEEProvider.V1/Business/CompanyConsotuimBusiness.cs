using AutoMapper;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.DAOs;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using models = Sistran.Core.Application.UniquePersonService.V1.Models;
using UPE = Sistran.Core.Application.UniquePersonV1.Entities;
namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    public class CompanyConsotuimBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyConsotuimBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        #region Consortim
        /// <summary>
        /// obtiene un agente
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public List<CompanyConsortium> GetCompanyConsurtiumCode(int id)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperConsortium();
                var result = coreProvider.GetConsortiumByInsurendId(id);
                List<CompanyConsortium> comanyConsortium = new List<CompanyConsortium>();
                if (result != null)
                {
                    foreach (var item in result)
                    {
                        var resultMapper = imapper.Map<models.Consortium, CompanyConsortium>(item);
                        comanyConsortium.Add(resultMapper);
                    }
                }                

                return comanyConsortium;

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Crea el agente por IndividualId.
        /// </summary>
        /// <param name="companyConsortuim">Modelo Agente</param>
        /// <returns></returns>
        public CompanyConsortium CreateCompanyConsortium(CompanyConsortium companyConsortuim)
        {
            try
            {
                var mapper = ModelAssembler.CreateMapperConsortium();
                var consortiumCore = mapper.Map<CompanyConsortium, models.Consortium>(companyConsortuim);
                var result = coreProvider.CreateConsortium(consortiumCore);
                return mapper.Map<models.Consortium, CompanyConsortium>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Actauliza el Agente por InvidualId
        /// </summary>
        /// <param name="companyAgent"></param>
        /// <returns></returns>
        public CompanyConsortium UpdateCompanyConsortium(CompanyConsortium companyConsortuim)
        {
            try
            {
                var map = ModelAssembler.CreateMapperConsortium();
                var consortiumCore = map.Map<CompanyConsortium, models.Consortium>(companyConsortuim);
                var result = coreProvider.UpdateConsortium(consortiumCore);
                return map.Map<models.Consortium, CompanyConsortium>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
       
        /// <summary>
        /// Delete el ramo para la persona.
        /// </summary>
        /// <param name="companyPrefix">model de ramos </param>
        /// <param name="IndividualId">codigo de la persona</param>
        /// <returns></returns>
        public bool DeleteCompanyConsortuim(CompanyConsortium companyConsortuim)
        {
            try
            {
                var imap = ModelAssembler.CreateMapperConsortium();
                var consortiumCore = imap.Map<CompanyConsortium, models.Consortium>(companyConsortuim);
                var result = coreProvider.DeleteConsortium(consortiumCore);
                return true;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public List<Models.CompanyConsortium> GetCoConsortiumsByInsuredCode(int insuredCode)
        {
            Entities.views.CoConsorcioViewV1 view = new Entities.views.CoConsorcioViewV1();
            ViewBuilder builder = new ViewBuilder("CoConsorcioViewV1");
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UPE.Insured.Properties.InsuredCode, typeof(Insured).Name);
            filter.Equal();
            filter.Constant(insuredCode);
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            List<Models.CompanyConsortium> consortiums = ModelAssembler.CreateCoConsortiums(view);
            return consortiums;
        }

        #endregion agent



    }
}
