using Sistran.Company.Application.UniquePersonAplicationServices.DTOs;
using MOCOUP = Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Error;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.UniquePersonAplicationServices.EEProvider.Assemblers;

namespace Sistran.Company.Application.UniquePersonServices.EEProvider.Business
{
    public class BusinessPerson
    {
        #region CompanyCoInsured

        public CompanyCoInsuredDTO CreateCompanyCoInsured(CompanyCoInsuredDTO companyCoInsuredDTO)
        {
            MOCOUP.CompanyCoInsured companyCoInsured = ModelAssembler.CreateCompanyCoInsured(companyCoInsuredDTO);
            var CoinSured = DelegateService.uniquePersonService.CreateCompanyCoInsured(companyCoInsured);
            var result = AplicationAssembler.CreateCompanyCoInsured(companyCoInsured);
            return result;
        }
        public CompanyCoInsuredDTO UpdateCompanyCoInsured(CompanyCoInsuredDTO companyCoInsuredDTO)
        {
            MOCOUP.CompanyCoInsured companyCoInsured = ModelAssembler.CreateCompanyCoInsured(companyCoInsuredDTO);
            var CoinSured = DelegateService.uniquePersonService.UpdateCompanyCoInsured(companyCoInsured);
            var result = AplicationAssembler.CreateCompanyCoInsured(companyCoInsured);
            return result;
        }

        public CompanyCoInsuredDTO GetCompanyCoInsuredIndivualID(int IndividualId)
        {

            var CoinSured = DelegateService.uniquePersonService.GetCompanyCoInsuredIndividualId(IndividualId);
            var result = AplicationAssembler.CreateCompanyCoInsured(CoinSured);
            return result;
        }
        public CompanyCoInsuredDTO GetCompanyCoInsured(string IdTributary)
        {

            var CoinSured = DelegateService.uniquePersonService.GetCompanyCoInsuredTributary(IdTributary);
            var result = AplicationAssembler.CreateCompanyCoInsured(CoinSured);
            return result;
        }

        #endregion CompanyCoInsured

        //#region Consortiums

        ///// <summary>
        ///// Obtiene Consorciado por Asegurado y por Individuo
        ///// </summary>
        ///// <param name="InsureCode">Id asegurado </param>
        ///// <param name="IndividualId">Id Individuo</param>
        ///// <returns>Se Retorna Consorciados</returns>
        //public ConsorciatedDTO GetConsortiumInsuredCodeAndIndividualID(int InsureCode, int IndividualId)
        //{
        //    MOCOUP.Consortium model = DelegateService.uniquePersonService.GetConsortiumByInsurendIdOnInvidualId(InsureCode, IndividualId);
        //    var result = AplicationAssembler.CreateConsortium(model);
        //    return result;
        //}

        ///// <summary>
        ///// Obtiene los Consorciados por Id asegurado
        ///// </summary>
        ///// <param name="InsureCode">Id Asegurado</param>
        ///// <returns>Retorna Consorciados Por Asegurado.</returns>
        //public List<ConsorciatedDTO> GetConsortiumInsuredCode(int InsureCode)
        //{
        //    List<MOCOUP.Consortium> model = DelegateService.uniquePersonService.GetConsortiumByInsurendId(InsureCode);
        //    var result = AplicationAssembler.CreateConsortiums(model);
        //    return result;
        //}

        ///// <summary>
        ///// Crea Consorciados para Un Individuo
        ///// </summary>
        ///// <param name="model">Informacion Consorciado</param>
        ///// <returns>Retorna Informacion Consorciado Creado</returns>
        ////public List<ConsorciatedDTO> CreateConsortium(List<ConsorciatedDTO> model)
        ////{
        ////    List<MOCOUP.Consortium> ConsortiumToModel = ModelAssembler.CreateConsortiums(model);
        ////    var ConsortuimToSave = DelegateService.uniquePersonService.CreateConsortium(ConsortiumToModel);
        ////    var result = AplicationAssembler.CreateConsortiums(ConsortiumToModel);
        ////    return result;
        ////}

        ///// <summary>
        ///// Actualiza Informacion Del Consorciado.
        ///// </summary>
        ///// <param name="model">Model Para Actualizar consorciados</param>
        ///// <returns>Retorna la actualizacion DE Consorciados.</returns>
        //public ConsorciatedDTO UpdateConsortium(ConsorciatedDTO model)
        //{
        //    MOCOUP.Consortium ConsortiumToModel = ModelAssembler.CreateConsortium(model);
        //    var ConsortuimToSave = DelegateService.uniquePersonService.UpdateConsortium(ConsortiumToModel);
        //    var result = AplicationAssembler.CreateConsortium(ConsortiumToModel);
        //    return result;
        //}

        ///// <summary>
        ///// Elimina Consorciados Por Asegurado 
        ///// </summary>
        ///// <param name="InsuredID">Id asegurado</param>
        ///// <returns>Retorna la Eliminación del Consorciado.</returns>
        //public bool DeleteConsortium(ConsorciatedDTO model)
        //{
        //    MOCOUP.Consortium ConsortiumToModel = ModelAssembler.CreateConsortium(model);
        //    var ConsortuimToSave = DelegateService.uniquePersonService.DeleteConsortium(ConsortiumToModel);
        //    return ConsortuimToSave;
        //}

        //#endregion Consortiums
    }
}
