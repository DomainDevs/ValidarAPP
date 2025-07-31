using Sistran.Core.Application.UniquePersonListRiskApplicationServices.DTO;
using Sistran.Core.Application.UniquePersonListRiskApplicationServices.Enum;
using Sistran.Core.Application.UniquePersonListRiskBusinessService.Model;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonListRiskApplicationServicesProvider.Assembler
{
    public static class ModelAssembler
    {

        public static List<ListRiskMatchDTO> CreateListRiskMatchDTOList(List<RiskListMatch> companyListRiskModel)
        {
            List<ListRiskMatchDTO> listRiskMatchDTOs = new List<ListRiskMatchDTO>();
            companyListRiskModel.ForEach(x => listRiskMatchDTOs.Add(CreateListRiskMatchDTO(x)));
            return listRiskMatchDTOs;
        }

        public static ListRiskMatchDTO CreateListRiskMatchDTO(RiskListMatch riskListMatch)
        {

            return new ListRiskMatchDTO
            {
                jModel = riskListMatch.jModel,
                listVersion = riskListMatch.listVersion,
                listType = riskListMatch.listType,
                registrationDate = riskListMatch.registrationDate
            };
        }



    }


}
