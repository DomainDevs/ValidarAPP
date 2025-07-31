using AutoMapper;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Integration.UniqueUserServices.DTOs;
using System.Collections.Generic;
using System.Linq;
using COMMONMDL = Sistran.Core.Application.CommonService.Models;


namespace IntegrationServiceEEProvider.Assemblers
{
    internal static class DTOAssembler
    {
        internal static UserDTO CreateUserByLogin(User user)
        {
            return new UserDTO() 
            {
                AccountName = user.AccountName,
                AuthenticationTypeCode = user.AuthenticationTypeCode,
                CreatedUserId = user.CreatedUserId,
                CreationDate = user.CreationDate,
                DisableDate  = user.DisableDate,
                ExpirationDate = user.ExpirationDate,                
                IsDisabledDateNull = user.IsDisabledDateNull,
                IsExpirationDateNull = user.IsExpirationDateNull,
                IsLockDateNull = user.IsLockDateNull,
                LastModificationDate = user.LastModificationDate,
                LockDate = user.LockDate,
                LockPassword = user.LockPassword,
                ModifiedUserId = user.ModifiedUserId,
                Name = user.Name,
                PersonId = user.PersonId,
                StatusDescription = user.StatusDescription,                
                UserDomain = user.UserDomain,
                UserId = user.UserId
            };
        }

        public static IMapper CreateMapBranchIntegration()
        {
            var config = MapperCache.GetMapper<COMMONMDL.Branch, BranchDTO>(cfg =>
            {
                cfg.CreateMap<COMMONMDL.Branch, BranchDTO>();
                cfg.CreateMap<COMMONMDL.SalePoint, SalePointDTO>();
            });
            return config;
        }

        public static BranchDTO ToDTO(this COMMONMDL.Branch individualDTO)
        {
            var config = CreateMapBranchIntegration();
            return config.Map<COMMONMDL.Branch, BranchDTO>(individualDTO);
        }

        public static IEnumerable<BranchDTO> ToDTOs(this IEnumerable<COMMONMDL.Branch> branches)
        {
            return branches.Select(ToDTO);
        }
        
    }
}
